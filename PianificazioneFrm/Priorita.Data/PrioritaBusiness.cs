using Pianificazione.Data.Core;
using Priorita.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Priorita.Data
{
    public class PrioritaBusiness : PrioritaBusinessBase
    {
        public PrioritaBusiness() : base() { }

        [DataContext]
        public void FillSEGNALATORI(PrioritaDS ds)
        {
            PrioritaAdapter a = new PrioritaAdapter(DbConnection, DbTransaction);
            a.FillSEGNALATORI(ds);
        }

        [DataContext]
        public void FillREPARTI(PrioritaDS ds)
        {
            PrioritaAdapter a = new PrioritaAdapter(DbConnection, DbTransaction);
            a.FillREPARTI(ds);
        }

        [DataContext]
        public void FillCLIFO(PrioritaDS ds)
        {
            PrioritaAdapter a = new PrioritaAdapter(DbConnection, DbTransaction);
            a.FillCLIFO(ds);
        }

        [DataContext]
        public void FillTABFAS(PrioritaDS ds)
        {
            PrioritaAdapter a = new PrioritaAdapter(DbConnection, DbTransaction);
            a.FillTABFAS(ds);
        }

        [DataContext]
        public void FillUSR_PRD_MOVFASI_Aperti(PrioritaDS ds, string codiceSegnalatore, string codiceReparto, string idtabfas, string Articolo)
        {
            PrioritaAdapter a = new PrioritaAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_MOVFASI_Aperti(ds, codiceSegnalatore, codiceReparto, idtabfas, Articolo);
        }

        [DataContext]
        public void FillUSR_PRD_MOVFASI_Chiusi(PrioritaDS ds, string codiceSegnalatore, string codiceReparto, string idtabfas, string Articolo, int giorniIndietro)
        {
            PrioritaAdapter a = new PrioritaAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_MOVFASI_Chiusi(ds, codiceSegnalatore, codiceReparto, idtabfas, Articolo, giorniIndietro);
        }

        [DataContext]
        public void FillMAGAZZ(PrioritaDS ds, List<string> IDMAGAZZ)
        {
            List<string> articoliPresenti = ds.MAGAZZ.Select(x => x.IDMAGAZZ).Distinct().ToList();
            List<string> articoliMancanti = IDMAGAZZ.Except(articoliPresenti).ToList();

            PrioritaAdapter a = new PrioritaAdapter(DbConnection, DbTransaction);
            while (articoliMancanti.Count > 0)
            {
                List<string> articoliDaCaricare;
                if (articoliMancanti.Count > 999)
                {
                    articoliDaCaricare = articoliMancanti.GetRange(0, 999);
                    articoliMancanti.RemoveRange(0, 999);
                }
                else
                {
                    articoliDaCaricare = articoliMancanti.GetRange(0, articoliMancanti.Count);
                    articoliMancanti.RemoveRange(0, articoliMancanti.Count);
                }
                a.FillMAGAZZ(ds, articoliDaCaricare);
            }
        }

        [DataContext]
        public void FillUSR_PRD_FASI(PrioritaDS ds, List<string> IDPRDFASE)
        {
            PrioritaAdapter a = new PrioritaAdapter(DbConnection, DbTransaction);
            while (IDPRDFASE.Count > 0)
            {
                List<string> articoliDaCaricare;
                if (IDPRDFASE.Count > 999)
                {
                    articoliDaCaricare = IDPRDFASE.GetRange(0, 999);
                    IDPRDFASE.RemoveRange(0, 999);
                }
                else
                {
                    articoliDaCaricare = IDPRDFASE.GetRange(0, IDPRDFASE.Count);
                    IDPRDFASE.RemoveRange(0, IDPRDFASE.Count);
                }
                a.FillUSR_PRD_FASI(ds, articoliDaCaricare);
            }
        }

        [DataContext]
        public void FillRW_SCADENZE(PrioritaDS ds, List<string> IDPRDMOVFASE)
        {
            PrioritaAdapter a = new PrioritaAdapter(DbConnection, DbTransaction);
            while (IDPRDMOVFASE.Count > 0)
            {
                List<string> articoliDaCaricare;
                if (IDPRDMOVFASE.Count > 999)
                {
                    articoliDaCaricare = IDPRDMOVFASE.GetRange(0, 999);
                    IDPRDMOVFASE.RemoveRange(0, 999);
                }
                else
                {
                    articoliDaCaricare = IDPRDMOVFASE.GetRange(0, IDPRDMOVFASE.Count);
                    IDPRDMOVFASE.RemoveRange(0, IDPRDMOVFASE.Count);
                }
                a.FillRW_SCADENZE(ds, articoliDaCaricare);
            }
        }

        [DataContext]
        public void FillRW_SCADENZE(PrioritaDS ds, DateTime dtInizio, DateTime dtFine)
        {
            PrioritaAdapter a = new PrioritaAdapter(DbConnection, DbTransaction);
            a.FillRW_SCADENZE(ds, dtInizio, dtFine);
        }

        [DataContext]
        public void FillUSR_PRD_FLUSSO_MOVFASI_By_RW_SCADENZE(PrioritaDS ds, DateTime dtInizio, DateTime dtFine)
        {
            PrioritaAdapter a = new PrioritaAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_FLUSSO_MOVFASI_By_RW_SCADENZE(ds, dtInizio, dtFine);
        }

        [DataContext]
        public void FillUSR_VENDITET(PrioritaDS ds, List<string> IDPRDMOVFASE)
        {
            PrioritaAdapter a = new PrioritaAdapter(DbConnection, DbTransaction);
            while (IDPRDMOVFASE.Count > 0)
            {
                List<string> articoliDaCaricare;
                if (IDPRDMOVFASE.Count > 999)
                {
                    articoliDaCaricare = IDPRDMOVFASE.GetRange(0, 999);
                    IDPRDMOVFASE.RemoveRange(0, 999);
                }
                else
                {
                    articoliDaCaricare = IDPRDMOVFASE.GetRange(0, IDPRDMOVFASE.Count);
                    IDPRDMOVFASE.RemoveRange(0, IDPRDMOVFASE.Count);
                }
                a.FillUSR_VENDITET(ds, articoliDaCaricare);
            }
        }

        [DataContext]
        public void FillUSR_PRD_LANCIOD(PrioritaDS ds, List<string> idIDLANCIOD)
        {
            PrioritaAdapter a = new PrioritaAdapter(DbConnection, DbTransaction);
            while (idIDLANCIOD.Count > 0)
            {
                List<string> articoliDaCaricare;
                if (idIDLANCIOD.Count > 999)
                {
                    articoliDaCaricare = idIDLANCIOD.GetRange(0, 999);
                    idIDLANCIOD.RemoveRange(0, 999);
                }
                else
                {
                    articoliDaCaricare = idIDLANCIOD.GetRange(0, idIDLANCIOD.Count);
                    idIDLANCIOD.RemoveRange(0, idIDLANCIOD.Count);
                }
                a.FillUSR_PRD_LANCIOD(ds, articoliDaCaricare);
            }
        }

        [DataContext]
        public void FillUSR_PRD_FLUSSO_MOVFASI(PrioritaDS ds, string IDPRMOVFASE)
        {
            PrioritaAdapter a = new PrioritaAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_FLUSSO_MOVFASI(ds, IDPRMOVFASE);
        }

        [DataContext(true)]
        public void UpdateRW_SCADENZE(PrioritaDS ds)
        {
            PrioritaAdapter a = new PrioritaAdapter(DbConnection, DbTransaction);
            a.UpdateTable("RW_SCADENZE", ds);
        }
    }
}
