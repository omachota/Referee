using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using Referee.Infrastructure;
using Referee.Infrastructure.DataServices;
using Referee.Models;

namespace Referee.ViewModels
{
	public class RozhodciViewModel : PersonViewModel
	{
		private bool? _isAllSelected;
		private readonly RozhodciService _rozhodciService;
		private Rozhodci _selectedRozhodci = Rozhodci.CreateEmpty();
		private Rozhodci _createRozhodci = Rozhodci.CreateEmpty();
		private Rozhodci _selectedRozhodciCache = Rozhodci.CreateEmpty();

		public ObservableCollection<Rozhodci> Collection { get; } = new();

		public RozhodciViewModel(RozhodciService rozhodciService, Printer printer)
		{
			_rozhodciService = rozhodciService;
			DialogSwitchViewModel = new DialogSwitchViewModel("Přidat", "rozhodčího", 480);
			OpenDialogHost = new Command<EditorMode>(x =>
			{
				if (x is EditorMode mode)
					DialogSwitchViewModel.SetValues(mode);
				SelectedRozhodci ??= Rozhodci.CreateEmpty();
				if (DialogSwitchViewModel.EditorMode == EditorMode.Edit)
				{
					EditIndex = Collection.IndexOf(SelectedRozhodci);
					if (EditIndex == -1)
					{
						SelectedRozhodci = Collection.FirstOrDefault(r => r.Id == _selectedRozhodciCache.Id);
						EditIndex = Collection.IndexOf(SelectedRozhodci);
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
			SelectionPrintCommand = new Command(() => printer.Print(Collection.Where(x => x.IsSelected).ToList()));
			LoadCommand = new Command(() => LoadRozhodciAsync());
			CreateOrEditCommand = new Command(() => HandleRozhodci(),
				() =>
				{
					if (DialogSwitchViewModel.EditorMode == EditorMode.Delete)
						return true;
					return Extensions.ValidatePerson(DialogSwitchViewModel.EditorMode == EditorMode.Edit ? SelectedRozhodci : CreateRozhodci);
				});
#pragma warning restore 4014
			CloseDialogHostCommand = new Command(CloseDialog);
			DeleteCommand = new Command(() =>
			{
				DialogSwitchViewModel.SetValues(EditorMode.Delete);
				IsDialogHostOpen = true;
			});
			SetRewardToSelectedCommand = new Command(() =>
			{
				foreach (var rozhodci in Collection.Where(x => x.IsSelected))
					rozhodci.Reward = Reward;
			});
			FilterCollection = CollectionViewSource.GetDefaultView(Collection);
		}

		public bool? IsAllSelected
		{
			get
			{
				var selected = Collection.Select(x => x.IsSelected).Distinct().ToList();
				if (selected.Count == 0)
					return false;
				return selected.Count == 1 ? selected.Single() : null;
			}
			set
			{
				if (value.HasValue)
				{
					for (int i = 0; i < Collection.Count; i++)
					{
						Collection[i].IsSelected = value.Value;
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

		public Rozhodci CreateRozhodci
		{
			get => _createRozhodci;
			private set => SetAndRaise(ref _createRozhodci, value);
		}

		private async Task LoadRozhodciAsync()
		{
			await foreach (var rozhodci in _rozhodciService.GetRozhodci())
			{
				rozhodci.PropertyChanged += (_, args) =>
				{
					if (args.PropertyName == nameof(Rozhodci.IsSelected))
					{
						SelectedCount = Collection.Count(x => x.IsSelected);
						OnPropertyChanged(nameof(IsAllSelected));
					}
				};
				Collection.Add(rozhodci);
				await Task.Delay(30);
			}
		}

		private void CloseDialog()
		{
			IsDialogHostOpen = false;
			if (DialogSwitchViewModel.EditorMode == EditorMode.Edit)
			{
				Collection[EditIndex].CopyValuesFrom(_selectedRozhodciCache);
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
					var created = new Rozhodci();
					created.CopyValuesFrom(CreateRozhodci);
					created.Id = await _rozhodciService.AddRozhodci(CreateRozhodci);
					created.PropertyChanged += (_, args) =>
					{
						if (args.PropertyName == nameof(Rozhodci.IsSelected))
						{
							SelectedCount = Collection.Count(x => x.IsSelected);
							OnPropertyChanged(nameof(IsAllSelected));
						}
					};
					Collection.Add(created);
					CreateRozhodci = Rozhodci.CreateEmpty();
					break;
				case EditorMode.Edit:
				{
					var index = Collection.IndexOf(SelectedRozhodci);
					if (index == -1)
						throw new IndexOutOfRangeException("Rozhodci was not found");
					Collection[index].CopyValuesFrom(SelectedRozhodci);
					await _rozhodciService.UpdateRozhodci(Collection[index]);
					break;
				}
				case EditorMode.Delete:
				{
					var selected = Collection.FirstOrDefault(x => x.Id == SelectedRozhodci.Id);
					SelectedRozhodci.IsSelected = false;
					await _rozhodciService.DeleteRozhodci(selected);
					Collection.Remove(selected);
					break;
				}
			}

			IsDialogHostOpen = false;
		}
	}
}
