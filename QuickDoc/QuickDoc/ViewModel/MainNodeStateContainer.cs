using System.Collections.ObjectModel;

namespace QuickDoc.ViewModel
{
    internal class MainNodeStateContainer
    {
        public MainNodeStateContainer PriorNode { get; set; }
        public NodeViewModel CurrentNode { get; set; }
        public ObservableCollection<NodeViewModel> Children { get; set; }
        public ObservableCollection<DocumentViewModel> Documents { get; set; }

        public MainNodeStateContainer(NodeViewModel currentNode, ObservableCollection<NodeViewModel> children, ObservableCollection<DocumentViewModel> documents)
        {
            CurrentNode = currentNode;
            Children = children;
            Documents = documents;
        }

        public MainNodeStateContainer(MainNodeStateContainer priorNode, NodeViewModel currentNode, ObservableCollection<NodeViewModel> children, ObservableCollection<DocumentViewModel> documents) : this(currentNode, children, documents)
        {
            PriorNode = priorNode;
        }
    }
}
