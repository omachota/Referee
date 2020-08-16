using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Referee.Infrastructure;
using Referee.Infrastructure.DataServices;
using Referee.Infrastructure.Print;
using Referee.Models;

namespace Referee.ViewModels
{
	public class RozhodciViewModel : BaseViewModel
	{
		private int _selectedRozhodciCount;
		private bool _isDialogHostOpen;
		private int _rawPagesCount;
		private bool? _isAllSelected;
		private readonly RozhodciService _rozhodciService;
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

		public RozhodciViewModel(RozhodciService rozhodciService, Printer printer)
		{
			_rozhodciService = rozhodciService;
			DialogSwitchViewModel = new DialogSwitchViewModel("Přidat", "rozhodčího");
			OpenDialogHost = new Command<EditorMode>(x =>
			{
				IsDialogHostOpen = true;
				SelectedRozhodci ??= Rozhodci.CreateEmpty();
				if (x is EditorMode mode)
					DialogSwitchViewModel.SetValues(mode);
				if (DialogSwitchViewModel.EditorMode == EditorMode.Edit)
				{
					var index = RozhodciCollection.IndexOf(SelectedRozhodci);
					if (index == -1)
					{
						_selectedRozhodciCache ??= Rozhodci.CreateEmpty();
						SelectedRozhodci = RozhodciCollection.FirstOrDefault(r => r.Id == _selectedRozhodciCache.Id);
					}
					_selectedRozhodciCache = new Rozhodci(SelectedRozhodci);
				}
			});
			RawPrintCommand = new Command(() => printer.RawPrint(RawPagesCount));
			SelectionPrintCommand = new Command(() =>
			{
				Debug.WriteLine($"{_selectedRozhodciCollection.Count}");
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
				printer.Print(_selectedRozhodciCollection.ToList(), sum);
				Debug.WriteLine($"Celkem: {sum}");
			});
#pragma warning disable 4014
			LoadCommand = new Command(() => LoadRozhodciAsync().ContinueWith(_ => RawPagesCount = 5));
#pragma warning restore 4014
			CreateOrEditRozhodciCommand = new Command(HandleRozhodci, () => Extensions.ValidateRozhodci(SelectedRozhodci));
			CloseDialogHostCommand = new Command(CloseDialog);
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
			await foreach (var rozhodci in _rozhodciService.LoadRozhodciFromDb())
			{
				rozhodci.PropertyChanged += (sender, args) =>
				{
					if (args.PropertyName == nameof(SelectableViewModel.IsSelected))
					{
						_selectedRozhodciCollection = RozhodciCollection.Where(x => x.IsSelected).ToList();
						SelectedRozhodciCount = _selectedRozhodciCollection.Count;
						OnPropertyChanged(nameof(IsAllSelected));
					}
				};
				RozhodciCollection.Add(rozhodci);
			}
		}

		private void CloseDialog()
		{
			IsDialogHostOpen = false;
			if (DialogSwitchViewModel.EditorMode == EditorMode.Edit)
			{
				SelectedRozhodci.CopyValuesFrom(_selectedRozhodciCache);
			}

			SelectedRozhodci = Rozhodci.CreateEmpty();
		}

		private async void HandleRozhodci()
		{
			if (DialogSwitchViewModel.EditorMode == EditorMode.Create)
			{
				Debug.WriteLine(SelectedRozhodci.Address);
			}
			else
			{
				//var index = RozhodciCollection.IndexOf(RozhodciCollection.FirstOrDefault(x => x.Id == SelectedRozhodci.Id));
				var index = RozhodciCollection.IndexOf(SelectedRozhodci);
				if (index == -1)
					throw new IndexOutOfRangeException("Rozhodci was not found");
				RozhodciCollection[index].CopyValuesFrom(SelectedRozhodci);
				await _rozhodciService.UpdateRozhodciInDatabase(RozhodciCollection[index]);
			}

			IsDialogHostOpen = false;

			SelectedRozhodci = Rozhodci.CreateEmpty();
		}
	}
}
