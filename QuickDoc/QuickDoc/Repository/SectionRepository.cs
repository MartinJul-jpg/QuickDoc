using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using QuickDoc.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Documents;
using QuickDoc.Model;
using Section = QuickDoc.Model.Section; // Section is to broad otherwise
using System.Net; 

namespace QuickDoc.Repository
{
    public class SectionRepository
    {

        private List<Section> sections;
        private readonly string ConnectionString;


        public SectionRepository()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            ConnectionString = config.GetConnectionString("MyDBConnection");
        }
        //TO FIND THE PARENT
        public Section getSection(int sectionNr, string unitNr, UnitRepository unitRepo)
        {
           return unitRepo.GetSpecificChild(unitNr, sectionNr);
        }

        public List<Section> GetUnitsChildren(string unitNr)
        {
            return sections.Where(x => x.ParentKey == unitNr).ToList();
        }

        public List<Section> GetSections(int sectionNr)
        {
            // return specific sections
            return sections.Where(x => x.SectionNumber == sectionNr).ToList();
        }

        public void ReadFromDatabase(string projectNum , TagRepository tagRepo)
        {
            List<Section> result = new List<Section>();
            List<Tag> ResultChildren;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"EXEC sp_ReadSectionFromDatabase @projectNum", con);
                cmd.Parameters.AddWithValue("@projectNum", projectNum);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        //CANNOT CAST CAUSE IT CANNOT HANDLE NULL VALUES FROM DATABASE!!! TERINARY OPERATOR TO CHECK FOR NULL 
                        int SectionNumber = dr["SectionNumber"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SectionNumber"]);
                        int OldSectionNumber = dr["OldSectionNumber"] == DBNull.Value ? 0 : Convert.ToInt32(dr["OldSectionNumber"]);
                        string Title = dr["Title"] == DBNull.Value ? "" : Convert.ToString(dr["Title"]);
                        string ParentKey = dr["UnitNumber"] == DBNull.Value ? "" : Convert.ToString(dr["UnitNumber"]);

                        // FILE
                        string title = dr["STitle"] == DBNull.Value ? "" : Convert.ToString(dr["STitle"]);
                        string description = dr["SDocDescription"] == DBNull.Value ? "" : Convert.ToString(dr["SDocDescription"]);
                        string filepath = dr["SFile"] == DBNull.Value ? "" : Convert.ToString(dr["SFile"]);


                        Section section = new Section(SectionNumber, OldSectionNumber, Title, ParentKey);

                        ResultChildren = tagRepo.GetSectionsChildren(SectionNumber, ParentKey);

                        if (!(title == "" && description == "" && filepath == ""))
                        {
                            section.Documents.Add(new Document(title, description, filepath));
                        }

                        section.Tags = ResultChildren;
                        result.Add(section);
                    }
                }
                sections = result;
            }
        }
    }
}
