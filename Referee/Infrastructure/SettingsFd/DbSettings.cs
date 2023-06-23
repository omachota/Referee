namespace Referee.Infrastructure.SettingsFd
{
	public class DbSettings : AbstractNotifyPropertyChanged
	{
		private string _serverAddress = "";
		private string _username = "";
		private string _database = "";
		private string _password = "";
		private bool _externalDb;

		public string ServerAddress
		{
			get => _serverAddress;
			set => SetAndRaise(ref _serverAddress, value);
		}

		public string Username
		{
			get => _username;
			set => SetAndRaise(ref _username, value);
		}

		public string Database
		{
			get => _database;
			set => SetAndRaise(ref _database, value);
		}

		public string Password
		{
			get => _password;
			set => SetAndRaise(ref _password, value);
		}

		public bool ExternalDb
		{
			get => _externalDb;
			set => SetAndRaise(ref _externalDb, value);
		}

		public override string ToString()
		{
			return _externalDb
				? $"Server={_serverAddress};Database={_database};uid={_username};pwd={_password};"
				: $"Data Source={Constants.DatabasePath}";
		}
	}
}
