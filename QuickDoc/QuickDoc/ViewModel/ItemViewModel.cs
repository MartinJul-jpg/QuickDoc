using QuickDoc.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuickDoc.ViewModel
{
    public class ItemViewModel : NodeViewModel
    {
        private Item item;
        public int ItemID { get; set; }
        public int ItemVariantID { get; set; }
        public string ItemNumber { get; set; } = string.Empty;
        public string LineNumber { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public float Quantity { get; set; }
        public string UnitOfMeasure { get; set; } = string.Empty;
        public Procurement? ItemProcurement { get; set; }
        public List<Document> Documents { get; set; } = new();
    }
}
