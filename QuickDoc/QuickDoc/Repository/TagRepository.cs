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
                SqlCommand cmd = new SqlCommand(@"SELECT TG.TagNumber, TG.UnionTagNumber, TG.TagLineNumber, TG.TagDescription, TG.SectionNumber, 
                                                TGD.TTitle, TGD.TDocDescription, TGD.TFile
                                                FROM TAG TG
                                                INNER JOIN TAGDOCUMENT TGD ON TG.TagNumber = TGD.TagNumber
                                                WHERE TG.ProjectNumber = @projectNum", con);
                
                //SqlCommand cmd = new SqlCommand(@"SELECT TG.TagNumber, TG.UnionTagNumber, TG.TagLineNumber, TG.TagDescription, TG.HaffmanTag, TG.CustomerTag, TG.VendorTag, TG.BelongsTo, TG.SectionNumber, 
                //                                TGD.TTitle, TGD.TDocDescription, TGD.TFile
                //                                FROM TAG TG
                //                                INNER JOIN TAGDOCUMENT TGD ON TG.TagNumber = TGD.TagNumber
                //                                WHERE TG.ProjectNumber = @projectNum", con);



                cmd.Parameters.AddWithValue("@projectNum", projectNum);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string tagNum = (string)dr["TagNumber"];
                        string unionTag = (string)dr["UnionTagNumber"];
                        string lineNum = (string)dr["TagLineNumber"];
                        string description = (string)dr["TagDescription"];
                        //string haffmanTag = (string)dr["HaffmanTag"];
                        //string customerTag = (string)dr["CustomerTag"];
                        //string vendorTag = (string)dr["VendorTag"];
                        //string belongsTo = (string)dr["BelongsTo"];
                        int sectionNr = (int)dr["SectionNumber"];
                        //FILE
                        string title = (string)dr["TTitle"];
                        string fileDescription = (string)dr["TDocDescription"];
                        string filepath = (string)dr["TFile"];

                        //Tag tag = new Tag(tagNum, unionTag, lineNum, description, haffmanTag, customerTag, vendorTag, belongsTo, sectionNr);
                        Tag tag = new Tag(tagNum, unionTag, lineNum, description, sectionNr);

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
