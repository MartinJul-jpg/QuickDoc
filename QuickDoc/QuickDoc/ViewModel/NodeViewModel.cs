using System;
using System.Collections.Generic;
using System.Text;

namespace QuickDoc.ViewModel
{
    public abstract class NodeViewModel
    {
        public abstract List<NodeViewModel> GetChildren();

        public abstract List<DocumentViewModel> GetDocuments();
    }
}
