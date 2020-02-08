using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Referee
{
    public class TechnickaCeta
    {
        public Int64 Id { get; set; }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string DatumNarozeni { get; set; }
        public string AdresaBydliste { get; set; }
        public string Mesto { get; set; }
        private bool cetarJeVybrany;
        public bool CetarJeVybrany
        {
            get
            {
                return cetarJeVybrany;
            }
            set
            {
                cetarJeVybrany = value;
                OnPropertyChanged("CetarJeVybrany");
            }
        }

        public TechnickaCeta(long id, string jmeno, string prijmeni, string datumNarozeni, string adresaBydliste, string mesto)
        {
            Id = id;
            Jmeno = jmeno;
            Prijmeni = prijmeni;
            DatumNarozeni = datumNarozeni;
            AdresaBydliste = adresaBydliste;
            Mesto = mesto;
            CetarJeVybrany = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
