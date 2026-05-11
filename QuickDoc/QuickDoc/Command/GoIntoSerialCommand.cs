using QuickDoc.View;
using QuickDoc.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace QuickDoc.Command
{
    public class GoIntoSerialCommand : ICommand
    {
        private MainNodeViewModel mnvm;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        // Has a custom constructor that takes a MainNodeViewModel, normally this would be provided in the commandparameter. 
        public GoIntoSerialCommand(MainNodeViewModel mnvm)
        {
            this.mnvm = mnvm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        // Calls the method, GoInto(), in MainViewModel that has the responsibility to change the ViewModels (contents) that live there. 
        // Secondly changes the view, in this case to SpecificItemView, the views whose contents bind to the ViewModels that live in the MainViewModel.  
        // The parameter given here is, necessarily but differently, the child node that has to do with the button being clicked. 
        public void Execute(object? parameter)
        {
            if (parameter is NodeViewModel nvm)
            {
                // Sets SelectedChild to be the child node in question. 
                mnvm.SelectedChild = nvm;
                // The call. 
                mnvm.GoInto();
                // The view change. 
                mnvm.NavigationStore.CurrentView = new SpecificItemView(mnvm.NavigationStore);
            }
        }
    }
}
