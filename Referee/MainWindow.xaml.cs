using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Referee.Infrastructure.SettingsFd;
using Referee.Infrastructure.WindowNavigation;
using Referee.ViewModels;

namespace Referee
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _firstChange = true;
        private readonly MainViewModel _mainViewModel;

        public MainWindow(MainViewModel mainViewModel)
        {
            DataContext = mainViewModel;
            _mainViewModel = mainViewModel;
            InitializeComponent();
            MenuListView.SelectedIndex = _mainViewModel.WindowManager.ActiveViewModelIndex;
        }

        private void OpenCloseMenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (OpenCloseMenuButton.IsChecked != null && OpenCloseMenuButton.IsChecked.Value)
            {
                var openMenu = (Storyboard)OpenCloseMenuButton.FindResource("OpenMenu");
                openMenu.Begin();
                _mainViewModel.IsDialogOpen = true;
            }
            else
            {
                var closeMenu = (Storyboard)OpenCloseMenuButton.FindResource("CloseMenu");
                closeMenu.Begin();
                _mainViewModel.IsDialogOpen = false;
            }
        }

        private void MenuGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            if (MenuGrid.Width > 180)
            {
                var closeMenu = (Storyboard)OpenCloseMenuButton.FindResource("CloseMenu");
                closeMenu.Begin();
                OpenCloseMenuButton.IsChecked = false;
                _mainViewModel.IsDialogOpen = false;
            }
        }

        private void MenuListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MenuGrid.Width > 180)
            {
                var closeMenu = (Storyboard)OpenCloseMenuButton.FindResource("CloseMenu");
                closeMenu.Begin();
                OpenCloseMenuButton.IsChecked = false;
                _mainViewModel.IsDialogOpen = false;
            }

            if (!_firstChange)
                switch (MenuListView.SelectedIndex)
                {
                    case 0:
                        _mainViewModel.WindowManager.UpdateWindowCommand.Execute(ViewType.Rozhodci);
                        break;
                    case 1:
                        _mainViewModel.WindowManager.UpdateWindowCommand.Execute(ViewType.Ceta);
                        break;
                    case 2:
                        _mainViewModel.WindowManager.UpdateWindowCommand.Execute(ViewType.Settings);
                        break;
                }
            else
                _firstChange = !_firstChange;
        }

        protected override async void OnClosed(EventArgs e)
        {
            await SettingsHelper.SaveSettingsAsync(_mainViewModel.Settings);

            base.OnClosed(e);
        }
    }
}
