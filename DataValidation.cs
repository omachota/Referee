using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Referee
{
    public class DataValidation
    {
        public string test { get; set; }
        public string JmenoRozhodciho { get; set; }
        public string PrijmeniRozhodciho { get; set; }
        public string ValidatingDate { get; set; }
        public string AdresaRozhodciho { get; set; }
        public string MestoRozhodciho { get; set; }

        public DataValidation()
        {
            test = "";
            JmenoRozhodciho = "";
            PrijmeniRozhodciho = "";
            ValidatingDate = "";
            AdresaRozhodciho = "";
            MestoRozhodciho = "";
        }
    }
}
