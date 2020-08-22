using System;
using Referee.Infrastructure.SettingsFd;
using Referee.Models;

namespace Referee.Infrastructure
{
	public static class Extensions
	{
		public static bool ValidateRozhodci(Rozhodci rozhodci)
		{
			if (rozhodci == null)
				return false;
			if (rozhodci.FirstName == "")
				return false;
			if (rozhodci.LastName == "")
				return false;
			if (rozhodci.Address == "")
				return false;
			if (rozhodci.City == "")
				return false;
			if (rozhodci.BirthDate >= DateTime.Today)
				return false;
			return true;
		}

		public static void CopyValuesFrom(this Rozhodci rozhodci, Rozhodci rozhodciCache)
		{
			rozhodci.FirstName = rozhodciCache.FirstName;
			rozhodci.LastName = rozhodciCache.LastName;
			rozhodci.BirthDate = rozhodciCache.BirthDate;
			rozhodci.Address = rozhodciCache.Address;
			rozhodci.City = rozhodciCache.City;
		}

		public static void CopyValuesFrom(this Settings settings, Settings settingsCache)
		{
			settings.ClubName = settingsCache.ClubName;
			settings.CompetitionName = settingsCache.CompetitionName;
			settings.CompetitionStartDate = settingsCache.CompetitionStartDate;
			settings.CompetitionEndDate = settingsCache.CompetitionEndDate;
			settings.CompetitionStartTime = settingsCache.CompetitionStartTime;
			settings.CompetitionEndTime = settingsCache.CompetitionEndTime;
			settings.CompetitionPlace = settingsCache.CompetitionPlace;
			settings.IsClubNameEnabled = settingsCache.IsClubNameEnabled;
			settings.IsCompetitionDateEnabled = settingsCache.IsCompetitionDateEnabled;
			settings.IsCompetitionNameEnabled= settingsCache.IsCompetitionNameEnabled;
			settings.IsCompetitionPlaceEnabled= settingsCache.IsCompetitionPlaceEnabled;
			settings.IsCompetitionTimeEnabled= settingsCache.IsCompetitionTimeEnabled;
			settings.DbSettings.ServerAddress = settingsCache.DbSettings.ServerAddress;
			settings.DbSettings.Database = settingsCache.DbSettings.Database;
			settings.DbSettings.Username = settingsCache.DbSettings.Username;
			settings.DbSettings.Password = settingsCache.DbSettings.Password;
		}
	}
}
