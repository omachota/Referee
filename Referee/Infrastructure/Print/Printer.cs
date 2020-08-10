using Referee.Infrastructure.SettingsFd;

namespace Referee.Infrastructure.Print
{
	public class Printer
	{
		private readonly Settings _settings;

		public Printer(Settings settings)
		{
			_settings = settings;
		}
	}
}
