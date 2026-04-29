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

        public void Execute(object parameter)
        {
            if (parameter is MainNodeViewModel mnvm)
            {
                if (mnvm.PriorNode != null)
                {
                    mnvm.GoBack();
                    mnvm.NavigationStore.CurrentView = new NodeView(mnvm.NavigationStore);
                }
                else
                {
                    mnvm.GoBack();
                    mnvm.NavigationStore.CurrentView = new SearchView(mnvm.NavigationStore);
                }
            }
        }
    }
}
