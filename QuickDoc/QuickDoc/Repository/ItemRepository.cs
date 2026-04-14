using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using QuickDoc.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuickDoc.Repository
{
    public class ItemRepository
    {
        private List<Item> items;
        private readonly string ConnectionString;

        public ItemRepository()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            ConnectionString = config.GetConnectionString("MyDBConnection");
        }

        // To Find a specific item
        public Item GetItem(string itemNumber)
        {
            return items.Where(x => x.ItemNumber == itemNumber).First();
        }
        // To find which tags own the item
        public List<Item> GetTagsChildren(string TagNumber)
        {
            return items.Where(x => x.TagParentKey == TagNumber).ToList();
        }
        
        public List<Item> GetSectionsChildren(int sectionNr )
        {
            return items.Where(x => x.SectionParentKey == sectionNr).ToList(); 
        }

        public void readFromDatabase(int projectNum)
        {
            List<Item> result = new List<Item>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                // 2 joins 
                SqlCommand cmd = new SqlCommand(
                     @"SELECT IT.ItemVariantID, IT.ItemNumber, IT.ItemDescription, 
                        IM.ItemID, IM.ProjectNumber, IM.ItemLineNumber, 
                        IM.UnitOfMeasure, IM.Quantity, IM.TagNumber
                        PR.ProcurementID, PR.ProcurementStatus, PR.PurchaseOrderNumber, TG.SectionNumber
                    FROM ITEMVARIANT IT
                    INNER JOIN ITEM IM ON IT.ItemVariantID = IM.ItemVariantID
                    INNER JOIN PROCUREMENT PR ON IM.ItemID = PR.ItemID
                    INNER JOIN TAG TG ON IM.TagNumber = TG.TagNumber
                    INNER JOIN SECTION SC ON TG.SectionNumber = SC.SectionNumber 
                    WHERE IM.ProjectNumber = @projectNum",
                     con
                 );

                cmd.Parameters.AddWithValue("@projectNum", projectNum);


                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        //Procurement Item and VariantItem
                        int ItemID = (int)dr["ItemID"];
                        int ItemVariantID = (int)dr["ItemVariantID"];
                        string ItemNumber = (string)dr["ItemNumber"];
                        string LineNumber = (string)dr["ItemLineNumber"];
                        string Description = (string)dr["ItemDescription"];
                        float Quantity = (float)dr["Quantity"];
                        string UnitOfMeasure = (string)dr["UnitOfMeasure"];
                        //ParentKey So the item in software level knows who its parent is + to avoid multiple sql searches
                        string TagParentKey = (string)dr["TagNumber"];
                        int SectionParentKey = (int)dr["SectionNumber"];
                        int ProcurementID = (int)dr["ProcurementID"];
                        string PurchaseOrderNumber = (string)dr["PurchaseOrderNumber"];
                        string ProcurementStatus = (string)dr["ProcurementStatus"];

                        Item item = new Item(ItemID, ItemVariantID, ItemNumber, LineNumber, Description, Quantity, UnitOfMeasure, TagParentKey, SectionParentKey);
                        item.ItemProcurement = new Procurement(ProcurementID, PurchaseOrderNumber, ProcurementStatus);

                        result.Add(item);
                    }
                }
                items = result;
            }
        }
    }
}
