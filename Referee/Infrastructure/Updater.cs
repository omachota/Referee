using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Referee.Infrastructure.SettingsFd;

namespace Referee.Infrastructure
{
	public static class Updater
	{
		public delegate Task NewVersionDetected();

		public static NewVersionDetected NewVersionDetectedEvent;


		// GET https://api.github.com/repos/omachota/Referee/releases
		public static async Task CheckVersion()
		{
			HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Add("User-Agent", "request");
			HttpResponseMessage respose = await client.GetAsync("https://api.github.com/repos/omachota/Referee/releases").ConfigureAwait(false);
			if (respose.IsSuccessStatusCode)
			{
				var content = await respose.Content.ReadAsStringAsync().ConfigureAwait(false);
				var json = JsonConvert.DeserializeObject<List<Temperatures>>(content);
				string downloadUrl = json[0].Assets[0].BrowserDownloadUrl.ToString();

				Assembly assembly = Assembly.GetExecutingAssembly();
				FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
				string version = fvi.FileVersion;

				if (!downloadUrl.Contains(version))
				{
#pragma warning disable 4014
					NewVersionDetectedEvent.Invoke();
#pragma warning restore 4014

					using (WebClient myWebClient = new WebClient())
					{
						await myWebClient.DownloadFileTaskAsync(new Uri(downloadUrl), Constants.WorkingDirectory + "\\" + downloadUrl.Split('/')[^1])
						                 .ConfigureAwait(false);
					}

					Process.Start("explorer.exe", Constants.WorkingDirectory);
				}
			}
			else
			{
				await Task.Delay(TimeSpan.FromSeconds(30)).ConfigureAwait(false);
				await CheckVersion().ConfigureAwait(false);
			}
		}
	}
}
