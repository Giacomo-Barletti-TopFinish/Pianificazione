using Pianificazione.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pianificazione.Data
{
    public class PianificazioneAdapter : PianificazioneAdapterBase
    {
        public PianificazioneAdapter(System.Data.IDbConnection connection, IDbTransaction transaction) :
            base(connection, transaction)
        { }

        public void FillMAGAZZ(PianificazioneDS ds, List<string> IDMAGAZZ)
        {
            string inCOndition = ConvertToStringForInCondition(IDMAGAZZ);

            string select = @"SELECT * FROM GRUPPO.MAGAZZ WHERE IDMAGAZZ in ( {0} )";
            select = string.Format(select, inCOndition);

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.MAGAZZ);
            }
        }

        public void FillUSR_PRD_FASI_ROOT(PianificazioneDS ds, DateTime dataInizio, DateTime dataFine)
        {
            string dtInizio = dataInizio.ToString("dd/MM/yyyy");
            dtInizio += " 00:00:01";
            string dtFine = dataFine.ToString("dd/MM/yyyy");
            dtFine += " 23:59:59";

            string select = @"SELECT * FROM PIANIFICAZIONE_FASE WHERE ( DATAINIZIO <= to_date('{1}','DD/MM/YYYY HH24:MI:SS') AND DATAINIZIO >= to_date('{0}','DD/MM/YYYY HH24:MI:SS') ) OR ( DATAFINE <= to_date('{1}','DD/MM/YYYY HH24:MI:SS') AND DATAFINE >= to_date('{0}','DD/MM/YYYY HH24:MI:SS') )";
            select = string.Format(select, dtInizio, dtFine);
            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.PIANIFICAZIONE_FASE);
            }
        }

        public void FillUSR_PRD_FASI(PianificazioneDS ds, List<string> IDROOTPRDFASE)
        {
            string inCOndition = ConvertToStringForInCondition(IDROOTPRDFASE);

            string select = @"SELECT * FROM ditta1.USR_PRD_FASI WHERE IDROOTPRDFASE IN ( {0} )";
            select = string.Format(select, inCOndition);

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.USR_PRD_FASI);
            }

            select = @"SELECT * FROM ditta2.USR_PRD_FASI WHERE IDROOTPRDFASE IN ( {0} )";
            select = string.Format(select, inCOndition);

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.USR_PRD_FASI);
            }
        }

        public void FillUSR_PRD_MOVFASI(PianificazioneDS ds, List<string> IDPRDFASE)
        {
            string inCOndition = ConvertToStringForInCondition(IDPRDFASE);

            string select = @"SELECT * FROM ditta1.USR_PRD_MOVFASI WHERE IDPRDFASE IN ( {0} )";
            select = string.Format(select, inCOndition);

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.USR_PRD_MOVFASI);
            }

            select = @"SELECT * FROM ditta2.USR_PRD_MOVFASI WHERE IDPRDFASE IN ( {0} )";
            select = string.Format(select, inCOndition);

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.USR_PRD_MOVFASI);
            }
        }

        public void UpdateTable(string tablename, PianificazioneDS ds)
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

        public void FillUSR_PRD_LANCIOD(PianificazioneDS ds, string IDLANCIOD)
        {
            string select = @"SELECT * FROM ditta1.USR_PRD_LANCIOD WHERE IDLANCIOD = $P{IDLANCIOD}";

            ParamSet ps = new ParamSet();
            ps.AddParam("IDLANCIOD", DbType.String, IDLANCIOD);

            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.USR_PRD_LANCIOD);
            }

            select = @"SELECT * FROM ditta2.USR_PRD_LANCIOD WHERE IDLANCIOD = $P{IDLANCIOD}";

            ps = new ParamSet();
            ps.AddParam("IDLANCIOD", DbType.String, IDLANCIOD);

            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.USR_PRD_LANCIOD);
            }
        }

        public void FillUSR_PRD_MOVFASIAperti(PianificazioneDS ds)
        {
            string select = @"SELECT * FROM USR_PRD_MOVFASI WHERE QTADATER > 0";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.USR_PRD_MOVFASI);
            }
        }

        public void FillUSR_PRD_LANCIODAperti(PianificazioneDS ds)
        {
            string select = @"SELECT DISTINCT LA.* FROM USR_PRD_LANCIOD LA 
                            INNER JOIN USR_PRD_FASI FA ON FA.IDLANCIOD = LA.IDLANCIOD
                            INNER JOIN USR_PRD_MOVFASI MF ON MF.IDPRDFASE = FA.IDPRDFASE
                            WHERE MF.QTADATER > 0";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.USR_PRD_LANCIOD);
            }

        }

        public void FillUSR_PRD_FASIByIDLANCIOD(PianificazioneDS ds, string IDLANCIOD)
        {
            string select = @"SELECT * FROM USR_PRD_FASI WHERE IDLANCIOD = $P{IDLANCIOD}";

            ParamSet ps = new ParamSet();
            ps.AddParam("IDLANCIOD", DbType.String, IDLANCIOD);

            using (DbDataAdapter da = BuildDataAdapter(select,ps))
            {
                da.Fill(ds.USR_PRD_FASI);
            }
        }

        public void FillUSR_PRD_MOVFASIByIDLANCIOD(PianificazioneDS ds, string IDLANCIOD)
        {
            string select = @"SELECT MF.* FROM USR_PRD_MOVFASI MF 
                                INNER JOIN USR_PRD_FASI FA ON MF.IDPRDFASE = FA.IDPRDFASE
                                WHERE FA.IDLANCIOD = $P{IDLANCIOD}";

            ParamSet ps = new ParamSet();
            ps.AddParam("IDLANCIOD", DbType.String, IDLANCIOD);

            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.USR_PRD_MOVFASI);
            }
        }

        public void FillPIANIFICAZIONE_LANCIO(PianificazioneDS ds, string IDLANCIOD)
        {
            string select = @"SELECT * FROM PIANIFICAZIONE_LANCIO WHERE IDLANCIOD = $P{IDLANCIOD}";

            ParamSet ps = new ParamSet();
            ps.AddParam("IDLANCIOD", DbType.String, IDLANCIOD);

            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.PIANIFICAZIONE_LANCIO);
            }
        }

        public void FillPIANIFICAZIONE_LANCIOByIdLancio(PianificazioneDS ds, decimal IDLANCIO)
        {
            string select = @"SELECT * FROM PIANIFICAZIONE_LANCIO WHERE IDLANCIO = $P{IDLANCIO}";

            ParamSet ps = new ParamSet();
            ps.AddParam("IDLANCIO", DbType.Decimal, IDLANCIO);

            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.PIANIFICAZIONE_LANCIO);
            }
        }

        public void FillPIANIFICAZIONE_FASE(PianificazioneDS ds, string IDLANCIOD)
        {
            string select = @"SELECT FA.* FROM PIANIFICAZIONE_FASE FA 
                                INNER JOIN PIANIFICAZIONE_LANCIO LA ON LA.IDLANCIO = FA.IDLANCIO WHERE LA.IDLANCIOD = $P{IDLANCIOD}";

            ParamSet ps = new ParamSet();
            ps.AddParam("IDLANCIOD", DbType.String, IDLANCIOD);

            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.PIANIFICAZIONE_FASE);
            }
        }
        public long GetID()
        {
            string select = @" SELECT LANCIO_SEQUENCE.NEXTVAL FROM DUAL";
            using (IDbCommand da = BuildCommand(select))
            {
                long lnNextVal = Convert.ToInt64(da.ExecuteScalar());
                return lnNextVal;
            }
        }

        public void InsertPianificazioneLog(string Tipo, string Nota)
        {
            string insert = @"INSERT INTO PIANIFICAZIONE_LOG  ( IDLOG, DATA,TIPO,NOTA  ) VALUES (NULL,to_date('{0}','DD/MM/YYYY HH24:MI:SS'),$P<TIPO>,$P<NOTA>)";
            insert = string.Format(insert, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            ParamSet ps = new ParamSet();
            ps.AddParam("TIPO", DbType.String, Tipo);
            ps.AddParam("NOTA", DbType.String, Nota);

            using (DbCommand cmd = BuildCommand(insert, ps))
            {
                cmd.ExecuteNonQuery();
            }
        }

    }
}
