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
        public List<Item> Items;
        public List<Document> Documents;
    }
    
}
