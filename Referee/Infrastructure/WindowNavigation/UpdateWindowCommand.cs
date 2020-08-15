using System;
using System.Windows.Input;
using Referee.Infrastructure.Print;
using Referee.Infrastructure.SettingsFd;
using Referee.ViewModels;

namespace Referee.Infrastructure.WindowNavigation
{
	public class UpdateWindowCommand : ICommand
	{
		private readonly WindowManager _windowManager;
		private readonly Settings _settings;
		private readonly Printer _printer;

		public UpdateWindowCommand(WindowManager windowManager, Settings settings, Printer printer)
		{
			_windowManager = windowManager;
			_settings = settings;
			_printer = printer;
		}

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public async void Execute(object parameter)
		{
			if (parameter is ViewType viewType && _windowManager.ViewType != viewType)
			{
				await SettingsHelper.SaveSettingsAsync(_settings);

				switch (viewType)
				{
					case ViewType.Rozhodci:
						_windowManager.ActiveViewModel = new RozhodciViewModel(_settings, _printer);
						break;
					case ViewType.Ceta:
						_windowManager.ActiveViewModel = new CetaViewModel(_settings, _printer);
						break;
					case ViewType.Settings:
						_windowManager.ActiveViewModel = new SettingsViewModel(_settings);
						break;
				}

				_windowManager.ViewType = viewType;
			}
		}
	}
}
