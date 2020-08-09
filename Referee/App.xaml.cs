using System.Windows;
using Referee.ViewModels;

namespace Referee
{
    /// <summary>
    /// Interakční logika pro App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Window window = new MainWindow(new MainViewModel());
            window.Show();

            base.OnStartup(e);
        }
    }
}
