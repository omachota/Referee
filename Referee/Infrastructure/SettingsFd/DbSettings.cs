
namespace Referee.Infrastructure.SettingsFd
{
	public class DbSettings : AbstractNotifyPropertyChanged
	{
		private string _serverAddress;
		private string _username;
		private string _database;
		private string _password;

		public DbSettings(string serverAddress, string username, string database, string password)
		{
			_serverAddress = serverAddress;
			_username = username;
			_database = database;
			_password = password;
		}

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

		public override string ToString()
		{
			return $"Server={_serverAddress};Database={_database};uid={_username};pwd={_password};";
		}
	}
}
