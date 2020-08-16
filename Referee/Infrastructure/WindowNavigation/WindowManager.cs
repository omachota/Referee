using System.Windows.Input;
using Referee.Infrastructure.DataServices;
using Referee.Infrastructure.Print;
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
		private readonly Settings _settings;
		private readonly Printer _printer;
		private BaseViewModel _activeViewModel;
		private readonly RozhodciService _rozhodciService;

		public WindowManager(Settings settings)
		{
			_settings = settings;
			_printer = new Printer(settings);
			_rozhodciService = new RozhodciService(_settings);
			_activeViewModel = new RozhodciViewModel(_rozhodciService, _printer);
		}

		public ViewType ViewType { get; set; }

		public ICommand UpdateWindowCommand => new UpdateWindowCommand(this, _rozhodciService, _settings, _printer);

		public BaseViewModel ActiveViewModel
		{
			get => _activeViewModel;
			set => SetAndRaise(ref _activeViewModel, value);
		}
	}
}
