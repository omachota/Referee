using System.Windows.Input;
using Referee.ViewModels;

namespace Referee.Infrastructure.WindowNavigation
{
	public enum ViewType
	{
		Rozhodci,
		Ceta,
		Settings
	}

	public class WindowManager : AbstractNotifyPropertyChanged, IWindowManager
	{
		private BaseViewModel _activeViewModel;

		public WindowManager()
		{
			_activeViewModel = new RozhodciViewModel();
		}

		public ViewType ViewType { get; set; }

		public ICommand UpdateWindowCommand => new UpdateWindowCommand(this);

		public BaseViewModel ActiveViewModel
		{
			get => _activeViewModel;
			set => SetAndRaise(ref _activeViewModel, value);
		}
	}
}
