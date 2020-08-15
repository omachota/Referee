using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MySqlConnector;
using Referee.Infrastructure.Print;
using Referee.Infrastructure.SettingsFd;

namespace Referee.ViewModels
{
	public class CetaViewModel : BaseViewModel
	{
		public CetaViewModel(Settings settings, Printer printer)
		{
			Debug.WriteLine(printer);

			Task.Run(() =>
			{
				using (MySqlConnection con = new MySqlConnection(settings.DbSettings.ToString()))
				{
					try
					{
						con.Open();
						Debug.WriteLine("Connected");
					}
					catch (Exception e)
					{
						Debug.WriteLine(e);
					}
				}
			});

		}
	}
}
