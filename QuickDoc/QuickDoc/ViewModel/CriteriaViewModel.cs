using System;
using System.Collections.Generic;
using System.Text;

namespace QuickDoc.ViewModel
{
    public class CriteriaViewModel
    {
        // These 5 properties represent the 5 boxes of different criteria you can enter info into. 
        public string ProjectCriteria { get; set; }
        public string UnitCriteria { get; set; }
        public int SectionCriteria { get; set; }
        public string TagCriteria { get; set; }
        public string ItemCriteria { get; set; }

        // This property represents the box that one would like to write a bar-code into. 
        // It has no backing field and merely reflects what is present in the 5 criteria. 
        public string ScanCriteria 
        {
            get 
            {
                return $"{ProjectCriteria};{UnitCriteria};{SectionCriteria};{TagCriteria};{ItemCriteria}";
            }
            set
            {
                string[] scanCriteria = value.Split(';');

                if (scanCriteria.Count() == 5)
                {
                    ProjectCriteria = scanCriteria[0];
                    UnitCriteria = scanCriteria[1];
                    if (int.TryParse(scanCriteria[2], out int sectionNumber)) { SectionCriteria = sectionNumber; }
                    TagCriteria = scanCriteria[3];
                    ItemCriteria = scanCriteria[4];
                }
            } 
        }
    }
}
