using System.Diagnostics;

namespace Referee.Infrastructure
{
	internal static class Browser
	{
		public static void OpenLink(string url)
		{
			url = url.Replace("&", "^&");
			// Process.Start(new ProcessStartInfo(new Uri(url).ToString()) { UseShellExecute = true });
			Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
		}
		
	}

}
