using System;
using System.Windows;
using Referee.Infrastructure;

namespace Referee.ViewModels
{
	public class DialogSwitchViewModel : AbstractNotifyPropertyChanged
	{
		private readonly string _editorText;
		private readonly string _type;
		private string _editorTitle;
		private string _editorButtonContent;
		private Visibility _create;
		private Visibility _edit;
		private EditorMode _editorMode;

		public DialogSwitchViewModel(string editorText, string type)
		{
			_editorTitle = $"{editorText} {type}";
			_editorButtonContent = editorText;
			_editorText = editorText;
			_type = type;
			_editorMode = EditorMode.Create;
			Create = Visibility.Visible;
			Edit = Visibility.Collapsed;
		}

		public string EditorTitle
		{
			get => _editorTitle;
			set => SetAndRaise(ref _editorTitle, value);
		}

		public string EditorButtonContent
		{
			get => _editorButtonContent;
			set => SetAndRaise(ref _editorButtonContent, value);
		}

		public Visibility Create
		{
			get => _create;
			set => SetAndRaise(ref _create, value);
		}

		public Visibility Edit
		{
			get => _edit;
			set => SetAndRaise(ref _edit, value);
		}

		public EditorMode EditorMode => _editorMode;

		public void SetValues(EditorMode editorMode)
		{
			_editorMode = editorMode;
			switch (editorMode)
			{
				case EditorMode.Create:
					EditorTitle = $"{_editorText} {_type}";
					EditorButtonContent = _editorText;
					Create = Visibility.Visible;
					Edit = Visibility.Collapsed;
					break;
				case EditorMode.Edit:
					EditorTitle = "Upravit " + _type;
					EditorButtonContent = "Upravit";
					Create = Visibility.Collapsed;
					Edit = Visibility.Visible;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(editorMode), editorMode, null);
			}
		}
	}

}
