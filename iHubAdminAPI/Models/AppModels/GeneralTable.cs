using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using iHubAdminAPI.Models.ProductAndPrice;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using iHubAdminAPI.Models.MasterDataAndUnits;
namespace iHubAdminAPI.Models
{

    public class ModelDBContext : DbContext
    {

        public ModelDBContext() : base("name=DefaultConnection") { }
        public DbSet<iH_Categories> iH_Categories { get; set; }
        // public DbSet<iH_Menus> iH_Menus{ get; set; }
        public DbSet<iHub_MegaDeals> iHub_MegaDeals { get; set; }
        public DbSet<iHub_Products> iHub_Products { get; set; }
        public DbSet<Cls_Vendor_Categories> Vendor_Categories { get; set; }
        public DbSet<Cls_Vendor_Categories_Products> Vendor_Categories_Products { get; set; }
        public DbSet<Cls_iHub_Vendors> iHub_Vendors { get; set; }
        //public DbSet<iH_Master_Locations> iH_Master_Locations { get; set; }
        //public DbSet<AppVersions> Appversions { get; set; }
        //public DbSet<iH_PaymentProcessForm> iH_PaymentProcessForm { get; set; }
        //public DbSet<iH_Master_Pincodes> iH_Master_Pincodes { get; set; }
        //public DbSet<iHub_Logistics> iHub_Logistics { get; set; }
        //public DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        //public DbSet<iHub_Units_DC_WH_ST> iHub_Units_DC_WH_ST { get; set; }
        //public DbSet<iHub_EMI_Orders> iHub_EMI_Orders { get; set; }
        //public DbSet<AspNetRoles> AspNetRoles { get; set; }     
        //public DbSet<Coupons> Coupons { get; set; }
        //public DbSet<Coupon_User_Validations> Coupon_User_Validations { get; set; }
        //public DbSet<RffleCoupon> RffleCoupon { get; set; }
        //public DbSet<Cls_Buyer_Registration> Buyer_Registration { get; set; }
        //public DbSet<Cls_iHub_Vendors> iHub_Vendors { get; set; }
        //public DbSet<Cls_iH_Masters_Data> iH_Masters_Data { get; set; }
        //public DbSet<iH_Master_Locations> iH_Master_Locations { get; set; }
        //public DbSet<Cls_Buyer_Categories> Buyer_Categories { get; set; }
        //public DbSet<Cls_Vendor_Categories> Vendor_Categories { get; set; }
        //public DbSet<Cls_iH_User_Address_Mapping> iH_User_Address_Mapping { get; set; }
        //public DbSet<Cls_Bundle_Products> Bundle_Products_Config { get; set; }
        //public DbSet<iH_Category_Product_Mapping> iH_Category_Product_Mapping { get; set; }
        //public DbSet<Cls_Pre_Reg_Buyers> Pre_Registered_Buyers { get; set; }
        //public DbSet<clsDuplicatePRBs> DuplicatePRBs { get; set; }
        //public DbSet<Cls_iH_Address_Details> iH_Address_Details { get; set; }
        //public DbSet<Cls_iHub_Wallet_Offer_Details> iHub_Wallet_Offer_Details { get; set; }
        //public DbSet<ClsTasks> Tasks { get; set; }
        //public DbSet<clsRoles_Tasks> Roles_Tasks { get; set; }
        //public DbSet<clsTaskActions> TaskActions { get; set; }
        //public DbSet<iD_Orders_Main> iD_Orders_Main { get; set; }
        //public DbSet<clsIhubEmployeeDetails> iHub_Employee_Details { get; set; }
        //public DbSet<ClsUnit_HierarchyLevel_Master> Unit_HierarchyLevel_Master { get; set; }
    }



    public class iHubUnits
    {
       
        public int Sub_Unit_ID { get; set; }
        public int Unit_Hierarchy_Level_ID { get; set; }
        public string Unit_Type { get; set; }
        public string Unit_Name { get; set; }
        public string Email { get; set; }
        public string Phone_Number { get; set; }
        public string ContactName { get; set; }
        public string AddressLine_One { get; set; }
        public string AddressLine_Two { get; set; }
        public int VillageLocation_ID { get; set; }
        public string Suggested_UserName { get; set; }
        public string Unit_Additional_Data { get; set; }
        public string Password { get; set; }
        public string Order_By { get; set; }
        public int Page_Index { get; set; }
        public int Page_Size { get; set; }
        public string BusinessDetails { get; set; }
        public string LocationDetails { get; set; }
        public string DocumentDetalis { get; set; }
        public string Gender { get; set; }
        public DateTime Date_Of_Birth { get; set; }
        public string Qualification { get; set; }
        public int Address_Proof_ID { get; set; }
        public string Address_Proof_Id_Number { get; set; }
        public int isWHcumStore { get; set; }
        public int RoleId { get; set; }
        public string Address_Proof_ID_employee { get; set; }
        public int user_ID { get; set; }
        public string NewPassword { get; set; }
        public string Domains { get; set; }
        public string Role_Subscription_IDs { get; set; }
        public string removeMasterIDs { get; set; }
    }

    [Table("iHub_Vendors")]
    public class Cls_iHub_Vendors
    {
        [Key]
        public int Vendor_ID { get; set; }
        public string Vendor_Name { get; set; }

    }
    [Table("iHub_Units_DC_WH_ST")]
    public class iHub_Units_DC_WH_ST
    {
        [Key]
        public Int16 iHub_Unit_ID { get; set; }
        public string Unit_Name { get; set; }
        public Int16 Unit_Hierarchy_Level { get; set; }
        public Int16 Distribution_Channel_ID { get; set; }
        public Int16 WareHouse_ID { get; set; }

    }

  
    public class EMISlab
    {
        public int EMI_Master_Data_ID { get; set; }
        public Decimal EMI_Slab_Lower_Limit { get; set; }
        public Decimal EMI_Slab_Upper_Limit { get; set; }
        public Byte No_Of_EMIs_Allowed { get; set; }
        public Int16 Processing_Fee { get; set; }
        public Decimal Interest_Rate { get; set; }
        public DateTime Created_DateTime { get; set; }
        public DateTime Last_Updated_DateTime { get; set; }
        public List<EMISlab> EmiSlabConfig { get; set; }

    }
    public class StoreAgent
    {
        public int AspNetId { get; set; }
        public int UnitId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone_Number { get; set; }
        public string AddressLine_One { get; set; }
        public int VillageLocation_ID { get; set; }
        public string Suggested_UserName { get; set; }
        public string Gender { get; set; }
        public string Date_Of_Birth { get; set; }
        public int Address_Proof_ID { get; set; }
        public string Address_Proof_Id_Number { get; set; }
        public int UserType { get; set; }
        public string Password { get; set; }
        public int DCUnitID { get; set; }
        public int ClusterID { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Page_Size { get; set; }
        public int Page_Index { get; set; }
        public int Id { get; set; }
        public int AddressId { get; set; }
        public string DOB { get; set; }
        public string Address { get; set; }
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }
        public int UserStatus { get; set; }
        public string UpdateData { get; set; }
        public string PayMode { get; set; }
        public int Years { get; set; }
        public int Months { get; set; }
        public string PaymentMode { get; set; }
    }
    public class Cashback
    {
        public string OfferType { get; set; }
        public string OfferName { get; set; }
        public string OfferDescription { get; set; }
        public int CashbackValue { get; set; }
        public Decimal MaxValue { get; set; }
        public int CreditCashback { get; set; }
        public string Startdate { get; set; }
        public int ExpiresInMonths { get; set; }
        public string Domains { get; set; }
        public int Status { get; set; }
        public string NameSearch { get; set; }
        public int MonthsSearch { get; set; }
        public int StatusSearch { get; set; }
        public string DomainSearch { get; set; }
        public int pagesize { get; set; }
        public int pageindex { get; set; }
        public int ID { get; set; }
        public string Refundnewamount { get; set; }
        public int productid { get; set; }
        public int Flag { get; set; }
        public string Notes { get; set; }
    }


}