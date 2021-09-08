using log4net;
using Excel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Configuration;
using System.IO;
using iHubAdminAPI.Models;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using System.Configuration;
using AspNet.Identity.SQLDatabase;

namespace iHubAdminAPI.Controllers
{
    //============== [Authorize]=========================================
    [RoutePrefix("api/iHubUsage")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ihubusageController : ApiController
    {
        string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private static readonly ILog log = LogManager.GetLogger(typeof(ihubusageController).FullName);
        public SQLDatabase _database;
        

        public ihubusageController()
        {
            _database = new SQLDatabase();
        }
        private class CustomResponse
        {
            internal string Response;
            internal int ResponseID;
        }

        #region "BEGIN of -- ihubusage SP Names"

        // =================================Command Rext For Api Calls====================================================
        string GetAvaliableProductsListByUnitID = "[iAdmin_Get_Avaliable_Products_List_By_UnitID_And_CategoryId]";
        string Usage_Products = "[iAdmin_Get_iHub_Usage_Products_List_By_Order_Id]";
        string GetSelectedProductsListByUnitID = "[iAdmin_Get_Selected_Products_List_By_UnitID]";
        string iHubUsageProducts = "[iAdmin_Create_iHub_Usage_Products]";
        string Get_Usage_records = "[iAdmin_Get_iHub_Usage_Products_List]";
        string Get_Usage_Product_Details = "[iAdmin_Get_iHub_Usage_Products_List_By_Order_Id]";

        #endregion "END of -- SP Names"

        #region "BEGIN of -- ihubusage Methods"

        //================================================To Get Available Product List=====================================
        [HttpPost]
        [Route("Get_Avaliable_Products_List_By_UnitID")]
        public IHttpActionResult Get_Avaliable_Products_List_By_UnitID(IHubUsageController Sc)
        {
            try
            {
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                    { "@UnitID", Sc.UnitID}
                    , { "@SKU", Sc.SKU }
                    , { "@ProductName", Sc.ProductName }
                    , { "@CategoryID", Sc.productid }
                    , { "@Page_Size", Sc.Page_Size }
                    , { "@Page_Index", Sc.Page_Index } };
                var res = _database.ProductListWithCount(GetAvaliableProductsListByUnitID, parameters);
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

        //===============================To Get Selected Products List============================================
        [HttpPost]
        [Route("Get_Selected_Products_List_By_UnitID")]
        public IHttpActionResult Get_Selected_Products_List_By_UnitID(IHubUsageController Sc)
        {
            try
            {
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                    { "@UnitID", Sc.UnitID}
                    , { "@Product_Ids", Sc.product_id }};
                var res = _database.ProductListWithCount(GetSelectedProductsListByUnitID, parameters);
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

        //===================================To Get iHub Usage Products====================================================
        [HttpPost]
        [Route("iHub_Usage_Products")]
        public IHttpActionResult iHub_Usage_Products(IHubUsageController Sc)
        {
            try
            {
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                    { "@UnitID", Sc.UnitID}
                    , { "@Product_Ids_Qty", Sc.product_id }};
                var res = _database.QueryValue(iHubUsageProducts, parameters);
                Dictionary<string, object> parameters2 = new Dictionary<string, object>() {
                    { "@OrderID", res} };
                var res2 = _database.ProductListWithCount(Usage_Products, parameters2);
                return Ok(res2);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("GetTableRecords----Error---------", ex);
                return Ok(cs);
            }
        }

        //=================================To Get ihub usage Records================================
        [HttpPost]
        [Route("Get_ihub_Usage_Records")]
        public IHttpActionResult Get_ihub_Usage_Records(IHubUsageController vm)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@OrderID",vm.OrderID }, { "@OrderDate_From", vm.orderdatefrom }, { "@OrderDate_To", vm.orderdateto },
                   { "@UnitName", vm.Unit_Type },{ "@Page_Index", vm.pageindex },{ "@Page_Size", vm.pagesize } };
                var locations = _database.ProductListWithCount(Get_Usage_records, parameters);
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

        //====================================To Get iHub Usage Product Details==========================================
        [HttpPost]
        [Route("Get_ihub_Usage_ProductDetails")]
        public IHttpActionResult Get_ihub_Usage_ProductDetails(IHubUsageController vm)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@OrderID", vm.OrderID } };
                var locations = _database.ProductListWithCount(Get_Usage_Product_Details, parameters);
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
        //====================================To Get iHub Usage Product Details==========================================     
        [HttpPost]
        [Route("Get_iHub_Ordered_Product_Details")]
        public IHttpActionResult Get_iHub_Ordered_Product_Details(IHubUsageController vm)
        {
            try
            {
                var cmd = "SELECT Quantity,Product_Name FROM \"UserBasket_Products\" WHERE Order_Details_ID=" + vm.OrderID;
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
        #endregion "END of -- ihubusage Methods"
    }
}