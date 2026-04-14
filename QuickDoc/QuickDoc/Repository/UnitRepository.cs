using System;
using System.Collections.Generic;
using System.Text;
using QuickDoc.Model;

namespace QuickDoc.Repository
{
    class UnitRepository
    {
        private List<Unit> units; 

        public List<Unit> GetUnits()
        {
            return units;
        } 

        private void readFromDatabase()
        {

        }
    }
}
