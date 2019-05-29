using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Priorita.Entities;

namespace Priorita.Data
{
    public class PrioritaAdapter : PrioritaAdapterBase
    {
        public PrioritaAdapter(System.Data.IDbConnection connection, IDbTransaction transaction) :
           base(connection, transaction)
        { }

        public void FillSEGNALATORI(PrioritaDS ds)
        {
            string select = @"select distinct cli.*
                                from usr_prd_lanciod ld
                                inner join gruppo.clifo cli on cli.codice = ld.segnalatore
                                where substr(segnalatore,1,1)='0' and segnalatore !=00001 
                                AND CLI.TIPO = 'C'
                                order by cli.RAGIONESOC";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.SEGNALATORI);
            }
        }

        public void FillREPARTI(PrioritaDS ds)
        {
            string select = @"select distinct *
                                from gruppo.clifo 
                                where substr(codice,1,1)<>'0'  
                                order by RAGIONESOC";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.REPARTI);
            }
        }

        public void FillCLIFO(PrioritaDS ds)
        {
            string select = @"select distinct *
                                from gruppo.clifo order by RAGIONESOC ";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.CLIFO);
            }
        }

        public void FillTABFAS(PrioritaDS ds)
        {
            string select = @"select distinct *
                                from gruppo.TABFAS order by CODICEFASE ";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.TABFAS);
            }
        }
        public void FillUSR_PRD_MOVFASI_Aperti(PrioritaDS ds, string codiceSegnalatore, string codiceReparto, string idtabfas, string Articolo)
        {
            string select = @"select mf.* from usr_prd_movfasi mf
                                inner join usr_prd_fasi fa on mf.idprdfase = fa.idprdfase
                                inner join usr_prd_lanciod ld on ld.idlanciod = fa.idlanciod                                
                                where 
                                mf.qtadater > 0 ";

            if (!String.IsNullOrEmpty(Articolo))
            {
                select = @"select mf.* from usr_prd_movfasi mf
                                inner join usr_prd_fasi fa on mf.idprdfase = fa.idprdfase
                                inner join usr_prd_lanciod ld on ld.idlanciod = fa.idlanciod 
                                inner join gruppo.magazz ma on ma.idmagazz = mf.idmagazz
                                where 
                                ma.modello like '%" + Articolo.ToUpper() + @"%'
                                and mf.qtadater > 0 ";
            }

            if (!string.IsNullOrEmpty(codiceSegnalatore))
                select = select + " and ld.segnalatore = '" + codiceSegnalatore + "'";

            if (!string.IsNullOrEmpty(codiceReparto))
                select = select + " and mf.codiceclifo = '" + codiceReparto + "'";

            if (!string.IsNullOrEmpty(idtabfas))
                select = select + " and mf.idtabfas = '" + idtabfas + "'";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.USR_PRD_MOVFASI);
            }
        }

        public void FillUSR_PRD_MOVFASI_Chiusi(PrioritaDS ds, string codiceSegnalatore, string codiceReparto, string idtabfas, string Articolo, int giorniIndietro)
        {
            string select = @"select mf.*
                from usr_prd_movfasi mf
                inner join usr_prd_fasi fa on mf.idprdfase = fa.idprdfase
                inner join usr_prd_lanciod ld on ld.idlanciod = fa.idlanciod
                where 
                mf.qtadater = 0
                and datamovfase >= sysdate-" + giorniIndietro.ToString();

            if (!String.IsNullOrEmpty(Articolo))
            {
                select = @"select mf.*
                            from usr_prd_movfasi mf
                            inner join usr_prd_fasi fa on mf.idprdfase = fa.idprdfase
                            inner join usr_prd_lanciod ld on ld.idlanciod = fa.idlanciod
                            inner join gruppo.magazz ma on ma.idmagazz = mf.idmagazz
                            where ma.modello like '%" + Articolo.ToUpper() + @"%'
                            and mf.qtadater = 0
                            and datamovfase >= sysdate-" + giorniIndietro.ToString();
            }
            if (!string.IsNullOrEmpty(codiceSegnalatore))
                select = select + " and ld.segnalatore = '" + codiceSegnalatore + "'";

            if (!string.IsNullOrEmpty(codiceReparto))
                select = select + " and mf.codiceclifo = '" + codiceReparto + "'";

            if (!string.IsNullOrEmpty(idtabfas))
                select = select + " and mf.idtabfas = '" + idtabfas + "'";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.USR_PRD_MOVFASI);
            }
        }

        public void FillMAGAZZ(PrioritaDS ds, List<string> IDMAGAZZ)
        {
            string inCOndition = ConvertToStringForInCondition(IDMAGAZZ);

            string select = @"SELECT DISTINCT * FROM GRUPPO.MAGAZZ WHERE IDMAGAZZ in ( {0} )";
            select = string.Format(select, inCOndition);

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.MAGAZZ);
            }
        }

        public void FillUSR_PRD_FASI(PrioritaDS ds, List<string> IDPRDFASE)
        {
            string inCOndition = ConvertToStringForInCondition(IDPRDFASE);

            string select = @"SELECT DISTINCT * FROM USR_PRD_FASI WHERE IDPRDFASE in ( {0} )";
            select = string.Format(select, inCOndition);

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.USR_PRD_FASI);
            }
        }

        public void FillRW_SCADENZE(PrioritaDS ds, DateTime dtInizio, DateTime dtFine)
        {
            string select = @"select * from rw_scadenze WHERE DATA BETWEEN $P{DATA1} and $P{DATA2}";

            ParamSet ps = new ParamSet();
            ps.AddParam("DATA1", DbType.DateTime, dtInizio);
            ps.AddParam("DATA2", DbType.DateTime, dtFine);

            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.RW_SCADENZE);
            }
        }

        public void FillUSR_PRD_FLUSSO_MOVFASI_By_RW_SCADENZE(PrioritaDS ds, DateTime dtInizio, DateTime dtFine)
        {
            string select = @"select fmf.* from USR_PRD_FLUSSO_MOVFASI fmf 
                                inner join rw_scadenze sc on sc.idprdmovfase = fmf.idprdmovfase
                                WHERE sc.DATA BETWEEN $P{DATA1} and $P{DATA2}";

            ParamSet ps = new ParamSet();
            ps.AddParam("DATA1", DbType.DateTime, dtInizio);
            ps.AddParam("DATA2", DbType.DateTime, dtFine);

            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.USR_PRD_FLUSSO_MOVFASI);
            }
        }

        public void FillRW_SCADENZE(PrioritaDS ds, List<string> IDPRDMOVFASE)
        {
            string inCOndition = ConvertToStringForInCondition(IDPRDMOVFASE);

            string select = @"SELECT DISTINCT * FROM RW_SCADENZE WHERE IDPRDMOVFASE in ( {0} )";
            select = string.Format(select, inCOndition);

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.RW_SCADENZE);
            }
        }
        public void FillUSR_PRD_LANCIOD(PrioritaDS ds, List<string> IDLANCIOD)
        {
            string inCOndition = ConvertToStringForInCondition(IDLANCIOD);

            string select = @"SELECT DISTINCT * FROM USR_PRD_LANCIOD WHERE IDLANCIOD in ( {0} )";
            select = string.Format(select, inCOndition);

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.USR_PRD_LANCIOD);
            }
        }

        public void FillUSR_PRD_FLUSSO_MOVFASI(PrioritaDS ds, string IDPRDMOVFASE)
        {

            string select = @"select mv.*,cau.desprdcaufase as causale from usr_prd_flusso_movfasi mv
                                inner join gruppo.usr_prd_caufasi cau on cau.idprdcaufase = mv.idprdcaufase
                                where idprdmovfase = $P<IDPRDMOVFASE>
                                order by idflussomovfase";
            ParamSet ps = new ParamSet();
            ps.AddParam("IDPRDMOVFASE", DbType.String, IDPRDMOVFASE);

            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.USR_PRD_FLUSSO_MOVFASI);
            }
        }

        public void FillUSR_VENDITET(PrioritaDS ds, List<string> IDPRDMOVFASE)
        {
            string inCOndition = ConvertToStringForInCondition(IDPRDMOVFASE);

            string select = @" select distinct mm.idprdmovfase,vt.*
                                 from usr_prd_movmate mm
                                 inner join USR_PRD_FLUSSO_MOVMATE fmm on fmm.idprdmovmate = mm.idprdmovmate
                                 inner join usr_venditet vt on vt.idvenditet = fmm.idvenditet
                                 WHERE mm.IDPRDMOVFASE in ( {0} )";
            select = string.Format(select, inCOndition);

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.USR_VENDITET);
            }
        }

        public void UpdateTable(string tablename, PrioritaDS ds)
        {
            string query = string.Format(CultureInfo.InvariantCulture, "SELECT * FROM {0}", tablename);

            using (DbDataAdapter a = BuildDataAdapter(query))
            {
                try
                {
                    a.ContinueUpdateOnError = false;
                    DataTable dt = ds.Tables[tablename];
                    DbCommandBuilder cmd = BuildCommandBuilder(a);
                    a.UpdateCommand = cmd.GetUpdateCommand();
                    a.DeleteCommand = cmd.GetDeleteCommand();
                    a.InsertCommand = cmd.GetInsertCommand();
                    a.Update(dt);
                }
                catch (DBConcurrencyException ex)
                {

                }
                catch
                {
                    throw;
                }
            }
        }

    }
}
