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
			// TODO : create a local database file
			
			var settings = await SettingsHelper.LoadSettings();

			Window window = new MainWindow(new MainViewModel(settings));
			window.Show();

			base.OnStartup(e);
#if !DEBUG
			await Updater.CheckVersion().ConfigureAwait(false);
#endif
		}
	}
}
