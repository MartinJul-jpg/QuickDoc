using QuickDoc.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuickDoc.ViewModel
{
    public class TagViewModel : NodeViewModel
    {
        private Tag tag;
        public int TagID;
        public string TagNumber;
        public string UnionTagNumber;
        public string Description;
        public string LineNumber;
        public string HaffmanTag;
        public string VendorTag;
        public string CustomerTag;
        public string BelongsTo;
        public List<Item> Items;
        public List<Document> Documents;
    }
}
