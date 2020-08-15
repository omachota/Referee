using System;
using System.Collections.Generic;
using System.Data.Common;
using MySqlConnector;
using Referee.Infrastructure.SettingsFd;
using Referee.Models;

namespace Referee.Infrastructure
{
	public static class RozhodciService
	{
		public static async IAsyncEnumerable<Rozhodci> LoadRozhodciFromDb(Settings settings)
		{
			using (MySqlConnection connection = new MySqlConnection(settings.DbSettings.ToString()))
			{
				await connection.OpenAsync();

				MySqlCommand command = new MySqlCommand("SELECT * FROM Rozhodci", connection);

				DbDataReader reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					yield return new Rozhodci
					{
						Id = int.Parse(reader["Id"].ToString()!),
						FirstName = reader["FirstName"].ToString(),
						LastName = reader["LastName"].ToString(),
						BirthDate = DateTime.Parse(reader["BirthDate"].ToString()!),
						Address = reader["Address"].ToString(),
						City = reader["City"].ToString(),
					};
				}
			}
		}
	}
}
