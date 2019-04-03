using Pianificazione.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pianificazione.Data.Core;

namespace Pianificazione.Data
{
    public class PianificazioneBusiness : PianificazioneBusinessBase
    {
        public PianificazioneBusiness() : base() { }

        [DataContext]
        public void FillMAGAZZ(PianificazioneDS ds, List<string> IDMAGAZZ)
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            a.FillMAGAZZ(ds, IDMAGAZZ);
        }

        [DataContext]
        public void FillUSR_PRD_FASI_ROOT(PianificazioneDS ds, DateTime dataInizio, DateTime dataFine)
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_FASI_ROOT(ds, dataInizio, dataFine);
        }

        [DataContext]
        public void FillPIANIFICAZIONE_FASEPerCommessa(PianificazioneDS ds, string commessa)
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            a.FillPIANIFICAZIONE_FASEPerCommessa(ds, commessa);
        }

        [DataContext]
        public void FillUSR_PRD_MOVFASI(PianificazioneDS ds, List<string> IDPRDFASE)
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_MOVFASI(ds, IDPRDFASE);
        }

        [DataContext]
        public void FillUSR_PRD_FASI(PianificazioneDS ds, List<string> IDROOTPRDFASE)
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_FASI(ds, IDROOTPRDFASE);
        }

        [DataContext]
        public void FillUSR_PRD_LANCIOD(PianificazioneDS ds, string IDLANCIOD)
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_LANCIOD(ds, IDLANCIOD);
        }

        [DataContext]
        public void FillPIANIFICAZIONE_LANCIOByIdLancio(PianificazioneDS ds, decimal IDLANCIO)
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            a.FillPIANIFICAZIONE_LANCIOByIdLancio(ds, IDLANCIO);
        }

        [DataContext]
        public void FillUSR_PRD_MOVFASIAperti(PianificazioneDS ds)
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_MOVFASIAperti(ds);
        }

        [DataContext]
        public void FillUSR_PRD_LANCIODAperti(PianificazioneDS ds)
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_LANCIODAperti(ds);
        }

        [DataContext]
        public void FillPIAN_CATENA_COMMESSA(PianificazioneDS ds)
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            a.FillPIAN_CATENA_COMMESSA(ds);
        }

        [DataContext]
        public void FillUSR_PRD_FASIAperti(PianificazioneDS ds)
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_FASIAperti(ds);
        }

        [DataContext]
        public void FillUSR_PRD_FASIByIDLANCIOD(PianificazioneDS ds, string IDLANCIOD)
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_FASIByIDLANCIOD(ds, IDLANCIOD);
        }

        [DataContext]
        public void FillUSR_PRD_MOVFASIByIDLANCIOD(PianificazioneDS ds, string IDLANCIOD)
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_MOVFASIByIDLANCIOD(ds, IDLANCIOD);
        }

        [DataContext]
        public void FillPIANIFICAZIONE_FASE(PianificazioneDS ds, string IDLANCIOD)
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            a.FillPIANIFICAZIONE_FASE(ds, IDLANCIOD);
        }

        [DataContext]
        public void FillPIANIFICAZIONE_LANCIO(PianificazioneDS ds, string IDLANCIOD)
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            a.FillPIANIFICAZIONE_LANCIO(ds, IDLANCIOD);
        }

        [DataContext(true)]
        public void InsertPianificazioneLog(string Tipo, string Nota, string Applicazione)
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            a.InsertPianificazioneLog(Tipo, Nota, Applicazione);
        }

        [DataContext]
        public void TruncateTable(string tabella)
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            a.TruncateTable(tabella);
        }
        [DataContext]
        public long GetID()
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            return a.GetID();
        }
        [DataContext(true)]
        public void SalvaPianificazione(PianificazioneDS ds)
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            a.UpdateTable(ds.PIANIFICAZIONE_LANCIO.TableName, ds);
            a.UpdateTable(ds.PIANIFICAZIONE_FASE.TableName, ds);
        }

        [DataContext(true)]
        public void SalvaPianificazioneStatica(PianificazioneDS ds)
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            a.UpdateTable(ds.PIANIFICAZIONE_STATICA.TableName, ds);
        }

        [DataContext]
        public void FillV_PIAN_AGGR_2(PianificazioneDS ds, DateTime dataInizio, DateTime dataFine, string reparto, string fase, string tipoODL)
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            a.FillV_PIAN_AGGR_2(ds, dataInizio, dataFine, reparto, fase,tipoODL);
        }

        [DataContext]
        public void FillPIANIFICAZIONE_STATICA(PianificazioneDS ds, DateTime dataInizio, DateTime dataFine)
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            a.FillPIANIFICAZIONE_STATICA(ds, dataInizio, dataFine);
        }

        [DataContext]
        public void FillTABFAS(PianificazioneDS ds)
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            a.FillTABFAS(ds);
        }

        [DataContext(true)]
        public void SalvaPianificazione_ODL(PianificazioneDS ds)
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            a.UpdateTable(ds.PIANIFICAZIONE_ODL.TableName, ds);
        }
        [DataContext(true)]
        public void CopiaPianificazioneSuRuntime()
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            a.CopiaPianificazioneSuRuntime();
        }
        [DataContext(true)]
        public void CopiaPianificazioneAggregata()
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            a.CopiaPianificazioneAggregata();
        }
        [DataContext(true)]
        public void SalvaTemporanea(PianificazioneDS ds)
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            a.UpdateTable(ds.PIAN_CATENA_COMMESSA.TableName, ds);
        }
        [DataContext(true)]
        public void ImpostaFaseAnnullataPerQuantita()
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            a.ImpostaFaseAnnullataPerQuantita();
        }

        [DataContext]
        public void FillUSR_ACCTO_CON(PianificazioneDS ds, string IDPRDFASE)
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            a.FillUSR_ACCTO_CON(ds, IDPRDFASE);
        }

        [DataContext]
        public List<string> GetDestinazioneMaterialeOrdineLavoro(string IDPRDFLUSSOMOVMATE)
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            return a.GetDestinazioneMaterialeOrdineLavoro(IDPRDFLUSSOMOVMATE);
        }

        [DataContext]
        public List<string> GetDestinazioneMaterialeDiCommessa(string IDPRDMOVMATE)
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            return a.GetDestinazioneMaterialeDiCommessa(IDPRDMOVMATE);
        }

        [DataContext]
        public void FillUSR_PRD_FASIDaIDPRDMATE(PianificazioneDS ds, string IDPRDMATE)
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_FASIDaIDPRDMATE(ds, IDPRDMATE);

        }

        [DataContext]
        public void FillUSR_PRD_FASI_Sorelle(PianificazioneDS ds, string IDPRDFASE)
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_FASI_Sorelle(ds, IDPRDFASE);

        }

        [DataContext]
        public void FillUSR_PRD_FASI_INFRAGRUPPO(PianificazioneDS ds, string IDPRDFASE_FROM)
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_FASI_INFRAGRUPPO(ds, IDPRDFASE_FROM);


        }
        [DataContext]
        public List<string> GetDestinazioneOrdineCliente(string IDVENDITED)
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            return a.GetDestinazioneOrdineCliente(IDVENDITED);
        }

        [DataContext]
        public void FillUSR_INFRA_FASE_TO_FASE(PianificazioneDS ds, DateTime dataLimiteRicerche)
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            a.FillUSR_INFRA_FASE_TO_FASE(ds, dataLimiteRicerche);
        }

        [DataContext]
        public void FillUSR_PRD_FASI_ConAccantonatoDaLavorare(PianificazioneDS ds)
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_FASI_ConAccantonatoDaLavorare(ds);
        }
        [DataContext]
        public void FillUSR_PRD_FASI_FaseFinaleCommessaDaIDORIGINE_Tipo_1(PianificazioneDS ds, string IDPRDMOVMATE)
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_FASI_FaseFinaleCommessaDaIDORIGINE_Tipo_1(ds, IDPRDMOVMATE);
        }
        [DataContext]
        public void FillUSR_PRD_FASI_FaseRipartenzaCommessaDaIDORIGINE_Tipo_1(PianificazioneDS ds, string IDPRDMOVMATE)
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_FASI_FaseRipartenzaCommessaDaIDORIGINE_Tipo_1(ds, IDPRDMOVMATE);
        }

        [DataContext]
        public void FillUSR_PRD_FASI_FaseFinaleCommessaDaIDORIGINE_Tipo_2(PianificazioneDS ds, string IDORIGINE)
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_FASI_FaseFinaleCommessaDaIDORIGINE_Tipo_2(ds, IDORIGINE);
        }
        [DataContext]
        public void FillUSR_PRD_FASI_FaseRipartenzaCommessaDaIDORIGINE_Tipo_2(PianificazioneDS ds, string IDORIGINE)
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_FASI_FaseRipartenzaCommessaDaIDORIGINE_Tipo_2(ds, IDORIGINE);
        }

    }
}
