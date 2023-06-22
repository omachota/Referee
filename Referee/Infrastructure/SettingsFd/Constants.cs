using System;
using System.IO;

namespace Referee.Infrastructure.SettingsFd
{
	public static class Constants
	{
		public static readonly string WorkingDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
			"OndrejMachota", "Referee");

		public static readonly Settings DefaultSettings = new Settings("ATLETIKA STARÁ BOLESLAV, z.s.", "", DateTime.Today, DateTime.Today,
			"Houštka, Stará Boleslav", new DbSettings());
	}
}
