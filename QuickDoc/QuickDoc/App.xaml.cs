using QuickDoc.Stores;
using QuickDoc.View;
using QuickDoc.ViewModel;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using QuickDoc.Model;

namespace QuickDoc
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        MainNodeViewModel mainNodeVM;
        NavigationStore navigationStore;

        protected override void OnStartup(StartupEventArgs e)
        {
            navigationStore = new NavigationStore();
            navigationStore.CurrentView = new SpecificItemView();

            mainNodeVM = new MainNodeViewModel();
            mainNodeVM.NavigationStore = navigationStore;
            mainNodeVM.CurrentNode = new ItemViewModel(new Item(2, 2, "2546", "35005", "Test item", "25", "Meter", "6515489525", "Test"));

            // Create and show the main window
            MainWindow mainWindow = new MainWindow(navigationStore)
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                DataContext = mainNodeVM
            };
            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}
