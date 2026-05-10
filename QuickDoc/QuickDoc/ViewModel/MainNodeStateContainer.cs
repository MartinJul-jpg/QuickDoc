using System.Collections.ObjectModel;

namespace QuickDoc.ViewModel
{
    public class MainNodeStateContainer
    {
        // Can contain a reference to an instance of itself, is used to keep track of prior states ad infinitum. (but sensibly lol)
        public MainNodeStateContainer PriorNode { get; set; }

        // These three properties represent a snapshot in time of MainNodeViewModel
        public NodeViewModel CurrentNode { get; set; }
        public List<NodeViewModel> Children { get; set; }
        public List<DocumentViewModel> Documents { get; set; }

        public MainNodeStateContainer(NodeViewModel currentNode, List<NodeViewModel> children, List<DocumentViewModel> documents)
        {
            CurrentNode = currentNode;
            Children = children;
            Documents = documents;
        }

        public MainNodeStateContainer(MainNodeStateContainer priorNode, NodeViewModel currentNode, List<NodeViewModel> children, List<DocumentViewModel> documents) : this(currentNode, children, documents)
        {
            PriorNode = priorNode;
        }
    }
}
