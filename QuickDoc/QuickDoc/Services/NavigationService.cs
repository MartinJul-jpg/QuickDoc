using QuickDoc.Stores;
using QuickDoc.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace QuickDoc.Services
{
    public class NavigationService
    {
        readonly NavigationStore navigationStore;
        readonly Func<UserControl> viewModelFactory;

        public NavigationService(NavigationStore navigationStore, Func<UserControl> viewModelFactory)
        {
            this.navigationStore = navigationStore;
            this.viewModelFactory = viewModelFactory;
        }

        public void Navigate()
        {
            navigationStore.CurrentView = viewModelFactory();
        }
    }
}
