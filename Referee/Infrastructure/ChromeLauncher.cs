using Microsoft.Win32;
using System.Diagnostics;

namespace Referee.Infrastructure
{
	internal static class ChromeLauncher
	{
		private const string ChromeAppKey = @"\Software\Microsoft\Windows\CurrentVersion\App Paths\chrome.exe";

		private static string ChromeAppFileName =>
			(string)(Registry.GetValue("HKEY_LOCAL_MACHINE" + ChromeAppKey, "", null) ??
					  Registry.GetValue("HKEY_CURRENT_USER" + ChromeAppKey, "", null));

		public static void OpenLink(string url)
		{
			string chromeAppFileName = ChromeAppFileName;
			if (string.IsNullOrEmpty(chromeAppFileName))
			{
				try
				{
					Process.Start(@"C:\Program Files\Internet Explorer\iexplore.exe", url);
				}
				catch
				{
					// ignored
				}
			}

			Process.Start(chromeAppFileName, url);
		}
	}

}
