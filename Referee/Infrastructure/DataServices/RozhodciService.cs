using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Referee.Models;

namespace Referee.Infrastructure.DataServices
{
	public class RozhodciService
	{
		private readonly DapperContext _context;

		public RozhodciService(DapperContext context)
		{
			_context = context;
		}

		public async IAsyncEnumerable<Rozhodci> GetRozhodci()
		{
			await using var connection = _context.CreateConnection();
			var reader = await connection.ExecuteReaderAsync("SELECT * FROM Rozhodci");
			var parser = reader.GetRowParser<Rozhodci>();

			while (await reader.ReadAsync())
			{
				yield return parser(reader);
			}

			while (await reader.NextResultAsync().ConfigureAwait(false))
			{
			}
		}

		public async Task<int> AddRozhodci(Rozhodci rozhodci)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@firstName", rozhodci.FirstName.Trim());
			parameters.Add("@lastName", rozhodci.LastName.Trim());
			parameters.Add("@birthDate", rozhodci.BirthDate.ToString("yyyy-M-d"));
			parameters.Add("@address", rozhodci.Address.Trim());
			parameters.Add("@city", rozhodci.City.Trim());
			parameters.Add("@email", rozhodci.Email);
			parameters.Add("@class", rozhodci.Class);
			parameters.Add("@telephoneNumber", rozhodci.TelephoneNumber);
			parameters.Add("@registrationNumber", rozhodci.RegistrationNumber);
			parameters.Add("@bankAccountNumber", rozhodci.BankAccountNumber);
			await using var connection = _context.CreateConnection();
			var id = await connection.QuerySingleAsync<int>(
				"INSERT INTO Rozhodci (FirstName, LastName, BirthDate, Address, City, Email, Class, TelephoneNumber, RegistrationNumber, BankAccountNumber) VALUES (@firstName, @lastName, @birthDate, @address, @city, @email, @class, @telephoneNumber, @registrationNumber, @bankAccountNumber); SELECT last_insert_rowid()",
				parameters);

			return id;
		}

		public async Task UpdateRozhodci(Rozhodci rozhodci)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@firstName", rozhodci.FirstName.Trim());
			parameters.Add("@lastName", rozhodci.LastName.Trim());
			parameters.Add("@birthDate", rozhodci.BirthDate.ToString("yyyy-M-d"));
			parameters.Add("@address", rozhodci.Address.Trim());
			parameters.Add("@city", rozhodci.City.Trim());
			parameters.Add("@email", rozhodci.Email);
			parameters.Add("@class", rozhodci.Class);
			parameters.Add("@telephoneNumber", rozhodci.TelephoneNumber);
			parameters.Add("@registrationNumber", rozhodci.RegistrationNumber);
			parameters.Add("@bankAccountNumber", rozhodci.BankAccountNumber);
			parameters.Add("@id", rozhodci.Id);
			await using var connection = _context.CreateConnection();
			await connection.ExecuteAsync(
				"UPDATE Rozhodci SET FirstName=@firstName, LastName=@lastName, BirthDate=@birthDate, Address=@address, City=@city, Email=@email, Class=@class, TelephoneNumber=@telephoneNumber, RegistrationNumber=@registrationNumber, BankAccountNumber=@bankAccountNumber WHERE Id=@id",
				parameters);
		}

		public async Task DeleteRozhodci(Rozhodci rozhodci)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@id", rozhodci.Id);
			await using var connection = _context.CreateConnection();
			await connection.ExecuteAsync("DELETE FROM Rozhodci WHERE Id=@id", parameters);
		}
	}
}
