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
        public List<Section> Children
        {
            get { return unit.Sections; }
            set { unit.Sections = value; }
        }

        public List<Document> Documents
        {
            get { return unit.Documents; }
            set { unit.Documents = value; }
        }

        public UnitViewModel(Unit unit)
        {
            this.unit = unit;
        }
    }
}
