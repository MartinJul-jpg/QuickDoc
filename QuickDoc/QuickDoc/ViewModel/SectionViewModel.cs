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
        public List<NodeViewModel> Tags
        {
            get
            {
                List<NodeViewModel> tags = new List<NodeViewModel>();

                foreach (Tag tag in section.Tags)
                {
                    tags.Add(new TagViewModel(tag));
                }

                return tags;
            }
        }
        public List<DocumentViewModel> Documents
        {
            get
            {
                List<DocumentViewModel> documents = new List<DocumentViewModel>();

                foreach (Document document in section.Documents)
                {
                    documents.Add(new DocumentViewModel(document));
                }

                return documents;
            }
        }

        public override List<NodeViewModel> GetChildren()
        {
            return Tags;
        }

        public override List<DocumentViewModel> GetDocuments()
        {
            return Documents;
        }

        public SectionViewModel(Section section)
        {
            this.section = section;
        }

        public override string ToString()
        {
            return $"{SectionNumber} - {Title}";
        }
    }
}





