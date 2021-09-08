using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iHubAdminAPI.Models
{
   
    public class VMModelsForInventory
    {
        public string SKU { get; set; }
        public int Dc_Unit_ID { get; set; }
        public int Pagesize { get; set; }
        public int PageIndex { get; set; }
        public string ProductName { get; set; }
        public string FilterJson { get; set; }
        public string Unit_Type { get; set; }
        public int ParentID { get; set; }
        public int Unit_ID { get; set; }
        public int CategoryID { get; set; }
        public int TopCategoryID { get; set; }
        public int ProductID { get; set; }
        public int Vpo_id { get; set; }
        public int TotalQtyAssign { get; set; }
        public string BrandName { get; set; }
        public string OrderDateFrom { get; set; }
        public string OrderDateTo { get; set; }
        public string flagDCStore { get; set; }
        public string DeliveryChallNo { get; set; }
        public string InvoiceNumber { get; set; }
        public int Order_Number { get; set; }
        public int Top_Category_ID { get; set; }
        public int Status { get; set; }
    }
}