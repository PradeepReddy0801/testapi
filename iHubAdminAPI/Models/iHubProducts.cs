using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iHubAdminAPI.Models
{
    public class iHubProducts
    {
        public int iHub_Product_ID { get; set; }
        public string Product_Name { get; set; }
        // public int Product_Status { get; set; }
        public Int16? Product_Status { get; set; }
        public Nullable<Boolean> Allow_Sales_In_Direct { get; set; }
        //public int Is_Repurchasable { get; set; }
        public Int16? Is_Repurchasable { get; set; }
        public decimal MRP { get; set; }
        public decimal Selling_Price { get; set; }
        public decimal Offer_Price { get; set; }
        public Int16 Booking_Percentage { get; set; }
        public int HSN_Code { get; set; }
        public string Image_Code { get; set; }
        public string Product_Series { get; set; }
        public string Product_Description { get; set; }
        public string Html_Content { get; set; }
        public Nullable<Int16> Is_Price_Approved { get; set; }
        public Nullable<int> Group_ID { get; set; }
        public string Accessories_Json { get; set; }
        public string Search_String { get; set; }
        public Nullable<Int16> Product_Delivery_Days { get; set; }
        public string Externallink { get; set; }
        public string Videolink { get; set; }
        public DateTime Created_Date_Time { get; set; }
        public int Reference_Number { get; set; }
        public Nullable<int> User_Id { get; set; }
        public string IP_Address { get; set; }
        public string Product_Code { get; set; }
        public DateTime Last_Updated_Time { get; set; }
    }
    public class ProductsTablewithCount
    {
        public List<iHubProducts> Resultset { get; set; }
        // public IQueryable<ResultsTable> rlist { get; set; }
        public int TotalCount { get; set; }
    }
}