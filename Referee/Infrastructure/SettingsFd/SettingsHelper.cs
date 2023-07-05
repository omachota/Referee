using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Referee.Infrastructure.SettingsFd
{
	public static class SettingsHelper
	{
		public static async Task SaveSettingsAsync(Settings settings)
		{
			await CheckFolderAndFile();

			var content = JsonConvert.SerializeObject(settings);

			await using (var streamWriter = new StreamWriter(Constants.SettingsPath, false))
			{
				await streamWriter.WriteAsync(content);
				await streamWriter.FlushAsync();
			}
		}

		public static async Task<Settings> LoadSettings()
		{
			await CheckFolderAndFile();

			string content;

			using (var streamReader = new StreamReader(Constants.SettingsPath))
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

			if (!File.Exists(Constants.SettingsPath))
			{
				await using (var streamWriter = new StreamWriter(Constants.SettingsPath))
				{
					await streamWriter.WriteAsync(JsonConvert.SerializeObject(Constants.DefaultSettings));
					await streamWriter.FlushAsync();
				}
			}
		}
	}
}
