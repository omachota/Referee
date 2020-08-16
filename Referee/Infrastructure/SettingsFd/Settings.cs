using System;

namespace Referee.Infrastructure.SettingsFd
{
	public class Settings : AbstractNotifyPropertyChanged
	{
		public Settings()
		{

		}

		public Settings(string clubName, string competitionName, DateTime competitionStartDate,
		                DateTime competitionEndDate, string competitionPlace, DbSettings dbSettings)
		{
			ClubName = clubName;
			CompetitionName = competitionName;
			CompetitionStartDate = competitionStartDate;
			CompetitionEndDate = competitionEndDate;
			CompetitionPlace = competitionPlace;
			DbSettings = dbSettings;
		}

		public DbSettings DbSettings { get; set; }

		// public List<OptionalColumn> OptionalColumns;

		#region ClubName

		private string _clubName;
		private bool _isClubNameEnabled;

		public string ClubName
		{
			get => _clubName;
			set => SetAndRaise(ref _clubName, value);
		}

		public bool IsClubNameEnabled
		{
			get => _isClubNameEnabled;
			set => SetAndRaise(ref _isClubNameEnabled, value);
		}

		#endregion

		#region CompetitionName

		private string _competitionName;
		private bool _isCompetitionNameEnabled;

		public string CompetitionName
		{
			get => _competitionName;
			set => SetAndRaise(ref _competitionName, value);
		}

		public bool IsCompetitionNameEnabled
		{
			get => _isCompetitionNameEnabled;
			set => SetAndRaise(ref _isCompetitionNameEnabled, value);
		}

		#endregion

		#region CompetitionDate

		private DateTime? _competitionStartDate;
		private DateTime? _competitionEndDate;
		private bool _isCompetitionDateEnabled;

		public DateTime? CompetitionStartDate
		{
			get => _competitionStartDate;
			set => SetAndRaise(ref _competitionStartDate, value);
		}

		public DateTime? CompetitionEndDate
		{
			get => _competitionEndDate;
			set => SetAndRaise(ref _competitionEndDate, value);
		}

		public bool IsCompetitionDateEnabled
		{
			get => _isCompetitionDateEnabled;
			set => SetAndRaise(ref _isCompetitionDateEnabled, value);
		}

		#endregion

		#region CompetitionTime

		private DateTime? _competitionStartTime;
		private DateTime? _competitionEndTime;
		private bool _isIsCompetitionTimeEnabled;

		public DateTime? CompetitionStartTime
		{
			get => _competitionStartTime;
			set => SetAndRaise(ref _competitionStartTime, value);
		}

		public DateTime? CompetitionEndTime
		{
			get => _competitionEndTime;
			set => SetAndRaise(ref _competitionEndTime, value);
		}

		public bool IsCompetitionTimeEnabled
		{
			get => _isIsCompetitionTimeEnabled;
			set => SetAndRaise(ref _isIsCompetitionTimeEnabled, value);
		}

		#endregion

		#region CompetitionPlace

		private string _competitionPlace;
		private bool _isCompetitionPlaceEnabled;

		public string CompetitionPlace
		{
			get => _competitionPlace;
			set => SetAndRaise(ref _competitionPlace, value);
		}

		public bool IsCompetitionPlaceEnabled
		{
			get => _isCompetitionPlaceEnabled;
			set => SetAndRaise(ref _isCompetitionPlaceEnabled, value);
		}

		#endregion
	}
}
