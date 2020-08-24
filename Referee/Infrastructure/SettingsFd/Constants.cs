using System;
using System.IO;

namespace Referee.Infrastructure.SettingsFd
{
	public static class Constants
	{
		public static readonly string WorkingDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
			"Ondřej Machota", "Referee");

		public static readonly Settings DefaultSettings = new Settings("ATLETIKA STARÁ BOLESLAV, z.s.", "", DateTime.Today, DateTime.Today,
			"Houštka, Stará Boleslav", new DbSettings("", "", "", ""));

		/*private static readonly List<OptionalColumn> OptionalColumns = new List<OptionalColumn>
		{
			new OptionalColumn("Jméno", nameof(IPerson.FirstName), true),
			new OptionalColumn("Příjmení", nameof(IPerson.LastName), true),
			new OptionalColumn("Datum narození", nameof(IPerson.BirthDate), true),
			new OptionalColumn("Adresa", nameof(IPerson.Address), true),
			new OptionalColumn("Město", nameof(IPerson.City), true),
		};*/
	}
}
