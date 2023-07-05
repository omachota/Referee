using System;
using System.IO;
using System.Windows;
using Referee.Infrastructure.SettingsFd;
#if !DEBUG
using Referee.Infrastructure.Update;
#endif
using Referee.ViewModels;

namespace Referee
{
	public partial class App : Application
	{
		protected override async void OnStartup(StartupEventArgs e)
		{
			var settings = await SettingsHelper.LoadSettings();
			var db = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Referee.db");
			if (!File.Exists(Constants.DatabasePath))
				File.Copy(db,  Constants.DatabasePath);	

			Window window = new MainWindow(new MainViewModel(settings));
			window.Show();

			base.OnStartup(e);
#if !DEBUG
			await Updater.CheckVersion().ConfigureAwait(false);
#endif
		}
	}
}
