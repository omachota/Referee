using System;
using System.Collections.Generic;
using System.Data.Common;
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
			await using (MySqlConnection connection = new MySqlConnection(_settings.DbSettings.ToString()))
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
						Email = reader["Email"].ToString(),
						Class = reader["Class"].ToString(),
						TelephoneNumber = reader["TelephoneNumber"].ToString(),
						RegistrationNumber = reader["RegistrationNumber"].ToString(),
						BankAccountNumber = reader["BankAccountNumber"].ToString()
					};
				}

				await connection.CloseAsync();
			}
		}

		public async Task UpdateRozhodciInDatabase(Rozhodci rozhodci)
		{
			await using (MySqlConnection connection = new MySqlConnection(_settings.DbSettings.ToString()))
			{
				await connection.OpenAsync();

				MySqlCommand command = new MySqlCommand(
					"UPDATE Rozhodci SET FirstName=@firstName, LastName=@lastName, BirthDate=@birthDate, Address=@address, City=@city, Email=@email, Class=@class, TelephoneNumber=@telephoneNumber, RegistrationNumber=@registrationNumber, BankAccountNumber=@bankAccountNumber WHERE Id=@id",
					connection);
				command.Parameters.AddWithValue("@firstName", rozhodci.FirstName);
				command.Parameters.AddWithValue("@lastName", rozhodci.LastName);
				command.Parameters.AddWithValue("@birthDate", rozhodci.BirthDate);
				command.Parameters.AddWithValue("@address", rozhodci.Address);
				command.Parameters.AddWithValue("@city", rozhodci.City);
				command.Parameters.AddWithValue("@email", rozhodci.Email);
				command.Parameters.AddWithValue("@class", rozhodci.Class);
				command.Parameters.AddWithValue("@telephoneNumber", rozhodci.TelephoneNumber);
				command.Parameters.AddWithValue("@registrationNumber", rozhodci.RegistrationNumber);
				command.Parameters.AddWithValue("@bankAccountNumber", rozhodci.BankAccountNumber);
				command.Parameters.AddWithValue("@id", rozhodci.Id);

				await command.ExecuteNonQueryAsync();

				await connection.CloseAsync();
			}
		}

		public async Task DeleteRozhodciFromDatabase(Rozhodci rozhodci)
		{
			await using (MySqlConnection connection = new MySqlConnection(_settings.DbSettings.ToString()))
			{
				await connection.OpenAsync();

				MySqlCommand command = new MySqlCommand("DELETE FROM Rozhodci WHERE Id=@id", connection);
				command.Parameters.AddWithValue("@id", rozhodci.Id);

				await command.ExecuteNonQueryAsync();
			}
		}
	}
}
