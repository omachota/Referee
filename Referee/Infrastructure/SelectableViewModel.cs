namespace Referee.Infrastructure
{
	public abstract class SelectableViewModel : AbstractNotifyPropertyChanged
	{
		private bool _isSelected;

		public bool IsSelected
		{
			get => _isSelected;
			set => SetAndRaise(ref _isSelected, value);
		}
	}
}
