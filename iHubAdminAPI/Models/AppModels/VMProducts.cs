using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace iHubAdminAPI.Models
{
    public class VMPagingResultsPost
    {
        internal object brandname;
        public string Track_Vachile_Id { get; set; }
        public string TrackingVechlNo { get; set; }
        public int Ordered_Product_Details_ID { get; set; }
        public string DamageFlag { get; set; }
        public int Consignmentids { get; set; }
        public string FilterJson { get; set; }
        public string filterjson { get; set; }
        public int CategoryID { get; set; }
        public int Dc_Unit_ID { get; set; }
        public int OrderFrom { get; set; }
        public string CategoryName { get; set; }
        public string ParentCategoryName { get; set; }
        public string ProductName { get; set; }
        public string TopCategory_ID { get; set; }
        public string TopCategoryName { get; set; }
        public int Pagesize { get; set; }
        public int PageIndex { get; set; }
        public int VendorID { get; set; }
        public string PurchaseOrderFrom { get; set; }
        public string PurchaseOrderTo { get; set; }
        public int ID { get; set; }
        public int StoreId { get; set; }
        public string PaymentMode { get; set; }
        public string OrderDate { get; set; }
        public string Catids { get; set; }
        public string Productids { get; set; }
        public int Productid { get; set; }
        public string PaymentJson { get; set; }
        public string paidamount { get; set; }
        public string TrackDetails { get; set; }
        public int OrderID { get; set; }
        public int productid { get; set; }
        public string imagecode { get; set; }
        public string OrderID2 { get; set; }
        public DateTime? orderdatefrom { get; set; }
        public DateTime? orderdateto { get; set; }
        public string MobileNumber { get; set; }
        public string WareHouseID { get; set; }
        public int Status { get; set; }
        public string CategoryID2 { get; set; }
        public string JsonData { get; set; }
        public string Status2 { get; set; }
        public string ProductLocation { get; set; }
        public int ProductLocationId { get; set; }
        public string StoreId2 { get; set; }
        //public string filterjson { get; set; }
        public int stock { get; set; }
        public string orderfrom { get; set; }
        public string orderto { get; set; }
        public int pagesize { get; set; }
        public int pageindex { get; set; }
        public string RelevanceType { get; set; }
        public int IsRefund { get; set; }
        public int aid { get; set; }
        public string StoreName { get; set; }
        public string StoreSales { get; set; }
        public int ClusterFranchiseID { get; set; }
        public string ClusterFranchiseName { get; set; }
        public string ClusterFranchiseSales { get; set; }
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public string LocationSales { get; set; }
        public int Unit_ID { get; set; }
        public string Unit_Type { get; set; }
        public int Count { get; set; }
        public int ParentID { get; set; }
        public int ProductID { get; set; }
        public string SKU { get; set; }
        public string Price { get; set; }
        public string Stock { get; set; }
        public string ProductSeries { get; set; }
        public string Type { get; set; }
        public int iStoreID { get; set; }
        public string BrandName { get; set; }
        public string Allow_Sales_In_Direct { get; set; }
        public string moduletype { get; set; }
        public List<Dictionary<string, string>> OrderDetails { get; set; }
        public List<Dictionary<string, string>> ProductDetails { get; set; }
        public VMPrdctDetails prdctdetails { get; set; }
        public List<object> productdetails { get; set; }
        public string payment_mode { get; set; }
        public string mobile_number { get; set; }
        public string transactionnumber { get; set; }
        public string Name { get; set; }
        //Properties For HomePage
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int DisplayOrder { get; set; }
        public string Imagecode { get; set; }
        public string SectionId { get; set; }
        public int category_id { get; set; }
        public int Quantitiy { get; set; }
        public string tablename { get; set; }
        //Properties for inflowoutflow
        public int Inflow_Or_Outflow { get; set; }
        public int Heirarachy_Level { get; set; }
        public int Dest_UnitID { get; set; }
        public int Source_UnitID { get; set; }
        //Properties for expenses
        public string expensedate { get; set; }
        public string expensestype { get; set; }
        public string Product_Mrp { get; set; }
        public string HtmlContent { get; set; }
        public int Hierarchy_Level { get; set; }
        public int Otp { get; set; }
        public int Notification_ID { get; set; }
        public float Amount { get; set; }
        public float Bonus_Amount { get; set; }
        public float OfferValue { get; set; }
        public float MinValue { get; set; }
        public float MaxValue { get; set; }
        //For mailer
        public string PhoneNumber { get; set; }
        public string EmailID { get; set; }
        public string Message { get; set; }
        public string EmailMessage { get; set; }
        public VMAddress buyeraddress { get; set; }
        public int Source_Name { get; set; }
        public int Destination_Name { get; set; }
        public int Order_Status { get; set; }
        public string ProductIds_Qty { get; set; }
        //Add Warehouse
        public int Sub_Unit_ID { get; set; }
        public string Unit_Name { get; set; }
        public string Phone_Number { get; set; }
        public string Email { get; set; }
        public string ContactName { get; set; }
        public string AddressLine_One { get; set; }
        public string AddressLine_Two { get; set; }
        public int VillageLocation_ID { get; set; }
        public string Suggested_UserName { get; set; }
        public string Unit_Additional_Data { get; set; }
        public int isWHcumStore { get; set; }
        public string Password { get; set; }
        public int user_ID { get; set; }
        //intrastore
        //public int OrderID { get; set; }
        //public string OrderDate { get; set; }
        //public int Status { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public int Order_Id { get; set; }
        public Nullable<DateTime> Order_Date { get; set; }
        //public string SKU { get; set; }
        //public string Order_Date { get; set; }
        public int TotalCount { get; set; }
        public Int16 Inventory_Product_Status { get; set; }
        public string Product_Name { get; set; }

        public string UnitName { get; set; }
        public string Order_productID { get; set; }
        public int TrackingID { get; set; }
        public int MasterID { get; set; }
        public int IsThirdParty { get; set; }
        public int todayorder { get; set; }
        public int ordernumber { get; set; }

        public int CID { get; set; }
        public int RejectStatus { get; set; }
        public string ReasonNote { get; set; }
        public int StoreID { get; set; }
        public string PayDate { get; set; }
        public string CashFlag { get; set; }
        public int Site_ID { get; set; }
        public Boolean bIsLeaf { get; set; }
        public Boolean bIsSectionStatus { get; set; }
        public string sProductType { get; set; }
        public string sParentID { get; set; }
        public List<VMBottomLinks> oFooterSections { get; set; }
        public Boolean bIsCheck { get; set; }
        public decimal LandingPriceINCGST { get; set; }
        public string BarCode { get; set; }
        public string sTitle { get; set; }
        public int iTitleStatus { get; set; }
        public Boolean Is_LeafNode { get; set; }
        public List<DomainsMasterData> oNewDomainData { get; set; }
        public List<MailSMSSettings> oNewMailSMSData { get; set; }
        public int Years { get; set; }
        public int Months { get; set; }
        public int UnitID { get; set; }
        public int Flag { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public List<Dictionary<string, object>> Resultset { get; set; }
        public string SearchDate { get; set; }
        public string SearchType { get; set; }
        public string PackedType { get; set; }
        public string Browser_Falg { get; set; }
        public Boolean bShowDistrict { get; set; }
        public string EditValues { get; set; }
        public string Percentage { get; set; }
        public int Orders_Value_Greter { get; set; }
        public int CashBacklistID { get; set; }
        public string ExcludedFilters { get; set; }
        public int WalletApplyon { get; set; }
        public string ExcludedEMIOrders { get; set; }
        public int Applyon { get; set; }
        public int saveToTables { get; set; }
        public string DomainName { get; set; }
        public string Subject { get; set; }
        public string sEmail { get; set; }
        public string Url { get; set; }
        public string ImagePath { get; set; }
        public string LogoTitle { get; set; }
        public List<PackedList> oPackedList { get; set; }
        public int ClusterID { get; set; }
        public int Inventory_Product_ID { get; set; }
        public string Damages { get; set; }
        public string Unique_Serial_Number { get; set; }
        public string Batch_Number { get; set; }
        public string Manufacture_Date { get; set; }
        public string Expiration_Date { get; set; }
        public int Login_UnitID { get; set; }
        public DateTime? Startdate { get; set; }
        public DateTime? Enddate { get; set; }
        public decimal ReceivingAmount { get; set; }
        public decimal Gatewaycharges { get; set; }
        public string NotesDescription { get; set; }
        public int ReconcileId { get; set; }
        public string paymentid { get; set; }
  		public int BuyerId { get; set; }
        public int ordercount { get; set; }
        public int ordercountfilter { get; set; }
        public string VendorOrder_type { get; set; }
		public string NewRefund { get; set; }
        public String Notes { get; set; }
		// Domains Configuration
        public int DCDaysFrom { get; set; }
        public int DCDaysTo { get; set; }
        public int VDDaysFrom { get; set; }
        public int VDDaysTo { get; set; }
        public int WHDaysFrom { get; set; }
        public int WHDaysTo { get; set; }
        public int ShowLocationType { get; set; }

		  //Stockrequestcancel
        public int RequestId { get; set; }

        public string fileName { get; set; }
    }
 public class VMUpdateCitiesOnDistrict
    {
        public int Site_ID { get; set; }
        public int Dist_ID { get; set; }
        public string City_IDs { get; set; }
        public string Delete_IDs { get; set; }
    }
    public class Scheme
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
        //Bulk orders
        public string Order_Date_From { get; set; }
        public string Order_Date_To { get; set; }
        public int Status { get; set; }

    }
    public class VMPrdctDetails
    {
        public string ProductName { get; set; }
        public string ProductSeries { get; set; }
        public string ImageCode { get; set; }
        public int Quantitiy { get; set; }
        public float Price { get; set; }
        public float TotalPrice { get; set; }
        public int Otp { get; set; }
        public int Notification_ID { get; set; }
        public string ConsignmentIDs { get; set; }
        public string Order_ID { get; set; }
        public int constatus { get; set; }
        public string Tax_Invoice_Number { get; set; }
        public int OrderedQuantity { get; set; }
        public string Mobile_Number { get; set; }
    }

    public class VMBottomLinks
    {
        public int PrimaryKey { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Site_ID { get; set; }
        public Boolean Is_LeafNode { get; set; }
        public string CategoryName { get; set; }
    }
    public class VMCategoriesModel
    {
        public int Category_Id { get; set; }
        public string Category_Name { get; set; }
        public string Category_Description { get; set; }
        public int Category_Parent_Id { get; set; }
        public int Category_Status { get; set; }
        public int Category_Priority { get; set; }
        public string Category_AliasNames { get; set; }
        public int User_Id { get; set; }

    }
    public class VMConsignments
    {
        public int ID { get; set; }
        public int OrderID { get; set; }
        public string Productids { get; set; }
        public int Status { get; set; }
        public int Consignment_ID { get; set; }
        public string Consignment_products { get; set; }
        public int NoOfBoxes { get; set; }
        public int Others { get; set; }
        public string Cons_Name { get; set; }
        public int Cons_Status { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public string CreateDate { get; set; }
        public string Source_UnitID { get; set; }
        public string Dest_UnitID { get; set; }
        public int pagesize { get; set; }
        public int pageindex { get; set; }
        public string Not_Recived_Inv_Ids { get; set; }
        public string Dammaged_Inv_Ids { get; set; }
        public int productid { get; set; }
        public string RemarksDeatils { get; set; }
        public string ProductName { get; set; }
        public string MobileNumber { get; set; }
        public string OrderType { get; set; }
        public string OrderBy { get; set; }
        public int Inv_Ids { get; set; }
        public int Unit_ID { get; set; }
        public int DC_Unit_ID { get; set; }
        public int Sku { get; set; }
        public int OrderNumber { get; set; }
        public int SKU { get; set; }
        public int Page_Size { get; set; }
        public int Page_Index { get; set; }
        public int CategoryID { get; set; }
        public int TopCategoryID { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public int Source_ID { get; set; }
        public int Dest_ID { get; set; }
        public int DomainId { get; set; }
    }

    public class BulkOrderDetails
    {
        public string Buyer_Name { get; set; }
        public int BUYER_ID { get; set; }
        public int UnitID { get; set; }
        public string ProductIds_Qty { get; set; }
        public string Payment_Mode_Type { get; set; }
        public string Use_CashBack_Amount { get; set; }
        public string Use_Wallet_Amount { get; set; }
        public decimal Coupon_Amount { get; set; }
        public int Buyer_Address_Id { get; set; }
        public string PaymentJson { get; set; }

        public int BuyerID { get; set; }
        public int AddressID { get; set; }
        public int PaymentType { get; set; }
        public string Products_Data { get; set; }
        public decimal BookingAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmnt { get; set; }

    }
    public class Address
    {
        public string PinCode { get; set; }
        public string CityName { get; set; }
        public string District { get; set; }
        public string LandMark { get; set; }
        public string StateName { get; set; }
        public string StreetName { get; set; }
        public string Housenumber { get; set; }
        public string MobileNumber { get; set; }
        public string Name { get; set; }
    }
    public class VMAddress
    {
        public Address Address { get; set; }
        public string FullName { get; set; }
        public string Village_Location { get; set; }
        public string Mandal_Location_Area { get; set; }
        public string Address_Line_One { get; set; }
        public string MobileNumber { get; set; }
        public string District_City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
    }

    public class FMCG
    {
        public string BatchNumber { get; set; }
        public string ManufactureDate { get; set; }
        public string ExpirationDate { get; set; }
        public int daystoexpire { get; set; }
        public int catid { get; set; }
        public string ProductName { get; set; }
        public string SKU { get; set; }
        public string BrandName { get; set; }
        public int pagesize { get; set; }
        public int pageindex { get; set; }
        public int Dc_ID { get; set; }
        public int ProductId { get; set; }
        public int qty { get; set; }
        public int Re_OrderLevel_Qnty { get; set; }
        public string AvailableQuantity { get; set; }
        public string SourceVendor { get; set; }
        public int MainCategory { get; set; }
        public int LeafCategory { get; set; }
        public int Quantity { get; set; }
        public int Condition { get; set; }
        public string sDc_ID { get; set; }
        public string flgExpired { get; set; }

    }

    public class iHub_Menu_Strip
    {
        [Key]
        public int Menu_ID { get; set; }
        public string Menu_Name { get; set; }
        public int Menu_Type { get; set; }
        public string Category_IDs { get; set; }
        public bool On_Hover { get; set; }
        public string Image_Name { get; set; }
        public string UIClass { get; set; }
        public int DisplayOrder { get; set; }
        public bool Status { get; set; }
        public int Parent_ID { get; set; }
        public string CategoryNames { get; set; }
        public int DomainID { get; set; }
        [NotMapped]
        public string Type { get; set; }
        [NotMapped]
        public string CatIDs { get; set; }
        [NotMapped]
        public string SubCatIDs { get; set; }
        [NotMapped]
        public string LeafIDs { get; set; }
        [NotMapped]
        public string RemoveIDs { get; set; }
    }

    public class iD_BottomLinks
    {
        [Key]
        public int ID { get; set; }
        public string LinkType { get; set; }
        public string Description { get; set; }
        public int MasterDataID { get; set; }
        public int iSiteID { get; set; }
        public string sName { get; set; }
        public int iDisplayOrder { get; set; }
        public int iStatus { get; set; }
        public string sType { get; set; }
        public string sParentID { get; set; }
        [NotMapped]
        public string Type { get; set; }
        [NotMapped]
        public string CatIDs { get; set; }
        [NotMapped]
        public string SubCatIDs { get; set; }
        [NotMapped]
        public string LeafIDs { get; set; }
    }

    public class PackedList
    {
        public int Dest_UnitID { get; set; }
        public int Source_UnitID { get; set; }
        public int OrderID { get; set; }
        public int productid { get; set; }
        public Boolean Selected { get; set; }
        public string ProductIDs { get; set; }
        public int status { get; set; }
        public int UnitID { get; set; }
        public float AllocatedAmt { get; set; }
        public float AvailableAmt { get; set; }
        public string FundsAmountAndType { get; set; }
        public string Notes { get; set; }
        public string Type { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int ReqId { get; set; }
        public string UpdateProData { get; set; }
        public float ReqAmnt { get; set; }
        public float AprvedAmnt { get; set; }
        public float DiffAmnt { get; set; }
        public string FundAdjustment { get; set; }
        public int Ordered_Product_Details_ID { get; set; }
        public int Ordered_Quantity { get; set; }
        [NotMapped]
        public int iLocationID { get; set; }
        [NotMapped]
        public int iProDetailsID { get; set; }
    }
    public class Traylist
    {
        public string ToDate { get; set; }
        public string FromDate { get; set; }
        public int Unit_ID { get; set; }
        public string DriverName { get; set; }
        public string TransactionType { get; set; }
        public int Quantity { get; set; }
        public int Condition { get; set; }
        public int TrayCount { get; set; }
        public string Notes { get; set; }
        public int pagesize { get; set; }
        public int pageindex { get; set; }
    }
    public class DomainsData
    {
        public int DomainID { get; set; }
        public string DomainName { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public int ProdSaleQtyLimit { get; set; }
        public bool showPriceRequestProd { get; set; }
        public string Type { get; set; }
        public string searchFlag { get; set; }
        public string PackageName { get; set; }
        public int Pagesize { get; set; }
        public int PageIndex { get; set; }


    }
}