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
    public partial class ExceptionFrm : Form
    {
        private Exception _ex;
        public ExceptionFrm(Exception ex)
        {
            _ex = ex;
            InitializeComponent();

            CaricaEccezione();
            timer1.Start();
        }

        private void CaricaEccezione()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("MESSAGGIO");
            sb.AppendLine(_ex.Message);
            sb.AppendLine(string.Empty);
            sb.AppendLine("STACK");
            sb.AppendLine(_ex.StackTrace);

            Exception ex = _ex.InnerException;
            while (ex != null)
            {
                sb.AppendLine("** INNER EXCELPTIO **");
                sb.AppendLine("MESSAGGIO");
                sb.AppendLine(ex.Message);
                sb.AppendLine(string.Empty);
                sb.AppendLine("STACK");
                sb.AppendLine(ex.StackTrace);
                ex = ex.InnerException;
            }

            txtErrore.Text = sb.ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Enabled = false;
            timer1.Stop();
        }

        private void ExceptionFrm_Load(object sender, EventArgs e)
        {

        }
    }
}
