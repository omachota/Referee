using System.ComponentModel;
using Referee.Infrastructure;

namespace Referee.ViewModels
{
	public abstract class BaseViewModel : AbstractNotifyPropertyChanged
	{
		public ICollectionView FilterCollection { get; set; }
	}
}
