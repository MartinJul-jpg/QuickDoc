using QuickDoc.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuickDoc.ViewModel
{
    public class TagViewModel : NodeViewModel
    {
        private Tag tag;
        public int TagID { get; set; }
        public string TagNumber { get; set; } = string.Empty;
        public string UnionTagNumber { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string LineNumber { get; set; } = string.Empty;
        public string HaffmanTag { get; set; } = string.Empty;
        public string VendorTag { get; set; } = string.Empty;
        public string CustomerTag { get; set; } = string.Empty;
        public string BelongsTo { get; set; } = string.Empty;
        public List<Item> Items { get; set; } = new();
        public List<Document> Documents { get; set; } = new();
    }
}
