using System;
using System.IO;

namespace Referee.Infrastructure.SettingsFd
{
	public static class Constants
	{
		public static readonly string WorkingDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
			"OndrejMachota", "Referee");

		public static readonly Settings DefaultSettings = new Settings("ATLETIKA STARÁ BOLESLAV, z.s.", "", DateTime.Today, DateTime.Today,
			"Houštka, Stará Boleslav", new DbSettings());

		public static readonly string DatabasePath = Path.Combine(WorkingDirectory, "Referee.db");
		public static readonly string SettingsPath = Path.Combine(WorkingDirectory, "Settings.json");
	}
}
