using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Reflection;
using Referee.Models;

namespace Referee
{
	public class Helper
	{
		private readonly List<OptionalColumn> _optionalColumns = new List<OptionalColumn>
		{
			new OptionalColumn("Jméno", nameof(IPerson.FirstName), true),
			new OptionalColumn("Příjmení", nameof(IPerson.LastName), true),
			new OptionalColumn("Datum narození", nameof(IPerson.BirthDate), true),
			new OptionalColumn("Adresa", nameof(IPerson.Address), true),
			new OptionalColumn("Město", nameof(IPerson.City), true),
		};

		public string GenerateNewSetting()
		{
			string nastaveniJsonText = JsonConvert.SerializeObject(_optionalColumns);
			return nastaveniJsonText;
		}


		public void SettingsFileExists()
		{
			string DTGDColumbsPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Settings.json");
			if (!File.Exists(DTGDColumbsPath))
			{
				File.Create(DTGDColumbsPath);
			}
		}
	}
}
