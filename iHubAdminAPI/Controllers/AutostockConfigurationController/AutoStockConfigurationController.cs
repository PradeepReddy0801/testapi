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
using iHubAdminAPI.Models.AutoStockModelandViewModels;
using AspNet.Identity.SQLDatabase;
using System.Configuration;
using System.Data.SqlClient;

namespace iHubAdminAPI.Controllers
{
    [RoutePrefix("api/AutoStockConfig")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AutoStockConfigurationController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MasterDataAndUnitsController).FullName);
        public SQLDatabase _database;
        

        public AutoStockConfigurationController()
        {
            _database = new SQLDatabase();
        }
        private class CustomResponse
        {
            internal string Response;
            internal int ResponseID;
        }

        #region "BEGIN of -- SP Names"
        // =================================Command Rext For Api Calls====================================================
        string RecordsList = "[iAdmin_Get_Table_Records_By_Table_Name]";
        string RecordsListPaging = "[iAdmin_Auto_Get_Table_Records_By_Table_Name_With_Paging]";
        string MasterList = "[iAdmin_Auto_Get_Master_Class_With_Paging]";
        string SaveRecordsList = "[iAdmin_Auto_Insert_New_Unit_Class]";
        string UpdateRecordsList = "[iAdmin_Auto_Update_Master_Class]";
        string Main_Category = "[iAdmin_Auto_Get_Main_Categorys_By_Class_Id]";
        string updateRecordsList = "[iAdmin_Auto_Update_Main_Category_Class_Details]";
        string LeafCategory = "[iAdmin_Auto_Get_Leaf_Categorys_By_Class_Id_Parent_Cat_Id]";
        string Update_Leaf_Category = "[iAdmin_Auto_Update_Leaf_Category_Class_Details]";
        string Unit_Configuration_Types = "[iAdmin_Auto_Get_Unit_Stock_Configuration_Types]";
        string Update_Unit_Configuration = "[iAdmin_Auto_Update_Unit_Configuration_Type_Details]";
        string Get_Parent_Product_Config = "[iAdmin_Auto_Get_Parent_Product_Config_Details_By_Class_Id_Cat_Id]";
        string Create_Product_Parent_Config = "[iAdmin_Auto_Create_Product_Parent_Config_Details]";
        string Update_Product_Parent_Config = "[iAdmin_Auto_Update_Product_Parent_Config_Details]";
        string Remove_Parent_Product = "[iAdmin_Auto_Remove_Parent_Product_Config_Details]";
        string Auto_Get_Products_By_Class = "[iAdmin_Auto_Get_Products_By_Class_Id_Cat_Id]";
        string Auto_Update_Unit_Product = "[iAdmin_Auto_Update_Unit_Product_Details]";
        string Auto_Bulk_Remove_Unit = "[iAdmin_Auto_Bulk_Remove_Unit_Products]";
        string Auto_Get_Unassign_Products = "[iAdmin_Auto_Get_Unassign_Products_By_Class_Id_Cat_Id]";
        string Auto_Assign_Products_To_Unit_Classes = "[iAdmin_Auto_Assign_Products_To_Unit_Clasas]";
        string Get_Attribute_Parent_Config_Details_By_Class = "[iAdmin_Auto_Get_Attribute_Parent_Config_Details_By_Class_Id_Cat_Id]";
        string UpdateAttributeConfiguration = "[iAdmin_Auto_Update_Attributes_Config]";
        string Get_Dynamic_Category_Attributes = "[iAdmin_Auto_Get_Dynamic_Category_Attributes]";
        string Add_New_Attributes = "[iAdmin_Auto_Add_New_Attributes_By_CategoryId]";
        string Get_Configured_Attribute = "[iAdmin_Auto_Get_Configured_Attribute_Products]";
        string CategoryId_And_Filters = "[iAdmin_Auto_Get_Products_By_CategoryId_And_Filters]";
        string Auto_Remove_Configuration = "[iAdmin_Auto_Remove_Configuration_attribute]";
        string GetAutostock = "[iAdmin_iA_Auto_Create_New_Attribute_Priority]";
        string Attributes_By_ConfigurationId = "[iAdmin_Auto_Get_All_Attributes_By_ConfigurationId]";
        string Remove_Attribute_Priority = "[iAdmin_Auto_Remove_Attribute_Priority]";
        string Unassigned_Unit_Class = "[iAdmin_Auto_Get_Unassigned_Unit_Class_List]";
        string Unit_ClassID_To_Units = "[iAdmin_Auto_Assign_Unit_ClassID_To_Units]";
        string Remove_Unit_Class_Id = "[iAdmin_Auto_Remove_Unit_Class_Id]";
        string Units_List_Bugget_Wise = "[iAdmin_Auto_Get_Units_List_Bugget_Wise]";
        string Unit_Main_Category_wise_Bugget_List = "[iAdmin_Auto_Get_Unit_Main_Category_wise_Bugget_List]";
        string Unit_Leaf_Category_wise_Bugget_List = "[iAdmin_Auto_Get_Unit_Leaf_Category_wise_Bugget_List]";
        string Product_Stock_details_List = "[iAdmin_Auto_Product_Stock_details_List]";
        string Inventory_Stock_Details_By_ProductID = "[iAdmin_Auto_Get_Inventory_Stock_Details_By_ProductID_And_UnitId]";
        string Revert_Assigned_Products = "[iAdmin_Auto_Revert_Assigned_Products_By_Inventory_Details]";
        string Remove_Products = "[iAdmin_Auto_Remove_Auto_Refill_Vendor_Products]";
        string Replace_Products = "[iAdmin_Auto_Get_Matching_Replace_Products]";
        string Auto_Replace_Assigned_Products = "[iAdmin_Auto_Replace_Assigned_Products_By_Inventory_Details]";
        string Auto_Stock_Updated_Units = "[iAdmin_Get_Auto_Stock_Updated_Units_List_By_Ref_Number]";
        string Updated_Units_Categories = "[iAdmin_Get_Auto_Stock_Updated_Units_Categories_By_Ref_Number]";
        string Auto_Stock_Updated_Units_Products = "[iAdmin_Get_Auto_Stock_Updated_Units_Products_By_Ref_Number]";
        string Revert_Stock_By_Auto_Stock = "[iAdmin_Auto_Revert_Stock_By_Auto_Stock_Ref_Number]";
        string Stock_Report_After_Job_Runs = "[iAdmin_Auto_Get_Auto_Low_Stock_Report_After_Job_Runs]";
        string DC_Wise_Low_Stock_Report = "[iAdmin_LS_Get_DC_Wise_Low_Stock_Report]";
        string LS_Get_Assigned_Main_Categories = "[iAdmin_LS_Get_Assigned_Main_Categories_By_Categories]";
        string Assigned_Stock_By_Sub_Categories = "[iAdmin_LS_Get_Assigned_Stock_By_Sub_Categories]";
        string Products_Stock_By_Leaf_Node = "[iAdmin_LS_Get_Assigned_Products_Stock_By_Leaf_Node]";
        string Unit_Product_Stock_Details = "[iAdmin_Auto_Get_Unit_Product_Stock_Details]";
        string Auto_Stock_Job_Run = "[iAdmin_Auto_Stock_Job_Run]";
        string Get_Auto_stock_data = "[iAdmin_Auto_Get_Auto_stock_data_Details]";

        #endregion "END of -- SP Names"

        #region "BEGIN of -- Auto Stock Configuration"

        //==================================To Get  Table Records============================================================
        [HttpPost]
        [Route("GetTableReocrdByTableName")]
        public IHttpActionResult GetTableReocrdByTableName(AutoStockDataMdel gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@TableName", gt.CategoryName } };
                var Cat_data = _database.ProductListWithCount(RecordsList, parameters);
                return Ok(Cat_data);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //=====================================To Get Table Records With Paging=========================================
        [HttpPost]
        [Route("GetTableReocrdByTableNameWithPaging")]
        public IHttpActionResult GetTableReocrdByTableNameWithPaging(AutoStockDataMdel gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                    { "@Table_Name", gt.CategoryName },
                    { "@Page_Index", gt.PageIndex },
                    { "@Page_Size", gt.Pagesize }
                  };
                var Cat_data = _database.ProductListWithCount(RecordsListPaging, parameters);
                return Ok(Cat_data);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //=======================================To Get Master Class===================================================
        [HttpPost]
        [Route("Get_Master_Class_With_Paging")]
        public IHttpActionResult Get_Master_Class_With_Paging(AutoStockDataMdel gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                    { "@Page_Index", gt.PageIndex },
                    { "@Page_Size", gt.Pagesize }
                  };
                var Cat_data = _database.ProductListWithCount(MasterList, parameters);
                return Ok(Cat_data);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //============================To Save Master Class=======================================
        [HttpPost]
        [Route("Save_Master_Class")]
        public IHttpActionResult Save_Master_Class(CreateNewUnitClass gt)
        {
            try
            {
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                    {"@ClassName",gt.ClassName },{ "@Description",gt.Description}
                    ,{"@Status",gt.Status },
                    {"@Budget",gt.Budget },{ "@ParentType", gt.Priority},{ "@UserID", User_Id} };
                var Cat_data = _database.QueryValuenew(SaveRecordsList, parameters);
                return Ok(Cat_data);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //===================================To Update Master Class================================
        [HttpPost]
        [Route("Update_Master_Class")]
        public IHttpActionResult Update_Master_Class(CreateNewUnitClass gt)
        {
            try
            {
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ID", gt.ID },
                  {"@ClassName",gt.ClassName },{ "@Description",gt.Description}
                ,{"@Status",gt.Status },{ "@ParentID", gt.Priority},
                    {"@Budget",gt.Budget },{ "@UserID", User_Id} };
                var Cat_data = _database.QueryValue(UpdateRecordsList, parameters);
                return Ok(Cat_data);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //=========================================To Get Main category By Class======================================
        [HttpPost]
        [Route("Get_Main_Categorys_By_Class_Id")]
        public IHttpActionResult Get_Main_Categorys_By_Class_Id(AutoStockDataMdel gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ClassId", gt.ID } };
                var Cat_data = _database.ProductListWithCount(Main_Category, parameters);
                return Ok(Cat_data);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //===================================Update Main Category Class =====================================
        [HttpPost]
        [Route("Update_Main_Category_Class")]
        public IHttpActionResult Update_Main_Category_Class(CreateNewUnitClass gt)
        {
            try
            {
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ID", gt.ID }
                , {"@Status",gt.Status },
                   {"@Budget",gt.Budget },{ "@PriorityID", gt.Priority},{ "@UserID", User_Id} };
                var Cat_data = _database.QueryValue(updateRecordsList, parameters);
                return Ok(Cat_data);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //==========================To Get Leaf category===========================================
        [HttpPost]
        [Route("Get_Leaf_Categorys_By_Class_Id_And_Parent_Cat_Id")]
        public IHttpActionResult Get_Leaf_Categorys_By_Class_Id_And_Parent_Cat_Id(AutoStockDataMdel gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ClassId", gt.ID },
                    {"@Parent_Cat_ID",gt.LocationID }};
                var Cat_data = _database.ProductListWithCount(LeafCategory, parameters);
                return Ok(Cat_data);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //===================================To Update Leaf Category=====================================
        [HttpPost]
        [Route("Update_Leaf_Category_Class")]
        public IHttpActionResult Update_Leaf_Category_Class(CreateNewUnitClass gt)
        {
            try
            {
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ID", gt.ID },
                 {"@Status",gt.Status },
                    {"@Budget",gt.Budget },
                    { "@PriorityID", gt.Priority},{ "@UserID", User_Id}
                 };
                var Cat_data = _database.QueryValue(Update_Leaf_Category, parameters);
                return Ok(Cat_data);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //====================================To Get Unit stock Configuration Types====================================
        [HttpPost]
        [Route("Get_Unit_Stock_Configuration_Types")]
        public IHttpActionResult Get_Unit_Stock_Configuration_Types(CreateNewUnitClass gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ClassId", gt.ID },
                   {"@CategoryID",gt.LocationID } };
                var Cat_data = _database.ProductListWithCount(Unit_Configuration_Types, parameters);
                return Ok(Cat_data);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //================================To Update Unit stock Configuration Types============================================
        [HttpPost]
        [Route("Update_Unit_Configuration_Type_Details")]
        public IHttpActionResult Update_Unit_Configuration_Type_Details(CreateNewUnitClass gt)
        {
            try
            {
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ID", gt.ID }
               , {"@Status",gt.Status },{ "@Prirority", gt.Priority},{ "@UserID", User_Id} };
                var Cat_data = _database.QueryValue(Update_Unit_Configuration, parameters);
                return Ok(Cat_data);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //=============================Product Configuration =============================================================
        [HttpPost]
        [Route("Get_Parent_Product_Config_Details_By_Cat_Id_Class_Id")]
        public IHttpActionResult Get_Parent_Product_Config_Details_By_Cat_Id_Class_Id(VMPagingResultsPost gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ClassId", gt.ID },
                    {"@CategoryID",gt.LocationID }};
                var Cat_data = _database.ProductListWithCount(Get_Parent_Product_Config, parameters);
                return Ok(Cat_data);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //==========================================Create Product Parent Configuration  Details==========================================
        [HttpPost]
        [Route("Create_Product_Parent_Config_Details")]
        public IHttpActionResult Create_Product_Parent_Config_Details(CreateNewUnitClass gt)
        {
            try
            {
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ClassId", gt.ID },
                    {"@CategoryID",gt.Priority },{ "@Name", gt.ClassName }
                    ,{ "@ProductSellingMinPrice", gt.ProductSellingMinPrice },{ "@ProductSellingMaxPrice", gt.ProductSellingMaxPrice }
                    ,{ "@ProductSellingMaxQuantity", gt.ProductSellingMaxQuantity }
                    ,{ "@IsAssignSameProduct", gt.Status },{ "@UserID",User_Id },{ "@StockMaxQuantity",gt.DCStockAlertQuantity }
                };
                var Cat_data = _database.QueryValue(Create_Product_Parent_Config, parameters);
                return Ok(Cat_data);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //===========================================Update Product Parent_Configuration Details===========================================
        [HttpPost]
        [Route("Update_Product_Parent_Config_Details")]
        public IHttpActionResult Update_Product_Parent_Config_Details(CreateNewUnitClass gt)
        {
            try
            {
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ID", gt.ID }
                    ,{ "@Name", gt.ClassName }
                    ,{ "@ProductSellingMinPrice", gt.ProductSellingMinPrice }
                    ,{ "@ProductSellingMaxPrice", gt.ProductSellingMaxPrice }
                    ,{ "@ProductSellingMaxQuantity", gt.ProductSellingMaxQuantity }
                    ,{ "@IsAssignSameProduct", gt.Budget }  ,{ "@Priority", gt.Priority }
                    ,{ "@Status", gt.Status },{ "@UserID",User_Id },{ "@MaxStockQuantity",gt.DCStockAlertQuantity }
                };
                var Cat_data = _database.QueryValue(Update_Product_Parent_Config, parameters);
                return Ok(Cat_data);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //==============================Remove Parent Product Configuration Details=============================================
        [HttpPost]
        [Route("Remove_Parent_Product_Config_Details")]
        public IHttpActionResult Remove_Parent_Product_Config_Details(CreateNewUnitClass gt)
        {
            try
            {
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Attribute_Child_Config_ID", gt.ID } };
                var Cat_data = _database.QueryValue(Remove_Parent_Product, parameters);
                return Ok(Cat_data);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //=========================================Get Products By ClassId and CatId==============================================
        [HttpPost]
        [Route("Get_Products_By_Class_Id_Cat_Id")]
        public IHttpActionResult Get_Products_By_Class_Id_Cat_Id(VMPagingResultsPost gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ID", gt.ID }
                    ,{ "@Filter_Json",gt.FilterJson },{ "@ProductName",gt.ProductName},
                    {"@SKU",gt.SKU }
                    ,{ "@Page_Index", gt.PageIndex },{ "@Page_Size", gt.Pagesize } };
                var Cat_data = _database.ProductListWithCount(Auto_Get_Products_By_Class, parameters);
                return Ok(Cat_data);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //===============================Update Unit Product Details============================================
        [HttpPost]
        [Route("Update_Unit_Product_Details")]
        public IHttpActionResult Update_Unit_Product_Details(CreateNewUnitClass gt)
        {
            try
            {
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ID", gt.ID }
                , {"@Status",gt.Status },
                    { "@ProductPriority", gt.Priority},
                    { "@UserID", (Convert.ToInt32(User_Id))}
                    ,{ "@ProductStockMaxQuantity", gt.ProductSellingMaxPrice} };
                var Cat_data = _database.QueryValue(Auto_Update_Unit_Product, parameters);
                return Ok(Cat_data);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //=======================================Auto Bulk Remove Products================================================
        [HttpPost]
        [Route("Auto_Bulk_Remove_Products")]
        public IHttpActionResult Auto_Bulk_Remove_Products(CreateNewUnitClass gt)
        {
            try
            {
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@IDs", gt.Description }
                ,   { "@UserID", (Convert.ToInt32(User_Id))}};
                var Cat_data = _database.QueryValue(Auto_Bulk_Remove_Unit, parameters);
                return Ok(Cat_data);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //===========================================To Get Unassigned Products List By ClassID=================================
        [HttpPost]
        [Route("Get_Unassigned_Products_List_By_ClassID_Cat_ID")]
        public IHttpActionResult Get_Unassigned_Products_List_By_ClassID_Cat_ID(VMPagingResultsPost gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ClassId", gt.ID }
                    ,{ "@Filter_Json", gt.FilterJson }
                    ,{ "@ProductName", gt.ProductName }
                     ,{"@Page_Index",gt.PageIndex }
                     ,{ "@SKU", gt.SKU }
                    ,{ "@Page_Size", gt.Pagesize }
                    ,{ "@Product_Config_ID", gt.Source_UnitID },
                    {"@Cat_ID",gt.LocationID } };
                var Cat_data = _database.ProductListWithCount(Auto_Get_Unassign_Products, parameters);
                return Ok(Cat_data);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //==============================Assign  Products To Unit Classes============================
        [HttpPost]
        [Route("Assign_Products_To_Unit_Clasas")]
        public IHttpActionResult Assign_Products_To_Unit_Clasas(CreateNewUnitClass gt)
        {
            try
            {
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ID", gt.ID },
                    {"@ProductIDs",gt.ClassName },{ "@UserID", User_Id} };
                var Cat_data = _database.QueryValue(Auto_Assign_Products_To_Unit_Classes, parameters);
                return Ok(Cat_data);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //=================================================Attribute Configuration=========================================
        [HttpPost]
        [Route("Get_Attribute_Parent_Config_Details_By_Class_Id_Cat_Id")]
        public IHttpActionResult Get_Attribute_Parent_Config_Details_By_Class_Id_Cat_Id(VMPagingResultsPost gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ClassID", gt.ID },
                    {"@Cat_ID",gt.LocationID }};
                var Cat_data = _database.ProductListWithCount(Get_Attribute_Parent_Config_Details_By_Class, parameters);
                return Ok(Cat_data);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }
        
        //==================================================Update Attribute Configuration==============================================
        [HttpPost]
        [Route("Update_Attribute_Configuration")]
        public IHttpActionResult Update_Attribute_Configuration(CreateNewUnitClass gt)
        {
            try
            {
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Attribute_Child_Config_ID", gt.ID },
                        { "@Status", gt.Status },
                        { "@Is_Assign_Differnt_Products", gt.LocationID},
                       { "@Priority", gt.Priority }, { "@ProductSellingMinPrice", gt.ProductSellingMinPrice },{ "@ProductSellingMaxPrice", gt.ProductSellingMaxPrice },
                        { "@Max_Quantity",gt.DCStockAlertQuantity},
                        { "@Product_Max_Quantity", gt.ProductSellingMaxQuantity }, { "@UserID", User_Id}, { "@BrandMinProducts", gt.BrandMinProductsQuantity}
                     };
                var Attribute_Configuration = _database.QueryValue(UpdateAttributeConfiguration, parameters);
                return Ok(Attribute_Configuration);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //==================================================Get Attribute Service  By CategoryID============================================
        [HttpGet]
        [Route("Get_Attributes_Service_By_CategoryID")]
        public IHttpActionResult Get_Attributes_Service_By_CategoryID(int CategoryID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@CategoryId", CategoryID } };
                var Category_Attributes = _database.ProductListWithCount(Get_Dynamic_Category_Attributes, parameters);
                return Ok(Category_Attributes);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //==========================================Create New All Attribute Configuration=================================================
        [HttpPost]
        [Route("Create_New_All_Attribute_Configuration")]
        public IHttpActionResult Create_New_All_Attribute_Configuration(CreateNewUnitClass gt)
        {
            try
            {
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                    { "@ChildName", gt.ClassName },{ "@ProductSellingMaxPrice", gt.ProductSellingMaxPrice }
                    ,{ "@ProductSellingMinPrice", gt.ProductSellingMinPrice }
                    ,{ "@ProductSellingMaxQuantity", gt.ProductSellingMaxQuantity },{"@MaxQuantityAllowed",gt.DCStockAlertQuantity}
                    ,{ "@Priority",gt.Priority },{ "@IsAssignSameProduct", gt.Status },{ "@FilterJson",gt.Description}
                    ,{ "@CategoryID",gt.CategoryID},{ "@UserID",User_Id }, { "@ClassId", gt.ID }, { "@BrandMinProducts", gt.BrandMinProductsQuantity }
                };
                var New_Attributes = _database.QueryValue(Add_New_Attributes, parameters);
                return Ok(New_Attributes);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //==============================Get Attribute Products By CongigID=============================================================
        [HttpGet]
        [Route("Get_Attribute_Products_By_CongigID")]
        public IHttpActionResult Get_Attribute_Products_By_CongigID(int AttributeCongigID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ConfigureId", AttributeCongigID } };
                var Configured_Attribute = _database.ProductListWithCount(Get_Configured_Attribute, parameters);
                return Ok(Configured_Attribute);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //==============================Get Attributes By CategoryId And Filters=======================================================
        [HttpPost]
        [Route("Get_Attributes_By_CategoryId_And_Filters")]
        public IHttpActionResult Get_Attributes_By_CategoryId_And_Filters(CreateNewUnitClass gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                    { "@ProductSellingMinPrice", gt.ProductSellingMinPrice },{ "@ProductSellingMaxPrice", gt.ProductSellingMaxPrice }
                   ,{ "@FilterJson",gt.Description},{ "@CategoryID",gt.CategoryID}, { "@ClassId", gt.ID }
                };
                var Get_Products = _database.ProductListWithCount(CategoryId_And_Filters, parameters);
                return Ok(Get_Products);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //=========================================Delete Attribute Configuration=================================================
        [HttpPost]
        [Route("DeleteAttributeConfiguration")]
        public IHttpActionResult DeleteAttributeConfiguration(CreateNewUnitClass gt)
        {
            try
            {
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Attribute_Child_Config_ID", gt.ID } };
                var AttributeConfiguration = _database.QueryValue(Auto_Remove_Configuration, parameters);
                return Ok(AttributeConfiguration);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //==================================To Save New Attribute Priority================================================
        [HttpPost]
        [Route("Save_New_Attribute_Priority")]
        public IHttpActionResult Save_New_Attribute_Priority(CreateNewUnitClass gt)
        {
            try
            {
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@AttributeJson", gt.Description },
                        { "@Priority", gt.Priority },{ "@ConfigId", gt.ID}};
                var Attribute_Priority = _database.QueryValue(GetAutostock, parameters);
                return Ok(Attribute_Priority);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //=======================================Get Configured Attributes By CongigID===================================================
        [Route("Get_Configured_Attributes_By_CongigID")]
        public IHttpActionResult Get_Configured_Attributes_By_CongigID(int ConfigurationId)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ParentConfigId", ConfigurationId } };
                var AttributesConfigurationId = _database.ProductListWithCount(Attributes_By_ConfigurationId, parameters);
                return Ok(AttributesConfigurationId);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //==========================================Remove Attribute  By Id=========================================================
        [HttpPost]
        [Route("Remove_Attribute_By_Id")]
        public IHttpActionResult Remove_Attribute_By_Id(int ConfigId, int Priority)
        {
            try
            {
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Auto_Stock_Attribute_Priority_ID",ConfigId },
                        { "@Priority", Priority }};
                var Remove_Attribute = _database.QueryValue(Remove_Attribute_Priority, parameters);
                return Ok(Remove_Attribute);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        #endregion "END of -- Auto Stock Configuration"

        #region  "Begin Assign Or Remove Units"

        //=====================================To Get Unassigned Unit List=========================================
        [HttpPost]
        [Route("Get_Unassigned_Unit_Class_List")]
        public IHttpActionResult Get_Unassigned_Unit_Class_List(CreateNewUnitClass gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ParentType", gt.ID } };
                var Unit_Class = _database.ProductListWithCount(Unassigned_Unit_Class, parameters);
                return Ok(Unit_Class);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //========================================Assign Unit Id To Class Id===================================
        [HttpPost]
        [Route("Assign_Unit_Id_to_Class_Id")]
        public IHttpActionResult Assign_Unit_Id_to_Class_Id(CreateNewUnitClass gt)
        {
            try
            {
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ClassId", gt.ID } ,
                { "@Unitids", gt.ClassName }, { "@UserID",User_Id} };
                var Unit_ClassID = _database.QueryValue(Unit_ClassID_To_Units, parameters);
                return Ok(Unit_ClassID);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //===================================To Remove Unit Class====================================
        [HttpPost]
        [Route("Remove_Unit_Class")]
        public IHttpActionResult Remove_Unit_Class(CreateNewUnitClass gt)
        {
            try
            {
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ID", gt.ID } };
                var Remove_Unit_Class = _database.QueryValue(Remove_Unit_Class_Id, parameters);
                return Ok(Remove_Unit_Class);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        #endregion "End Assign Or Remove Units"

        #region "Unit Stock Details" 

        //===================To Get Units List===========================================
        [HttpPost]
        [Route("Get_Units_List_By_Class_Id")]
        public IHttpActionResult Get_Units_List_By_Class_Id(AutoStockDataMdel gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ClassId", gt.ID },
                     {"@Page_Index",gt.PageIndex },{ "@Page_Size", gt.Pagesize } };
                var Class_Id = _database.ProductListWithCount(Units_List_Bugget_Wise, parameters);
                return Ok(Class_Id);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //==============================To Get Unit Main Category Wise Bugget List==============================
        [HttpPost]
        [Route("Get_Unit_Main_Category_wise_Bugget_List")]
        public IHttpActionResult Get_Unit_Main_Category_wise_Bugget_List(CreateNewUnitClass gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@UnitID", gt.ID } };
                var Bugget_List = _database.ProductListWithCount(Unit_Main_Category_wise_Bugget_List, parameters);
                return Ok(Bugget_List);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //======================================To Get Unit Leaf Category wise Bugget List==============================
        [HttpPost]
        [Route("Get_Unit_Leaf_Category_wise_Bugget_List")]
        public IHttpActionResult Get_Unit_Leaf_Category_wise_Bugget_List(CreateNewUnitClass gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@UnitID", gt.ID }
                    , { "@CatID", gt.CategoryID}
                     };
                var Cat_data = _database.ProductListWithCount(Unit_Leaf_Category_wise_Bugget_List, parameters);
                return Ok(Cat_data);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //========================================== To View Stock Details========================================================
        [HttpPost]
        [Route("ViewStockDetails_Service")]
        public IHttpActionResult ViewStockDetails_Service(AutoStockDataMdel gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@UnitID", gt.ID }
                    ,{ "@Filter_Json", gt.FilterJson }
                    ,{ "@ProductName", gt.ProductName }
                     ,{"@Page_Index",gt.PageIndex }
                     ,{ "@SKU", gt.SKU }
                    ,{ "@Page_Size", gt.Pagesize },
                    {"@CatID",gt.LocationID } };
                var Cat_data = _database.ProductListWithCount(Product_Stock_details_List, parameters);
                return Ok(Cat_data);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //===============================Tracking  Product Status=================================
        [HttpGet]
        [Route("Tracking_AS_ProductStatus")]
        public IHttpActionResult Tracking_AS_ProductStatus(int ProductId, int UnitID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ProductId", ProductId }, { "@UnitId", UnitID } };
                var Inventory_Stock = _database.ProductListWithCount(Inventory_Stock_Details_By_ProductID, parameters);
                return Ok(Inventory_Stock);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //====================================Revert Assigned Products By Details==========================================
        [HttpPost]
        [Route("Revert_Assigned_Products_By_Details")]
        public IHttpActionResult Revert_Assigned_Products_By_Details(AutoStockDataMdel gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@SourceUnitID", gt.SourceUnitID },
                        { "@DestUnitID", gt.DestUnitID },{ "@ProductID", gt.ProductID},{ "@Quantitiy", gt.Quantitiy }};
                var Cat_data = _database.QueryValue(Revert_Assigned_Products, parameters);
                return Ok(Cat_data);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //=======================Remove Products From Assigned Stock==========================================================
        [HttpPost]
        [Route("Remove_Products_From_Assigned_Stock")]
        public IHttpActionResult Remove_Products_From_Assigned_Stock(AutoStockDataMdel gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@SourceUnitID", gt.SourceUnitID },
                        { "@DestUnitID", gt.DestUnitID },{ "@ProductID", gt.ProductID},{ "@Quantitiy", gt.Quantitiy }};
                var Assigned_Stock = _database.QueryValue(Remove_Products, parameters);
                return Ok(Assigned_Stock);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //==================================================Replace With Another Product============================================
        [HttpPost]
        [Route("Replace_With_Another_Product")]
        public IHttpActionResult Replace_With_Another_Product(AutoStockDataMdel gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@SourceUnitID", gt.SourceUnitID },
                        { "@DestUnitID", gt.DestUnitID },{ "@ProductID", gt.ProductID},{ "@Quantitiy", gt.Quantitiy }};
                var Cat_data = _database.ProductListWithCount(Replace_Products, parameters);
                return Ok(Cat_data);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //==================================Update Replace New Product=============================================================
        [HttpPost]
        [Route("Update_Replace_New_Product")]
        public IHttpActionResult Update_Replace_New_Product(AutoStockDataMdel gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@SourceUnitID", gt.SourceUnitID },
                        { "@DestUnitID", gt.DestUnitID },{ "@Assigned_ProductID", gt.ProductID},{ "@Replace_ProductID", gt.ID},{ "@Quantitiy", gt.Quantitiy }};
                var Cat_data = _database.QueryValue(Auto_Replace_Assigned_Products, parameters);
                return Ok(Cat_data);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        #endregion

        #region "Begin Job Run Details"

        //=============================================Get AutoStock Updated Units List By Reference Number================================
        [HttpPost]
        [Route("Get_Auto_Stock_Updated_Units_List_By_Ref_Number")]
        public IHttpActionResult Get_Auto_Stock_Updated_Units_List_By_Ref_Number(VMPagingResultsPost gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@RefNumber", gt.ID } };
                var Cat_data = _database.ProductListWithCount(Auto_Stock_Updated_Units, parameters);
                return Ok(Cat_data);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //======================================Get AutoStock Updated Units Categories By Reference Number==============================
        [HttpPost]
        [Route("Get_Auto_Stock_Updated_Units_Categories_By_Ref_Number")]
        public IHttpActionResult Get_Auto_Stock_Updated_Units_Categories_By_Ref_Number(AutoStockDataMdel gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@RefNumber", gt.ID },
                    {"@UnitID",gt.UnitID }};
                var Ref_Number = _database.ProductListWithCount(Updated_Units_Categories, parameters);
                return Ok(Ref_Number);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //================================Get AutoStock Updated Units Products By Reference Number=============================
        [HttpPost]
        [Route("Get_Auto_Stock_Updated_Units_Products_By_Ref_Number")]
        public IHttpActionResult Get_Auto_Stock_Updated_Units_Products_By_Ref_Number(AutoStockDataMdel gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@RefNumber", gt.ID },
                    {"@UnitID",gt.UnitID },{"@CatgoryID",gt.CategoryID } };
                var Units_Products = _database.ProductListWithCount(Auto_Stock_Updated_Units_Products, parameters);
                return Ok(Units_Products);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //===================================Revert Stock By AutoStock Reference Number========================================
        [HttpPost]
        [Route("Revert_Stock_By_Auto_Stock_Ref_Number")]
        public IHttpActionResult Revert_Stock_By_Auto_Stock_Ref_Number(AutoStockDataMdel gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@RefNumber", gt.ID },
                    {"@UnitID",gt.UnitID },
                    {"@ProductID",gt.ProductID },
                    { "@Quantity", gt.Quantitiy } };
                var Auto_Stock = _database.QueryValue(Revert_Stock_By_Auto_Stock, parameters);
                return Ok(Auto_Stock);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        #endregion "Begin Job Run Details"       

        #region "Begin Low Stock Report"

        //===============================Get AutoStock Low Stock Report===================================
        [HttpPost]
        [Route("Get_Auto_stock_Low_Stock")]
        public IHttpActionResult Get_Auto_stock_Low_Stock(AutoStockDataMdel gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@SourceUnitId", gt.SourceUnitID },
                        { "@DestinationUnitId", gt.DestUnitID },{ "@Sku", gt.FilterJson},{ "@ProductName", gt.ProductName },{ "@CategoryId", gt.Leaf_Category_Id } };
                var Cat_data = _database.ProductListWithCount(Stock_Report_After_Job_Runs, parameters);
                return Ok(Cat_data);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //======================================DC Wise AutoStock Report=============================================
        [HttpGet]
        [Route("LowStock_Get_DC_Wise_Low_Stock_Report")]
        public IHttpActionResult LowStock_Get_DC_Wise_Low_Stock_Report(int UnitId)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@UnitID", UnitId } };
                var Cat_data = _database.ProductListWithCount(DC_Wise_Low_Stock_Report, parameters);
                return Ok(Cat_data);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //=============================To Assign Main Categories By Categories===============================
        [Route("Get_Assigned_Main_Categories_By_Categories")]
        public IHttpActionResult Get_Assigned_Main_Categories_By_Categories(int UnitId)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@UnitID", UnitId } };
                var Cat_data = _database.ProductListWithCount(LS_Get_Assigned_Main_Categories, parameters);
                return Ok(Cat_data);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //===================================== To  Get Assigned Stock By Sub Categories==============================
        [Route("Get_Assigned_Stock_By_Sub_Categories")]
        public IHttpActionResult Get_Assigned_Stock_By_Sub_Categories(int UnitId, int CategoryId)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@UnitID", UnitId }, { "@MainCategoryId", CategoryId } };
                var Cat_data = _database.ProductListWithCount(Assigned_Stock_By_Sub_Categories, parameters);
                return Ok(Cat_data);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //========================To Get Assigned Products Stock BY Leaf Node===================================
        [Route("Get_Assigned_Products_Stock_By_Leaf_Node")]
        public IHttpActionResult Get_Assigned_Products_Stock_By_Leaf_Node(int UnitId, int MainCategoryId, int SubCategoryId)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@UnitID", UnitId }, { "@MainCategoryId", MainCategoryId }, { "@LeafCategoryId", SubCategoryId } };
                var Cat_data = _database.ProductListWithCount(Products_Stock_By_Leaf_Node, parameters);
                return Ok(Cat_data);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //============================To Get AutoStock Product Details=================================================
        [Route("Get_AutoStock_Product_Details")]
        public IHttpActionResult Get_AutoStock_Product_Details(int ProductId, int UnitID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ProductID", ProductId }, { "@UnitID", UnitID } };
                var Cat_data = _database.ProductListWithCount(Unit_Product_Stock_Details, parameters);
                return Ok(Cat_data);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        #endregion "End Low Stock Report"        

        #region "Start Job Run"

        //============================================To Get AutoStock Data Details===========================================
        [HttpPost]
        [Route("Get_Auto_stock_data_Details")]
        public IHttpActionResult Get_Auto_stock_data_Details(AutoStockDataMdel gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ClassID", gt.ID },
                        { "@UnitID", gt.UnitID },{ "@TopCatID", gt.CategoryID},{ "@CatID", gt.LocationID } };
                var Cat_data = _database.ProductListWithCount(Get_Auto_stock_data, parameters);
                return Ok(Cat_data);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        //===============================AutoStock Update JobRun API========================================================
        [HttpPost]
        [Route("Auto_Stock_Update_Job_Run_API")]
        public IHttpActionResult Auto_Stock_Update_Job_Run_API(List<AutoStockDataMdel> gt)
        {
            try
            {
                int CategoriesCount = 0; int UnitsCount = 0;
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                for (int i = 0; i < gt.Count(); i++)
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ClassID", gt[i].Unit_Class_Id },
                        { "@UnitID", gt[i].iHub_Unit_ID },{ "@TopCatID", gt[i].Main_Category_ID},{ "@CatID", gt[i].Leaf_Category_Id } , { "@UserID", User_Id}, { "@Output_Updated_Quantity", 0}};
                    var Cat_data = _database.QueryValue(Auto_Stock_Job_Run, parameters);
                    CategoriesCount = CategoriesCount + 1;
                }
                var ListOfUsers = gt.GroupBy(x => x.iHub_Unit_ID)
                              .Select(g => g.First())
                              .ToList();
                UnitsCount = ListOfUsers.Count();
                var ResponseString = "<b Style=Color:green>Total " + CategoriesCount + "  Categories Updated For  " + UnitsCount + " Stores</b>";
                return Ok(ResponseString);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Category--------Error---------", ex);
                return Ok(cs);
            }
        }

        #endregion "END Job Run"
    }
}