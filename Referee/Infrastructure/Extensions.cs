using System;
using Referee.Infrastructure.SettingsFd;
using Referee.Models;

namespace Referee.Infrastructure
{
	public static class Extensions
	{
		public static bool ValidatePerson<T>(T person) where T : IPerson
		{
			if (person == null)
				return false;
			if (person.FirstName == "")
				return false;
			if (person.LastName == "")
				return false;
			if (person.Address == "")
				return false;
			if (person.City == "")
				return false;
			if (person.BirthDate >= DateTime.Today)
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
			rozhodci.Email = rozhodciCache.Email;
			rozhodci.Class = rozhodciCache.Class;
			rozhodci.TelephoneNumber = rozhodciCache.TelephoneNumber;
			rozhodci.RegistrationNumber = rozhodciCache.RegistrationNumber;
			rozhodci.BankAccountNumber = rozhodciCache.BankAccountNumber;
		}

		public static void CopyValuesFrom(this Cetar cetar, Cetar cetarCache)
		{
			cetar.FirstName = cetarCache.FirstName;
			cetar.LastName = cetarCache.LastName;
			cetar.BirthDate = cetarCache.BirthDate;
			cetar.Address = cetarCache.Address;
			cetar.City = cetarCache.City;
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
