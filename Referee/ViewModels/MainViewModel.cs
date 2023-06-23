using System.Threading.Tasks;
using System.Windows.Input;
using Referee.Infrastructure;
using Referee.Infrastructure.SettingsFd;
using Referee.Infrastructure.WindowNavigation;

namespace Referee.ViewModels
{
	public class MainViewModel : BaseViewModel
	{
		private bool _isDialogOpen;
		private bool _isSettingsDialogOpen;
		private bool _isMessageOpen;
		private SettingsViewModel _settingsViewModel;

		public ICommand OpenCloseSettings { get; }

		public Settings Settings { get; }

		public WindowManager WindowManager { get; }

		public MainViewModel(Settings settings)
		{
			var context = new DapperContext(settings.DbSettings);
			SettingsViewModel = new SettingsViewModel(settings);
			WindowManager = new WindowManager(settings, context);
			Settings = settings;
			Updater.NewVersionDetectedEvent += NewVersionDetectedEvent;
			OpenCloseSettings = new Command(() =>
			{
				if (!IsSettingsDialogOpen)
					SettingsViewModel.UpdateChangesMadeValue();
				IsSettingsDialogOpen = !IsSettingsDialogOpen;
			});
#if !DEBUG
			IsSettingsDialogOpen = true;
#endif
		}

		private async Task NewVersionDetectedEvent()
		{
			IsMessageOpen = true;
			await Task.Delay(4500);
			IsMessageOpen = false;
		}

		public bool IsDialogOpen
		{
			get => _isDialogOpen;
			set => SetAndRaise(ref _isDialogOpen, value);
		}

		public bool IsSettingsDialogOpen
		{
			get => _isSettingsDialogOpen;
			set => SetAndRaise(ref _isSettingsDialogOpen, value);
		}

		public bool IsMessageOpen
		{
			get => _isMessageOpen;
			set => SetAndRaise(ref _isMessageOpen, value);
		}

		public string Search
		{
			get => WindowManager.Search;
			set
			{
				if (SetAndRaise(ref WindowManager.Search, value))
				{
					WindowManager.UpdateRegex();
					WindowManager.ActiveViewModel.FilterCollection.Refresh();
				}
			}
		}

		public SettingsViewModel SettingsViewModel
		{
			get => _settingsViewModel;
			set => SetAndRaise(ref _settingsViewModel, value);
		}
	}
}
