using System.Windows.Input;
using Referee.Infrastructure;
using Referee.Infrastructure.SettingsFd;

namespace Referee.ViewModels
{
	public class SettingsViewModel : BaseViewModel
	{
		private readonly string _repositoryAddress = "https://github.com/omachota/Referee";

		public ICommand OpenRepositoryCommand { get; }

		public SettingsViewModel(Settings settings)
		{
			Settings = settings;
			OpenRepositoryCommand = new Command(() => ChromeLauncher.OpenLink(_repositoryAddress));
		}

		public Settings Settings { get; set; }
	}
}
