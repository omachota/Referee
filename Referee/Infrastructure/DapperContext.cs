using System.Data.Common;
using System.Data.SQLite;
using MySqlConnector;
using Referee.Infrastructure.SettingsFd;

namespace Referee.Infrastructure;

public class DapperContext
{
	private readonly DbSettings _settings;

	public DapperContext(DbSettings settings)
	{
		_settings = settings;
	}

	public DbConnection CreateConnection()
	{
		if (_settings.ExternalDb)
			return new MySqlConnection(_settings.ToString());
		// check database
		return new SQLiteConnection(_settings.ToString());
	}
}
