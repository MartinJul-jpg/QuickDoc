using System;
using System.Collections.Generic;
using System.Text;

namespace QuickDoc.ViewModel
{
    public class CriteriaViewModel
    {
        public string ProjectCriteria { get; set; } = string.Empty;
        public string UnitCriteria { get; set; } = string.Empty;
        public int SectionCriteria { get; set; }
        public string TagCriteria { get; set; } = string.Empty;
        public string ItemCriteria { get; set; } = string.Empty;
    }
}
