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
	public class CetaViewModel : BaseViewModel
	{
		private int _selectedCetaCount;
		private bool _isDialogHostOpen;
		private int _rawPagesCount;
		private int _editIndex;
		private int _reward;
		private bool? _isAllSelected;
		private readonly CetaService _cetaService;
		private Cetar _selectedCetar = Cetar.CreateEmpty();
		private Cetar _createCetar = Cetar.CreateEmpty();
		private Cetar _selectedCetarCache = Cetar.CreateEmpty();
		private List<Cetar> _selectedCetaCollection = new List<Cetar>();

		public ObservableCollection<int> RawPages { get; set; } = new ObservableCollection<int>(Enumerable.Range(1, 9));
		public ObservableCollection<Cetar> CetaCollection { get; set; } = new ObservableCollection<Cetar>();

		public ICommand OpenDialogHost { get; }
		public ICommand RawPrintCommand { get; }
		public ICommand SelectionPrintCommand { get; }
		public ICommand LoadCommand { get; }
		public ICommand CloseDialogHostCommand { get; }
		public ICommand DeleteCetarCommand { get; }
		public ICommand CreateOrEditCetarCommand { get; }
		public ICommand SetRewardToSelectedCetari { get; }

		public CetaViewModel(CetaService cetaService, Printer printer)
		{
			_cetaService = cetaService;
			DialogSwitchViewModel = new DialogSwitchViewModel("Přidat", "četaře", 340);
			RawPagesCount = 1;
			OpenDialogHost = new Command<EditorMode>(x =>
			{
				if (x is EditorMode mode)
					DialogSwitchViewModel.SetValues(mode);
				SelectedCetar ??= Cetar.CreateEmpty();
				if (DialogSwitchViewModel.EditorMode == EditorMode.Edit)
				{
					_editIndex = CetaCollection.IndexOf(SelectedCetar);
					if (_editIndex == -1)
					{
						SelectedCetar = CetaCollection.FirstOrDefault(r => r.Id == _selectedCetarCache.Id);
						_editIndex = CetaCollection.IndexOf(SelectedCetar);
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
			SelectionPrintCommand = new Command(() => printer.Print(_selectedCetaCollection.ToList()));
			LoadCommand = new Command(() => LoadCetaAsync());
			CreateOrEditCetarCommand = new Command(() => HandleCetar(),
				() =>
				{
					if (DialogSwitchViewModel.EditorMode == EditorMode.Delete)
						return true;
					return Extensions.ValidatePerson(DialogSwitchViewModel.EditorMode == EditorMode.Edit ? SelectedCetar : CreateCetar);
				});
#pragma warning restore 4014
			CloseDialogHostCommand = new Command(CloseDialog);
			DeleteCetarCommand = new Command(() =>
			{
				DialogSwitchViewModel.SetValues(EditorMode.Delete);
				IsDialogHostOpen = true;
			});
			SetRewardToSelectedCetari = new Command(() =>
			{
				foreach (var cetar in CetaCollection.Where(x => x.IsSelected))
					cetar.Reward = Reward;
			});
		}

		public DialogSwitchViewModel DialogSwitchViewModel { get; }

		public int SelectedCetaCount
		{
			get => _selectedCetaCount;
			set => SetAndRaise(ref _selectedCetaCount, value);
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
				var selected = CetaCollection.Select(x => x.IsSelected).Distinct().ToList();
				if (selected.Count == 0)
					return false;
				return selected.Count == 1 ? selected.Single() : (bool?) null;
			}
			set
			{
				if (value.HasValue)
				{
					for (int i = 0; i < CetaCollection.Count; i++)
					{
						CetaCollection[i].IsSelected = value.Value;
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

		public Cetar SelectedCetar
		{
			get => _selectedCetar;
			set => SetAndRaise(ref _selectedCetar, value);
		}

		public Cetar CreateCetar
		{
			get => _createCetar;
			set => SetAndRaise(ref _createCetar, value);
		}

		private async Task LoadCetaAsync()
		{
			await foreach (var cetar in _cetaService.LoadCetaFromDb())
			{
				cetar.PropertyChanged += (sender, args) =>
				{
					if (args.PropertyName == nameof(Cetar.IsSelected))
					{
						_selectedCetaCollection = CetaCollection.Where(x => x.IsSelected).ToList();
						SelectedCetaCount = _selectedCetaCollection.Count;
						OnPropertyChanged(nameof(IsAllSelected));
					}
				};
				CetaCollection.Add(cetar);
				await Task.Delay(35);
			}
		}

		private void CloseDialog()
		{
			IsDialogHostOpen = false;
			if (DialogSwitchViewModel.EditorMode == EditorMode.Edit)
			{
				CetaCollection[_editIndex].CopyValuesFrom(_selectedCetarCache);
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
					Cetar cetar = await _cetaService.AddNewCetar(CreateCetar);
					cetar.PropertyChanged += (sender, args) =>
					{
						if (args.PropertyName == nameof(Cetar.IsSelected))
						{
							_selectedCetaCollection = CetaCollection.Where(x => x.IsSelected).ToList();
							SelectedCetaCount = _selectedCetaCollection.Count;
							OnPropertyChanged(nameof(IsAllSelected));
						}
					};
					CetaCollection.Add(cetar);
					CreateCetar = Cetar.CreateEmpty();
					break;
				case EditorMode.Edit:
				{
					//var index = CetaCollection.IndexOf(CetaCollection.FirstOrDefault(x => x.Id == SelectedCetar.Id));
					var index = CetaCollection.IndexOf(SelectedCetar);
					if (index == -1)
						throw new IndexOutOfRangeException("Cetar was not found");
					CetaCollection[index].CopyValuesFrom(SelectedCetar);
					await _cetaService.UpdateCetarInDatabase(CetaCollection[index]);
					break;
				}
				case EditorMode.Delete:
				{
					var selected = CetaCollection.FirstOrDefault(x => x.Id == SelectedCetar.Id);
					SelectedCetar.IsSelected = false;
					await _cetaService.DeleteCetarFromDatabase(selected);
					CetaCollection.Remove(selected);
					break;
				}
			}

			IsDialogHostOpen = false;
		}
	}
}
