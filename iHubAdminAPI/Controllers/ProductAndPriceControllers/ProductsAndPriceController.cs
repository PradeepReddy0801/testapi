using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using iHubAdminAPI.Models;
using System.Configuration;
using AspNet.Identity.SQLDatabase;
using System.Web.Script.Serialization;
using Excel;
using iHubAdminAPI.Models.ProductAndPrice;
using System.IO;
using iHubAdminAPI.Mailer;
using System.Web.Http.Results;
using System.Web.Configuration;
using System.Web.Caching;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;

namespace iHubAdminAPI.Controllers
{
    [RoutePrefix("api/ProductAndPrice")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProductsAndPriceController : ApiController
    {
        string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private static readonly ILog log = LogManager.GetLogger(typeof(ProductsAndPriceController).FullName);
        public SQLDatabase _database;
        iHubDBContext dbContext = new iHubDBContext();
        IUsermailer Mailer = new Usermailer();

        public ProductsAndPriceController()
        {
            _database = new SQLDatabase();
        }
        private class CustomResponse
        {
            internal string Response;
            internal int ResponseID;
        }
        #region "BEGIN of -- SP Names"

        // ===================>>>>>>>>>>>>>  Command Text For Api Calls  <<<<<<<<<<<<==========================
        string Brands_List = "iAdmin_Get_All_Brands";
        string Group_Filters_Types = "[iAdmin_Get_Grouping_Keys_By_Category_ID]";
        string UnGrouped_Product_List = "[iAdmin_Get_Un_Grouped_Products_By_AttributeNames]";
        string View_Products_Details = "[iAdmin_Get_Product_Details_By_ID]";
        string Change_Repurchasable_Status = "[iAdmin_Change_Repurchasable_Status]";
        string Update_Products_By_Manual = "[iAdmin_Update_Multiple_Products_With_Execl_And_Leaf_CategoryID]";
        string Product_Status_Changed = "[iAdmin_Change_Product_Status]";
        string Update_Grouping_Products_By_GroupID = "[iAdmin_Update_Grouping_Products_By_GroupID]";
        string Accessories_To_Productss = "[iAdmin_Assign_Accessories_To_Products]";
        string Products_By_IDs = "[iAdmin_Products_By_Ids]";
        string View_Cash_Reports_Details = "[iStore_Get_Cash_Reports_Details]";
        string product_GetList_New = "[iAdmin_Get_Category_Products_By_Filter_With_Paging_And_Excel]";
        string product_GetList_Bin_Location = "[iAdmin_Get_Category_Products_By_With_Paging_And_Bin_Location]";
        string un_Price_Category_GetListnew = "[iAdmin_Get_Product_Types_For_Price_Approvel_Products]";
        string Get_ProductList_onlyPricing_New = "[iAdmin_Get_Price_Approal_Products_By_Filter_With_Paging]";
        string update_Product_Price_Bulk_New = "[iAdmin_Approve_Product_Price]";
        string change_bulk_Status = "[iAdmin_Update_Bulk_Status]";
        string Products_List = "[iAdmin_Get_All_Brands_Category_List]";
        string Cat_List = "[iAdmin_Get_All_Brands_Sub_Category_List]";
        string Update_Product_Percentage_New = "[iAdmin_Update_Product_Percentages]";
        string Update_Footer_Sections_New = "[iAdmin_Add_Update_Footer_Sections]"; //Ragi Naresh
        string All_Brands_Sub_Category_And_Product_List = "[iAdmin_Get_All_Brands_Sub_Category_And_Product_List]";
        string pricing_Product_GetList_New = "[iAdmin_Get_Active_Products_By_Category_And_Filters_With_Paging]";
        string Create_Product_Group = "[iAdmin_Create_Or_Update_Or_Remove_Product_Groupings]";
        string Grouped_Product_List = "[iAdmin_Get_Grouped_Products_By_Filters]";
        string AllowDirectSales = "[iAdmin_Update_Sales_Status]";
        string StoreOrder = "[iAdmin_Get_All_Store_Orders_For_Admin]";
        string DirectOrder = "[iAdmin_Get_All_Direct_Orders_For_Admin]";
        string BUlkOrder = "[iAdmin_Get_All_Bulk_Orders_For_Admin]";
        string CancelOrder = "[iAdmin_Get_All_Cancelled_Orders_Admin]";
        string Get_PriceRequest_Data = "[iAdmin_Get_Buyer_Price_Requests]";
        string Get_SpecialRequest_Data = "[iAdmin_Get_Customer_Special_Requests]";
        string Category_GetListnew = "[iAdmin_Get_Buyer_Order_Product_Details]";
        string Get_Category_Products = "[iAdmin_Get_Category_Products_By_Filter_With_Paging]";
        string Add_Update_Address = "[iAdmin_Add_Update_Address_Details_By_User_Id]";
        string Get_Add_Details = "[iAdmin_Get_Address_Details_By_User_Id]";
        string Remove_Accessories_Products = "[iAdmin_Remove_Accessories_By_Prod_ID_And_Acces_ID]";
        string Get_Products_List_By_Skus = "[iAdmin_Get_All_Products_List_By_Skus]";
        string Get_Details = "[iAdmin_Get_User_Details_By_Mobile_Number]";
        string Generate_Bulk_Order_For_Selected_Unit = "[iAdmin_Raise_New_Bulk_Order]";
        string Get_orders_Paging = "[iAdmin_Get_All_Store_Orders_For_Admin_Cancel]";
        string Change_SpecialRequest_Status = "[iAdmin_Change_SpecialRequest_Status]";
        string Change_CustomerRequest_Status = "[iAdmin_Change_CustomerRequest_Status]";
        string Raised_Bulk_Orders = "[iAdmin_Get_All_Raised_And_Created_Bulk_Order]";
        string EMI_Request_Details = "[iAdmin_Get_Buyer_EMI_Settings]";
        string Buyer_EMI = "[iAdmin_Add_Update_Buyer_EMI_Settings]";
        string Get_Ordered_WareHouse = "[iAdmin_Get_All_WareHouse_Orders_Admin]";
        string Add_Master_Location_Data = "[iAdmin_Add_Master_Locations_And_Pincodes_By_Excel]";
        string Validate_OTP = "[iAdmin_Validate_OTP]";
        string To_Add_Update_Wallet_Amount = "[iAdmin_Add_Update_Wallet_Amount]";
        string Admin_Dash_Board_Deatails = "[iAdmin_Admin_Dash_Board_Deatails]";
        string GetOrdered_Products_Direct = "[iAdmin_Get_Buyer_Order_Product_Details_Direct]";
        string All_Raised_List = "[iAdmin_Get_All_Raised_Bulk_Order_Products]";
        string To_Cancel_Ordered_Products_On_Request = "[iAdmin_Cancel_Ordered_Products_On_Request]";
        string Get_Ordered_Products_Loc = "[iAdmin_Get_Buyer_Order_Product_Details_Location]";
        string Get_assign_Logistic = "[iAdmin_Assigned_Logistic_Orders]";
        string Change_Auto_Stock_Status = "[iAdmin_Change_Auto_Stock_Allocation_Status]";
        string Get_All_Categories = "[iAdmin_Get_Categories_For_Price_Request]";
        string Update_Campaign_Wallet_Amount = "[iAdmin_Add_Update_Campaign_Wallet_Amount]";
        string Category_GetList_EMI = "[iAdmin_Get_Buyer_EMI_Order_Product_Details]";
        string Get_Ordered_Products_Inv = "[iAdmin_Get_Buyer_Order_Product_Details_Inventory]";
        string Order_details = "[iAdmin_Get_Order_Details_By_Order_Number_Direct]";
        string Update_Warehouse_and_vendor_Status_Direct = "[iAdmin_Update_Warehouse_and_vendor_Status_Direct]";
        string assign_Logistic = "[iAdmin_Assign_Order_To_Logistics]";
        string Order_details_Inv = "[iAdmin_Get_Order_Details_By_Order_Number_Inventory]";
        string To_Add_Top_Brands = "[iAdmin_Add_Top_Brands]";
        string View_Cash_Reports = "[iStore_Get_Cash_Reports]";
        string Products_Hsn_Code_Update = "[iAdmin_Update_Product_HSN_Code_Bulk]";
        string Get_Direct_orders_Logistics = "[iAdmin_Get_All_Direct_Orders_For_Logistics]";
        string Get_Direct_orders_WHDC = "[iAdmin_Get_All_Direct_Orders_For_WHDC]";
        string Upload_New_Products = "[iAdmin_Add_Products_With_Selected_Category]";
        string GetRTSDuePayments = "[iAdmin_Get_RTS_Due_Payments]";
        string Get_Direct_orders_WHDC_App = "[iAdmin_Get_All_Direct_Orders_For_WHDC_App]";
        string Get_Ordered_Products_Inv_App = "[iAdmin_Get_Buyer_Order_Product_Details_Inventory_DCAPP]";
        string Get_Product_IMEIInfo_App = "[SP_Get_IMEIInfo_Product]";
        string Get_Productslist_BinlocationApp = "[iAdmin_Get_Products_By_SkuProdname_And_Bin_Location]";
        string Close_EMI = "[iAdmin_Close_EMI_Manually]";
        string MakeEMIPayment = "[iAdmin_Make_EMI_Payment]";
        string Get_UnitIds_Having_SerialNo = "[iAdmin_Get_IMEI_Report]";
        string Get_Product_serialNo_By_UnitId = "[iAdmin_Get_IMEI_List_ByUnitID]";
        string Update_serialNo = "[iAdmin_Update_IMEI_For_Inventory_Products]";
        string Get_Tray_Reords = "[iAdmin_View_Total_Available_Trays]";
        string Tray_Transactions= "[iAdmin_View_Tray_Transactions]";
        string HandOver_Trays="[iAdmin_Update_HandOver_Trays]";
        string Get_Consignment_ProductIMEI = "[iAdmin_Consignment_GetIMEIInfo_Product]";
        string Add_Franchise_Master_Location_Data = "[Add_Franchise_Master_Locations_And_Pincodes_By_Excel]";
        string To_Cancel_Order_On_Request = "[iAdmin_Cancel_Order_On_Request]";
        string Get_All_iHub_PackagesBundlesList = "[iAdmin_GetAll_Packages_Bundles]";
        string MapDomains_toProducts = "[iAdmin_MapDomain_Product_Category]";

        #endregion "BEGIN of -- SP Names"

        #region "BEGIN of -- ProductAndPrice"

        [HttpPost]
        [Route("Get_RTS_Due_Payments")]
        public IHttpActionResult Get_RTS_Due_Payments(VMPagingResultsPost vm)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                    { "@unitid", vm.Unit_ID },
                    { "@orderid", vm.OrderID },
                    { "@sku", vm.SKU },
                    { "@productname", vm.ProductName },
                    { "@orderdatefrom", vm.orderdatefrom },
                    { "@orderdateto", vm.orderdateto },
                    { "@mobilenumber", vm.MobileNumber},
                    { "@topcategoryid", vm.CategoryID},
                    { "@pagesize", vm.Pagesize},
                    { "@pageindex", vm.PageIndex}
                };
                var rtsduepayments = _database.GetMultipleResultsList2(GetRTSDuePayments, parameters);
                return Ok(rtsduepayments);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Get_RTS_Due_Payments---Error---------", ex);
                return Ok(cs);
            }
        }

        //===================Get ProductList With Excel======================//
        [HttpPost]
        [Route("GetProductListWithExcel")]
        public IHttpActionResult GetProductListWithExcel(VMModelsForProduct Cat)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Filter_Json", Cat.FilterJson },
                                                                                           { "@Category_ID", Cat.CategoryID },
                                                                                           { "@ProductName", Cat.ProductName },
                                                                                           { "@SKU", Cat.SKU },
                                                                                           { "@PageSize", Cat.Pagesize },
                                                                                           { "@PageIndex", Cat.PageIndex } };
                var product_Get_List = _database.GetMultipleResultsList(product_GetList_New, parameters);
                return Ok(product_Get_List);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Get_Category_Attributes---Error---------", ex);
                return Ok(cs);
            }
        }

        //===================Get ProductList With Location======================//
        [HttpPost]
        [Route("GetProductListWithBinLocation")]
        public IHttpActionResult GetProductListWithBinLocation(VMModelForProductCategoryUnitBinLocation Cat)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                                                                                           { "@categoryid", Cat.CategoryID },
                                                                                           { "@unitid", Cat.UnitID},
                                                                                           { "@productname", Cat.ProductName },
                                                                                           { "@sku", Cat.SKU },
                                                                                           { "@orderdatefrom", Cat.CreatedDateFrom},
                                                                                           { "@orderdateto", Cat.CreatedDateTo},
                                                                                           { "@pagesize", Cat.PageSize },
                                                                                           { "@pageindex", Cat.PageIndex } };
                var product_Get_List = _database.GetMultipleResultsList(product_GetList_Bin_Location, parameters);
                return Ok(product_Get_List);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Get_Category_Attributes---Error---------", ex);
                return Ok(cs);
            }
        }
        //===================Get Only UnPriceCategoriesList Only======================//
        [HttpGet]
        [Route("GetOnlyUnPriceCategoriesListOnly")]
        public IHttpActionResult GetOnlyUnPriceCategoriesListOnly()
        {

            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { };
                var un_Price_CategoryGetList = _database.ProductListWithCount(un_Price_Category_GetListnew, parameters);
                return Ok(un_Price_CategoryGetList);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Get_Category_Attributes---Error---------", ex);
                return Ok(cs);
            }

        }

        //===================Get ProductList Only Pricing======================//
        [HttpPost]
        [Route("GetProductListOnlyPricing")]
        public IHttpActionResult GetProductListOnlyPricing(VMModelsForProduct Cat)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Filter_Json", Cat.FilterJson },
                                                                                           { "@Category_ID", Cat.CategoryID },
                                                                                           { "@ProductName", Cat.ProductName },
                                                                                           { "@pagesize", Cat.Pagesize },
                                                                                           { "@pageindex", Cat.PageIndex } };
                var Get_ProductList_onlyPricing = _database.ProductListWithCount(Get_ProductList_onlyPricing_New, parameters);
                return Ok(Get_ProductList_onlyPricing);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Get_Category_Attributes---Error---------", ex);
                return Ok(cs);
            }
        }
        //===================Update Product Price bulk======================//
        [HttpPost]
        [Route("UpdateProductPricebulk")]
        public IHttpActionResult UpdateProductPricebulk(VMModelsForProduct bulk)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ProductIDs", bulk.productids },
                                                                                           { "@User_ID", 1 } };
                var update_Product_Price_Bulk = _database.ProductListWithCount(update_Product_Price_Bulk_New, parameters);
                return Ok(update_Product_Price_Bulk);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Get_Category_Attributes---Error---------", ex);
                return Ok(cs);
            }
        }

        //===================Change Bulk Status Service======================//

        [HttpPost]
        [Route("ChangebBulkStatusService")]
        public IHttpActionResult ChangebBulkStatusService(VMModelsForProduct bulk)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ProductIDs", bulk.JsonData },
                                                                                           { "@Status", bulk.Status },
                                                                                           { "@Status_Type", bulk.Status2 } };
                var bulk_Status = _database.Query(change_bulk_Status, parameters);
                return Ok(bulk_Status);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Get_Category_Attributes---Error---------", ex);
                return Ok(cs);
            }
        }

        //===================Update Products Percentage======================//
        [HttpPost]
        [Route("Update_Products_Percentage")]
        public IHttpActionResult Update_Products_Percentage(VMModelsForProduct bulk)
        {
            CustomResponse CR = new CustomResponse();
            try
            {
                int i = 0;
                int iProID = 0;
                decimal dSelling = 0;
                //return null;
                if (bulk.productids != null)
                {
                    if (bulk.productids != null && bulk.sSellingPrice != null)
                    {
                        var oPro = bulk.productids.Split(',').ToList();
                        var oSell = bulk.sSellingPrice.Split(',').ToList();
                        if (oPro.Count() == oSell.Count())
                        {
                            foreach (var P in oPro)
                            {
                                //return null;
                                iProID = Convert.ToInt32(P);
                                dSelling = Convert.ToDecimal((oSell[i]));
                                Dictionary<string, object> param = new Dictionary<string, object>() { { "@Product_Ids", iProID },
                                                                                           { "@Category_ID", bulk.CategoryID },
                                                                                           { "@Brand_Name", bulk.BrandName },
                                                                                           { "@Percentage_Type", bulk.PercentageType },
                                                                                           { "@Pecentage", bulk.Pecentage},
                                                                                           { "@SellingPrice", dSelling}};
                                var Update_Product_Percentages = _database.QueryValue(Update_Product_Percentage_New, param);
                                i++;
                            }
                            CR.ResponseID = i;
                        }
                    }
                }
                return Ok(i + " Records Updated Successfully!");
                //Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Product_Ids", bulk.productids },
                //                                                                           { "@Category_ID", bulk.CategoryID },
                //                                                                           { "@Brand_Name", bulk.BrandName },
                //                                                                           { "@Percentage_Type", bulk.PercentageType },
                //                                                                           { "@Pecentage", bulk.Pecentage},
                //                                                                           { "@SellingPrice", bulk.SellingPrice}};
                //var Update_Product_Percentage = _database.QueryValue(Update_Product_Percentage_New, parameters);
                //return Ok(Update_Product_Percentage);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Get_Category_Attributes---Error---------", ex);
                return Ok(cs);
            }
        }


        //===================Get All Brands======================//
        [HttpGet]
        [Route("Get_Brands_List")]
        public IHttpActionResult Get_Brands_List()
        {

            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { };

                var res = _database.ProductListWithCount(Brands_List, parameters);

                return Ok(res);
            }
            catch (Exception ex)
            {
                log.Error("Error--", ex);
                CustomResponse cs = new CustomResponse();
                cs.ResponseID = 0;
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        //===================Get Categories Based On Brand Name======================//
        [HttpGet]
        [Route("Get_Catgory_Brands_List")]
        public IHttpActionResult Get_Catgory_Brands_List(string Brandname)
        {

            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@brandname", Brandname } };

                var res = _database.ProductListWithCount(Products_List, parameters);

                return Ok(res);
            }
            catch (Exception ex)
            {
                log.Error("Error--", ex);
                CustomResponse cs = new CustomResponse();
                cs.ResponseID = 0;
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }
        //===================Get Categories Brand List======================//
        [HttpPost]
        [Route("Get_Sub_Catgory_Brands_List")]
        public IHttpActionResult Get_Sub_Catgory_Brands_List(string Brandname, int Catid)
        {

            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@brandname", Brandname }, { "@categoryid", Catid } };

                var res = _database.ProductListWithCount(Cat_List, parameters);

                return Ok(res);
            }
            catch (Exception ex)
            {
                log.Error("Error--", ex);
                CustomResponse cs = new CustomResponse();
                cs.ResponseID = 0;
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        //===================Get Sub Catgory Products List======================//
        [HttpGet]
        [Route("Get_Sub_Catgory_Products_List")]
        public IHttpActionResult Get_Sub_Catgory_Products_List(string Brandname, int Catid, int Page_Index, int Page_Size)
        {

            try
            {

                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@brandname", Brandname }, { "@categoryid", Catid }, { "@Page_Index", Page_Index }, { "@Page_Size", Page_Size } };

                var res = _database.ProductListWithCount(All_Brands_Sub_Category_And_Product_List, parameters);

                return Ok(res);
            }
            catch (Exception ex)
            {
                log.Error("Error--", ex);
                CustomResponse cs = new CustomResponse();
                cs.ResponseID = 0;
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }
        //===================Get Pricing Product List======================//
        [HttpPost]
        [Route("GetPricingProductList")]
        public IHttpActionResult GetPricingProductList(VMModelsForProduct Cat)
        {

            try
            {

                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Filter_Json", Cat.FilterJson },
                                                                                           { "@Category_ID", Cat.CategoryID },
                                                                                           { "@ProductName", Cat.ProductName },
                                                                                           { "@pagesize", Cat.Pagesize },
                                                                                           { "@pageindex", Cat.PageIndex }  };
                var pricing_Product_Get_List = _database.ProductListWithCount(pricing_Product_GetList_New, parameters);
                return Ok(pricing_Product_Get_List);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Get_Category_Attributes---Error---------", ex);
                return Ok(cs);
            }
        }
        // =================================To Get UnGrouped Product List====================================
        [HttpPost]
        [Route("Get_UnGrouped_Product_List")]
        public IHttpActionResult Get_UnGrouped_Product_List(VMModelsForProduct vmodel)
        {
            try
            {

                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Category_ID", vmodel.CategoryID },
                                                                                            { "@AttributeFilter", vmodel.FilterJson },
                                                                                             { "@ProductName",vmodel.ProductName },
                                                                                                { "@PageSize", vmodel.Pagesize },
                                                                                                { "@PageIndex", vmodel.PageIndex } };
                var ProductsList = _database.ProductListWithCount(UnGrouped_Product_List, parameters);
                return Ok(ProductsList);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Get_UnGrouped_Product_List-----------Error---------", ex);
                return Ok(cs);
            }
        }
        // =================================To Get_Group_Filter_Types====================================
        [HttpGet]
        [Route("Get_Group_Filter_Types")]
        public IHttpActionResult Get_Group_Filter_Types(int CategoryID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Category_ID", CategoryID } };
                var res = _database.ProductListWithCount(Group_Filters_Types, parameters);
                return Ok(res);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Get_Group_Filter_Types---Error---------", ex);
                return Ok(cs);
            }
        }
        // =================================To Create Update Product Grouping==================================== 
        [HttpPost]
        [Route("Create_Update_Product_Grouping")]
        public IHttpActionResult Create_Update_Product_Grouping(VMModelsForProduct vmodel)
        {
            try
            {

                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@GroupID", vmodel.CategoryID },
                                                                                            { "@Group_Keys", vmodel.FilterJson },
                                                                                            { "@Product_IDs",vmodel.ProductName }  };
                var ProductsList = _database.QueryValue(Create_Product_Group, parameters);
                return Ok(ProductsList);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Get_UnGrouped_Product_List-----------Error---------", ex);
                return Ok(cs);
            }
        }
        // =================================To Get Product Groups List==================================== 
        [HttpPost]
        [Route("Get_Product_Groups_List")]
        public IHttpActionResult Get_Product_Groups_List(VMModelsForProduct vmodel)
        {
            try
            {

                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Filter_Json", vmodel.FilterJson },
                                                                                            { "@Category_ID", vmodel.CategoryID },
                                                                                            { "@ProductName",vmodel.ProductName },
                                                                                            { "@pagesize", vmodel.Pagesize },
                                                                                            { "@pageindex", vmodel.PageIndex },
                                                                                            { "@GroupID",vmodel.ID } };
                var ProductsList = _database.ProductListWithCount(Grouped_Product_List, parameters);
                return Ok(ProductsList);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Get_UnGrouped_Product_List-----------Error---------", ex);
                return Ok(cs);
            }
        }
        // =================================To GetProductDetailsByID====================================
        [HttpGet]
        [Route("GetProductDetailsByID")]
        public IHttpActionResult GetProductDetailsByID(int ProductID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@productid", ProductID } };
                var productsdetailsview = _database.Query(View_Products_Details, parameters);
                return Ok(productsdetailsview);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("GetProductDetailsByID---Error---------", ex);
                return Ok(cs);
            }
        }
        // =================================To GetProductDetailsByID====================================
        [HttpPost]
        [Route("Update_Grouping_Products")]
        public IHttpActionResult Update_Grouping_Products(int GropuID, string GroupingData)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@GropuID", GropuID }, { "@GroupingData", GroupingData } };
                var res = _database.QueryValue(Update_Grouping_Products_By_GroupID, parameters);
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

        //=============================== UpdateProductsByManual=================================
        [HttpPost]
        [Route("UpdateProductsByManual")]
        public IHttpActionResult UpdateProductsByManual(VMModelsForProduct vm)
        {

            try
            {
                var random = new Random(System.DateTime.Now.Millisecond);
                var randomNumber = random.Next(1, 5000000);
                int userid = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());
                //vm.ProductName = vm.ProductName.Replace("'", "''");
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@User_ID", userid }, { "@RF_Number", randomNumber }, { "@IP", "205.122.23322" }, { "@HtmlContent", vm.ProductName }, { "@Product_ID", vm.ID }, { "@Json_Data", vm.JsonData } };
                var res = _database.QueryValue(Update_Products_By_Manual, parameters);
                return Ok(res);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("UpdateProductsByManual---Error---------", ex);
                return Ok(cs);
            }
        }

        //=============================== ChangeRepurchasableStatus=================================
        [HttpPost]
        [Route("ChangeRepurchasableStatus")]
        public IHttpActionResult ChangeRepurchasableStatus(int ID, int Repurchasable)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@product_id", ID }, { "@repurchasable", Repurchasable } };
                var changed_status = _database.Query(Change_Repurchasable_Status, parameters);
                return Ok(changed_status);
            }
            catch (Exception ex)
            {
                log.Error("ChangeRepurchasableStatus----Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        //=============================== AllowSalesInDirect Status =================================
        [HttpGet]
        [Route("AllowSalesInDirect")]
        public IHttpActionResult AllowSalesInDirect(int ID, int DirectStatus)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ID", ID }, { "@DirectStatus", DirectStatus } };
                var changed_status = _database.Query(AllowDirectSales, parameters);
                return Ok(changed_status);
            }
            catch (Exception ex)
            {
                log.Error("ChangeRepurchasableStatus----Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }
        //=============================== AllowSalesInDirect Status =================================
        [HttpPost]
        [Route("iH_Get_All_Store_Orders_For_Admin")]
        public IHttpActionResult iH_Get_All_Store_Orders_For_Admin(VMModelsForOrderManagement order)
        {
            try
            {
                int User_ID = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());
                if (order.OrderType == "2")
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>() {
                            { "@Order_Number", order.OrderNumber },
                            { "@Order_Date_From", order.OrderDateFrom },
                            { "@Order_Date_To",  order.OrderDateTo },
                            { "@Mobile_Number", order.MobileNumber },
                            { "@UnitID", order.UnitID },
                            { "@Status", order.StatusTwo },
                            { "@Page_index", order.PageIndex },
                            { "@Page_size", order.PageSize },
                       };
                    var Ordermange = _database.ProductListWithCount(StoreOrder, parameters);
                    return Ok(Ordermange);
                }
                else if (order.OrderType == "3")
                {

                    Dictionary<string, object> parameters = new Dictionary<string, object>() {
                            { "@Order_Number", order.OrderNumber },
                            { "@Order_Date_From", order.OrderDateFrom },
                            { "@Order_Date_To",  order.OrderDateTo },
                            { "@Mobile_Number", order.MobileNumber },
                            { "@UnitID", order.UnitID },
                            { "@Status", order.Status },
                            { "@Pageindex", order.PageIndex },
                            { "@Pagesize", order.PageSize },
                       };
                    var Ordermange = _database.ProductListWithCount(BUlkOrder, parameters);
                    return Ok(Ordermange);
                }
                else
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>() {
                            { "@Order_Number", order.OrderNumber },
                            { "@Order_Date_From", order.OrderDateFrom },
                            { "@Order_Date_To",  order.OrderDateTo },
                            { "@Mobile_Number", order.MobileNumber },
                            { "@UnitID", order.UnitID },
                            { "@Status", order.StatusTwo },
                            { "@Pageindex", order.PageIndex },
                            { "@Pagesize", order.PageSize },
                            { "@UserId", User_ID },
                            {"@Order_From",order.OrderType}
                       };
                    var Ordermange = _database.GetMultipleResultsList(DirectOrder, parameters);
                    return Ok(Ordermange);
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
        //=============================== Get Store Cancelled Order =================================
        [HttpPost]
        [Route("Get_Store_Cancelled_Orders")]
        public IHttpActionResult Get_Store_Cancelled_Orders(VMModelsForOrderManagement cancel)
        {
            try
            {

                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                            { "@Order_Number", cancel.OrderNumber },
                            { "@Order_Date_From", cancel.OrderDateFrom },
                            { "@Order_Date_To",  cancel.OrderDateTo },
                            { "@Mobile_Number", cancel.MobileNumber },
                            { "@UnitID", cancel.UnitID },
                            { "@Status", "60" },
                            { "@Page_index", cancel.PageIndex },
                            { "@Page_size", cancel.PageSize },
                       };
                var Ordermange = _database.ProductListWithCount(StoreOrder, parameters);


                /*Dictionary<string, object> parameters = new Dictionary<string, object>() {
                            { "@Order_Number", cancel.OrderNumber },
                            { "@Order_Date_From", cancel.OrderDateFrom },
                            { "@Order_Date_To",  cancel.OrderDateTo },
                            { "@Mobile_Number", cancel.MobileNumber },
                            { "@UnitID", cancel.UnitID },
                            { "@Page_Index", cancel.PageIndex },
                            { "@Page_Size", cancel.PageSize },
                       };
                var Ordermange = _database.ProductListWithCount(CancelOrder, parameters);*/
                return Ok(Ordermange);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        //=================================To Get Buyer Price Requests====================================================
        [HttpPost]
        [Route("Get_Buyer_Price_Requests")]
        public IHttpActionResult Get_Buyer_Price_Requests(VMModelsForOrderManagement VM)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { {"@cutomer_name",VM.Name},
                                                                                            { "@mobile_number", VM.MobileNumber},
                                                                                            { "@category_name",VM.CategoryName},
                                                                                            {"@sku",VM.SKU},{"@unit_name",VM.StoreName },
                                                                                            { "@from_date",VM.OrderDateFrom },
                                                                                            { "@to_date",VM.OrderDateTo },{ "@status", VM.Status },
                                                                                            { "@Page_Size", VM.PageSize },
                                                                                            { "@Page_Index", VM.PageIndex } };
                var Result = _database.ProductListWithCountnew(Get_PriceRequest_Data, parameters);
                return Ok(Result);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }
        // =================================To Get Customer Special Requests====================================================
        [HttpPost]
        [Route("Get_Special_Request_Data")]
        public IHttpActionResult Get_Special_Request_Data(VMModelsForOrderManagement VM)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { {"@customer_name",VM.Name},{ "@mobile_number", VM.MobileNumber}, {"@product_name",VM.ProductName},
                   {"@unit_name",VM.StoreName },{"@requested_date",VM.OrderDateFrom },
                   { "@status", VM.Status }, { "@Page_Size", VM.PageSize }, { "@Page_Index", VM.PageIndex } };
                if (VM.Status != 0)
                {
                    Get_SpecialRequest_Data = "iAdmin_Get_Customer_Special_Requests_Chat";
                }

                var Result = _database.ProductListWithCount(Get_SpecialRequest_Data, parameters);
                return Ok(Result);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }
        //=============================== ProductStatusChange=================================
        [HttpGet]
        [Route("GetOrderedProducts")]
        public IHttpActionResult GetOrderedProducts(int orderid)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@order_id", orderid } };
                //var categoryGetList = _database.ProductListWithCount(Category_GetListnew, parameters);
                var oCatList = _database.GetMultipleResultsListAll(Category_GetListnew, parameters);
                if (oCatList != null && oCatList.Addressset != null && oCatList.Addressset.Count() > 0)
                {
                    foreach (var item in oCatList.Addressset)
                    {
                        if (Convert.ToInt32(item["Package_ID"]) != 0)
                        {
                            var iPDID = Convert.ToInt32(item["Ordered_Product_Details_ID"]);
                            var packlist = (from pc in dbContext.UserBasket_Products
                                            join ih in dbContext.iHub_Products on pc.Product_ID equals ih.iHub_Product_ID
                                            where pc.Order_Details_ID == iPDID
                                            select new
                                            {
                                                pc.Product_ID,
                                                pc.Quantity,
                                                pc.UserBasketPakage_ID,
                                                ih.Product_Name,
                                                ih.Image_Code,
                                                ih.Product_Series
                                            }).ToList();
                            var json = new JavaScriptSerializer().Serialize(packlist);
                            item.Add("packagelist", json);
                        }
                        if (oCatList.Resultset != null)
                        {
                            var iOrderFrom = oCatList.Resultset.FirstOrDefault().Where(m => m.Key == "Order_From").Select(m => m.Value).FirstOrDefault();
                            //string Query = "SELECT * FROM Domains_Master_Configuration WHERE DomainID=" + iOrderFrom;
                            //Dictionary<string, object> Param = new Dictionary<string, object>() { };
                            //var res = _database.Query(Query, Param);
                            //if (res != null)
                            //{
                            //    oCatList.Addressset3 = res;
                            //}
                        }
                    }
                }
                return Ok(oCatList);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        //=============================== ProductStatusChange=================================
        [HttpPost]
        [Route("ProductStatusChange")]
        public IHttpActionResult ProductStatusChange(int ID, int Status)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@productid", ID }, { "@status", Status } };
                var res = _database.Query(Product_Status_Changed, parameters);
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
        //=============================== ProductStatusChange=================================
        [HttpPost]
        [Route("GetProductList")]
        public IHttpActionResult GetProductList(VMModelsForProduct vmodel)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Filter_Json", vmodel.FilterJson }
                   , { "@Category_ID", vmodel.CategoryID }, { "@ProductName",vmodel.ProductName }, { "@Pagesize", vmodel.Pagesize }
                   , { "@PageIndex", vmodel.PageIndex }};
                var ProductsList = _database.ProductListWithCount(Get_Category_Products, parameters);
                return Ok(ProductsList);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("GetProductList-----------Error---------", ex);
                return Ok(cs);
            }
        }
        //=============================== Get_Produts_By_IDs=================================
        [HttpGet]
        [Route("Get_Produts_By_IDs")]
        public IHttpActionResult Get_Produts_By_IDs(string ProductID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@productids", ProductID } };
                var res = _database.Query(Products_By_IDs, parameters);
                return Ok(res);
            }
            catch (Exception ex)
            {
                log.Error("Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                return Ok(cs);
            }
        }
        //=============================== Assign_Accessories_Products=================================
        [HttpPost]
        [Route("Assign_Accessories_Products")]
        public IHttpActionResult Assign_Accessories_Products(string ProductID, string AccessoriesID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@productids", ProductID }, { "@accessoriesids", AccessoriesID } };
                var res = _database.QueryValue(Accessories_To_Productss, parameters);
                return Ok(res);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Assign_Accessories_Products--Error---------", ex);
                return Ok(cs);
            }
        }


        //====================>>>>>>>>>>>>>> To Add & Update Buyer address <<<<<<<<<<<============================
        [HttpPost]
        [Route("Add_Update_Buyer_Address")]
        public IHttpActionResult Add_Update_Buyer_Address(VMModelsForProduct buyerModel)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@user_Id", buyerModel.user_Id}, { "@Address_PK_ID", buyerModel.ID},
                {"@Address_Line_1", buyerModel. Address_Line_1},  { "@Address_Line_2", buyerModel.Address_Line_2 }, { "@Village_Area_ID", buyerModel.Village_Area_ID },{ "@Contact_Number", buyerModel.MobileNumber },{ "@ContactName", buyerModel.FullName }};
                var save_address_result = _database.QueryValue(Add_Update_Address, parameters);
                return Ok(save_address_result);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }
        //======================>>>>>>>>>>> To Get Location based on Pincode <<<<<<<<<<=============================
        [HttpGet]
        [Route("Get_BuyerAddress_By_ID")]
        public IHttpActionResult Get_BuyerAddress_By_ID(int Userid, int UnitID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@user_id", Userid },
                                                                                            { "@unit_id", UnitID },
                                                                                            { "@order_from", 2 } };
                var locations = _database.ProductListWithCount(Get_Add_Details, parameters);
                return Ok(locations);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }
        //=============================== Remove_Accessories=================================
        [HttpPost]
        [Route("Remove_Accessories")]
        public IHttpActionResult Remove_Accessories(VMModelsForProduct vmodel)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@productid", vmodel.ProductId },
                                                                                            { "@accessoriesid", vmodel.AccessoriesId } };
                var res = _database.QueryValue(Remove_Accessories_Products, parameters);
                return Ok(res);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Error---------", ex);
                return Ok(cs);
            }

        }
        //==========================================PRODUCTS SKU SEARCH===================================
        [HttpPost]
        [Route("GET_All_Prds_List_BY_IDS")]
        public IHttpActionResult GET_All_Prds_List_BY_IDS(VMModelsForProduct gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ProductIDs", gt.productids } };
                var res = _database.ProductListWithCount(Get_Products_List_By_Skus, parameters);
                return Ok(res);
            }
            catch (Exception ex)
            {
                log.Error("Error--", ex);
                CustomResponse cs = new CustomResponse();
                cs.ResponseID = 0;
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        //=============================== Update_HSN_Codes_InBulk=================================
        [HttpPost]
        [Route("Update_HSN_Codes_InBulk")]
        public IHttpActionResult Update_HSN_Codes_InBulk(VMModelsForProduct gt)
        {
            try
            {

                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Product_IDs", gt.productids },
                                                                                                    { "@HSNCode", gt.HSN_Code } };
                var res = _database.QueryValue(Products_Hsn_Code_Update, parameters);
                return Ok(res);
            }
            catch (Exception ex)
            {
                log.Error("Error--", ex);
                CustomResponse cs = new CustomResponse();
                cs.ResponseID = 0;
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }
        //====================>>>>>> Get Buyer Details Using Mobile Number <<<<<====================
        [HttpPost]
        [Route("Get_Buyer_Details_By_Mobile_Number")]
        public IHttpActionResult Get_Buyer_Details_By_Mobile_Number(string MobileNumber)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@MobileNumber", MobileNumber } };
                var result = _database.ProductListWithCount(Get_Details, parameters);
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

        //===================>>>>>>>> Generate Bulk Order <<<<<<<<<=====================
        [HttpPost]
        [Route("Generate_Bulk_Order")]
        public IHttpActionResult Generate_Bulk_Order(BulkOrderDetails GF)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@BuyerID", GF.BuyerID },
                                                                                            { "@UnitID", GF.UnitID },
                                                                                            { "@AddressID", GF.AddressID },
                                                                                            { "@PaymentType", GF.PaymentType },
                                                                                            { "@Products_Data", GF.Products_Data },
                                                                                            { "@BookingAmount", GF.BookingAmount },
                                                                                            { "@TotalAmount", GF.TotalAmount },
                                                                                            { "@Discount", GF.DiscountAmnt }};
                var res = _database.QueryValue(Generate_Bulk_Order_For_Selected_Unit, parameters);
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
        //============================================Get Ordered For Cancel by super Admin=====================================
        [HttpPost]
        [Route("iH_Get_Orders_For_Admin_ForCancel")]
        public IHttpActionResult iH_Get_Orders_For_Admin_ForCancel(VMModelsForOrderManagement GF)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Order_Number", GF.OrderNumber },
                                                                                            { "@Order_Date_From", GF.OrderDateFrom },
                                                                                            { "@Order_Date_To", GF.OrderDateTo },
                                                                                            { "@Mobile_Number", GF.MobileNumber },
                                                                                            { "@UnitID", GF.UnitID },
                                                                                            { "@Status", GF.Status },
                                                                                            { "@Page_Index", GF.PageIndex },
                                                                                            { "@Page_Size",GF.PageSize } };
                var res = _database.ProductListWithCount(Get_orders_Paging, parameters);
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
        //============================================Get Ordered For Cancel by super Admin=====================================
        [HttpPost]
        [Route("Get_Store_Vendor_Orders")]
        public IHttpActionResult Get_Store_Vendor_Orders(VMModelsForOrderManagement GF)
        {
            try
            {

                string Get_Ordered_Vendor = "";
                if (GF.OrderType == "1")
                {
                    Get_Ordered_Vendor = "[iAdmin_Get_All_Direct_Orders_For_Vendor]";
                    Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Order_Number", GF.OrderNumber },
                                                                                                { "@Order_Date_From", GF.OrderDateFrom },
                                                                                                { "@Order_Date_To", GF.OrderDateTo },
                                                                                                { "@Mobile_Number", GF.MobileNumber },
                                                                                                { "@UnitID", GF.UnitID },
                                                                                                { "@Page_Index", GF.PageIndex },
                                                                                                { "@Page_Size",GF.PageSize } };
                    var Result = _database.ProductListWithCount(Get_Ordered_Vendor, parameters);
                    return Ok(Result);
                }
                else
                {
                    Get_Ordered_Vendor = "[iAdmin_Get_All_Vendor_Orders_Admin]";
                    Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Order_Number", GF.OrderNumber },
                                                                                                { "@Order_Date_From", GF.OrderDateFrom },
                                                                                                { "@Order_Date_To", GF.OrderDateTo },
                                                                                                { "@Mobile_Number", GF.MobileNumber },
                                                                                                { "@UnitID", GF.UnitID },
                                                                                                { "@Page_Index", GF.PageIndex },
                                                                                                { "@OrderFrom", GF.OrderType },
                                                                                                { "@Page_Size",GF.PageSize } };
                    var Result = _database.ProductListWithCount(Get_Ordered_Vendor, parameters);
                    return Ok(Result);
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
        //======================================To Change Special Request Status
        [HttpPost]
        [Route("Change_Special_Request_Status")]
        public IHttpActionResult Change_Special_Request_Status(VMModelsForOrderManagement sp)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@customer_id", sp.CustomerID }, { "@status", sp.Status } };
                var changed_status = _database.QueryValue(Change_SpecialRequest_Status, parameters);
                var nfgyyf = Common.sendmessage(sp.MobileNumber, sp.Message, "TODO");
                return Ok(changed_status);
            }
            catch (Exception ex)
            {
                log.Error("Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }
        //======================================To Change Price Request Status
        [HttpPost]
        [Route("Change_Customer_Request_Status")]
        public IHttpActionResult Change_Customer_Request_Status(VMModelsForOrderManagement Pricing)
        {
            try
            {
                String Message = "";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@customer_id", Pricing.CustomerID },
                                                                                            { "@status", Pricing.Status } };
                var changed_status = _database.QueryValue(Change_CustomerRequest_Status, parameters);
                if (Convert.ToInt32(changed_status) != 0)
                {
                    if (Pricing.Status == 20)
                    {
                        Message = "Your Price Request for " + Pricing.ProductName + " is Under Process";
                    }
                    else if (Pricing.Status == 30)
                    {
                        Message = "Your Price Request for " + Pricing.ProductName + " is Confirmed";
                    }
                    else
                    {
                        Message = "Your Price Request for " + Pricing.ProductName + " is Cannot be fulfilled";
                    }
                }

                var Messsage = Common.sendmessage(Pricing.MobileNumber, Message, "TODO");
                return Ok(changed_status);
            }
            catch (Exception ex)
            {
                log.Error("Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }
        //======================================To Change Price Request Status
        [HttpPost]
        [Route("GetAllRaisedBulkOrdersByUnitID")]
        public IHttpActionResult GetRaisedBulkOrdersService(VMModelsForOrderManagement Sc)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                   { "@Order_Number", Sc.OrderNumber}
                   ,{ "@Order_Date_From", Sc.OrderDateFrom}
                   ,{ "@Order_Date_To", Sc.OrderDateTo}
                   ,{ "@Mobile_Number", Sc.MobileNumber}
                   ,{ "@UnitName", Sc.StoreName}
                   ,{ "@Status", Sc.Status}
                   ,{ "@Page_Index", Sc.PageIndex}
                   , { "@Page_Size", Sc.PageSize }};
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

        //===============>>>>>>>>To Get Buyer EMI request in Grid<<<<<<<<<<<<<<<<<<============
        [HttpPost]
        [Route("Get_Buyer_EMI_Settings")]
        public IHttpActionResult Get_Buyer_EMI_Settings(EMIRequestList GF)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {{ "@MobileNumber", GF.MobileNumber },
                                                                                            { "@UnitName", GF.Name },
                                                                                            { "@EMI_Min_Limit", GF.EMI_Min_Limit },
                                                                                            { "@EMI_Max_Limit", GF.EMI_Max_Limit },
                                                                                            { "@No_Of_EMIs", GF.No_Of_EMIs },
                                                                                            { "@Status", GF.Status },
                                                                                            { "@Description", GF.CounterDescription },
                                                                                            { "@Created_Date", GF.CreatedDate },
                                                                                            { "@Approved_Time", GF.ApprovedTimeLimit },
                                                                                            { "@ApprovedCode", GF.ApprovedCode },
                                                                                            { "@BuyerEMIId", GF.BuyerEMIId },
                                                                                            { "@pagesize", GF.PageSize },
                                                                                            { "@pageindex", GF.PageIndex }};
                var res = _database.ProductListWithCount(EMI_Request_Details, parameters);
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

        //========================>>>>>>>>>>>>>>>To Save Buyer EMI request<<<<<<<<<<<<===========================
        [HttpPost]
        [Route("Buyer_EMI_Request")]
        public IHttpActionResult Buyer_EMI_Request(EMIRequestList SaveEMIRequest)
        {
            try
            {
                var Approved_code = 0;
                if (Convert.ToInt16(SaveEMIRequest.Status2) == 20)
                {
                    Approved_code = Convert.ToInt32(Common.GenerateOTP());
                }
                Dictionary<string, object> parameters = new Dictionary<string, object>() { {"@BuyerEMIConfigID", SaveEMIRequest.EMIConfigID},
                                                                                            {"@BuyerID", SaveEMIRequest.buyerid},
                                                                                            { "@MobileNumber", SaveEMIRequest.MobileNumber},
                                                                                            { "@EMI_Min_Limit", SaveEMIRequest.EMI_Min_Limit },
                                                                                            { "@EMI_Max_Limit", SaveEMIRequest.EMI_Max_Limit },
                                                                                            { "@No_Of_EMIs", SaveEMIRequest.No_Of_EMIs },
                                                                                            { "@Status", SaveEMIRequest.Status2 },
                                                                                            { "@UnitId", SaveEMIRequest.UnitId },
                                                                                            { "@UserId", 0 },
                                                                                            { "@Description", SaveEMIRequest.CounterDescription },
                                                                                            { "@ApprovedTimeLimit", SaveEMIRequest.ApprovedTimeLimit },
                                                                                            { "@ApprovedCode",Approved_code} };
                var Result = _database.QueryValue(Buyer_EMI, parameters);
                if (Convert.ToInt16(SaveEMIRequest.Status2) == 20)
                {
                    var send = Common.sendmessage(SaveEMIRequest.MobileNumber, "Dear iHub User, Here is your EMI Approval Code:" + Approved_code + ". Please make sure to submit it at the store while placing the order. OTP expires in " + SaveEMIRequest.ApprovedTimeLimit + "hrs.", "TODO");
                }
                return Ok(Result);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }

        //============================================Get Store WareHouse Orders=====================================
        [HttpPost]
        [Route("Get_Store_WareHouse_Orders")]
        public IHttpActionResult Get_Store_WareHouse_Orders(WareStoreOrderManagement GF)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Order_Number", GF.OrderNumber },
                                                                                            { "@Order_Date_From", GF.OrderDateFrom },
                                                                                            { "@Order_Date_To", GF.OrderDateTo },
                                                                                            { "@Mobile_Number", GF.MobileNumber },
                                                                                            { "@UnitID", GF.UnitID },
                                                                                            { "@Status", GF.Status },
                                                                                            { "@Page_Index", GF.PageIndex },
                                                                                            { "@Page_Size",GF.PageSize } };
                var res = _database.ProductListWithCount(Get_Ordered_WareHouse, parameters);
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

        //============================================Get Store WareHouse Orders=====================================
        [HttpPost]
        [Route("UploadLocationsWithPincodes")]
        public IHttpActionResult UploadLocationsWithPincodes()
        {

            try
            {
                HttpPostedFile file = HttpContext.Current.Request.Files[0];
                var pathToExcel = file.FileName;
                var Dictionary = new List<string>();
                string str = "";
                IExcelDataReader reader = null;
                List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();
                Dictionary<string, string> product;
                string xlfilename = file.FileName;
                var random = new Random(System.DateTime.Now.Millisecond);
                var randomNumber = random.Next(1, 5000000);

                using (var stream = file.InputStream)
                {

                    if (file.FileName.EndsWith(".xls"))
                    {
                        reader = ExcelReaderFactory.CreateBinaryReader(stream); //ExcelReaderFactory thowing an error but not an issue
                    }
                    else if (file.FileName.EndsWith(".xlsx"))
                    {
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }
                    else
                    {
                        CustomResponse cr = new CustomResponse();
                        cr.Response = "Invalid file format";
                        cr.ResponseID = -1;
                        log.Info("Invalid file format");
                        return Ok(cr);
                    }

                    DataSet result = reader.AsDataSet();
                    reader.Close();
                    DataTable data = result.Tables[0];
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    var excelColdata = new List<string>();
                    List<int> ColumnIndexes = new List<int>();
                    List<List<string>> sListOfRecords = new List<List<string>>();
                    int i = 1;
                    foreach (var column in data.Rows[0].ItemArray.Where(m => m is string))
                    {
                        string ColumnName = column.ToString();
                        ColumnName.Replace("'", " ");

                        Dictionary.Add(ColumnName);
                    }
                    foreach (DataRow row in data.Rows)
                    {
                        var excelRowdata = new List<string>();
                        if (i == 1)
                        {
                            int j = 1;
                            foreach (var item in row.ItemArray)
                            {
                                if (Dictionary.Contains(item.ToString()))
                                {
                                    excelRowdata.Add(item.ToString());
                                    ColumnIndexes.Add(j);
                                }
                                j++;
                            }
                            if (excelRowdata.Where(m => !string.IsNullOrWhiteSpace(m)).Count() > 0)
                            {
                                sListOfRecords.Add(excelRowdata);
                            }
                        }
                        else
                        {
                            // int productid = 0;
                            string Village_Name = "";
                            string Mandal_Name = "";
                            string District_Name = "";
                            string State_Name = "";
                            int Pin_code = 0;
                            int j = 1;
                            product = new Dictionary<string, string>();
                            if (Convert.ToString(row.ItemArray[0]) == string.Empty) { break; }
                            foreach (var item in row.ItemArray)
                            {
                                if (ColumnIndexes.Contains(j))
                                {

                                    if (sListOfRecords[0][j - 1].TrimStart().TrimEnd() == "VillageName")
                                    {
                                        Village_Name = item.ToString().TrimStart().TrimEnd();
                                    }

                                    if (sListOfRecords[0][j - 1].TrimStart().TrimEnd() == "MandalName")
                                    {
                                        Mandal_Name = item.ToString().TrimStart().TrimEnd();
                                    }
                                    if (sListOfRecords[0][j - 1].TrimStart().TrimEnd() == "DistrictName")
                                    {
                                        District_Name = item.ToString().TrimStart().TrimEnd();
                                    }
                                    if (sListOfRecords[0][j - 1].TrimStart().TrimEnd() == "StateName")
                                    {
                                        State_Name = item.ToString().TrimStart().TrimEnd();
                                    }
                                    if (sListOfRecords[0][j - 1].TrimStart().TrimEnd() == "PinCode")
                                    {
                                        Pin_code = Convert.ToInt32((item.ToString().TrimStart().TrimEnd()));
                                    }
                                }
                                j++;
                            }
                            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Village_Name", Village_Name }, { "@Mandal_Name", Mandal_Name }, { "@District_Name", District_Name }, { "@State_Name", State_Name }, { "@Pin_code", Pin_code } };
                            var response = _database.QueryValue(Add_Master_Location_Data, parameters);
                        }
                        i++;
                    }
                    return Ok("Add Successfully");
                }
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }

        // =================================store side Get buyer Details by Mobile Number====================================================
        [HttpPost]
        [Route("GetVerificationOTP")]
        public IHttpActionResult GetVerificationOTP()
        {
            try
            {
                string Purpose = "Order Cancel By Admin";
                int userID = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());
                string commandText = "SELECT \"PhoneNumber\" FROM \"AspNetUsers\" WHERE \"Id\" = @id";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@id", userID } };
                var PhoneNumber = _database.GetStrValue(commandText, parameters);
                string ReturnOTP = Common.sendAdminOTP(userID, PhoneNumber, Purpose);
                return Ok(ReturnOTP);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }

        // =================================store side Get buyer Details by Mobile Number====================================================
        [HttpPost]
        [Route("Add_Update_Wallet_Amount")]
        public IHttpActionResult Add_Update_Wallet_Amount(VMWalletdetails vmodel)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@WalletAmount", vmodel.WalletAmount },
                                                                                            { "@Transaction_Type", vmodel.TransactionType },
                                                                                            { "@BuyerID", vmodel.BuyerID },
                                                                                            { "@MobileNumber", vmodel.MobileNumber },
                                                                                            { "@Remarks",vmodel.Remarks } };
                var res = _database.QueryValue(To_Add_Update_Wallet_Amount, parameters);
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
        // =================================store side Get buyer Details by Mobile Number====================================================
        [HttpPost]
        [Route("UploadBuyerslist")]
        public IHttpActionResult UploadBuyerslist()
        {
            try
            {
                CustomResponse cs = new CustomResponse();
                List<string> resss = new List<string>();
                var res1 = "";
                var categorydata = HttpContext.Current.Request.Form[0];
                iH_Categories category = JsonConvert.DeserializeObject<iH_Categories>(categorydata);
                HttpPostedFile file = HttpContext.Current.Request.Files[0];
                var pathToExcel = file.FileName;
                var Dictionary = new List<string>();
                string str = "";
                IExcelDataReader reader = null;
                List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();
                Dictionary<string, string> product;
                string xlfilename = file.FileName;
                var random = new Random(System.DateTime.Now.Millisecond);
                var randomNumber = random.Next(1, 5000000);
                using (var stream = file.InputStream)
                {

                    if (file.FileName.EndsWith(".xls"))
                    {
                        reader = ExcelReaderFactory.CreateBinaryReader(stream); //ExcelReaderFactory thowing an error but not an issue
                    }
                    else if (file.FileName.EndsWith(".xlsx"))
                    {
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }
                    else
                    {

                        return Ok("Invalid file format");
                    }
                    DataSet result = reader.AsDataSet();
                    reader.Close();
                    DataTable data = result.Tables[0];
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    var excelColdata = new List<string>();
                    List<int> ColumnIndexes = new List<int>();
                    List<List<string>> sListOfRecords = new List<List<string>>();
                    int i = 1;
                    bool ColomnrequiredMobileNumber = false;
                    bool ColomnrequiredAmount = false;
                    bool ColomnrequiredDescription = false;

                    foreach (var column in data.Rows[0].ItemArray.Where(m => m is string))
                    {
                        if (column.ToString().Trim() == "MobileNumber")
                        {
                            ColomnrequiredMobileNumber = true;
                        }
                        if (column.ToString().Trim() == "Amount")
                        {
                            ColomnrequiredAmount = true;
                        }
                        if (column.ToString().Trim() == "Description")
                        {
                            ColomnrequiredDescription = true;
                        }
                        string ColumnName = column.ToString();
                        ColumnName.Replace("'", " ");
                        Dictionary.Add(ColumnName);

                    }

                    if (ColomnrequiredMobileNumber == false)
                    {
                        return Ok("Invalid MobileNumber Coloumn Name");
                    }
                    if (ColomnrequiredAmount == false)
                    {
                        return Ok("Invalid Amount Coloumn Name");
                    }
                    if (ColomnrequiredDescription == false)
                    {
                        return Ok("Invalid Description Coloumn Name");
                    }

                    foreach (DataRow row in data.Rows)
                    {
                        bool flag = false;
                        var excelRowdata = new List<string>();
                        if (i == 1)
                        {
                            int j = 1;
                            foreach (var item in row.ItemArray)
                            {
                                if (Dictionary.Contains(item.ToString()))
                                {
                                    excelRowdata.Add(item.ToString());
                                    ColumnIndexes.Add(j);

                                }
                                j++;
                            }
                            if (excelRowdata.Where(m => !string.IsNullOrWhiteSpace(m)).Count() > 0)
                            {
                                sListOfRecords.Add(excelRowdata);
                            }
                        }
                        else
                        {
                            int j = 1;
                            product = new Dictionary<string, string>();
                            if (Convert.ToString(row.ItemArray[0]) == string.Empty) { break; }
                            foreach (var item in row.ItemArray)
                            {

                                if (ColumnIndexes.Contains(j))
                                {
                                    if (sListOfRecords[0][j - 1] == "")
                                    {

                                    }

                                    excelRowdata.Add(item.ToString());
                                    string itemname = item.ToString();
                                    if (sListOfRecords[0][j - 1].ToString().Trim() == "MobileNumber")
                                    {

                                        if (itemname == "")
                                        {
                                            cs.Response = "Invalid MobileNumber";
                                            cs.ResponseID = Convert.ToInt32(15);
                                            resss.Clear();
                                            flag = true;
                                            break;
                                        }
                                    }
                                    if (sListOfRecords[0][j - 1].ToString().Trim() == "Amount")
                                    {

                                        if (itemname == "")
                                        {
                                            cs.Response = "Invalid Amount Value";
                                            cs.ResponseID = Convert.ToInt32(16);
                                            resss.Clear();
                                            flag = true;
                                            break;
                                        }
                                    }
                                    if (sListOfRecords[0][j - 1].ToString().Trim() == "Description")
                                    {

                                        if (itemname == "")
                                        {
                                            cs.Response = "Invalid Description Value";
                                            cs.ResponseID = Convert.ToInt32(16);
                                            resss.Clear();
                                            flag = true;
                                            break;
                                        }
                                    }
                                    if (flag) break;
                                    itemname = itemname.Replace("'", "");
                                    itemname = itemname.Replace(@"""", @"\""");
                                    str += '"' + sListOfRecords[0][j - 1].TrimStart().TrimEnd() + '"' + ":" + '"' + itemname.ToString().TrimStart().TrimEnd() + '"' + ",";
                                    product.Add(sListOfRecords[0][j - 1].TrimStart().TrimEnd(), itemname.ToString().TrimStart().TrimEnd());
                                }
                                if (flag) break;
                                j++;
                            }
                            if (flag) break;
                            res1 = serializer.Serialize(product);
                            resss.Add(res1);
                            if (excelRowdata.Where(m => !string.IsNullOrWhiteSpace(m)).Count() > 0)
                            {
                                sListOfRecords.Add(excelRowdata);
                            }

                        }
                        i++;

                    }
                    if (resss.Count != 0)
                    {
                        foreach (var resultss in resss)
                        {
                            VMWalletdetails a = JsonConvert.DeserializeObject<VMWalletdetails>(resultss);
                            string commandTextnew = "Select \"Buyers_ID\" from \"iD_Buyers\" where \"Mobile_Number\" = " + "'" + a.MobileNumber + "'";
                            Dictionary<string, object> parametersnw = new Dictionary<string, object> { };
                            var buyerId = _database.QueryValueCOUNT(commandTextnew, parametersnw);
                            string commandText = "[iAdmin_Add_Update_Wallet_Amount_EXCEL]";
                            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@WalletAmount", a.Amount }, { "@Transaction_Type", "C" }, { "@BuyerID", buyerId }, { "@MobileNumber", a.MobileNumber }, { "@Remarks", a.Description }, { "@TransactionNumber", randomNumber.ToString() } };
                            var response = _database.Query(commandText, parameters);
                        }
                        string commandText2 = "[iAdmin_Get_wallet_details_by_Reference_Number_EXCEL]";
                        Dictionary<string, object> parameters2 = new Dictionary<string, object>() { { "@TransactionNumber", randomNumber.ToString() } };
                        var response2 = _database.ProductListWithCount(commandText2, parameters2);
                        return Ok(response2);
                    }
                    else
                    {
                        return Ok("Invalid Values Entered");
                    }
                }
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("UploadWalletDetails---Error---------", ex);
                return Ok(cs);
            }
        }

        [HttpPost]
        [Route("UploadNewProductsForEditDetails")]
        public IHttpActionResult UploadNewProductsForEditDetails(int userid)
        {
            try
            {
                CustomResponse cr = new CustomResponse();
                HttpPostedFile file = HttpContext.Current.Request.Files[0];
                var pathToExcel = file.FileName;
                var Dictionary = new List<string>();
                string str = "";
                IExcelDataReader reader = null;
                List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();
                Dictionary<string, string> product;
                string xlfilename = file.FileName;
                var random = new Random(System.DateTime.Now.Millisecond);
                var randomNumber = random.Next(1, 5000000);
                var bIsMRPSelling = true;
                var bIsSellLanding = true;
                string iIndexOut = string.Empty;
                string sResponse = string.Empty;
                using (var stream = file.InputStream)
                {
                    if (file.FileName.EndsWith(".xls"))
                    {
                        reader = ExcelReaderFactory.CreateBinaryReader(stream); //ExcelReaderFactory thowing an error but not an issue
                    }
                    else if (file.FileName.EndsWith(".xlsx"))
                    {
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }
                    else
                    {
                        //CustomResponse cr = new CustomResponse();
                        cr.Response = "Invalid file format";
                        cr.ResponseID = -1;
                        log.Info("Invalid file format");
                        return Ok(cr);
                    }

                    DataSet result = reader.AsDataSet();
                    reader.Close();
                    DataTable data = result.Tables[0];

                    //Code for Validating the MRP, SellingPrice and LandingPrice From Uploaded Excel Sheet
                    var oData = data.Rows[0];
                    var iMrpIndex = 0;
                    var iSPIndex = 0;
                    var iLPIndex = 0;
                    var results = oData.Table.Select().Select(dr => dr.ItemArray.Select(x => x.ToString()).ToArray()).ToList();
                    var oNewTotal = results.ToArray().Skip(1).ToList(); //Removing the Header Index i.e, 0 Index.
                    var oTotalIndex = results[0].ToList();
                    var bShowMRP = false;
                    var bShowSelling = false;
                    int q = 0;
                    foreach (var SingleIn in oTotalIndex)
                    {
                        if (SingleIn.ToLower() == "mrp")
                        {
                            iMrpIndex = q;
                            bShowMRP = true;
                        }
                        else if (SingleIn.ToLower() == "sellingprice")
                        {
                            iSPIndex = q;
                            bShowSelling = true;
                        }
                        else if (SingleIn.ToLower() == "landingpriceincgst")
                        {
                            iLPIndex = q;
                        }
                        q++;
                    }
                    var oMRPList = new List<string>();
                    var oSellingList = new List<string>();
                    var oLandingList = new List<string>();
                    foreach (var list in oNewTotal)
                    {
                        if (iMrpIndex != 0 && iSPIndex != 0)
                        {
                            oMRPList.Add(list[iMrpIndex]);
                            oSellingList.Add(list[iSPIndex]);
                        }
                        if (iLPIndex != 0)
                        {
                            oLandingList.Add(list[iLPIndex]);
                        }
                    }
                    int u = 0;
                    foreach (var isd in oMRPList)
                    {
                        decimal a = 0;
                        decimal b = 0;
                        if (isd != "")
                        {
                            a = Convert.ToDecimal(isd);
                            b = Convert.ToDecimal(oSellingList[u]);
                        }
                        if (a == b || a > b)
                        {
                            if (bIsMRPSelling != false)
                            {
                                bIsMRPSelling = true;
                            }
                        }
                        else
                        {
                            sResponse = "You have Entered Selling Price greater than MRP in the following Rows at - ";
                            int y = 2;
                            if (!string.IsNullOrEmpty(iIndexOut))
                            {
                                y = y + u;
                                iIndexOut = iIndexOut + ", " + y;
                            }
                            else
                            {
                                iIndexOut = y.ToString();
                            }
                            bIsMRPSelling = false;
                        }
                        u++;
                    }
                    if (bShowMRP == true && bShowSelling == true)
                    {
                        if (bIsMRPSelling == true)
                        {
                            int p = 0;
                            foreach (var oLL in oLandingList)
                            {
                                decimal a = 0;
                                decimal b = 0;
                                decimal c = 0;
                                if (oLL != " ")
                                {
                                    a = Convert.ToDecimal(oLL);
                                    if (oMRPList.Count() > 0 && oSellingList.Count() > 0)
                                    {
                                        b = Convert.ToDecimal(oMRPList[p]);
                                        c = Convert.ToDecimal(oSellingList[p]);
                                    }
                                }
                                if ((a == b || a < b) && (a == c || a < c))
                                {
                                    if (bIsSellLanding != false)
                                    {
                                        bIsSellLanding = true;
                                    }
                                }
                                else
                                {
                                    sResponse = "You have Entered Landing Price Less than MRP or Selling Price in the following Rows at - ";
                                    int y = 2;
                                    if (!string.IsNullOrEmpty(iIndexOut))
                                    {
                                        y = y + u;
                                        iIndexOut = iIndexOut + ", " + y;
                                    }
                                    else
                                    {
                                        iIndexOut = y.ToString();
                                    }
                                    bIsSellLanding = false;
                                }
                                p++;
                            }
                        }
                    }
                    if (bIsMRPSelling == true && bIsSellLanding == true)
                    {
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        var excelColdata = new List<string>();
                        List<int> ColumnIndexes = new List<int>();
                        List<List<string>> sListOfRecords = new List<List<string>>();
                        int i = 1;
                        foreach (var column in data.Rows[0].ItemArray.Where(m => m is string))
                        {
                            string ColumnName = column.ToString();
                            ColumnName.Replace("'", " ");
                            Dictionary.Add(ColumnName);
                        }
                        foreach (DataRow row in data.Rows)
                        {
                            var excelRowdata = new List<string>();
                            if (i == 1)
                            {
                                int j = 1;
                                foreach (var item in row.ItemArray)
                                {
                                    if (Dictionary.Contains(item.ToString()))
                                    {
                                        excelRowdata.Add(item.ToString());
                                        ColumnIndexes.Add(j);
                                    }
                                    j++;
                                }
                                if (excelRowdata.Where(m => !string.IsNullOrWhiteSpace(m)).Count() > 0)
                                {
                                    sListOfRecords.Add(excelRowdata);
                                }
                            }
                            else
                            {
                                int productid = 0;
                                string HtmlContent = "";
                                int j = 1;
                                product = new Dictionary<string, string>();
                                if (Convert.ToString(row.ItemArray[0]) == string.Empty) { break; }
                                foreach (var item in row.ItemArray)
                                {
                                    if (ColumnIndexes.Contains(j))
                                    {
                                        if (sListOfRecords[0][j - 1].TrimStart().TrimEnd() == "SKU")
                                        {
                                            var sku = item.ToString().TrimStart().TrimEnd();
                                            productid = Convert.ToInt32(sku.Split('-')[1]) - 100000;
                                        }
                                        if (sListOfRecords[0][j - 1].TrimStart().TrimEnd() == "HtmlContent")
                                        {
                                            HtmlContent = item.ToString().TrimStart().TrimEnd();
                                        }
                                        excelRowdata.Add(item.ToString());
                                        string itemname = item.ToString();

                                        itemname = itemname.Replace("'", "");
                                        itemname = itemname.Replace(@"""", @"\""");
                                        str += '"' + sListOfRecords[0][j - 1].TrimStart().TrimEnd() + '"' + ":" + '"' + itemname.ToString().TrimStart().TrimEnd() + '"' + ",";
                                        if (sListOfRecords[0][j - 1].ToString() == "Status")
                                        {
                                            if (itemname.ToString() == "Active")
                                            {
                                                itemname = "10";
                                            }
                                            else if (itemname.ToString() == "InActive")
                                            {
                                                itemname = "20";
                                            }
                                            else if (itemname.ToString() == "New")
                                            {
                                                itemname = "0";
                                            }
                                        }
                                        if (sListOfRecords[0][j - 1].ToString() == "Is_Repurchasable")
                                        {
                                            if (itemname.ToString() == "Active")
                                            {
                                                itemname = "1";
                                            }
                                            else if (itemname.ToString() == "InActive")
                                            {
                                                itemname = "0";
                                            }
                                        }
                                        if (sListOfRecords[0][j - 1].ToLower().ToString() == "hasbarcode")
                                        {
                                            if (itemname.ToLower().ToString() == "true" || itemname.ToString() == "1")
                                            {
                                                itemname = "1";
                                            }
                                            else if (itemname.ToLower().ToString() == "false" || itemname.ToString() == "0")
                                            {
                                                itemname = "0";
                                            }
                                        }
                                        product.Add(sListOfRecords[0][j - 1].TrimStart().TrimEnd(), itemname.ToString().TrimStart().TrimEnd());
                                    }
                                    j++;
                                }
                                product.Add("EditSheet_Number", randomNumber.ToString());
                                rows.Add(product);

                                var res1 = serializer.Serialize(product);
                                //int userid = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());
                                //HtmlContent = HtmlContent.Replace("'", "''");
                                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@User_ID", userid }, { "@RF_Number", randomNumber }, { "@IP", "205.122.23322" }, { "@HtmlContent", HtmlContent }, { "@Product_ID", productid }, { "@Json_Data", res1 } };
                                var response = _database.QueryValue(Update_Products_By_Manual, parameters);
                                if (excelRowdata.Where(m => !string.IsNullOrWhiteSpace(m)).Count() > 0)
                                {
                                    sListOfRecords.Add(excelRowdata);
                                }
                            }
                            i++;
                        }
                        var res = serializer.Serialize(rows);
                    }
                }
                if (bIsMRPSelling == true && bIsSellLanding == true)
                {
                    string commandText2 = "[iAdmin_Get_Products_By_Random_Number]";
                    Dictionary<string, object> parameters2 = new Dictionary<string, object>() { { "@randomnumber", randomNumber.ToString() } };
                    var response2 = _database.ProductListWithCount(commandText2, parameters2);
                    return Ok(response2);
                }
                else
                {
                    cr.Response = sResponse + iIndexOut;
                    return Ok(cr.Response);
                }
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }

        //============================================ADMIN DASHBOARD=====================================

        [HttpPost]
        [Route("Get_WareHouse_DashBoard_Service")]
        public IHttpActionResult Get_WareHouse_DashBoard_Service(VMModelsForCategory GF)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@UserID", userID } };
                var res = _database.ProductListWithCount(Admin_Dash_Board_Deatails, parameters);
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
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@OrderID", OrderID } };
                var res = _database.ProductListWithCount(GetOrdered_Products_Direct, parameters);
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
        //=====================  View All Raised Bulk Orders by order id=================
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
        //===================== To Upload MisMtached Stock=================
        [HttpPost]
        [Route("StockAudit")]
        public IHttpActionResult StockAudit()
        {
            try
            {
                CustomResponse cs = new CustomResponse();
                List<string> resss = new List<string>();
                var res1 = "";
                var categorydata = HttpContext.Current.Request.Form[0];
                //iH_Categories category = JsonConvert.DeserializeObject<iH_Categories>(categorydata);
                HttpPostedFile file = HttpContext.Current.Request.Files[0];
                var pathToExcel = file.FileName;
                var Dictionary = new List<string>();
                string str = "";
                IExcelDataReader reader = null;
                List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();
                Dictionary<string, string> product;
                string xlfilename = file.FileName;
                var random = new Random(System.DateTime.Now.Millisecond);
                var randomNumber = random.Next(1, 5000000);
                using (var stream = file.InputStream)
                {

                    if (file.FileName.EndsWith(".xls"))
                    {
                        reader = ExcelReaderFactory.CreateBinaryReader(stream); //ExcelReaderFactory thowing an error but not an issue
                    }
                    else if (file.FileName.EndsWith(".xlsx"))
                    {
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }
                    else
                    {

                        return Ok("Invalid file format");
                    }

                    DataSet result = reader.AsDataSet();
                    reader.Close();
                    DataTable data = result.Tables[0];
                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    var excelColdata = new List<string>();
                    List<int> ColumnIndexes = new List<int>();
                    List<List<string>> sListOfRecords = new List<List<string>>();
                    int i = 1;
                    bool Colomnrequired = false;
                    bool ColomnrequiredStatus = false;
                    bool ColomnrequiredProductName = false;


                    foreach (var column in data.Rows[0].ItemArray.Where(m => m is string))
                    {
                        if (column.ToString().Trim() == "iHub_Unit_ID")
                        {
                            ColomnrequiredProductName = true;
                        }
                        if (column.ToString().Trim() == "SKU")
                        {
                            Colomnrequired = true;
                        }
                        if (column.ToString().Trim() == "Quantity")
                        {
                            ColomnrequiredStatus = true;
                        }


                        string ColumnName = column.ToString();
                        ColumnName.Replace("'", " ");

                        Dictionary.Add(ColumnName);

                    }
                    if (ColomnrequiredProductName == false)
                    {
                        return Ok("iHub_Unit_ID Coloumn required");
                    }
                    if (Colomnrequired == false)
                    {
                        return Ok("SKU Coloumn required");
                    }
                    if (ColomnrequiredStatus == false)
                    {
                        return Ok("Quantity Coloumn required");
                    }


                    foreach (DataRow row in data.Rows)
                    {


                        bool flag = false;
                        var excelRowdata = new List<string>();
                        int productid = 0;
                        int iHub_Unit_ID = 0;
                        if (i == 1)
                        {
                            int j = 1;
                            foreach (var item in row.ItemArray)
                            {
                                if (Dictionary.Contains(item.ToString()))
                                {
                                    excelRowdata.Add(item.ToString());
                                    ColumnIndexes.Add(j);

                                }
                                j++;
                            }
                            if (excelRowdata.Where(m => !string.IsNullOrWhiteSpace(m)).Count() > 0)
                            {
                                sListOfRecords.Add(excelRowdata);
                            }
                        }
                        else
                        {
                            int j = 1;
                            product = new Dictionary<string, string>();
                            if (Convert.ToString(row.ItemArray[0]) == string.Empty) { break; }
                            foreach (var item in row.ItemArray)
                            {

                                if (ColumnIndexes.Contains(j))
                                {
                                    if (sListOfRecords[0][j - 1] == "")
                                    {

                                    }

                                    excelRowdata.Add(item.ToString());
                                    string itemname = item.ToString();


                                    if (sListOfRecords[0][j - 1].ToString().Trim() == "iHub_Unit_ID")
                                    {

                                        if (itemname == "")
                                        {

                                            cs.Response = "Invalid Status Value";
                                            cs.ResponseID = Convert.ToInt32(14);
                                            resss.Clear();
                                            flag = true;
                                            break;
                                        }
                                        else
                                        {
                                            var sku = item.ToString().TrimStart().TrimEnd();
                                            iHub_Unit_ID = Convert.ToInt32(sku);
                                            product.Add("Dc_Unit_ID", iHub_Unit_ID.ToString());
                                        }
                                    }
                                    //if (data.Rows[0][1]=="SKU" )
                                    if (sListOfRecords[0][j - 1] == "SKU")
                                    {
                                        if (itemname == "")
                                        {
                                            cs.Response = "Invalid Status Value";
                                            cs.ResponseID = Convert.ToInt32(14);
                                            resss.Clear();
                                            flag = true;
                                            break;

                                        }
                                        else
                                        {
                                            var sku = item.ToString().TrimStart().TrimEnd();
                                            productid = Convert.ToInt32(sku.Split('-')[1]) - 100000;

                                            product.Add("ID", productid.ToString());
                                        }

                                    }
                                    if (sListOfRecords[0][j - 1].ToString().Trim() == "Quantity")
                                    {

                                        if (itemname == "")
                                        {
                                            cs.Response = "Invalid Status Value";
                                            cs.ResponseID = Convert.ToInt32(14);
                                            resss.Clear();
                                            flag = true;
                                            break;
                                        }
                                        else
                                        {
                                            var sku = item.ToString().TrimStart().TrimEnd();
                                            product.Add("Quantitiy", sku.ToString());
                                        }
                                    }


                                }
                                if (flag) break;
                                j++;
                            }
                            if (flag) break;

                            res1 = serializer.Serialize(product);
                            resss.Add(res1);
                            if (excelRowdata.Where(m => !string.IsNullOrWhiteSpace(m)).Count() > 0)
                            {
                                sListOfRecords.Add(excelRowdata);
                            }

                        }
                        i++;

                        //}
                    }
                    if (resss.Count != 0)
                    {
                        foreach (var resultss in resss)
                        {
                            var result1 = JsonConvert.DeserializeObject<VMPagingResultsPost>(resultss);
                            int userid = 0;
                            string Update_Inventoey_Miss_Macth_Data = "[iAdmin_Update_Miss_Matched_Inventory_Stock]";
                            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ProductID", result1.ID }, { "@Request_Qty", result1.Quantitiy }, { "@LandingPrice", 0 }, { "@UnitID", result1.Dc_Unit_ID }, { "@User_ID", userid }, { "@RF_Number", randomNumber } };

                            var Result = _database.QueryValue(Update_Inventoey_Miss_Macth_Data, parameters);

                            if (Convert.ToInt32(Result) == -1)
                            {
                                return Ok("Please check Execel Sheet");
                            }
                            //else
                            //{
                            //    return Ok(Result);
                            //}
                        }

                    }
                    else
                    {
                        return Ok("Invalid Values Entered");
                    }
                }
                return Ok("a");
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Upload---Error---------", ex);
                return Ok(cs);
            }
        }
        //=================>>>>>>>>>>> Cancel_Ordered_Products_On_Request <<<<<<<<<<<<<<<<========================//
        //[HttpPost]
        //[Route("Cancel_Ordered_Products_On_Request")]
        //public IHttpActionResult Cancel_Ordered_Products_On_Request(VMPagingResultsPost vmodal)
        //{
        //    try
        //    {
        //        Dictionary<string, object> param = new Dictionary<string, object>() { { "@otp", vmodal.Otp },
        //                                                                                { "@notification_ID", vmodal.Notification_ID } };
        //        var validate_result = _database.QueryValue(Validate_OTP, param);
        //        int userID = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());
        //        if (Convert.ToBoolean(validate_result))
        //        {
        //            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@order_id", vmodal.OrderID },
        //                                                                                        { "@product_id", vmodal.Productid } ,
        //                                                                                         { "@UserId", userID }};
        //            var res = _database.QueryValue(To_Cancel_Ordered_Products_On_Request, parameters);
        //            return Ok(res);
        //        }
        //        else
        //        {
        //            return Ok(0);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CustomResponse cs = new CustomResponse();
        //        cs.Response = ex.Message;
        //        log.Error("Error---------", ex);
        //        return Ok(cs);
        //    }
        //}
        
        //============================================Get Ordered Products By OrderID=====================================
        [HttpPost]
        [Route("GetOrderedProducts_Loc")]
        public IHttpActionResult GetOrderedProducts_Loc(VMModelsForOrderManagement GF)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {{ "@order_number", GF.OrderNumber },
                                                                                            { "@orderdatefrom", GF.OrderDateFrom },
                                                                                            { "@orderdateto", GF.OrderDateTo },
                                                                                            { "@unitid", GF.UnitID },
                                                                                            { "@villageid", GF.ParentID },
                                                                                            { "@Page_Size",GF.PageSize },
                                                                                            { "@Page_Index", GF.PageIndex }};
                var res = _database.GetMultipleResultsList(Get_Ordered_Products_Loc, parameters);
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
        //============================================Get ONLINE  Logistic_Orders=====================================
        [HttpPost]
        [Route("GetAssigned_Logistic_Orders")]
        public IHttpActionResult GetAssigned_Logistic_Orders(VMModelsForOrderManagement GF)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {{ "@order_number", GF.OrderNumber },
                                                                                            { "@orderdatefrom", GF.OrderDateFrom },
                                                                                            { "@orderdateto", GF.OrderDateTo },
                                                                                            { "@unitid", GF.UnitID },
                                                                                            { "@villageid", GF.ParentID },
                                                                                            { "@logistictype", GF.Status },
                                                                                            { "@Page_Size",GF.PageSize },
                                                                                            { "@Page_Index", GF.PageIndex }};
                var res = _database.GetMultipleResultsList(Get_assign_Logistic, parameters);
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
        //=============================== ChangeRepurchasableStatus=================================
        [HttpPost]
        [Route("ChangeAutoStockStatus")]
        public IHttpActionResult ChangeAutoStockStatus(int ID, int Status)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@product_id", ID }, { "@status", Status } };
                var changed_status = _database.Query(Change_Auto_Stock_Status, parameters);
                return Ok(changed_status);
            }
            catch (Exception ex)
            {
                log.Error("ChangeAutoStockStatus----Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }
        //============================================Get Ordered Products By OrderID=====================================
        [HttpPost]
        [Route("iH_Get_All_Store_Orders_For_Logistics")]
        public IHttpActionResult iH_Get_All_Store_Orders_For_Logistics(VMModelsForOrderManagement GF)
        {
            try
            {
                var rolename = HttpContext.Current.User.Identity.GetRoleName();
                var commandText = Get_Direct_orders_Logistics;
                if (rolename != "LogisticAdmin")
                {
                    commandText = Get_Direct_orders_WHDC;
                }

                Dictionary<string, object> parameters = new Dictionary<string, object>() {{ "@Order_Number", GF.OrderNumber },
                                                                                            { "@Order_Date_From", GF.OrderDateFrom },
                                                                                            { "@Order_Date_To", GF.OrderDateTo },
                                                                                            { "@Mobile_Number", GF.MobileNumber },
                                                                                            { "@UnitID", GF.UnitID },
                                                                                            { "@Page_Index", GF.PageIndex },
                                                                                            { "@Page_Size",GF.PageSize }};
                var res = _database.ProductListWithCount(commandText, parameters);
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
        //======================================To Get All Price Request Categories
        [HttpGet]
        [Route("Get_All_Price_Request_Categories")]
        public IHttpActionResult Get_All_Price_Request_Categories()
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { };
                var result = _database.ProductListWithCount(Get_All_Categories, parameters);
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
        //==============================================To Upadte User Celebration Wallet Amount
        [HttpPost]
        [Route("Add_Update_Celebration_Wallet_Amount")]
        public IHttpActionResult Add_Update_Celebration_Wallet_Amount(VMWalletdetails vmodel)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@WalletAmount", vmodel.WalletAmount },
                                                                                            { "@Transaction_Type", vmodel.TransactionType },
                                                                                            { "@BuyerID", vmodel.BuyerID },
                                                                                            { "@MobileNumber", vmodel.MobileNumber },
                                                                                            { "@Remarks",vmodel.Remarks } };
                var res = _database.QueryValue(Update_Campaign_Wallet_Amount, parameters);
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
        //------------ To view get EMI ordered products------------
        [HttpGet]
        [Route("GetEMIOrderedProducts")]
        public IHttpActionResult GetEMIOrderedProducts(int orderid)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@order_id", orderid } };
                var categoryGetList = _database.ProductListWithCount(Category_GetList_EMI, parameters);
                return Ok(categoryGetList);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        //============================================Get Ordered Products By OrderID=====================================
        [HttpGet]
        [Route("GetOrderedProducts_Inventory")]
        public IHttpActionResult GetOrderedProducts_Inventory(int OrderID, int unitid)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@order_id", OrderID }, { "@unitid", unitid } };
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
        //=============================================To Get Order Details With OrderId==========================================================
        [HttpGet]
        [Route("GetOrderDetails_ByOrderId")]
        public IHttpActionResult GetOrderDetails_ByOrderId(int OrderId)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@OrderNumber", OrderId } };
                var Orders = _database.GetMultipleResultsListAll(Order_details, parameters);
                return Ok(Orders);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }
        //=============>>>>>>>>>>>> To Get Order Details <<<<<<<=============================
        private static void GetorderDetails(VMPagingResultsPost vmodel, IHttpActionResult res1)
        {
            if (res1 is OkNegotiatedContentResult<VMDataTableResponse>)
            {
                VMAddress add = new VMAddress();
                var ress = res1 as OkNegotiatedContentResult<VMDataTableResponse>;
                var result = ress.Content;
                if (result.Addressset.Count() > 0)
                {
                    vmodel.PhoneNumber = result.Addressset[0]["Alternative_Mobile_Number"];
                    vmodel.EmailID = result.Addressset[0]["Email"].ToString();
                    vmodel.Name = result.Addressset[0]["Contact_Person_Name"];
                    var buyeraddress = result.Addressset[0]["AddressJson"];
                    var address = result.Addressset[0]["Order_Delivery_Address_Line_One"];
                    var name = result.Addressset[0]["Contact_Person_Name"];
                    var Number = result.Addressset[0]["Alternative_Mobile_Number"];
                    vmodel.Site_ID = Convert.ToInt32(result.Resultset[0]["Order_From"]);
                }
            }
        }
        //==============>>>>>>>>>Update_Ordered_Products_Status_Change_Direct<<<<<<===================
        [HttpPost]
        [Route("Update_Ordered_Products_Status_Change_Direct")]
        public IHttpActionResult Update_Ordered_Products_Status_Change_Direct(VMPagingResultsPost vmodal)
        {
            try
            {
                var message = "";
                var Emailmessage = "";
                string sType = string.Empty;
                int iTotal = 0; int iPacked = 1; string sPhoneNumber = string.Empty;
                string Link = WebConfigurationManager.AppSettings["Track"];
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Product_details_Id", vmodal.productid },
                                                                                            { "@unitid", vmodal.Unit_ID },
                                                                                            { "@orderedlocation_id", vmodal.OrderID2 },
                                                                                            { "@Status", vmodal.Status } };
                var res = _database.QueryValue(Update_Warehouse_and_vendor_Status_Direct, parameters);

                var Result = GetOrderDetails_ByOrderId(Convert.ToInt32(vmodal.OrderID));
                var oAddress = ((System.Web.Http.Results.OkNegotiatedContentResult<iHubAdminAPI.Models.VMDataTableResponse>)Result).Content;
                if (oAddress.Addressset != null && oAddress.Addressset.Count() > 0)
                {
                    VMAddress add = new VMAddress();
                    var ress = Result as OkNegotiatedContentResult<VMDataTableResponse>;
                    var result = ress.Content;
                    var buyeraddress = result.Addressset[0]["AddressJson"];
                    var name = result.Addressset[0]["Contact_Person_Name"];
                    var Number = result.Addressset[0]["Alternative_Mobile_Number"];
                    var Pin_Code = result.Addressset[0]["Pin_Code"];
                    add.Address_Line_One = buyeraddress;
                    add.FullName = name;
                    add.MobileNumber = Number;
                    add.Pincode = Pin_Code;
                    vmodal.buyeraddress = add;
                }

                Dictionary<string, object> Param = new Dictionary<string, object>() { { "@OrderNumber", Convert.ToInt32(vmodal.OrderID) } };
                var oCatList = _database.GetMultipleResultsListAll("iS_Get_Order_Details_By_Order_Number", Param);
                if (oCatList != null)
                {
                    if (oCatList.Addressset != null && oCatList.Addressset.Count() > 0)
                    {
                        iTotal = oCatList.Addressset.Count();
                        sPhoneNumber = oCatList.Addressset[0]["Mobile_Number"];
                    }
                }

                if (vmodal.Status == 20)
                {
                    message = "Your item(s) " + vmodal.ProductName + " of order id " + vmodal.OrderID + " is Packed and will be ready for dispatch soon. Track order details here: " + Link;
                    Emailmessage = "Your item(s) " + vmodal.ProductName + " of order id " + vmodal.OrderID + " is Packed and will be ready for dispatch soon.";
                    sType = "Packed";
                }
                else if (vmodal.Status == 80)
                {
                    message = "Your item(s) " + vmodal.ProductName + " of order id " + vmodal.OrderID + " is dispatched and will be delivered soon. Track order details here: " + Link;
                    Emailmessage = "Your item(s) " + vmodal.ProductName + " of order id " + vmodal.OrderID + " is dispatched and will be delivered soon.";
                }
                else if (vmodal.Status == 50)
                {
                    message = "Your item(s) " + vmodal.ProductName + " of order id " + vmodal.OrderID + " is successfully delivered today. Thank you for shopping & visit again. ";
                    Emailmessage = "Your item(s) " + vmodal.ProductName + " of order id " + vmodal.OrderID + " is successfully delivered today. Thank you for shopping & visit again.";
                    sType = "Delivered";
                }
                else if (vmodal.Status == 60)
                {
                    message = "Based on your request, " + vmodal.ProductName + " from order Id:" + vmodal.OrderID + " is cancelled. we look forward to serve you again. ";
                    Emailmessage = "Based on your request, " + vmodal.ProductName + " from order Id:" + vmodal.OrderID + " is cancelled. we look forward to serve you again.";
                    sType = "Cancelled";
                }
                else if (vmodal.Status == 30)
                {
                    message = "Your item(s) " + vmodal.ProductName + " of order id " + vmodal.OrderID + " is Ready for Dispatch and will be dispatched soon. Track order details here: " + Link;
                    Emailmessage = "Your item(s) " + vmodal.ProductName + " of order id " + vmodal.OrderID + " is Ready for Dispatch and will be dispatched soon.";
                    sType = "Picked";
                }
                vmodal.Message = message;
                vmodal.EmailMessage = Emailmessage;
                GetorderDetails(vmodal, Result);
                var iDomainID = vmodal.Site_ID;
                var oSMSMailData = dbContext.MailSMSSettings.Where(m => m.iDomainID == iDomainID).Where(m => m.sTemplateType == "MAIL").Where(m => m.sType == sType).FirstOrDefault();
                var DomainDetails = dbContext.iHub_Domains.Where(x => x.Site_ID == iDomainID).FirstOrDefault();
                vmodal.DomainName = DomainDetails.sName;
                vmodal.Subject = "Order Status";
                var oSMSD = dbContext.MailSMSSettings.Where(m => m.iDomainID == iDomainID).Where(m => m.sTemplateType == "SMS").Where(m => m.sType == sType).FirstOrDefault();

                if (oSMSD != null && oSMSD.SMS == true)
                {
                    if (oSMSD.sBody != null && oSMSD.sBody != "" && oSMSD.sBody.Contains("{{"))
                    {
                        string sMsg = oSMSD.sBody;

                        sMsg = sMsg.Replace("{{P}}", iPacked.ToString());

                        sMsg = sMsg.Replace("{{T}}", iTotal.ToString());

                        sMsg = sMsg.Replace("{{O}}", vmodal.OrderID.ToString());
                        message = sMsg;
                    }
                    Common.sendmessage(sPhoneNumber, message, oSMSD.sdltid);
                }
                var oMasterD = dbContext.DomainsMasterData.Where(m => m.DomainID == iDomainID).ToList().FirstOrDefault();
                if (oMasterD != null)
                {
                    var sImgpath = System.Configuration.ConfigurationManager.AppSettings["ImgPath"].ToString();
                    vmodal.ImagePath = sImgpath + iDomainID + "_Logo" + "/" + oMasterD.sLogo;
                    vmodal.LogoTitle = oMasterD.sLogoTagLine;
                }
                if (oSMSMailData != null && oSMSMailData.sSubject != null && oSMSMailData.sSubject != "")
                {
                    vmodal.Subject = oSMSMailData.sSubject;
                    if (oSMSMailData.sTrackhere != null && oSMSMailData.sTrackhere != "")
                    {
                        vmodal.TrackDetails = oSMSMailData.sTrackhere;
                    }
                    if (oSMSMailData.sEmail != null && oSMSMailData.sEmail != "")
                    {
                        vmodal.sEmail = oSMSMailData.sEmail;
                    }
                }
                if (oSMSMailData != null && oSMSMailData.sMobileNumber != null && oSMSMailData.sMobileNumber != "")
                {
                    vmodal.MobileNumber = oSMSMailData.sMobileNumber;
                }
                else
                {
                    vmodal.MobileNumber = "040-48542234";
                }
                string sDom = string.Empty;
                if (vmodal.DomainName != null)
                {
                    sDom = vmodal.DomainName.Replace(" ", string.Empty).ToLower();
                    //vmodal.sEmail = "info@" + "" + sDom + ".in";
                    vmodal.Url = "www." + "" + sDom + ".in";
                }
                if (vmodal.EmailID != "")
                {
                    if (oSMSMailData != null && oSMSMailData.MAIL == true)
                    {
                        //Common.sendemail(vmodal.EmailID, Emailmessage);
                       Mailer.ChangeBuyerOrderStatus(vmodal, vmodal.EmailID).Send();
                    }
                }
                
                
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

        public string DynamicSMSBuild(string sProductName, int iOrderNumber, string sBody)
        {
            string sMsg = string.Empty;
            string sRemoveMsg = string.Empty;
            sMsg = sBody;
            var sSplitstring = sMsg.Split(' ').ToList();
            foreach (var s in sSplitstring)
            {
                if (s.Contains("}}"))
                {
                    int iCollectionIndex = sMsg.IndexOf("}}");
                    int iStartPosi = sMsg.LastIndexOf("{{", iCollectionIndex);
                    int iStringLength = "}}".Length;
                    var ssMsg = sMsg.Substring(iStartPosi, (iCollectionIndex - iStartPosi) + iStringLength).Replace(" ", "");
                    if (ssMsg.ToLower() == "{{productname}}")
                    {
                        if (sMsg.Contains(ssMsg))
                        {
                            if (sMsg.Contains(ssMsg))
                            {
                                sMsg = sMsg.Replace(ssMsg, sProductName.ToString());
                            }
                        }
                    }
                    else if (ssMsg.ToLower() == "{{orderid}}")
                    {
                        if (sMsg.Contains(ssMsg))
                        {
                            if (sMsg.Contains(ssMsg))
                            {
                                sMsg = sMsg.Replace(ssMsg, iOrderNumber.ToString());
                            }
                        }
                    }
                }
            }
            return sMsg;
        }


        
        //=============================================To Get Order Details With OrderId==========================================================
        [HttpGet]
        [Route("GetOrderDetails_ByOrderId_Inv")]
        public IHttpActionResult GetOrderDetails_ByOrderId_Inv(int OrderId)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@OrderNumber", OrderId } };
                var Orders = _database.GetMultipleResultsListAll(Order_details_Inv, parameters);
                return Ok(Orders);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }
        //=========>>>>>>>>>>>>>>>> To Add Top Brands <<<<<<<<<<<<<<==============//
        [HttpPost]
        [Route("Add_Top_Brands")]
        public IHttpActionResult Add_Top_Brands(VMPagingResultsPost vmodel)
        {
            try
            {
                var cmd = "SELECT Name FROM iH_Brands WHERE ID=" + vmodel.ID;
                Dictionary<string, object> parameter1 = new Dictionary<string, object> { };
                var res = _database.SelectQuery(cmd, parameter1);
                if (res != null)
                {
                    if (vmodel.sProductType == "A")
                    {
                        string sBrandName = res.FirstOrDefault().Select(m => m.Value).FirstOrDefault();
                        var cmd1 = "SELECT ID FROM Domain_Brands_Mapping WHERE iBrandID=" + vmodel.ID + " and iSiteID=" + vmodel.Site_ID;
                        Dictionary<string, object> Param2 = new Dictionary<string, object> { };
                        var res1 = _database.SelectQuery(cmd1, Param2);
                        if (res1 != null && res1.Count() > 0)
                        {
                            var iID = res1.FirstOrDefault().Select(m => m.Value).FirstOrDefault();
                            cmd = "Update Domain_Brands_Mapping set Is_BestSelling=" + "'" + vmodel.Status + "'" + " where iSiteID=" + vmodel.Site_ID + " and ID=" + iID;
                            Dictionary<string, object> Param = new Dictionary<string, object> { };
                            var oUpdate = _database.SelectQuery(cmd, Param);
                        }
                        else
                        {
                            Dictionary<string, object> Param1 = new Dictionary<string, object> { };
                            var sColumns = "iSiteID,iBrandID,sBrandName,Is_BestSelling";
                            var sValues = "" + vmodel.Site_ID + "," + vmodel.ID + ",'" + sBrandName + "'," + vmodel.Status + "";
                            string sQuery = "INSERT into Domain_Brands_Mapping (" + sColumns + ") VALUES (" + sValues + ")";
                            SQLDatabase oSql = new SQLDatabase();
                            var oObje = oSql.InsertQueryCommand(sQuery, Param1);
                        }
                    }
                    else if (vmodel.sProductType == "R")
                    {
                        cmd = "Update Domain_Brands_Mapping set Is_BestSelling=" + "'" + vmodel.Status + "'" + " where iSiteID=" + vmodel.Site_ID + " and iBrandID=" + vmodel.ID;
                        Dictionary<string, object> Param = new Dictionary<string, object> { };
                        var oUpdate = _database.SelectQuery(cmd, Param);
                    }
                    string CacheKey = "HomePageTopBrands_" + vmodel.Site_ID.ToString();
                    var keyItems = dbContext.CacheTransactions.Where(x => x.CacheKey == CacheKey && !x.Status).ToList();
                    if (keyItems != null)
                    {
                        foreach (var keyItem in keyItems)
                        {
                            keyItem.Status = true;
                            keyItem.Last_Updated_DateTime = DateTime.Now;
                        }
                    }

                    CacheTransactions ct = new CacheTransactions();
                    ct.CacheKey = CacheKey;
                    ct.Type = "HomePage";
                    ct.Status = false;
                    ct.Last_Updated_DateTime = DateTime.Now;
                    dbContext.CacheTransactions.Add(ct);
                    var num = dbContext.SaveChanges();

                }
                //Dictionary<string, object> parameter = new Dictionary<string, object> { { "@bid", vmodel.ID }, { "@status", vmodel.Status }, { "@siteid", vmodel.Site_ID } };
                //var response = _database.Query(To_Add_Top_Brands, parameter);
                return Ok('1');
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }
        //=========>>>>>>>>>>>>>>>> To Add Top Brands <<<<<<<<<<<<<<==============//
        [HttpGet]
        [Route("Top_Brands_List")]
        public IHttpActionResult Top_Brands_List(int iSiteID)
        {
            try
            {
                var cmd = string.Empty;
                if (iSiteID != 0)
                {
                    cmd = "SELECT * FROM Domain_Brands_Mapping where Is_BestSelling=1 and iSiteID=" + iSiteID;
                }
                //var cmd = "SELECT * FROM \"iH_Brands\" where \"Is_BestSelling\"=" + 1;                
                Dictionary<string, object> parameter1 = new Dictionary<string, object> { };
                var res = _database.SelectQuery(cmd, parameter1);
                return Ok(res);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }
        //===================== GET Cash Reports List =================
        [HttpPost]
        [Route("ViewCashReports_Records")]
        public IHttpActionResult ViewCashReports_Records(VMModelsForMaster vmodel)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@unit_id", vmodel.Unit_ID },
                                                                                          { "@Year", vmodel.year },
                                                                                          { "@Month", vmodel.month }  };
                var result = _database.GetMultipleResultsList2(View_Cash_Reports, parameters);
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

        //===================== GET Cash Reports List =================
        [HttpPost]
        [Route("ViewCashReports_Records_Details")]
        public IHttpActionResult ViewCashReports_Records_Details(VMModelsForMaster vmodel)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@unit_id", vmodel.Unit_ID },
                                                                                          { "@DATE", vmodel.Date }
                                                                                           };
                var result = _database.ProductListWithCount(View_Cash_Reports_Details, parameters);
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
        [HttpGet]
        [Route("GetFilesCount")]
        public IHttpActionResult GetFilesCount(string productcode)
        {
            try
            {
                string my_path = HttpContext.Current.Server.MapPath("~/" + "Images/ProductImages");
                FileInfo[] files = new DirectoryInfo(my_path + "/" + productcode).GetFiles();
                int fileCount = files.Count();
                return Ok(fileCount);
            }
            catch (Exception ex)
            {
                log.Error("Error---------", ex);
                return Ok(0);
            }
        }
        //=============================== Manual Product Adding=================================
        [HttpPost]
        [Route("AddMultipleProducts")]
        public IHttpActionResult AddMultipleProducts(VMPagingResultsPost vm)
        {
            try
            {
                var random = new Random(System.DateTime.Now.Millisecond);
                var randomNumber = random.Next(1, 5000000);
                int userid = 1;
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@User_ID", userid }, { "@RF_Number", randomNumber }, { "@IP", "205.122.23322" }, { "@Json_Data", vm.JsonData }, { "@HtmlContent", vm.HtmlContent } };
                var addingproducts = _database.QueryValue(Upload_New_Products, parameters);
                return Ok(addingproducts);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("AddMultipleProducts---Error---------", ex);
                return Ok(cs);
            }
        }
        [HttpPost]
        [Route("Get_Hsncodevalidation_Controller")]
        public IHttpActionResult Get_Hsncodevalidation_Controller(int Hsncode)
        {
            try
            {
                var cmd = "SELECT * FROM iH_HSN_Code where HSN_Code_ID=" + Hsncode;
                Dictionary<string, object> parameter1 = new Dictionary<string, object> { };
                var res = _database.SelectQuery(cmd, parameter1);
                return Ok(res);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        [HttpPost]
        [Route("Update_Product_MRP_With_SellingPrice")]
        public IHttpActionResult Update_Product_MRP_With_SellingPrice(int iUpdateProductID, float dSP)
        {
            try
            {
                var cmd = "UPDATE iHub_Products SET MRP=" + dSP + " WHERE iHub_Product_ID=" + iUpdateProductID;
                Dictionary<string, object> parameter1 = new Dictionary<string, object> { };
                var res = _database.SelectQuery(cmd, parameter1);
                return Ok(res);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        //=====================>>>>>> Cash in Store<<<<<<=================        
        [HttpPost]
        [Route("Get_All_CashInStores_By_Date")]
        public IHttpActionResult Get_All_CashInStores_By_Date(VMPagingResultsPost CA)
        {
            try
            {
                string Cash_In_Store = "iAdmin_Cash_In_Store_By_Date";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@PayDate", CA.PayDate }, { "@UnitID", CA.StoreID }, { "@CashFlag", CA.CashFlag }, { "@ClusterID", CA.ClusterID } };
                var res = _database.GetMultipleResultsListAll(Cash_In_Store, parameters);
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

        [HttpGet]
        [Route("GetOrdered_PackageProducts")]
        public IHttpActionResult GetOrdered_PackageProducts(int Orderid, int PackageID, int Orderdetailsid)
        {
            try
            {
                // var result = dbContext.UserBasket_Products.Where(x => x.Order_ID == Orderid && x.UserBasketPakage_ID == PackageID && x.Order_Details_ID==Orderdetailsid).ToList();
                var result = (from pc in dbContext.UserBasket_Products
                              join up in dbContext.User_Packages on pc.UserBasketPakage_ID equals up.User_PackageID
                              join ih in dbContext.iHub_Products on pc.Product_ID equals ih.iHub_Product_ID
                              where pc.Order_ID == Orderid && up.Package_ID == PackageID && pc.Order_Details_ID == Orderdetailsid
                              select new
                              {
                                  pc.Product_ID,
                                  pc.Quantity,
                                  pc.UserBasketPakage_ID,
                                  ih.Product_Name,
                                  ih.iHub_Product_ID,
                                  ih.Selling_Price,
                                  ih.Product_Series,
                                  ih.Reference_Number
                              }).ToList();

                List<dynamic> test = new List<dynamic>();
                foreach (var m in result)
                {
                    VMiHub_Products vmprodutc = new VMiHub_Products();
                    var iCount = dbContext.iHub_Inventory_Products.Where(x => x.Product_Id == m.Product_ID && x.Inventory_Product_Status == 10 && x.Order_Id == 0).Count();
                    var AssiCount = dbContext.iHub_Inventory_Products.Where(x => x.Product_Id == m.Product_ID && (x.Inventory_Product_Status == 30 || x.Inventory_Product_Status == 31) && x.Order_Id == Orderid).Count();
                    int reqCount = 0;
                    if (m.Quantity > iCount)
                    {
                        reqCount = m.Quantity;
                    }
                    if (AssiCount >= m.Quantity)
                    {
                        reqCount = 0;
                    }
                    vmprodutc.Product_ID = m.Product_ID;
                    vmprodutc.Quantity = m.Quantity;
                    vmprodutc.UserBasketPakage_ID = m.UserBasketPakage_ID;
                    vmprodutc.Reference_Number = reqCount;
                    vmprodutc.Product_Name = m.Product_Name;
                    vmprodutc.iHub_Product_ID = m.iHub_Product_ID;
                    vmprodutc.Selling_Price = m.Selling_Price;
                    vmprodutc.Product_Series = m.Product_Series;
                    test.Add(vmprodutc);
                }


                return Ok(test);

            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        //========================================= GET All iHubDomains List =========================================//

        [HttpGet]
        [Route("Get_All_iHub_Domains_List")]
        public IHttpActionResult Get_All_iHub_Domains_List()
        {
            try
            {
                var cmd = "SELECT Site_ID,Domain_Name from \"iHub_Domains\"";
                Dictionary<string, object> parameter1 = new Dictionary<string, object> { };
                var res = _database.SelectQuery(cmd, parameter1);
                return Ok(res);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        //========================================= GET All iHubDomains List =========================================//

        [HttpPost]
        [Route("Get_All_Section_Status_List")]
        public IHttpActionResult Get_All_Section_Status_List(VMPagingResultsPost vmodel)
        {
            try
            {
                var cmd = "SELECT SectionID, bIsSectionStatus, sTitle, iTitleStatus, sProductType FROM iD_HomePage WHERE Status = 10 and Site_ID =" + vmodel.Site_ID + " GROUP BY SectionID,bIsSectionStatus,sTitle,iTitleStatus,sProductType";
                Dictionary<string, object> parameter1 = new Dictionary<string, object> { };
                var res = _database.SelectQuery(cmd, parameter1);
                return Ok(res);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        //=========>>>>>>>>>>>>>>>> To Add Footer Sections <<<<<<<<<<<<<<==============//
        [HttpPost]
        [Route("Get_All_FooterSections_Details")]
        public IHttpActionResult Get_All_FooterSections_Details(VMPagingResultsPost vmodel)
        {
            try
            {
                var cmd = "SELECT ID, CategoryName, Is_LeafNode FROM iH_Categories WHERE ParentID =" + vmodel.ID + " ORDER BY ID";
                Dictionary<string, object> parameter1 = new Dictionary<string, object> { };
                var res = _database.SelectQuery(cmd, parameter1);
                return Ok(res);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        //=========>>>>>>>>>>>>>>>> To Footer sections categories <<<<<<<<<<<<<<==============//
        [HttpPost]
        [Route("Update_Selected_Footer_Categories")]
        public IHttpActionResult Update_Selected_Footer_Categories(VMPagingResultsPost vmodel)
        {
            try
            {
                if (vmodel.oFooterSections != null && vmodel.oFooterSections.Count() != 0)
                {
                    if (vmodel.sParentID != "0")
                    {
                        var cmd = "SELECT * FROM iH_FooterSections WHERE ParentID=" + vmodel.sParentID + " AND SiteID=" + vmodel.Site_ID;
                        Dictionary<string, object> parameter1 = new Dictionary<string, object> { };
                        var res = _database.SelectQuery(cmd, parameter1);
                        if (res.Count() != 0)
                        {
                            var cmd1 = "DELETE FROM iH_FooterSections WHERE ParentID=" + vmodel.sParentID + " AND SiteID=" + vmodel.Site_ID; ;
                            Dictionary<string, object> parameter2 = new Dictionary<string, object> { };
                            var res1 = _database.SelectQuery(cmd1, parameter2);
                        }
                    }
                    foreach (var item in vmodel.oFooterSections)
                    {
                        var leafcount = dbContext.iH_Categories.Where(x => x.ParentID == item.ID).Count();
                        if (leafcount > 0)
                        {
                            item.Is_LeafNode = false;
                        }
                        else
                        {
                            item.Is_LeafNode = true;
                        }
                        Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@categoryid", item.ID },
                                                                                          { "@parentid", vmodel.sParentID },
                                                                                          { "@name", item.CategoryName } ,
                            {"@siteid",vmodel.Site_ID },
                        {"@bIsLeaf",item.Is_LeafNode }};
                        var result = _database.QueryValue(Update_Footer_Sections_New, parameters);
                    }
                }

                string CacheKey = "HomePageSelFooter_" + vmodel.Site_ID.ToString();
                var keyItems = dbContext.CacheTransactions.Where(x => x.CacheKey == CacheKey && !x.Status).ToList();
                if (keyItems != null)
                {
                    foreach (var keyItem in keyItems)
                    {
                        keyItem.Status = true;
                        keyItem.Last_Updated_DateTime = DateTime.Now;
                    }
                }

                CacheTransactions ct = new CacheTransactions();
                ct.CacheKey = CacheKey;
                ct.Type = "HomePage";
                ct.Status = false;
                ct.Last_Updated_DateTime = DateTime.Now;
                dbContext.CacheTransactions.Add(ct);
                var num = dbContext.SaveChanges();

                return Ok(1);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        //=========>>>>>>>>>>>>>>>> To Add Footer Sections <<<<<<<<<<<<<<==============//
        [HttpPost]
        [Route("Get_All_Selected_FooterSections")]
        public IHttpActionResult Get_All_Selected_FooterSections(VMPagingResultsPost vmodel)
        {
            try
            {
                List<VMPagingResultsPost> oResu = new List<VMPagingResultsPost>();
                var oSecNames = vmodel.SectionId.Split(',').ToList();
                if (oSecNames.Count() > 0)
                {
                    foreach (var sec in oSecNames)
                    {

                        var cmd = "SELECT Type_ID FROM iD_HomePage WHERE SectionID=" + "'" + sec + "' and Status=10 and Site_ID=" + vmodel.Site_ID;
                        Dictionary<string, object> parameter = new Dictionary<string, object> { };
                        var res = _database.SelectQuery(cmd, parameter);
                        if (res.Count() > 0)
                        {
                            var iSectionID = res[0].Values.FirstOrDefault();
                            var cmd1 = "SELECT CategoryID,Name FROM iH_FooterSections WHERE ParentID=" + iSectionID + " and SiteID=" + vmodel.Site_ID;
                            Dictionary<string, object> parameter1 = new Dictionary<string, object> { };
                            var resu = _database.SelectQuery(cmd1, parameter1);
                            if (resu.Count() > 0)
                            {
                                foreach (var it in resu)
                                {
                                    VMPagingResultsPost oPage = new VMPagingResultsPost();
                                    oPage.CategoryID = Convert.ToInt32(it.ToList().Where(m => m.Key == "CategoryID").Select(m => m.Value).FirstOrDefault());
                                    oPage.Name = it.ToList().Where(m => m.Key == "Name").Select(m => m.Value).FirstOrDefault();
                                    oPage.SectionId = sec;
                                    oResu.Add(oPage);
                                }
                            }
                        }

                    }
                }
                return Ok(oResu);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        [HttpPost]
        [Route("Get_Package_Products_Data")]
        public IHttpActionResult Get_Package_Products_Data(VMModelsForProduct vmodel)
        {
            try
            {
                var oPCmd = "SELECT * FROM iH_Packages WHERE iH_PackagesID=" + vmodel.CategoryID;
                Dictionary<string, object> oP = new Dictionary<string, object> { };
                var oPD = _database.SelectQuery(oPCmd, oP);
                if (oPD != null && oPD.Count() > 0)
                {
                    var oPCCmd = "SELECT * FROM PackageCategory WHERE PackageID=" + vmodel.CategoryID;
                    Dictionary<string, object> oPC = new Dictionary<string, object> { };
                    var oPCD = _database.SelectQuery(oPCCmd, oPC);
                    if (oPCD != null && oPCD.Count() > 0)
                    {
                        string sPProIDs = string.Empty;
                        var ProdID = new List<string>();
                        foreach (var oPCS in oPCD)
                        {
                            var iValue = oPCS.Where(m => m.Key.ToLower() == "packagecategoryid").Select(m => m.Value).FirstOrDefault();
                            ProdID.Add(iValue);
                        }
                        sPProIDs = string.Join(",", ProdID);
                        var oPPCmd = "SELECT PP.PackageProductsID,PP.PackageCategoryID,PP.iHub_Product_ID,PP.Product_Name,PP.Status,PP.isMandatory,PP.MaxQuantity,PP.DisplayOrder,PP.Image_Code,PP.isFreeProduct,PP.MRP,PP.Selling_Price,PP.Package_ID,HP.Product_Series FROM PackageProducts PP INNER JOIN iHub_Products HP ON HP.iHub_Product_ID = PP.iHub_Product_ID WHERE PackageCategoryID IN(" + sPProIDs + ")";
                        Dictionary<string, object> oPP = new Dictionary<string, object> { };
                        var oPPD = _database.SelectQuery(oPPCmd, oPP);
                        return Ok(oPPD);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Get_Package_Products_Data-----------Error---------", ex);
                return Ok(cs);
            }
        }

        //=============================== HasBarcode Status Changes =================================
        [HttpGet]
        [Route("ChangeHasBarcodeStatus")]
        public IHttpActionResult ChangeHasBarcodeStatus(int ID, int DirectStatus)
        {
            try
            {
                var cmd = "UPDATE iHub_Products SET HasBarcode=" + DirectStatus + " WHERE iHub_Product_ID=" + ID;
                Dictionary<string, object> parameter1 = new Dictionary<string, object> { };
                var res = _database.SelectQuery(cmd, parameter1);
                return Ok(res);
            }
            catch (Exception ex)
            {
                log.Error("ChangeRepurchasableStatus----Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        //------------=========================================App Related Code =============================

        [HttpPost]
        [Route("iH_Get_All_OnlineDCorders")]
        public IHttpActionResult iH_Get_All_OnlineDCorders(VMModelsForOrderManagement GF)
        {
            try
            {
                var rolename = HttpContext.Current.User.Identity.GetRoleName();
                var commandText = Get_Direct_orders_WHDC_App;

                Dictionary<string, object> parameters = new Dictionary<string, object>() {{ "@Order_Number", GF.OrderNumber },
                                                                                            { "@Order_Date_From", GF.OrderDateFrom },
                                                                                            { "@Order_Date_To", GF.OrderDateTo },
                                                                                            { "@Mobile_Number", GF.MobileNumber },
                                                                                            { "@UnitID", GF.UnitID },
                                                                                            { "@CategoryID",GF.CategoryID},
                                                                                            { "@OrderID",GF.OrderID},
                                                                                            { "@SKU",GF.Status},
                                                                                            { "@Page_Index", GF.PageIndex },
                                                                                            { "@Page_Size",GF.PageSize }};
                var res = _database.ProductListWithCount(commandText, parameters);
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

        //============================================Get Ordered Products By OrderID and Category iD modified on 10Sep20=====================================
        [HttpGet]
        [Route("GetOrderedProducts_Inventory_DCApp")]
        public IHttpActionResult GetOrderedProducts_Inventory_DCApp(int OrderID, int unitid, int ParentCategoryID, int paramSKU)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@order_id", OrderID }, { "@unitid", unitid }, { "@ParentCategoryID", ParentCategoryID }, { "@SKU", paramSKU } };
                var res = _database.ProductListWithCount(Get_Ordered_Products_Inv_App, parameters);
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

        //API to get IMEI details of the Product ,added on 1stOctober2020
        [HttpGet]
        [Route("Get_ProductIMEI_DCApp")]
        public IHttpActionResult Get_ProductIMEI_DCApp(int OrderID, string ProductID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@OrderID", OrderID }, { "@ProductID", ProductID } };
                var res = _database.ProductListWithCount(Get_Product_IMEIInfo_App, parameters);
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

        //------------=========================================App Related Code END=============================

        #region Product_HasBarcode_Change_By_Category

        [HttpPost]
        [Route("Change_Product_Values_by_Category")]
        public IHttpActionResult Change_Product_Values_by_Category(VMModelsForCategory oCat)
        {
            CustomResponse cs = new CustomResponse();
            try
            {
                string cmd = "[Change_Product_Values_by_CategoryID]";
                Dictionary<string, object> Param = new Dictionary<string, object>() { { "@ID", oCat.Category_Id }, { "@iValue", oCat.iParentID }, { "@sColumn", oCat.ProductName } };
                var oCTM = _database.Query(cmd, Param);
                cs.ResponseID = 1;
                return Ok(cs);
            }
            catch (Exception ex)
            {
                cs.Response = ex.Message;
                log.Error("Change_Product_Values_by_CategoryID--Error---------", ex);
                return Ok(cs);
            }
        }

        #endregion Product_HasBarcode_Change_By_Category

        #region DomainsMasteDataCode

        [HttpGet]
        [Route("GetDistrictsByStateID")]
        public IHttpActionResult GetDistrictsByStateID(int iStateID, int hierarchy)
        {
            try
            {
                var Cmd = "SELECT * FROM iH_Master_Locations where ParentID=" + iStateID + "and Location_Hierarchy_Level=" + hierarchy;
                Dictionary<string, object> Param = new Dictionary<string, object> { };
                var res = _database.SelectQuery(Cmd, Param);
                return Ok(res);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        //[HttpPost] --harsha
        //[Route("Update_Selected_Domains_Master_Districts")]
        //public IHttpActionResult Update_Selected_Domains_Master_Districts(VMPagingResultsPost vmodel)
        //{
        //    try
        //    {
        //        var oDList = vmodel.oFooterSections.ToList().Select(m => m.ID).ToList();
        //        var oAllDList = dbContext.DomainsDistData.Where(m => m.DomainID == vmodel.Site_ID).Select(m => m.DistID).ToList();
        //        if (oDList.Count() > 0)
        //        {
        //            var oExList = oDList.Except(oAllDList).ToList();
        //            var oExList1 = oAllDList.Except(oDList).ToList();
        //            if (oExList1.Count() > 0)
        //            {
        //                foreach (var item in oExList1)
        //                {
        //                    var oRemove = dbContext.DomainsDistData.Where(m => m.DomainID == vmodel.Site_ID).Where(m => m.DistID == item).ToList();
        //                    dbContext.DomainsDistData.RemoveRange(oRemove);
        //                    dbContext.SaveChanges();
        //                }
        //            }
        //            foreach (var dID in oExList)
        //            {
        //                DomainsDistConfiguration oDM = new DomainsDistConfiguration();
        //                oDM.DistID = dID;
        //                oDM.DomainID = vmodel.Site_ID;
        //                dbContext.DomainsDistData.Add(oDM);
        //                dbContext.SaveChanges();
        //            }
        //        }
        //        else if (oDList.Count() == 0)
        //        {
        //            var oExList1 = oAllDList.Except(oDList).ToList();
        //            if (oExList1.Count() > 0)
        //            {
        //                foreach (var item in oExList1)
        //                {
        //                    var oRemove = dbContext.DomainsDistData.Where(m => m.DomainID == vmodel.Site_ID).Where(m => m.DistID == item).ToList();
        //                    dbContext.DomainsDistData.RemoveRange(oRemove);
        //                    dbContext.SaveChanges();
        //                }
        //            }
        //        }
        //        int iShowDist = 0;
        //        if (vmodel.bShowDistrict == true)
        //        {
        //            iShowDist = 1;
        //        }
        //        else
        //        {
        //            iShowDist = 0;
        //        }
        //        var Cmd = "update Domain_District_Configuration set bShowDistrict=" + iShowDist + " where DomainID=" + vmodel.Site_ID;
        //        Dictionary<string, object> Param = new Dictionary<string, object> { };
        //        var res = _database.SelectQuery(Cmd, Param);
        //        return Ok(1);
        //    }
        //    catch (Exception ex)
        //    {
        //        CustomResponse cs = new CustomResponse();
        //        cs.Response = ex.Message;
        //        return Ok(cs);
        //    }
        //}

        [HttpPost]
        [Route("Update_Selected_Domains_Master_Districts")]
        public IHttpActionResult Update_Selected_Domains_Master_Districts(VMPagingResultsPost vmodel)
        {
            try
            {
                var oDList = vmodel.oFooterSections.ToList().Select(m => m.ID).ToList();
                var oAllDList = dbContext.DomainsDistData.Where(m => m.DomainID == vmodel.Site_ID).Select(m => m.DistID).ToList();
                if (oDList.Count() > 0)
                {
                    var oExList = oDList.Except(oAllDList).ToList();
                    var oExList1 = oAllDList.Except(oDList).ToList();
                    if (oExList1.Count() > 0)
                    {
                        foreach (var item in oExList1)
                        {
                            //var oRemove = dbContext.DomainsDistData.Where(m => m.DomainID == vmodel.Site_ID).Where(m => m.DistID == item).ToList();
                            var oRemove = dbContext.DomainsDistData.Where(m => m.DomainID == vmodel.Site_ID).Where(m => m.DistID == item).Where(m => m.CityID == 0).ToList();
                            dbContext.DomainsDistData.RemoveRange(oRemove);
                            dbContext.SaveChanges();
                        }
                    }
                    foreach (var dID in oExList)
                    {
                        DomainsDistConfiguration oDM = new DomainsDistConfiguration();
                        oDM.DistID = dID;
                        oDM.DomainID = vmodel.Site_ID;
                        dbContext.DomainsDistData.Add(oDM);
                        dbContext.SaveChanges();
                    }
                }
                else if (oDList.Count() == 0)
                {
                    var oExList1 = oAllDList.Except(oDList).ToList();
                    if (oExList1.Count() > 0)
                    {
                        foreach (var item in oExList1)
                        {
                            //var oRemove = dbContext.DomainsDistData.Where(m => m.DomainID == vmodel.Site_ID).Where(m => m.DistID == item).ToList();
                            var oRemove = dbContext.DomainsDistData.Where(m => m.DomainID == vmodel.Site_ID).Where(m => m.DistID == item).Where(m => m.CityID == 0).ToList();
                            dbContext.DomainsDistData.RemoveRange(oRemove);
                            dbContext.SaveChanges();
                        }
                    }
                }
                int iShowDist = 0;
                if (vmodel.bShowDistrict == true)
                {
                    iShowDist = 1;
                }
                else
                {
                    iShowDist = 0;
                }
                var Cmd = "update Domain_District_Configuration set bShowDistrict=" + iShowDist + " where DomainID=" + vmodel.Site_ID;
                Dictionary<string, object> Param = new Dictionary<string, object> { };
                var res = _database.SelectQuery(Cmd, Param);

                string CacheKey = "HomePageDomainLocations_" + vmodel.Site_ID.ToString();
                var keyItems = dbContext.CacheTransactions.Where(x => x.CacheKey == CacheKey && !x.Status).ToList();
                if (keyItems != null && keyItems.Count > 0)
                {
                    foreach (var keyItem in keyItems)
                    {
                        keyItem.Status = true;
                        keyItem.Last_Updated_DateTime = DateTime.Now;
                    }
                }

                CacheTransactions ct = new CacheTransactions();
                ct.CacheKey = CacheKey;
                ct.Type = "HomePage";
                ct.Status = false;
                ct.Last_Updated_DateTime = DateTime.Now;
                dbContext.CacheTransactions.Add(ct);
                var num = dbContext.SaveChanges();
                return Ok(1);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        [HttpPost]
        [Route("Update_Domains_Master_Data")]
        public IHttpActionResult Update_Domains_Master_Data(VMPagingResultsPost DM)
        {
            try
            {
                var oAddD = DM.oNewDomainData.ToList().FirstOrDefault();
                DomainsMasterData oDM = new DomainsMasterData();
                var iExistID = dbContext.DomainsMasterData.Where(m => m.DomainID == oAddD.DomainID).Select(m => m.ID).FirstOrDefault();
                if (iExistID > 0)
                {
                    oDM = dbContext.DomainsMasterData.Find(iExistID);
                    if (oDM == null)
                    {
                        oDM = new DomainsMasterData();
                    }
                }
                if (oAddD != null)
                {
                    oDM.DomainID = oAddD.DomainID;
                    oDM.bMinShopValue = oAddD.bMinShopValue;
                    oDM.iMinShopValue = oAddD.iMinShopValue;
                    oDM.sAlertMessage = oAddD.sAlertMessage;
                    oDM.bDeliveryCharges = oAddD.bDeliveryCharges;
                    oDM.iDCValue1 = oAddD.iDCValue1;
                    oDM.iDCValue2 = oAddD.iDCValue2;
                    oDM.sContactNumber = oAddD.sContactNumber;
                    oDM.sAlternateContactNumber = oAddD.sAlternateContactNumber;
                    oDM.sEmail = oAddD.sEmail;
                    oDM.sAddress = oAddD.sAddress;
                    oDM.sHeaderMessage = oAddD.sHeaderMessage;
                    //oDM.sLogo = "";
                    oDM.sLogoTagLine = oAddD.sLogoTagLine;
                    oDM.sPageTitle = oAddD.sPageTitle;
                    oDM.sPaymentGateway = oAddD.sPaymentGateway;
					oDM.iDCDaysFrom = oAddD.iDCDaysFrom;
                    oDM.iDCDaysTo = oAddD.iDCDaysTo;
                    oDM.iVDDaysFrom = oAddD.iVDDaysFrom;
                    oDM.iVDDaysTo = oAddD.iVDDaysTo;
                    oDM.iWHDaysFrom = oAddD.iWHDaysFrom;
                    oDM.iWHDaysTo = oAddD.iWHDaysTo;
                    oDM.iShowLocationType = oAddD.iShowLocationType;
                    if (iExistID == 0)
                    {
                        dbContext.DomainsMasterData.Add(oDM);
                    }
                    dbContext.SaveChanges();
                    iExistID = oDM.ID;
                    if (iExistID > 0)
                    {
                        if (oDM.sLogo == null || oDM.sLogo == "")
                        {
                            if (oAddD.sLogo != null || oAddD.sLogo != "")
                            {
                                oDM.sLogo = iExistID + "." + oAddD.sLogo;
                            }
                            oDM.sLogo = oDM.sLogo;
                        }
                        dbContext.SaveChanges();
                    }
                }

                string CacheKey = "HomePageDomainMasterData_" + oAddD.DomainID.ToString();
                var keyItems = dbContext.CacheTransactions.Where(x => x.CacheKey == CacheKey && !x.Status).ToList();
                if (keyItems != null && keyItems.Count > 0)
                {
                    foreach (var keyItem in keyItems)
                    {
                        keyItem.Status = true;
                        keyItem.Last_Updated_DateTime = DateTime.Now;
                    }
                }

                CacheTransactions ct = new CacheTransactions();
                ct.CacheKey = CacheKey;
                ct.Type = "HomePage";
                ct.Status = false;
                ct.Last_Updated_DateTime = DateTime.Now;
                dbContext.CacheTransactions.Add(ct);
                var num = dbContext.SaveChanges();

                return Ok(iExistID);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        [HttpGet]
        [Route("Get_All_Saved_Master_Data")]
        public IHttpActionResult Get_All_Saved_Master_Data(int iDomainID)
        {
            try
            {
                string sKey = "DomainMasterData_" + iDomainID.ToString();
                if (HttpRuntime.Cache[sKey] == null)
                {
                    var Cmd = "SELECT * FROM Domains_Master_Configuration where DomainID=" + iDomainID;
                    Dictionary<string, object> Param = new Dictionary<string, object> { };
                    var res = _database.SelectQuery(Cmd, Param);
                    HttpRuntime.Cache.Add(sKey, res, null, DateTime.Now.AddHours(1), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                    return Ok(res);
                }
                else
                {
                    var res = HttpRuntime.Cache[sKey];
                    return Ok(res);
                }
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        [HttpGet]
        [Route("Get_All_Mapped_Dist")]
        public IHttpActionResult Get_All_Mapped_Dist(int iDomainID)
        {
            try
            {
                //string sKey = "DistrictMapping_" + iDomainID.ToString();
                //if (HttpRuntime.Cache[sKey] == null)
                //{
                var Cmd = "SELECT DistID,CityID FROM Domain_District_Configuration where DomainID=" + iDomainID;
                Dictionary<string, object> Param = new Dictionary<string, object> { };
                var res = _database.SelectQuery(Cmd, Param);
                //HttpRuntime.Cache.Add(sKey, res, null, DateTime.Now.AddHours(1), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                return Ok(res);
                //}
                //else
                //{
                //var res = HttpRuntime.Cache[sKey];
                //return Ok(res);
                //}
                //return null;
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        [HttpGet]
        [Route("Get_All_Mapped_Dist_CityID_Zero")]
        public IHttpActionResult Get_All_Mapped_Dist_CityID_Zero(int iDomainID)
        {
            try
            {
                //string sKey = "DistrictMapping_" + iDomainID.ToString();
                //if (HttpRuntime.Cache[sKey] == null)
                //{
                var Cmd = "SELECT DistID FROM Domain_District_Configuration where DomainID=" + iDomainID + " and CityID=0";
                Dictionary<string, object> Param = new Dictionary<string, object> { };
                var res = _database.SelectQuery(Cmd, Param);
                //HttpRuntime.Cache.Add(sKey, res, null, DateTime.Now.AddHours(1), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                return Ok(res);
                //}
                //else
                //{
                //var res = HttpRuntime.Cache[sKey];
                //return Ok(res);
                //}
                //return null;
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        [HttpGet]
        [Route("GetCitiesByDistID")]
        public IHttpActionResult GetCitiesByDistID(int iDomainID, int iDistID)
        {
            try
            {
                var Cmd = "SELECT * FROM iH_Master_Locations where Location_Hierarchy_Level=3 and ParentID=" + iDistID;
                Dictionary<string, object> Param = new Dictionary<string, object> { };
                var res = _database.SelectQuery(Cmd, Param);
                return Ok(res);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        [HttpPost]
        [Route("Update_City_OnDist")]
        public IHttpActionResult Update_City_OnDist(int Site_ID, int iNDistID, int iCityID)
        {
            try
            {
                DomainsDistConfiguration oDM = new DomainsDistConfiguration();
                var iExists = dbContext.DomainsDistData.Where(m => m.DomainID == Site_ID).Where(m => m.DistID == iNDistID).Select(m => m.ID).FirstOrDefault();
                if (iExists > 0)
                {
                    oDM = dbContext.DomainsDistData.Find(iExists);
                    oDM.CityID = iCityID;
                    dbContext.SaveChanges();
                }
                else
                {
                    oDM.DomainID = Site_ID;
                    oDM.DistID = iNDistID;
                    oDM.CityID = iCityID;
                    dbContext.DomainsDistData.Add(oDM);
                    dbContext.SaveChanges();
                }
                return Ok(1);
            }
            catch (Exception ex)
            {
                log.Error("Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(0);
            }
        }

        [HttpPost]
        [Route("Upload_Domains_Master_Logo")]
        public IHttpActionResult Upload_Domains_Master_Logo(int ID, int iSiteID)
        {
            try
            {
                HttpPostedFile Photo = HttpContext.Current.Request.Files[0];
                var image = Photo.ContentType.Split('/');
                var imagetype = image[1];
                if (imagetype == "jpeg")
                {
                    imagetype = "jpg";
                }
                string str = string.Empty;
                string type = string.Empty;
                string physicalPath = HttpContext.Current.Server.MapPath("~/" + "Images/" + iSiteID + "_Logo/");
                if (!System.IO.File.Exists(physicalPath))
                {
                    Directory.CreateDirectory(physicalPath);
                }
                string imgPath = HttpContext.Current.Server.MapPath("~/" + "images/" + iSiteID + "_Logo/" + ID + "." + imagetype);
                FileInfo file = new FileInfo(imgPath);
                if (file.Exists.Equals(true))
                {
                    System.IO.File.Delete(imgPath);
                }
                Photo.SaveAs(imgPath);
                CustomResponse cs = new CustomResponse();
                cs.Response = "Image Uploaded Successfully...!";
                cs.ResponseID = Convert.ToInt32(ID);
                return Ok(cs);
            }
            catch (Exception ex)
            {
                log.Error("Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        [HttpPost]
        [Route("ClearCache")]
        public IHttpActionResult ClearCache()
        {
            try
            {
                IDictionaryEnumerator cacheEnumerator = HttpContext.Current.Cache.GetEnumerator();
                while (cacheEnumerator.MoveNext())
                {
                    HttpContext.Current.Cache.Remove(cacheEnumerator.Key.ToString());
                }
                return null;
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        #endregion DomainsMasteDataCode

        #region CacheManagement

        //[HttpGet]
        //[Route("Get_CacheList")]
        //public IHttpActionResult Get_CacheList()
        //{
        //    try
        //    {
        //        VM_CacheData result = new VM_CacheData();
        //        List<VM_CacheConfig> Model = new List<VM_CacheConfig>();
        //        var cacheobj = HttpRuntime.Cache.GetEnumerator();
        //        while (cacheobj.MoveNext())
        //        {
        //            var pair = (DictionaryEntry)cacheobj.Current;
        //            {
        //                using (Stream s = new MemoryStream())
        //                {
        //                    string json = JsonConvert.SerializeObject(pair.Value, Newtonsoft.Json.Formatting.Indented);
        //                    BinaryFormatter formatter = new BinaryFormatter();
        //                    formatter.Serialize(s, json);
        //                    Model.Add(new VM_CacheConfig { sKey = pair.Key.ToString(), Size = (s.Length / 1024), CacheType = EnumCacheTypes.Application });
        //                }
        //            }
        //        }
        //        result.CacheList = Model.OrderByDescending(s => s.Size).ToList();
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        CustomResponse cs = new CustomResponse();
        //        cs.Response = ex.Message;
        //        return Ok(cs);
        //    }
        //}

        [HttpPost]
        [Route("RemoveKeyfromCache")]
        public IHttpActionResult RemoveKeyfromCache(string sKey)
        {
            try
            {
                if (!string.IsNullOrEmpty(sKey))
                {
                    if (HttpRuntime.Cache[sKey] != null)
                    {
                        HttpRuntime.Cache.Remove(sKey);
                    }
                }
                return Ok(1);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        #endregion CacheManagement


        [HttpPost]
        [Route("Save_MAILSMS_Master_Data")]
        public IHttpActionResult Save_MAILSMS_Master_Data(VMPagingResultsPost DM)
        {
            try
            {
                var oAddD = DM.oNewMailSMSData.ToList().FirstOrDefault();
                MailSMSSettings oDM = new MailSMSSettings();
                var iExistID = dbContext.MailSMSSettings.Where(m => m.iDomainID == oAddD.iDomainID).Where(m => m.sType == oAddD.sType).Where(m => m.sTemplateType == oAddD.sTemplateType).Select(m => m.ID).FirstOrDefault();
                if (iExistID > 0)
                {
                    oDM = dbContext.MailSMSSettings.Find(iExistID);
                    if (oDM == null)
                    {
                        oDM = new MailSMSSettings();
                    }
                }
                if (oAddD != null)
                {
                    oDM.iDomainID = oAddD.iDomainID;
                    oDM.sTemplateType = oAddD.sTemplateType;
                    oDM.sType = oAddD.sType;
                    oDM.sSubject = oAddD.sSubject;
                    oDM.sFrom = oAddD.sFrom;
                    oDM.sTrackhere = oAddD.sTrackhere;
                    oDM.sHelpcenter = oAddD.sHelpcenter;
                    oDM.sMobileNumber = oAddD.sMobileNumber;
                    oDM.sEmail = oAddD.sEmail;
                    oDM.sBody = oAddD.sBody;
                    oDM.SMS = oAddD.SMS;
                    oDM.MAIL = oAddD.MAIL;
                    if (iExistID == 0)
                    {
                        dbContext.MailSMSSettings.Add(oDM);
                    }
                    dbContext.SaveChanges();
                }
                return Ok(iExistID);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }


        [HttpGet]
        [Route("Get_All_Saved_MAILSMS_Data")]
        public IHttpActionResult Get_All_Saved_MAILSMS_Data(int iDomainID, string sType, string sTempType)
        {
            try
            {
                MailSMSSettings oSMData = new MailSMSSettings();
                string sKey = "DomainSMData_" + sType + "_" + iDomainID.ToString() + "_" + sTempType;
                if (HttpRuntime.Cache[sKey] == null)
                {
                    oSMData = dbContext.MailSMSSettings.Where(m => m.iDomainID == iDomainID).Where(m => m.sTemplateType == sTempType).Where(m => m.sType == sType).ToList().FirstOrDefault();
                    HttpRuntime.Cache.Add(sKey, oSMData, null, DateTime.Now.AddHours(1), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                    return Ok(oSMData);
                }
                else
                {
                    var res = HttpRuntime.Cache[sKey];
                    return Ok(res);
                }
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs.Response.FirstOrDefault());
            }
        }

        //Removing Logo for Domain,if exists//
        [HttpPost]
        [Route("RemoveDomainLogos")]
        public IHttpActionResult RemoveDomainLogos(int iDomainID)
        {
            try
            {
                DomainsMasterData oD = new DomainsMasterData();
                oD = dbContext.DomainsMasterData.Where(m => m.DomainID == iDomainID).ToList().FirstOrDefault();
                oD.sLogo = "";
                dbContext.SaveChanges();
                return Ok(1);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        //===================Get ProductList With Location by SKU /ProductName======================//
        [HttpPost]
        [Route("GetProductListWithBinLocation_BySKUApp")]
        public IHttpActionResult GetProductListWithBinLocation_BySKUApp(VMModelForProductCategoryUnitBinLocation Cat)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                                                                                           { "@sku", Cat.SKU },
                                                                                           { "@unitid", Cat.UnitID},
                                                                                           { "@productname", Cat.ProductName },
                                                                                            { "@productid", Cat.ProductID },
                                                                                           { "@categoryid", Cat.CategoryID },
                                                                                           { "@pagesize", Cat.PageSize },
                                                                                           { "@pageindex", Cat.PageIndex },
                                                                                           { "@barcode", Cat.Barcode }
                };
                var product_Get_List = _database.GetMultipleResultsList(Get_Productslist_BinlocationApp, parameters);
                return Ok(product_Get_List);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Get_Category_Attributes---Error---------", ex);
                return Ok(cs);
            }
        }

        //=================>>>>>>>>>>> Update FMCG Data <<<<<<<<<<<<<=========================//
        [HttpPost]
        [Route("Close_EMI_Manually")]
        public IHttpActionResult Close_EMI_Manually(VMModelsForOrderManagement gt)
        {
            try
            {

                Dictionary<string, object> param = new Dictionary<string, object>() { { "@otp", gt.OTP },
                                                                                        { "@notification_ID", gt.Notification_ID } };
                var validate_result = _database.QueryValue(Validate_OTP, param);
                int userID = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());
                if (Convert.ToBoolean(validate_result))
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>() {
                    { "@OrderId", gt.OrderID },
                    { "@TotalPaidAmount",gt.TotalPaidAmount },
                    { "@Notes", gt.Notes },
                    { "@otp", gt.OTP },
                    { "@userID", userID },
                    };
                    var res = _database.QueryValue(Close_EMI, parameters);
                    return Ok(res);
                }
                else
                {
                    return Ok(0);
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

        //=================>>>>>>>>>>> Update FMCG Data <<<<<<<<<<<<<=========================//
        [HttpPost]
        [Route("Make_EMI_Payment")]
        public IHttpActionResult Make_EMI_Payment(VMModelsForOrderManagement gt)
        {
            try
            {

                Dictionary<string, object> param = new Dictionary<string, object>() { { "@otp", gt.OTP },
                                                                                        { "@notification_ID", gt.Notification_ID } };
                var validate_result = _database.QueryValue(Validate_OTP, param);
                int userID = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());
                if (Convert.ToBoolean(validate_result))
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>() {
                    { "@OrderId", gt.OrderID },
                    { "@TotalPaidAmount",gt.TotalPaidAmount },
                    //{ "@Notes", gt.Notes },
                    //{ "@otp", gt.OTP },
                    { "@userID", userID },
                    };
                    var res = _database.QueryValue(MakeEMIPayment, parameters);
                    return Ok(res);
                }
                else
                {
                    return Ok(0);
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

        [HttpPost]
        [Route("Update_Customer_Special_Requested")]
        public IHttpActionResult Update_Customer_Special_Requested(int RequestedID, int iValue, string sType)
        {
            try
            {
                var oCP = dbContext.Customer_Special_Requested_Products.Where(m => m.Customer_Requested_ID == RequestedID).ToList().FirstOrDefault();
                if (oCP != null)
                {
                    if (sType == "C")
                    {
                        oCP.iCategoryID = iValue;
                    }
                    else
                    {
                        oCP.Customer_Request_Status = iValue;
                    }
                    oCP.Last_Updated_Time = DateTime.Now;
                    dbContext.SaveChanges();
                }
                return Ok(RequestedID);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error:While updating the iCategoryID Column in iH_Customer_Special_Requested_Products Table", ex);
                return Ok(cs);
            }
        }

        [HttpPost]
        [Route("Get_Customer_Special_Requested_Products")]
        public IHttpActionResult Get_Customer_Special_Requested_Products(VMModelsForOrderManagement VM)
        {
            try
            {
                if (VM.Status == 200)
                {
                    VM.Status = 10;
                    Get_SpecialRequest_Data = "iAdmin_Get_Customer_Special_Requests_ProUserIDs";
                    Dictionary<string, object> parameters = new Dictionary<string, object>() { {"@customer_name",VM.Name},{ "@mobile_number", VM.MobileNumber}, {"@product_name",VM.ProductName},
                   {"@unit_name",VM.StoreName },{"@requested_date",VM.OrderDateFrom },{ "@status", VM.Status }, { "@Page_Size", VM.PageSize },
                    { "@Page_Index", VM.PageIndex } ,{ "@RequestID", VM.Customer_Requested_ID },{ "@sCategoryName", VM.CategoryName },{ "@iCatNotMapped", VM.iCatNotMapped },{ "@ProUserIDs", VM.sUserIDs }};
                    var Result = _database.GetMultipleResultsList(Get_SpecialRequest_Data, parameters);
                    return Ok(Result);
                }
                else
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>() { {"@customer_name",VM.Name},{ "@mobile_number", VM.MobileNumber}, {"@product_name",VM.ProductName},
                   {"@unit_name",VM.StoreName },{"@requested_date",VM.OrderDateFrom },{ "@status", VM.Status }, { "@Page_Size", VM.PageSize },
                    { "@Page_Index", VM.PageIndex } ,{ "@RequestID", VM.Customer_Requested_ID },{ "@sCategoryName", VM.CategoryName },{ "@iCatNotMapped", VM.iCatNotMapped }};
                    var Result = _database.GetMultipleResultsList(Get_SpecialRequest_Data, parameters);
                    return Ok(Result);
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
//=================>>>>>>>>>>> Cancel_Stock_Requested_Products_On_Request <<<<<<<<<<<<<<<<========================//
        [HttpPost]
        [Route("Cancel_Stock_Req_Products")]
        public IHttpActionResult Cancel_Stock_Req_Products(VMPagingResultsPost vmodal)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());
                string Cancel_Stock_Requested_Products = "[iAdmin_Cancel_Stock_Requested_Products]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@product_id", vmodal.Productid },
                                                                                            { "@quantity", vmodal.Quantitiy },
                                                                                            { "@UserId", userID },
                                                                                            { "@RequestId", vmodal.RequestId },
                                                                                            { "@Refundamt", vmodal.Amount }, 
                                                                                            { "@notes", vmodal.Message },
                                                                                            { "@unitid", vmodal.Unit_ID }};
                var res = _database.QueryValue(Cancel_Stock_Requested_Products, parameters);
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
	 //=================>>>>>>>>>>> Modify_Ordered_Products <<<<<<<<<<<<<<<<========================//
        [HttpPost]
        [Route("Modify_Ordered_Products")]
        public IHttpActionResult Modify_Ordered_Products(VMPagingResultsPost vmodal)
        {
            try
            {
                string Modify_Ordered_Products= "[iAdmin_Modify_Ordered_Products]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Ordernumber", vmodal.OrderID },
                                                                                           { "@Oldproductid", vmodal.Productid } ,
                                                                                           { "@Newproductid", vmodal.productid },
                                                                                           { "@OrderedQty", vmodal.Count },
                                                                                           { "@NewQty", vmodal.Quantitiy },
                                                                                           { "@NewQtySP", vmodal.Amount },
                                                                                           { "@DifferenceAmount", vmodal.Bonus_Amount },
                                                                                           { "@PaidBy", vmodal.Notification_ID },
                                                                                           { "@CustomerPayingAmount", vmodal.OfferValue },
                                                                                           { "@AddToWallet", vmodal.ID }};
                var res = _database.QueryValue(Modify_Ordered_Products, parameters);
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
        //===================================Get IMEI units================== 
        [HttpPost]
        [Route("Get_IMEI_Products_ByUnitID")]
        public IHttpActionResult Get_IMEI_Products_ByUnitID(FMCG gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                    { "@SKU",gt.SKU },{ "@ProductName",gt.ProductName },{ "@Catogory",gt.LeafCategory }, { "@PageSize", gt.pagesize },{ "@PageIndex", gt.pageindex } };
                var res = _database.GetMultipleResultsList(Get_UnitIds_Having_SerialNo, parameters);
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
        //===================== Get IMEI Units=================
        [HttpPost]
        [Route("Get_Product_SerialNo_ByUnitID")]
        public IHttpActionResult Get_Product_SerialNo_ByUnitID(VMModelsForInventory s)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@unitid", s.Unit_ID } };
                var result = _database.ProductListWithCount(Get_Product_serialNo_By_UnitId, parameters);
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
        //===================== Update IMEI Units=================
        [HttpPost]
        [Route("Update_IMEI_Details_ByUnitID")]
        public IHttpActionResult Update_IMEI_Details_ByUnitID(VMModelsForInventory s)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ProductIds_Qty", s.FilterJson } };
                var result = _database.ProductListWithCount(Update_serialNo, parameters);
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
        //===================================Get Tray units================== 
        [HttpPost]
        [Route("Available_Trays_By_Unit_Id")]
        public IHttpActionResult Available_Trays_By_Unit_Id(Traylist gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                    { "@UnitID",gt.Unit_ID },{ "@Condition",gt.Condition },{ "@TrayValue",gt.Quantity },{"@DateFrom",gt.FromDate },
                    { "@DateTo",gt.ToDate}, { "@PageSize", gt.pagesize }, { "@PageIndex", gt.pageindex } };
                var res = _database.GetMultipleResultsList(Get_Tray_Reords, parameters);
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
        //=== Method To View Consignment  Transactions===//
        [HttpPost]
        [Route("View_Transaction_Details")]
        public IHttpActionResult View_Transaction_Details(Traylist vM)
        {
            try
            {

                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@TransactionType",vM.TransactionType },
                                                                                           { "@DriverName",vM.DriverName },
                                                                                           { "@DateFrom", vM.FromDate },
                                                                                           { "@DateTo", vM.ToDate },
                                                                                           { "@UserID", vM.Unit_ID }};
                var result = _database.GetMultipleResultsList(Tray_Transactions, parameters);
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
        //=== Method To HandOver Consignment ===//
        [HttpPost]
        [Route("Update_HandOver_Details")]
        public IHttpActionResult Update_HandOver_Details(Traylist vM)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@TraysCount",vM.TrayCount },
                                                                                           { "@HandOverDate",vM.FromDate },
                                                                                           { "@DriverName", vM.DriverName },
                                                                                           { "@RemarksDeatils", vM.Notes },
                                                                                           { "@UserID", vM.Unit_ID } };
                var result = _database.ProductListWithCount(HandOver_Trays, parameters);
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
	[HttpPost]
        [Route("Update_Cities_OnDist")]
        public IHttpActionResult Update_Cities_OnDist(VMUpdateCitiesOnDistrict DM)
        {
            try
            {
                string UpdateCities = "iAdmin_Update_Cities_Distict_Dist_Configuration";
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                                                                                           { "@Site_ID", DM.Site_ID },
                                                                                           { "@Dist_ID", DM.Dist_ID},
                                                                                           { "@City_IDs", DM.City_IDs },
                                                                                           {"@Delete_IDs", DM.Delete_IDs }
                };
                var res = _database.QueryValue(UpdateCities, parameters);
                string CacheKey = "HomePageDomainLocations_" + DM.Site_ID.ToString();
                var keyItems = dbContext.CacheTransactions.Where(x => x.CacheKey == CacheKey && !x.Status).ToList();
                if (keyItems != null)
                {
                    foreach (var keyItem in keyItems)
                    {
                        keyItem.Status = true;
                        keyItem.Last_Updated_DateTime = DateTime.Now;
                    }
                }

                CacheTransactions ct = new CacheTransactions();
                ct.CacheKey = CacheKey;
                ct.Type = "HomePage";
                ct.Status = false;
                ct.Last_Updated_DateTime = DateTime.Now;
                dbContext.CacheTransactions.Add(ct);
                var num = dbContext.SaveChanges();
                return Ok(res);

            }
            catch (Exception ex)
            {
                log.Error("Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(0);
            }
        }

        //API to get IMEI details of the Product while Consignment Creation
        [HttpGet]
        [Route("Get_Consignment_ProductIMEI_DCApp")]
        public IHttpActionResult Get_Consignment_ProductIMEI_DCApp(int OrderID, string ProductID,int UnitID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@OrderID", OrderID }, { "@ProductID", ProductID }, { "@UnitID", UnitID }   };
                var res = _database.ProductListWithCount(Get_Consignment_ProductIMEI, parameters);
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


        //============================================Get Store WareHouse Orders=====================================
        [HttpPost]
        [Route("UploadFranchiseLocationsWithPincodes")]
        public IHttpActionResult UploadFranchiseLocationsWithPincodes()
        {

            try
            {
                HttpPostedFile file = HttpContext.Current.Request.Files[0];
                var pathToExcel = file.FileName;
                var Dictionary = new List<string>();
                string str = "";
                IExcelDataReader reader = null;
                List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();
                Dictionary<string, string> product;
                string xlfilename = file.FileName;
                var random = new Random(System.DateTime.Now.Millisecond);
                var randomNumber = random.Next(1, 5000000);

                using (var stream = file.InputStream)
                {

                    if (file.FileName.EndsWith(".xls"))
                    {
                        reader = ExcelReaderFactory.CreateBinaryReader(stream); //ExcelReaderFactory thowing an error but not an issue
                    }
                    else if (file.FileName.EndsWith(".xlsx"))
                    {
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }
                    else
                    {
                        CustomResponse cr = new CustomResponse();
                        cr.Response = "Invalid file format";
                        cr.ResponseID = -1;
                        log.Info("Invalid file format");
                        return Ok(cr);
                    }

                    DataSet result = reader.AsDataSet();
                    reader.Close();
                    DataTable data = result.Tables[0];
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    var excelColdata = new List<string>();
                    List<int> ColumnIndexes = new List<int>();
                    List<List<string>> sListOfRecords = new List<List<string>>();
                    int i = 1;
                    foreach (var column in data.Rows[0].ItemArray.Where(m => m is string))
                    {
                        string ColumnName = column.ToString();
                        ColumnName.Replace("'", " ");

                        Dictionary.Add(ColumnName);
                    }
                    foreach (DataRow row in data.Rows)
                    {
                        var excelRowdata = new List<string>();
                        if (i == 1)
                        {
                            int j = 1;
                            foreach (var item in row.ItemArray)
                            {
                                if (Dictionary.Contains(item.ToString()))
                                {
                                    excelRowdata.Add(item.ToString());
                                    ColumnIndexes.Add(j);
                                }
                                j++;
                            }
                            if (excelRowdata.Where(m => !string.IsNullOrWhiteSpace(m)).Count() > 0)
                            {
                                sListOfRecords.Add(excelRowdata);
                            }
                        }
                        else
                        {
                            // int productid = 0;
                            string Village_Name = "";
                            string Mandal_Name = "";
                            string District_Name = "";
                            string State_Name = "";
                            int Pin_code = 0;
                            int j = 1;
                            product = new Dictionary<string, string>();
                            if (Convert.ToString(row.ItemArray[0]) == string.Empty) { break; }
                            foreach (var item in row.ItemArray)
                            {
                                if (ColumnIndexes.Contains(j))
                                {

                                    if (sListOfRecords[0][j - 1].TrimStart().TrimEnd() == "VillageName")
                                    {
                                        Village_Name = item.ToString().TrimStart().TrimEnd();
                                    }

                                    if (sListOfRecords[0][j - 1].TrimStart().TrimEnd() == "MandalName")
                                    {
                                        Mandal_Name = item.ToString().TrimStart().TrimEnd();
                                    }
                                    if (sListOfRecords[0][j - 1].TrimStart().TrimEnd() == "DistrictName")
                                    {
                                        District_Name = item.ToString().TrimStart().TrimEnd();
                                    }
                                    if (sListOfRecords[0][j - 1].TrimStart().TrimEnd() == "StateName")
                                    {
                                        State_Name = item.ToString().TrimStart().TrimEnd();
                                    }
                                    if (sListOfRecords[0][j - 1].TrimStart().TrimEnd() == "PinCode")
                                    {
                                        Pin_code = Convert.ToInt32((item.ToString().TrimStart().TrimEnd()));
                                    }
                                }
                                j++;
                            }
                            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Village_Name", Village_Name }, { "@Mandal_Name", Mandal_Name }, { "@District_Name", District_Name }, { "@State_Name", State_Name }, { "@Pin_code", Pin_code } };
                            var response = _database.QueryValue(Add_Franchise_Master_Location_Data, parameters);
                        }
                        i++;
                    }
                    return Ok("Add Successfully");
                }
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }
        //---------Ramya
        //=================>>>>>>>>>>> Cancel_Ordered_Products_On_Request <<<<<<<<<<<<<<<<========================//
        [HttpPost]
        [Route("Cancel_Ordered_Products_On_Request")]
        public IHttpActionResult Cancel_Ordered_Products_On_Request(VMPagingResultsPost vmodal)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>() { { "@otp", vmodal.Otp },
                                                                                        { "@notification_ID", vmodal.Notification_ID } };
                var validate_result = _database.QueryValue(Validate_OTP, param);
                int userID = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());
                if (Convert.ToBoolean(validate_result))
                {
                    if (vmodal.Flag == 1)
                    {
                        Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@order_id", vmodal.OrderID },
                                                                                                { "@product_id", vmodal.Productid } ,
                                                                                                 { "@UserId", userID },
                                                                                                 {"@NewRefund",vmodal.NewRefund  },
                                                                                                        {"@Notes",vmodal.Notes  }
                        };
                        var res = _database.QueryValue(To_Cancel_Ordered_Products_On_Request, parameters);
                        return Ok(res);
                    }
                    else
                    {
                        Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@order_id", vmodal.OrderID },
                                                                                                     { "@product_id", vmodal.Productid } ,
                                                                                                 { "@UserId", userID },
                                                                                                  {"@NewRefund",vmodal.NewRefund  },
                                                                                                   {"@Notes",vmodal.Notes  }
                                                                                                    };
                        var res = _database.QueryValue(To_Cancel_Order_On_Request, parameters);
                        return Ok(res);
                    }
                }
                else
                {
                    return Ok(0);
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
        

       // =========================================Added on 26Mar21 to Map all the Domains to Product(s) or Leaft Categorywise products =========================================//

        [HttpPost]
        [Route("MapDomains_To_Products")]

        public IHttpActionResult MapDomains_To_Products(string ProductIDs, int CategoryID, string DomainListCond)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Productids", ProductIDs }, { "@CategoryID", CategoryID }, { "@DomainIds", DomainListCond } };
                var res = _database.QueryValue(MapDomains_toProducts, parameters);
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
        [HttpPost]
        [Route("Get_Searchbased_PackagesBundles")]
        public IHttpActionResult Get_Searchbased_PackagesBundles(DomainsData Packagemodel)
        {
            try
            {

                Dictionary<string, object> parameter1 = new Dictionary<string, object> { { "@Type", Packagemodel.Type }, { "@DomainID", Packagemodel.DomainID }, { "@PackageName", Packagemodel.PackageName }, { "@Status", Packagemodel.Status }, { "@searchFlag", Packagemodel.searchFlag }, { "@PageSize", Packagemodel.Pagesize }, { "@PageIndex", Packagemodel.PageIndex } };
                var res = _database.ProductListWithCount(Get_All_iHub_PackagesBundlesList, parameter1);
                return Ok(res);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }
        [Route("Assign_Orders_To_Logistics")]
        public IHttpActionResult Assign_Orders_To_Logistics(VMPagingResultsPost GF)
        {
            try
            {
                string assign_Logistic = "[iB_Assign_Order_To_Logistics]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() {{ "@ordered_product_Id", GF.Order_productID }, { "@master_id", GF.MasterID },
                                { "@isThirdParty",GF.IsThirdParty },{ "@trackingid",GF.TrackingVechlNo }};
                var res = _database.QueryValue(assign_Logistic, parameters);
                return Ok(res);
                //return Ok(1);

            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }
        #endregion "END of -- ProductAndPrice"
    }
}
