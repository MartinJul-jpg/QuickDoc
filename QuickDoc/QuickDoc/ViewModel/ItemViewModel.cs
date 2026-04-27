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
        public string SerialNumber
        {
            get { return item.SerialNumber; }
            set { item.SerialNumber = value; }
        }

        public Procurement ItemProcurement
        {
            get { return item.ItemProcurement; }
            set { item.ItemProcurement = value; }
        }
        public List<DocumentViewModel> Documents
        {
            get
            {
                List<DocumentViewModel> documents = new List<DocumentViewModel>();

                foreach (Document document in item.Documents)
                {
                    documents.Add(new DocumentViewModel(document));
                }

                return documents;
            }
        }

        public override List<NodeViewModel> GetChildren()
        {
            return new List<NodeViewModel>();
        }

        public override List<DocumentViewModel> GetDocuments()
        {
            return Documents;
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
