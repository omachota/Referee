using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Referee.Infrastructure;

namespace Referee.ViewModels
{
	public abstract class BaseViewModel : AbstractNotifyPropertyChanged
	{
		private bool _isDialogHostOpen;
		private int _rawPagesCount;
		protected int EditIndex = -1;
		private int _reward;
		private int _selectedCount;
		
		public bool IsDialogHostOpen
		{
			get => _isDialogHostOpen;
			set => SetAndRaise(ref _isDialogHostOpen, value);
		}

		public int RawPagesCount
		{
			get => _rawPagesCount;
			set => SetAndRaise(ref _rawPagesCount, value);
		}

		public int Reward
		{
			get => _reward;
			set => SetAndRaise(ref _reward, value);
		}
		
		public int SelectedCount
		{
			get => _selectedCount;
			set => SetAndRaise(ref _selectedCount, value);
		}

		protected BaseViewModel()
		{
			RawPagesCount = 1;
		}

		#region Commands

		public ICommand OpenDialogHost { get; protected init; }
		public ICommand RawPrintCommand { get; protected init; }
		public ICommand SelectionPrintCommand { get; protected init; }
		public ICommand LoadCommand { get; protected init; }
		public ICommand CloseDialogHostCommand { get; protected init; }
		public ICommand DeleteCommand { get; protected init; }
		public ICommand CreateOrEditCommand { get; protected init; }
		public ICommand SetRewardToSelectedCommand { get; protected init; }

		#endregion
		
		public ICollectionView FilterCollection { get; protected init; }
		public ObservableCollection<int> RawPages { get; } = new(Enumerable.Range(1, 9));
		public DialogSwitchViewModel DialogSwitchViewModel { get; protected init; }
	}
}
