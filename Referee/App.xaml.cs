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
			Settings settings = await SettingsHelper.LoadSettings();

			Window window = new MainWindow(new MainViewModel(settings));
			window.Show();

			base.OnStartup(e);

			await Updater.CheckVersion().ConfigureAwait(false);
		}
	}
}
