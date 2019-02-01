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

        public void FillPIANIFICAZIONE_FASEPerCommessa(PianificazioneDS ds, string commessa)
        {
            string select = @"SELECT FA.* FROM PIANIFICAZIONE_FASE FA
                                INNER JOIN PIANIFICAZIONE_LANCIO LA ON LA.IDLANCIO = FA.IDLANCIO
                                WHERE LA.NOMECOMMESSA LIKE $P<COMMESSA>";

            ParamSet ps = new ParamSet();
            commessa = string.Format("%{0}%", commessa);
            ps.AddParam("COMMESSA", DbType.String, commessa);

            using (DbDataAdapter da = BuildDataAdapter(select, ps))
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
            string select = @"SELECT * FROM USR_PRD_MOVFASI WHERE QTADATER > 0 AND IDTIPOMOVFASE <> 'ODM0000001'";

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

        public void FillPIAN_CATENA_COMMESSA(PianificazioneDS ds)
        {
            string select = @"SELECT * from PIAN_CATENA_COMMESSA";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.PIAN_CATENA_COMMESSA);
            }

        }

        public void FillUSR_PRD_FASIAperti(PianificazioneDS ds)
        {
            string select = @"SELECT DISTINCT TF.* FROM USR_PRD_FASI TF 
                            INNER JOIN USR_PRD_LANCIOD LA ON TF.IDLANCIOD = LA.IDLANCIOD
                            INNER JOIN USR_PRD_FASI FA ON FA.IDLANCIOD = LA.IDLANCIOD
                            INNER JOIN USR_PRD_MOVFASI MF ON MF.IDPRDFASE = FA.IDPRDFASE
                            WHERE MF.QTADATER > 0";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.USR_PRD_FASI);
            }

        }

        public void FillUSR_PRD_FASI_Sorelle(PianificazioneDS ds, string IDPRDFASE)
        {
            string select = @"select FA.* FROM usr_prd_fasi fa
                                inner join usr_prd_fasi ma on ma.idlanciod = fa.idlanciod
                                where ma.IDPRDFASE =  $P{IDPRDFASE} ";

            ParamSet ps = new ParamSet();
            ps.AddParam("IDPRDFASE", DbType.String, IDPRDFASE);

            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.USR_PRD_FASI);
            }
        }

        public void FillUSR_PRD_FASI_INFRAGRUPPO(PianificazioneDS ds, string IDPRDFASE_FROM)
        {
            string select = @"select * from usr_prd_fasi fa
                    inner join siglapp.usr_infra_fase_to_fase infra on infra.idprdfase_to = fa.idprdfase
                    where infra.idprdfase_from =  $P{IDPRDFASE_FROM} ";

            ParamSet ps = new ParamSet();
            ps.AddParam("IDPRDFASE_FROM", DbType.String, IDPRDFASE_FROM);

            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.USR_PRD_FASI);
            }
        }

        public void FillUSR_INFRA_FASE_TO_FASE(PianificazioneDS ds, DateTime dataLimiteRicerche)
        {
            string select = @"select * from siglapp.usr_infra_fase_to_fase where datavr >  $P{DATALIMITE} ";

            ParamSet ps = new ParamSet();
            ps.AddParam("DATALIMITE", DbType.DateTime, dataLimiteRicerche);

            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.USR_INFRA_FASE_TO_FASE);
            }
        }


        public void FillUSR_PRD_FASIByIDLANCIOD(PianificazioneDS ds, string IDLANCIOD)
        {
            string select = @"SELECT * FROM USR_PRD_FASI WHERE IDLANCIOD = $P{IDLANCIOD}";

            ParamSet ps = new ParamSet();
            ps.AddParam("IDLANCIOD", DbType.String, IDLANCIOD);

            using (DbDataAdapter da = BuildDataAdapter(select, ps))
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

        public void FillUSR_PRD_FASI_ConAccantonatoDaLavorare(PianificazioneDS ds)
        {
            string select = @"select distinct fa.* from usr_prd_fasi fa 
                                inner join usr_accto_con_doc doc on doc.iddestinazione = fa.idprdfase
                                inner join usr_accto_con con on con.idacctocon = doc.idacctocon
                                where idprdfasepadre is null and con.origine in (0,1,2)
                                ";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.USR_PRD_FASI);
            }
        }

        public void FillUSR_PRD_FASI_FaseFinaleCommessaDaIDORIGINE_Tipo_1(PianificazioneDS ds, string IDPRDMOVMATE)
        {
            string select = @"select fa1.* FROM  usr_prd_mate ma 
inner join usr_prd_fasi fa1 on fa1.idlanciod = ma.idlanciod
                                where fa1.idprdfasepadre is null 
                                and ma.idprdmate =  $P{IDPRDMOVMATE} ";

            ParamSet ps = new ParamSet();
            ps.AddParam("IDPRDMOVMATE", DbType.String, IDPRDMOVMATE);

            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.USR_PRD_FASI);
            }
        }

        public void FillUSR_PRD_FASI_FaseFinaleCommessaDaIDORIGINE_Tipo_2(PianificazioneDS ds, string IDPRDFLUSSOMOVMATE)
        {
            string select = @"select FA1.* FROM usr_prd_fasi FA 
                                INNER JOIN usr_prd_movmate MM ON MM.IDPRDFASE = FA.IDPRDFASE
                                INNER JOIN usr_prd_movFASI MF ON MF.IDPRDMOVFASE = MM.IDPRDMOVFASE
                                INNER JOIN usr_prd_flusso_movmate FMM ON FMM.idprdmovmate = MM.idprdmovmate
                                inner join usr_prd_fasi fa1 on fa1.idlanciod = fa.idlanciod
                                where FMM.idprdflussomovmate =  $P{IDPRDFLUSSOMOVMATE}
                                and fa1.idprdfasepadre is null";

            ParamSet ps = new ParamSet();
            ps.AddParam("IDPRDFLUSSOMOVMATE", DbType.String, IDPRDFLUSSOMOVMATE);

            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.USR_PRD_FASI);
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

        public long TruncateTable(string tabella)
        {
            string select = @" TRUNCATE TABLE " + tabella;
            using (IDbCommand da = BuildCommand(select))
            {
                long lnNextVal = Convert.ToInt64(da.ExecuteNonQuery());
                return lnNextVal;
            }
        }

        public List<string> GetDestinazioneOrdineCliente(string IDVENDITED)
        {
            List<string> valori = new List<string>();
            string select = @" SELECT 
                                    vd.data_conferma ||' ('||vt.fullnumdoc||'-'||vd.nrriga||')'
                                    FROM usr_venditeD VD 
                                    inner JOIN usr_venditeT VT ON VD.idvenditeT = VT.IDVENDITET
                                    WHERE vd.IDVENDITED =  $P{IDVENDITED} ORDER BY  vd.data_conferma  ";

            ParamSet ps = new ParamSet();
            ps.AddParam("IDVENDITED", DbType.String, IDVENDITED);
            using (IDbCommand da = BuildCommand(select, ps))
            {
                using (IDataReader reader = da.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string valore = reader.GetString(0);
                        if (valore != "(-)")
                            valori.Add(valore);
                    }
                }
            }

            return valori;
        }

        public List<string> GetDestinazioneMaterialeOrdineLavoro(string IDPRDFLUSSOMOVMATE)
        {
            List<string> valori = new List<string>();
            string select = @" select LA.NOMECOMMESSA ||'('||MF.nummovfase||')' FROM USR_PRD_LANCIOD LA
                                INNER JOIN usr_prd_fasi FA ON LA.IDLANCIOD = FA.IDLANCIOD
                                INNER JOIN usr_prd_movmate MM ON MM.IDPRDFASE = FA.IDPRDFASE
                                INNER JOIN usr_prd_movFASI MF ON MF.IDPRDMOVFASE = MM.IDPRDMOVFASE
                                INNER JOIN usr_prd_flusso_movmate FMM ON FMM.idprdmovmate = MM.idprdmovmate
                                where FMM.idprdflussomovmate =  $P{IDPRDFLUSSOMOVMATE}  ";

            ParamSet ps = new ParamSet();
            ps.AddParam("IDPRDFLUSSOMOVMATE", DbType.String, IDPRDFLUSSOMOVMATE);
            using (IDbCommand da = BuildCommand(select, ps))
            {
                using (IDataReader reader = da.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string valore = reader.GetString(0);
                        if (valore != "()")
                            valori.Add(valore);
                    }
                }
            }

            return valori;
        }

        public List<string> GetDestinazioneMaterialeDiCommessa(string IDPRDMOVMATE)
        {
            List<string> valori = new List<string>();
            string select = @" select LA.NOMECOMMESSA FROM USR_PRD_LANCIOD LA
                                inner join usr_prd_mate ma on ma.idlanciod = la.idlanciod
                                where ma.idprdmate =  $P{IDPRDMOVMATE}  ";

            ParamSet ps = new ParamSet();
            ps.AddParam("IDPRDMOVMATE", DbType.String, IDPRDMOVMATE);
            using (IDbCommand da = BuildCommand(select, ps))
            {
                using (IDataReader reader = da.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string valore = reader.GetString(0);
                        if (valore != "()")
                            valori.Add(valore);
                    }
                }
            }

            return valori;

        }
        public void FillUSR_PRD_FASIDaIDPRDMATE(PianificazioneDS ds, string IDPRDMATE)
        {
            string select = @" select * FROM usr_prd_fasi fa
                                inner join usr_prd_mate ma on ma.idprdfase = fa.idprdfase
                                where ma.idprdmate =  $P{IDPRDMATE}  ";

            ParamSet ps = new ParamSet();
            ps.AddParam("IDPRDMATE", DbType.String, IDPRDMATE);

            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.USR_PRD_FASI);
            }
        }

        public void FillUSR_ACCTO_CON(PianificazioneDS ds, string IDPRDFASE)
        {

            string select = @" select * from usr_accto_con con 
                            inner join usr_accto_con_doc doc on doc.idacctocon = con.idacctocon
                            where doc.destinazione=1
                            and doc.iddestinazione =  $P{IDPRDFASE} ";

            ParamSet ps = new ParamSet();
            ps.AddParam("IDPRDFASE", DbType.String, IDPRDFASE);

            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.USR_ACCTO_CON);
            }
        }

        public void InsertPianificazioneLog(string Tipo, string Nota, string Applicazione)
        {
            string insert = @"INSERT INTO PIANIFICAZIONE_LOG  ( IDLOG, DATA,TIPO,NOTA,APPLICAZIONE  ) VALUES (NULL,to_date('{0}','DD/MM/YYYY HH24:MI:SS'),$P<TIPO>,$P<NOTA>,$P<APPLICAZIONE>)";
            insert = string.Format(insert, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            ParamSet ps = new ParamSet();
            ps.AddParam("TIPO", DbType.String, Tipo);
            ps.AddParam("NOTA", DbType.String, Nota);
            ps.AddParam("APPLICAZIONE", DbType.String, Applicazione);

            using (DbCommand cmd = BuildCommand(insert, ps))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void CopiaPianificazioneSuRuntime()
        {
            string insert = @"insert into PIANIFICAZIONE_RUNTIME select * from pianificazione_odl";

            using (DbCommand cmd = BuildCommand(insert))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void ImpostaFaseAnnullataPerQuantita()
        {
            string update = @"UPDATE PIANIFICAZIONE_FASE SET STATO = 'ANNULLATO' where  QTA = QTAANN ";

            using (DbCommand cmd = BuildCommand(update))
            {
                cmd.ExecuteNonQuery();
            }
        }

    }
}
