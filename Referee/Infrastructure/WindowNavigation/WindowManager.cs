using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Referee.Infrastructure.DataServices;
using Referee.Infrastructure.SettingsFd;
using Referee.Models;
using Referee.ViewModels;

namespace Referee.Infrastructure.WindowNavigation
{
	public enum ViewType
	{
		None, // Do not delete
		Rozhodci,
		Ceta,
		Settings
	}

	public class WindowManager : AbstractNotifyPropertyChanged, IWindowManager
	{
		private BaseViewModel _activeViewModel;
		private readonly UpdateWindowCommand _updateWindowCommand;

		private readonly IDictionary<Type, int> _modelsIndexes = new Dictionary<Type, int>
		{
			{ typeof(RozhodciViewModel), 0 },
			{ typeof(CetaViewModel), 1 },
			{ typeof(SettingsViewModel), 2 }
		};

		public WindowManager(Settings settings, DapperContext context)
		{
			var printer = new Printer(settings);
			var rozhodciService = new RozhodciService(context);
			var cetaService = new CetarService(context);
			_updateWindowCommand = new UpdateWindowCommand(this, rozhodciService, cetaService, settings, printer);
			_updateWindowCommand.Execute(ViewType.Rozhodci);
		}

		public ViewType ViewType { get; set; }

		public ICommand UpdateWindowCommand => _updateWindowCommand;

		public BaseViewModel ActiveViewModel
		{
			get => _activeViewModel;
			set => SetAndRaise(ref _activeViewModel, value);
		}

		public int ActiveViewModelIndex => _modelsIndexes[ActiveViewModel.GetType()];

		public string Search;
		private Regex _reg;

		public bool Filter(object o)
		{
			if (Search is { Length: > 2 })
			{
				return o is IPerson item && _reg.IsMatch(item.FullName);
			}

			return false;
		}

		public void UpdateRegex()
		{
			var pattern = new StringBuilder();

			pattern.Append(Search);
			pattern.Append("[a-zA-Z]*");
			
			_reg = new Regex(pattern.ToString(), RegexOptions.IgnoreCase);
		}
	}
}
