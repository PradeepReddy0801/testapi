using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iHubAdminAPI.Models.AutoStockModelandViewModels
{
    public class AutoStockDataMdel
    {
        public int ID { get; set; }
        public int UnitID { get; set; }
        public int CategoryID { get; set; }
        public int ProductID { get; set; }
        public int Quantitiy { get; set; }
        public string CategoryName { get; set; }
        public int Pagesize { get; set; }
        public int PageIndex { get; set; }
        public int LocationID { get; set; }
        public string FilterJson { get; set; }
        public string ProductName { get; set; }
        public string SKU { get; set; }
        public int Unit_Class_Id { get; set; }
        public int iHub_Unit_ID { get; set; }
        public int Main_Category_ID { get; set; }
        public int Leaf_Category_Id { get; set; }
        public int SourceUnitID { get; set; }
        public int DestUnitID { get; set; }
        public int ReferenceNumber { get; set; }
        public int InventoryID { get; set; }
    }
}