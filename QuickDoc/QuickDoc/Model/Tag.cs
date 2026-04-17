using System;
using System.Collections.Generic;
using System.Text;

namespace QuickDoc.Model
{
    public class Tag
    {
        public int TagID { get; set; }
        public string TagNumber { get; set; }
        public string UnionTagNumber { get; set; }
        public string Description { get; set; }
        public string LineNumber { get; set; }
        public string HaffmanTag { get; set; }
        public string VendorTag { get; set; }
        public string CustomerTag { get; set; }
        public string BelongsTo { get; set; }

        public int SectionParentKey; // SectionNumber

        public List<Item> Items { get; set; }
        public List<Document> Documents { get; set; }

        public Tag(string tagNumber, string unionTagNumber, string description, string lineNumber, string haffmanTag, string vendorTag, string customerTag, string belongsTo, int sectionParentKey) // Big LINE //How long? just to suffer
        {
            TagNumber = tagNumber;
            UnionTagNumber = unionTagNumber;
            Description = description;
            LineNumber = lineNumber;
            HaffmanTag = haffmanTag;
            VendorTag = vendorTag;
            CustomerTag = customerTag;
            BelongsTo = belongsTo;
            SectionParentKey = sectionParentKey;
        }

        public override string ToString()
            {
                return $"Tag: {TagNumber} - {Description}";
        }
    }
}