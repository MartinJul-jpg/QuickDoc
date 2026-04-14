using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace QuickDoc.Model
{
    public class Project
    {
        public int ProjectNumber;
        public string Description;
        public List<Unit> Units;
        public List<Document> Documents;

        public Project(int projectNumber, string description)
        {
            ProjectNumber = projectNumber;
            Description = description;
        }
    }
}
