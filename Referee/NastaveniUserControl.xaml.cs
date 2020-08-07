using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;
using Newtonsoft.Json;
using System.Reflection;

namespace Referee
{
    /// <summary>
    /// Interakční logika pro NastaveniUserControl.xaml
    /// </summary>
    public partial class NastaveniUserControl : UserControl
    {
        public NastaveniUserControl()
        {
            InitializeComponent();
            Helper helper = new Helper();
            helper.SettingsFileExists();
            DirectPrintSelectionItemsControl.ItemsSource = GetDTGDColumbs();
        }

        private List<DTGDColumb> dTGDColumbs = new List<DTGDColumb>();

        private List<DTGDColumb> GetDTGDColumbs()
        {
            string DTGDColumbsPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DTGDColumbs.json";
            string text = File.ReadAllText(DTGDColumbsPath);
            if (text == "")
            {
                Helper helper = new Helper();
                string zapis = helper.GenerateNewSetting();
                File.WriteAllText(DTGDColumbsPath, zapis);
                text = zapis;
            }
            dTGDColumbs = (List<DTGDColumb>)JsonConvert.DeserializeObject(text, typeof(List<DTGDColumb>));
            return dTGDColumbs;
        }

        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            string nastaveniJsonText = JsonConvert.SerializeObject(dTGDColumbs);
            string DTGDColumbsPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DTGDColumbs.json";
            File.WriteAllText(DTGDColumbsPath, nastaveniJsonText);
        }
    }
}
