using System;
using System.Windows.Input;
using Referee.ViewModels;

namespace Referee.Infrastructure.WindowNavigation
{
	public class UpdateWindowCommand : ICommand
	{
		private readonly WindowManager _windowManager;

		public UpdateWindowCommand(WindowManager windowManager)
		{
			_windowManager = windowManager;
		}

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			if (parameter is ViewType viewType && _windowManager.ViewType != viewType)
			{
				switch (viewType)
				{
					case ViewType.Rozhodci:
						_windowManager.ActiveViewModel = new RozhodciViewModel();
						break;
					case ViewType.Ceta:
						_windowManager.ActiveViewModel = new CetaUserControl();
						break;
					case ViewType.Settings:
						_windowManager.ActiveViewModel = new CetaUserControl();
						break;
				}

				_windowManager.ViewType = viewType;
			}
		}
	}
}
