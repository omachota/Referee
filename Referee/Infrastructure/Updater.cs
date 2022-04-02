using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
			HttpResponseMessage respose;
			var client = new HttpClient();
			client.DefaultRequestHeaders.Add("User-Agent", "request");
			try
			{
				respose = await client.GetAsync("https://api.github.com/repos/omachota/Referee/releases").ConfigureAwait(false);
			}
			catch
			{
				respose = new HttpResponseMessage(HttpStatusCode.Forbidden);
			}

			if (respose.IsSuccessStatusCode)
			{
				var content = await respose.Content.ReadAsStringAsync().ConfigureAwait(false);
				var json = JsonConvert.DeserializeObject<List<GithubAssets>>(content);
				if (json != null)
				{
					var downloadUrl = json[0].Assets[0].BrowserDownloadUrl.ToString();

					var assembly = Assembly.GetExecutingAssembly();
					var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
					var version = fvi.FileVersion;

					if (!string.IsNullOrEmpty(version) && !downloadUrl.Contains(version))
					{
						await NewVersionDetectedEvent.Invoke(); // TODO: await?
						try
						{
							var path = Constants.WorkingDirectory + "\\" + downloadUrl.Split('/')[^1];
							
							using (var httpClient = new HttpClient())
							{
								var stream = await httpClient.GetStreamAsync(new Uri(downloadUrl)).ConfigureAwait(false);
								await using (var writer = File.Create(path))
								{
									await stream.CopyToAsync(writer);
								}
							}

							Process.Start(path);
						}
						catch (Exception)
						{
							// ignored
						}
					}
				}
			}

			client.Dispose();
		}
	}
}
