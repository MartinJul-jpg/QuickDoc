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

        public List<Section> getSections(int sectionNr)
        {
            // return a specific sections
            return sections = sections.Where(x => x.SectionNumber == sectionNr).ToList();
        }

        public void ReadFromDatabase(string projectNum , TagRepository tagRepo)
        {
            List<Section> result = new List<Section>();
            List<Tag> ResultChildren;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT SC.SectionNumber, SC.OldSectionNumber, SC.Title, SC.UnitNumber,
                                                SCD.STitle, SCD.SDocDescription, SCDSFile   
                                                FROM SECTION SC
                                                INNER JOIN SECTIONDOCUMENT SCD ON SCD.SectionNumber = SD.SectionNumber
                                                WHERE ProjectNumber = @projectNum", con);
                cmd.Parameters.AddWithValue("@projectNum", projectNum);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        int SectionNumber = (int)dr["SectionNumber"];
                        int OldSecTionNumber = (int)dr["OldSecTionNumber"];
                        string Title = (string)dr["Title"];
                        string ParentKey = (string)dr["UnitNumber"];
                        //FILE
                        string title = (string)dr["STitle"];
                        string description = (string)dr["SDocDescription"];
                        string filepath = (string)dr["SCDSFile"];


                        Section section = new Section(SectionNumber, OldSecTionNumber, Title, ParentKey);

                        ResultChildren = tagRepo.GetSectionsChildren(SectionNumber);
                        section.Documents.Add(new Document(title, description, filepath));
                        section.Tags = ResultChildren;
                        result.Add(section);
                    }
                }
                sections = result;
            }
        }
    }
}
