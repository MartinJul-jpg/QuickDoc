using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace QuickDoc.Model
{
    public class Project
    {
        public string ProjectNumber;
        public string Description;
        public List<Unit> Units;
        public List<Document> Documents;

        public Project(string projectNumber, string description)
        {
            ProjectNumber = projectNumber;
            Description = description;
            Units = new List<Unit>();
            Documents = new List<Document>(); 
        }

        public override string ToString()
        {
            return $"Project: {ProjectNumber} - {Description}";
        }
    }
}
