using QuickDoc.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using QuickDoc.Services;

namespace QuickDoc.Commands
{
    public class NavigateCommand : ICommand
    {
        readonly NavigationService navigationService;

        public NavigateCommand(NavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            navigationService.Navigate();
        }
    }
}
