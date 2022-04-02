using System;
using System.Windows.Input;
using Referee.Infrastructure.DataServices;
using Referee.Infrastructure.SettingsFd;
using Referee.ViewModels;

namespace Referee.Infrastructure.WindowNavigation
{
	public class UpdateWindowCommand : ICommand
	{
		private readonly WindowManager _windowManager;
		private readonly RozhodciService _rozhodciService;
		private readonly CetaService _cetaService;
		private readonly Settings _settings;
		private readonly Printer _printer;

		public UpdateWindowCommand(WindowManager windowManager, RozhodciService rozhodciService, CetaService cetaService, Settings settings, Printer printer)
		{
			_windowManager = windowManager;
			_rozhodciService = rozhodciService;
			_cetaService = cetaService;
			_settings = settings;
			_printer = printer;
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
				_windowManager.ActiveViewModel = viewType switch
				{
					ViewType.Rozhodci => new RozhodciViewModel(_rozhodciService, _printer),
					ViewType.Ceta => new CetaViewModel(_cetaService, _printer),
					ViewType.Settings => new SettingsViewModel(_settings),
					_ => new RozhodciViewModel(_rozhodciService, _printer)
				};

				if (_windowManager.ActiveViewModel.FilterCollection != null)
					_windowManager.ActiveViewModel.FilterCollection.Filter = _windowManager.Filter;  
				_windowManager.ViewType = viewType;
			}
		}
	}
}
