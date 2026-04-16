using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using QuickDoc.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuickDoc.Repository
{
    public class UnitRepository
    {
        private List<Unit> units;
        private readonly string ConnectionString;

        public UnitRepository()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
             ConnectionString = config.GetConnectionString("MyDBConnection");
        }
        public Unit GetUnit(string unitNr)
        {
            Unit unit = units.Where(x => x.UnitNumber == unitNr).ToList().First();
            return unit;
        } 

        public List<Unit> GetAllUnits()
        {
            return new List<Unit>(units);
        }

        // FIND A SPECIFIC CHILD
        public Section GetSpecificChild(string unitNr, int sectionNr)
        {
            Unit unit = units.Where(x => x.UnitNumber == unitNr).ToList().First();
            return unit.Sections.Where(x => x.SectionNumber == sectionNr).ToList().First();
        }
        public void ReadFromDatabase(string projectNum, SectionRepository secRepo)
        {
            List<Section> ResultChildren;
            List<Unit> result = new List<Unit>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT UN.UnitNumber, UN.UnitDescription,
                                                UND.UTitle, UND.UDocDescription, UND.UFile
                                                FROM UNIT UN
                                                INNER JOIN UNITDOCUMENT UND WHERE ProjectNumber = @projectNum", con);
                cmd.Parameters.AddWithValue("@projectNum", projectNum);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string UnitNumber = (string)dr["UnitNumber"];
                        string UnitDescription = (string)dr["UnitDescription"];
                        //FILE DATA
                        string title = (string)dr["UTitle"];
                        string description = (string)dr["UDocDescription"];
                        string filepath = (string)dr["UFile"];

                        Unit unit = new Unit(UnitNumber, UnitDescription);
                        //For Children

                        ResultChildren = secRepo.GetUnitsChildren(UnitNumber);
                        unit.Documents.Add(new Document(title, description, filepath));
                        unit.Sections = ResultChildren;
                        result.Add(unit);
                   
                    }
                }
                units = result;
            }
        }
    }
}
