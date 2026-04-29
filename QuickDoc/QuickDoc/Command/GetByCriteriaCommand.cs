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
                mnvm.GetByCriteria();

                bool tagFull = !string.IsNullOrEmpty(mnvm.Criteria.TagCriteria);
                bool itemFull = !string.IsNullOrEmpty(mnvm.Criteria.ItemCriteria);

                if (tagFull && itemFull)
                {
                    mnvm.NavigationStore.CurrentView = new SpecificItemView(mnvm.NavigationStore);
                }
                else
                {
                    mnvm.NavigationStore.CurrentView = new NodeView(mnvm.NavigationStore);
                }
            }
        }
    }
}
