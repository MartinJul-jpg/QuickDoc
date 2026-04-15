using System;
using System.Collections.Generic;
using System.Text;

namespace QuickDoc.Model
{
    public class Document
    {
        public string Title;
        public string Description;
        public string FilePath;


    public Document(string title, string description, string filepath)
        {
            Title = title;
            Description = description;
            FilePath = filepath;

        }
    }
}
