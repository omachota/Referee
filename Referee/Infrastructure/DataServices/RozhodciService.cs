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
	public class RozhodciService
	{
		private readonly Settings _settings;

		public RozhodciService(Settings settings)
		{
			_settings = settings;
		}

		public async IAsyncEnumerable<Rozhodci> LoadRozhodciFromDb()
		{
			await using (var connection = new MySqlConnection(_settings.DbSettings.ToString()))
			{
				await connection.OpenAsync().ConfigureAwait(false);

				var command = new MySqlCommand("SELECT * FROM Rozhodci", connection);

				DbDataReader reader = await command.ExecuteReaderAsync(new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token)
				                                   .ConfigureAwait(false);
				while (await reader.ReadAsync().ConfigureAwait(false))
				{
					yield return new Rozhodci
					{
						Id = int.Parse(reader["Id"].ToString()!),
						FirstName = reader["FirstName"].ToString(),
						LastName = reader["LastName"].ToString(),
						BirthDate = DateTime.Parse(reader["BirthDate"].ToString()!),
						Address = reader["Address"].ToString(),
						City = reader["City"].ToString(),
						Email = reader["Email"].ToString(),
						Class = reader["Class"].ToString(),
						TelephoneNumber = reader["TelephoneNumber"].ToString(),
						RegistrationNumber = reader["RegistrationNumber"].ToString(),
						BankAccountNumber = reader["BankAccountNumber"].ToString()
					};
				}

				await connection.CloseAsync().ConfigureAwait(false);
			}
		}

		public async Task<Rozhodci> AddNewRozhodci(Rozhodci rozhodci)
		{
			await using (var connection = new MySqlConnection(_settings.DbSettings.ToString()))
			{
				await connection.OpenAsync().ConfigureAwait(false);

				var command = new MySqlCommand(
					"INSERT INTO Rozhodci (FirstName, LastName, BirthDate, Address, City, Email, Class, TelephoneNumber, RegistrationNumber, BankAccountNumber) VALUES (@firstName, @lastName, @birthDate, @address, @city, @email, @class, @telephoneNumber, @registrationNumber, @bankAccountNumber)",
					connection);
				command.Parameters.AddWithValue("@firstName", rozhodci.FirstName.Trim());
				command.Parameters.AddWithValue("@lastName", rozhodci.LastName.Trim());
				command.Parameters.AddWithValue("@birthDate", rozhodci.BirthDate);
				command.Parameters.AddWithValue("@address", rozhodci.Address.Trim());
				command.Parameters.AddWithValue("@city", rozhodci.City.Trim());
				command.Parameters.AddWithValue("@email", rozhodci.Email.Trim());
				command.Parameters.AddWithValue("@class", rozhodci.Class);
				command.Parameters.AddWithValue("@telephoneNumber", rozhodci.TelephoneNumber);
				command.Parameters.AddWithValue("@registrationNumber", rozhodci.RegistrationNumber);
				command.Parameters.AddWithValue("@bankAccountNumber", rozhodci.BankAccountNumber);

				await command.ExecuteNonQueryAsync().ConfigureAwait(false);

				var getCommand = new MySqlCommand("select * FROM Rozhodci where Id=(SELECT LAST_INSERT_ID());", connection);

				DbDataReader reader = await getCommand.ExecuteReaderAsync().ConfigureAwait(false);
				var r = Rozhodci.CreateEmpty();
				while (await reader.ReadAsync().ConfigureAwait(false))
				{
					r = new Rozhodci
					{
						Id = int.Parse(reader["Id"].ToString()!),
						FirstName = reader["FirstName"].ToString(),
						LastName = reader["LastName"].ToString(),
						BirthDate = DateTime.Parse(reader["BirthDate"].ToString()!),
						Address = reader["Address"].ToString(),
						City = reader["City"].ToString(),
						Email = reader["Email"].ToString(),
						Class = reader["Class"].ToString(),
						TelephoneNumber = reader["TelephoneNumber"].ToString(),
						RegistrationNumber = reader["RegistrationNumber"].ToString(),
						BankAccountNumber = reader["BankAccountNumber"].ToString()
					};
					break;
				}

				await connection.CloseAsync().ConfigureAwait(false);

				return r;
			}
		}

		public async Task UpdateRozhodciInDatabase(Rozhodci rozhodci)
		{
			await using (var connection = new MySqlConnection(_settings.DbSettings.ToString()))
			{
				await connection.OpenAsync().ConfigureAwait(false);

				var command = new MySqlCommand(
					"UPDATE Rozhodci SET FirstName=@firstName, LastName=@lastName, BirthDate=@birthDate, Address=@address, City=@city, Email=@email, Class=@class, TelephoneNumber=@telephoneNumber, RegistrationNumber=@registrationNumber, BankAccountNumber=@bankAccountNumber WHERE Id=@id",
					connection);
				command.Parameters.AddWithValue("@firstName", rozhodci.FirstName.Trim());
				command.Parameters.AddWithValue("@lastName", rozhodci.LastName.Trim());
				command.Parameters.AddWithValue("@birthDate", rozhodci.BirthDate);
				command.Parameters.AddWithValue("@address", rozhodci.Address.Trim());
				command.Parameters.AddWithValue("@city", rozhodci.City.Trim());
				command.Parameters.AddWithValue("@email", rozhodci.Email.Trim());
				command.Parameters.AddWithValue("@class", rozhodci.Class);
				command.Parameters.AddWithValue("@telephoneNumber", rozhodci.TelephoneNumber);
				command.Parameters.AddWithValue("@registrationNumber", rozhodci.RegistrationNumber);
				command.Parameters.AddWithValue("@bankAccountNumber", rozhodci.BankAccountNumber);
				command.Parameters.AddWithValue("@id", rozhodci.Id);

				await command.ExecuteNonQueryAsync().ConfigureAwait(false);

				await connection.CloseAsync().ConfigureAwait(false);
			}
		}

		public async Task DeleteRozhodciFromDatabase(Rozhodci rozhodci)
		{
			await using (var connection = new MySqlConnection(_settings.DbSettings.ToString()))
			{
				await connection.OpenAsync().ConfigureAwait(false);

				var command = new MySqlCommand("DELETE FROM Rozhodci WHERE Id=@id", connection);
				command.Parameters.AddWithValue("@id", rozhodci.Id);

				await command.ExecuteNonQueryAsync().ConfigureAwait(false);
			}
		}
	}
}
