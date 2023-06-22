using System;
using System.Diagnostics;
using System.Text;
using System.Windows;

namespace Referee.Infrastructure
{
	internal static class Browser
	{
		public static void OpenLink(string url)
		{
			var sb = new StringBuilder();
			sb.Append("file:///");
			sb.Append(url.Replace("\\", "/"));
			try
			{
				Process.Start(new ProcessStartInfo(sb.ToString()) { UseShellExecute = true });
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, "PDF error");
			}
		}
	}
}
