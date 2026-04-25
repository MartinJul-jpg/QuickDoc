using QuickDoc.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuickDoc.ViewModel
{
    public class UnitViewModel : NodeViewModel
    {
        private Unit unit;

        public string UnitNumber
        {
            get { return unit.UnitNumber; }
            set { unit.UnitNumber = value; }
        }
        public string Description
        {
            get { return unit.Description; }
            set { unit.Description = value; }
        }
        public List<NodeViewModel> Sections
        {
            get
            {
                List<NodeViewModel> sections = new List<NodeViewModel>();

                foreach (Section section in unit.Sections)
                {
                    sections.Add(new SectionViewModel(section));
                }

                return sections;
            }
        }
        public List<DocumentViewModel> Documents
        {
            get
            {
                List<DocumentViewModel> documents = new List<DocumentViewModel>();

                foreach (Document document in unit.Documents)
                {
                    documents.Add(new DocumentViewModel(document));
                }

                return documents;
            }
        }

        public override List<NodeViewModel> GetChildren()
        {
            return Sections;
        }

        public override List<DocumentViewModel> GetDocuments()
        {
            return Documents;
        }

        public UnitViewModel(Unit unit)
        {
            this.unit = unit;
        }

        public override string ToString()
        {
            return $"{UnitNumber} - {Description}";
        }
    }
}
