using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Reflection;

namespace Referee
{
	public class Helper
	{
		private readonly List<DTGDColumb> _dTgdColumbs = new List<DTGDColumb>
		{
			new DTGDColumb("Jméno", "Jmeno", true),
			new DTGDColumb("Příjmení", "Prijmeni", true),
			new DTGDColumb("Datum narození", "DatumNarozeni", true),
			new DTGDColumb("Adresa", "AdresaBydliste", true),
			new DTGDColumb("Město", "Mesto", true),
		};

		public string GenerateNewSetting()
		{
			string nastaveniJsonText = JsonConvert.SerializeObject(_dTgdColumbs);
			return nastaveniJsonText;
		}


		public void SettingsFileExists()
		{
			string DTGDColumbsPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DTGDColumbs.json";
			if (!File.Exists(DTGDColumbsPath))
			{
				File.Create(DTGDColumbsPath);
			}
		}
	}
}
