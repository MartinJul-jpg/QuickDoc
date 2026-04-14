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

        public List<Item> GetItems()
        {
            return null;
        }

        public void ReadFromDatabase(int projectNum, ItemRepository ItemRepo)
        {
            List<Item> ResultChildren;
            List<Tag> result = new List<Tag>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT TagNumber, UnionTagNumber, TagLineNumber, TagDescription, HaffmanTag, CustomerTag, VendorTag, BelongsTo " 
                                                + "FROM TAG WHERE ProjectNumber = @projectNum", con);
                cmd.Parameters.AddWithValue("@projectNum", projectNum);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string tagNum = (string)dr["TagNumber"];
                        string unionTag = (string)dr["UnionTagNumber"];
                        string lineNum = (string)dr["TagLineNumber"];
                        string description = (string)dr["TagDescription"];
                        string haffmanTag = (string)dr["HaffmanTag"];
                        string customerTag = (string)dr["CustomerTag"];
                        string vendorTag = (string)dr["VendorTag"];
                        string belongsTo = (string)dr["BelongsTo"];

                        Tag tag = new Tag(tagNum, unionTag, lineNum, description, haffmanTag, customerTag, vendorTag, belongsTo);
                        //For Children

                        ResultChildren = ItemRepo.GetTagsChildren(tagNum);

                        tag.Items = ResultChildren;
                        result.Add(tag);

                    }
                }
                tags = result;
            }
        }
    }
}
