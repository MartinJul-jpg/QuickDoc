using System;
using System.Collections.Generic;
using System.Text;

namespace QuickDoc.Model
{
    public class Procurement
    {
        public int ProcurementID;
        public string PurchaseOrderNumber;
        public string ProcurementStatus;

        public Procurement(int procurementID, string purchaseOrderNumber, string procurementStatus)
        {
            ProcurementID = procurementID;
            this.PurchaseOrderNumber = purchaseOrderNumber;
            ProcurementStatus = procurementStatus;
        }
    }
}
