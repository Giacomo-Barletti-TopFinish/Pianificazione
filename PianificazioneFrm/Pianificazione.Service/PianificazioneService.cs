using Pianificazione.Data;
using Pianificazione.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pianificazione.Service
{
    public class PianificazioneService
    {
        private PianificazioneDS _ds;
        private void CaricaListaLanciAperti()
        {
            using (PianificazioneBusiness bPianificazione = new PianificazioneBusiness())
            {

                bPianificazione.FillUSR_PRD_LANCIODAperti(_ds);
            }
        }
        public void CreaPianificazione()
        {
            _ds = new PianificazioneDS();
            try
            {
                CaricaListaLanciAperti();
            }
            catch (Exception ex)
            {
                SciviLog(ex);
                return;
            }
            SciviLog("INFO", string.Format("Trovati {0} lanci da verificare", _ds.USR_PRD_LANCIOD.Count));

            int numeroElementiLancio = _ds.USR_PRD_LANCIOD.Count();
            int contatoreLancio = 1;
            foreach (PianificazioneDS.USR_PRD_LANCIODRow lancio in _ds.USR_PRD_LANCIOD.OrderBy(x => x.IDLANCIOD))
            {
                using (PianificazioneBusiness bPianificazione = new PianificazioneBusiness())
                {
                    try
                    {
                        SciviLog("INFO", string.Format("Elaborazione {0} di {1} IDLANCIOD: {2}", contatoreLancio, numeroElementiLancio, lancio.IDLANCIOD));
                        contatoreLancio++;

                        bPianificazione.FillUSR_PRD_FASIByIDLANCIOD(_ds, lancio.IDLANCIOD);
                        bPianificazione.FillUSR_PRD_MOVFASIByIDLANCIOD(_ds, lancio.IDLANCIOD);

                        bPianificazione.FillPIANIFICAZIONE_LANCIO(_ds, lancio.IDLANCIOD);
                        bPianificazione.FillPIANIFICAZIONE_FASE(_ds, lancio.IDLANCIOD);


                        PianificazioneDS.PIANIFICAZIONE_LANCIORow pLancio = _ds.PIANIFICAZIONE_LANCIO.Where(x => x.IDLANCIOD == lancio.IDLANCIOD).FirstOrDefault();
                        if (pLancio == null)
                        {
                            long idlancio = bPianificazione.GetID();
                            SciviLog("INFO", string.Format("IDLANCIOD: {0} non trovato in PIANIFICAZIONE_LANCIO CREATO NUOVO CON ID: {1}", lancio.IDLANCIOD, idlancio));
                            pLancio = CreaPianificazione_Lancio(idlancio, lancio);
                            _ds.PIANIFICAZIONE_LANCIO.AddPIANIFICAZIONE_LANCIORow(pLancio);
                        }
                        else
                        {
                            SciviLog("INFO", string.Format("IDLANCIOD: {0} trovato in PIANIFICAZIONE_LANCIO CON ID: {1}", lancio.IDLANCIOD, pLancio.IDLANCIO));
                        }

                        PianificazioneDS.USR_PRD_FASIRow faseRoot = _ds.USR_PRD_FASI.Where(x => x.ROOTSN == 1 && x.IDLANCIOD == lancio.IDLANCIOD).FirstOrDefault();
                        if (faseRoot == null)
                        {
                            SciviLog("WARNING", string.Format("IDLANCIOD: {0} IMPOSSIBILE TROVARE FASE ROOT", lancio.IDLANCIOD));
                            continue;
                        }
                        else
                        {
                            CreaPianificazione_Fase(faseRoot.IDPRDFASE, faseRoot, pLancio.IDLANCIO);

                            List<PianificazioneDS.USR_PRD_FASIRow> fasiFiglie = _ds.USR_PRD_FASI.Where(x => !x.IsIDPRDFASEPADRENull() && x.IDPRDFASEPADRE == faseRoot.IDPRDFASE).ToList();
                            CreaAlbertoPianificazione(fasiFiglie, pLancio.IDLANCIO);
                        }
                        CorreggiDate(pLancio);

                        bPianificazione.SalvaPianificazione(_ds);
                        _ds.AcceptChanges();

                        _ds.USR_PRD_FASI.Clear();
                        _ds.USR_PRD_MOVFASI.Clear();
                        _ds.PIANIFICAZIONE_LANCIO.Clear();
                        _ds.PIANIFICAZIONE_FASE.Clear();
                    }
                    catch (Exception ex)
                    {
                        SciviLog("ERRORE", string.Format("IDLANCIOD: {0} ECCEZIONE AL PASSO {1} DI {2}", lancio.IDLANCIOD, contatoreLancio - 1, numeroElementiLancio));
                        SciviLog(ex);
                    }

                }
            }
        }

        private void CorreggiDate(PianificazioneDS.PIANIFICAZIONE_LANCIORow pLancio)
        {
            PianificazioneDS.PIANIFICAZIONE_FASERow faseRoot = _ds.PIANIFICAZIONE_FASE.Where(x => x.IDLANCIO == pLancio.IDLANCIO).FirstOrDefault();
            if (faseRoot == null)
            {
                SciviLog("WARNING", string.Format("IDLANCIO: {0} non ha fase di root", pLancio.IDLANCIO));
                return;
            }

            if (faseRoot.STATO != StatoFasePianificazione.PIANIFICATO)
                return;
            CorreggiDateFasePianificata(faseRoot);
        }

        private void CorreggiDateFasePianificata(PianificazioneDS.PIANIFICAZIONE_FASERow fasePianificata)
        {

            List<PianificazioneDS.PIANIFICAZIONE_FASERow> fasiFiglie = _ds.PIANIFICAZIONE_FASE.Where(x => !x.IsIDFASEPADRENull() && x.IDFASEPADRE == fasePianificata.IDFASE).ToList();
            if (fasiFiglie.Count == 0) return;

            DateTime finePrecedente = fasePianificata.DATAINIZIO;
            if (fasiFiglie.Any(x => x.STATO == StatoFasePianificazione.PIANIFICATO))
            {
                List<PianificazioneDS.PIANIFICAZIONE_FASERow> fasiFigliePianificate = fasiFiglie.Where(x => x.STATO == StatoFasePianificazione.PIANIFICATO).ToList();
                foreach (PianificazioneDS.PIANIFICAZIONE_FASERow faseFigliaPianificata in fasiFigliePianificate)
                {
                    CorreggiDateFasePianificata(faseFigliaPianificata);
                }
            }
            fasiFiglie = _ds.PIANIFICAZIONE_FASE.Where(x => !x.IsIDFASEPADRENull() && x.IDFASEPADRE == fasePianificata.IDFASE).ToList();

            finePrecedente = fasiFiglie.Where(x => !x.IsDATAFINENull()).Max(x => x.DATAFINE);
            fasePianificata.DATAINIZIO = finePrecedente;
            fasePianificata.DATAFINE = finePrecedente.AddDays((double)fasePianificata.OFFSETTIME);
            if (fasePianificata.OFFSETTIME == 0) fasePianificata.DATAINIZIO = finePrecedente.AddHours(1);
        }

        private void CreaAlbertoPianificazione(List<PianificazioneDS.USR_PRD_FASIRow> fasi, decimal IDLANCIO)
        {
            foreach (PianificazioneDS.USR_PRD_FASIRow figlia in fasi)
            {
                CreaPianificazione_Fase(figlia.IDPRDFASE, figlia, IDLANCIO);
                List<PianificazioneDS.USR_PRD_FASIRow> fasiFiglie = _ds.USR_PRD_FASI.Where(x => !x.IsIDPRDFASEPADRENull() && x.IDPRDFASEPADRE == figlia.IDPRDFASE).ToList();
                CreaAlbertoPianificazione(fasiFiglie, IDLANCIO);
            }
        }

        private void CreaPianificazione_Fase(string IDPRDFASE, PianificazioneDS.USR_PRD_FASIRow faseRow, decimal idLancio)
        {
            List<PianificazioneDS.USR_PRD_MOVFASIRow> movFasi = _ds.USR_PRD_MOVFASI.Where(x => x.IDPRDFASE == faseRow.IDPRDFASE).ToList();
            PianificazioneDS.PIANIFICAZIONE_FASERow fase = _ds.PIANIFICAZIONE_FASE.Where(x => x.IDPRDFASE == IDPRDFASE).FirstOrDefault();

            if (fase == null)
            {
                fase = _ds.PIANIFICAZIONE_FASE.NewPIANIFICAZIONE_FASERow();
                using (PianificazioneBusiness bPianificazione = new PianificazioneBusiness())
                {
                    long IDFASE = bPianificazione.GetID();
                    fase.IDFASE = IDFASE;
                }

                fase.IDLANCIO = idLancio;
                fase.AZIENDA = faseRow.AZIENDA;
                fase.IDPRDFASE = faseRow.IDPRDFASE;
                fase.STATO = StatoFasePianificazione.PIANIFICATO;
                AssegnaPadre(faseRow, fase);

                fase.CODICECLIFO = faseRow.IsCODICECLIFONull() ? string.Empty : faseRow.CODICECLIFO;
                if (!faseRow.IsDATAINIZIONull())
                    fase.DATAINIZIO = faseRow.DATAINIZIO;
                if (!faseRow.IsDATAFINENull())
                    fase.DATAFINE = faseRow.DATAFINE;
                if (!faseRow.IsIDMAGAZZNull())
                    fase.IDMAGAZZ = faseRow.IDMAGAZZ;

                if (!faseRow.IsOFFSETTIMENull())
                    fase.OFFSETTIME = faseRow.OFFSETTIME;
                if (!faseRow.IsLEADTIMENull())
                    fase.LEADTIME = faseRow.LEADTIME;
                if (!faseRow.IsIDDIBAMETHIDNull())
                    fase.IDDIBAMETHOD = faseRow.IDDIBAMETHID;
                if (!faseRow.IsVERSIONNull())
                    fase.VERSION = faseRow.VERSION;
                if (!faseRow.IsQTANull())
                    fase.QTA = faseRow.QTA;
                if (!faseRow.IsQTAANNNull())
                    fase.QTAANN = faseRow.QTAANN;
                if (!faseRow.IsQTANETNull())
                    fase.QTANET = faseRow.QTANET;
                if (!faseRow.IsQTAASSNull())
                    fase.QTAASS = faseRow.QTAASS;
                if (!faseRow.IsQTACONNull())
                    fase.QTACON = faseRow.QTACON;
                if (!faseRow.IsQTATERNull())
                    fase.QTATER = faseRow.QTATER;
                if (!faseRow.IsQTADATERNull())
                    fase.QTADATER = faseRow.QTADATER;
                if (!faseRow.IsQTAACCNull())
                    fase.QTAACC = faseRow.QTAACC;
                if (!faseRow.IsQTADACNull())
                    fase.QTADAC = faseRow.QTADAC;
                if (!faseRow.IsQTALAVNull())
                    fase.QTALAV = faseRow.QTALAV;
                if (!faseRow.IsQTADAPIANull())
                    fase.QTADAPIA = faseRow.QTADAPIA;
                if (!faseRow.IsQTAPIANull())
                    fase.QTAPIA = faseRow.QTAPIA;
                if (!faseRow.IsQTAACCLENull())
                    fase.QTAACCLE = faseRow.QTAACCLE;
                if (!faseRow.IsRIFERIMENTO_INFRANull())
                    fase.RIFERIMENTO_INFRA = faseRow.RIFERIMENTO_INFRA;
                if (!faseRow.IsDATARIF_INFRANull())
                    fase.DATARIF_INFRA = faseRow.DATARIF_INFRA;

                _ds.PIANIFICAZIONE_FASE.AddPIANIFICAZIONE_FASERow(fase);
            }

            if (movFasi.Count() > 0)
            {
                fase.QTA = movFasi.Sum(x => x.QTA);
                fase.QTAANN = movFasi.Sum(x => x.QTAANN);
                fase.QTANET = movFasi.Sum(x => x.QTANET);
                fase.QTAASS = movFasi.Sum(x => x.QTAASS);
                fase.QTACON = movFasi.Sum(x => x.QTACON);
                fase.QTATER = movFasi.Sum(x => x.QTATER);
                fase.QTADATER = movFasi.Sum(x => x.QTADATER);
                fase.QTAACC = movFasi.Sum(x => x.QTAACC);
                fase.QTADAC = movFasi.Sum(x => x.QTADAC);
                fase.QTALAV = movFasi.Sum(x => x.QTALAV);
                fase.QTADAPIA = movFasi.Sum(x => x.QTADAPIA);
                fase.QTAPIA = movFasi.Sum(x => x.QTAPIA);
                fase.QTAACCLE = movFasi.Sum(x => x.QTAACCLE);

                fase.DATAINIZIO = movFasi.Where(x => !x.IsDATAINIZIONull()).Min(x => x.DATAINIZIO);
                fase.DATAFINE = movFasi.Where(x => !x.IsDATAFINENull()).Max(x => x.DATAFINE);

                fase.STATO = (fase.QTADATER == 0) ? StatoFasePianificazione.CHIUSO : StatoFasePianificazione.APERTO;

            }

        }

        private void AssegnaPadre(PianificazioneDS.USR_PRD_FASIRow faseRow, PianificazioneDS.PIANIFICAZIONE_FASERow fase)
        {
            if (!faseRow.IsIDPRDFASEPADRENull())
            {
                PianificazioneDS.PIANIFICAZIONE_FASERow fasePadre = _ds.PIANIFICAZIONE_FASE.Where(x => x.IDPRDFASE == faseRow.IDPRDFASEPADRE).FirstOrDefault();
                if (fasePadre == null)
                {
                    SciviLog("WARNING", string.Format("IDPRDFASEPADRE: {0} IMPOSSIBILE TROVARE PIANIFICAZIONE FASE", faseRow.IDPRDFASEPADRE));
                }
                else
                {
                    fase.IDFASEPADRE = fasePadre.IDFASE;
                    fase.IDPRDFASEPADRE = faseRow.IDPRDFASEPADRE;
                }
            }
        }

        private PianificazioneDS.PIANIFICAZIONE_LANCIORow CreaPianificazione_Lancio(long idLancio, PianificazioneDS.USR_PRD_LANCIODRow lancio)
        {
            PianificazioneDS.PIANIFICAZIONE_LANCIORow pLancio = _ds.PIANIFICAZIONE_LANCIO.NewPIANIFICAZIONE_LANCIORow();
            pLancio.AZIENDA = lancio.AZIENDA;
            if (!lancio.IsCODICECLIFONull())
                pLancio.CODICECLIFO = lancio.CODICECLIFO;

            if (!lancio.IsDATACOMMESSANull())
                pLancio.DATACOMMESSA = lancio.DATACOMMESSA;

            if (!lancio.IsDATARIFNull())
                pLancio.DATARIF = lancio.DATARIF;
            if (!lancio.IsDATARIFRIGANull())
                pLancio.DATARIFRIGA = lancio.DATARIFRIGA;
            if (!lancio.IsDATARIF_INFRANull())
                pLancio.DATARIF_INFRA = lancio.DATARIF_INFRA;
            if (!lancio.IsDATDOCNull())
                pLancio.DATDOC = lancio.DATDOC;
            if (!lancio.IsIDDIBAMETHIDNull())
                pLancio.IDDIBAMETHID = lancio.IDDIBAMETHID;

            pLancio.IDLANCIO = idLancio;
            if (!lancio.IsIDLANCIODNull())
                pLancio.IDLANCIOD = lancio.IDLANCIOD;
            if (!lancio.IsIDLANCIOTNull())
                pLancio.IDLANCIOT = lancio.IDLANCIOT;
            if (!lancio.IsIDMAGAZZNull())
                pLancio.IDMAGAZZ = lancio.IDMAGAZZ;
            if (!lancio.IsNOMECOMMESSANull())
                pLancio.NOMECOMMESSA = lancio.NOMECOMMESSA;
            if (!lancio.IsNRRIGANull())
                pLancio.NRRIGA = lancio.NRRIGA;
            if (!lancio.IsNUMDOCNull())
                pLancio.NUMDOC = lancio.NUMDOC;
            if (!lancio.IsQTALANCIONull())
                pLancio.QTALANCIO = lancio.QTALANCIO;

            if (!lancio.IsRIFERIMENTONull())
                pLancio.RIFERIMENTO = lancio.RIFERIMENTO;
            if (!lancio.IsRIFERIMENTORIGANull())
                pLancio.RIFERIMENTORIGA = lancio.RIFERIMENTORIGA;
            if (!lancio.IsRIFERIMENTO_INFRANull())
                pLancio.RIFERIMENTO_INFRA = lancio.RIFERIMENTO_INFRA;
            if (!lancio.IsSEGNALATORENull())
                pLancio.SEGNALATORE = lancio.SEGNALATORE;
            if (!lancio.IsVERSIONNull())
                pLancio.VERSION = lancio.VERSION;

            return pLancio;
        }

        private void SciviLog(string tipo, string nota)
        {
            using (PianificazioneBusiness bPianificazione = new PianificazioneBusiness())
            {
                bPianificazione.InsertPianificazioneLog(tipo, nota);
            }
        }

        private void SciviLog(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            while (ex != null)
            {
                sb.AppendLine("Errore:" + ex.Message);
                sb.AppendLine("Sorgente: " + ex.Source);
                sb.AppendLine("Stack: " + ex.StackTrace);

                ex = ex.InnerException;
            }
            string messaggio = sb.ToString();
            if (messaggio.Length > 500) messaggio = messaggio.Substring(0, 500);
            using (PianificazioneBusiness bPianificazione = new PianificazioneBusiness())
            {
                bPianificazione.InsertPianificazioneLog("EXCEPTION", messaggio);
            }
            Console.WriteLine(messaggio);
        }
    }
}
