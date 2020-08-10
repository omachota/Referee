using System.Diagnostics;
using System.Threading.Tasks;
using Referee.Infrastructure.SettingsFd;

namespace Referee.ViewModels
{
	public class SettingsViewModel : BaseViewModel
	{

		public SettingsViewModel(Settings settings)
		{
			Settings = settings;
			Task.Delay(2500).ContinueWith(_ => Debug.WriteLine(Settings.IsClubNameEnabled));
		}

		public Settings Settings { get; set; }
	}
}
