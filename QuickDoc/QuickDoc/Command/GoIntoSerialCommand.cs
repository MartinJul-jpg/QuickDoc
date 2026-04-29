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
        private MainNodeViewModel mvm;
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (parameter is NodeViewModel nvm)
            {
                mvm.CurrentNode = nvm;
                mvm.GoInto();
                mvm.NavigationStore.CurrentView = new NodeView(mvm.NavigationStore);
            }
        }
    }
}
