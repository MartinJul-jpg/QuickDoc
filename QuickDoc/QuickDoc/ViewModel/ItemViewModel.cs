using QuickDoc.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuickDoc.ViewModel
{
    public class ItemViewModel : NodeViewModel
    {
        private Item item;
        public int ItemID;
        public int ItemVariantID;
        public string ItemNumber;
        public string LineNumber;
        public string Description;
        public float Quantity;
        public string UnitOfMeasure;
        public Procurement ItemProcurement;
        public List<Document> Documents;
    }
}
