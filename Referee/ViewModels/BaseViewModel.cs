using System.Collections.ObjectModel;
using Referee.Infrastructure;
using Referee.Models;

namespace Referee.ViewModels
{
	public abstract class BaseViewModel : AbstractNotifyPropertyChanged
	{
		public SearchChangedEvent SearchChanged;

		public ObservableCollection<IPerson> FilterCollection { get; set; } = new();
	}
}
