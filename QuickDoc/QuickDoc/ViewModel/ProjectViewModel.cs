using QuickDoc.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuickDoc.ViewModel
{
    public class ProjectViewModel : NodeViewModel
    {
        private Project project;
        public int ProjectNumber;
        public string Description;
        public List<Unit> Units;
        public List<Document> Documents;
    }
}
