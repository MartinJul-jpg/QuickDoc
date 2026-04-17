using System;
using System.Collections.Generic;
using System.Text;

namespace QuickDoc.ViewModel
{
    internal class MainNodeStateContainer
    {
        public MainNodeStateContainer PriorNode { get; set; }
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
