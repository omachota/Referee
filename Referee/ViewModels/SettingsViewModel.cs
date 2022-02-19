using System.Windows.Input;
using Referee.Infrastructure;
using Referee.Infrastructure.SettingsFd;

namespace Referee.ViewModels
{
	public class SettingsViewModel : BaseViewModel
	{
		private bool _changedMade;
		private Settings _cachedSettings;
		private readonly string _repositoryAddress = "https://github.com/omachota/Referee";

		public ICommand OpenRepositoryCommand { get; }

		public ICommand RevertChanges { get; }

		public SettingsViewModel(Settings settings)
		{
			Settings = settings;
			_cachedSettings = new Settings(settings);
			Settings.PropertyChanged += (_, _) => { _changedMade = true; };
			Settings.DbSettings.PropertyChanged += (_, _) => { _changedMade = true; };
			OpenRepositoryCommand = new Command(() => Browser.OpenLink(_repositoryAddress));
			RevertChanges = new Command(() =>
			{
				Settings.CopyValuesFrom(_cachedSettings);
				_changedMade = false;
			}, () => _changedMade);
		}

		public Settings Settings { get; set; }

		public void UpdateChangesMadeValue()
		{
			_changedMade = false;
			_cachedSettings = new Settings(Settings);
		}
	}
}
