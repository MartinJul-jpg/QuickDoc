using QuickDoc.Stores;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using QuickDoc.Model;

namespace QuickDoc.ViewModel
{
    public class MainNodeViewModel : INotifyPropertyChanged
    {
        public NavigationStore NavigationStore { get; set; }

        private NodeViewModel? _currentNode;
        public NodeViewModel? CurrentNode
        {
            get => _currentNode;
            set
            {
                if (ReferenceEquals(_currentNode, value))
                {
                    return;
                }

                _currentNode = value;
                OnPropertyChanged();
            }
        }
        public List<Document> Documents { get; set; }
        public string ProjectCriteria { get; set; }
        public int UnitCriteria { get; set; }
        public int SectionCriteria { get; set; }
        public string TagCriteria { get; set; }
        public string ItemCriteria { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void GetNodesByCriteria()
        {

        }
    }
}