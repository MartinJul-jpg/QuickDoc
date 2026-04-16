using QuickDoc.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuickDoc.ViewModel
{
    public class UnitViewModel : NodeViewModel
    {
        public int me_bombaclaat;
        public string UnitNumber { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<Document> Documents { get; set; } = new();
    }
}
