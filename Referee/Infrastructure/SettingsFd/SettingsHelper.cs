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

			string content = JsonConvert.SerializeObject(settings);

			using (StreamWriter streamWriter = new StreamWriter(FilePath, false))
			{
				await streamWriter.WriteAsync(content);
				await streamWriter.FlushAsync();
			}
		}

		public static async Task<Settings> LoadSettings()
		{
			await CheckFolderAndFile();

			string content;

			using (StreamReader streamReader = new StreamReader(FilePath))
			{
				content = await streamReader.ReadToEndAsync();
			}

			return JsonConvert.DeserializeObject<Settings>(content);
		}

		private static async Task CheckFolderAndFile()
		{
			if (!Directory.Exists(Constants.WorkingDirectory))
				Directory.CreateDirectory(Constants.WorkingDirectory);

			if (!File.Exists(FilePath))
			{
				using (StreamWriter streamWriter = new StreamWriter(FilePath))
				{
					await streamWriter.WriteAsync(JsonConvert.SerializeObject(Constants.DefaultSettings));
					await streamWriter.FlushAsync();
				}
			}
		}
	}
}
