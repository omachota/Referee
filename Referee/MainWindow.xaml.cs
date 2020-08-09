using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Referee.Infrastructure.WindowNavigation;
using Referee.ViewModels;

namespace Referee
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _mainViewModel;

        public MainWindow(MainViewModel mainViewModel)
        {
            DataContext = mainViewModel;
            _mainViewModel = mainViewModel;
            Helper helper = new Helper();
            helper.SettingsFileExists();
            InitializeComponent();

            mainViewModel.WindowManager.UpdateWindowCommand.Execute(ViewType.Rozhodci);
            //GridProUserControl.Children.Add(_dialogHost);
            // MenuListView.SelectedIndex = 0;
        }

        private void MenuListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = MenuListBox.SelectedIndex;
            switch (index)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    Environment.Exit(0);
                    break;
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            try
            {
                DragMove();
            }
            catch
            {
                // ignored
            }
        }

        private void OpenCloseMenuButton_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)OpenCloseMenuButton.IsChecked)
            {
                Storyboard openMenu = (Storyboard)OpenCloseMenuButton.FindResource("OpenMenu");
                openMenu.Begin();
                _mainViewModel.IsInfoOpen = true;
            }
            else
            {
                Storyboard closeMenu = (Storyboard)OpenCloseMenuButton.FindResource("CloseMenu");
                closeMenu.Begin();
                _mainViewModel.IsInfoOpen = false;
            }
        }

        private void MenuGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            if (MenuGrid.Width > 180)
            {
                Storyboard closeMenu = (Storyboard)OpenCloseMenuButton.FindResource("CloseMenu");
                closeMenu.Begin();
                OpenCloseMenuButton.IsChecked = false;
                _mainViewModel.IsInfoOpen = false;
            }
        }

        private void MenuListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MenuGrid.Width > 180)
            {
                Storyboard closeMenu = (Storyboard)OpenCloseMenuButton.FindResource("CloseMenu");
                closeMenu.Begin();
                OpenCloseMenuButton.IsChecked = false;
                _mainViewModel.IsInfoOpen = false;
            }
            int index = MenuListView.SelectedIndex;

            switch (index)
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
        }
    }
}
