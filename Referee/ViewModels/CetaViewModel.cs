using System.Diagnostics;
using Referee.Infrastructure.Print;

namespace Referee.ViewModels
{
	public class CetaViewModel : BaseViewModel
	{
		public CetaViewModel(Printer printer)
		{
			Debug.WriteLine(printer);
		}
	}
}
