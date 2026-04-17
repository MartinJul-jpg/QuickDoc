using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using QuickDoc.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuickDoc.Repository
{
    public class TagRepository
    {
        private List<Tag> tags;
        private readonly string ConnectionString;

        public TagRepository()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .Build();
            ConnectionString = config.GetConnectionString("MyDBConnection");
        }

        public Tag GetTag(string tagNumber)
        {
            return tags.Where(x => x.TagNumber == tagNumber).First();
        }

        public List<Tag> GetSectionsChildren(int sectionNr)
        {
            return tags.Where(x => x.SectionParentKey == sectionNr).ToList();
        }

        public void ReadFromDatabase(string projectNum, ItemRepository ItemRepo)
        {
            List<Item> ResultChildren;
            List<Tag> result = new List<Tag>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();                
                SqlCommand cmd = new SqlCommand(@"SELECT TG.TagNumber, TG.UnionTagNumber, TG.TagLineNumber, TG.TagDescription, TG.HaffmanTag, TG.CustomerTag, TG.VendorTag, TG.BelongsTo, TG.SectionNumber, 
                                                TGD.TTitle, TGD.TDocDescription, TGD.TFile
                                                FROM TAG TG
                                                LEFT JOIN TAGDOCUMENT TGD ON TG.TagNumber = TGD.TagNumber
                                                WHERE TG.ProjectNumber = @projectNum", con);

                cmd.Parameters.AddWithValue("@projectNum", projectNum);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string tagNum = dr["TagNumber"] == DBNull.Value ? "" : Convert.ToString(dr["TagNumber"]);         
                        string unionTag = dr["UnionTagNumber"] == DBNull.Value ? "" : Convert.ToString(dr["UnionTagNumber"]);
                        string lineNum = dr["TagLineNumber"] == DBNull.Value ? "" : Convert.ToString(dr["TagLineNumber"]);
                        string description = dr["TagDescription"] == DBNull.Value ? "" : Convert.ToString(dr["TagDescription"]);
                        string haffmanTag = dr["HaffmanTag"] == DBNull.Value ? "" : Convert.ToString(dr["HaffmanTag"]);
                        string customerTag = dr["CustomerTag"] == DBNull.Value ? "" : Convert.ToString(dr["CustomerTag"]);
                        string vendorTag = dr["VendorTag"] == DBNull.Value ? "" : Convert.ToString(dr["VendorTag"]);
                        string belongsTo = dr["BelongsTo"] == DBNull.Value ? "" : Convert.ToString(dr["BelongsTo"]);
                        int sectionNr = (int)dr["SectionNumber"];
                        //FILE
                        string title = dr["TTitle"] == DBNull.Value ? "" : Convert.ToString(dr["TTitle"]);
                        string fileDescription = dr["TDocDescription"] == DBNull.Value ? "" : Convert.ToString(dr["TDocDescription"]);
                        string filepath = dr["TFile"] == DBNull.Value ? "" : Convert.ToString(dr["TFile"]);

                        Tag tag = new Tag(tagNum, unionTag, lineNum, description, haffmanTag, customerTag, vendorTag, belongsTo, sectionNr);
                        //For Children

                        ResultChildren = ItemRepo.GetTagsChildren(tagNum);
                        tag.Items = ResultChildren;
                        tag.Documents.Add(new Document(title, fileDescription, filepath));
                        result.Add(tag);

                    }
                }
                tags = result;
            }
        }
    }
}
