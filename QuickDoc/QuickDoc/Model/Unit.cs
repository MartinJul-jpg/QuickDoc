using System;
using System.Collections.Generic;
using System.Text;

namespace QuickDoc.Model
{
    public class Unit
    {
        public string UnitNumber;
        public string Description;
        public List<Section> Sections;
        public List<Document> Documents;

        public Unit(string unitNumber, string description)
        {
            UnitNumber = unitNumber;
            Description = description;
            Documents = new List<Document>(); 

        public override string ToString()
        {
            return $"Unit: {UnitNumber} - {Description}";
        }
    }
}
