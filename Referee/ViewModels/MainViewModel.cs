using Referee.Infrastructure.SettingsFd;
using Referee.Infrastructure.WindowNavigation;

namespace Referee.ViewModels
{
	public class MainViewModel : BaseViewModel
	{
		private bool _isDialogOpen;
		public Settings Settings { get; }

		public WindowManager WindowManager { get; set; }

		public MainViewModel(Settings settings)
		{
			WindowManager = new WindowManager(settings);
			Settings = settings;
		}

		public bool IsDialogOpen
		{
			get => _isDialogOpen;
			set => SetAndRaise(ref _isDialogOpen, value);
		}

	}
}
