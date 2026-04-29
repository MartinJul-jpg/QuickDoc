using QuickDoc.ViewModel;
using QuickDoc.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace QuickDoc.Command
{
    public class WriteToSerialCommand : ICommand
    {
        private MainNodeViewModel _mainNodeViewModel;
        public WriteToSerialCommand(MainNodeViewModel mvn)
        {
            _mainNodeViewModel = mvn;
        }
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (parameter is NodeViewModel nvm)
            {
                _mainNodeViewModel.WriteToSerialNumber(nvm);
                
            }
        }
    }
}
