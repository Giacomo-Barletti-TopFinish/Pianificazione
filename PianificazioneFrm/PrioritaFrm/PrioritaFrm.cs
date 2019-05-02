using Priorita.Data;
using Priorita.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        SEGNALATORE,
        COMMESSA,
        DATACOMMESSA,
        ODL,
        DATAODL,
        BARCODE,
        REPARTO,
        ARTICOLO,
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
        private string _nomeTabellaGrigliaScadenze = "GrigliaScadenze";
        public PrioritaFrm()
        {
            InitializeComponent();
        }

        private void PrioritaFrm_Load(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;
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
            CreaDSGrigliaScadenze();
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
            }
        }

        private void CaricaDDL()
        {
            ddlSegnalatore.Items.Add(string.Empty);
            ddlSegnalatore.Items.AddRange(_dsAnagrafica.SEGNALATORI.Select(x => x.RAGIONESOC).ToArray());

            ddlReparto.Items.Add(string.Empty);
            ddlReparto.Items.AddRange(_dsAnagrafica.REPARTI.Select(x => x.RAGIONESOC).ToArray());

        }

        private void btnTrova_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                _dsGriglieODL.Tables[_nomeTabellaGrigliaODL].Clear();
                string codiceSegnalatore = string.Empty;
                string codiceReparto = string.Empty;
                if (ddlSegnalatore.SelectedIndex != -1)
                {
                    codiceSegnalatore = ddlSegnalatore.SelectedItem as string;
                    if (_dsAnagrafica.SEGNALATORI.Any(x => x.RAGIONESOC == codiceSegnalatore))
                        codiceSegnalatore = _dsAnagrafica.SEGNALATORI.Where(x => x.RAGIONESOC == codiceSegnalatore).Select(x => x.CODICE).FirstOrDefault();
                }

                if (ddlReparto.SelectedIndex != -1)
                {
                    codiceReparto = ddlReparto.SelectedItem as string;
                    if (_dsAnagrafica.REPARTI.Any(x => x.RAGIONESOC == codiceReparto))
                        codiceReparto = _dsAnagrafica.REPARTI.Where(x => x.RAGIONESOC == codiceReparto).Select(x => x.CODICE).FirstOrDefault();
                }
                _dsPriorita = new PrioritaDS();
                using (PrioritaBusiness bPriorita = new PrioritaBusiness())
                {
                    bPriorita.FillUSR_PRD_MOVFASI_Aperti(_dsPriorita, codiceSegnalatore, codiceReparto);
                    bPriorita.FillUSR_PRD_MOVFASI_Chiusi(_dsPriorita, codiceSegnalatore, codiceReparto, 7);

                    List<string> articoli = _dsPriorita.USR_PRD_MOVFASI.Select(x => x.IDMAGAZZ).Distinct().ToList();
                    bPriorita.FillMAGAZZ(_dsAnagrafica, articoli);

                    List<string> IDPRDFASE = _dsPriorita.USR_PRD_MOVFASI.Select(x => x.IDPRDFASE).Distinct().ToList();
                    bPriorita.FillUSR_PRD_FASI(_dsPriorita, IDPRDFASE);

                    List<string> IDLANCIOD = _dsPriorita.USR_PRD_FASI.Select(x => x.IDLANCIOD).Distinct().ToList();
                    bPriorita.FillUSR_PRD_LANCIOD(_dsPriorita, IDLANCIOD);

                    PopolaDSGrigliaODL();
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
            dtGriglia.Columns.Add("Segnalatore", Type.GetType("System.String")).ReadOnly = true;
            dtGriglia.Columns.Add("Commessa", Type.GetType("System.String")).ReadOnly = true;
            dtGriglia.Columns.Add("Data commessa", Type.GetType("System.String")).ReadOnly = true;
            dtGriglia.Columns.Add("ODL", Type.GetType("System.String")).ReadOnly = true;
            dtGriglia.Columns.Add("Data ODL", Type.GetType("System.String")).ReadOnly = true;
            dtGriglia.Columns.Add("Barcode", Type.GetType("System.String")).ReadOnly = true;
            dtGriglia.Columns.Add("Reparto", Type.GetType("System.String")).ReadOnly = true;
            dtGriglia.Columns.Add("Articolo", Type.GetType("System.String")).ReadOnly = true;
            dtGriglia.Columns.Add("Quantità", Type.GetType("System.Decimal")).ReadOnly = true;
            dtGriglia.Columns.Add("Da terminare", Type.GetType("System.Decimal")).ReadOnly = true;

        }

        private void CreaDSGrigliaScadenze()
        {

            DataTable dtGriglia = _dsGriglieODL.Tables.Add();
            dtGriglia.TableName = _nomeTabellaGrigliaScadenze;
            dtGriglia.Columns.Add("IDPRDMOVFASE", Type.GetType("System.String")).ReadOnly=false;
            dtGriglia.Columns.Add("IDMAGAZZ", Type.GetType("System.String")).ReadOnly = false;
            dtGriglia.Columns.Add("Data", Type.GetType("System.String")).ReadOnly = false;
            dtGriglia.Columns.Add("Quantità", Type.GetType("System.Decimal")).ReadOnly = false;
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
                riga[(int)Colonne.REPARTO] = odl.IsCODICECLIFONull() ? string.Empty : odl.CODICECLIFO;
                if (articolo != null)
                    riga[(int)Colonne.ARTICOLO] = articolo.MODELLO;
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
            dgvODL.Columns[(int)Colonne.SEGNALATORE].Width = 150;
            dgvODL.Columns[(int)Colonne.COMMESSA].Width = 140;
            dgvODL.Columns[(int)Colonne.DATACOMMESSA].Width = 80;
            dgvODL.Columns[(int)Colonne.ODL].Width = 130;
            dgvODL.Columns[(int)Colonne.DATAODL].Width = 80;
            dgvODL.Columns[(int)Colonne.BARCODE].Width = 100;
            dgvODL.Columns[(int)Colonne.REPARTO].Width = 70;
            dgvODL.Columns[(int)Colonne.ARTICOLO].Width = 200;
            dgvODL.Columns[(int)Colonne.QUANTITA].Width = 70;
            dgvODL.Columns[(int)Colonne.QUANTITADATERMINARE].Width = 70;

        }

        private void dgvODL_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (dgvODL.Rows[e.RowIndex].Cells[(int)Colonne.QUANTITADATERMINARE].Value == null) return;

            decimal qtaDaTer = (decimal)dgvODL.Rows[e.RowIndex].Cells[(int)Colonne.QUANTITADATERMINARE].Value;

            if (qtaDaTer == 0)
            {
                System.Windows.Forms.DataGridViewCellStyle boldStyle = new System.Windows.Forms.DataGridViewCellStyle();
                boldStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
                dgvODL.Rows[e.RowIndex].DefaultCellStyle = boldStyle;
            }
        }

        private void dgvODL_CellClick(object sender, DataGridViewCellEventArgs e)
        {
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


                    dgvScadenze.DataSource = _dsGriglieODL;
                    dgvScadenze.DataMember = _nomeTabellaGrigliaScadenze;
                }
            }
            catch (Exception ex)
            {
                ExceptionFrm form = new ExceptionFrm(ex);
                form.ShowDialog();
            }
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
            dgvTermini.Columns[5].Width = 350;
            dgvTermini.Columns[6].Visible = true;
        }

        private void CreaGrigliaScadenze()
        {

            dgvScadenze.DataSource = _dsGriglieODL;
            dgvScadenze.DataMember = _nomeTabellaGrigliaScadenze;

            dgvScadenze.Columns[0].Visible = false;
            dgvScadenze.Columns[1].Visible = false;
            dgvScadenze.Columns[2].Width = 80;
            dgvScadenze.Columns[3].Width = 80;

        }
    }
}
