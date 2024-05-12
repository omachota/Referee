using System;
using System.Windows.Input;
using Referee.Infrastructure.DataServices;
using Referee.Infrastructure.SettingsFd;
using Referee.ViewModels;
using Serilog;

namespace Referee.Infrastructure.WindowNavigation
{
	public class UpdateWindowCommand : ICommand
	{
		private readonly ILogger _logger;
		private readonly WindowManager _windowManager;
		private readonly RozhodciService _rozhodciService;
		private readonly CetarService _cetarService;
		private readonly Settings _settings;
		private readonly Printer _printer;

		public UpdateWindowCommand(WindowManager windowManager, RozhodciService rozhodciService, CetarService cetarService, Settings settings, Printer printer, ILogger logger)
		{
			_windowManager = windowManager;
			_rozhodciService = rozhodciService;
			_cetarService = cetarService;
			_settings = settings;
			_printer = printer;
			_logger = logger.ForContext<UpdateWindowCommand>();
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
					ViewType.Rozhodci => new RozhodciViewModel(_rozhodciService, _printer, _logger),
					ViewType.Ceta => new CetaViewModel(_cetarService, _printer, _logger),
					ViewType.Settings => new SettingsViewModel(_settings, _logger),
					_ => new RozhodciViewModel(_rozhodciService, _printer, _logger)
				};

				if (_windowManager.ActiveViewModel.FilterCollection != null)
					_windowManager.ActiveViewModel.FilterCollection.Filter = _windowManager.Filter;  
				_windowManager.ViewType = viewType;
			}
		}
	}
}
