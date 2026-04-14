using QuickDoc.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuickDoc.ViewModel
{
    public class TagViewModel : NodeViewModel
    {
        private Tag tag;

        public int TagID
        {
            get { return tag.TagID; }
            set { tag.TagID = value; }
        }
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
        public List<Item> Items
        {
            get { return tag.Items; }
            set { tag.Items = value; }
        }
        public List<Document> Documents
        {
            get { return tag.Documents; }
            set { tag.Documents = value; }
        }

        public TagViewModel(Tag tag)
        {
            this.tag = tag;
        }
    }
}
