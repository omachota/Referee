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
			var rozhodciService = new RozhodciService(context, settings.DbSettings);
			var cetaService = new CetarService(context, settings.DbSettings);
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
		private const string Patt = "[ěščřžýáíéúůóďťňa-zA-Z ]*";
		private Regex _reg;
		private bool _onlySpaces;

		public bool Filter(object o)
		{
			if (Search is { Length: > 2 } && !_onlySpaces)
			{
				return o is IPerson item && (_reg.IsMatch(item.FullName) || _reg.IsMatch(item.FullNameInverted));
			}

			return false;
		}

		public void UpdateRegex()
		{
			_onlySpaces = false;
			var pattern = new StringBuilder();
			pattern.Append('^');
			
			// ignore leading whitespace
			var i = 0;
			for (; i < Search.Length; i++)
			{
				if (Search[i] != ' ')
					break;
			}
			
			// invalidate if Search contains only spaces
			if (i == Search.Length)
				_onlySpaces = true;

			var lastSpace = -1;
			for (; i < Search.Length; i++)
			{
				if (Search[i] == ' ')
				{
					if (lastSpace == i - 1) // ignore multiple spaces
					{
						lastSpace = i;
						continue;
					}
					lastSpace = i;	
					pattern.Append(Patt);
					pattern.Append(' ');
				}
				else
				{
					pattern.Append(Search[i]);
				}
			}

			if (Search.Length > 0 && Search[^1] != ' ')
				pattern.Append(Patt);

			_reg = new Regex(pattern.ToString(), RegexOptions.IgnoreCase);
		}
	}
}
