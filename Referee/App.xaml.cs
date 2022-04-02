using System.Windows;
using Referee.Infrastructure;
using Referee.Infrastructure.SettingsFd;
using Referee.ViewModels;

namespace Referee
{
	/// <summary>
	/// Interakční logika pro App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override async void OnStartup(StartupEventArgs e)
		{
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
