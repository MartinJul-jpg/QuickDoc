using QuickDoc.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace QuickDoc.Commands
{
    public class NavigateCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            throw new NotImplementedException();
        }
    }
}
