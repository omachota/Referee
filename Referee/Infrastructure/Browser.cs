using System;
using System.Diagnostics;
using System.Text;
using System.Windows;
using Serilog;

namespace Referee.Infrastructure
{
	internal static class Browser
	{
		public static void OpenLink(string url, ILogger logger)
		{
			logger = logger.ForContext(typeof(Browser));
			var sb = new StringBuilder();
			sb.Append("file:///");
			sb.Append(url.Replace("\\", "/"));
			try
			{
				Process.Start(new ProcessStartInfo(sb.ToString()) { UseShellExecute = true });
			}
			catch (Exception e)
			{
				logger.Error("Cannot open link in browser");
				MessageBox.Show(e.Message, "PDF error");
			}
		}
	}
}
