using QuickDoc.View;
using QuickDoc.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace QuickDoc.Command
{
    internal class GoToScanCommand : ICommand
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

        // No calls to MainViewModel as there are no changes that need to be made in the ViewModels that live there. 
        // Merely enters the ScanView, which is an alternate kind of search window. 
        public void Execute(object parameter)
        {
            if (parameter is MainNodeViewModel mnvm)
            {
                mnvm.NavigationStore.CurrentView = new ScanView(mnvm.NavigationStore);
            }
        }
    }
}
