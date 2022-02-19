using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Referee.Infrastructure.SettingsFd
{
	public static class SettingsHelper
	{
		private static readonly string FilePath = Path.Combine(Constants.WorkingDirectory, "Settings.json");

		public static async Task SaveSettingsAsync(Settings settings)
		{
			await CheckFolderAndFile();

			var content = JsonConvert.SerializeObject(settings);

			await using (var streamWriter = new StreamWriter(FilePath, false))
			{
				await streamWriter.WriteAsync(content);
				await streamWriter.FlushAsync();
			}
		}

		public static async Task<Settings> LoadSettings()
		{
			await CheckFolderAndFile();

			string content;

			using (var streamReader = new StreamReader(FilePath))
			{
				content = await streamReader.ReadToEndAsync();
			}

			var settings = JsonConvert.DeserializeObject<Settings>(content);
			if (settings != null)
			{
				if (settings.CompetitionStartDate < DateTime.Today)
				{
					settings.CompetitionStartDate = DateTime.Today;
				}
				
				if (settings.CompetitionEndDate < DateTime.Today)
				{
					settings.CompetitionEndDate = DateTime.Today;
				}
			}

			return settings;
		}

		private static async Task CheckFolderAndFile()
		{
			if (!Directory.Exists(Constants.WorkingDirectory))
				Directory.CreateDirectory(Constants.WorkingDirectory);

			if (!File.Exists(FilePath))
			{
				await using (var streamWriter = new StreamWriter(FilePath))
				{
					await streamWriter.WriteAsync(JsonConvert.SerializeObject(Constants.DefaultSettings));
					await streamWriter.FlushAsync();
				}
			}
		}
	}
}
