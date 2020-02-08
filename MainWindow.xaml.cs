using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;

namespace Referee
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Helper helper = new Helper();
            helper.SettingsFileExits();
            InitializeComponent();
            GridProUserControl.Children.Add(dialogHost);
            //MainGrid.Children.Add(NovyZavodnikDialogHost);
            MenuListView.SelectedIndex = 0;
        }

        //private DialogHost NovyZavodnikDialogHost = new DialogHost();

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
                this.DragMove();
            }
            catch { }
        }

        private void OpenCloseMenuButton_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)OpenCloseMenuButton.IsChecked)
            {
                Storyboard OpenMenu = (Storyboard)OpenCloseMenuButton.FindResource("OpenMenu");
                OpenMenu.Begin();
                dialogHost.IsOpen = true;
            }
            else
            {
                Storyboard CloseMenu = (Storyboard)OpenCloseMenuButton.FindResource("CloseMenu");
                CloseMenu.Begin();
                dialogHost.IsOpen = false;
            }
        }

        private void MenuGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            if (MenuGrid.Width > 180)
            {
                Storyboard CloseMenu = (Storyboard)OpenCloseMenuButton.FindResource("CloseMenu");
                CloseMenu.Begin();
                OpenCloseMenuButton.IsChecked = false;
                dialogHost.IsOpen = false;
            }
        }

        private DialogHost dialogHost = new DialogHost();

        private void MenuListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MenuGrid.Width > 180)
            {
                Storyboard CloseMenu = (Storyboard)OpenCloseMenuButton.FindResource("CloseMenu");
                CloseMenu.Begin();
                OpenCloseMenuButton.IsChecked = false;
                dialogHost.IsOpen = false;
            }
            GridProUserControl.Children.Clear();
            int index = MenuListView.SelectedIndex;
            switch (index)
            {
                case 0:
                    DirectPrintUserControl directPrintUserControl = new DirectPrintUserControl();
                    GridProUserControl.Children.Add(directPrintUserControl);
                    break;
                case 1:
                    TechnickaCetaUserControl technickaCetaUserControl = new TechnickaCetaUserControl();
                    GridProUserControl.Children.Add(technickaCetaUserControl);
                    break;
                case 2:
                    NastaveniUserControl nastaveniUserControl = new NastaveniUserControl();
                    GridProUserControl.Children.Add(nastaveniUserControl);
                    break;
            }
            GridProUserControl.Children.Add(dialogHost);
        }
    }
}
