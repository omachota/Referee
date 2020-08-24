using System.Windows.Input;
using Referee.ViewModels;

namespace Referee.Infrastructure.WindowNavigation
{
	public interface IWindowManager
	{
		BaseViewModel ActiveViewModel { get; set; }
		ICommand UpdateWindowCommand { get; }
	}
}
