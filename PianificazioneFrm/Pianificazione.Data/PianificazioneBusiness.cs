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
        public void InsertPianificazioneLog(string Tipo, string Nota)
        {
            PianificazioneAdapter a = new PianificazioneAdapter(DbConnection, DbTransaction);
            a.InsertPianificazioneLog(Tipo, Nota);
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

    }
}
