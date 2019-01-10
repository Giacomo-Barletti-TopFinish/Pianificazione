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
            SciviLog("START", string.Format("Trovati {0} lanci da verificare", _ds.USR_PRD_LANCIOD.Count));

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

            using (PianificazioneBusiness bPianificazione = new PianificazioneBusiness())
            {
                bPianificazione.ImpostaFaseAnnullataPerQuantita();
                SciviLog("INFO", "ImpostaFaseAnnullataPerQuantita");
            }
            SciviLog("END", "Fine elaborazione");

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
            int attendibilita = 1;
            CorreggiDateFasePianificata(faseRoot, ref attendibilita);
        }

        private void CorreggiDateFasePianificata(PianificazioneDS.PIANIFICAZIONE_FASERow fasePianificata, ref int attendibilita)
        {

            List<PianificazioneDS.PIANIFICAZIONE_FASERow> fasiFiglie = _ds.PIANIFICAZIONE_FASE.Where(x => !x.IsIDFASEPADRENull() && x.IDFASEPADRE == fasePianificata.IDFASE).ToList();
            if (fasiFiglie.Count == 0) return;

            DateTime finePrecedente = fasePianificata.DATAINIZIO;
            if (fasiFiglie.Any(x => x.STATO == StatoFasePianificazione.PIANIFICATO))
            {
                List<PianificazioneDS.PIANIFICAZIONE_FASERow> fasiFigliePianificate = fasiFiglie.Where(x => x.STATO == StatoFasePianificazione.PIANIFICATO).ToList();
                foreach (PianificazioneDS.PIANIFICAZIONE_FASERow faseFigliaPianificata in fasiFigliePianificate)
                {
                    CorreggiDateFasePianificata(faseFigliaPianificata, ref attendibilita);
                }
            }
            //    fasiFiglie = _ds.PIANIFICAZIONE_FASE.Where(x => !x.IsIDFASEPADRENull() && x.IDFASEPADRE == fasePianificata.IDFASE).ToList();
            attendibilita++;
            finePrecedente = fasiFiglie.Where(x => !x.IsDATAFINENull()).Max(x => x.DATAFINE);
            fasePianificata.DATAINIZIO = finePrecedente;

            finePrecedente = CorreggiGiornoSeFestivo(finePrecedente);

            fasePianificata.DATAFINE = CalcolaGiorno(finePrecedente, fasePianificata.OFFSETTIME);
            fasePianificata.ATTENDIBILITA = attendibilita;
            //fasePianificata.DATAFINE = finePrecedente.AddDays((double)fasePianificata.OFFSETTIME);
            //if (fasePianificata.OFFSETTIME == 0) fasePianificata.DATAINIZIO = finePrecedente.AddHours(1);
        }

        private DateTime CalcolaGiorno(DateTime dalGiorno, decimal offset)
        {
            if (offset == 0)
                return dalGiorno.AddHours(1);
            DateTime giorno = new DateTime();
            for (int i = 0; i < offset; i++)
            {
                giorno = dalGiorno.AddDays(1);
                giorno = CorreggiGiornoSeFestivo(giorno);
                dalGiorno = giorno;
            }

            return giorno;
        }

        private DateTime CorreggiGiornoSeFestivo(DateTime giorno)
        {

            if (giorno.Month == 1 && giorno.Day == 1)
                giorno = giorno.AddDays(1);

            if (giorno.Month == 6 && giorno.Day == 1)
                giorno = giorno.AddDays(1);

            if (giorno.Month == 25 && giorno.Day == 4)
                giorno = giorno.AddDays(1);

            if (giorno.Month == 5 && giorno.Day == 1)
                giorno = giorno.AddDays(1);

            if (giorno.Month == 6 && giorno.Day == 2)
                giorno = giorno.AddDays(1);

            if (giorno.Month == 8 && giorno.Day == 15)
                giorno = giorno.AddDays(1);

            if (giorno.Month == 11 && giorno.Day == 1)
                giorno = giorno.AddDays(1);

            if (giorno.Month == 12 && giorno.Day == 8)
                giorno = giorno.AddDays(1);

            if (giorno.Month == 12 && giorno.Day == 25)
                giorno = giorno.AddDays(1);

            if (giorno.Month == 12 && giorno.Day == 26)
                giorno = giorno.AddDays(1);

            if (giorno.DayOfWeek == DayOfWeek.Saturday)
                return giorno.AddDays(2);

            if (giorno.DayOfWeek == DayOfWeek.Sunday)
                return giorno.AddDays(1);

            return giorno;
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
                fase.ATTENDIBILITA = 2;
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

                if (fase.QTAANN > 0)
                    fase.STATO = StatoFasePianificazione.ANNULLATO;
            }

            if (fase.STATO == StatoFasePianificazione.PIANIFICATO)
                fase.ATTENDIBILITA = 2;
            else
                fase.ATTENDIBILITA = 1;

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

        public void CreaPianificazioneSuBaseODL()
        {
            _ds = new PianificazioneDS();
            try
            {
                using (PianificazioneBusiness bPianificazione = new PianificazioneBusiness())
                {

                    bPianificazione.FillUSR_PRD_MOVFASIAperti(_ds);
                    bPianificazione.FillUSR_PRD_FASIAperti(_ds);
                    bPianificazione.FillUSR_PRD_LANCIODAperti(_ds);
                }
            }
            catch (Exception ex)
            {
                SciviLog(ex);
                return;
            }
            SciviLog("START", string.Format("Trovati {0} ODL aperti", _ds.USR_PRD_MOVFASI.Count));

            int numeroODLAperti = _ds.USR_PRD_MOVFASI.Count();
            int contatoreODL = 1;

            foreach (PianificazioneDS.USR_PRD_MOVFASIRow odl in _ds.USR_PRD_MOVFASI)
            {
                string IDPRDMOVFASE_ORIGINE = odl.IDPRDMOVFASE;
                int attendibilita = 1;
                using (PianificazioneBusiness bPianificazione = new PianificazioneBusiness())
                {
                    try
                    {
                        SciviLog("INFO", string.Format("Elaborazione {0} di {1} ODL: {2}", contatoreODL, numeroODLAperti, IDPRDMOVFASE_ORIGINE));
                        contatoreODL++;

                        if (odl.IsIDPRDFASENull())
                        {
                            SciviLog("WARNING", string.Format("ODL: {0} IDPRDFASE NULL", IDPRDMOVFASE_ORIGINE));
                            continue;
                        }


                        PianificazioneDS.USR_PRD_FASIRow fase = _ds.USR_PRD_FASI.Where(x => x.IDPRDFASE == odl.IDPRDFASE).FirstOrDefault();
                        if (fase == null)
                        {
                            SciviLog("WARNING", string.Format("ODL: {0} IMPOSSIBILE TROVARE FASE", IDPRDMOVFASE_ORIGINE));
                            continue;
                        }

                        decimal quantita = odl.QTA;
                        CreaPianificazione_ODL(odl, fase, IDPRDMOVFASE_ORIGINE, attendibilita, quantita);

                        while (!fase.IsIDPRDFASEPADRENull())
                        {
                            fase = _ds.USR_PRD_FASI.Where(x => x.IDPRDFASE == fase.IDPRDFASEPADRE).FirstOrDefault();

                            if(
                                fase.IDTABFAS== "0000000077" || // SALNDATURA
                                fase.IDTABFAS == "0000000066" || // MONTAGGIO
                                fase.IDTABFAS == "0000000173" || // MONTAGGIO CAMPIONI
                                fase.IDTABFAS == "0000000203" || // MONTAGGIO SU FINITO
                                fase.IDTABFAS == "0000000202"  // MONTAGGIO SU GREZZO
                                )
                            {
                                fase = null;
                                break;
                            }
                            attendibilita++;
                            CreaPianificazione_ODL(null, fase, IDPRDMOVFASE_ORIGINE, attendibilita, quantita);
                        }

                        CorreggiDatePianificazioneODL(odl);

                        bPianificazione.SalvaPianificazione_ODL(_ds);
                        _ds.AcceptChanges();

                        _ds.PIANIFICAZIONE_LANCIO.Clear();
                        _ds.PIANIFICAZIONE_FASE.Clear();
                        _ds.PIANIFICAZIONE_ODL.Clear();
                    }
                    catch (Exception ex)
                    {
                        SciviLog("ERRORE", string.Format("ODL: {0} ECCEZIONE AL PASSO {1} DI {2}", odl.IDPRDMOVFASE, contatoreODL - 1, numeroODLAperti));
                        SciviLog(ex);
                    }

                }
            }

            SciviLog("END", "Fine elaborazione");

        }

        private void CorreggiDatePianificazioneODL(PianificazioneDS.USR_PRD_MOVFASIRow odl)
        {
            PianificazioneDS.PIANIFICAZIONE_ODLRow odlAperto = _ds.PIANIFICAZIONE_ODL.Where(x => x.IDPRDMOVFASE == odl.IDPRDMOVFASE && x.STATO == StatoFasePianificazione.APERTO).FirstOrDefault();
            if (odlAperto == null)
            {
                SciviLog("ERRORE", string.Format("ODL: {0} IMPOSSIBILE TROVARE ODL APERTO PER CORREZIONE DATE", odl.IDPRDMOVFASE));
                return;
            }


            List<PianificazioneDS.PIANIFICAZIONE_ODLRow> pianificati = _ds.PIANIFICAZIONE_ODL.Where(x => x.IDPRDMOVFASE == odl.IDPRDMOVFASE).OrderBy(x => x.ATTENDIBILITA).ToList();
            DateTime datafine = odlAperto.DATAFINE;
            foreach (PianificazioneDS.PIANIFICAZIONE_ODLRow pianificato in pianificati.Where(x => x.IDODL != odlAperto.IDODL))
            {
                pianificato.DATAINIZIO = datafine;
                pianificato.DATAFINE = CalcolaGiorno(datafine, pianificato.OFFSETTIME);

                datafine = pianificato.DATAFINE;
            }
        }

        private void CreaPianificazione_ODL(PianificazioneDS.USR_PRD_MOVFASIRow odl, PianificazioneDS.USR_PRD_FASIRow fase, string IDPRDMOVFASE_ORIGINE, int attendibilita, decimal quantita)
        {

            PianificazioneDS.PIANIFICAZIONE_ODLRow pODL = _ds.PIANIFICAZIONE_ODL.NewPIANIFICAZIONE_ODLRow();
            using (PianificazioneBusiness bPianificazione = new PianificazioneBusiness())
            {
                long IDODL = bPianificazione.GetID();
                pODL.IDODL = IDODL;
                pODL.DATA_ELABORAZIONE = DateTime.Now;
            }
            pODL.IDLANCIOD = fase.IsIDLANCIODNull() ? string.Empty : fase.IDLANCIOD;
            pODL.AZIENDA = fase.AZIENDA;
            pODL.STATO = StatoFasePianificazione.PIANIFICATO;
            pODL.IDPRDMOVFASE = IDPRDMOVFASE_ORIGINE;
            pODL.CODICECLIFO = fase.IsCODICECLIFONull() ? string.Empty : fase.CODICECLIFO;
            pODL.IDPRDFASE = fase.IsIDPRDFASENull() ? string.Empty : fase.IDPRDFASE;

            if (!fase.IsDATAINIZIONull())
                pODL.DATAINIZIO = fase.DATAINIZIO;
            if (!fase.IsDATAFINENull())
                pODL.DATAFINE = fase.DATAFINE;

            if (!fase.IsOFFSETTIMENull())
                pODL.OFFSETTIME = fase.OFFSETTIME;

            if (!fase.IsLEADTIMENull())
                pODL.LEADTIME = fase.LEADTIME;

            if (!fase.IsIDMAGAZZNull())
                pODL.IDMAGAZZ = fase.IDMAGAZZ;

            pODL.QTA = quantita;
            pODL.QTAANN = 0;
            pODL.QTANET = 0;
            pODL.QTAASS = 0;
            pODL.QTACON = 0;
            pODL.QTATER = 0;
            pODL.QTADATER = quantita;
            pODL.QTAACC = 0;
            pODL.QTADAC = 0;
            pODL.QTALAV = 0;
            pODL.QTADAPIA = 0;
            pODL.QTAPIA = 0;
            pODL.QTAACCLE = 0;

            if (!fase.IsRIFERIMENTO_INFRANull())
                pODL.RIFERIMENTO_INFRA = fase.RIFERIMENTO_INFRA;
            if (!fase.IsDATARIF_INFRANull())
                pODL.DATARIF_INFRA = fase.DATARIF_INFRA;

            pODL.ATTENDIBILITA = attendibilita;


            if (odl != null)
            {
                pODL.ATTENDIBILITA = 1;

                if (!odl.IsQTADATERNull())
                {
                    if (odl.QTADATER == 0) pODL.STATO = StatoFasePianificazione.CHIUSO;
                    else pODL.STATO = StatoFasePianificazione.APERTO;
                }

                if (!odl.IsQTAANNNull())
                    if (odl.QTAANN > 0) pODL.STATO = StatoFasePianificazione.ANNULLATO;


                pODL.IDPRDMOVFASE = odl.IDPRDMOVFASE;
                pODL.CODICECLIFO = odl.IsCODICECLIFONull() ? string.Empty : odl.CODICECLIFO;

                if (!odl.IsDATAINIZIONull())
                    pODL.DATAINIZIO = odl.DATAINIZIO;
                if (!odl.IsDATAFINENull())
                    pODL.DATAFINE = odl.DATAFINE;

                if (!odl.IsOFFSETTIMENull())
                    pODL.OFFSETTIME = odl.OFFSETTIME;

                if (!odl.IsLEADTIMENull())
                    pODL.LEADTIME = odl.LEADTIME;

                if (!odl.IsIDMAGAZZNull())
                    pODL.IDMAGAZZ = odl.IDMAGAZZ;

                if (!odl.IsQTANull())
                    pODL.QTA = odl.QTA;
                if (!odl.IsQTAANNNull())
                    pODL.QTAANN = odl.QTAANN;
                if (!odl.IsQTANETNull())
                    pODL.QTANET = odl.QTANET;
                if (!odl.IsQTAASSNull())
                    pODL.QTAASS = odl.QTAASS;
                if (!odl.IsQTACONNull())
                    pODL.QTACON = odl.QTACON;
                if (!odl.IsQTATERNull())
                    pODL.QTATER = odl.QTATER;
                if (!odl.IsQTADATERNull())
                    pODL.QTADATER = odl.QTADATER;
                if (!odl.IsQTAACCNull())
                    pODL.QTAACC = odl.QTAACC;
                if (!odl.IsQTADACNull())
                    pODL.QTADAC = odl.QTADAC;
                if (!odl.IsQTALAVNull())
                    pODL.QTALAV = odl.QTALAV;
                if (!odl.IsQTADAPIANull())
                    pODL.QTADAPIA = odl.QTADAPIA;
                if (!odl.IsQTAPIANull())
                    pODL.QTAPIA = odl.QTAPIA;
                if (!odl.IsQTAACCLENull())
                    pODL.QTAACCLE = odl.QTAACCLE;
                if (!odl.IsRIFERIMENTO_INFRANull())
                    pODL.RIFERIMENTO_INFRA = odl.RIFERIMENTO_INFRA;
                if (!odl.IsDATARIF_INFRANull())
                    pODL.DATARIF_INFRA = odl.DATARIF_INFRA;

            }
            _ds.PIANIFICAZIONE_ODL.AddPIANIFICAZIONE_ODLRow(pODL);

        }
    }
}
