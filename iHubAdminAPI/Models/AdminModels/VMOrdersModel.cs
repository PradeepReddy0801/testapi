using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iHubAdminAPI.Models
{
    public class VMOrdersModel
    {
        public string MobileNumber { get; set; }
        public string UserSessionID { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentJson { get; set; }
        public string OrderJson { get; set; }
        public string CouponJson { get; set; }
    }
    public class VMPricingModel
    {
        public string MobileNumber { get; set; }
        public int CustomerID { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
        public string ProductName { get; set; }
    }
}