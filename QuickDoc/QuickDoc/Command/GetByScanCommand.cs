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

                if (string.IsNullOrEmpty(mnvm.Criteria.ProjectCriteria))

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
                // The call. 
                mnvm.GetByScan();

                // Shorthand Bools. 
                bool tagFull = !string.IsNullOrEmpty(mnvm.Criteria.TagCriteria);
                bool itemFull = !string.IsNullOrEmpty(mnvm.Criteria.ItemCriteria);

                // Only when looking for a specific concrete item do we enter the view equipped to handle extra features relevant in that case. 
                if (tagFull && itemFull)
                {
                    mnvm.NavigationStore.CurrentView = new SpecificItemView(mnvm.NavigationStore);
                }
                // Switches to NodeView. Currently you would either be in the SearchView or ScanView. 
                else
                {
                    mnvm.NavigationStore.CurrentView = new NodeView(mnvm.NavigationStore);
                }
            }
        }
    }
}
