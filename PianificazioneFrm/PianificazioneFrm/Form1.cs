using Pianificazione.Data;
using Pianificazione.Entities;
using PianificazioneFrm.Gantt;
using PianificazioneFrm.Helpers;
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

namespace PianificazioneFrm
{

    public partial class Form1 : Form
    {
        private DataSet _dsGriglia;
        private string _nomeTabella = "Griglia";
        private PianificazioneDS _dsPianificazione;
        List<PianificazioneDS.TABFASRow> _fasi;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnTrova_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                _dsPianificazione = new PianificazioneDS();
                string reparto = string.Empty;
                string fase = string.Empty;

                if (ddlReparto.SelectedIndex != -1)
                    reparto = (string)ddlReparto.SelectedItem;

                if (ddlFase.SelectedIndex != -1)
                    fase = (string)ddlFase.SelectedItem;

                using (PianificazioneBusiness bPianificazione = new PianificazioneBusiness())
                {
                    bPianificazione.FillV_PIAN_AGGR_2(_dsPianificazione, dtDal.Value, dtAl.Value, reparto, fase);
                    bPianificazione.FillPIANIFICAZIONE_STATICA(_dsPianificazione, dtDal.Value, dtAl.Value);

                }

                CreaDSGriglia();
                dgvGriglia.Columns.Clear();
                //                dgvGriglia.AutoGenerateColumns = true;
                dgvGriglia.DataSource = _dsGriglia;
                dgvGriglia.DataMember = _nomeTabella;

                dgvGriglia.Columns[(int)Colonne.IDMAGAZZFASE].Visible = false;
                dgvGriglia.Columns[(int)Colonne.IDMAGAZZLancio].Visible = false;
                dgvGriglia.Columns[(int)Colonne.Segnalatore].Frozen = true;
                dgvGriglia.Columns[(int)Colonne.ModelloLancio].Frozen = true;
                dgvGriglia.Columns[(int)Colonne.Modello].Frozen = true;
                dgvGriglia.Columns[(int)Colonne.Modello].Width = 70;
                dgvGriglia.Columns[(int)Colonne.Reparto].Frozen = true;
                dgvGriglia.Columns[(int)Colonne.Reparto].Width = 70;
                dgvGriglia.Columns[(int)Colonne.Fase].Frozen = true;
                dgvGriglia.Columns[(int)Colonne.Fase].Width = 70;
                dgvGriglia.Columns[(int)Colonne.Materiale].Frozen = true;
                dgvGriglia.Columns[(int)Colonne.Materiale].Width = 60;
                dgvGriglia.Columns[(int)Colonne.Finitura].Width = 60;
                dgvGriglia.Columns[(int)Colonne.Finitura].Frozen = true;
                dgvGriglia.Columns[(int)Colonne.PezziBarra].Width = 50;
                dgvGriglia.Columns[(int)Colonne.PezziBarra].Frozen = true;

                dgvGriglia.Columns[(int)Colonne.Gruppo].Frozen = true;
                dgvGriglia.Columns[(int)Colonne.Gruppo].Width = 50;

                dgvGriglia.Columns[(int)Colonne.NumeroPezzi].Frozen = true;
                dgvGriglia.Columns[(int)Colonne.NumeroPezzi].Width = 50;
                dgvGriglia.Columns[(int)Colonne.NumeroPezzi].DefaultCellStyle.BackColor = Color.DarkRed;
                dgvGriglia.Columns[(int)Colonne.NumeroPezzi].DefaultCellStyle.ForeColor = Color.White;

                int numeroGiorni = GetNumeroGiorni();
                for (int i = 0; i < numeroGiorni; i++)
                {
                    dgvGriglia.Columns[(int)Colonne.NumeroPezzi + 1 + 2 * i].Width = 70;
                    dgvGriglia.Columns[(int)Colonne.NumeroPezzi + 1 + 2 * i + 1].Width = 70;
                    dgvGriglia.Columns[(int)Colonne.NumeroPezzi + 1 + 2 * i].DefaultCellStyle.ForeColor = Color.Red;
                    dgvGriglia.Columns[(int)Colonne.NumeroPezzi + 1 + 2 * i + 1].DefaultCellStyle.ForeColor = Color.Green;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

        }

        private int GetNumeroGiorni()
        {
            return (((TimeSpan)(dtAl.Value - dtDal.Value)).Days) + 1;
        }
        private enum Colonne { IDMAGAZZFASE, IDMAGAZZLancio, Segnalatore, ModelloLancio, Modello, Reparto, Fase, Materiale, Finitura, PezziBarra, Gruppo, NumeroPezzi }
        private void CreaDSGriglia()
        {

            _dsGriglia = new DataSet();
            DataTable dtGriglia = _dsGriglia.Tables.Add();
            dtGriglia.TableName = _nomeTabella;
            dtGriglia.Columns.Add("IDMAGAZZFASE", Type.GetType("System.String")).ReadOnly = true;
            dtGriglia.Columns.Add("IDMAGAZZLancio", Type.GetType("System.String")).ReadOnly = true;
            dtGriglia.Columns.Add("Segnalatore", Type.GetType("System.String")).ReadOnly = true;

            dtGriglia.Columns.Add("Modello lancio", Type.GetType("System.String")).ReadOnly = true;
            dtGriglia.Columns.Add("Modello", Type.GetType("System.String")).ReadOnly = true;
            dtGriglia.Columns.Add("Reparto", Type.GetType("System.String")).ReadOnly = true;
            dtGriglia.Columns.Add("Fase", Type.GetType("System.String")).ReadOnly = true;
            dtGriglia.Columns.Add("Materiale", Type.GetType("System.String")).ReadOnly = true;
            dtGriglia.Columns.Add("Finitura", Type.GetType("System.String")).ReadOnly = true;
            dtGriglia.Columns.Add("Pezzi x barra", Type.GetType("System.String"));
            dtGriglia.Columns.Add("Gruppo", Type.GetType("System.String"));
            dtGriglia.Columns.Add("Numero pezzi", Type.GetType("System.String"));

            int numeroGiorni = GetNumeroGiorni();

            for (int i = 0; i < numeroGiorni; i++)
            {
                dtGriglia.Columns.Add(dtDal.Value.AddDays(i).ToShortDateString(), Type.GetType("System.String")).ReadOnly = true;
                dtGriglia.Columns.Add(dtDal.Value.AddDays(i).ToShortDateString() + " s", Type.GetType("System.String"));
            }

            var gruppi =
                  from articolo in _dsPianificazione.V_PIAN_AGGR_2
                  group articolo by new
                  {
                      articolo.IDMAGAZZ,
                      articolo.IDMAGAZZ_FASE,
                      articolo.SEGNALATORE,
                      articolo.MODELLOLANCIO,
                      articolo.MODELLO,
                      articolo.REPARTO,
                      articolo.CODICEFASE,
                      articolo.MATERIALE,
                      articolo.FINITURA,
                      articolo.PEZZI,
                      articolo.GRUPPO
                  }
            into GruppiArticolo
                  select GruppiArticolo.Key;

            foreach (var gruppo in gruppi)
            {
                DataRow riga = dtGriglia.NewRow();

                riga[(int)Colonne.IDMAGAZZFASE] = gruppo.IDMAGAZZ_FASE;
                riga[(int)Colonne.IDMAGAZZLancio] = gruppo.IDMAGAZZ;
                riga[(int)Colonne.Segnalatore] = gruppo.SEGNALATORE;
                riga[(int)Colonne.ModelloLancio] = gruppo.MODELLOLANCIO;
                riga[(int)Colonne.Modello] = gruppo.MODELLO;
                riga[(int)Colonne.Reparto] = gruppo.REPARTO;
                riga[(int)Colonne.Fase] = gruppo.CODICEFASE;
                riga[(int)Colonne.Materiale] = gruppo.MATERIALE;
                riga[(int)Colonne.Finitura] = gruppo.FINITURA;
                riga[(int)Colonne.PezziBarra] = gruppo.PEZZI.ToString();
                riga[(int)Colonne.Gruppo] = gruppo.GRUPPO.ToString();
                decimal totale = 0;
                for (int i = 0; i < numeroGiorni; i++)
                {
                    PianificazioneDS.PIANIFICAZIONE_STATICARow statico = _dsPianificazione.PIANIFICAZIONE_STATICA.Where(x =>
                                       x.IDMAGAZZ == gruppo.IDMAGAZZ &&
                                       x.IDMAGAZZ_FASE == gruppo.IDMAGAZZ_FASE &&
                                       x.CODICEFASE == gruppo.CODICEFASE &&
                                       x.REPARTO == gruppo.REPARTO &&
                                       x.DATA == dtDal.Value.AddDays(i)).FirstOrDefault();
                    string valoreStatico = string.Empty;
                    if (statico != null)
                        valoreStatico = statico.QTA;

                    if (i == 0)
                    {
                        decimal aux =
                        _dsPianificazione.V_PIAN_AGGR_2.Where(x =>
                        x.IDMAGAZZ == gruppo.IDMAGAZZ &&
                        x.IDMAGAZZ_FASE == gruppo.IDMAGAZZ_FASE &&
                        x.SEGNALATORE == gruppo.SEGNALATORE &&
                        x.REPARTO == gruppo.REPARTO &&
                        x.CODICEFASE == gruppo.CODICEFASE &&
                        x.DATAINIZIO <= dtDal.Value).Sum(x => x.QTA);

                        totale += aux;
                        riga[(int)Colonne.NumeroPezzi + 1] = aux.ToString();
                        riga[(int)Colonne.NumeroPezzi + 1 + 1] = valoreStatico;
                    }
                    else
                    {
                        decimal aux = _dsPianificazione.V_PIAN_AGGR_2.Where(x =>
                         x.IDMAGAZZ == gruppo.IDMAGAZZ &&
                         x.IDMAGAZZ_FASE == gruppo.IDMAGAZZ_FASE &&
                         x.SEGNALATORE == gruppo.SEGNALATORE &&
                         x.REPARTO == gruppo.REPARTO &&
                         x.CODICEFASE == gruppo.CODICEFASE &&
                         x.DATAINIZIO == dtDal.Value.AddDays(i)).Sum(x => x.QTA);
                        totale += aux;
                        riga[(int)Colonne.NumeroPezzi + 1 + i * 2] = aux.ToString();
                        riga[(int)Colonne.NumeroPezzi + 1 + i * 2 + 1] = valoreStatico;
                    }
                }
                riga[(int)Colonne.NumeroPezzi] = totale.ToString();

                dtGriglia.Rows.Add(riga);
            }

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;

            dtAl.Value = DateTime.Today.AddDays(+7);

            dtDal.Value = DateTime.Today.AddDays(-1);

            using (PianificazioneBusiness bPianificazione = new PianificazioneBusiness())
            {
                _dsPianificazione = new PianificazioneDS();
                bPianificazione.FillTABFAS(_dsPianificazione);

                List<string> reparti = new List<string>();
                reparti.Add(" ");
                reparti.AddRange(_dsPianificazione.TABFAS.Where(x => !x.IsCODICECLIFOPREDFASENull()).OrderBy(x => x.CODICECLIFOPREDFASE).Select(x => x.CODICECLIFOPREDFASE).Distinct().ToList());

                _fasi = _dsPianificazione.TABFAS.ToList();

                ddlReparto.Items.AddRange(reparti.ToArray());
            }
        }

        private void CaricaFasi(string reparto)
        {
            ddlFase.Items.Clear();
            if (!string.IsNullOrEmpty(reparto.Trim()))
            {
                List<string> strFasi = new List<string>();
                strFasi.Add(" ");
                strFasi.AddRange(_fasi.Where(x => !x.IsCODICECLIFOPREDFASENull() && x.CODICECLIFOPREDFASE == reparto).OrderBy(x => x.CODICEFASE).Select(x => x.CODICEFASE).Distinct().ToList());
                ddlFase.Items.AddRange(strFasi.ToArray());
            }
        }

        private void ddlReparto_SelectedIndexChanged(object sender, EventArgs e)
        {
            string reparto = (string)ddlReparto.SelectedItem;
            CaricaFasi(reparto);
        }

        private void btnSalva_Click(object sender, EventArgs e)
        {
            int numeroGiorni = GetNumeroGiorni();
            foreach (DataRow riga in _dsGriglia.Tables[_nomeTabella].Rows)
            {
                string IDMAGAZZLancio = (string)riga[(int)Colonne.IDMAGAZZLancio];
                string IDMAGAZZFase = (string)riga[(int)Colonne.IDMAGAZZFASE];
                string reparto = (string)riga[(int)Colonne.Reparto];
                string fase = (string)riga[(int)Colonne.Fase];

                for (int i = 0; i < numeroGiorni; i++)
                {
                    string valore = (string)riga[(int)Colonne.NumeroPezzi + 1 + 2 * i + 1];
                    valore = valore.Trim();
                    string data = dgvGriglia.Columns[(int)Colonne.NumeroPezzi + 1 + 2 * i].Name;
                    DateTime dt = DateTime.Parse(data);

                    PianificazioneDS.PIANIFICAZIONE_STATICARow rigaDaInserire = _dsPianificazione.PIANIFICAZIONE_STATICA.Where(x =>
                    x.IDMAGAZZ == IDMAGAZZLancio &&
                    x.IDMAGAZZ_FASE == IDMAGAZZFase &&
                    x.CODICEFASE == fase &&
                    x.REPARTO == reparto &&
                    x.DATA == dt).FirstOrDefault();

                    if (!string.IsNullOrEmpty(valore))
                    {
                        if (rigaDaInserire == null)
                        {
                            rigaDaInserire = _dsPianificazione.PIANIFICAZIONE_STATICA.NewPIANIFICAZIONE_STATICARow();
                            rigaDaInserire.CODICEFASE = fase;
                            rigaDaInserire.DATA = dt;
                            rigaDaInserire.IDMAGAZZ = IDMAGAZZLancio;
                            rigaDaInserire.IDMAGAZZ_FASE = IDMAGAZZFase;
                            rigaDaInserire.QTA = valore;
                            rigaDaInserire.REPARTO = reparto;
                            _dsPianificazione.PIANIFICAZIONE_STATICA.AddPIANIFICAZIONE_STATICARow(rigaDaInserire);
                        }
                        else
                            rigaDaInserire.QTA = valore;
                    }
                    else
                    {
                        if (rigaDaInserire != null)
                            rigaDaInserire.Delete();
                    }
                }
            }

            using (PianificazioneBusiness bPianificazione = new PianificazioneBusiness())
            {
                bPianificazione.SalvaPianificazioneStatica(_dsPianificazione);
                _dsPianificazione.AcceptChanges();
            }
        }

        private void btnEsporta_Click(object sender, EventArgs e)
        {
            FileStream fs = null;
            if (_dsGriglia == null || _dsGriglia.Tables[_nomeTabella].Rows.Count == 0)
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
                ExcelHelper hExcel = new ExcelHelper();
                byte[] fileExcel = hExcel.CreaExcelPianificazione(_dsGriglia, _nomeTabella);

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
            }

        }
    }
}
