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
        private PianificazioneDS _dsInfragruppo;
        private PianificazioneDS _dsAccantonato;
        private List<string> _elencoIDPRDFASI_to_infragruppo = new List<string>();
        private List<string> _elencoIDPRDFASI_to_accantonato = new List<string>();
        private string Applicazione;
        private DateTime _dataLimiteRicerche;

        public PianificazioneService(string dataLimiteRicerche)
        {
            DateTime dataInizioStandard = new DateTime(2018, 9, 1);
            if (string.IsNullOrEmpty(dataLimiteRicerche)) _dataLimiteRicerche = dataInizioStandard;

            string[] str = dataLimiteRicerche.Split('/');
            if (str.Length < 3) _dataLimiteRicerche = dataInizioStandard;
            try
            {
                int giorno = int.Parse(str[0]);
                int mese = int.Parse(str[1]);
                int anno = int.Parse(str[2]);
                _dataLimiteRicerche = new DateTime(anno, mese, giorno);
            }
            catch
            {
                _dataLimiteRicerche = dataInizioStandard;
                SciviLog("ERRORE", "ERRORE nella conversione della dataLimiteRicerche", "");
            }
        }

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
            Applicazione = "Pianificazione lanci";
            try
            {
                CaricaListaLanciAperti();
            }
            catch (Exception ex)
            {
                SciviLog(ex, Applicazione);
                return;
            }
            SciviLog("START", string.Format("Trovati {0} lanci da verificare", _ds.USR_PRD_LANCIOD.Count), Applicazione);

            int numeroElementiLancio = _ds.USR_PRD_LANCIOD.Count();
            int contatoreLancio = 1;
            foreach (PianificazioneDS.USR_PRD_LANCIODRow lancio in _ds.USR_PRD_LANCIOD.OrderBy(x => x.IDLANCIOD))
            {
                using (PianificazioneBusiness bPianificazione = new PianificazioneBusiness())
                {
                    try
                    {
                        SciviLog("INFO", string.Format("Elaborazione {0} di {1} IDLANCIOD: {2}", contatoreLancio, numeroElementiLancio, lancio.IDLANCIOD), Applicazione);
                        contatoreLancio++;

                        bPianificazione.FillUSR_PRD_FASIByIDLANCIOD(_ds, lancio.IDLANCIOD);
                        bPianificazione.FillUSR_PRD_MOVFASIByIDLANCIOD(_ds, lancio.IDLANCIOD);

                        bPianificazione.FillPIANIFICAZIONE_LANCIO(_ds, lancio.IDLANCIOD);
                        bPianificazione.FillPIANIFICAZIONE_FASE(_ds, lancio.IDLANCIOD);


                        PianificazioneDS.PIANIFICAZIONE_LANCIORow pLancio = _ds.PIANIFICAZIONE_LANCIO.Where(x => x.IDLANCIOD == lancio.IDLANCIOD).FirstOrDefault();
                        if (pLancio == null)
                        {
                            long idlancio = bPianificazione.GetID();
                            SciviLog("INFO", string.Format("IDLANCIOD: {0} non trovato in PIANIFICAZIONE_LANCIO CREATO NUOVO CON ID: {1}", lancio.IDLANCIOD, idlancio), Applicazione);
                            pLancio = CreaPianificazione_Lancio(idlancio, lancio);
                            _ds.PIANIFICAZIONE_LANCIO.AddPIANIFICAZIONE_LANCIORow(pLancio);
                        }
                        else
                        {
                            SciviLog("INFO", string.Format("IDLANCIOD: {0} trovato in PIANIFICAZIONE_LANCIO CON ID: {1}", lancio.IDLANCIOD, pLancio.IDLANCIO), Applicazione);
                        }

                        PianificazioneDS.USR_PRD_FASIRow faseRoot = _ds.USR_PRD_FASI.Where(x => x.ROOTSN == 1 && x.IDLANCIOD == lancio.IDLANCIOD).FirstOrDefault();
                        if (faseRoot == null)
                        {
                            SciviLog("WARNING", string.Format("IDLANCIOD: {0} IMPOSSIBILE TROVARE FASE ROOT", lancio.IDLANCIOD), Applicazione);
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
                        SciviLog("ERRORE", string.Format("IDLANCIOD: {0} ECCEZIONE AL PASSO {1} DI {2}", lancio.IDLANCIOD, contatoreLancio - 1, numeroElementiLancio), Applicazione);
                        SciviLog(ex, Applicazione);
                    }

                }
            }

            using (PianificazioneBusiness bPianificazione = new PianificazioneBusiness())
            {
                bPianificazione.ImpostaFaseAnnullataPerQuantita();
                SciviLog("INFO", "ImpostaFaseAnnullataPerQuantita", Applicazione);
            }
            SciviLog("END", "Fine elaborazione", Applicazione);

        }

        private void CorreggiDate(PianificazioneDS.PIANIFICAZIONE_LANCIORow pLancio)
        {
            PianificazioneDS.PIANIFICAZIONE_FASERow faseRoot = _ds.PIANIFICAZIONE_FASE.Where(x => x.IDLANCIO == pLancio.IDLANCIO).FirstOrDefault();
            if (faseRoot == null)
            {
                SciviLog("WARNING", string.Format("IDLANCIO: {0} non ha fase di root", pLancio.IDLANCIO), Applicazione);
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
                    SciviLog("WARNING", string.Format("IDPRDFASEPADRE: {0} IMPOSSIBILE TROVARE PIANIFICAZIONE FASE", faseRow.IDPRDFASEPADRE), Applicazione);
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

        private void SciviLog(string tipo, string nota, string Applicazione)
        {
            using (PianificazioneBusiness bPianificazione = new PianificazioneBusiness())
            {
                bPianificazione.InsertPianificazioneLog(tipo, nota, Applicazione);
            }
        }

        private void SciviLog(Exception ex, string Applicazione)
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
                bPianificazione.InsertPianificazioneLog("EXCEPTION", messaggio, Applicazione);
            }
            Console.WriteLine(messaggio);
        }

        public void CreaPianificazioneSuBaseODL()
        {
            DateTime dtInizio = DateTime.Now;
            _ds = new PianificazioneDS();
            _dsInfragruppo = new PianificazioneDS();
            _dsAccantonato = new PianificazioneDS();
            _elencoIDPRDFASI_to_infragruppo = new List<string>();
            _elencoIDPRDFASI_to_accantonato = new List<string>();
            Applicazione = "Pianificazione base ODL";
            try
            {
                using (PianificazioneBusiness bPianificazione = new PianificazioneBusiness())
                {
                    bPianificazione.TruncateTable(_ds.PIANIFICAZIONE_ODL.TableName);
                    bPianificazione.FillUSR_PRD_MOVFASIAperti(_ds);
                    bPianificazione.FillUSR_PRD_FASIAperti(_ds);
                    bPianificazione.FillUSR_PRD_LANCIODAperti(_ds);
                    bPianificazione.FillPIAN_CATENA_COMMESSA(_ds);

                    bPianificazione.FillUSR_INFRA_FASE_TO_FASE(_ds, _dataLimiteRicerche);
                }
            }
            catch (Exception ex)
            {
                SciviLog(ex, Applicazione);
                return;
            }
            SciviLog("START", string.Format("Trovati {0} ODL aperti", _ds.USR_PRD_MOVFASI.Count), Applicazione);

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
                        if (contatoreODL % 50 == 0)
                            SciviLog("INFO", string.Format("Elaborazione {0} di {1} ODL: {2}", contatoreODL, numeroODLAperti, IDPRDMOVFASE_ORIGINE), Applicazione);
                        contatoreODL++;

                        if (odl.IsIDPRDFASENull())
                        {
                            SciviLog("WARNING", string.Format("ODL: {0} IDPRDFASE NULL", IDPRDMOVFASE_ORIGINE), Applicazione);
                            continue;
                        }


                        PianificazioneDS.USR_PRD_FASIRow fase = _ds.USR_PRD_FASI.Where(x => x.IDPRDFASE == odl.IDPRDFASE).FirstOrDefault();
                        if (fase == null)
                        {
                            SciviLog("WARNING", string.Format("ODL: {0} IMPOSSIBILE TROVARE FASE", IDPRDMOVFASE_ORIGINE), Applicazione);
                            continue;
                        }

                        decimal quantita = odl.QTA;
                        CreaPianificazione_ODL(odl, fase, IDPRDMOVFASE_ORIGINE, attendibilita, quantita, false, false);

                        while (!fase.IsIDPRDFASEPADRENull())
                        {
                            fase = _ds.USR_PRD_FASI.Where(x => x.IDPRDFASE == fase.IDPRDFASEPADRE).FirstOrDefault();

                            if (
                                fase.IDTABFAS == "0000000077" || // SALNDATURA
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
                            CreaPianificazione_ODL(null, fase, IDPRDMOVFASE_ORIGINE, attendibilita, quantita, false, false);
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
                        SciviLog("ERRORE", string.Format("ODL: {0} ECCEZIONE AL PASSO {1} DI {2}", odl.IDPRDMOVFASE, contatoreODL - 1, numeroODLAperti), Applicazione);
                        SciviLog(ex, Applicazione);
                    }

                }
            }
            using (PianificazioneBusiness bPianificazione = new PianificazioneBusiness())
            {
                bPianificazione.TruncateTable("PIANIFICAZIONE_RUNTIME");
                bPianificazione.CopiaPianificazioneSuRuntime();
            }

            double durata = (DateTime.Now - dtInizio).TotalMinutes;
            string messaggio = string.Format("Fine elaborazione - {0}", durata);
            SciviLog("END", messaggio, Applicazione);
        }

        public void TrovaOCPerFasiAccantonate()
        {
            DateTime dtInizio = DateTime.Now;
            _ds = new PianificazioneDS();
            Applicazione = "OC per accantonati";
            try
            {
                using (PianificazioneBusiness bPianificazione = new PianificazioneBusiness())
                {
                    bPianificazione.TruncateTable(_ds.PIAN_CATENA_COMMESSA.TableName);

                    bPianificazione.FillUSR_PRD_FASI_ConAccantonatoDaLavorare(_ds);
                }
            }
            catch (Exception ex)
            {
                SciviLog(ex, Applicazione);
                return;
            }
            SciviLog("START", string.Format("Trovati {0} Fasi ", _ds.USR_PRD_FASI.Count), Applicazione);

            int fasiDaLavorare = _ds.USR_PRD_FASI.Count();
            int contatoreFasi = 1;

            foreach (PianificazioneDS.USR_PRD_FASIRow fase in _ds.USR_PRD_FASI)
            {
                string idprdfaseOrigine = fase.IDPRDFASE;
                using (PianificazioneBusiness bPianificazione = new PianificazioneBusiness())
                {
                    try
                    {
                        if (contatoreFasi % 50 == 0)
                            SciviLog("INFO", string.Format("Elaborazione {0} di {1} Fase: {2}", contatoreFasi, fasiDaLavorare, idprdfaseOrigine), Applicazione);
                        contatoreFasi++;

                        using (PianificazioneDS ds1 = new PianificazioneDS())
                        {
                            TrovaOC(ds1, fase, fase.IDPRDFASE, 0, string.Empty, 0);
                        }
                    }
                    catch (Exception ex)
                    {
                        SciviLog("ERRORE", string.Format("Fase: {0} ECCEZIONE AL PASSO {1} DI {2}", fase.IDPRDFASE, contatoreFasi - 1, fasiDaLavorare), Applicazione);
                        SciviLog(ex, Applicazione);
                    }

                }
            }
            double durata = (DateTime.Now - dtInizio).TotalMinutes;
            string messaggio = string.Format("Fine elaborazione - {0}", durata);
            SciviLog("END", messaggio, Applicazione);

        }

        private void CreaRigaCatenaCommessa(string riferimento, string padre, string IDPRDFASE, int livello, int durata, int durata_cumulativa, bool OC, decimal quatita_accantonata, string idFaseRipartenza, DateTime dataAccantonato)
        {
            using (PianificazioneBusiness bPianificazione = new PianificazioneBusiness())
            {
                PianificazioneDS.PIAN_CATENA_COMMESSARow cc = _ds.PIAN_CATENA_COMMESSA.NewPIAN_CATENA_COMMESSARow();
                long idcc = bPianificazione.GetID();
                cc.IDCC = idcc;
                cc.RIFERIMENTO = riferimento;
                cc.PADRE = padre;
                cc.IDPRDFASE = IDPRDFASE;
                cc.LIVELLO = livello;
                cc.DURATA = durata;
                cc.DURATA_COMULATIVA = durata_cumulativa;
                cc.OC = OC ? "1" : "0";
                cc.QTA_ACCANTONATA = quatita_accantonata;
                cc.QTA_LAVORATA = 0;
                cc.IDPRDFASERIPARTENZA = idFaseRipartenza;
                cc.DATAACCANTONATO = dataAccantonato;
                _ds.PIAN_CATENA_COMMESSA.AddPIAN_CATENA_COMMESSARow(cc);
                bPianificazione.SalvaTemporanea(_ds);
                _ds.PIAN_CATENA_COMMESSA.AcceptChanges();
            }
        }

        private void TrovaOC(PianificazioneDS ds, PianificazioneDS.USR_PRD_FASIRow fase, string IDPRDFASE_RIFERIMENTO, int livello, string padre, int durata_cumulativa_precedente)
        {
            ds.USR_ACCTO_CON.Clear();
            using (PianificazioneBusiness bPianificazione = new PianificazioneBusiness())
            {
                bPianificazione.FillUSR_ACCTO_CON(ds, fase.IDPRDFASE);


                foreach (PianificazioneDS.USR_ACCTO_CONRow accantonato in ds.USR_ACCTO_CON)
                {
                    switch (accantonato.ORIGINE)
                    {
                        case 0:
                            List<string> OC = bPianificazione.GetDestinazioneOrdineCliente(accantonato.IDORIGINE);
                            foreach (string oc in OC)
                            {
                                CreaRigaCatenaCommessa(oc, padre, IDPRDFASE_RIFERIMENTO, livello, 0, durata_cumulativa_precedente, true, accantonato.QUANTITA_ORI, string.Empty, accantonato.DATACONSEGNA_DEST);
                            }
                            return;

                        case 1: // materiale da commessa - USR_PRD_MATE

                            PianificazioneDS.USR_PRD_FASIRow faseDaLavorare1;
                            using (PianificazioneDS ds1 = new PianificazioneDS())
                            {
                                bPianificazione.FillUSR_PRD_FASI_FaseFinaleCommessaDaIDORIGINE_Tipo_1(ds1, accantonato.IDORIGINE);
                                faseDaLavorare1 = ds1.USR_PRD_FASI.FirstOrDefault();
                                if (faseDaLavorare1 == null) return;
                                if (faseDaLavorare1.IDPRDFASE == fase.IDPRDFASE)
                                {
                                    SciviLog("WARNING", string.Format("Fase: {0} LOOP ACCANTONAMENTO", fase.IDPRDFASE), Applicazione);
                                    return;
                                }
                                bPianificazione.FillUSR_PRD_LANCIOD(ds1, faseDaLavorare1.IDLANCIOD);
                                PianificazioneDS.USR_PRD_LANCIODRow lancio = ds1.USR_PRD_LANCIOD.Where(x => x.IDLANCIOD == faseDaLavorare1.IDLANCIOD).FirstOrDefault();
                                if (lancio == null)
                                    return;
                                livello++;
                                int durata = CalcolaDurataCommessa(ds1, faseDaLavorare1);
                                CreaRigaCatenaCommessa(lancio.NOMECOMMESSA, padre, IDPRDFASE_RIFERIMENTO, livello, durata, durata + durata_cumulativa_precedente, false, accantonato.QUANTITA_ORI, faseDaLavorare1.IDPRDFASE, accantonato.DATACONSEGNA_DEST);
                                TrovaOC(ds1, faseDaLavorare1, IDPRDFASE_RIFERIMENTO, livello, lancio.NOMECOMMESSA, durata + durata_cumulativa_precedente);
                                return;
                            }


                        case 2: // materiale ordine di lavoro - USR_PRD_FLUSSO_MOVMATE
                            PianificazioneDS.USR_PRD_FASIRow faseDaLavorare2;
                            using (PianificazioneDS ds1 = new PianificazioneDS())
                            {
                                bPianificazione.FillUSR_PRD_FASI_FaseFinaleCommessaDaIDORIGINE_Tipo_2(ds1, accantonato.IDORIGINE);
                                faseDaLavorare2 = ds1.USR_PRD_FASI.FirstOrDefault();
                                if (faseDaLavorare2 == null) return;
                                if (faseDaLavorare2.IDPRDFASE == fase.IDPRDFASE)
                                {
                                    SciviLog("WARNING", string.Format("Fase: {0} LOOP ACCANTONAMENTO", fase.IDPRDFASE), Applicazione);
                                    return;
                                }
                                bPianificazione.FillUSR_PRD_LANCIOD(ds1, faseDaLavorare2.IDLANCIOD);
                                bPianificazione.FillUSR_PRD_FASIByIDLANCIOD(ds1, faseDaLavorare2.IDLANCIOD);
                                PianificazioneDS.USR_PRD_LANCIODRow lancio = ds1.USR_PRD_LANCIOD.Where(x => x.IDLANCIOD == faseDaLavorare2.IDLANCIOD).FirstOrDefault();
                                if (lancio == null)
                                    return;
                                livello++;
                                int durata = CalcolaDurataCommessa(ds1, faseDaLavorare2);
                                CreaRigaCatenaCommessa(lancio.NOMECOMMESSA, padre, IDPRDFASE_RIFERIMENTO, livello, durata, durata + durata_cumulativa_precedente, false, accantonato.QUANTITA_ORI, faseDaLavorare2.IDPRDFASE, accantonato.DATACONSEGNA_DEST);
                                TrovaOC(ds1, faseDaLavorare2, IDPRDFASE_RIFERIMENTO, livello, lancio.NOMECOMMESSA, durata + durata_cumulativa_precedente);

                                return;
                            }
                    }
                }
                return;
            }
        }

        private int CalcolaDurataCommessa(PianificazioneDS ds, PianificazioneDS.USR_PRD_FASIRow faseDaLavorare)
        {
            decimal durata = 0;
            PianificazioneDS.USR_PRD_FASIRow fase = faseDaLavorare;
            if (!fase.IsOFFSETTIMENull())
                durata = fase.OFFSETTIME;
            while (!fase.IsIDPRDFASEPADRENull())
            {
                fase = ds.USR_PRD_FASI.Where(x => x.IDPRDFASE == fase.IDPRDFASEPADRE).FirstOrDefault();
                if (!fase.IsOFFSETTIMENull())
                    durata += fase.OFFSETTIME;
            }

            return (int)durata;
        }
        private void EstraiDestinazione(PianificazioneDS.USR_PRD_FASIRow fase, PianificazioneDS.PIANIFICAZIONE_ODLRow pODL)
        {
            List<string> valori = new List<string>();
            _ds.USR_ACCTO_CON.Clear();
            using (PianificazioneBusiness bPianificazione = new PianificazioneBusiness())
            {
                bPianificazione.FillUSR_ACCTO_CON(_ds, fase.IDPRDFASE);
                if (_ds.USR_ACCTO_CON.Count == 0) return;

                foreach (PianificazioneDS.USR_ACCTO_CONRow accantonato in _ds.USR_ACCTO_CON)
                {
                    if (accantonato.IsORIGINENull()) continue;
                    if (accantonato.IsIDORIGINENull()) continue;
                    switch (accantonato.ORIGINE)
                    {
                        case 0: // ordine cliente - USR_VENDITED
                            List<string> ListaOC = bPianificazione.GetDestinazioneOrdineCliente(accantonato.IDORIGINE);
                            valori.AddRange(ListaOC);
                            break;
                        case 1: // materiale da commessa - USR_PRD_MATE
                            List<string> listaCommessa = bPianificazione.GetDestinazioneMaterialeDiCommessa(accantonato.IDORIGINE);
                            valori.AddRange(listaCommessa);
                            break;
                        case 2: // materiale ordine di lavoro - USR_PRD_FLUSSO_MOVMATE
                            List<string> listaODL = bPianificazione.GetDestinazioneMaterialeOrdineLavoro(accantonato.IDORIGINE);
                            valori.AddRange(listaODL);
                            break;
                    }
                }
            }


            if (valori.Count == 0) return;
            StringBuilder valore = new StringBuilder();

            foreach (string str in valori)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    valore.Append(str);
                    valore.Append("; ");
                }
            }

            string destinazione = valore.ToString();
            if (destinazione.Length > 300)
                destinazione = destinazione.Substring(0, 300);
            pODL.DESTINAZIONE = destinazione;
            return;
        }

        private void CorreggiDatePianificazioneODL(PianificazioneDS.USR_PRD_MOVFASIRow odl)
        {
            PianificazioneDS.PIANIFICAZIONE_ODLRow odlAperto = _ds.PIANIFICAZIONE_ODL.Where(x => x.IDPRDMOVFASE == odl.IDPRDMOVFASE && x.STATO == StatoFasePianificazione.APERTO).FirstOrDefault();
            if (odlAperto == null)
            {
                SciviLog("ERRORE", string.Format("ODL: {0} IMPOSSIBILE TROVARE ODL APERTO PER CORREZIONE DATE", odl.IDPRDMOVFASE), Applicazione);
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

        private void CreaPianificazione_ODL(PianificazioneDS.USR_PRD_MOVFASIRow odl, PianificazioneDS.USR_PRD_FASIRow fase, string IDPRDMOVFASE_ORIGINE, int attendibilita, decimal quantita, bool daAccantonato, bool daInfragruppo)
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
            if (daAccantonato) pODL.STATO = StatoFasePianificazione.ACCANTONATO;
            if (daInfragruppo) pODL.STATO = StatoFasePianificazione.INFRAGRUPPO;

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

                if (!pODL.IsDATAFINENull())
                {
                    if (pODL.DATAFINE < DateTime.Today)
                        pODL.DATAFINE = DateTime.Today;
                }

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

            if (fase.IsIDPRDFASEPADRENull())
            {
                EstraiDestinazione(fase, pODL);

                List<PianificazioneDS.PIAN_CATENA_COMMESSARow> listaCatenaCommessa = _ds.PIAN_CATENA_COMMESSA.Where(x => x.IDPRDFASE == fase.IDPRDFASE && x.OC == "1").OrderBy(x => x.DURATA_COMULATIVA).ToList();
                if (listaCatenaCommessa.Count > 0)
                {
                    PianificazioneDS.PIAN_CATENA_COMMESSARow catenaCommessa = listaCatenaCommessa[0];
                    pODL.ORDINECLIENTE = catenaCommessa.RIFERIMENTO;
                    pODL.DURATA = catenaCommessa.DURATA_COMULATIVA;
                }

            }
            ContinuaPianificazioneODLDaAccantonato(fase, IDPRDMOVFASE_ORIGINE, attendibilita, quantita);
            ContinuaPianificazioneODLDaInfragruppo(fase, IDPRDMOVFASE_ORIGINE, attendibilita, quantita);

            _ds.PIANIFICAZIONE_ODL.AddPIANIFICAZIONE_ODLRow(pODL);

        }

        private void ContinuaPianificazioneODLDaAccantonato(PianificazioneDS.USR_PRD_FASIRow faseDaEstendere, string IDPRDMOVFASE_ORIGINE, int attendibilita, decimal quantitaFasePartenza)
        {
            using (PianificazioneBusiness bPianificazione = new PianificazioneBusiness())
            {
                decimal quantitaDaLavorare = quantitaFasePartenza;
                foreach (PianificazioneDS.PIAN_CATENA_COMMESSARow catenaCommessa in _ds.PIAN_CATENA_COMMESSA.Where(x => x.IDPRDFASE == faseDaEstendere.IDPRDFASE && x.QTA_ACCANTONATA > x.QTA_LAVORATA && x.OC == "0").OrderBy(x => x.DATAACCANTONATO))
                {
                    if (quantitaDaLavorare <= 0) continue;
                    if (quantitaDaLavorare <= (catenaCommessa.QTA_ACCANTONATA - catenaCommessa.QTA_LAVORATA))
                    {
                        catenaCommessa.QTA_LAVORATA += quantitaDaLavorare;
                        //  using (PianificazioneDS ds1 = new PianificazioneDS())
                        {

                            if (!_elencoIDPRDFASI_to_accantonato.Contains(catenaCommessa.IDPRDFASERIPARTENZA))
                            {
                                bPianificazione.FillUSR_PRD_FASI_Sorelle(_dsAccantonato, catenaCommessa.IDPRDFASERIPARTENZA);
                                _elencoIDPRDFASI_to_infragruppo.Add(catenaCommessa.IDPRDFASERIPARTENZA);
                            }
                            //else
                            //{
                            //    SciviLog("INFO", string.Format("ACCANTONATO: {0} TROVATA !", catenaCommessa.IDPRDFASERIPARTENZA), Applicazione);
                            //}

                            PianificazioneDS.USR_PRD_FASIRow fase = _dsAccantonato.USR_PRD_FASI.Where(x => x.IDPRDFASE == catenaCommessa.IDPRDFASERIPARTENZA).FirstOrDefault();

                            if (fase.IDLANCIOD == faseDaEstendere.IDLANCIOD) continue; // esclude il caso di accantonato naturale

                            attendibilita++;
                            CreaPianificazione_ODL(null, fase, IDPRDMOVFASE_ORIGINE, attendibilita, quantitaDaLavorare, true, false);

                            while (!fase.IsIDPRDFASEPADRENull())
                            {
                                fase = _ds.USR_PRD_FASI.Where(x => x.IDPRDFASE == fase.IDPRDFASEPADRE).FirstOrDefault();

                                if (
                                    fase.IDTABFAS == "0000000077" || // SALNDATURA
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
                                CreaPianificazione_ODL(null, fase, IDPRDMOVFASE_ORIGINE, attendibilita, quantitaDaLavorare, true, false);
                            }
                            quantitaDaLavorare = 0;
                        }
                    }
                    else
                    {
                        decimal quantita = (catenaCommessa.QTA_ACCANTONATA - catenaCommessa.QTA_LAVORATA);
                        quantitaDaLavorare -= quantita;
                        catenaCommessa.QTA_LAVORATA += quantitaDaLavorare;
                        //  using (PianificazioneDS ds1 = new PianificazioneDS())
                        {
                            if (!_elencoIDPRDFASI_to_accantonato.Contains(catenaCommessa.IDPRDFASERIPARTENZA))
                            {
                                bPianificazione.FillUSR_PRD_FASI_Sorelle(_dsAccantonato, catenaCommessa.IDPRDFASERIPARTENZA);
                                _elencoIDPRDFASI_to_infragruppo.Add(catenaCommessa.IDPRDFASERIPARTENZA);
                            }
                            //else
                            //{
                            //    SciviLog("INFO", string.Format("ACCANTONATO: {0} TROVATA !", catenaCommessa.IDPRDFASERIPARTENZA), Applicazione);
                            //}

                            PianificazioneDS.USR_PRD_FASIRow fase = _dsAccantonato.USR_PRD_FASI.Where(x => x.IDPRDFASE == catenaCommessa.IDPRDFASERIPARTENZA).FirstOrDefault();
                            if (fase.IDLANCIOD == faseDaEstendere.IDLANCIOD) continue; // esclude il caso di accantonato naturale
                            attendibilita++;
                            CreaPianificazione_ODL(null, fase, IDPRDMOVFASE_ORIGINE, attendibilita, quantita, true, false);

                            while (!fase.IsIDPRDFASEPADRENull())
                            {
                                fase = _ds.USR_PRD_FASI.Where(x => x.IDPRDFASE == fase.IDPRDFASEPADRE).FirstOrDefault();

                                if (
                                    fase.IDTABFAS == "0000000077" || // SALNDATURA
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
                                CreaPianificazione_ODL(null, fase, IDPRDMOVFASE_ORIGINE, attendibilita, quantita, true, false);
                            }
                        }
                    }
                }
            }

        }

        private void ContinuaPianificazioneODLDaInfragruppo(PianificazioneDS.USR_PRD_FASIRow faseDaEstendere, string IDPRDMOVFASE_ORIGINE, int attendibilita, decimal quantitaFasePartenza)
        {
            using (PianificazioneBusiness bPianificazione = new PianificazioneBusiness())
            {
                //    using (PianificazioneDS ds1 = new PianificazioneDS())
                {
                    PianificazioneDS.USR_INFRA_FASE_TO_FASERow infra = _ds.USR_INFRA_FASE_TO_FASE.Where(x => x.IDPRDFASE_FROM == faseDaEstendere.IDPRDFASE).FirstOrDefault();
                    if (infra != null)
                    {
                        if (!_elencoIDPRDFASI_to_infragruppo.Contains(infra.IDPRDFASE_TO))
                        {
                            bPianificazione.FillUSR_PRD_FASI_Sorelle(_dsInfragruppo, infra.IDPRDFASE_TO);
                            _elencoIDPRDFASI_to_infragruppo.Add(infra.IDPRDFASE_TO);
                        }
                        //else
                        //{
                        //    SciviLog("INFO", string.Format("INFRAGRUPPO TO: {0} TROVATA !", infra.IDPRDFASE_TO), Applicazione);
                        //}

                        PianificazioneDS.USR_PRD_FASIRow faseInfragruppo = _dsInfragruppo.USR_PRD_FASI.Where(x => x.IDPRDFASE == infra.IDPRDFASE_TO).FirstOrDefault();

                        if (faseInfragruppo == null) return;

                        if (faseInfragruppo.IDLANCIOD == faseDaEstendere.IDLANCIOD) return; // esclude il caso di accantonato naturale

                        attendibilita++;
                        CreaPianificazione_ODL(null, faseInfragruppo, IDPRDMOVFASE_ORIGINE, attendibilita, faseInfragruppo.QTA, false, true);

                        while (!faseInfragruppo.IsIDPRDFASEPADRENull())
                        {
                            faseInfragruppo = _dsInfragruppo.USR_PRD_FASI.Where(x => x.IDPRDFASE == faseInfragruppo.IDPRDFASEPADRE).FirstOrDefault();

                            if (
                                faseInfragruppo.IDTABFAS == "0000000077" || // SALNDATURA
                                faseInfragruppo.IDTABFAS == "0000000066" || // MONTAGGIO
                                faseInfragruppo.IDTABFAS == "0000000173" || // MONTAGGIO CAMPIONI
                                faseInfragruppo.IDTABFAS == "0000000203" || // MONTAGGIO SU FINITO
                                faseInfragruppo.IDTABFAS == "0000000202"  // MONTAGGIO SU GREZZO
                                )
                            {
                                faseInfragruppo = null;
                                break;
                            }
                            attendibilita++;
                            CreaPianificazione_ODL(null, faseInfragruppo, IDPRDMOVFASE_ORIGINE, attendibilita, faseInfragruppo.QTA, false, true);
                        }
                    }
                }

            }

        }

    }

}
