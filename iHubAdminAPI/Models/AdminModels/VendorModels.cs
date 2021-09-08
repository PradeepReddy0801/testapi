using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iHubAdminAPI.Models
{
    public class VendorInvoiceCreation
    {
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string IsAdmin { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string StoreName { get; set; }
        public string FilterJson { get; set; }
        public string ParentCategoryName { get; set; }
        public int VendorID { get; set; }
        public string BrandName { get; set; }
        public string Message { get; set; }
        public int CategoryID { get; set; }
        public int ID { get; set; }
        public string Stock { get; set; }
        public string Status2 { get; set; }
        public string transactionnumber { get; set; }
        public string HtmlContent { get; set; }
        public int Status { get; set; }
        public int VendorInvoiceID { get; set; }
        public string From_Date { get; set; }
        public string VPOStatus { get; set; }
        public Nullable<DateTime> DateFrom { get; set; }
        public Nullable<DateTime> DateTo { get; set; }
        public string PickupDate { get; set; }

    }
}