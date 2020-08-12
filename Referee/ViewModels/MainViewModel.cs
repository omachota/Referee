using System.Windows.Input;
using Referee.Infrastructure;
using Referee.Infrastructure.SettingsFd;
using Referee.Infrastructure.WindowNavigation;

namespace Referee.ViewModels
{
	public class MainViewModel : BaseViewModel
	{
		private bool _isDialogOpen;
		public ICommand OpenDialog { get; }
		public ICommand CloseDialog { get; }

		public WindowManager WindowManager { get; set; }

		public MainViewModel(Settings settings)
		{
			WindowManager = new WindowManager(settings);
			OpenDialog = new Command(() => IsDialogOpen = true);
			CloseDialog = new Command(() => IsDialogOpen = false);
		}

		public bool IsDialogOpen
		{
			get => _isDialogOpen;
			set => SetAndRaise(ref _isDialogOpen, value);
		}

	}
}
