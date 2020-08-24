using System.Windows.Input;
using Referee.Infrastructure;
using Referee.Infrastructure.SettingsFd;

namespace Referee.ViewModels
{
	public class SettingsViewModel : BaseViewModel
	{
		private readonly string _repositoryAddress = "https://github.com/omachota/Referee";

		public ICommand OpenRepositoryCommand { get; }

		public ICommand RevertChanges { get; }

		public SettingsViewModel(Settings settings)
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
		}

		public Settings Settings { get; set; }
	}
}
