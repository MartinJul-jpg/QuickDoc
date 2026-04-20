using QuickDoc.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuickDoc.ViewModel
{
    public class ItemViewModel : NodeViewModel
    {
        private Item item;

        public int ItemID
        {
            get { return item.ItemID; }
            set { item.ItemID = value; }
        }
        public int ItemVariantID
        {
            get { return item.ItemVariantID; }
            set { item.ItemVariantID = value; }
        }
        public string ItemNumber
        {
            get { return item.ItemNumber; }
            set { item.ItemNumber = value; }
        }
        public string LineNumber
        {
            get { return item.LineNumber; }
            set { item.LineNumber = value; }
        }
        public string Description
        {
            get { return item.Description; }
            set { item.Description = value; }
        }
        public string Quantity
        {
            get { return item.Quantity; }
            set { item.Quantity = value; }
        }
        public string UnitOfMeasure
        {
            get { return item.UnitOfMeasure; }
            set { item.UnitOfMeasure = value; }
        }
        public Procurement ItemProcurement
        {
            get { return item.ItemProcurement; }
            set { item.ItemProcurement = value; }
        }
        public List<Document> Documents
        {
            get { return item.Documents; }
            set { item.Documents = value; }
        }

        public ItemViewModel(Item item)
        {
            this.item = item;
        }

        public override string ToString()
        {
            return $"{ItemNumber} - {Description}";
        }
    }
}
