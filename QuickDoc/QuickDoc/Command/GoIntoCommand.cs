using QuickDoc.View;
using QuickDoc.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace QuickDoc.Command
{
    public class GoIntoCommand : ICommand
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

                switch (mnvm.SelectedChild)
                {
                    case null:
                        check = false;
                        break;
                }
            }

            CommandManager.InvalidateRequerySuggested();

            return check;
        }

        public void Execute(object parameter)
        {
            if (parameter is MainNodeViewModel mnvm)
            {
                mnvm.GoInto();
                mnvm.NavigationStore.CurrentView = new NodeView(mnvm.NavigationStore);
            }
        }
    }
}
