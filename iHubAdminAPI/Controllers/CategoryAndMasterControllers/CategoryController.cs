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
namespace iHubAdminAPI.Controllers
{
    [RoutePrefix("api/Category")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CategoryController : ApiController
    {
        string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private static readonly ILog log = LogManager.GetLogger(typeof(CategoryController).FullName);
        public SQLDatabase _database;


        public CategoryController()
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


        string Get_All_Categories = "[iAdmin_Get_All_Categories]";
        string Get_AllPackages = "[iAdmin_Get_All_Packages]";
        string Category_AttributeAlias = "[iAdmin_Update_Alias_Names]";
        string Category_Attributenew = "[iAdmin_Get_Category_Attributes]";
        string Category_GetListnew = "[iAdmin_Get_Category_List]";
        string Category_AddListnew = "[iAdmin_Add_Update_Category_Items_New]";
        string CatBinLocation_AddList = "[iAdmin_Add_Update_Category_Bin_Locations]";
        string ProductCatBinLocation_Add_Update_List = "[iAdmin_Add_Update_Product_Category_Bin_Locations]";
        string Grouped_Product_List = "[iAdmin_Get_Grouped_Products_By_Filters]";
        string product_GetList_New = "[iAdmin_Get_Category_Products_By_Filter_With_Paging_And_Excel]";
        string Get_All_EMI_Orders = "[iAdmin_Get_All_EMI_Orders]";
        string Shuffle_Category_And_Products = "[iAdmin_Shuffle_Category_And_Products]";
        string Add_Category_Attribute = "[iAdmin_Add_Or_Edit_Category_Attributes]";
        string Cash_Deposits = "[iAdmin_Cash_Deposits]";
        string Get_Cash_Deposits_List = "[iAdmin_Get_Cash_Deposits]";
        string Get_Units__Cluster = "[iAdmin_Get_Unit_List_With_UnitLevel_Cluster]";
        string Get_Category_Unit_Bin_Locations_List = "[iAdmin_Get_Category_Bin_Locations]";
        string CatBinLocation_del = "[iAdmin_Delete_Category_Bin_Locations]";
        string Get_Top_Categories = "[iAdmin_Get_Top_Category_Names]";

        #endregion "BEGIN of -- SP Names"

        #region "BEGIN of -- Category Management"

        // =================================To get All Category==================================== 
        [HttpPost]
        [Route("GetAllCategories")]
        public IHttpActionResult GetAllCategories()
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { };
                var Get_All_Categories_List = _database.ProductListWithCount(Get_All_Categories, parameters);
                return Ok(Get_All_Categories_List);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        // =================================To get All Category==================================== 
        [HttpPost]
        [Route("GetTopCategories")]
        public IHttpActionResult GetTopCategories()
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { };
                var Get_Top_Categories_List = _database.ProductListWithCount(Get_Top_Categories, parameters);
                return Ok(Get_Top_Categories_List);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        // =================================To Get_Category_Attributes====================================
        [HttpGet]
        [Route("Get_Category_Attributes")]
        public IHttpActionResult Get_Category_Attributes(int CategoryID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@categoryid", CategoryID } };
                var categoryattributes = _database.Query(Category_Attributenew, parameters);
                return Ok(categoryattributes);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Get_Category_Attributes---Error---------", ex);
                return Ok(cs);
            }
        }
        [HttpPost]
        [Route("Get_Category_Unit_Bin_Locations")]
        public IHttpActionResult Get_Category_Unit_Bin_Locations(VMModelForProductCategoryUnitBinLocation model)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { {"@Category_ID", model.CategoryID },
                    {"@UnitName", model.UnitName}, { "@TopCategoryName", model.TopCategoryName},
                    { "@DateFrom", model.CreatedDateFrom }, {"@DateTo", model.CreatedDateTo }, { "@PageIndex", model.PageIndex},
                    {"@PageSize", model.PageSize } };

                var catBinLocList = _database.ProductListWithCount(Get_Category_Unit_Bin_Locations_List, parameters);

                return Ok(catBinLocList);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Get_Category_Unit_Bin_Locations----Error-------", ex);
                return Ok(cs);
            }
        }
        // =================================To Get Category List====================================
        [HttpGet]
        [Route("GetCategoryList")]
        public IHttpActionResult GetCategoryList(int ParentID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@parentid", ParentID } };
                var categoryGetList = _database.Query(Category_GetListnew, parameters);
                return Ok(categoryGetList);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Get_Category_Attributes---Error---------", ex);
                return Ok(cs);
            }

        }
        // =================================To Add Edit Category====================================
        [HttpPost]
        [Route("Add_Edit_Category")]
        public IHttpActionResult Add_Edit_Category(VMModelsForCategory Cat)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@CatID", Cat.Category_Id },
                                                                                          { "@Category_Name", Cat.Category_Name },
                                                                                          { "@Category_Description", Cat.Category_Description },
                                                                                          { "@Category_Parent_Id", Cat.Category_Parent_Id },
                                                                                          { "@Category_Status", Cat.Category_Status },
                                                                                          { "@Category_Priority", Cat.Category_Priority },
                                                                                          { "@Category_AliasNames", Cat.Category_AliasNames },
                                                                                          { "@UserID", Cat.User_Id },
                                                                                          { "@BatchNumber", Cat.Is_Batch_Number },
                                                                                            { "@ManufactureDate", Cat.Is_Manufacture_Date },
                                                                                             { "@ExpiryDate", Cat.Is_Expiry_Date },
                                                                                             { "@IMEINumber", Cat.Is_IMEI_Number },
                                                                                             { "@sCategoryType", Cat.sCategoryType }
                                                                                              };
                var category_Add_Update_List = _database.Query(Category_AddListnew, parameters).ToList();
                return Ok(category_Add_Update_List);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Get_Category_Attributes---Error---------", ex);
                return Ok(cs);
            }

        }
        // =================================To UploadCategoryImage====================================
        [HttpPost]
        [Route("UploadCategoryImage")]
        public IHttpActionResult UploadCategoryImage(int CategoryID)
        {
            try
            {

                HttpPostedFile Photo = HttpContext.Current.Request.Files[0];
                string str = string.Empty;
                string type = string.Empty;
                string physicalPath = HttpContext.Current.Server.MapPath("~/" + "Images/CategoryImages/");
                if (!System.IO.File.Exists(physicalPath))
                {
                    Directory.CreateDirectory(physicalPath);
                }
                string imgPath = HttpContext.Current.Server.MapPath("~/" + "images/CategoryImages/" + CategoryID + ".jpg");
                Photo.SaveAs(imgPath);
                CustomResponse cs = new CustomResponse();
                cs.Response = "Image uploaded successfully.";
                cs.ResponseID = Convert.ToInt32(CategoryID);
                return Ok(cs);
            }
            catch (Exception ex)
            {
                log.Error("UploadCategoryImage--Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }
        // =================================To Add Edit Category====================================
        [HttpPost]
        [Route("Add_Edit_Category_Bin_Locations")]
        public IHttpActionResult Add_Edit_Category_Bin_Locations(VMModelForProductCategoryUnitBinLocation CatBinLoc)
        {
            try
            {
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ID", CatBinLoc.ID },
                                                                                          { "@Category_ID", CatBinLoc.CategoryID },
                                                                                          { "@Unit_ID", CatBinLoc.UnitID },
                                                                                          { "@Location", CatBinLoc.Location },
                                                                                          { "@UserID", User_Id},
                                                                                          { "@ApplySubCatFlag", CatBinLoc.ApplySubCatFlag}
                                                                                          };
                var category_Add_Update_List = _database.Query(CatBinLocation_AddList, parameters).ToList();
                return Ok(category_Add_Update_List);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Add_Edit_Category_Attributes---Error---------", ex);
                return Ok(cs);
            }
        }

        [HttpPost]
        [Route("Add_Edit_Product_Category_Bin_Locations")]
        public IHttpActionResult Add_Edit_Product_Category_Bin_Locations(VMModelForProductCategoryUnitBinLocation ProdCatBinLoc)
        {
            try
            {
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                Dictionary<string, object> parameters = new Dictionary<string, object>() { {"@binid", ProdCatBinLoc.ID },
                                                                                          { "@productids", ProdCatBinLoc.IDs },
                                                                                          { "@categoryid", ProdCatBinLoc.CategoryID },
                                                                                          { "@unitid", ProdCatBinLoc.UnitID },
                                                                                          { "@location", ProdCatBinLoc.Location },
                                                                                          { "@producttype", ProdCatBinLoc.ProductType },
                                                                                          { "@userID", User_Id}
                                                                                          };
                var category_Add_Update_List = _database.Query(ProductCatBinLocation_Add_Update_List, parameters).ToList();
                return Ok(category_Add_Update_List);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Add_Product_Category_Attributes---Error---------", ex);
                return Ok(cs);
            }

        }


        [HttpPost]
        [Route("Delete_Category_Bin_Locations")]
        public IHttpActionResult Delete_Category_Bin_Locations(VMModelForProductCategoryUnitBinLocation CatBinLoc)
        {
            try
            {
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ID", CatBinLoc.ID }, { "@userid", User_Id } };
                var del = _database.Query(CatBinLocation_del, parameters).ToList();
                return Ok(del);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Delete_Category_Attributes---Error---------", ex);
                return Ok(cs);
            }
        }
        // =================================To Update Records To Table Alias===================================
        [HttpPost]
        [Route("UpdateRecordsToTable_Alias")]
        public IHttpActionResult UpdateRecordsToTable_Alias(VMModelsForCategory Model)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@AliasNames", Model.Category_Name }
                    ,{ "@category_Id", Model.Category_Id } };
                var categoryattributes = _database.Query(Category_AttributeAlias, parameters);
                return Ok(categoryattributes);


            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        // =================================To Get Product Groups List====================================
        [HttpPost]
        [Route("Get_Product_Groups_List")]
        public IHttpActionResult Get_Product_Groups_List(VMModelsForCategory vmodel)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Filter_Json", vmodel.FilterJson }
                  , { "@Category_ID", vmodel.Category_Id }, { "@ProductName",vmodel.ProductName }, { "@pagesize", vmodel.Pagesize }
                  , { "@pageindex", vmodel.PageIndex }, {"@GroupID",vmodel.ID } };
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
        // =================================To UpdateRecordsToTable=========================================
        [HttpPost]
        [Route("UpdateRecordsToTable")]
        public IHttpActionResult UpdateRecordsToTable(VMModelsForCategory gt)
        {
            try
            {
                if (gt.ID == null || gt.ID < 0 || gt.JsonData == null || gt.TableName == null || gt.TableName == string.Empty || gt.JsonData == string.Empty)
                {
                    return Ok(0);
                }
                List<string[]> Rows = new List<string[]>();
                List<string> Columns = new List<string>();
                var lst = JsonConvert.DeserializeObject<Dictionary<string, string>>(gt.JsonData);

                string UpdateString = "";
                foreach (var item in lst)
                {
                    UpdateString = UpdateString + ",\"" + item.Key + "\"='" + item.Value + "'";
                }
                UpdateString = UpdateString.TrimStart(',').TrimEnd(',');
                var res = _database.UpdateRecordsToTable(gt.TableName, UpdateString, gt.ID);
                CustomResponse cs = new CustomResponse();
                cs.Response = "Status changed  Successfully";
                cs.ResponseID = Convert.ToInt32(res);
                return Ok(cs);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("UpdateRecordsToTable--Error---", ex);
                return Ok(cs);

            }
        }
        // =================================To get Products for a given Category==================================== 
        [HttpPost]
        [Route("GetProductsForCategory")]
        public IHttpActionResult GetProductsForCategory(VMModelsForCategory sp)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Filter_Json", sp.FilterJson },
                                                                                           { "@Category_ID", sp.Category_Id },
                                                                                           { "@ProductName", sp.ProductName },
                                                                                           { "@SKU", sp.SKU },
                                                                                           { "@PageSize", sp.Pagesize },
                                                                                           { "@PageIndex", sp.PageIndex },
                                                                                                                            };
                var product_Get_List = _database.ProductListWithCount(product_GetList_New, parameters);
                return Ok(product_Get_List);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("GetProductsForCategory--Error---------", ex);
                return Ok(cs);
            }
        }
        //=========================Update_Shuffled_CategoryAndProducts==============================================
        [HttpPost]
        [Route("Update_Shuffled_CategoryAndProducts")]
        public IHttpActionResult Update_Shuffled_CategoryAndProducts(VMModelsForCategory sp)
        {

            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Product_Ids", sp.ProductIds }, { "@Current_Category_ID", sp.Category_Id }, { "@Target_Category_ID", sp.TargetCategoryID }, { "@Shuffle_Type", sp.ShuffleType } };

                var res = _database.QueryValue(Shuffle_Category_And_Products, parameters);

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
        // =================================To AddCategoryAttribute====================================
        [HttpPost]
        [Route("AddCategoryAttribute")]
        public IHttpActionResult AddCategoryAttribute(VMModelsForCategory cmodel)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@categoryid", cmodel.ID }, { "@jsondata", cmodel.JsonData } };
                var SaveCatAttr = _database.Query(Add_Category_Attribute, parameters);
                return Ok(SaveCatAttr);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("AddCategoryAttribute--Error---------", ex);
                return Ok(cs);
            }
        }
        //===============To Get All Packages List ========================== //
        [HttpPost]
        [Route("Get_All_Packages")]
        public IHttpActionResult Get_All_Packages()
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { };
                var Get_All_Packages_List = _database.ProductListWithCount(Get_AllPackages, parameters);
                return Ok(Get_All_Packages_List);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        #endregion "END of -- Category Management"

        #region "BEGIN of -- Cash_Deposits"

        //----------------------------------To Get Cash_Deposits
        [HttpPost]
        [Route("iH_Cash_Deposits")]
        public IHttpActionResult iH_Cash_Deposits(VMModelsForMaster GF)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                    //                                      { "@unit_id", GF.LocationIDs },
                                                            { "JsonData", GF.JsonData},
                                                            { "@cluster_id", GF.Cluster_ID },
                                                            { "@status", GF.Status }
                                                        };
                var res = _database.QueryValue(Cash_Deposits, parameters);
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

        //----------------------------------To Get Cash_Deposits
        [HttpPost]
        [Route("Get_Cash_Deposits")]
        public IHttpActionResult Get_Cash_Deposits(VMModelsForMaster GF)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                { {"@Unit_Id" ,GF.UnitId} , {"@Cluster_Id",GF.Cluster_ID }, {"@Date_From",GF.CreatedDate}, {"@Date_To",GF.LastUpdateDate},{"@UserType",GF.UsertypeName},{ "@UserName",GF.UserName} };
                var categorylist = _database.GetMultipleResultsList2(Get_Cash_Deposits_List, parameters);
                return Ok(categorylist);
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
        //----------------------------------To Get Units Data by heirarchy level
        [HttpPost]
        [Route("Get_Units_Data_Cluster")]
        public IHttpActionResult Get_Units_Data_Cluster(VMPagingResultsPost vmodel)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@unit_type", vmodel.Unit_Type }, { "@parent_id", vmodel.WareHouseID } };
                var unit_list = _database.ProductListWithCount(Get_Units__Cluster, parameters);
                return Ok(unit_list);
            }
            catch (Exception ex)
            {
                log.Error("Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }


        #endregion "END of -- Cash_Deposits"

        #region "BEGIN of -- EMI_Orders"

        //=======================>>>>>>> Get All EMI Orders <<<<<<<<<<<<===========================
        [HttpPost]
        [Route("iH_Get_All_EMI_Orders")]
        public IHttpActionResult iH_Get_All_EMI_Orders(VMModelsForCategory GF)
        {
            try
            {

                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Order_Number", GF.OrderNumber }, { "@Mobile_Number", GF.MobileNumber }, { "@Tenure", GF.Tenure }, { "@Order_Date_From", GF.OrderDateFrom }, { "@Order_Date_To", GF.OrderDateTo }, { "@Unit_Name", GF.UnitName }, { "@Status", GF.Status }, { "@Page_Index", GF.PageIndex }, { "@Page_Size", GF.Pagesize } };
                var res = _database.ProductListWithCount(Get_All_EMI_Orders, parameters);
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


        #endregion "END of -- EMI_Orders"

        #region Category_Type_Mapping

        [HttpPost]
        [Route("Create_CategoryType_Mapping")]
        public IHttpActionResult Create_CategoryType_Mapping(VMModelsForCategory oCat)
        {
            try
            {
                string commandText2 = "[Admin_Category_Type_Mapping]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ID", oCat.Category_Id }, { "@sCategoryType", oCat.sCategoryType }, { "@iCount", oCat.iParentID } };
                var oCTM = _database.Query(commandText2, parameters);
                return Ok(oCTM);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("AddCategoryAttribute--Error---------", ex);
                return Ok(cs);
            }
        }

        //----------------------------------To Get Cash_Deposits
        [HttpPost]
        [Route("iH_StoreKeepers_Cash_Deposits")]
        public IHttpActionResult iH_StoreKeepers_Cash_Deposits(VMModelsForMaster GF)
        {
            try
            {
                string SK_Cash_Deposits = "[iAdmin_StoreKeeper_Cash_Deposits]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@SkData", GF.LocationIDs } };
                var res = _database.ProductListWithCount(SK_Cash_Deposits, parameters);
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
        #endregion Category_Type_Mapping
    }
}
