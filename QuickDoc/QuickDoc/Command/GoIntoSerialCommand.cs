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

        public GoIntoSerialCommand(MainNodeViewModel mnvm)
        {
            this.mnvm = mnvm;
        }

        public bool CanExecute(object parameter)
        {
            bool check = true;

            if (parameter is NodeViewModel nvm)
            {
                if (nvm == null)
                {
                    check = false;
                }
            }

            CommandManager.InvalidateRequerySuggested();

            return check;
        }

        public void Execute(object? parameter)
        {
            if (parameter is NodeViewModel nvm)
            {
                mnvm.SelectedChild = nvm;
                mnvm.GoInto();
                //mnvm.NavigationStore.CurrentView = new SpecificItemView(mnvm.NavigationStore);
            }
        }
    }
}
