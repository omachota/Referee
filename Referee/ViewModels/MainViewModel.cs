using System.Windows;
using System.Windows.Input;
using Referee.Infrastructure;
using Referee.Infrastructure.WindowNavigation;

namespace Referee.ViewModels
{
	public class MainViewModel : BaseViewModel
	{
		private bool _isInfoOpen;

		public ICommand ExitApplication { get; }
		public ICommand OpenInfo { get; }
		public ICommand CloseInfo { get; }

		public WindowManager WindowManager { get; set; }

		public MainViewModel()
		{
			WindowManager = new WindowManager();
			ExitApplication = new Command(() =>  Application.Current.Shutdown());
			OpenInfo = new Command(() => IsInfoOpen = true);
			CloseInfo = new Command(() => IsInfoOpen = false);
		}

		public bool IsInfoOpen
		{
			get => _isInfoOpen;
			set => SetAndRaise(ref _isInfoOpen, value);
		}

	}
}
