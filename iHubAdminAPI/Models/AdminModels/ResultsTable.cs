using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iHubAdminAPI.Models
{
    public class ResultsTablenew
    {
        public string CategoryName { get; set; }
        public string Image { get; set; }
        public Int16? ID { get; set; }
        public Int16? ParentID { get; set; }
        public Byte Status { get; set; }
        public string TableName { get; set; }


        public int TotalCount { get; set; }
        public string Description { get; set; }
        public Int16? Top_Category_Id { get; set; }
        public Int16? Priority { get; set; }
        // public bool Is_LeafNode { get; set; }
        public int Category_Id { get; set; }
        public string Category_Name { get; set; }
        public string Category_Description { get; set; }
        public int Category_Parent_Id { get; set; }
        public int Category_Status { get; set; }
        public int Category_Priority { get; set; }
        public string Category_AliasNames { get; set; }
        public int User_Id { get; set; }
        public int Responce_ID { get; set; }
        ////////Products Model data/////

        public int iHub_Product_ID { get; set; }
        public string Product_Name { get; set; }
        public Int16? Product_Status { get; set; }
        public Nullable<Boolean> Allow_Sales_In_Direct { get; set; }
        public Int16? Is_Repurchasable { get; set; }

        public decimal Selling_Price { get; set; }
        public decimal Offer_Price { get; set; }
        public Int16 Booking_Percentage { get; set; }
        public int HSN_Code { get; set; }
        public string Image_Code { get; set; }
        public string Product_Series { get; set; }
        public string Product_Description { get; set; }
        public string Html_Content { get; set; }
        public Int16? Is_Price_Approved { get; set; }
        public string Accessories_Json { get; set; }
        public Int16? Product_Delivery_Days { get; set; }
        public string Externallink { get; set; }
        public string Videolink { get; set; }
        public string Attribute_Json { get; set; }
        public decimal MRP { get; set; }
        public decimal Buyer_Percentage { get; set; }
        public Int16? Masters_ID { get; set; }
        public string Item_Name { get; set; }
        public string Item_Description { get; set; }
        public Int16? Item_Status { get; set; }
        public Int16? Item_CreatedBy { get; set; }
        public DateTime Item_CreatedOn { get; set; }
        public Int16? Item_UpdatedBy { get; set; }
        public DateTime Item_UpdatedOn { get; set; }
        public Int16? Parent_Master_ID { get; set; }
        public int HSN_Code_ID { get; set; }
        public decimal GST_Percentage { get; set; }
        /// <summary>
        /// /units table///
        /// </summary>

        public Int16? iHub_Unit_ID { get; set; }
        public Int16? Unit_Hierarchy_Level { get; set; }
        public Int16? Distribution_Channel_ID { get; set; }
        public Int16? WareHouse_ID { get; set; }
        public Int16? Store_ID { get; set; }
        public Int16? Master_Franchise_ID { get; set; }
        public string PhoneNumber { get; set; }
        public int Unit_Address_ID { get; set; }
        public Boolean Is_Unit_Active { get; set; }
        public string Unit_Name { get; set; }
        public string Email { get; set; }
        public string Unit_User_Name { get; set; }






      


        public Int16? Booking_Type { get; set; }
        public int Ordered_Product_Details_ID { get; set; }
        public int Order_Id { get; set; }
        public int Product_Id { get; set; }
        public decimal Ordered_Quantity { get; set; }
        public int Ordered_Product_Status_Id { get; set; }
        public decimal Selling_Price_On_Ordered_Date { get; set; }
        public decimal MRP_On_Ordered_Date { get; set; }
        public decimal Due_Amount { get; set; }
        public Int16? Order_From { get; set; }
        public string Ordered_Product_Location { get; set; }
        public decimal Total_Paid_Amount { get; set; }
        public string SKU { get; set; }
        public Int64? Available_new { get; set; }
        public Int32? Available { get; set; }
        public Int32? SOLD { get; set; }
        public Int32? InTransit { get; set; }
        public Int32? Total { get; set; }
        public Int32? ReservedForOrder { get; set; }
        public Int32? Damaged { get; set; }
        public int Qunatity { get; set; }
        public int Stock { get; set; }
        public string Brand { get; set; }
        public Int32? iHubUsage { get; set; }
        public Int32? StockAudit { get; set; }
        public Int32? InventoryShinkage { get; set; }
        public Int32? VendorOrderRequest { get; set; }

        // defult massages//
        public string Name { get; set; }
        public string Message { get; set; }
       // public DateTime Last_Updated_Date { get; set; }

        //Inventory checkbox
        public int Inventory_Product_ID { get; set; }
        public string Unique_Serial_Number { get; set; }
        public string Batch_Number { get; set; }
        public string Manufacture_Date { get; set; }
        public string Expiration_Date { get; set; }

        //Assigned stock for units
        public Int16 Inventory_Product_Status { get; set; }
        //public int Product_Id { get; set; }
        //public int Order_Id { get; set; }
        public Int16? Consignment_Status { get; set; }
        public int Unit_Id { get; set; }
        
        public int Previous_Unit_Id { get; set; }
        //public string Product_Name { get; set; }
        //public Nullable<DateTime> Order_Date { get; set; }
        public string Order_Date { get; set; }
        //public int Consignment_ID { get; set; }
        public Int16? Source_Unit_Id { get; set; }
        public int Quantity { get; set; }
        public string SourceName { get; set; }
        public string DestinationName { get; set; }
        public string Order_Type { get; set; }
        
        //Damaged
        public int Consignment_Id { get; set; }
        public string Consignment_Name { get; set; }
        public DateTime Created_Date { get; set; }
        public int ToalCount { get; set; }
        public string Source_Name { get; set; }
        public string Destination_Name { get; set; }


       // Manage Expences
       public int Expenses_ID { get; set; }
        public int Expensed_UnitID { get; set; }
        public decimal Expensed_Amount { get; set; }
        public string Expensed_On { get; set; }
        public string Other_Expenses { get; set; }
        public DateTime Expensed_Date { get; set; }
        public Int16 Expens_Status { get; set; }
        public string Expenses_Description { get; set; }
        public Nullable<DateTime> Last_Updated_Date { get; set; }
        //public string Unit_Name { get; set; }
        //public string Expenses_Description { get; set; }
        //public string Expenses_Description { get; set; }

        //Orders
        public Int64? Orders_Main_ID { get; set; }
        public int Order_Number { get; set; }
        public string Mobile_Number { get; set; }
        public int Buyer_Id { get; set; }
        //public DateTime Order_Date { get; set; }
        public decimal Total_Sale_Amount { get; set; }
        public Int16 Order_Status { get; set; }
        //public string Mobile_Number { get; set; }
        //public Int16? Unit_ID { get; set; }

        //public Int16? Booking_Type { get; set; }
        //public int Ordered_Product_Details_ID { get; set; }
        //public int Order_Id { get; set; }
        //public int Product_Id { get; set; }
        //public decimal Ordered_Quantity { get; set; }
        //public int Ordered_Product_Status_Id { get; set; }
        //public decimal Selling_Price_On_Ordered_Date { get; set; }
        //public decimal MRP_On_Ordered_Date { get; set; }
        //public decimal Due_Amount { get; set; }
        //public Int16? Order_From { get; set; }
        //public string Ordered_Product_Location { get; set; }
        //public decimal Total_Paid_Amount { get; set; }
        //public string SKU { get; set; }



    }



    public class ResultsTablewithCount
    {
        public List<ResultsTablenew> Resultset { get; set; }
        // public IQueryable<ResultsTable> rlist { get; set; }
        public int TotalCount { get; set; }
    }
    // Master Location Modal//

    public class Resultsinlocation
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public string Location_Name { get; set; }
        public string Location_Code { get; set; }
        public int Location_Hierarchy_Level { get; set; }
        public int Pincode { get; set; }



        public Int64 Orders_Main_ID { get; set; }
        public Nullable<int> Order_Number { get; set; }
        public string Mobile_Number { get; set; }
        public DateTime Order_Date { get; set; }
        public decimal Total_Sale_Amount { get; set; }
        public Int16? Unit_ID { get; set; }

        //public string Order_Date { get; set; }
    }
    public class ResultsinlocationResultset
    {
        public List<Resultsinlocation> Resultset { get; set; }
        // public IQueryable<ResultsTable> rlist { get; set; }
        public int TotalCount { get; set; }
    }
    public class ResultsAccount
    {
        //ACCOUNT  DETAILS
        public int CounterFiles_ID { get; set; }
        public string CounterFile_Description { get; set; }
        public string CounterFile_Type { get; set; }
        public decimal Total_Amount { get; set; }
        public Nullable<Int16> CounterFile_Status { get; set; }
        //public int Unit_Id { get; set; }
        public string Handovered_By { get; set; }
        public string Collected_By { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Last_Updated_Date { get; set; }
        public Nullable<int> Unit_ID { get; set; }
        public string Unit_Name { get; set; }
        public int Order_Id { get; set; }
        public DateTime Order_Date { get; set; }
    }
    public class ResultsinAccountResultset
    {
        public List<ResultsAccount> Resultset { get; set; }
        // public IQueryable<ResultsTable> rlist { get; set; }
        public int TotalCount { get; set; }
    }
    public class Units
    {
        public Int16 ID { get; set; }
        public string Unit_Name { get; set; }
        public string Email { get; set; }
        public string Unit_User_Name { get; set; }
        public string Unit_Additional_Data { get; set; }
        public string PhoneNumber { get; set; }
        public Nullable<Int16> Distribution_Channel_ID { get; set; }
        public Int16 Unit_Hierarchy_Level { get; set; }
        public Nullable<Int16> WareHouse_ID { get; set; }
        public Nullable<Int16> Store_ID { get; set; }
        public Nullable<Int16> Master_Franchise_ID { get; set; }
        public Nullable<int> Unit_Address_ID { get; set; }
        public bool Is_Unit_Active { get; set; }
        public Int16 ParentID { get; set; }
        public string Product_Name { get; set; }
        public string SKU { get; set; }
        public int Quantity { get; set; }
        //public string Unit_Name { get; set; }
        public int TotalCount { get; set; }

        //Orders

        public int Orders_Main_ID { get; set; }
        public Nullable<int> Order_Number { get; set; }
        public string Mobile_Number { get; set; }
        public DateTime Order_Date { get; set; }
        public decimal Total_Sale_Amount { get; set; }
        public Int16? Unit_ID { get; set; }


    }
    public class ResultsUnitswithCount
    {
        public List<Units> Resultset { get; set; }
        // public IQueryable<ResultsTable> rlist { get; set; }
        public int TotalCount { get; set; }
        public int AspNetID { get; set; }
    }
}