using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pianificazione.Entities
{
    public static class StatoFasePianificazione
    {
        public static readonly string PIANIFICATO = "PIANIFICATO";
        public static readonly string APERTO = "APERTO";
        public static readonly string CHIUSO = "CHIUSO";
        public static readonly string ANNULLATO = "ANNULLATO";
        public static readonly string ACCANTONATO = "DA ACCANTONATO";
        public static readonly string INFRAGRUPPO = "DA INFRAGRUPPO";
    }
}
