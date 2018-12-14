using Pianificazione.Data;
using Pianificazione.Entities;
using PianificazioneFrm.Gantt;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PianificazioneFrm
{
    public partial class Form1 : Form
    {
        GanttChart ganttChart1;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnTrova_Click(object sender, EventArgs e)
        {
            PianificazioneDS dsRoot = new PianificazioneDS();

            using (PianificazioneBusiness b = new PianificazioneBusiness())
            {
                tableLayoutPanel1.Controls.Clear();
                ganttChart1 = new GanttChart();
                ganttChart1.AllowChange = true;
                ganttChart1.Dock = DockStyle.Fill;
                ganttChart1.FromDate = new DateTime(dtDal.Value.Year, dtDal.Value.Month, dtDal.Value.Day, 0, 0, 0);
                ganttChart1.ToDate = new DateTime(dtAl.Value.Year, dtAl.Value.Month, dtAl.Value.Day, 23, 59, 0);
                tableLayoutPanel1.Controls.Add(ganttChart1, 0, 0);

                b.FillUSR_PRD_FASI_ROOT(dsRoot, dtDal.Value, dtAl.Value);
                List<decimal> idlancio = dsRoot.PIANIFICAZIONE_FASE.Where(x => !x.IsIDLANCIONull()).Select(x => x.IDLANCIO).Distinct().ToList();

                List<Lavorazione> lavorazioni = CreaListaLavorazione(dsRoot);
                List<BarInformation> lst1 = CreaListaBarInformation(lavorazioni);

                foreach (BarInformation bar in lst1)
                {
                    ganttChart1.AddChartBar(bar.RowText, bar.Label, bar, bar.FromTime, bar.ToTime, bar.Color, bar.HoverColor, bar.Index);
                }
            }

            tableLayoutPanel1.Update();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dtAl.Value = DateTime.Today;

            dtDal.Value = DateTime.Today.AddDays(-7);
        }

        private List<Lavorazione> CreaListaLavorazione(PianificazioneDS ds)
        {
            List<Lavorazione> lavorazioni = new List<Lavorazione>();

            int indiceRamo = 0;
            Dictionary<decimal, int> riferimentoBarra = new Dictionary<decimal, int>();
            int colore = 0;
            foreach (PianificazioneDS.PIANIFICAZIONE_FASERow fase in ds.PIANIFICAZIONE_FASE.OrderBy(x => x.IDLANCIO).ThenBy(x => x.IDFASE))
            {
                Lavorazione lavorazione = new Lavorazione();

                lavorazione.Commessa = string.Empty;
                PianificazioneDS.PIANIFICAZIONE_LANCIORow lanciod = GetLancioD(fase.IDLANCIO, ds);
                if (lanciod != null)
                    lavorazione.Commessa = lanciod.IsNOMECOMMESSANull() ? string.Empty : lanciod.NOMECOMMESSA;

                lavorazione.Reparto = fase.CODICECLIFO;

                DateTime inizio = new DateTime(fase.DATAINIZIO.Year, fase.DATAINIZIO.Month, fase.DATAINIZIO.Day, 1, 0, 0);
                DateTime fine = new DateTime(fase.DATAFINE.Year, fase.DATAFINE.Month, fase.DATAFINE.Day, 1, 0, 0);

                if (!fase.IsIDPRDFASEPADRENull())
                {
                    Lavorazione padre = lavorazioni.Where(x => x.IDPRDFASE == fase.IDPRDFASEPADRE).FirstOrDefault();
                    if (padre != null)
                    {
                        if (StessoGiorno(padre.Fine, fase.DATAINIZIO))
                        {
                            inizio = padre.Fine.AddHours(1);
                        }
                    }
                }

                if (StessoGiorno(fase.DATAINIZIO, fase.DATAFINE))
                {
                    fine = inizio.AddHours(1);
                }

                lavorazione.Inizio = inizio;
                lavorazione.Fine = fine;

                lavorazione.colore = Color.Green;
                if (colore % 2 == 0)
                    lavorazione.colore = Color.Red;
                colore++;

                lavorazione.Ramo = indiceRamo;

                if (!fase.IsIDPRDFASEPADRENull() && riferimentoBarra.ContainsKey(fase.IDFASEPADRE))
                {
                    lavorazione.Ramo = riferimentoBarra[fase.IDFASEPADRE];
                    if (lavorazione.Ramo == -1)
                    {
                        indiceRamo++;
                        lavorazione.Ramo = indiceRamo;
                    }
                }
                else
                {
                    indiceRamo++;
                    lavorazione.Ramo = indiceRamo;
                }

                if (
                    fase.CODICECLIFO.Trim() == "MONT" ||
                    fase.CODICECLIFO.Trim() == "SALD"
                    )
                    riferimentoBarra.Add(fase.IDFASE, -1);
                else
                    riferimentoBarra.Add(fase.IDFASE, lavorazione.Ramo);

                lavorazioni.Add(lavorazione);
            }


            return lavorazioni;
        }

        private bool StessoGiorno(DateTime giorno1, DateTime giorno2)
        {
            return (giorno1.Year == giorno2.Year && giorno1.Month == giorno2.Month && giorno1.Day == giorno2.Day);
        }

        private List<BarInformation> CreaListaBarInformation(PianificazioneDS dsRoot)
        {
            List<BarInformation> lst1 = new List<BarInformation>();
            int i = -1;
            Dictionary<string, int> riferimentoBarra = new Dictionary<string, int>();
            int indiceColore = 0;
            foreach (PianificazioneDS.USR_PRD_FASIRow fase in dsRoot.USR_PRD_FASI.OrderBy(x => x.IDLANCIOD).ThenBy(x => x.IDPRDFASE))
            {
                int indice = 0;
                if (!fase.IsIDPRDFASEPADRENull() && riferimentoBarra.ContainsKey(fase.IDPRDFASEPADRE))
                {
                    indice = riferimentoBarra[fase.IDPRDFASEPADRE];
                    if (indice == -1)
                    {
                        i++;
                        indice = i;
                    }
                }
                else
                {
                    i++;
                    indice = i;
                }
                DateTime inizio = new DateTime(fase.DATAINIZIO.Year, fase.DATAINIZIO.Month, fase.DATAINIZIO.Day, 1, 0, 0);
                DateTime fine = new DateTime(fase.DATAFINE.Year, fase.DATAFINE.Month, fase.DATAFINE.Day, 1, 0, 0);
                Color colore = Color.Green;
                if (indiceColore % 2 == 0)
                    colore = Color.Red;
                lst1.Add(new BarInformation(fase.IDLANCIOD, fase.CODICECLIFO, inizio, fine, colore, Color.Khaki, indice));
                if (fase.CODICECLIFO.Trim() == "MONT")
                    riferimentoBarra.Add(fase.IDPRDFASE, -1);
                else
                    riferimentoBarra.Add(fase.IDPRDFASE, indice);
                indiceColore++;
            }

            return lst1;
        }

        private List<BarInformation> CreaListaBarInformation(List<Lavorazione> lavorazioni)
        {
            List<BarInformation> lst1 = new List<BarInformation>();
            int i = -1;

            foreach (Lavorazione lavorazione in lavorazioni)
            {
                lst1.Add(new BarInformation(lavorazione.Commessa, lavorazione.Reparto, lavorazione.Inizio, lavorazione.Fine, lavorazione.colore, Color.Khaki, lavorazione.Ramo));
            }

            return lst1;
        }

        private PianificazioneDS.PIANIFICAZIONE_LANCIORow GetLancioD(decimal IDLANCIO, PianificazioneDS ds)
        {
            PianificazioneDS.PIANIFICAZIONE_LANCIORow lanciod = ds.PIANIFICAZIONE_LANCIO.Where(x => x.IDLANCIO == IDLANCIO).FirstOrDefault();
            if (lanciod == null)
            {

                using (PianificazioneBusiness b = new PianificazioneBusiness())
                {
                    b.FillPIANIFICAZIONE_LANCIOByIdLancio(ds, IDLANCIO);
                }
                lanciod = ds.PIANIFICAZIONE_LANCIO.Where(x => x.IDLANCIO == IDLANCIO).FirstOrDefault();
            }
            return lanciod;
        }
    }
}
