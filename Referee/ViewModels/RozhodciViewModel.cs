using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Referee.Infrastructure;
using Referee.Infrastructure.Print;
using Referee.Infrastructure.SettingsFd;
using Referee.Models;

namespace Referee.ViewModels
{
	public class RozhodciViewModel : BaseViewModel
	{
		private int _selectedRozhodciCount;
		private bool _isDialogHostOpen;
		private int _rawPagesCount;
		private bool? _isAllSelected;
		private Rozhodci _selectedRozhodci = Rozhodci.CreateEmpty();
		private Rozhodci _selectedRozhodciCache;
		private List<Rozhodci> _selectedRozhodciCollection = new List<Rozhodci>();

		public ObservableCollection<int> RawPages { get; set; } = new ObservableCollection<int>(Enumerable.Range(1, 9));
		public ObservableCollection<Rozhodci> RozhodciCollection { get; set; } = new ObservableCollection<Rozhodci>();

		public ICommand OpenDialogHost { get; }
		public ICommand RawPrintCommand { get; }
		public ICommand SelectionPrintCommand { get; }
		public ICommand LoadCommand { get; }
		public ICommand CloseDialogHostCommand { get; }
		public ICommand DeleteRozhodciCommand { get; }
		public ICommand CreateOrEditRozhodciCommand { get; }

		public RozhodciViewModel(Settings settings, Printer printer)
		{
			DialogSwitchViewModel = new DialogSwitchViewModel("HEader", "EE");
			OpenDialogHost = new Command<EditorMode>(x =>
			{
				IsDialogHostOpen = true;
				if (x is EditorMode mode)
					DialogSwitchViewModel.SetValues(mode);
				if (DialogSwitchViewModel.EditorMode == EditorMode.Edit)
				{
					var index = RozhodciCollection.IndexOf(SelectedRozhodci);
					if (index == -1)
						SelectedRozhodci = new Rozhodci(_selectedRozhodciCache);
					_selectedRozhodciCache = new Rozhodci(SelectedRozhodci);
				}
			});
			RawPrintCommand = new Command(() =>
			{
				/* TODO : RawPrint */
			});
			SelectionPrintCommand = new Command(() =>
			{
				Debug.WriteLine($"{_selectedRozhodciCollection.Count}");
				Debug.WriteLine($"Odměna: {SelectedRozhodci.Reward}");
				bool countRewards = true;
				for (int i = 0; i < _selectedRozhodciCollection.Count; i++)
				{
					if (!_selectedRozhodciCollection[i].Reward.HasValue)
					{
						countRewards = false;
						break;
					}
				}

				int? sum = 0;
				if (countRewards)
					_selectedRozhodciCollection.ForEach(x => sum += x.Reward);
				else
					sum = null;
				Debug.WriteLine($"Celkem: {sum}");
			});
#pragma warning disable 4014
			LoadCommand = new Command(() => LoadRozhodciAsync().ContinueWith(_ => RawPagesCount = 5));
#pragma warning restore 4014
			CreateOrEditRozhodciCommand = new Command(() =>
			{
				if (DialogSwitchViewModel.EditorMode == EditorMode.Create)
				{
					Debug.WriteLine(SelectedRozhodci.Address);
				}

				IsDialogHostOpen = false;

				Task.Delay(150).ContinueWith(_ => SelectedRozhodci = Rozhodci.CreateEmpty());
			});
			CloseDialogHostCommand = new Command(() =>
			{
				IsDialogHostOpen = false;
				if (DialogSwitchViewModel.EditorMode == EditorMode.Edit)
				{
					var index = RozhodciCollection.IndexOf(SelectedRozhodci);
					if (index == -1)
						index = RozhodciCollection.IndexOf(RozhodciCollection.FirstOrDefault(x => x.Id == SelectedRozhodci.Id));
					RozhodciCollection[index] = _selectedRozhodciCache;
				}

				SelectedRozhodci = Rozhodci.CreateEmpty();
			});
			DeleteRozhodciCommand = new Command(() => { Debug.WriteLine($"Delete: {SelectedRozhodci.Id}"); });
		}

		public DialogSwitchViewModel DialogSwitchViewModel { get; }

		public int SelectedRozhodciCount
		{
			get => _selectedRozhodciCount;
			set => SetAndRaise(ref _selectedRozhodciCount, value);
		}

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

		public bool? IsAllSelected
		{
			get
			{
				var selected = RozhodciCollection.Select(x => x.IsSelected).Distinct().ToList();
				if (selected.Count == 0)
					return false;
				return selected.Count == 1 ? selected.Single() : (bool?) null;
			}
			set
			{
				if (value.HasValue)
				{
					for (int i = 0; i < RozhodciCollection.Count; i++)
					{
						RozhodciCollection[i].IsSelected = value.Value;
					}

					SetAndRaise(ref _isAllSelected, value);
				}
			}
		}

		public Rozhodci SelectedRozhodci
		{
			get => _selectedRozhodci;
			set => SetAndRaise(ref _selectedRozhodci, value);
		}

		private async Task LoadRozhodciAsync()
		{
			// TODO: Load Rozhodci from database

			for (int i = 0; i < 5; i++)
			{
				Rozhodci rozhoci = new Rozhodci(i.ToString(), "Test", DateTime.Now, "Address", "City", i);
				rozhoci.PropertyChanged += (sender, args) =>
				{
					if (args.PropertyName == nameof(SelectableViewModel.IsSelected))
					{
						_selectedRozhodciCollection = RozhodciCollection.Where(x => x.IsSelected).ToList();
						SelectedRozhodciCount = _selectedRozhodciCollection.Count;
						OnPropertyChanged(nameof(IsAllSelected));
					}
				};
				RozhodciCollection.Add(rozhoci);
				await Task.Delay(500);
			}
		}
	}
}
