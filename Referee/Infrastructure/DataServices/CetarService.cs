using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Referee.Infrastructure.SettingsFd;
using Referee.Models;

namespace Referee.Infrastructure.DataServices
{
	public class CetarService
	{
		private readonly DapperContext _context;
		private readonly DbSettings _dbSettings;

		public CetarService(DapperContext context, DbSettings dbSettings)
		{
			_context = context;
			_dbSettings = dbSettings;
		}

		public async IAsyncEnumerable<Cetar> GetCeta()
		{
			await using var connection = _context.CreateConnection();
			var reader = await connection.ExecuteReaderAsync("SELECT * FROM Ceta");
			var parser = reader.GetRowParser<Cetar>();

			while (await reader.ReadAsync())
			{
				yield return parser(reader);
			}

			while (await reader.NextResultAsync().ConfigureAwait(false))
			{
			}
		}

		public async Task<int> AddCetar(Cetar cetar)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@firstName", cetar.FirstName.Trim());
			parameters.Add("@lastName", cetar.LastName.Trim());
			parameters.Add("@birthDate", cetar.BirthDate.ToString("yyyy-M-d"));
			parameters.Add("@address", cetar.Address.Trim());
			parameters.Add("@city", cetar.City.Trim());

			await using var connection = _context.CreateConnection();
			var id = await connection.QuerySingleOrDefaultAsync<int>(
				$"INSERT INTO Ceta (FirstName, LastName, BirthDate, Address, City) VALUES (@firstName, @lastName, @birthDate, @address, @city); SELECT {DatabaseExtension.LastId(_dbSettings.ExternalDb)}",
				parameters);

			return id;
		}

		public async Task UpdateCetar(Cetar cetar)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@firstName", cetar.FirstName.Trim(), DbType.String);
			parameters.Add("@lastName", cetar.LastName.Trim(), DbType.String);
			parameters.Add("@birthDate", cetar.BirthDate.ToString("yyyy-M-d"), DbType.String);
			parameters.Add("@address", cetar.Address.Trim(), DbType.String);
			parameters.Add("@city", cetar.City.Trim(), DbType.String);
			parameters.Add("@id", cetar.Id);

			await using var connection = _context.CreateConnection();
			await connection.ExecuteAsync(
				"UPDATE Ceta SET FirstName=@firstName, LastName=@lastName, BirthDate=@birthDate, Address=@address, City=@city WHERE Id=@id",
				parameters);
		}

		public async Task DeleteCetar(Cetar cetar)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@id", cetar.Id);
			await using var connection = _context.CreateConnection();
			await connection.ExecuteAsync("DELETE FROM Ceta WHERE Id=@id", parameters);
		}
	}
}
