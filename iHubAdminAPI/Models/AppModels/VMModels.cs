using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iHubAdminAPI.Models.ProductAndPrice;
using System.Data.Entity;

namespace iHubAdminAPI.Models
{



    public class OfferCreateModel
    {
        public int ParentOfferID { get; set; }
        public string ChildOfferName { get; set; }
        public string ChildOfferDescription { get; set; }
        public int OfferApplicationDomainId { get; set; }
        public int OfferUserType { get; set; }
        public int OfferUsage_Limit { get; set; }
        public string Offer_StartDate { get; set; }
        public string Offer_EndDate { get; set; }
        public int OfferApplicable_Unit_Type_Id { get; set; }
        public int OfferApplicable_Parent_Unit_Id { get; set; }
        public string UnitIDs { get; set; }
        public int IsCorporate_Discount { get; set; }
        public int Offer_Sub_ChildID { get; set; }
        public decimal Offer_BuyQuantity { get; set; }
        public decimal Offer_GetQuantity { get; set; }
        public decimal OfferSale_Amount { get; set; }
        public decimal OfferDiscount_Amount { get; set; }
        public decimal OfferDiscount_Percentage { get; set; }
        public int OfferBasedOnId { get; set; }
        public int IsApplied { get; set; }
        public string ProductIDs { get; set; }
        public string CategoryIDs { get; set; }
        public string BrandIDs { get; set; }
        public int UserID { get; set; }
        public int childOfferID { get; set; }
        public int UpdateFlag { get; set; }
        public int Status { get; set; }
        public int PriorityID { get; set; }
        public string DomainID { get; set; }

    }
    public class cashbackdetails
    {
        public string MobileNumber { get; set; }
    }
    public class WareStoreOrderManagement
    {
        public int OrderNumber { get; set; }
        public string OrderDateFrom { get; set; }
        public string OrderDateTo { get; set; }
        public string MobileNumber { get; set; }
        public int UnitID { get; set; }
        public int Status { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
    public class EMIRequestList
    {
        public string MobileNumber { get; set; }
        public string Name { get; set; }
        public decimal EMI_Min_Limit { get; set; }
        public decimal EMI_Max_Limit { get; set; }
        public int No_Of_EMIs { get; set; }
        public int Status { get; set; }
        public string CounterDescription { get; set; }
        public string CreatedDate { get; set; }
        public int ApprovedTimeLimit { get; set; }
        public int ApprovedCode { get; set; }
        public int BuyerEMIId { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int EMIConfigID { get; set; }
        public int buyerid { get; set; }
        public int Status2 { get; set; }
        public int UnitId { get; set; }

    }
    public class vmmodelsforDCoutflow
    {
        public string SKU { get; set; }
        public string ProductName { get; set; }
        public int OrderID { get; set; }
        public string OrderDate { get; set; }
        public int Dest_UnitID { get; set; }
        public int Source_UnitID { get; set; }
        public int Status { get; set; }
        public int Pagesize { get; set; }
        public int PageIndex { get; set; }
        public int Unit_ID { get; set; }
        public int Heirarachy_Level { get; set; }
    }
    public class VMModelsForWHProducts
    {
        public int Unit_ID { get; set; }
        public string ProductName { get; set; }
        public string SKU { get; set; }
        public int ID { get; set; }
        public string FilterJson { get; set; }
        public string CategoryID { get; set; }
        public int Pagesize { get; set; }
        public int PageIndex { get; set; }

    }
    public class VMModelForCategories
    {
        public Int32 ID { get; set; }
        public string CategoryName { get; set; }
        public Int32 ParentID { get; set; }
        public string Image { get; set; }
        public Int32? Status { get; set; }
        public Int16 CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Int16 UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string Description { get; set; }
        public decimal? BookingPercentage { get; set; }
        public string AliasNames { get; set; }
        public Int32? Priority { get; set; }
        public Boolean? Is_LeafNode { get; set; }
        public decimal? BuyerPercentage { get; set; }
        public Int16? Top_Category_Id { get; set; }
        public Int16? Is_Batch_Number { get; set; }
        public Int16? Is_Manufacture_Date { get; set; }
        public Int16? Is_Expiry_Date { get; set; }
        public Int16? Is_IMEI_Number { get; set; }
    }
    //==================================================For category Model==========================================
    public class VMModelsForCategory
    {
        public int Category_Id { get; set; }
        public string Category_Name { get; set; }
        public string Category_Description { get; set; }
        public int Category_Parent_Id { get; set; }
        public int Category_Status { get; set; }
        public int Category_Priority { get; set; }
        public string Category_AliasNames { get; set; }
        public int User_Id { get; set; }
        public string ProductName { get; set; }
        public string FilterJson { get; set; }
        public string SKU { get; set; }
        public int Pagesize { get; set; }
        public int PageIndex { get; set; }

        public string JsonData { get; set; }
        public int? ID { get; set; }
        public string TableName { get; set; }
        public int TargetCategoryID { get; set; }
        public string ShuffleType { get; set; }
        public string ProductIds { get; set; }
        public Int16? Is_Batch_Number { get; set; }
        public Int16? Is_Manufacture_Date { get; set; }
        public Int16? Is_Expiry_Date { get; set; }
        public Int16? Is_IMEI_Number { get; set; }
        public int OrderNumber { get; set; }
        public string MobileNumber { get; set; }
        public int Tenure { get; set; }
        public string OrderDateFrom { get; set; }
        public string OrderDateTo { get; set; }
        public string UnitName { get; set; }
        public string sCategoryType { get; set; }
        public Int16? iParentID { get; set; }
        public int Status { get; set; }
        public int POID { get; set; }

    }
    public class VMWalletdetails
    {
        public int WalletAmount { get; set; }
        public string TransactionType { get; set; }
        public int BuyerID { get; set; }
        public int OrderNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Remarks { get; set; }
        public string Description { get; set; }
        public string TransactionNumber { get; set; }
        public decimal Amount { get; set; }
        public decimal RaffleAmount { get; set; }
    }

    public class SizeChart
    {
        public string SizeChartType { get; set; }
        public string BrandName { get; set; }
        public string ProductsList { get; set; }
        public int Categoryid { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Sizechart { get; set; }
        public string ProductName { get; set; }
        public string Category_Name { get; set; }
        public string ProductSku { get; set; }
        public string Flag { get; set; }
        public int ProductId { get; set; }

    }
    public class VMModelsForHelpManual
    {
        public string Role { get; set; }
        public string DocName { get; set; }
        public string Format { get; set; }
        public string VedioUrl { get; set; }
        public string FolderName { get; set; }
        public string LastUpdateDate { get; set; }
        public string Description { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

    }
    public class PaymentModel
    {
        public int ProductID { get; set; }
        public int Buyer_Id { get; set; }
        public string Payment_Mode_Type { get; set; }
        public string Use_CashBack_Amount { get; set; }
        public string Use_Wallet_Amount { get; set; }
        public int Coupon_Amount { get; set; }
        public int Buyer_Address_Id { get; set; }
        public int Order_From_ID { get; set; }
        public int Unit_Id { get; set; }
        public int User_ID { get; set; }
        public string pamentjson { get; set; }
        public int StoreOTP { get; set; }
        public int Notification_ID { get; set; }
        public string CouponJson { get; set; }
        public string TXID { get; set; }
        public string Amount { get; set; }
        public string Buyer_Mobile_Number { get; set; }
        public int CCOrderNumber { get; set; }
        public string IsWallet { get; set; }
        public List<Dictionary<string, string>> Products { get; set; }
        public List<Dictionary<string, string>> OrderDetails { get; set; }
        public List<object> productdetails { get; set; }
        public VMPrdctDetails prdctdetails { get; set; }
        public List<Dictionary<string, string>> BuyerAddress { get; set; }
        public Address address { get; set; }
        public string BuyerName { get; set; }
        public string Use_Schemes_Amount { get; set; }
        public string EmailID { get; set; }
        public float CoupounAmount { get; set; }
        public float WalletAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public float TotalPrice { get; set; }
        public int OrderID { get; set; }
        public bool IsEmi { get; set; }
        public int CampWallet { get; set; }
        public DateTime PaymentReceivedDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string ReferenceID { get; set; }
        public string Description { get; set; }
        public decimal BalanceAmount { get; set; }
        public decimal OrderDueAmount { get; set; }
        // public int @EmployeeID { get; set; }
        public string From_Date { get; set; }
        public int POID { get; set; }
    }
    public class VMModelsForWareHouseIntrastore
    {
        public int Unit_ID { get; set; }
        public string SKU { get; set; }
        public string ProductName { get; set; }
        public int OrderID { get; set; }
        public string OrderDate { get; set; }
        public int Source_UnitID { get; set; }
        public int Dest_UnitID { get; set; }
        public string Status2 { get; set; }
        public int PageIndex { get; set; }
        public int Pagesize { get; set; }


    }
    public class VMModelForProductCategoryUnitBinLocation
    {
        public int ID { get; set; }
        public string IDs { get; set; } // comma separated product ids.
        public string CategoryName { get; set; }
        public int CategoryID { get; set; }
        public string UnitName { get; set; }
        public int UnitID { get; set; }
        public string Location { get; set; }
        public string ApplySubCatFlag { get; set; }
        public string TopCategoryName { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public string SKU { get; set; }
        public string CreatedDateFrom { get; set; }
        public string CreatedDateTo { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int ProductID { get; set; }
        public string Barcode { get; set; }
    }
}