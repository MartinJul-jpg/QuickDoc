using QuickDoc.Stores;
using QuickDoc.Styles;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuickDoc.View
{
    /// <summary>
    /// Interaction logic for SearchView.xaml
    /// </summary>
    public partial class SearchView : UserControl
    {
        public NavigationStore navigationStore { get; set; }

        public SearchView(NavigationStore navigationStore)
        {
            this.navigationStore = navigationStore;
            InitializeComponent();
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            navigationStore.CurrentView = new NodeView(navigationStore);
        }
    }
}
