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
	public class CetaViewModel : BaseViewModel
	{
		private bool? _isAllSelected;
		private readonly CetarService _cetarService;
		private Cetar _selectedCetar = Cetar.CreateEmpty();
		private Cetar _createCetar = Cetar.CreateEmpty();
		private Cetar _selectedCetarCache = Cetar.CreateEmpty();

		public ObservableCollection<Cetar> Collection { get; } = new();

		public CetaViewModel(CetarService cetarService, Printer printer)
		{
			_cetarService = cetarService;
			DialogSwitchViewModel = new DialogSwitchViewModel("Přidat", "četaře", 340);
			OpenDialogHost = new Command<EditorMode>(x =>
			{
				if (x is EditorMode mode)
					DialogSwitchViewModel.SetValues(mode);
				SelectedCetar ??= Cetar.CreateEmpty();
				if (DialogSwitchViewModel.EditorMode == EditorMode.Edit)
				{
					EditIndex = Collection.IndexOf(SelectedCetar);
					if (EditIndex == -1)
					{
						SelectedCetar = Collection.FirstOrDefault(r => r.Id == _selectedCetarCache.Id);
						EditIndex = Collection.IndexOf(SelectedCetar);
					}

					if (SelectedCetar != null && SelectedCetar.FullName != " ")
						_selectedCetarCache = new Cetar(SelectedCetar);
				}

				if (DialogSwitchViewModel.EditorMode == EditorMode.Create)
					CreateCetar ??= Cetar.CreateEmpty();
				IsDialogHostOpen = true;
			});
#pragma warning disable 4014
			RawPrintCommand = new Command(() => printer.RawPrint<Cetar>(RawPagesCount));
			SelectionPrintCommand = new Command(() => printer.Print(Collection.Where(x => x.IsSelected).ToList()));
			LoadCommand = new Command(() => LoadCetaAsync());
			CreateOrEditCommand = new Command(() => HandleCetar(),
				() =>
				{
					if (DialogSwitchViewModel.EditorMode == EditorMode.Delete)
						return true;
					return Extensions.ValidatePerson(DialogSwitchViewModel.EditorMode == EditorMode.Edit ? SelectedCetar : CreateCetar);
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
				foreach (var cetar in Collection.Where(x => x.IsSelected))
					cetar.Reward = Reward;
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

		public Cetar SelectedCetar
		{
			get => _selectedCetar;
			set => SetAndRaise(ref _selectedCetar, value);
		}

		public Cetar CreateCetar
		{
			get => _createCetar;
			private set => SetAndRaise(ref _createCetar, value);
		}

		private async Task LoadCetaAsync()
		{
			await foreach (var cetar in _cetarService.GetCeta())
			{
				cetar.PropertyChanged += (_, args) =>
				{
					if (args.PropertyName == nameof(Cetar.IsSelected))
					{
						SelectedCount = Collection.Count(x => x.IsSelected);
						OnPropertyChanged(nameof(IsAllSelected));
					}
				};
				Collection.Add(cetar);
				await Task.Delay(30);
			}
		}

		private void CloseDialog()
		{
			IsDialogHostOpen = false;
			if (DialogSwitchViewModel.EditorMode == EditorMode.Edit)
			{
				Collection[EditIndex].CopyValuesFrom(_selectedCetarCache);
				SelectedCetar = new Cetar(_selectedCetarCache);
			}

			if (DialogSwitchViewModel.EditorMode == EditorMode.Create)
				CreateCetar = Cetar.CreateEmpty();
		}

		private async Task HandleCetar()
		{
			switch (DialogSwitchViewModel.EditorMode)
			{
				case EditorMode.Create:
					var created = new Cetar();
					created.CopyValuesFrom(CreateCetar);
					created.Id = await _cetarService.AddCetar(CreateCetar);
					created.PropertyChanged += (_, args) =>
					{
						if (args.PropertyName == nameof(Cetar.IsSelected))
						{
							SelectedCount = Collection.Count(x => x.IsSelected);
							OnPropertyChanged(nameof(IsAllSelected));
						}
					};
					Collection.Add(created);
					CreateCetar = Cetar.CreateEmpty();
					break;
				case EditorMode.Edit:
				{
					var index = Collection.IndexOf(SelectedCetar);
					if (index == -1)
						throw new IndexOutOfRangeException("Cetar was not found");
					Collection[index].CopyValuesFrom(SelectedCetar);
					await _cetarService.UpdateCetar(Collection[index]);
					break;
				}
				case EditorMode.Delete:
				{
					var selected = Collection.FirstOrDefault(x => x.Id == SelectedCetar.Id);
					SelectedCetar.IsSelected = false;
					await _cetarService.DeleteCetar(selected);
					Collection.Remove(selected);
					break;
				}
			}

			IsDialogHostOpen = false;
		}
	}
}
