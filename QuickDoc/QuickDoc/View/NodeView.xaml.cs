using QuickDoc.Stores;
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
using QuickDoc.View;

namespace QuickDoc.View
{
    /// <summary>
    /// Interaction logic for NodeView.xaml
    /// </summary>
    public partial class NodeView : UserControl
    {
        public NavigationStore navigationStore { get; set; }

        public NodeView(NavigationStore navigationStore)
        {
            this.navigationStore = navigationStore;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            navigationStore.CurrentView = new SearchView(navigationStore);
        }
    }
}
