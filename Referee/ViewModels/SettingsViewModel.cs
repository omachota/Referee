using System.Threading.Tasks;
using System.Windows.Input;
using Referee.Infrastructure;
using Referee.Infrastructure.SettingsFd;

namespace Referee.ViewModels
{
	public class SettingsViewModel : BaseViewModel
	{
		private bool _isExpanderExpanded;
		private readonly string _repositoryAddress = "https://github.com/omachota/Referee";

		public ICommand OpenRepositoryCommand { get; }

		public ICommand RevertChanges { get; }

		public SettingsViewModel(Settings settings, bool isExpanderExpanded = false)
		{
			Settings = settings;
			var cachedSettings = new Settings(settings);
			var changedMade = false;
			Settings.PropertyChanged += (sender, args) => changedMade = true;
			Settings.DbSettings.PropertyChanged += (sender, args) => changedMade = true;
			OpenRepositoryCommand = new Command(() => ChromeLauncher.OpenLink(_repositoryAddress));
			RevertChanges = new Command(() =>
			{
				Settings.CopyValuesFrom(cachedSettings);
				changedMade = false;
			}, () => changedMade);
			if (isExpanderExpanded)
				Task.Delay(700).ContinueWith(x => IsExpanderExpaded = true);
		}

		public Settings Settings { get; set; }

		public bool IsExpanderExpaded
        {
			get => _isExpanderExpanded;
			set => SetAndRaise(ref _isExpanderExpanded, value);
        }
	}
}
