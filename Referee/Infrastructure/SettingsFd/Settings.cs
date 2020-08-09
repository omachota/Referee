using System;
using System.Collections.Generic;

namespace Referee.Infrastructure.SettingsFd
{
	public class Settings
	{
		public List<OptionalColumn> OptionalColumns;

		public string ClubName;

		public string CompetitionName;

		public DateTime CompetitionStartDate;

		public DateTime CompetitionEndDate;

		public string CompetitionPlace;

		public Settings(List<OptionalColumn> optionalColumns, string clubName, string competitionName, DateTime competitionStartDate, DateTime competitionEndDate, string competitionPlace)
		{
			OptionalColumns = optionalColumns;
			ClubName = clubName;
			CompetitionName = competitionName;
			CompetitionStartDate = competitionStartDate;
			CompetitionEndDate = competitionEndDate;
			CompetitionPlace = competitionPlace;
		}
	}
}
