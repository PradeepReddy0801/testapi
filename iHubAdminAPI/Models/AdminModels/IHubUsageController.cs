using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iHubAdminAPI.Models
{
    public class IHubUsageController 
    {
        public int productid { get; set; }
        public string Schemetype { get; set; }
        public int SchemePurchaseValue { get; set; }
        public int SchemeOfferValue { get; set; }
        public int MonthlyClaimAmount { get; set; }
        public int NoOfInstallments { get; set; }
        public int OrderNumber { get; set; }
        public string MobileNumber { get; set; }
        public string SKU { get; set; }
        public string BuyerName { get; set; }
        public int Page_Index { get; set; }
        public int Page_Size { get; set; }
        public int UnitID { get; set; }
        public string ProductName { get; set; }
        public string product_id { get; set; }
        public int OrderID { get; set; }
        public string orderdatefrom { get; set; }
        public string orderdateto { get; set; }
        public string Unit_Type { get; set; }
        public int pagesize { get; set; }
        public int pageindex { get; set; }

    }
}
