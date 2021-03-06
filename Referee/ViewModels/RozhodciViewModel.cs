using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Referee.Infrastructure;
using Referee.Infrastructure.DataServices;
using Referee.Models;

namespace Referee.ViewModels
{
	public class RozhodciViewModel : BaseViewModel
	{
		private int _selectedRozhodciCount;
		private bool _isDialogHostOpen;
		private int _rawPagesCount;
		private int _editIndex;
		private int _reward;
		private bool? _isAllSelected;
		private readonly RozhodciService _rozhodciService;
		private Rozhodci _selectedRozhodci = Rozhodci.CreateEmpty();
		private Rozhodci _createRozhodci = Rozhodci.CreateEmpty();
		private Rozhodci _selectedRozhodciCache = Rozhodci.CreateEmpty();
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
		public ICommand SetRewardToSelectedRozhodci { get; }

		public RozhodciViewModel(RozhodciService rozhodciService, Printer printer)
		{
			_rozhodciService = rozhodciService;
			DialogSwitchViewModel = new DialogSwitchViewModel("Přidat", "rozhodčího", 480);
			RawPagesCount = 1;
			OpenDialogHost = new Command<EditorMode>(x =>
			{
				if (x is EditorMode mode)
					DialogSwitchViewModel.SetValues(mode);
				SelectedRozhodci ??= Rozhodci.CreateEmpty();
				if (DialogSwitchViewModel.EditorMode == EditorMode.Edit)
				{
					_editIndex = RozhodciCollection.IndexOf(SelectedRozhodci);
					if (_editIndex == -1)
					{
						SelectedRozhodci = RozhodciCollection.FirstOrDefault(r => r.Id == _selectedRozhodciCache.Id);
						_editIndex = RozhodciCollection.IndexOf(SelectedRozhodci);
					}

					if (SelectedRozhodci != null && SelectedRozhodci.FullName != " ")
						_selectedRozhodciCache = new Rozhodci(SelectedRozhodci);
				}

				if (DialogSwitchViewModel.EditorMode == EditorMode.Create)
					CreateRozhodci ??= Rozhodci.CreateEmpty();
				IsDialogHostOpen = true;
			});
#pragma warning disable 4014
			RawPrintCommand = new Command(() => printer.RawPrint<Rozhodci>(RawPagesCount));
			SelectionPrintCommand = new Command(() => printer.Print(_selectedRozhodciCollection.ToList()));
			LoadCommand = new Command(() => LoadRozhodciAsync());
			CreateOrEditRozhodciCommand = new Command(() => HandleRozhodci(),
				() =>
				{
					if (DialogSwitchViewModel.EditorMode == EditorMode.Delete)
						return true;
					return Extensions.ValidatePerson(DialogSwitchViewModel.EditorMode == EditorMode.Edit ? SelectedRozhodci : CreateRozhodci);
				});
#pragma warning restore 4014
			CloseDialogHostCommand = new Command(CloseDialog);
			DeleteRozhodciCommand = new Command(() =>
			{
				DialogSwitchViewModel.SetValues(EditorMode.Delete);
				IsDialogHostOpen = true;
			});
			SetRewardToSelectedRozhodci = new Command(() =>
			{
				foreach (var rozhodci in RozhodciCollection.Where(x => x.IsSelected))
					rozhodci.Reward = Reward;
			});
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

		public int Reward
		{
			get => _reward;
			set => SetAndRaise(ref _reward, value);
		}

		public Rozhodci SelectedRozhodci
		{
			get => _selectedRozhodci;
			set => SetAndRaise(ref _selectedRozhodci, value);
		}

		public Rozhodci CreateRozhodci
		{
			get => _createRozhodci;
			set => SetAndRaise(ref _createRozhodci, value);
		}

		private async Task LoadRozhodciAsync()
		{
			await foreach (var rozhodci in _rozhodciService.LoadRozhodciFromDb())
			{
				rozhodci.PropertyChanged += (sender, args) =>
				{
					if (args.PropertyName == nameof(Rozhodci.IsSelected))
					{
						_selectedRozhodciCollection = RozhodciCollection.Where(x => x.IsSelected).ToList();
						SelectedRozhodciCount = _selectedRozhodciCollection.Count;
						OnPropertyChanged(nameof(IsAllSelected));
					}
				};
				RozhodciCollection.Add(rozhodci);
				await Task.Delay(35);
			}
		}

		private void CloseDialog()
		{
			IsDialogHostOpen = false;
			if (DialogSwitchViewModel.EditorMode == EditorMode.Edit)
			{
				RozhodciCollection[_editIndex].CopyValuesFrom(_selectedRozhodciCache);
				SelectedRozhodci = new Rozhodci(_selectedRozhodciCache);
			}

			if (DialogSwitchViewModel.EditorMode == EditorMode.Create)
				CreateRozhodci = Rozhodci.CreateEmpty();
		}

		private async Task HandleRozhodci()
		{
			switch (DialogSwitchViewModel.EditorMode)
			{
				case EditorMode.Create:
					Rozhodci rozhodci = await _rozhodciService.AddNewRozhodci(CreateRozhodci);
					rozhodci.PropertyChanged += (sender, args) =>
					{
						if (args.PropertyName == nameof(Rozhodci.IsSelected))
						{
							_selectedRozhodciCollection = RozhodciCollection.Where(x => x.IsSelected).ToList();
							SelectedRozhodciCount = _selectedRozhodciCollection.Count;
							OnPropertyChanged(nameof(IsAllSelected));
						}
					};
					RozhodciCollection.Add(rozhodci);
					CreateRozhodci = Rozhodci.CreateEmpty();
					break;
				case EditorMode.Edit:
				{
					//var index = CetaCollection.IndexOf(CetaCollection.FirstOrDefault(x => x.Id == SelectedCetar.Id));
					var index = RozhodciCollection.IndexOf(SelectedRozhodci);
					if (index == -1)
						throw new IndexOutOfRangeException("Rozhodci was not found");
					RozhodciCollection[index].CopyValuesFrom(SelectedRozhodci);
					await _rozhodciService.UpdateRozhodciInDatabase(RozhodciCollection[index]);
					break;
				}
				case EditorMode.Delete:
				{
					var selected = RozhodciCollection.FirstOrDefault(x => x.Id == SelectedRozhodci.Id);
					SelectedRozhodci.IsSelected = false;
					await _rozhodciService.DeleteRozhodciFromDatabase(selected);
					RozhodciCollection.Remove(selected);
					break;
				}
			}

			IsDialogHostOpen = false;
		}
	}
}
