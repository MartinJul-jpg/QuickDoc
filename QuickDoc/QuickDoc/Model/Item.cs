using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls.Primitives;

namespace QuickDoc.Model
{
    public class Item
    {
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
