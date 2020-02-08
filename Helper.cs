using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Reflection;
using System.Windows;
using System;

namespace Referee
{
    public class Helper
    {
        private List<DTGDColumb> dTGDColumbs = new List<DTGDColumb>
        {
            new DTGDColumb("Jméno", "Jmeno", true),
            new DTGDColumb("Příjmení", "Prijmeni", true),
            new DTGDColumb("Datum narození", "DatumNarozeni", true),
            new DTGDColumb("Adresa", "AdresaBydliste", true),
            new DTGDColumb("Město", "Mesto", true),
        };

        public string GenerateNewSetting()
        {
            string nastaveniJsonText = JsonConvert.SerializeObject(dTGDColumbs);
            return nastaveniJsonText;
        }


        public bool SettingsFileExits()
        {
            try
            {
                string DTGDColumbsPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DTGDColumbs.json";
                if (!File.Exists(DTGDColumbsPath))
                {
                    File.Create(DTGDColumbsPath);
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
    }
}
