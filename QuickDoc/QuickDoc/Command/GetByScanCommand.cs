using QuickDoc.View;
using QuickDoc.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace QuickDoc.Command
{
    internal class GetByScanCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            bool check = true;

            if (parameter is MainNodeViewModel mnvm)
            {

                if (string.IsNullOrEmpty(mnvm.Criteria.ScanCriteria))

                {
                    check = false;
                }
            }

            CommandManager.InvalidateRequerySuggested();

            return check;
        }

        public void Execute(object parameter)
        {
            if (parameter is MainNodeViewModel mnvm)
            {
                mnvm.GetByScan();
                mnvm.NavigationStore.CurrentView = new NodeView(mnvm.NavigationStore);
            }
        }
    }
}
