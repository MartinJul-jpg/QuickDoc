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
        

        public void ReadFromDatabase(string projectNum)
        {
            List<Item> result = new List<Item>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                // 3 joins 
                SqlCommand cmd = new SqlCommand(
                     @"SELECT IT.ItemVariantID, IT.ItemNumber, IT.ItemDescription, 
                        IM.ItemID, IM.ProjectNumber, IM.ItemLineNumber, IM.UnitOfMeasure, IM.Quantity, IM.TagNumber,
                        PR.ProcurementID, PR.ProcurementStatus, PR.PurchaseOrderNumber,
                        IMD.ITitle, IMD.IDocDescription, IMD.IFile
                    FROM ITEMVARIANT IT
                    LEFT JOIN ITEM IM ON IT.ItemVariantID = IM.ItemVariantID
                    LEFT JOIN PROCUREMENT PR ON IM.ItemID = PR.ItemID
                    LEFT JOIN TAG TG ON IM.TagNumber = TG.TagNumber
                    LEFT JOIN ITEMVARIANTDOCUMENT IMD ON IT.ItemVariantID = IMD.ItemVariantID 
                    WHERE IM.ProjectNumber = @projectNum",
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

                        // Procurement
                        int ProcurementID = dr["ProcurementID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ProcurementID"]);
                        string purchaseOrderNumber = dr["PurchaseOrderNumber"] == DBNull.Value ? "" : Convert.ToString(dr["PurchaseOrderNumber"]);
                        string procurementStatus = dr["ProcurementStatus"] == DBNull.Value ? "" : Convert.ToString(dr["ProcurementStatus"]);
                        // FILE DATA
                        string title = dr["ITitle"] == DBNull.Value ? "" : Convert.ToString(dr["ITitle"]);
                        string description = dr["IDocDescription"] == DBNull.Value ? "" : Convert.ToString(dr["IDocDescription"]);
                        string filepath = dr["IFile"] == DBNull.Value ? "" : Convert.ToString(dr["IFile"]);

                        Item item = new Item(ItemID, ItemVariantID, ItemNumber, LineNumber, Description, Quantity, UnitOfMeasure, TagParentKey);
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
