using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace QuickDoc.ViewModel
{
    public class CriteriaViewModel
    {
        private string _projectCriteria;
        public string ProjectCriteria
        { 
            get { return _projectCriteria; }
            set
            {
                _projectCriteria = value;
                _scanCriteria = ScanCriteria;
            }
        }

        private string _unitCriteria;
        public string UnitCriteria
        { 
            get { return _unitCriteria; }
            set
            {
                _unitCriteria = value;
                _scanCriteria = ScanCriteria;
            }
        }

        private int _sectionCriteria;
        public int SectionCriteria
        { 
            get { return _sectionCriteria; }
            set
            {
                _sectionCriteria = value;
                _scanCriteria = ScanCriteria;
            }
        }

        private string _tagCriteria;
        public string TagCriteria
        {
            get { return _tagCriteria; }
            set
            {
                _tagCriteria = value;
                _scanCriteria = ScanCriteria;
            }
        }

        private string _itemCriteria;
        public string ItemCriteria 
        {
            get { return _itemCriteria; }
            set
            {
                _itemCriteria = value;
                _scanCriteria = ScanCriteria;
            }
        }

        public string _scanCriteria;
        public string ScanCriteria 
        {
            get 
            {
                return $"{_projectCriteria};{_unitCriteria};{_sectionCriteria};{_tagCriteria};{_itemCriteria}";
            }
            set
            {
                _scanCriteria = value;

                if ((value.Split(';') is string[] scanCriteria) && scanCriteria.Count() == 5)
                {
                    _projectCriteria = scanCriteria[0];
                    _unitCriteria = scanCriteria[1];
                    if (int.TryParse(scanCriteria[2], out int sectionNumber)) { _sectionCriteria = sectionNumber; }
                    _tagCriteria = scanCriteria[3];
                    _itemCriteria = scanCriteria[4];
                }
            } 
        }
    }
}
