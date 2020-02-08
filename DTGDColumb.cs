using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Referee
{
    public class DTGDColumb
    {
        public string Jmeno { get; set; }
        public string Binding { get; set; }
        public bool IsSelected { get; set; }

        public DTGDColumb(string jmeno, string binding, bool isSelected)
        {
            Jmeno = jmeno;
            Binding = binding;
            IsSelected = isSelected;
        }
    }
}
