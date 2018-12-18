using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pianificazione.Entities
{
    public class Lavorazione
    {
        public string Reparto { get; set; }
        public DateTime Inizio { get; set; }
        public DateTime Fine { get; set; }
        public string Commessa { get; set; }
        public Color colore { get; set; }
        public int Ramo { get; set; }
        public string IDPRDFASE { get; set; }
        public decimal Qta{ get; set; }
    }
}
