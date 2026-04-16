using System;
using System.Collections.Generic;
using System.Text;
using QuickDoc.Model;

namespace QuickDoc.ViewModel
{
    public class DocumentViewModel
    {
        private Document document;

        public string Title 
        {
            get {  return document.Title; }
            set { document.Title = value; }
        }
        public string Description 
        { 
            get { return document.Description; }
            set { document.Description = value; }
        }
        public string FilePath 
        { 
            get { return document.FilePath; }
            set { document.FilePath = value; }
        }

        public DocumentViewModel(Document document)
        {
            this.document = document;
        }
    }
}
