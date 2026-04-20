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
            set { if (project.ProjectNumber == null)
                {
                    throw new NullReferenceException();
                }
                else { project.ProjectNumber = value; }  }
        }
        public string Description
        {
            get { return project.Description; }
            set { project.Description = value; }
        }
        public List<Unit> Units
        {

            get { return project.Units; }
            set { project.Units = value; }
        }
        public List<Document> Documents
        {
            get { return project.Documents; }
            set { project.Documents = value; }
        }

        public ProjectViewModel(Project project)
        {
            if (project == null)
            {
                project = new Project(string.Empty, string.Empty)
                {
                    Units = new List<Unit>(),
                    Documents = new List<Document>()
                };
            }
            this.project = project;
        }

        public override string ToString()
        {
            return $"{ProjectNumber} - {Description}";
        }
    }
}
