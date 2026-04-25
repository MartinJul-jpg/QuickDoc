using QuickDoc.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuickDoc.ViewModel
{
    public class TagViewModel : NodeViewModel
    {
        private Tag tag;
        public string TagNumber
        {
            get { return tag.TagNumber; }
            set { tag.TagNumber = value; }
        }
        public string UnionTagNumber
        {
            get { return tag.UnionTagNumber; }
            set { tag.UnionTagNumber = value; }
        }
        public string Description
        {
            get { return tag.Description; }
            set { tag.Description = value; }
        }
        public string LineNumber
        {
            get { return tag.LineNumber; }
            set { tag.LineNumber = value; }
        }
        public string HaffmanTag
        {
            get { return tag.HaffmanTag; }
            set { tag.HaffmanTag = value; }
        }
        public string VendorTag
        {
            get { return tag.VendorTag; }
            set { tag.VendorTag = value; }
        }
        public string CustomerTag
        {
            get { return tag.CustomerTag; }
            set { tag.CustomerTag = value; }
        }
        public string BelongsTo
        {
            get { return tag.BelongsTo; }
            set { tag.BelongsTo = value; }
        }
        public List<NodeViewModel> Items
        {
            get
            {
                List<NodeViewModel> items = new List<NodeViewModel>();

                foreach (Item item in tag.Items)
                {
                    items.Add(new ItemViewModel(item));
                }

                return items;
            }
        }
        public List<DocumentViewModel> Documents
        {
            get
            {
                List<DocumentViewModel> documents = new List<DocumentViewModel>();

                foreach (Document document in tag.Documents)
                {
                    documents.Add(new DocumentViewModel(document));
                }

                return documents;
            }
        }

        public override List<NodeViewModel> GetChildren()
        {
            return Items;
        }

        public override List<DocumentViewModel> GetDocuments()
        {
            return Documents;
        }

        public TagViewModel(Tag tag)
        {
            this.tag = tag;
        }

        public override string ToString()
        {
            return $"{TagNumber} - {Description}";
        }
    }
}
