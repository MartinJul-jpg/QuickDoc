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
            navigationStore.CurrentView = new SearchView(navigationStore);

            mainNodeVM = new MainNodeViewModel();
            mainNodeVM.NavigationStore = navigationStore;
            
            // Test code 
            Document document = new Document()
            {
                Title = "Test Document",
                Description = "This is a test document.",
            };
            mainNodeVM.Documents = new List<Document>() { document, document, document, document, document, document, document, document, document, document, document, document, document, document, document, document, document, document, document, };

            mainNodeVM.CurrentNode = new UnitViewModel()
            {
                UnitNumber = "405-B",
                Description = "This is a test unit.",
            };

            // End of test code

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
