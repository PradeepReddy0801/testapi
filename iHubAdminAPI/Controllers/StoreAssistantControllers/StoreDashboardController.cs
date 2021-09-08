using Excel;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;
using System.Web.Configuration;
using System.IO;
using iHubAdminAPI.Models;
using System.Configuration;
using AspNet.Identity.SQLDatabase;
namespace iHubAdminAPI.Controllers
{
    [RoutePrefix("api/iH_Store")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class StoreDashboardController : ApiController
    {
        string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private static readonly ILog log = LogManager.GetLogger(typeof(StoreDashboardController).FullName);
        public SQLDatabase _database;

          public StoreDashboardController()
          {
            _database = new SQLDatabase();
          }

        private class CustomResponse
        {
            internal string Response;
            internal int ResponseID;
        }

        #region "BEGIN of -- SP Names"

        // =================================Command Rext For Api Calls====================================================//
        string Inner_DashBoard = "[iStore_Get_Inner_Snippets_Data_For_Unit_Dashboard]";
        string Outer_DashBoard = "[iStore_Get_Outer_Snippets_Data_For_Unit_Dashboard]";
        string Store_cat_List = "[IS_Get_Store_Category_List ]";
        string Get_Store_Orders = "[iS_Get_All_Orders_By_Filters]";
        string Order_status = "[iS_Get_Store_Orders_By_Status_With_Month_Filter]";
        string Booking_Orders = "[iS_Get_Booking_Orders_List]";
        string OldEMI_Orders = "[iS_Get_Old_EMI_Orders_List]";
        string Reached_Booking_Orders = "[iS_Get_Reached_To_Store_Orders_With_Booking_Amount]";
        string Get_SubCat_By_ParentID = "[iS_Get_SubCategories_By_ParentID]";
        string Get_Products_By_CatIDs = "[iS_Get_Products_By_CategoryID]";
        string Cheque_Counter = "[iAdmin_Get_Cheque_Types_Details_By_UnitID]";
        string CounterFile_ByUnitId = "[iAdmin_Get_Cash_CounterFiles_By_UnitID]";
        string Check_Clear = "[iS_Get_Pending_And_Cleared_Cheque_CounterFiles]";
        string Created_Cheque = "[iAdmin_Get_created_Cheque_CounterFiles_By_UnitID]";
        string Get_Store_Orders_For_W_C_CB = "[iS_Get_All_Orders_For_Wallet_Coupon_CashBack_By_Filters]";
        string IN_Outflow = "[iS_InflowOutFlow_StockDetails]";
        string Get_Payment_Details = "[iS_Get_Buyer_Payment_Details]";
        string Raised_Bulk_Orders = "[iS_Get_All_Raised_Bulk_Orders]";
        string Consignment_List = "[iS_Get_Consignment_List_With_Filters]";
        string View_Consignment_List = "[iS_View_Created_Consignments]";
        string Change_Bulk_Status = "[iS_Get_Dammaged_Invetory_Product_Consignments_List]";
        string Change_Bulk_Status_Details = "[iS_Get_Dammaged_Inventory_Product_List_By_Consignment_Id]";
        string getDirectOrders = "[iDirect_Get_Direct_Orders_For_Store_Home_Delivery]";
        string Get_Ordered_Products = "[iDirect_Get_Buyer_Order_Product_Details_Direct]";//Gopi
        string Get_Ordered_Products_Inv = "[iDirect_Get_Buyer_Order_Product_Details_Inventory]" ;
        string All_Raised_List = "[iS_Get_All_Raised_Bulk_Order_Products]";
        string Get_Assigned_Products_List = "iS_Get_Assigned_Products_List_By_Source_UnitID_Dest_Unit_ID";

        #endregion "BEGIN of -- SP Names"

        #region "BEGIN of -- Select Tables"

        string Get_Assigned_Stock_ST_Units = "SELECT iHub_Unit_ID AS UnitID,Store_ID AS StoreID,Unit_Name AS UnitName,WareHouse_ID AS WareHouseID,Unit_Hierarchy_Level AS HierarchyLevel FROM iHub_Units_DC_WH_ST" +
                                              " JOIN iHub_Inventory_Products ON Unit_Id =iHub_Unit_ID WHERE Inventory_Product_Status = 30 AND Consignment_Id=0 AND Order_Id=0 AND Previous_Unit_Id =";
        string Get_Units_List = "SELECT \"iHub_Unit_ID\" AS \"UnitID\",\"Store_ID\" AS \"StoreID\",\"Unit_Name\" AS \"UnitName\"" +
                                  "FROM \"iHub_Units_DC_WH_ST\" WHERE \"Unit_Hierarchy_Level\" IN (3,2) AND \"Is_Unit_Active\" ='true' Order by \"Store_ID\" desc ";

        #endregion "END of -- Select Tables"

        #region "BEGIN of -- Dashboard"

        //===================== GET Assigned Stock From WH To Store By Filter ========================================================//
        [HttpGet]
        [Route("GetAssignedStockToStore")]
        public IHttpActionResult GetAssignedStockToStore(int WHID, int StoreID)
        {
            try
            {
                string Get_Assigned_Stock_To_Store = "SELECT \"iHub_Unit_ID\" AS \"UnitID\",\"Store_ID\" AS \"StoreID\",\"WareHouse_ID\" AS \"WareHouseID\",\"Unit_Hierarchy_Level\" AS \"HierarchyLevel\",\"Unit_Name\" AS \"UnitName\",  COUNT(\"Inventory_Product_ID\") AS \"Stock\" FROM \"iHub_Units_DC_WH_ST\"" +
                                                    " JOIN \"iHub_Inventory_Products\" ON \"Unit_Id\" = \"iHub_Unit_ID\" WHERE \"Inventory_Product_Status\" = 30 AND \"Consignment_Id\"=0 AND \"Previous_Unit_Id\" =" + WHID + "AND \"Unit_Id\"=" + StoreID + " GROUP BY iHub_Unit_ID,Store_ID,WareHouse_ID,Unit_Hierarchy_Level,Unit_Name ORDER BY iHub_Unit_ID";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { };
                var result = _database.SelectQuery(Get_Assigned_Stock_To_Store, parameters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                log.Error("Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        //=====================>>>>>>>>>Get_Data_For_Unit_Inner_Dashboard<<<<<<<<<<<<<<<<=========================//
        [Route("Get_Data_For_Unit_Inner_Dashboard")]
        public IHttpActionResult Get_Data_For_Unit_Inner_Dashboard(DateTime FirstDay, DateTime LastDay,int StoreId)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@UnitID", StoreId },
                                                                                            { "@OrderStartDate", FirstDay },
                                                                                            { "@OrderEndDate", LastDay },
                                                                                            { "@UserId", 0 },
                                                                                            { "@UserType", 0 } };
                var dashboard_result = _database.QueryValue(Inner_DashBoard, parameters);
                return Ok(dashboard_result);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }

        // =================================Method For getting Outer Dashboard Details by UnitID====================================================//
        [Route("Get_Data_For_Unit_Outer_Dashboard")]
        public IHttpActionResult Get_Data_For_Unit_Outer_Dashboard(DateTime FirstDay, DateTime LastDay,int StoreId)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@UnitID", StoreId },
                                                                                                { "@OrderStartDate", FirstDay },
                                                                                                { "@OrderEndDate", LastDay },
                                                                                                { "@UserId", 0 },
                                                                                                { "@UserType", 0 } };
                var dashboard_result = _database.QueryValue(Outer_DashBoard, parameters);
                return Ok(dashboard_result);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }

        // =================================Method For getting Dashboard Details by UnitID With Total Stock Detail====================================================//
        [HttpGet]
        [Route("Get_Stock_With_CatogoryList")]
        public IHttpActionResult Get_Stock_With_CatogoryList(int Unit_ID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@UnitID", Unit_ID }};
                var total_Stock = _database.ProductListWithCount(Store_cat_List, parameters);
                return Ok(total_Stock);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }

        // =================================Method For getting Subcatogory List by ParentId====================================================//
        [HttpPost]
        [Route("GetAllOrdersByStoreID")]
        public IHttpActionResult GetAllOrdersByStoreID(VMPagingResultsPost vmodel)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@UnitID", vmodel.StoreId },
                   { "@Is_Order_From", vmodel.orderfrom }, { "@Status", "" }, { "@Is_Today_Order", vmodel.todayorder },
                   { "@Payment_Type", vmodel.PaymentMode.ToLower() } ,{ "@Order_From", vmodel.orderdatefrom },{ "@Order_To", vmodel.orderdateto }
                   ,{ "@Order_Number ", vmodel.ordernumber } ,{ "@Buyer_Number", vmodel.MobileNumber }
                   ,{ "@PageSize", vmodel.Pagesize } ,{ "@PageIndex", vmodel.PageIndex }, { "@UserId", 0 }, { "@UserType", 0 }};
                var store_orders_result = _database.ProductListWithCount(Get_Store_Orders, parameters);
                return Ok(store_orders_result);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }

       //===================== method for Order status ======================================//
        [HttpPost]
        [Route("GetOrdersListBysatus")]
        public IHttpActionResult GetOrdersListBysatus(VMPagingResultsPost Vmo)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@UnitID", Vmo.StoreId }, { "@Status", Vmo.Status2 },
                 {"@Is_Order_From",Convert.ToInt32(Vmo.orderfrom) } , {"@Order_DateFrom",Vmo.orderdatefrom } , {"@Order_DateTo",Vmo.orderdateto } , {"@MobileNumber",Vmo.MobileNumber }, {"@OrderNumber",Vmo.ordernumber }, {"@OrderDate",Vmo.FilterJson }
                 , {"@PageSize",Vmo.Pagesize } , {"@PageIndex",Vmo.PageIndex }, {"@UserId",0 }, {"@UserType",0 }};
                var res = _database.ProductListWithCount(Order_status, parameters);
                return Ok(res);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }

        //===============================Admin Dashboard=============================================================
        [HttpPost]
        [Route("iHubAdminDashbord")]
        public IHttpActionResult iHubAdminDashbord()
        {
            try
            {            
                Dictionary<string, object> parameters = new Dictionary<string, object>() { };
                var res = _database.SelectQuery(Get_Units_List, parameters);
                return Ok(res);
            }
            catch (Exception ex)
            {
                log.Error("Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        //===================== Method for Get All Booking Orders =================//
        [HttpPost]
        [Route("GetBookingOrders")]
        public IHttpActionResult GetBookingOrders(VMPagingResultsPost vmodel)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@UnitID", vmodel.StoreId },{ "@IS_Order_From",vmodel.orderfrom },{ "@Order_DateFrom",vmodel.orderdatefrom },
                    { "@Order_DateTo", vmodel.orderdateto }, {"@MobileNumber", vmodel.MobileNumber}, {"@OrderNumber",vmodel.ordernumber }
                    , {"@OrderType",vmodel.Status  }, { "@Page_Size", vmodel.Pagesize }, { "@Page_Index", vmodel.PageIndex }, {"@UserId",0 }, {"@UserType",0 } };
                var res = _database.ProductListWithCount(Booking_Orders, parameters);
                return Ok(res);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }

        //===================== Method for Get All Booking Orders ==================//
        [HttpPost]
        [Route("GetOldEMIOrders")]
        public IHttpActionResult GetOldEMIOrders(VMPagingResultsPost vmodel)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@UnitID", vmodel.StoreId },
                    { "@IS_Order_From",vmodel.orderfrom },{ "@Order_DateFrom",vmodel.orderdatefrom },
                    { "@Order_DateTo", vmodel.orderdateto }, {"@MobileNumber", vmodel.MobileNumber}, {"@OrderNumber",vmodel.ordernumber }
                    , { "@Page_Size", vmodel.Pagesize }, { "@Page_Index", vmodel.PageIndex } };

                var res = _database.ProductListWithCount(OldEMI_Orders, parameters);
                return Ok(res);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }

        //===================== Method for Get All Booking Orders =================//
        [HttpPost]
        [Route("GetReachedToStoreBookingOrders")]
        public IHttpActionResult GetReachedToStoreBookingOrders(VMPagingResultsPost vmodel)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@UnitID", vmodel.StoreId }
                                                                                            ,{ "@OrderDateFrom",vmodel.orderdatefrom },
                                                                                            { "@OrderDateTo", vmodel.orderdateto },
                                                                                            { "@MobileNumber",vmodel.MobileNumber },
                                                                                            { "@OrderNumber", vmodel.ordernumber}
                                                                                            , {"@Pagesize",vmodel.Pagesize  },
                                                                                           { "@Pageindex", vmodel.PageIndex }, {"@UserId",0 }, {"@UserType",0 } };
                var res = _database.ProductListWithCount(Reached_Booking_Orders, parameters);
                return Ok(res);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }

        //===================== Get Registered Users =================//
        [HttpGet]
        [Route("Get_Registered_Users")]
        public IHttpActionResult Get_Registered_Users(string FilterDate,int StoreId)
        {
            try
            {            
                string Registered_Users = "SELECT * FROM iD_Buyers WHERE Register_UnitID=" + StoreId + " AND CONVERT(DATE,Created_Date)  LIKE '%" + FilterDate + "%'";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { };
                var result = _database.SelectQuery(Registered_Users, parameters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                log.Error("Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        // =================================Method For getting Subcatogory List by ParentId====================================================//
        [HttpGet]
        [Route("GetStoreProductsByCategoryID")]
        public IHttpActionResult GetStoreProductsByCategoryID(int CategoryID, int UnitID, int PageIndex, int PageSize)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Parent_Id", CategoryID }};
                var total_Stock = _database.ProductListWithCount(Get_SubCat_By_ParentID, parameters);
                string IDs = "";
                if (total_Stock.Resultset.Count > 0)
                {
                    foreach (var item in total_Stock.Resultset)
                    {
                        IDs = IDs + item["ID"] + ",";
                    }
                    IDs = IDs.Remove(IDs.Length - 1).ToString();
                    Dictionary<string, object> Parameters = new Dictionary<string, object>() { { "@Cat_Ids", IDs }, { "@UnitID", UnitID }, { "@Pageindex", PageIndex }, { "@Pagesize", PageSize } };
                    var Result = _database.ProductListWithCount(Get_Products_By_CatIDs, Parameters);
                    return Ok(Result);
                }
                else
                {
                    return Ok("No Products Found");
                }
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }

        //==========================>>>>>>>>>>>GetAllChequeCounterFileDetails<<<<<<<<<<<<<<=======================//
        [HttpGet]
        [Route("GetAllChequeCounterFileDetails")]
        public IHttpActionResult GetAllChequeCounterFileDetails(int Unitid)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Unitid", Unitid } };
                var res = _database.QueryValue(Cheque_Counter, parameters);
                return Ok(res);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }

        //=========================To Get Cash Counter Files========================================================
        [HttpPost]
        [Route("GetCashCounterFilesByUnitID")]
        public IHttpActionResult GetCashCounterFilesByUnitID(VMPagingResultsPost VMP)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@UnitId", VMP.StoreId }, { "@FileType", VMP.ParentCategoryName },  { "@PageIndex", VMP.PageIndex }, { "@PageSize", VMP.Pagesize } };
                var res = _database.ProductListWithCount(CounterFile_ByUnitId, parameters);
                return Ok(res);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }

        //=========================Method For get Counter File Cheque Cleared=================================
        [HttpPost]
        [Route("GetChequePendingAndClearedCounterFiles")]
        public IHttpActionResult GetChequePendingAndClearedCounterFiles(VMPagingResultsPost VMP)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@UnitID", VMP.StoreId }, { "@Status", VMP.Status }, { "@OrderDateFrom", VMP.orderdatefrom }, { "@OrderDateTo", VMP.orderdateto }
                   ,{ "@OrderNumber", VMP.OrderID },{ "@PageSize", VMP.Pagesize } , { "@PageIndex", VMP.PageIndex }, };
                var res = _database.ProductListWithCount(Check_Clear, parameters);
                return Ok(res);
                
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }

        //===================== Method for Created Cheque CounterFiles=================
        [HttpPost]
        [Route("Get_Created_Cheque_CounterFiles")]
        public IHttpActionResult Get_Created_Cheque_CounterFiles(VMPagingResultsPost vmodel)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@UnitID", vmodel.StoreId }, { "@orderdatefrom", vmodel.orderdatefrom }, { "@orderdateto", vmodel.orderdateto }
                , { "@OrderID", vmodel.OrderID }, { "@pagesize", vmodel.Pagesize }, { "@pageindex", vmodel.PageIndex }};
                var res = _database.ProductListWithCount(Created_Cheque, parameters);
                return Ok(res);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }

        // =================================Method For getting Subcatogory List by ParentId====================================================
        [HttpPost]
        [Route("GetAllOrdersForWalletCouponCashBackByStoreID")]
        public IHttpActionResult GetAllOrdersForWalletCouponCashBackByStoreID(VMPagingResultsPost vmodel)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@UnitID", vmodel.StoreId },
                   { "@is_order_from", vmodel.orderfrom }, { "@status", "" }, { "@is_today_order", vmodel.todayorder },
                   { "@payment_type", vmodel.PaymentMode.ToLower() } ,{ "@orderdate_from", vmodel.orderdatefrom },{ "@orderdate_to", vmodel.orderdateto }
                   ,{ "@order_number", vmodel.ordernumber } ,{ "@buyer_number", vmodel.MobileNumber }
                   ,{ "@pagesize", vmodel.Pagesize } ,{ "@pageindex", vmodel.PageIndex }, {"@UserId",0 }, {"@UserType",0 }};
                var FilterOrders = _database.ProductListWithCount(Get_Store_Orders_For_W_C_CB, parameters);
                return Ok(FilterOrders);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }

        //===================== Get Special Requests =================
        [HttpGet]
        [Route("Get_Special_Requested_Products")]
        public IHttpActionResult Get_Special_Requested_Products(string FilterDate,int StoreId)
        {
            try
            {
                string Special_Requested_Products = "SELECT * FROM iH_Customer_Special_Requested_Products WHERE Unit_Id=" + StoreId + " AND CONVERT(DATE,Requested_Date)  LIKE '%" + FilterDate + "%' ";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { };
                var result = _database.Query(Special_Requested_Products, parameters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                log.Error("Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        //===================== Method for Inflow and out flow stock =================
        [HttpPost]
        [Route("Get_IN_Outflow_Stock")]
        public IHttpActionResult Get_IN_Outflow_Stock(VMPagingResultsPost gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                                                       { "@intUnitID", gt.Source_UnitID },
                                                       { "@intRequest_Type", Convert.ToInt32(gt.RelevanceType) },
                                                       { "@dtOrder_From", gt.orderdatefrom },
                                                       { "@dtOrder_To", gt.orderdatefrom },
                                                       { "@intOrder_Number", gt.OrderID },
                                                       { "@vcStatus", gt.Status2 },
                                                       { "@intSKU", gt.ProductID },
                                                       { "@intPrevUnitID", gt.LocationID },
                                                       { "@intHierarchy_Level", gt.Hierarchy_Level },
                                                       { "@intPageSize",  gt.Pagesize},
                                                       { "@intPageIndex", gt.pageindex}};
                var res = _database.GetMultipleResultsList2(IN_Outflow, parameters);
                return Ok(res);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }

        //============================================Get Payment Details=====================================//
        [HttpGet]
        [Route("Get_PayementDetails")]
        public IHttpActionResult Get_PayementDetails(int IDPK)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@OrdersMainID", IDPK }};
                var res = _database.ProductListWithCount(Get_Payment_Details, parameters);
                return Ok(res);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }

        //===================== Get Price Requests =================
        [HttpGet]
        [Route("Get_Requested_Products")]
        public IHttpActionResult Get_Requested_Products(string FilterDate,int StoreId)
        {
            try
            {
                string Requested_Products = "SELECT CR.*,Product_Series+'-'+CONVERT(VARCHAR(120),iHub_Product_ID+100000) AS SKU,Product_Name AS ProductName FROM iH_Customer_Requested_Products CR" +
                  " JOIN iHub_Products ON iHub_Product_ID=CR.Product_Id"
                  + " WHERE Unit_Id=" + StoreId + " AND CONVERT(DATE,Requested_Date) LIKE '%" + FilterDate + "%'";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { };
                var result = _database.Query(Requested_Products, parameters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                log.Error("Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        //==================================raised Bulk Order=================================================
        [HttpPost]
        [Route("GetAllRaisedBulkOrdersByUnitID")]
        public IHttpActionResult GetRaisedBulkOrdersService(Scheme Sc)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                    { "@Order_Number", Sc.OrderNumber}
                    ,{ "@Order_Date_From", Sc.Order_Date_From}
                    ,{ "@Order_Date_To", Sc.Order_Date_To}
                    ,{ "@Mobile_Number", Sc.MobileNumber}
                    ,{ "@UnitID", Sc.UnitID}
                    ,{ "@Status", Sc.Status}
                    ,{ "@Page_Index", Sc.Page_Index}
                    , { "@Page_Size", Sc.Page_Size }
                   };
                var res = _database.ProductListWithCount(Raised_Bulk_Orders, parameters);
                return Ok(res);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("GetTableRecords----Error---------", ex);
                return Ok(cs);
            }
        }

        //============================To Get Consignment List=======================================================
        [HttpPost]
        [Route("Get_Consignments_List")]
        public IHttpActionResult Get_Consignments_List(VMConsignments consignment)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Cons_Name", consignment.Cons_Name},
                                                                                           { "@Cons_Status", consignment.Cons_Status },
                                                                                           { "@Source_ID", consignment.Source_UnitID },
                                                                                           { "@Dest_ID", consignment.Dest_UnitID },
                                                                                           { "@CreateDate", consignment.CreateDate },
                                                                                           { "@Page_size", consignment.pagesize},
                                                                                           { "@Page_Index", consignment.pageindex }
                };
                var result = _database.ProductListWithCount(Consignment_List, parameters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                log.Error("Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        //===================== GET Assigned Products List =====================================================
        [HttpGet]
        [Route("ViewProductsInConsignment")]
        public IHttpActionResult ViewProductsInConsignment(int ConsignmentID)
        {
            try
            {         
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Con_ID", ConsignmentID } };
                var result = _database.ProductListWithCount(View_Consignment_List, parameters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                log.Error("Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        //==============================================Get Dammaged Inventory product====================================
        [HttpPost]
        [Route("Get_Dammaged_Invetory_Product_Consignments_List")]
        public IHttpActionResult Get_Dammaged_Invetory_Product_Consignments_List(VMConsignments consignment)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {{ "@Source_Unit_ID", Convert.ToInt32(consignment.Source_UnitID) },
                                                                                           { "@Dest_Unit_ID", Convert.ToInt32(consignment.Dest_UnitID) },
                                                                                           { "@ProductID", consignment.productid },
                                                                                           { "@Cons_ID", consignment.Consignment_ID },
                                                                                           { "@ConsignmentName",consignment.Cons_Name },
                                                                                            { "@Con_Created_Date",consignment.CreateDate }};
                var result = _database.ProductListWithCount(Change_Bulk_Status, parameters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                log.Error("Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        //===================================Get Dammaged Inventory Product Consignment  List ===================================
        [HttpPost]
        [Route("Get_Dammaged_Invetory_Product_List_Consignments_List")]
        public IHttpActionResult Get_Dammaged_Invetory_Product_List_Consignments_List(VMConsignments consignment)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Cons_ID", consignment.Consignment_ID },
                                                                                           { "@Source_Unit_ID", Convert.ToInt32(consignment.Source_UnitID) },
                                                                                           { "@Dest_Unit_ID", Convert.ToInt32(consignment.Dest_UnitID) },
                                                                                           { "@ProductID", consignment.productid }
                                                                                           };
                var result = _database.ProductListWithCount(Change_Bulk_Status_Details, parameters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                log.Error("Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        // =================================Make Paymnet ====================================================
        [HttpPost]
        [Route("GetDirectOrdersForStoreHomeDelivery")]
        public IHttpActionResult GetDirectOrdersForStoreHomeDelivery(VMPagingResultsPost vmodel)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                    { "@OrderNumber", Convert.ToInt32(vmodel.OrderID) },
                    { "@OrderDateFrom", vmodel.orderdatefrom },
                    { "@OrderDateTo", vmodel.orderdateto },
                    { "@MobileNumber", vmodel.MobileNumber },
                    { "@UnitID", vmodel.iStoreID },
                    { "@Order_Type", Convert.ToInt32(vmodel.ID) },
                    { "@Page_Index", vmodel.pagesize },
                    { "@Page_Size", vmodel.PageIndex }};
                var res = _database.ProductListWithCount(getDirectOrders, parameters);
                return Ok(res);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }

        //============================================Get Ordered Products By OrderID=====================================
        [HttpGet]
        [Route("GetOrderedProducts_Direct")]
        public IHttpActionResult GetOrderedProducts_Direct(int OrderID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@OrderID", OrderID }};
                var res = _database.ProductListWithCount(Get_Ordered_Products, parameters);
                return Ok(res);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }

        //============================================Get Ordered Products By OrderID=====================================
        [HttpGet]
        [Route("GetOrderedProducts_Inventory")]
        public IHttpActionResult GetOrderedProducts_Inventory(int OrderID, int unitid)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@OrderID", OrderID }, { "@unitid", unitid }};
                var res = _database.ProductListWithCount(Get_Ordered_Products_Inv, parameters);
                return Ok(res);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }

        //===================== GET Stores List By WareHouse ID =================
        [HttpGet]
        [Route("GetAssignedStockStoresUnits")]
        public IHttpActionResult GetAssignedStockStoresUnits(int WareHouseID)
        {
            try
            {
                Get_Assigned_Stock_ST_Units = Get_Assigned_Stock_ST_Units + WareHouseID + " GROUP BY iHub_Unit_ID,Store_ID,Unit_Name,WareHouse_ID,Unit_Hierarchy_Level ORDER BY iHub_Unit_ID";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { };
                var result = _database.SelectQuery(Get_Assigned_Stock_ST_Units, parameters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                log.Error("Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        //===================== GET Assigned Stock From WH To Store =================
        [HttpGet]
        [Route("GetAssignedStockWHToStore")]
        public IHttpActionResult GetAssignedStockWHToStore(int WHID)
        {
            try
            {
                string Get_Assigned_Stock_WH_To_Store = "SELECT iHub_Unit_ID AS UnitID,Store_ID AS StoreID,Unit_Name AS UnitName,COUNT(Inventory_Product_ID) AS Stock FROM iHub_Units_DC_WH_ST" +
                                                 " JOIN iHub_Inventory_Products ON Unit_Id = iHub_Unit_ID WHERE Inventory_Product_Status = 30 AND Consignment_Id=0 AND Order_Id=0 AND Previous_Unit_Id =" + WHID + " GROUP BY iHub_Unit_ID,Store_ID,Unit_Name ORDER BY iHub_Unit_ID";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { };
                var result = _database.SelectQuery(Get_Assigned_Stock_WH_To_Store, parameters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                log.Error("Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        //===================== GET Assigned Products List =================
        [HttpPost]
        [Route("GetAssignedProductsList")]
        public IHttpActionResult GetAssignedProductsList(VMConsignments consignment)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Unit_ID", consignment.Unit_ID },
                                                                                           { "@DC_Unit_ID", consignment.DC_Unit_ID },
                                                                                           { "@SKU",consignment.Sku },
                                                                                           { "@OrderNumber",consignment.OrderNumber },
                                                                                           { "@ProductName",consignment.ProductName },
                                                                                           { "@MobileNumber",consignment.MobileNumber },
                                                                                           { "@OrderType",consignment.OrderType },
                                                                                           { "@OrderBy",consignment.OrderBy },
                                                                                           { "@Page_Size", 10 },
                                                                                           { "@Page_Index",1 }, };
                var result = _database.ProductListWithCount(Get_Assigned_Products_List, parameters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                log.Error("Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        //===================== GET Assigned Products List =================
        [HttpGet]
        [Route("ViewAllRaisedProductsByOrderID")]
        public IHttpActionResult Get_All_Raised_Bulk_Order_Products(int OrderID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@OrderId", OrderID } };
                var result = _database.GetMultipleResultsList(All_Raised_List, parameters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                log.Error("Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        #endregion "END of -- Dashboard"

    }
}