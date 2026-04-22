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
        public string Quantity;
        public string UnitOfMeasure;
        public string TagParentKey; // TagNumber
        public string SerialNumber;
        public Procurement ItemProcurement;
        public List<Document> Documents;

        public Item(int itemID ,int itemVariantID, string itemNumber, string lineNumber, string description , string quantity , string unitOfMeasure, string tagParentKey, string serialNumber)
        {
            ItemID = itemID;
            ItemVariantID = itemVariantID;
            ItemNumber = itemNumber;
            LineNumber = lineNumber;
            Description = description;
            Quantity = quantity;
            UnitOfMeasure = unitOfMeasure;
            TagParentKey = tagParentKey;
            SerialNumber = serialNumber;
            Documents = new List<Document>();
        }

        override public string ToString()
        {
            return $"Item: {ItemNumber} - {Description}";
        }
    }
}
