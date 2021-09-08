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
using iHubAdminAPI.Models.ProductAndPrice;
using System.Web.Script.Serialization;
using System.Net.Http;
using System.Net;
using System.Runtime.Serialization.Json;

namespace iHubAdminAPI.Controllers
{
    [RoutePrefix("api/VendorandProcurement")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class VendorandProcurementController : ApiController
    {
        string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private static readonly ILog log = LogManager.GetLogger(typeof(VendorandProcurementController).FullName);
        public SQLDatabase _database;
        ModelDBContext dbContext = new ModelDBContext();
        public VendorandProcurementController()
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
        string Get_All_vendors = "[iAdmin_Get_All_Vendors]";
        string Creating_vendor = "[iAdmin_Create_New_Vendor_With_Serving_Categories]";
        string Get_vendor_Categories = "[iAdmin_Get_Serving_Categories_By_Vendor_ID]";
        string Updating_vendor = "[iAdmin_Update_Vendor_Details_And_Serving_Categories]";
        string Get_Vendors_List = "[iAdmin_Get_Vendors_By_Selected_Category]";
        string Vendor_Invoice_Details = "[iAdmin_Get_All_Created_Vendor_Invoice_Details]";
        string Creating_vendor_Invoice = "[iAdmin_Create_New_Vendor_Invoice]";
        string Update_Invoice = "[iAdmin_Update_Vendor_Invoice_Details]";
        string Get_All_Brands_Based_On_Last_Node_Category = "[iAdmin_Get_All_Brands_By_CatID]";
        string Get_Products_To_Upload_SizeChart_Based_On_Category = "[iAdmin_Get_Products_To_Upload_SizeChart_Based_On_Category]";
        string Updating_Delete_SizeChart = "[iAdmin_Update_Or_Delete_Size_Chart_For_Product]";
        string Create_Size_Chart_Based_On_Brand_Or_Products = "[iAdmin_Create_Size_Chart_Based_On_Brand_Or_Products]";


        #endregion "End of -- SP Names"

        #region "BEGIN of -- Vendor Management"

        //===============>>>>>>>>To Get All Vendors List<<<<<<<<<<<<<<<<<<=================================================//
        [HttpPost]
        [Route("Get_All_Created_Vendors")]
        public IHttpActionResult Get_All_Created_Vendors(VendorInvoiceCreation GF)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {{ "@Name", GF.Name }, { "@Mobile_Number", GF.MobileNumber },
                        { "@IsActive", GF.IsAdmin }, { "@Page_Index", GF.PageIndex }, { "@Page_Size", GF.PageSize }};
                var res = _database.ProductListWithCount(Get_All_vendors, parameters);
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

        //================>>>>>>>>>>>>> Create New Vendor <<<<<<<<<<<<<<<<=====================//
        [HttpPost]
        [Route("Create_New_vendor")]
        public IHttpActionResult Create_New_vendor(VendorInvoiceCreation GF)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Name", GF.Name }, { "@ContactName", GF.StoreName }, { "@MobileNumber", GF.MobileNumber },
                                                        { "@AddressDetails", GF.FilterJson },{ "@ServeCategories", GF.ParentCategoryName }};
                var res = _database.QueryValue(Creating_vendor, parameters);
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

        //================>>>>>>>>>>>>> To Get Serving Categories By VendorID <<<<<<<<<<<<<<<<=====================//
        [HttpGet]
        [Route("Get_Serving_Categories_By_Vendor_ID")]
        public IHttpActionResult Get_Serving_Categories_By_Vendor_ID(int VendorId)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Vendor_Id", VendorId } };
                var res = _database.ProductListWithCount(Get_vendor_Categories, parameters);
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

        //=======================>>>>>>>>>> Update Created Vendor <<<<<<<<<<<<<<==================//
        [HttpPost]
        [Route("Update_Created_Vendor")]
        public IHttpActionResult Update_Created_Vendor(VendorInvoiceCreation GF)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@VendorID", GF.VendorID },{ "@ContactName", GF.StoreName }, { "@MobileNumber", GF.MobileNumber },
                                                        { "@AddressDetails", GF.FilterJson },{ "@ServeCategories", GF.ParentCategoryName }};
                var res = _database.QueryValue(Updating_vendor, parameters);
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

        //=======================>>>>>>>>>> Get Vendor By Category <<<<<<<<<<<<<<=======================//
        [HttpGet]
        [Route("Get_Vendors_By_Selected_Category")]
        public IHttpActionResult Get_Vendors_By_Selected_Category(int CategoryId)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Category_Id", CategoryId } };
                var res = _database.ProductListWithCount(Get_Vendors_List, parameters);
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

        //=======================>>>>>>>>>> Get All Created Vendor Invoice Details <<<<<<<<<<<<<<==================//
        [HttpPost]
        [Route("Get_All_Created_Vendor_Invoice_Details")]
        public IHttpActionResult Get_All_Created_Vendor_Invoice_Details(VendorInvoiceCreation GF)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {{ "@Name", GF.Name }, { "@CategoryName", GF.BrandName },
                        { "@ApprovedBy", GF.Message }, { "@InvoiceNumber", GF.MobileNumber }, { "@Page_Index", GF.PageIndex }, { "@Page_Size", GF.PageSize }};
                var res = _database.ProductListWithCount(Vendor_Invoice_Details, parameters);
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

        //=============================>>>>>>>>>>> Create New Vendor <<<<<<<<<<<<<<<<=================//
        [HttpPost]
        [Route("Create_New_Vendor_Invoice")]
        public IHttpActionResult Create_New_Vendor_Invoice(VendorInvoiceCreation GF)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@CategoryId", GF.CategoryID }, { "@VendorId", GF.ID }, { "@StockAgainst", GF.Stock },
                { "@PaymentStatus", GF.Status2 },{ "@InvoiceNumber", GF.transactionnumber } ,{ "@ApprovedBy", GF.Name },{ "@InvoiceRemarks", GF.HtmlContent }};
                var res = _database.QueryValue(Creating_vendor_Invoice, parameters);
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

        //=================>>>>>>>>>>> Update Vendor Invoice <<<<<<<<<<<<<=========================//
        [HttpPost]
        [Route("Update_Vendor_Invoice")]
        public IHttpActionResult Update_Vendor_Invoice(VendorInvoiceCreation GF)
        {
            try
            {             
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@InvoiceID", GF.VendorInvoiceID },
                { "@UpdatePaymentStatus", GF.Status2 },{ "@InvoiceNumber", GF.transactionnumber }
                ,{ "@UpdateInvoiceStatus", GF.Status },{ "@UpdateInvoiceRemarks", GF.HtmlContent }};
                var res = _database.QueryValue(Update_Invoice, parameters);
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

        //============>>>>>>> Get All Brands Based on Last Node <<<<<<<<<<<<<===========================//
        [HttpGet]
        [Route("GetAllBrandsBasedOnLastNodeCategory")]
        public IHttpActionResult GetAllBrandsBasedOnLastNodeCategory(int CatID)
        {
            try
            {                
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@CategoryId", CatID } };
                var res = _database.ProductListWithCount(Get_All_Brands_Based_On_Last_Node_Category, parameters);
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

        //================>>>>>>> Create Size Chart Based on Brands <<<<<<<<<<=================//
        [HttpPost]
        [Route("CreateSizeChartBasedOnBrandOrProducts")]
        public IHttpActionResult CreateSizeChartBasedOnBrandOrProducts(SizeChart vmodel)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@SizeChartType", vmodel.SizeChartType },
                                                                                           { "@BrandName", vmodel.BrandName },
                                                                                           { "@ProductsList", vmodel.ProductsList },
                                                                                           { "@CategoryId", vmodel.Categoryid } };
                var res = _database.QueryValue(Create_Size_Chart_Based_On_Brand_Or_Products, parameters);
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

        //======>>>>>>> Method For Upload Size Chart Image For Brands And Products <<<<<<<<<<========//
        [HttpPost]
        [Route("Upload_Size_Chart_Image")]
        public IHttpActionResult Upload_Size_Chart_Image(int ClaimID)
        {
            try
            {
                HttpPostedFile file = HttpContext.Current.Request.Files[0];
                string physicalPath = HttpContext.Current.Server.MapPath("~/" + "Images/SizeCharts/");
                if (!System.IO.File.Exists(physicalPath))
                {
                    Directory.CreateDirectory(physicalPath);
                }
                string ExteName = Path.GetExtension(file.FileName);
                string imgPath = "";
                imgPath = HttpContext.Current.Server.MapPath("~/" + "Images/SizeCharts/" + ClaimID + ".png");
                file.SaveAs(imgPath);
                return Ok(ClaimID);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }

        //===============>Get All Products Based on Last Node <<<<<<<<<<===========//
        [HttpPost]
        [Route("GetProductsToUploadSizeChartBasedOnCategory")]
        public IHttpActionResult GetProductsToUploadSizeChartBasedOnCategory(SizeChart vmodel)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Categoryid", vmodel.Categoryid },
                                                                                          { "@ProductName", vmodel.ProductName },
                                                                                          { "@Category_Name", vmodel.Category_Name },
                                                                                          { "@ProductSku", vmodel.ProductSku },
                                                                                          { "@Page_Size", vmodel.PageSize },
                                                                                          { "@Page_Index", vmodel.PageIndex },
                                                                                          { "@SizeChart",vmodel.Sizechart }  };
                var res = _database.ProductListWithCount(Get_Products_To_Upload_SizeChart_Based_On_Category, parameters);
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

        //=============>>>>>>>> Update Created Vendor <<<<<<<<<<<<===================//
        [HttpPost]
        [Route("Update_Delete_Size_Chart")]
        public IHttpActionResult Update_Delete_Size_Chart(SizeChart vmodel)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Flag", vmodel.Flag }, { "@ProductId", vmodel.ProductId }, { "@SizeChartId", vmodel.Sizechart } };
                var res = _database.QueryValue(Updating_Delete_SizeChart, parameters);
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
        //====Start=====>>>>Added For Warehouse App Purpose<<<<<==========//       
        //===================== Get_New requests count =================
        [HttpGet]
        [Route("Get_WH_Requests")]
        public IHttpActionResult Get_WH_Requests(int unitid)
        {
            try
            {
                string Get_requests = "[iAdmin_Get_WH_NewRequests_Count]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@unitid", unitid } };
                var result = _database.Query(Get_requests, parameters);
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
        //===================== Get_Warehouse details =================
        [HttpGet]
        [Route("Get_WH_Details")]
        public IHttpActionResult Get_WH_Details(int unitid)
        {
            try
            {
                string Get_details = "SELECT * FROM iHub_Units_DC_WH_ST WHERE iHub_Unit_ID =" + unitid;
                Dictionary<string, object> parameters = new Dictionary<string, object>() { };
                var result = _database.InsertQueryCommand(Get_details, parameters);
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

        //=====================Get_CategoryStock_By_UnitID=================
        [HttpGet]
        [Route("Get_CategoryStock_By_UnitID")]
        public IHttpActionResult Get_CategoryStock_By_UnitID(int UnitID)
        {
            try
            {
                string Get_CategoryStock = "[iAdmin_Get_Store_Category_List]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@UnitID", UnitID } };
                var result = _database.ProductListWithCount(Get_CategoryStock, parameters);
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

        //=====================Get_Stock_By_UnitID=================
        [Authorize]
        [HttpGet]
        [Route("Get_Stock_By_UnitID")]
        public IHttpActionResult Get_Stock_By_UnitID(int unitid)
        {
            try
            {
                string Get_unit_stock = "[iAdmin_Get_Stock_By_UnitID_For_WH]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@unitid", unitid } };
                var stock = _database.Query(Get_unit_stock, parameters);
                return Ok(stock);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("GetDynamicMenu---Error---------", ex);
                return Ok(cs);
            }

        }
        //===================== Get_Product_Details =================       
        [HttpGet]
        [Route("Get_Product_Details")]
        public IHttpActionResult Get_Product_Details(int productID)
        {
            try
            {
                string Get_productdetails = "[iAdmin_Get_Product_Details_By_ID]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@productid", productID } };
                var result = _database.Query(Get_productdetails, parameters);
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


        //=========>>>>Adding new Methods for Vendor/Procurement From B2B<<<<<==========//

        [HttpPost]
        [Route("Get_All_Created_Vendors_B2b")]
        public IHttpActionResult Get_All_Created_Vendors_B2b(VendorInvoiceCreation GF)
        {
            try
            {
                string Get_All_vendorsB2b = "[iB_Get_All_Vendors]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() {{ "@EName", GF.StoreName }, { "@VendorName", GF.Name },{ "@VendorType", GF.Status2 },
                        { "@Status", GF.Status }, { "@Page_Index", GF.PageIndex }, { "@Page_Size", GF.PageSize }};
                var res = _database.ProductListWithCount(Get_All_vendorsB2b, parameters);
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

        //================>>>>>>>>>>>>> Create New Vendor <<<<<<<<<<<<<<<<=====================
        public bool AssignCategoriesToBuyer(List<Vm_RoleCategories> AssignCatsRoles, int BuyerID)
        {
            try
            {
                var UserID = HttpContext.Current.User.Identity.GetUserID().ToString();
                var delete = dbContext.Vendor_Categories.Where(a => a.Vendor_ID == BuyerID).ToList();
                if (delete.Count > 0)
                    dbContext.Vendor_Categories.RemoveRange(delete);
                List<Dictionary<string, string>> AddProductsData = new List<Dictionary<string, string>>();
                var ProductsData = AssignCatsRoles.ToList().Where(m => m.PIDsWithLP.Count() > 0).Select(m => m.PIDsWithLP).ToList();
                foreach (var items in ProductsData)
                {
                    var iTaskID = items.FirstOrDefault().CatID;
                    AddProductsData = new List<Dictionary<string, string>>();
                    foreach (var item in items)
                    {
                        Dictionary<string, string> ProductsDataFields = new Dictionary<string, string>();
                        ProductsDataFields["iHub_Product_ID"] = item.iHub_Product_ID.ToString();
                        ProductsDataFields["Landing_Price"] = item.Landing_Price.ToString();
                        ProductsDataFields["CatID"] = item.CatID.ToString();
                        ProductsDataFields["Selling_Price"] = item.Selling_Price.ToString();
                        ProductsDataFields["MRP"] = item.MRP.ToString();
                        ProductsDataFields["ProductCode"] = item.ProductCode.ToString();
                        ProductsDataFields["Product_Name"] = item.Product_Name.ToString();
                        ProductsDataFields["HSN_Code"] = item.HSN_Code.ToString();
                        ProductsDataFields["GST_Percentage"] = item.GST_Percentage.ToString();
                        ProductsDataFields["Inventory_Count"] = item.Inventory_Count.ToString();
                        ProductsDataFields["Count"] = item.Count.ToString();
                        AddProductsData.Add(ProductsDataFields);
                    }
                    string ProductsJson1 = JsonConvert.SerializeObject(AddProductsData);
                    var ADD1 = (from ds in AssignCatsRoles.Where(a => a.TaskAssigned == true && a.PIDsWithLP != null && a.TaskID == iTaskID)
                                select new Cls_Vendor_Categories
                                {
                                    CatID = ds.TaskID,
                                    Vendor_ID = BuyerID,
                                    CreatedBy = UserID,
                                    CreatedDate = DateTime.Now,
                                    UpdatedBy = UserID,
                                    UpdatedDate = DateTime.Now,
                                    PIDsWithLP = ProductsJson1
                                }).ToList();
                    if (ADD1.Count() > 0)
                        dbContext.Vendor_Categories.AddRange(ADD1);
                    dbContext.SaveChanges();
                }
                var ADD = (from ds in AssignCatsRoles.Where(a => a.TaskAssigned == true && a.PIDsWithLP.Count() == 0)
                           select new Cls_Vendor_Categories
                           {
                               CatID = ds.TaskID,
                               Vendor_ID = BuyerID,
                               CreatedBy = UserID,
                               CreatedDate = DateTime.Now,
                               UpdatedBy = UserID,
                               UpdatedDate = DateTime.Now,
                               PIDsWithLP = null
                           }).ToList();
                if (ADD.Count() > 0)
                    dbContext.Vendor_Categories.AddRange(ADD);
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                CommonMethods.LogError(ex);
                throw ex;
            }
        }

        [HttpPost]
        [Route("Create_New_vendor_B2b")]
        public IHttpActionResult Create_New_vendor_B2b(Dictionary<string, object> InputProducts)
        {
            try
            {
                string Command = "[iB_Create_New_Vendor_With_Serving_Categories]";
                List<Vm_RoleCategories> Categories = new List<Vm_RoleCategories>();
                JavaScriptSerializer js = new JavaScriptSerializer();
                var oData = InputProducts["Categories"].ToString();
                List<Vm_RoleCategories> persons = js.Deserialize<List<Vm_RoleCategories>>(oData);
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    { "@json", InputProducts["ProductsData"] },
                    { "@MasterQutation", InputProducts["MasterQutation"] },
                    { "@App_type", InputProducts["type"]}
                };
                var ResultSet = _database.BusinessResultset(Command, parameters);
                var news = ResultSet.Resultset[0];
                int vendorID = Convert.ToInt32(news.ToList()[1].Value);
                var ds = AssignCategoriesToBuyer(persons, vendorID);
                return Ok(ResultSet);
            }
            catch (Exception ex)
            {
                CommonMethods.LogError(ex);
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.NotFound, ex));
            }
        }


        //=========================>>>>>>>>>>>get categories aginst Vendor<<<<<<<<<<<<<<<<<<<<<<<<==============
        [HttpGet]
        [Route("GetCategories_Vendor")]
        public IHttpActionResult GetCategories_Vendor(int id)
        {
            try
            {
                var GetBuyerID = dbContext.iHub_Vendors.Where(i => i.Vendor_ID == id).Select(k => k.Vendor_ID).FirstOrDefault();
                var CatIDs = dbContext.Vendor_Categories.Where(g => g.Vendor_ID == GetBuyerID).Select(u => u.CatID).ToList();
                var onlyactiveCats = "";//sele * from cate where stat=10 parentid=0.


                List<Vm_RoleCategories> tasks = (from t in dbContext.iH_Categories
                                                 join bc in dbContext.Vendor_Categories
                                                on t.ID equals bc.CatID into categories
                                                 from bct in categories.DefaultIfEmpty()
                                                 select new Vm_RoleCategories
                                                 {
                                                     Top_Category_Id = t.Top_Category_Id ?? 0,
                                                     Is_LeafNode = t.Is_LeafNode ?? true,
                                                     TaskAssigned = CatIDs.Contains(t.ID) ? true : false,
                                                     ParentTaskID = t.ParentID,
                                                     TaskID = t.ID,
                                                     TaskName = t.CategoryName,
                                                     MOQ = t.MOQ ?? 0
                                                 }).GroupBy(t => t.TaskID).Select(t => t.FirstOrDefault()).OrderBy(A => A.ParentTaskID).OrderBy(d => d.TaskName).ToList();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                //CommonMethods.LogError(ex);
                //log.Error("Error", ex);
                //throw new HttpResponseException(
                //Request.CreateErrorResponse(HttpStatusCode.NotFound, ex));
                log.Error("Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        //================>>>>>>>>>>>>> To Change status Of Purchse Order By ID <<<<<<<<<<<<<<<<=====================
        [HttpGet]
        [Route("Change_Vendor_Status")]
        public IHttpActionResult Change_Vendor_Status(int VendorID, int Status)
        {
            try
            {
                string Get_vendor_Categories = "[iB_Change_Vendor_Status]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@VendorId", VendorID }, { "@StatusID", Status } };
                var res = _database.ProductListWithCount(Get_vendor_Categories, parameters).Resultset;
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


        //===============================get Purchase orders
        [HttpPost]
        [Route("Get_Pending_Recevied_VPOs")]
        public IHttpActionResult Get_Pending_Recevied_VPOs(VendorInvoiceCreation GF)
        {
            try
            {
                DateTime newdate = new DateTime();
                if (GF.From_Date != null && GF.From_Date != "")
                {
                    newdate = Convert.ToDateTime(GF.From_Date);
                    string Get_vendor_Categories = "[iB_Get_Pending_Recevied_VPOs]";
                    Dictionary<string, object> parameters = new Dictionary<string, object>() {{ "@OrderID", GF.ID }, { "@EName", GF.Name },{ "@DateFrom",newdate },//{ "@DateTO",  Convert.ToDateTime(null) },
                        { "@Status", GF.Status }, { "@Page_Index", GF.PageIndex }, { "@Page_Size", GF.PageSize },{ "@VPOStatus", GF.VPOStatus }};
                    var res = _database.ProductListWithCount(Get_vendor_Categories, parameters);
                    return Ok(res);
                }
                else
                {
                    string Get_vendor_Categories = "[iB_Get_Pending_Recevied_VPOs]";
                    Dictionary<string, object> parameters = new Dictionary<string, object>() {{ "@OrderID", GF.ID }, { "@EName", GF.Name },{ "@DateFrom",GF.From_Date },//{ "@DateTO",  Convert.ToDateTime(null) },
                        { "@Status", GF.Status }, { "@Page_Index", GF.PageIndex }, { "@Page_Size", GF.PageSize },{ "@VPOStatus", GF.VPOStatus }};
                    var res = _database.ProductListWithCount(Get_vendor_Categories, parameters);
                    return Ok(res);
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
        //===============>>>>>>>>To Get All Vendors List<<<<<<<<<<<<<<<<<<============
        [HttpPost]
        [Route("Get_All_Vendor_Orders")]
        public IHttpActionResult Get_All_Vendor_Orders(VMModelsForProduct GF)
        {
            try
            {
                string Get_All_vendors = "[iB_Get_All_Vendors_Orders_Products]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() {{ "@Top_Category_Id", GF.ID },{ "@CategoryID", GF.CategoryID }, { "@ProductName", GF.ProductName },
                    { "@SKU", GF.SKU },  { "@Page_Index", GF.PageIndex }, { "@Page_Size", GF.Pagesize }, { "@VendorID", GF.VendorID },{ "@OrderID", GF.OrderID },{ "@UnitName", GF.UnitName },{ "@OrderType", GF.OrderType }};
                var res = _database.GetMultipleResultsList(Get_All_vendors, parameters);
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
        [Route("Get_UnitNames_for_Procurement")]
        public IHttpActionResult Get_UnitNames_for_Procurement()
        {
            try
            {
                
                    string Get_UnitNames = "select iHub_Unit_ID,Unit_Name from iHub_Units_DC_WH_ST where Unit_Hierarchy_Level IN (1,2,3) AND Is_Unit_Active=1 order by Unit_Name asc";
                    Dictionary<string, object> parameters = new Dictionary<string, object>() { };
                    var UnitNames = _database.SelectQuery(Get_UnitNames, parameters);
                    return Ok(UnitNames);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }
        //=======================>>>>>>> To Get Last Category Nodes <<<<<<<<<<<<===========================
        [HttpGet]
        [Route("Get_Child_Categories")]
        public IHttpActionResult Get_Child_Categories(int TopCategoryID, int VendorId)
        {
            try
            {
                if (VendorId != 0)
                {
                    string Get_Areas = "[iB_Get_Vendor_Assigned_Sub_CategoryList_Count]";
                    Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@TopCategoryID", TopCategoryID }, { "@VendorId", VendorId } };
                    var locations = _database.ProductListWithCount(Get_Areas, parameters).Resultset;
                    return Ok(locations);
                }
                else
                {
                    string Get_Areas = "[iB_Get_Sub_Categories_List_with_Ordered_Count]";
                    Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@TopCategoryID", TopCategoryID } };
                    var locations = _database.ProductListWithCount(Get_Areas, parameters).Resultset;
                    return Ok(locations);
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


        [HttpGet]
        [Route("Get_Child_CategoriesNew")]
        public IHttpActionResult Get_Child_CategoriesNew(int TopCategoryID, int VendorId)
        {
            try
            {
                if (VendorId != 0)
                {
                    string Get_Areas = "select ib.ID,ib.CategoryName,ib.ParentID From iH_Categories ib join iH_Categories ia on ia.ID != ib.ParentID JOIN Vendor_Categories VC ON VC.CatID = ib.ID Where ib.Top_Category_Id = " + TopCategoryID + " and VC.Vendor_ID = " + VendorId + " AND ib.Status = 10 and ib.Is_LeafNode=1  Group By ib.ID,ib.CategoryName,ib.ParentID order by 3 asc";
                    Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ProductId", TopCategoryID } };
                    var locations = _database.SelectQuery(Get_Areas, parameters);
                    return Ok(locations);
                }
                else
                {
                    string Get_Areas = "select ib.ID,ib.CategoryName,ib.ParentID From iH_Categories ib join iH_Categories ia on ia.ID != ib.ParentID Where ib.Top_Category_Id =" + TopCategoryID + "and ib.Status = 10 and ib.Top_Category_Id<> ib.ParentID and ib.ParentID <> 0 Group By ib.ID,ib.CategoryName,ib.ParentID order by 3 asc";
                    Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ProductId", TopCategoryID } };
                    var locations = _database.SelectQuery(Get_Areas, parameters);
                    return Ok(locations);
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


        //===================Get Pricing Product List======================//
        [HttpPost]
        [Route("Get_Products_BySKU")]
        public IHttpActionResult Get_Products_BySKU(VMModelsForInventory Cat)
        {
            try
            {
                string Stock_Moving = "[iB_Get_Products_List_By_Skus]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ProductIDs", Cat.FilterJson },{ "@unitid", Cat.Unit_ID }

                };
                var Stock_Moving_ListNew = _database.ProductListWithCount(Stock_Moving, parameters);
                return Ok(Stock_Moving_ListNew);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        //=======================>>>>>>> To Get vendor List Nodes <<<<<<<<<<<<===========================

        [HttpGet]
        [Route("GetVendorList")]
        public IHttpActionResult GetVendorList()
        {
            try
            {
                var cmd = "SELECT * FROM iHub_Vendors WHERE Vendor_Status = 1";
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

        [HttpGet]
        [Route("GetVendorListByCatID")]
        public IHttpActionResult GetVendorListByCatID(int CatID)
        {
            try
            {
                var cmd = "[iB_Get_Vendors_By_CategoryID]";
                Dictionary<string, object> parameter1 = new Dictionary<string, object> { { "@Catid", CatID } };
                var res = _database.ProductListWithCount(cmd, parameter1);
                return Ok(res);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        //===============>>>>>>>>To Get All Top Category List<<<<<<<<<<<<<<<<<<============
        [HttpGet]
        [Route("Top_Category_List")]
        public IHttpActionResult Top_Category_List(int VendorID)
        {
            try
            {
                if (VendorID != 0)
                {
                    var cmd = "select IH.ID,CategoryName from Vendor_Categories JOIN iH_Categories IH ON IH.ID = CatID WHERE IH.Status = 10 AND Vendor_ID = " + VendorID + " AND IH.ParentID = 0 GROUP BY IH.ID,CategoryName";
                    Dictionary<string, object> parameter1 = new Dictionary<string, object> { };
                    var res = _database.SelectQuery(cmd, parameter1);
                    return Ok(res);
                }
                else
                {
                    var cmd = "SELECT \"ID\",\"CategoryName\" FROM \"iH_Categories\" where \"ParentID\"=0 AND \"Status\"=10 ORDER BY 1";
                    Dictionary<string, object> parameter1 = new Dictionary<string, object> { };
                    var res = _database.SelectQuery(cmd, parameter1);
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
        [Route("Top_Category_List_PO")]
        public IHttpActionResult Top_Category_List_PO(int VendorID)
        {
            try
            {
                if (VendorID != 0)
                {
                    var cmd = "[iB_Get_Vendor_Assigned_CategoryList_Count]";
                    Dictionary<string, object> parameter1 = new Dictionary<string, object> { { "@VendorID", VendorID } };
                    var res = _database.ProductListWithCount(cmd, parameter1).Resultset;
                    return Ok(res);
                }
                else
                {
                    var cmd = "[iB_Get_Categories_List_with_Ordered_Count]";
                    Dictionary<string, object> parameter1 = new Dictionary<string, object> { };
                    var res = _database.SelectQuery(cmd, parameter1);
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
        //================================== Method To unit List==============
        [HttpGet]
        [Route("Get_Unit_List")]
        public IHttpActionResult Get_Unit_List(int HierarchyLevel)
        {
            try
            {
                var cmd = "SELECT * FROM iHub_Units_DC_WH_ST WHERE Unit_Hierarchy_Level =" + HierarchyLevel;
                Dictionary<string, object> parameter1 = new Dictionary<string, object> { };
                var res = _database.SelectQuery(cmd, parameter1);
                return Ok(res);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        //================================== Method To Get unit adress=============
        [HttpGet]
        [Route("Get_Unit_Address")]
        public IHttpActionResult Get_Unit_Address(int AddressID)
        {
            try
            {
                string Get_Areas = " SELECT dbo.iB_Get_Addres_Unit (@AddressId)";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@AddressId", AddressID } };
                var locations = _database.QueryFunction(Get_Areas, parameters);
                return Ok(locations);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        //================>>>>>>>>>>>>> Create New Purchase Order against Vendor <<<<<<<<<<<<<<<<=====================

        [HttpPost]
        [Route("Create_Vendor_PO")]
        public IHttpActionResult Create_Vendor_PO(Dictionary<string, object> InputProducts)
        {
            try
            {
                string Command = "[iB_Create_New_Vendor_Purchase_Order]";
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    { "@json", InputProducts["ProductsData"] },
                    { "@ProductJson", InputProducts["MasterQutation"] },
                    { "@App_type", InputProducts["type"]}
                };
                var ResultSet = _database.BusinessResultset(Command, parameters).Resultset;
                return Ok(ResultSet);
            }
            catch (Exception ex)
            {
                CommonMethods.LogError(ex);
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.NotFound, ex));
            }
        }

        //===============================get Purchase orders
        [HttpPost]
        [Route("Get_Generate_PO_Vendor_Details")]
        public IHttpActionResult Get_Generate_PO_Vendor_Details(VendorInvoiceCreation GF)
        {
            try
            {
                DateTime newdate = new DateTime();
                if (GF.From_Date != null && GF.From_Date != "")
                {
                    newdate = Convert.ToDateTime(GF.From_Date);
                    string Get_vendor_Categories = "[iB_Get_Vendor_Purchase_Orders_Details]";
                    Dictionary<string, object> parameters = new Dictionary<string, object>() {{ "@OrderID", GF.ID }, { "@EName", GF.Name },{ "@DateFrom",newdate },//{ "@DateTO",  Convert.ToDateTime(null) },
                        { "@Status", GF.Status }, { "@Page_Index", GF.PageIndex }, { "@Page_Size", GF.PageSize }};
                    var res = _database.ProductListWithCount(Get_vendor_Categories, parameters);
                    return Ok(res);
                }
                else
                {
                    string Get_vendor_Categories = "[iB_Get_Vendor_Purchase_Orders_Details]";
                    Dictionary<string, object> parameters = new Dictionary<string, object>() {{ "@OrderID", GF.ID }, { "@EName", GF.Name },{ "@DateFrom",GF.From_Date },//{ "@DateTO",  Convert.ToDateTime(null) },
                        { "@Status", GF.Status }, { "@Page_Index", GF.PageIndex }, { "@Page_Size", GF.PageSize }};
                    var res = _database.ProductListWithCount(Get_vendor_Categories, parameters);
                    return Ok(res);
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


        //===============================get Purchase orders
        [HttpPost]
        [Route("Get_Vendor_Order_Reports")]
        public IHttpActionResult Get_Vendor_Order_Reports(VendorInvoiceCreation GF)
        {
            try
            {
                DateTime newdate = new DateTime();
                if (GF.From_Date != null && GF.From_Date != "")
                {
                    newdate = Convert.ToDateTime(GF.From_Date);
                    string Get_vendor_Categories = "[iB_Get_Vendor_Order_Reports]";
                    Dictionary<string, object> parameters = new Dictionary<string, object>() {{ "@OrderID", GF.ID }, { "@EName", GF.Name },{ "@DateFrom",newdate },//{ "@DateTO",  Convert.ToDateTime(null) },
                        { "@Status", GF.Status }, { "@Page_Index", GF.PageIndex }, { "@Page_Size", GF.PageSize },{ "@VendorID", GF.VendorID }};
                    var res = _database.ProductListWithCount(Get_vendor_Categories, parameters);
                    return Ok(res);
                }
                else
                {
                    string Get_vendor_Categories = "[iB_Get_Vendor_Order_Reports]";
                    Dictionary<string, object> parameters = new Dictionary<string, object>() {{ "@OrderID", GF.ID }, { "@EName", GF.Name },{ "@DateFrom",GF.From_Date },//{ "@DateTO",  Convert.ToDateTime(null) },
                        { "@Status", GF.Status }, { "@Page_Index", GF.PageIndex }, { "@Page_Size", GF.PageSize },{ "@VendorID", GF.VendorID }};
                    var res = _database.ProductListWithCount(Get_vendor_Categories, parameters);
                    return Ok(res);
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



        //================================== Method To Get unit adress=============
        [HttpGet]
        [Route("Get_Vendor_details")]
        public IHttpActionResult Get_Vendor_details(int VendorID)
        {
            try
            {
                var cmd = "SELECT * FROM iHub_Vendors where Vendor_ID =" + VendorID;
                Dictionary<string, object> parameter1 = new Dictionary<string, object> { };
                var res = _database.SelectQuery(cmd, parameter1);
                return Ok(res);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        //===============================to get Purchase Order Invoice with Gst Details=====================//
        [HttpGet]
        [Route("GetPOInvoiceByPOId")]
        public IHttpActionResult GetPOInvoiceByPOId(int POID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                    { "@PurchaseOrderID", POID } };
                var Orders = _database.ProductListWithCount("IB_Invoice_Purchase_Order", parameters);
                return Ok(Orders);
            }
            catch (Exception ex)
            {
                CommonMethods.LogError(ex);
                log.Error("Error", ex);
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.NotFound, ex));
            }
        }
        //===============================to get Purchase Order Invoice with Gst Details=====================//
        [HttpGet]
        [Route("GetPOInvoiceByPOIdNew")]
        public IHttpActionResult GetPOInvoiceByPOIdNew(int POID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                    { "@PurchaseOrderID", POID } };
                var Orders = _database.ProductListWithCount("IB_Invoice_Purchase_Order_New", parameters);
                return Ok(Orders);
            }
            catch (Exception ex)
            {
                CommonMethods.LogError(ex);
                log.Error("Error", ex);
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.NotFound, ex));
            }
        }

        //================>>>>>>>>>>>>> To Change status Of Purchse Order By ID <<<<<<<<<<<<<<<<=====================
        [HttpPost]
        [Route("Change_Status_VPO")]
        public IHttpActionResult Change_Status_VPO(int PO_ID, int Status, Dictionary<string, object> InputProducts)
        {
            try
            {
                string Get_vendor_Categories = "[iB_Change_VPO_Status]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@VPO_ID", PO_ID }, { "@StatusID", Status }, { "@Comment", InputProducts["ProductsData"] } };
                var res = _database.ProductListWithCount(Get_vendor_Categories, parameters).Resultset;
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
        [Route("Update_Vendor_Status")]
        public IHttpActionResult Update_Vendor_Status(VMModelsForProduct data)
        {
            try
            {
                string Stock_Moving = "[iB_Update_PO_Vendor_Sent_Status]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@PO_ID", data.ID }

                };
                var Stock_Moving_ListNew = _database.QueryValue(Stock_Moving, parameters);
                return Ok(Stock_Moving_ListNew);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        //===============================get Purchase orders Onle Approved By Super Admin For Payment   
        [HttpPost]
        [Route("Get_Generate_VPO_Approved")]
        public IHttpActionResult Get_Generate_VPO_Approved(VendorInvoiceCreation GF)
        {
            try
            {
                string Get_vendor_Categories = "[iB_Get_Vendor_Purchase_Orders_Approved]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() {{ "@OrderID", GF.ID }, { "@EName", GF.Name },
                    { "@DateFrom", GF.DateFrom },{ "@DateTO",  GF.DateTo },
                        { "@Page_Index", GF.PageIndex }, { "@Page_Size", GF.PageSize }};
                var res = _database.ProductListWithCount(Get_vendor_Categories, parameters);
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

        //=================================get Approved Purchase Orde Payment Details
        [HttpGet]
        [Route("GetVendorPaymentTranscations")]
        public IHttpActionResult GetVendorPaymentTranscations(int POID)
        {
            try
            {
                string GetVendorPaymentTranscations = "[iB_Vendor_Payment_Transcations]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@POID", POID } };
                var Payment = _database.ProductListWithCount(GetVendorPaymentTranscations, parameters);
                return Ok(Payment);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }

        //=================== Make Payment For Purchase Orders ==========================//
        [HttpPost]
        [Route("MakePayMent_For_PurchaseOrders")]
        public IHttpActionResult MakePayMent_For_PurchaseOrders(PaymentModel VModel)
        {
            try
            {
                string Make_DuePayment = "[iB_Make_Payment_For_Purchase_Orders]";
                if (VModel.From_Date != null && VModel.From_Date != "")
                {
                    DateTime newdate = new DateTime();
                    newdate = Convert.ToDateTime(VModel.From_Date);
                    Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@POID", VModel.POID }, { "@PayingAmount",  VModel.Amount }, { "@paymentMode",  VModel.Payment_Mode_Type },
                     { "@ReferenceID", VModel.ReferenceID },{ "@TotalAmount", VModel.TotalAmount },{ "@PaymentReceivedDate", VModel.PaymentReceivedDate },
                    { "@BalanceAmount", VModel.BalanceAmount },{ "@paymentJson", VModel.pamentjson } ,{ "@PaymentNextDate", newdate },{ "@Comment", VModel.Description }};
                    var Result = _database.ProductListWithCount(Make_DuePayment, parameters);
                    return Ok(Result.Resultset);
                }
                else
                {
                    var newdate = VModel.From_Date;
                    Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@POID", VModel.POID }, { "@PayingAmount",  VModel.Amount }, { "@paymentMode",  VModel.Payment_Mode_Type },
                     { "@ReferenceID", VModel.ReferenceID },{ "@TotalAmount", VModel.TotalAmount },{ "@PaymentReceivedDate", VModel.PaymentReceivedDate },
                    { "@BalanceAmount", VModel.BalanceAmount },{ "@paymentJson", VModel.pamentjson } ,{ "@PaymentNextDate", newdate },{ "@Comment", VModel.Description }};
                    var Result = _database.ProductListWithCount(Make_DuePayment, parameters);
                    return Ok(Result.Resultset);
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

        //=== Method For Api For GetBank Details from Master table ===//
        [HttpGet]
        [Route("GetBankDetailsList_ByMasterName")]
        public IHttpActionResult GetBankDetailsList_ByMasterName(string MasterName)
        {
            try
            {
                string Bank_details = "iS_Get_Master_Values_List_By_Name";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@mastername", MasterName } };
                var bankDetails = _database.GetMultipleResultsList2(Bank_details, parameters);
                return Ok(bankDetails);
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
        [Route("ShowInBoundDetailsByPOID")]
        public IHttpActionResult ShowInBoundDetailsByPOID(int POID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                    { "@POID", POID } };
                var Orders = _database.GetMultipleResultsList("iB_Get_Inbound_Details_POID", parameters);
                return Ok(Orders);
            }
            catch (Exception ex)
            {
                CommonMethods.LogError(ex);
                log.Error("Error", ex);
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.NotFound, ex));
            }
        }


        //=======================>>>>>>> To Get_Vendor_Categories <<<<<<<<<<<<===========================
        [HttpGet]
        [Route("Get_Vendor_Categories")]
        public IHttpActionResult Get_Vendor_Categories(int VendorID)
        {
            try
            {
                string Get_VendorCat = "Get_Vendor_Categories_List";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@VendorID", VendorID } };
                var locations = _database.ProductListWithCount(Get_VendorCat, parameters);
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

        [HttpGet]
        [Route("Get_Vendor_All_Category_Products")]
        public IHttpActionResult Get_Vendor_All_Category_Products(int TopCatID, int CatID,int VendorID)
        {
            try
            {
                string Get_VendorCat = "iB_Get_Vendor_All_Category_Products";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@TopCatID", TopCatID }, { "@CatID", CatID },{ "@VendorID" , VendorID } };
                var locations = _database.ProductListWithCount(Get_VendorCat, parameters);
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
        [HttpGet]
        [Route("ShowVPOProductQty")]
        public IHttpActionResult ShowVPOProductQty(int POID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                    { "@POID", POID } };
                var Orders = _database.ProductListWithCount("iB_Get_Product_Qty_by_POID", parameters);
                return Ok(Orders);
            }
            catch (Exception ex)
            {
                CommonMethods.LogError(ex);
                log.Error("Error", ex);
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.NotFound, ex));
            }
        }

        [HttpPost]
        [Route("Get_All_PO_Orders_history")]
        public IHttpActionResult Get_All_PO_Orders_history(VMModelsForCategory gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Product_Id", gt.ProductIds }, { "@POID", gt.POID } };
                var res = _database.ProductListWithCount("iB_Get_PO_Orders_History", parameters);
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


        [HttpPost]
        [Route("Get_VendorPickup_Reports")]
        public IHttpActionResult Get_VendorPickup_Reports(VendorInvoiceCreation GF)
        {
            try
            {
                {
                    string Get_vendor_Categories = "[iB_Get_Vendor_Pickup_Reports]";
                    Dictionary<string, object> parameters = new Dictionary<string, object>() {{ "@OrderID", GF.ID }, { "@EName", GF.Name },{ "@DateFrom",GF.From_Date },//{ "@DateTO",  Convert.ToDateTime(null) },
                         { "@Status", GF.Status }, { "@VendorID", GF.VendorID },{"@PickupDate",GF.PickupDate } };
                    var res = _database.ProductListWithCount(Get_vendor_Categories, parameters);
                    return Ok(res);
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

        [HttpGet]
        [Route("ShowVendorAssignedProducts")]
        public IHttpActionResult ShowVendorAssignedProducts(int VendorID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                    { "@VendorID", VendorID } };
                var Orders = _database.ProductListWithCount("iB_Get_Products_ListOf_Vendor", parameters);
                return Ok(Orders);
            }
            catch (Exception ex)
            {
                CommonMethods.LogError(ex);
                log.Error("Error", ex);
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.NotFound, ex));
            }
        }



        //===================Get ProductList With Excel======================//
        [HttpPost]
        [Route("AddMoreProductList_Vendor")]
        public IHttpActionResult AddMoreProductList_Vendor(VMModelsForProduct Cat)
        {
            try
            {

                string ProList = "[iB_Get_More_ProductsOf_VendorSupply_new]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Filter_Json", Cat.FilterJson },
                                                                                           { "@Category_ID", Cat.CategoryID },
                                                                                           { "@ProductName", Cat.ProductName },
                                                                                           { "@SKU", Cat.SKU },
                                                                                           { "@PageSize", Cat.Pagesize },
                                                                                           { "@PageIndex", Cat.PageIndex },
                                                                                           { "@VendorID", Cat.VendorID } };
                var product_Get_List = _database.GetMultipleResultsList(ProList, parameters);
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
        //================================== Method To unit List==============
        [HttpGet]
        [Route("IsVendorRegistered")]
        public IHttpActionResult IsVendorRegistered(string MobileNumber)
        {
            try
            {
                var cmd = "SELECT Vendor_Phone_Number FROM iHub_Vendors WHERE Vendor_Phone_Number =" + MobileNumber;
                Dictionary<string, object> parameter1 = new Dictionary<string, object> { };
                var res = _database.SelectQuery(cmd, parameter1);
                return Ok(res);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }


        [HttpGet]
        [Route("VendorGSTNoCheck")]
        public IHttpActionResult VendorGSTNoCheck()
        {
            try
            {
                var cmd = "SELECT GST_PAN_Details FROM iHub_Vendors";
                Dictionary<string, object> parameter1 = new Dictionary<string, object> { };
                var res = _database.SelectQuery(cmd, parameter1);
                return Ok(res);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        #endregion "END of -- Vendor Management"
    }
}
