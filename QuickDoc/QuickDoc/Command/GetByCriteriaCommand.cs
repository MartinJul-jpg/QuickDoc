using QuickDoc.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using QuickDoc.View;

namespace QuickDoc.Command
{
    public class GetByCriteriaCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        // Grays out the command bound button when the project box is empty. 
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

        // Calls the method, GetByCriteria(), in MainViewModel that has the responsibility to change the ViewModels (contents) that live there. 
        // Secondly changes the view, the views whose contents bind to the ViewModels that live in the MainViewModel.  
        public void Execute(object parameter)
        {
            if (parameter is MainNodeViewModel mnvm)
            {
                // The call. 
                mnvm.GetByCriteria();

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
