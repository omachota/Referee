using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Referee.Infrastructure;
using Referee.Models;

namespace Referee.ViewModels
{
	public class RozhodciViewModel : BaseViewModel
	{
		private int _selectedRozhodciCount;
		private bool _isDialogHostOpen;
		private int _rawPagesCount;
		private List<int> _rawPages;
		private List<Rozhodci> _rozhoci;

		public ICommand OpenDialogHost { get; }
		public ICommand RawPrintCommand { get; }
		public ICommand SelectionPrintCommand { get; }
		public ICommand LoadCommand { get; }
		public ICommand CloseDialogHostCommand { get; }
		public ICommand DeleteRozhodciCommand { get; }
		public ICommand CreateOrEditRozhodciCommand { get; }
		public ICommand SelectedAllCommand { get; }
		public ICommand UnselectedAllCommand { get; }
		public ICommand CellContentCheckBoxChecked { get; }
		public ICommand CellContentCheckBoxUnchecked { get; }

		public List<Rozhodci> Rozhoci
		{
			get => _rozhoci;
			set => SetAndRaise(ref _rozhoci, value);
		}

		public RozhodciViewModel(List<int> rawPages)
		{
			_rawPages = rawPages;
			OpenDialogHost = new Command<EditorMode>(x =>
			{
				IsDialogHostOpen = true;
				if (x is EditorMode mode)
					DialogSwitchViewModel.SetValues(mode);
			});
			RawPrintCommand = new Command(() =>
			{
				/* TODO : RawPrint */
			});
			SelectionPrintCommand = new Command(() =>
			{
				/* TODO : SelectionPrint */
			});
#pragma warning disable 4014
			LoadCommand = new Command(() => LoadRozhodciAsync());
#pragma warning restore 4014
			CloseDialogHostCommand = new Command(() => IsDialogHostOpen = false);
			DeleteRozhodciCommand = new Command(() => { });
			CreateOrEditRozhodciCommand = new Command(() =>
			{
				/*if (DialogSwitchViewModel.EditorMode == EditorMode.Create)
					*/
			});
			SelectedAllCommand = new Command(() => { Debug.Write(nameof(SelectedAllCommand)); });
			UnselectedAllCommand = new Command(() => { Debug.Write(nameof(UnselectedAllCommand)); });
			CellContentCheckBoxChecked = new Command(() => { Debug.Write(nameof(CellContentCheckBoxChecked)); });
			CellContentCheckBoxUnchecked = new Command(() => { Debug.Write(nameof(CellContentCheckBoxChecked)); });
			SelectedRozhodciCount = 10;
		}

		public DialogSwitchViewModel DialogSwitchViewModel { get; set; }

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

		public List<int> RawPages
		{
			get => _rawPages;
			set => SetAndRaise(ref _rawPages, value);
		}

		private async Task LoadRozhodciAsync()
		{
			for (int i = 0; i < 5; i++)
			{
				Rozhoci.Add(new Rozhodci(i.ToString(), "Test", DateTime.Now, "Address", "City", i));
				await Task.Delay(50);
			}
		}
	}
}
