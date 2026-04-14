using QuickDoc.Model;
using System;
using System.Collections.Generic;
using System.Text;


namespace QuickDoc.ViewModel
{
    public class SectionViewModel : NodeViewModel
    {
        private Section section;

        public int SectionNumber
        {
            get { return section.SectionNumber; }
            set { section.SectionNumber = value; }
        }
        public int OldSectionNumber
        {
            get { return section.OldSecTionNumber; }
            set { section.OldSecTionNumber = value; }
        }
        public string Title
        {
            get { return section.Title; }
            set { section.Title = value; }
        }
        public List<Document> Documents
        {
            get { return section.Documents; }
            set { section.Documents = value; }
        }
        public List<Item> Children
        {
            get { return section.Items; }
            set { section.Items = value; }
        }

        public SectionViewModel(Section section)
        {
            this.section = section;
        }
    }
}





