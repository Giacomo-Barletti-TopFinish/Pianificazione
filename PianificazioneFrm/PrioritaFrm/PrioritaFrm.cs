using Priorita.Data;
using Priorita.Entities;
using PrioritaFrm.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrioritaFrm
{
    internal enum Colonne
    {
        IDLANCIOD,
        IDPRDFASE,
        IDPRDMOVFASE,
        IDMAGAZZ,
        AZIENDA,
        SEGNALATORE,
        COMMESSA,
        DATACOMMESSA,
        ODL,
        DATAODL,
        BARCODE,
        REPARTO,
        FASE,
        ARTICOLO,
        BIL,
        QUANTITA,
        QUANTITADATERMINARE
    };
    public partial class PrioritaFrm : Form
    {
        private PrioritaDS _dsAnagrafica = new PrioritaDS();
        private PrioritaDS _dsPriorita = new PrioritaDS();

        private DataSet _dsGriglieODL;
        private string _nomeTabellaGrigliaODL = "GrigliaODL";
        private string _nomeTabellaGrigliaTermini = "GrigliaTermini";

        private bool _disabilitaCancellazione = false;
        //        private string _nomeTabellaGrigliaScadenze = "GrigliaScadenze";
        public PrioritaFrm()
        {
            InitializeComponent();
        }

        private void VerificaRispettoScadenza()
        {
            PrioritaDS ds = new PrioritaDS();
            using (PrioritaBusiness bPriorita = new PrioritaBusiness())
            {
                bPriorita.FillRW_SCADENZE(ds, DateTime.Today.AddDays(-8), DateTime.Today);
                bPriorita.FillUSR_PRD_FLUSSO_MOVFASI_By_RW_SCADENZE(ds, DateTime.Today.AddDays(-8), DateTime.Today);

                List<string> IDPRDMOVFASI = ds.RW_SCADENZE.Select(x => x.IDPRDMOVFASE).Distinct().ToList();

                foreach (string IDPRDMOVFASE in IDPRDMOVFASI)
                {
                    List<PrioritaDS.RW_SCADENZERow> scadenze = ds.RW_SCADENZE.Where(x => x.IDPRDMOVFASE == IDPRDMOVFASE && x.DATA <= DateTime.Today).OrderBy(x => x.DATA).ToList();
                    List<PrioritaDS.USR_PRD_FLUSSO_MOVFASIRow> termini = ds.USR_PRD_FLUSSO_MOVFASI.Where(x => x.IDPRDMOVFASE == IDPRDMOVFASE && x.IDPRDCAUFASE == "0000000008").OrderBy(x => x.DATAFLUSSOMOVFASE).ToList();

                    foreach (PrioritaDS.RW_SCADENZERow scadenza in scadenze)
                    {
                        decimal quantitainScadenza = scadenze.Where(x => x.DATA <= scadenza.DATA).Sum(x => x.QTA);
                        decimal lavoroEseguito = termini.Where(x => x.DATAFLUSSOMOVFASE <= scadenza.DATA).Sum(x => x.QTAFLUSSO);

                        if (lavoroEseguito < quantitainScadenza)
                            scadenza.SCADUTO = 1;
                        else
                            scadenza.SCADUTO = 0;
                    }

                }

            }
        }

        private void PrioritaFrm_Load(object sender, EventArgs e)
        {

            // TEST

            VerificaRispettoScadenza();

            // FINE TEST



            lblMessage.Text = string.Empty;
            statusBarLabel.Text = string.Empty;
            dtScadenza.MinDate = DateTime.Today;
            try
            {
                CaricaAnagrafica();
                CaricaDDL();
            }
            catch (Exception ex)
            {
                ExceptionFrm form = new ExceptionFrm(ex);
                form.ShowDialog();
            }

            _dsGriglieODL = new DataSet();
            CreaDSGrigliaODL();
            CreaDSGrigliaTermini();
            CreaGrigliaODL();
            CreaGrigliaTermini();
            CreaGrigliaScadenze();
        }

        private void CaricaAnagrafica()
        {
            using (PrioritaBusiness bPriorita = new PrioritaBusiness())
            {
                bPriorita.FillCLIFO(_dsAnagrafica);
                bPriorita.FillREPARTI(_dsAnagrafica);
                bPriorita.FillSEGNALATORI(_dsAnagrafica);
                bPriorita.FillTABFAS(_dsAnagrafica);
            }
        }

        private void CaricaDDL()
        {
            ddlSegnalatore.Items.Add(string.Empty);
            ddlSegnalatore.Items.AddRange(_dsAnagrafica.SEGNALATORI.Select(x => x.RAGIONESOC).ToArray());

            ddlReparto.Items.Add(string.Empty);
            //            ddlReparto.Items.AddRange(_dsAnagrafica.REPARTI.Select(x => x.RAGIONESOC).ToArray());
            ddlReparto.Items.AddRange(_dsAnagrafica.CLIFO.Where(x => !x.IsRAGIONESOCNull() && x.TIPO == "F").Select(x => x.RAGIONESOC).ToArray());

            ddlFase.Items.Add(string.Empty);
            ddlFase.Items.AddRange(_dsAnagrafica.TABFAS.Where(x => !x.IsCODICECLIFOPREDFASENull()).OrderBy(x => x.DESTABFAS).Select(x => x.DESTABFAS).ToArray());

        }

        private void btnTrova_Click(object sender, EventArgs e)
        {
            if (bgwBIL.IsBusy)
            {
                MessageBox.Show("Attività di recupero BIL in corso, impossibile interrompere adesso. Aspetta che l'attività sia stata annullata e riprova", "ATTENZIONE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CancellaBackgroundWorker();
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            statusBarLabel.Text = "Ricerca in corso...";
            try
            {
                _dsGriglieODL.Tables[_nomeTabellaGrigliaODL].Clear();
                string codiceSegnalatore = string.Empty;
                string idTabFas = string.Empty;
                string codiceReparto = GetCodiceReparto();
                if (ddlSegnalatore.SelectedIndex != -1)
                {
                    codiceSegnalatore = ddlSegnalatore.SelectedItem as string;
                    if (_dsAnagrafica.SEGNALATORI.Any(x => x.RAGIONESOC == codiceSegnalatore))
                        codiceSegnalatore = _dsAnagrafica.SEGNALATORI.Where(x => x.RAGIONESOC == codiceSegnalatore).Select(x => x.CODICE).FirstOrDefault();
                }

                if (ddlFase.SelectedIndex != -1)
                {
                    idTabFas = ddlFase.SelectedItem as string;
                    if (_dsAnagrafica.TABFAS.Any(x => x.DESTABFAS == idTabFas))
                        idTabFas = _dsAnagrafica.TABFAS.Where(x => x.DESTABFAS == idTabFas).Select(x => x.IDTABFAS).FirstOrDefault();
                }

                _dsPriorita = new PrioritaDS();
                using (PrioritaBusiness bPriorita = new PrioritaBusiness())
                {
                    bPriorita.FillUSR_PRD_MOVFASI_Aperti(_dsPriorita, codiceSegnalatore, codiceReparto, idTabFas);
                    bPriorita.FillUSR_PRD_MOVFASI_Chiusi(_dsPriorita, codiceSegnalatore, codiceReparto, idTabFas, 7);

                    List<string> articoli = _dsPriorita.USR_PRD_MOVFASI.Select(x => x.IDMAGAZZ).Distinct().ToList();
                    bPriorita.FillMAGAZZ(_dsAnagrafica, articoli);

                    List<string> IDPRDFASE = _dsPriorita.USR_PRD_MOVFASI.Select(x => x.IDPRDFASE).Distinct().ToList();
                    bPriorita.FillUSR_PRD_FASI(_dsPriorita, IDPRDFASE);

                    List<string> IDLANCIOD = _dsPriorita.USR_PRD_FASI.Select(x => x.IDLANCIOD).Distinct().ToList();
                    bPriorita.FillUSR_PRD_LANCIOD(_dsPriorita, IDLANCIOD);

                    List<string> IDPRDMOVFASE = _dsPriorita.USR_PRD_MOVFASI.Select(x => x.IDPRDMOVFASE).Distinct().ToList();
                    bPriorita.FillRW_SCADENZE(_dsPriorita, IDPRDMOVFASE);
                    //IDPRDMOVFASE = _dsPriorita.USR_PRD_MOVFASI.Select(x => x.IDPRDMOVFASE).Distinct().ToList();
                    //bPriorita.FillUSR_VENDITET(_dsPriorita, IDPRDMOVFASE);

                    PopolaDSGrigliaODL();
                    bgwBIL.RunWorkerAsync();
                    statusBarLabel.Text = "Recupero BIL in corso...";
                }

            }
            catch (Exception ex)
            {
                ExceptionFrm form = new ExceptionFrm(ex);
                form.ShowDialog();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void CreaDSGrigliaODL()
        {

            DataTable dtGriglia = _dsGriglieODL.Tables.Add();
            dtGriglia.TableName = _nomeTabellaGrigliaODL;
            dtGriglia.Columns.Add("IDLANCIOD", Type.GetType("System.String")).ReadOnly = true;
            dtGriglia.Columns.Add("IDPRDFASE", Type.GetType("System.String")).ReadOnly = true;
            dtGriglia.Columns.Add("IDPRDMOVFASE", Type.GetType("System.String")).ReadOnly = true;
            dtGriglia.Columns.Add("IDMAGAZZ", Type.GetType("System.String")).ReadOnly = true;
            dtGriglia.Columns.Add("Azienda", Type.GetType("System.String")).ReadOnly = true;
            dtGriglia.Columns.Add("Segnalatore", Type.GetType("System.String")).ReadOnly = true;
            dtGriglia.Columns.Add("Commessa", Type.GetType("System.String")).ReadOnly = true;
            dtGriglia.Columns.Add("Data commessa", Type.GetType("System.String")).ReadOnly = true;
            dtGriglia.Columns.Add("ODL", Type.GetType("System.String")).ReadOnly = true;
            dtGriglia.Columns.Add("Data ODL", Type.GetType("System.String")).ReadOnly = true;
            dtGriglia.Columns.Add("Barcode", Type.GetType("System.String")).ReadOnly = true;
            dtGriglia.Columns.Add("Reparto", Type.GetType("System.String")).ReadOnly = true;
            dtGriglia.Columns.Add("Fase", Type.GetType("System.String")).ReadOnly = true;
            dtGriglia.Columns.Add("Articolo", Type.GetType("System.String")).ReadOnly = true;
            dtGriglia.Columns.Add("BIL", Type.GetType("System.String")).ReadOnly = false;
            dtGriglia.Columns.Add("Quantità", Type.GetType("System.Decimal")).ReadOnly = true;
            dtGriglia.Columns.Add("Da terminare", Type.GetType("System.Decimal")).ReadOnly = true;

        }

        private void PopolaDSGrigliaODL()
        {

            foreach (PrioritaDS.USR_PRD_MOVFASIRow odl in _dsPriorita.USR_PRD_MOVFASI)
            {
                DataTable dtGriglia = _dsGriglieODL.Tables[_nomeTabellaGrigliaODL];

                DataRow riga = dtGriglia.NewRow();

                PrioritaDS.USR_PRD_FASIRow fase = _dsPriorita.USR_PRD_FASI.Where(x => x.IDPRDFASE == odl.IDPRDFASE).FirstOrDefault();
                if (fase == null) new Exception(String.Format("ODL senza fase. IDPRDMOVFASE: {0} IDPRDFASE: {1}", odl.IDPRDMOVFASE, odl.IDPRDFASE));

                PrioritaDS.USR_PRD_LANCIODRow lanciod = _dsPriorita.USR_PRD_LANCIOD.Where(x => x.IDLANCIOD == fase.IDLANCIOD).FirstOrDefault();
                if (lanciod == null) new Exception(String.Format("ODL senza lancio. IDPRDMOVFASE: {0} IDPRDFASE: {1} IDLANCIOD: {2}", odl.IDPRDMOVFASE, odl.IDPRDFASE, fase.IDLANCIOD));

                PrioritaDS.MAGAZZRow articolo = _dsAnagrafica.MAGAZZ.Where(x => x.IDMAGAZZ == odl.IDMAGAZZ).FirstOrDefault();


                riga[(int)Colonne.IDLANCIOD] = lanciod.IsIDLANCIODNull() ? string.Empty : lanciod.IDLANCIOD;
                riga[(int)Colonne.IDPRDFASE] = fase.IsIDPRDFASENull() ? string.Empty : fase.IDPRDFASE;
                riga[(int)Colonne.IDPRDMOVFASE] = odl.IsIDPRDMOVFASENull() ? string.Empty : odl.IDPRDMOVFASE;
                riga[(int)Colonne.IDMAGAZZ] = odl.IsIDMAGAZZNull() ? string.Empty : odl.IDMAGAZZ;

                riga[(int)Colonne.AZIENDA] = odl.AZIENDA;
                if (!lanciod.IsSEGNALATORENull())
                {
                    PrioritaDS.SEGNALATORIRow segnalatore = _dsAnagrafica.SEGNALATORI.Where(x => x.CODICE == lanciod.SEGNALATORE).FirstOrDefault();
                    if (segnalatore != null)
                        riga[(int)Colonne.SEGNALATORE] = segnalatore.IsRAGIONESOCNull() ? string.Empty : segnalatore.RAGIONESOC;
                }

                riga[(int)Colonne.COMMESSA] = lanciod.IsNOMECOMMESSANull() ? string.Empty : lanciod.NOMECOMMESSA;
                riga[(int)Colonne.DATACOMMESSA] = lanciod.IsDATACOMMESSANull() ? string.Empty : lanciod.DATACOMMESSA.ToShortDateString();
                riga[(int)Colonne.ODL] = odl.IsNUMMOVFASENull() ? string.Empty : odl.NUMMOVFASE;
                riga[(int)Colonne.DATAODL] = odl.IsDATAMOVFASENull() ? string.Empty : odl.DATAMOVFASE.ToShortDateString();
                riga[(int)Colonne.BARCODE] = odl.IsBARCODENull() ? string.Empty : odl.BARCODE;

                if (!odl.IsCODICECLIFONull())
                {
                    PrioritaDS.CLIFORow clifo = _dsAnagrafica.CLIFO.Where(x => x.CODICE == odl.CODICECLIFO).FirstOrDefault();
                    if (clifo != null)
                    {
                        riga[(int)Colonne.REPARTO] = clifo.IsRAGIONESOCNull() ? string.Empty : clifo.RAGIONESOC;
                    }
                }

                if (!odl.IsCODICECLIFONull())
                {
                    PrioritaDS.TABFASRow tabfas = _dsAnagrafica.TABFAS.Where(x => x.IDTABFAS == odl.IDTABFAS).FirstOrDefault();
                    if (tabfas != null)
                    {
                        riga[(int)Colonne.FASE] = tabfas.IsDESTABFASPPRENull() ? string.Empty : tabfas.DESTABFAS;
                    }
                }

                if (articolo != null)
                    riga[(int)Colonne.ARTICOLO] = articolo.MODELLO;

                //StringBuilder BIL = new StringBuilder();
                //List<PrioritaDS.USR_VENDITETRow> vendite = _dsPriorita.USR_VENDITET.Where(x => x.IDPRDMOVFASE == odl.IDPRDMOVFASE).ToList();
                //if (vendite.Count > 0)
                //{
                //    foreach (PrioritaDS.USR_VENDITETRow vendita in vendite)
                //    {
                //        string str = string.Format("{0} del {1}, ", vendita.NUMDOC, vendita.DATDOC.ToShortDateString());
                //        BIL.Append(str);
                //    }
                //}

                //riga[(int)Colonne.BIL] = BIL.Length == 0 ? string.Empty : BIL.ToString().Substring(0, BIL.Length - 2);
                riga[(int)Colonne.QUANTITA] = odl.QTA;
                riga[(int)Colonne.QUANTITADATERMINARE] = odl.IsQTADATERNull() ? 0 : odl.QTADATER;

                dtGriglia.Rows.Add(riga);
            }

        }

        private void CreaGrigliaODL()
        {
            dgvODL.Columns.Clear();
            dgvODL.DataSource = _dsGriglieODL;
            dgvODL.DataMember = _nomeTabellaGrigliaODL;

            dgvODL.Columns[(int)Colonne.IDLANCIOD].Visible = false;
            dgvODL.Columns[(int)Colonne.IDPRDFASE].Visible = false;
            dgvODL.Columns[(int)Colonne.IDPRDMOVFASE].Visible = false;
            dgvODL.Columns[(int)Colonne.IDMAGAZZ].Visible = false;
            dgvODL.Columns[(int)Colonne.AZIENDA].Width = 70;
            dgvODL.Columns[(int)Colonne.SEGNALATORE].Width = 170;
            dgvODL.Columns[(int)Colonne.COMMESSA].Width = 160;
            dgvODL.Columns[(int)Colonne.DATACOMMESSA].Width = 90;
            dgvODL.Columns[(int)Colonne.ODL].Width = 130;
            dgvODL.Columns[(int)Colonne.DATAODL].Width = 90;
            dgvODL.Columns[(int)Colonne.BARCODE].Width = 120;
            dgvODL.Columns[(int)Colonne.REPARTO].Width = 150;
            dgvODL.Columns[(int)Colonne.FASE].Width = 200;
            dgvODL.Columns[(int)Colonne.ARTICOLO].Width = 260;
            dgvODL.Columns[(int)Colonne.BIL].Width = 150;
            dgvODL.Columns[(int)Colonne.QUANTITA].Width = 80;
            dgvODL.Columns[(int)Colonne.QUANTITADATERMINARE].Width = 100;

        }

        private void dgvODL_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            SuspendLayout();
            if (dgvODL.Rows[e.RowIndex].Cells[(int)Colonne.QUANTITADATERMINARE].Value == null) return;

            decimal qtaDaTer = (decimal)dgvODL.Rows[e.RowIndex].Cells[(int)Colonne.QUANTITADATERMINARE].Value;
            string IDPRDMOVFASE = (string)dgvODL.Rows[e.RowIndex].Cells[(int)Colonne.IDPRDMOVFASE].Value;

            if (qtaDaTer == 0)
            {
                System.Windows.Forms.DataGridViewCellStyle boldStyle = new System.Windows.Forms.DataGridViewCellStyle();
                boldStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
                dgvODL.Rows[e.RowIndex].DefaultCellStyle = boldStyle;
            }
            else
            {
                List<PrioritaDS.RW_SCADENZERow> scadenze = _dsPriorita.RW_SCADENZE.Where(x => x.IDPRDMOVFASE == IDPRDMOVFASE).OrderBy(x => x.DATA).ToList();
                if (scadenze.Count > 0)
                {
                    dgvODL.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                }
            }
            ResumeLayout();
        }

        private void ResetCampiScadenza()
        {
            nmQuantita.Value = 0;
            dtScadenza.Value = DateTime.Today;
        }

        private void dgvODL_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            _disabilitaCancellazione = true;
            ResetCampiScadenza();
            if (e.RowIndex == -1) return;
            try
            {
                string IDLANCIOD = dgvODL.Rows[e.RowIndex].Cells[(int)Colonne.IDLANCIOD].Value as string;
                string IDPRDFASE = dgvODL.Rows[e.RowIndex].Cells[(int)Colonne.IDPRDFASE].Value as string;
                string IDPRDMOVFASE = dgvODL.Rows[e.RowIndex].Cells[(int)Colonne.IDPRDMOVFASE].Value as string;
                string IDMAGAZZ = dgvODL.Rows[e.RowIndex].Cells[(int)Colonne.IDMAGAZZ].Value as string;

                using (PrioritaBusiness bPriorita = new PrioritaBusiness())
                {
                    _dsPriorita.USR_PRD_FLUSSO_MOVFASI.Clear();

                    bPriorita.FillUSR_PRD_FLUSSO_MOVFASI(_dsPriorita, IDPRDMOVFASE);

                    PopolaDSGrigliaTermini();
                    dgvTermini.DataSource = _dsGriglieODL;
                    dgvTermini.DataMember = _nomeTabellaGrigliaTermini;


                    AssociaScadenze(IDPRDMOVFASE);
                }
            }
            catch (Exception ex)
            {
                ExceptionFrm form = new ExceptionFrm(ex);
                form.ShowDialog();
            }
            finally
            {
                _disabilitaCancellazione = false;
            }
        }

        private void AssociaScadenze(string IDPRDMOVFASE)
        {
            DataView custDV = new DataView(_dsPriorita.Tables["RW_SCADENZE"]);
            custDV.RowFilter = "IDPRDMOVFASE = '" + IDPRDMOVFASE + "'";

            dgvScadenze.DataSource = custDV;
        }

        private void CreaDSGrigliaTermini()
        {
            DataTable dtGriglia = _dsGriglieODL.Tables.Add();
            dtGriglia.TableName = _nomeTabellaGrigliaTermini;
            dtGriglia.Columns.Add("IDPRDFLUSSOMOVFASE", Type.GetType("System.String")).ReadOnly = true;
            dtGriglia.Columns.Add("IDPRDMOVFASE", Type.GetType("System.String")).ReadOnly = true;
            dtGriglia.Columns.Add("Data", Type.GetType("System.String")).ReadOnly = true;
            dtGriglia.Columns.Add("Qta flusso", Type.GetType("System.String")).ReadOnly = true;
            dtGriglia.Columns.Add("Qta flusso succ", Type.GetType("System.String")).ReadOnly = true;
            dtGriglia.Columns.Add("Causale", Type.GetType("System.String")).ReadOnly = true;
            dtGriglia.Columns.Add("Barcode", Type.GetType("System.String")).ReadOnly = true;

        }

        private void PopolaDSGrigliaTermini()
        {
            DataTable dtGriglia = _dsGriglieODL.Tables[_nomeTabellaGrigliaTermini];
            dtGriglia.Clear();
            foreach (PrioritaDS.USR_PRD_FLUSSO_MOVFASIRow termine in _dsPriorita.USR_PRD_FLUSSO_MOVFASI)
            {
                DataRow riga = dtGriglia.NewRow();

                riga[0] = termine.IsIDFLUSSOMOVFASENull() ? string.Empty : termine.IDFLUSSOMOVFASE;
                riga[1] = termine.IsIDPRDMOVFASENull() ? string.Empty : termine.IDPRDMOVFASE;
                riga[2] = termine.IsDATAFLUSSOMOVFASENull() ? string.Empty : termine.DATAFLUSSOMOVFASE.ToShortDateString();
                riga[3] = termine.IsQTAFLUSSONull() ? string.Empty : termine.QTAFLUSSO.ToString();
                riga[4] = termine.IsQTAFLUSSISUCCNull() ? string.Empty : termine.QTAFLUSSISUCC.ToString();
                riga[5] = termine.IsCAUSALENull() ? string.Empty : termine.CAUSALE;
                riga[6] = termine.IsBARCODENull() ? string.Empty : termine.BARCODE;

                dtGriglia.Rows.Add(riga);
            }

        }

        private void CreaGrigliaTermini()
        {
            dgvTermini.DataSource = _dsGriglieODL;
            dgvTermini.DataMember = _nomeTabellaGrigliaTermini;

            dgvTermini.Columns[0].Visible = false;
            dgvTermini.Columns[1].Visible = false;
            dgvTermini.Columns[2].Width = 80;
            dgvTermini.Columns[3].Width = 80;
            dgvTermini.Columns[4].Width = 80;
            dgvTermini.Columns[5].Width = 380;
            dgvTermini.Columns[6].Width = 120;
        }

        private void CreaGrigliaScadenze()
        {

            dgvScadenze.DataSource = _dsPriorita;
            dgvScadenze.DataMember = _dsPriorita.RW_SCADENZE.TableName;

            dgvScadenze.Columns[0].Visible = false;
            dgvScadenze.Columns[1].Visible = false;
            dgvScadenze.Columns[2].Width = 100;
            dgvScadenze.Columns[3].Width = 100;
        }

        private void btnInserisciScadenza_Click(object sender, EventArgs e)
        {
            if (dgvODL.SelectedCells.Count == 0)
            {
                MessageBox.Show("Selezionare un ODL", "ATTENZIONE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (nmQuantita.Value == 0)
            {
                MessageBox.Show("La quantità non può essere 0", "ATTENZIONE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int rowIndex = dgvODL.SelectedCells[0].RowIndex;

            decimal quantitaDaTerminare = (decimal)dgvODL.Rows[rowIndex].Cells[(int)Colonne.QUANTITADATERMINARE].Value;
            if (quantitaDaTerminare == 0)
            {
                MessageBox.Show("La quantità da terminare dell'ODL è 0. Non ci sono articoli da pianificare.", "ATTENZIONE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string IDPRDMOVFASE = dgvODL.Rows[rowIndex].Cells[(int)Colonne.IDPRDMOVFASE].Value as string;
            decimal qtaODL = (decimal)dgvODL.Rows[rowIndex].Cells[(int)Colonne.QUANTITA].Value;
            using (PrioritaBusiness bPriorita = new PrioritaBusiness())
            {
                if (VerificaCreazioneNuovaScadenza(qtaODL, IDPRDMOVFASE))
                {
                    dgvScadenze.DataSource = null;
                    PrioritaDS.RW_SCADENZERow nuovaScadenza = _dsPriorita.RW_SCADENZE.NewRW_SCADENZERow();
                    nuovaScadenza.IDSCADENZA = bPriorita.GetID();
                    nuovaScadenza.IDPRDMOVFASE = IDPRDMOVFASE;
                    nuovaScadenza.DATA = EstraiSoloData(dtScadenza.Value);
                    nuovaScadenza.QTA = nmQuantita.Value;
                    _dsPriorita.RW_SCADENZE.AddRW_SCADENZERow(nuovaScadenza);

                }
                bPriorita.UpdateRW_SCADENZE(_dsPriorita);
                _dsPriorita.AcceptChanges();

                AssociaScadenze(IDPRDMOVFASE);
            }
            AggiornaColoreRiga(rowIndex, dgvScadenze.Rows.Count > 0);

        }

        private DateTime EstraiSoloData(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
        }

        private bool VerificaCreazioneNuovaScadenza(decimal qtaODL, string IDPRDMOVFASE)
        {
            decimal totaleInScadenza = _dsPriorita.RW_SCADENZE.Where(x => x.IDPRDMOVFASE == IDPRDMOVFASE).Sum(x => x.QTA);
            if (qtaODL < totaleInScadenza + nmQuantita.Value)
            {
                string messaggio = string.Format("La quantita indicata ({0}) sommata alle quanità già in scadenza ({1}) è superiore alla quantità dell'ODL ({2})", nmQuantita.Value, totaleInScadenza, qtaODL);
                MessageBox.Show(messaggio, "ATTENZIONE", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return false;
            }
            PrioritaDS.RW_SCADENZERow scadenzaEsistente = _dsPriorita.RW_SCADENZE.Where(x => x.DATA == EstraiSoloData(dtScadenza.Value) && x.IDPRDMOVFASE == IDPRDMOVFASE).FirstOrDefault();
            if (scadenzaEsistente != null)
            {
                MessageBox.Show("Esiste già una scadenza con questa data. La quantità indicata sarà sommata a quella già in tabella", "ATTENZIONE", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                scadenzaEsistente.QTA += nmQuantita.Value;
                return false;
            }
            return true;
        }


        private void dgvScadenze_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (_disabilitaCancellazione) return;
            int rowIndex = dgvODL.SelectedCells[0].RowIndex;
            string IDPRDMOVFASE = dgvODL.Rows[rowIndex].Cells[(int)Colonne.IDPRDMOVFASE].Value as string;

            using (PrioritaBusiness bPriorita = new PrioritaBusiness())
            {
                bPriorita.UpdateRW_SCADENZE(_dsPriorita);
            }
            int numeroRighe = _dsPriorita.RW_SCADENZE.Where(x => x.IDPRDMOVFASE == IDPRDMOVFASE).Count();
            AggiornaColoreRiga(rowIndex, numeroRighe > 0);
        }

        private void AggiornaColoreRiga(int indiceRiga, bool haScadenze)
        {
            if (haScadenze)
                dgvODL.Rows[indiceRiga].DefaultCellStyle.ForeColor = Color.Red;
            else
                dgvODL.Rows[indiceRiga].DefaultCellStyle.ForeColor = Color.Black;

            dgvODL.Refresh();
        }

        private void dgvScadenze_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void btnEsporta_Click(object sender, EventArgs e)
        {
            FileStream fs = null;
            if (_dsPriorita == null || _dsPriorita.RW_SCADENZE.Rows.Count == 0)
            {
                MessageBox.Show("Non ci sono dati esportare", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog d = new SaveFileDialog();
            d.Filter = "Excel Files (*.xlsx)|*.xlsx";
            d.DefaultExt = "xlsx";
            d.AddExtension = true;
            if (d.ShowDialog() == DialogResult.Cancel) return;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ExcelHelper hExcel = new ExcelHelper();
                byte[] fileExcel = hExcel.CreaExcelScadenze(_dsPriorita, _dsAnagrafica);

                if (File.Exists(d.FileName)) File.Delete(d.FileName);

                fs = new FileStream(d.FileName, FileMode.Create);
                fs.Write(fileExcel, 0, fileExcel.Length);
                fs.Flush();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERRORE", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (fs != null) fs.Close();
                MessageBox.Show("Export to excel terminato con successo", "Informazione", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Cursor.Current = Cursors.Default;
            }
        }

        private string GetCodiceReparto()
        {
            if (ddlReparto.SelectedIndex != -1)
            {
                string codiceReparto = ddlReparto.SelectedItem as string;
                if (_dsAnagrafica.CLIFO.Any(x => !x.IsRAGIONESOCNull() && x.RAGIONESOC == codiceReparto))
                {
                    return _dsAnagrafica.CLIFO.Where(x => !x.IsRAGIONESOCNull() && x.RAGIONESOC == codiceReparto && x.TIPO == "F").Select(x => x.CODICE).FirstOrDefault();
                }
            }
            return string.Empty;
        }

        private void bgwBIL_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            try
            {
                List<string> IDPRDMOVFASES = _dsPriorita.USR_PRD_MOVFASI.Select(x => x.IDPRDMOVFASE).Distinct().ToList();
                using (PrioritaBusiness bPriorita = new PrioritaBusiness())
                {
                    bPriorita.FillUSR_VENDITET(_dsPriorita, IDPRDMOVFASES);
                }
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    return;
                }
                DataTable dtGriglia = _dsGriglieODL.Tables[_nomeTabellaGrigliaODL];

                foreach (DataRow riga in dtGriglia.Rows)
                {
                    string IDPRDMOVFASE = riga[(int)Colonne.IDPRDMOVFASE].ToString();

                    StringBuilder BIL = new StringBuilder();
                    List<PrioritaDS.USR_VENDITETRow> vendite = _dsPriorita.USR_VENDITET.Where(x => x.IDPRDMOVFASE == IDPRDMOVFASE).ToList();
                    if (worker.CancellationPending == true)
                    {
                        e.Cancel = true;
                        break;
                    }

                    if (vendite.Count > 0)
                    {
                        foreach (PrioritaDS.USR_VENDITETRow vendita in vendite)
                        {
                            string str = string.Format("{0} del {1}, ", vendita.NUMDOC, vendita.DATDOC.ToShortDateString());
                            BIL.Append(str);
                        }
                    }

                    riga[(int)Colonne.BIL] = BIL.Length == 0 ? string.Empty : BIL.ToString().Substring(0, BIL.Length - 2);

                }

            }
            catch (Exception ex)
            {
                ExceptionFrm form = new ExceptionFrm(ex);
                form.ShowDialog();
            }

        }

        private void bgwBIL_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                statusBarLabel.Text = "Recupero BIL annullato";
                return;
            }

            dgvODL.Refresh();
            dgvODL.Parent.Refresh();
            statusBarLabel.Text = "Recupero BIL completato";
        }

        private void CancellaBackgroundWorker()
        {
            if (bgwBIL.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                bgwBIL.CancelAsync();
            }
            statusBarLabel.Text = "Recupero BIL in fase di annullamento....";

        }

        private void bgwBIL_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            dgvODL.Refresh();
            dgvODL.Parent.Refresh();
        }
    }
}
