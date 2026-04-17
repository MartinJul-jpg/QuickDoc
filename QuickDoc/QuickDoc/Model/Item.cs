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
        public string TagParentKey; // TagNumber
        public Procurement ItemProcurement;
        public List<Document> Documents;

        public Item(int itemID, int itemVariantID, string itemNumber, string lineNumber, string description, float quantity, string unitOfMeasure, string tagParentKey)
        {
            ItemID = itemID;
            ItemVariantID = itemVariantID;
            ItemNumber = itemNumber;
            LineNumber = lineNumber;
            Description = description;
            Quantity = quantity;
            UnitOfMeasure = unitOfMeasure;
            TagParentKey = tagParentKey;
        }

        override public string ToString()
        {
            return $"Item: {ItemNumber} - {Description}";
        }
    }
}
