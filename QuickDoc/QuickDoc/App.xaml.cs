using QuickDoc.ViewModel;
using System.Configuration;
using System.Data;
using System.Windows;

namespace QuickDoc
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        MainNodeViewModel mainNodeVM;

        protected override void OnStartup(StartupEventArgs e)
        {
            mainNodeVM = new MainNodeViewModel();

            // Create and show the main window
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            base.OnStartup(e);
        }
    }
}
