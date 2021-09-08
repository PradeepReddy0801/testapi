using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iHubAdminAPI.Models.ProductAndPrice
{



    [Table("iHub_Products")]
    public class iHub_Products
    {
        [Key]
        public int iHub_Product_ID { get; set; }
        public string Product_Name { get; set; }
        public Byte Product_Status { get; set; }
        public Boolean Allow_Sales_In_Direct { get; set; }
        public int Is_Repurchasable { get; set; }
        public decimal MRP { get; set; }
        public decimal Selling_Price { get; set; }
        public decimal Offer_Price { get; set; }
        public int Booking_Percentage { get; set; }
        public int HSN_Code { get; set; }
        public string Image_Code { get; set; }
        public string Product_Series { get; set; }
        public string Product_Description { get; set; }
        public string Html_Content { get; set; }
        public int Is_Price_Approved { get; set; }
        public int Group_ID { get; set; }
        public string Accessories_Json { get; set; }
        public Byte? Product_Delivery_Days { get; set; }
        public string Externallink { get; set; }
        public string Videolink { get; set; }
        public DateTime Created_Date_Time { get; set; }
        public int Reference_Number { get; set; }
        public int User_Id { get; set; }
        public string IP_Address { get; set; }
        public string Product_Code { get; set; }
        public DateTime Last_Updated_Time { get; set; }
    }

    public class VMiHub_Products
    {
        
        public int iHub_Product_ID { get; set; }
        public string Product_Name { get; set; }
        public Byte Product_Status { get; set; }
        public Boolean Allow_Sales_In_Direct { get; set; }
        public int Is_Repurchasable { get; set; }
        public decimal MRP { get; set; }
        public decimal Selling_Price { get; set; }
        public decimal Offer_Price { get; set; }
        public int Booking_Percentage { get; set; }
        public int HSN_Code { get; set; }
        public string Image_Code { get; set; }
        public string Product_Series { get; set; }
        public string Product_Description { get; set; }
        public string Html_Content { get; set; }
        public int Is_Price_Approved { get; set; }
        public int Group_ID { get; set; }
        public string Accessories_Json { get; set; }
        public Byte? Product_Delivery_Days { get; set; }
        public string Externallink { get; set; }
        public string Videolink { get; set; }
        public DateTime Created_Date_Time { get; set; }
        public int Reference_Number { get; set; }
        public int User_Id { get; set; }
        public string IP_Address { get; set; }
        public string Product_Code { get; set; }
        public DateTime Last_Updated_Time { get; set; }
        public int Package_ID { get; set; }
        public int Inv_Count { get; set; }
        public int Product_ID { get; set; }
        public int Quantity { get; set; }
        public int UserBasketPakage_ID { get; set; }
    }

    public class VMModelsForProduct
    {
        public int PoNo { get; set; }
        public string InboundDate { get; set; }
        public int CategoryID { get; set; }
        public string Category_Name { get; set; }
        public string Category_Description { get; set; }
        public int Category_Parent_Id { get; set; }
        public int Category_Status { get; set; }
        public int Category_Priority { get; set; }
        public string Category_AliasNames { get; set; }
        public int userid { get; set; }
        public string ProductName { get; set; }
        public string FilterJson { get; set; }
        public string JsonData { get; set; }
        public int Pagesize { get; set; }
        public int PageIndex { get; set; }
        public int ID { get; set; }
        public string BrandName { get; set; }
        public string PercentageType { get; set; }
        public int Pecentage { get; set; }
        public string productids { get; set; }
        public int Status { get; set; }
        public int HSN_Code { get; set; }
        public string Status2 { get; set; }
        public string SKU { get; set; }
        public string Address_Line_1 { get; set; }
        public string Address_Line_2 { get; set; }
        public int Village_Area_ID { get; set; }
        public string MobileNumber { get; set; }
        public string FullName { get; set; }
        public int ProductId { get; set; }
        public int AccessoriesId { get; set; }
        public int user_Id { get; set; }
        public decimal SellingPrice { get; set; }
        public int Percentage { get; set; }
        public int VendorID { get; set; }
        public string sSellingPrice { get; set; }
        public string POID { get; set; }
        public string OrderID { get; set; }
        public string OrderType { get; set; }
        public string UnitName { get; set; }


    }

}