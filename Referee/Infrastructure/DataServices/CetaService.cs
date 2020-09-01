using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using MySqlConnector;
using Referee.Infrastructure.SettingsFd;
using Referee.Models;

namespace Referee.Infrastructure.DataServices
{
	public class CetaService
	{
		private readonly Settings _settings;

		public CetaService(Settings settings)
		{
			_settings = settings;
		}

		public async IAsyncEnumerable<Cetar> LoadCetaFromDb()
		{
			await using (MySqlConnection connection = new MySqlConnection(_settings.DbSettings.ToString()))
			{
				await connection.OpenAsync().ConfigureAwait(false);

				MySqlCommand command = new MySqlCommand("SELECT * FROM Ceta", connection);

				DbDataReader reader = await command.ExecuteReaderAsync(new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token)
				                                   .ConfigureAwait(false);
				while (await reader.ReadAsync().ConfigureAwait(false))
				{
					yield return new Cetar
					{
						Id = int.Parse(reader["Id"].ToString()!),
						FirstName = reader["FirstName"].ToString(),
						LastName = reader["LastName"].ToString(),
						BirthDate = DateTime.Parse(reader["BirthDate"].ToString()!),
						Address = reader["Address"].ToString(),
						City = reader["City"].ToString(),
					};
				}

				await connection.CloseAsync().ConfigureAwait(false);
			}
		}

		public async Task<Cetar> AddNewCerat(Cetar cetar)
		{
			await using (MySqlConnection connection = new MySqlConnection(_settings.DbSettings.ToString()))
			{
				await connection.OpenAsync().ConfigureAwait(false);

				MySqlCommand command = new MySqlCommand(
					"INSERT INTO Ceta (FirstName, LastName, BirthDate, Address, City) VALUES (@firstName, @lastName, @birthDate, @address, @city)",
					connection);
				command.Parameters.AddWithValue("@firstName", cetar.FirstName.Trim());
				command.Parameters.AddWithValue("@lastName", cetar.LastName.Trim());
				command.Parameters.AddWithValue("@birthDate", cetar.BirthDate);
				command.Parameters.AddWithValue("@address", cetar.Address.Trim());
				command.Parameters.AddWithValue("@city", cetar.City.Trim());

				await command.ExecuteNonQueryAsync().ConfigureAwait(false);

				MySqlCommand getCommand = new MySqlCommand("select * FROM Ceta where Id=(SELECT LAST_INSERT_ID());", connection);

				DbDataReader reader = await getCommand.ExecuteReaderAsync().ConfigureAwait(false);
				Cetar r = Cetar.CreateEmpty();
				while (await reader.ReadAsync().ConfigureAwait(false))
				{
					r = new Cetar
					{
						Id = int.Parse(reader["Id"].ToString()!),
						FirstName = reader["FirstName"].ToString(),
						LastName = reader["LastName"].ToString(),
						BirthDate = DateTime.Parse(reader["BirthDate"].ToString()!),
						Address = reader["Address"].ToString(),
						City = reader["City"].ToString(),
					};
					break;
				}

				await connection.CloseAsync().ConfigureAwait(false);

				return r;
			}
		}

		public async Task UpdateCetarInDatabase(Cetar cetar)
		{
			await using (MySqlConnection connection = new MySqlConnection(_settings.DbSettings.ToString()))
			{
				await connection.OpenAsync().ConfigureAwait(false);

				MySqlCommand command = new MySqlCommand(
					"UPDATE Ceta SET FirstName=@firstName, LastName=@lastName, BirthDate=@birthDate, Address=@address, City=@city WHERE Id=@id",
					connection);
				command.Parameters.AddWithValue("@firstName", cetar.FirstName.Trim());
				command.Parameters.AddWithValue("@lastName", cetar.LastName.Trim());
				command.Parameters.AddWithValue("@birthDate", cetar.BirthDate);
				command.Parameters.AddWithValue("@address", cetar.Address.Trim());
				command.Parameters.AddWithValue("@city", cetar.City.Trim());
				command.Parameters.AddWithValue("@id", cetar.Id);

				await command.ExecuteNonQueryAsync().ConfigureAwait(false);

				await connection.CloseAsync().ConfigureAwait(false);
			}
		}

		public async Task DeleteCetarFromDatabase(Cetar cetar)
		{
			await using (MySqlConnection connection = new MySqlConnection(_settings.DbSettings.ToString()))
			{
				await connection.OpenAsync().ConfigureAwait(false);

				MySqlCommand command = new MySqlCommand("DELETE FROM Ceta WHERE Id=@id", connection);
				command.Parameters.AddWithValue("@id", cetar.Id);

				await command.ExecuteNonQueryAsync().ConfigureAwait(false);
			}
		}
	}
}
