using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace iHubAdminAPI.Models
{

    public class VMModelsForOrderManagement
    {

        public DateTime? OrderDateFrom { get; set; }
        public DateTime? OrderDateTo { get; set; }
        public string MobileNumber { get; set; }
        public int UnitID { get; set; }
        public int Status { get; set; }
        public int OrderNumber { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string OrderType { get; set; }
        public string StoreName { get; set; }
        public string ProductName { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public string CategoryName { get; set; }
        public string CustomerID { get; set; }
        public string Message { get; set; }
        public int OrderID { get; set; }
        public int ParentID { get; set; }
        public int StatusTwo { get; set; }
        public int CategoryID { get; set; }
        public int OrderFrom { get; set; }
        // EMI_Close_Manually variables
        public decimal TotalPaidAmount { get; set; }
        public string Notes { get; set; }
        public int OTP { get; set; }
        public int Notification_ID { get; set; }
        [NotMapped]
        public int Customer_Requested_ID { get; set; }
        [NotMapped]
        public int iCatNotMapped { get; set; }
        [NotMapped]
        public string sUserIDs { get; set; }
    }
}