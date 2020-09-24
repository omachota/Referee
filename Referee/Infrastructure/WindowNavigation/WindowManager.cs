using System;
using System.Collections.Generic;
using System.Windows.Input;
using Referee.Infrastructure.DataServices;
using Referee.Infrastructure.SettingsFd;
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
		private readonly UpdateWindowCommand _updateWindowCommand;
		private readonly IDictionary<Type, int> _modelsIndexes;

		public WindowManager(Settings settings)
		{
			var printer = new Printer(settings);
			var rozhodciService = new RozhodciService(settings);
			var cetaService = new CetaService(settings);
			_updateWindowCommand = new UpdateWindowCommand(this, rozhodciService, cetaService, settings, printer);
			_modelsIndexes = new Dictionary<Type, int>
			{
				{typeof(RozhodciViewModel), 0},
				{typeof(CetaViewModel), 1},
				{typeof(SettingsViewModel), 2}
			};
			_activeViewModel = new SettingsViewModel(settings, true);
			ViewType = (ViewType) _modelsIndexes[ActiveViewModel.GetType()];
		}

		public ViewType ViewType { get; set; }

		public ICommand UpdateWindowCommand => _updateWindowCommand;

		public BaseViewModel ActiveViewModel
		{
			get => _activeViewModel;
			set => SetAndRaise(ref _activeViewModel, value);
		}

		public int ActiveViewModelIndex => _modelsIndexes[ActiveViewModel.GetType()];
	}
}
