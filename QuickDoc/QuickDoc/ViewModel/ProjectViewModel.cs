using QuickDoc.Model;
using System;
using System.Collections.Generic;
using System.Text;
using QuickDoc.View;

namespace QuickDoc.ViewModel
{
    public class ProjectViewModel : NodeViewModel
    {
        private Project project;

        public string ProjectNumber
        { 
            get { return project.ProjectNumber; } 
            set { project.ProjectNumber = value; }
        }
        public string Description
        {
            get { return project.Description; }
            set { project.Description = value; }
        }
        public List<NodeViewModel> Units
        {
            get 
            { 
                List<NodeViewModel> units = new List<NodeViewModel>();

                foreach (Unit unit in project.Units)
                {
                    units.Add(new UnitViewModel(unit));
                }

                return units; 
            }
        }
        public List<DocumentViewModel> Documents
        {
            get
            {
                List<DocumentViewModel> documents = new List<DocumentViewModel>();

                foreach (Document document in project.Documents)
                {
                    documents.Add(new DocumentViewModel(document));
                }

                return documents;
            }
        }

        public override List<NodeViewModel> GetChildren()
        {
            return Units;
        }

        public override List<DocumentViewModel> GetDocuments()
        {
            return Documents;
        }

        public ProjectViewModel(Project project)
        {
            this.project = project;
        }

        public override string ToString()
        {
            return $"{ProjectNumber} - {Description}";
        }
    }
}
