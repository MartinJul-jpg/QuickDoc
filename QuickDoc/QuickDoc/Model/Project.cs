using System;
using System.Collections.Generic;
using System.Text;

namespace QuickDoc.Model
{
    public class Project
    {
        public int ProjectNumber;
        public string Description;
        public List<Unit> Units;
        public List<Document> Documents;
    }
}
