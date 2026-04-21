using System;
using System.Collections.Generic;
using System.Text;

namespace QuickDoc.Model
{
    public class Section
    {
        public int SectionNumber;
        public int OldSecTionNumber;
        public string Title;
        public string ParentKey; // From UnitNumber cause otherwise there is no correlation bettween parent and child 
        public List<Tag> Tags;
        public List<Document> Documents;

        public Section(int sectionNumber, int oldSecTionNumber, string title, string parentKey)
        {
            SectionNumber = sectionNumber;
            OldSecTionNumber = oldSecTionNumber;
            Title = title;
            ParentKey = parentKey;
            Tags = new List<Tag>(); // <- prevents another NullReferenceException
            Documents = new List<Document>(); // <- prevents the NullReferenceException
        }

        public override string ToString()
        {
            return $"Section: {SectionNumber} - {Title}";
        }
    }
}
