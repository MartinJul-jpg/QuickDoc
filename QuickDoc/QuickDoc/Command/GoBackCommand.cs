using QuickDoc.View;
using QuickDoc.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace QuickDoc.Command
{
    public class GoBackCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        // Calls the method, GoBack(), in MainViewModel that has the responsibility to change the ViewModels (contents) that live there. 
        // Secondly changes the view, the views whose contents bind to the ViewModels that live in the MainViewModel.  
        public void Execute(object parameter)
        {
            if (parameter is MainNodeViewModel mnvm)
            {
                // If there is a prior state, simply sets the view to be a NodeView. 
                if (mnvm.PriorNode != null)
                {
                    mnvm.GoBack();
                    mnvm.NavigationStore.CurrentView = new NodeView(mnvm.NavigationStore);
                }
                // If not, sets view to be a SearchView. 
                else
                {
                    mnvm.GoBack();
                    mnvm.NavigationStore.CurrentView = new SearchView(mnvm.NavigationStore);
                }
            }
        }
    }
}
