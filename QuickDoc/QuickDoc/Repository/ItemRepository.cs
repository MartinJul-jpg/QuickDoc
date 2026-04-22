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
            Item item;
            bool exists = items.Any(x => x.ItemNumber == itemNumber);
            if (exists == false)
            {
                return new Item(0, 0,null,  null, null, null, null, null, null);
            }
            return items.Where(x => x.ItemNumber == itemNumber).First();
        }
        // To find which tags own the item
        public List<Item> GetTagsChildren(string TagNumber)
        {
            return items.Where(x => x.TagParentKey == TagNumber).ToList();
        }
        

        public void ReadFromDatabase(string projectNum)
        {
            List<Item> result = new List<Item>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                // 3 joins 
                SqlCommand cmd = new SqlCommand(
                     @"EXEC sp_ReadItemFromDatabase @projectNum",
                     con
                 );

                cmd.Parameters.AddWithValue("@projectNum", projectNum);


                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        //CANNOT CAST CAUSE IT CANNOT HANDLE NULL VALUES FROM DATABASE!!!
                        //Procurement Item and VariantItem
                        int ItemID = dr["ItemID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ItemID"]);
                        int ItemVariantID = dr["ItemVariantID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ItemVariantID"]);
                        string ItemNumber = dr["ItemNumber"] == DBNull.Value ? "" : Convert.ToString(dr["ItemNumber"]);
                        string LineNumber = dr["ItemLineNumber"] == DBNull.Value ? "" : Convert.ToString(dr["ItemLineNumber"]);
                        string Description = dr["ItemDescription"] == DBNull.Value ? "" : Convert.ToString(dr["ItemDescription"]);
                        string Quantity = dr["Quantity"] == DBNull.Value ? "" : Convert.ToString(dr["Quantity"]);
                        string UnitOfMeasure = dr["UnitOfMeasure"] == DBNull.Value ? "" : Convert.ToString(dr["UnitOfMeasure"]);
                        string TagParentKey = dr["TagNumber"] == DBNull.Value ? "" : Convert.ToString(dr["TagNumber"]);
                        string SerialNumber = dr["SerialNumber"] == DBNull.Value ? "" : Convert.ToString(dr["SerialNumber"]);

                        // Procurement
                        int ProcurementID = dr["ProcurementID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ProcurementID"]);
                        string purchaseOrderNumber = dr["PurchaseOrderNumber"] == DBNull.Value ? "" : Convert.ToString(dr["PurchaseOrderNumber"]);
                        string procurementStatus = dr["ProcurementStatus"] == DBNull.Value ? "" : Convert.ToString(dr["ProcurementStatus"]);
                        // FILE DATA
                        string title = dr["ITitle"] == DBNull.Value ? "" : Convert.ToString(dr["ITitle"]);
                        string description = dr["IDocDescription"] == DBNull.Value ? "" : Convert.ToString(dr["IDocDescription"]);
                        string filepath = dr["IFile"] == DBNull.Value ? "" : Convert.ToString(dr["IFile"]);

                        Item item = new Item(ItemID, ItemVariantID, ItemNumber, LineNumber, Description, Quantity, UnitOfMeasure, null, TagParentKey); //serialnumber needs to be handled
                        item.ItemProcurement = new Procurement(ProcurementID, purchaseOrderNumber, procurementStatus);
                        
                        if ( !(title == "" && description == "" && filepath == "") )
                        {
                            item.Documents.Add(new Document(title, description, filepath));
                        }

                        result.Add(item);
                    }
                }
                items = result;
            }
        }
    }
}
