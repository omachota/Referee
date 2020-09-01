using System.Threading.Tasks;
using Referee.Infrastructure;
using Referee.Infrastructure.SettingsFd;
using Referee.Infrastructure.WindowNavigation;

namespace Referee.ViewModels
{
	public class MainViewModel : BaseViewModel
	{
		private bool _isDialogOpen;
		private bool _isMessageOpen;
		public Settings Settings { get; }

		public WindowManager WindowManager { get; set; }

		public MainViewModel(Settings settings)
		{
			WindowManager = new WindowManager(settings);
			Settings = settings;
			Updater.NewVersionDetectedEvent += NewVersionDetectedEvent;
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

		public bool IsMessageOpen
		{
			get => _isMessageOpen;
			set => SetAndRaise(ref _isMessageOpen, value);
		}
	}
}
