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
            //DocumentViewModel document = new DocumentViewModel(new Document("Test Document", "This is a test document.", "da"));
            //mainNodeVM.Documents = new List<DocumentViewModel>() { document, document, document, document, document, document, document, document, document, document, document, document, document, document, document, document, document, document, document, };

            //mainNodeVM.CurrentNode = new ItemViewModel(new Item(2,2,"164852", "35005", "Test item", 10, "pcs", "GV-542698"));
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
