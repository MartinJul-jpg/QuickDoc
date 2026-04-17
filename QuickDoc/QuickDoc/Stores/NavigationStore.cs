using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace QuickDoc.Stores
{
    public class NavigationStore : INotifyPropertyChanged
    {
        private UserControl _currentView;
        public UserControl CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
                OnCurrentViewChanged();
            }
        }

        public event Action CurrentViewChanged;

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnCurrentViewChanged()
        {
            if (CurrentViewChanged != null)
            {
                CurrentViewChanged.Invoke();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
