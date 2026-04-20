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
            units = new List<Unit>();
        }
        public Unit GetUnit(string unitNr)
        {
            Unit unit;
            bool exists = units.Any(x => x.UnitNumber == unitNr);
            if (exists == false)
            {
                return new Unit("", "") { Documents = new List<Document>(), Sections = new List<Section>() };

            }
            else
            {
                unit = units.Where(x => x.UnitNumber == unitNr).First();
            }
            

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
                                                LEFT JOIN UNITDOCUMENT UND ON UN.UnitNumber = UND.UnitNumber
                                                WHERE UN.ProjectNumber = @projectNum", con);
                cmd.Parameters.AddWithValue("@projectNum", projectNum);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string UnitNumber = dr["UnitNumber"] == DBNull.Value ? "" : Convert.ToString(dr["UnitNumber"]);
                        string UnitDescription = dr["UnitDescription"] == DBNull.Value ? "" : Convert.ToString(dr["UnitDescription"]);
                        //FILE DATA
                        string title = dr["UTitle"] == DBNull.Value ? "" : Convert.ToString(dr["UTitle"]);
                        string description = dr["UDocDescription"] == DBNull.Value ? "" : Convert.ToString(dr["UDocDescription"]);
                        string filepath = dr["UFile"] == DBNull.Value ? "" : Convert.ToString(dr["UFile"]);

                        Unit unit = new Unit(UnitNumber, UnitDescription);
                        //For Children

                        ResultChildren = secRepo.GetUnitsChildren(UnitNumber);

                        if (!(title == "" && description == "" && filepath == ""))
                        {
                            unit.Documents.Add(new Document(title, description, filepath));
                        }
                        
                        unit.Sections = ResultChildren;
                        result.Add(unit);
                   
                    }
                }
                units = result;
            }
        }
    }
}
