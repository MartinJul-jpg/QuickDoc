using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using System.Windows.Controls;

namespace QuickDoc.ViewModel
{
    public class MainNodeViewModel
    {
        public NodeViewModel CurrentNode;
        public List<Document> Documents { get; set; }
        public string ProjectCriteria { get; set; }
        public int UnitCriteria { get; set; }
        public int SectionCriteria { get; set; }
        public string TagCriteria { get; set; }
        public string ItemCriteria { get; set; }

        public void GetNodesByCriteria()
        {

        }
    }
}