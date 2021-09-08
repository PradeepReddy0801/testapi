using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using Excel;
using System.Web.Configuration;
using System.IO;
using System.Net.Http;
using System.Configuration;
using System.Net.Http.Headers;
using System.Data.SqlClient;
using iHubAdminAPI.Models;
using System.Web.Http.Cors;
using System.Data.Entity.Infrastructure;
using AspNet.Identity.SQLDatabase;
using System.Web.Script.Serialization;
using iHubAdminAPI.Models.MasterDataAndUnits;
using iHubAdminAPI.Models.ProductAndPrice;
using Microsoft.AspNet.Identity;
using System.Text;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Data.Entity;
using iHubAdminAPI.Models.AppModels;
using System.Web.Caching;

namespace iHubAdminAPI.Controllers
{
    [RoutePrefix("api/MasterDataAndUnits")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MasterDataAndUnitsController : ApiController
    {
        string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private static readonly ILog log = LogManager.GetLogger(typeof(MasterDataAndUnitsController).FullName);
        public SQLDatabase _database;
        iHubDBContext dbContext = new iHubDBContext();
        private ApplicationUserManager _userManager;
        private class CustomResponse
        {
            internal string Response;
            internal int ResponseID;
        }
        public MasterDataAndUnitsController()
        {
            _database = new SQLDatabase();
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        #region "BEGIN of -- SP Names"

        // =================================Command Text For Api Calls====================================================
        string Units_Table_Tree = "[iAdmin_Get_Units_List_By_Parent_ID]";
        string NewUnitAdd = "[iAdmin_Create_New_Unit_with_Parent_ID]";
        string ChangeUnitStatus = "[iAdmin_Change_Unit_Status]";
        string Get_Results_With_Paging_Count = "[iAdmin_Get_Results_With_Paging_And_Count]";
        string Add_Edit_Master_Data = "[iAdmin_Add_Edit_Masters_Data]";
        string Get_Masters_Data = "[iAdmin_Get_Master_Values_List]";
        string Get_Master_Location_By_Parent_ID = "[iAdmin_Get_Master_Locations_List_By_Parent_ID]";
        string Get_Master_Location = "[iAdmin_Get_Master_Locations]";
        string Get_HSN_Code = "[iAdmin_Add_Update_HSN_Code]";
        string Get_HSN_List = "[iAdmin_Get_HSN_Code_List]";
        string Insert_Default_Messages = "[iAdmin_Save_Default_Messages]";
        string Update_Location_Details = "[iAdmin_Add_Update_Location_Details]";
        string Add_Location_Details = "[iAdmin_Add_Update_Location_Details]";
        string GetCluster = "[iAdmin_GetClusterID_By_MandalID]";
        string Assign_mandals_pincodes = "[iAdmin_Assign_Mandals_To_Cluster]";
        string All_Locations = "[iAdmin_Get_All_Locations]";
        string EMI_Master_Data = "[iAdmin_iHub_EMI_Master_Data]";
        string ADD_Or_Update_EmiConfiguration = "[iAdmin_ADD_Or_Update_EmiConfiguration]";
        string Upload_HSNCode = "[iAdmin_Add_Update_HSN_Code]";
        string VerificationOTP = "iAdmin_Get_PhoneNumber_Based_On_Id";
        string Get_UnitAddress = "[iAdmin_Get_Address_Details_DCUnitID]";
        string Get_Locations_By_Pincode = "[iAdmin_Get_Locations_By_Pincode]";
        string Units_Tree = "[iAdmin_Get_Units_Tree_Data]";
        string Update_Default_Messages = "[iAdmin_Update_Default_Message]";
        string Get_Masters_List = "[iAdmin_Get_Master_Values_List_By_Name]";
        string View_Manual = "[iAdmin_Get_All_Manuals]";
        string Add_Help_Manual = "[iAdmin_Add_HelpManual]";
        string Get_Role_Names = "[iAdmin_Get_All_Roles_For_Manual_By_Role_ID]";
        string Save_BottomLinks_Data = "[iAdmin_Save_Update_BottomLinks_Description]";
        string Add_mandals_Data = "[iAdmin_Add_Mandals_By_Excel]";
        string Get_Home_Products_By_Flag = "[iAdmin_Get_Home_Products_By_Flag]";
        string Save_Menu_Strip_Item = "[iAdmin_Save_Menu_Strip_Item]";
        string Get_Menu_Strip_Item = "[iAdmin_Get_Menu_Strip_Item]";
        string Get_Foot_Menu_Strip_Item = "[iAdmin_Get_Foot_Menu_Strip_Item]";
        string Save_Foot_Menu_Strip_Item = "[iAdmin_Save_Foot_Menu_Strip_Item]";
        string Get_Franchise_Master_Location_By_ParentID = "[Get_Franchise_Master_Locations_By_ParentID]";
        string Get_Franchise_Master_Locations = "[Get_Franchise_Master_Locations]";
        string Update_Franchise_Location_Details = "[iAdmin_Add_Update_Franchise_Location_Details]";
        string Add_Franchise_Location_Details = "[iAdmin_Add_Update_Franchise_Location_Details]";
        string Get_TopLevelMenu_Strip_Item = "[iAdmin_Get_TopLevelMenu_Strip_Item]";
        string Save_Update_DomainMasterData = "[iAdmin_Manage_DomainsMasterData]";

        #endregion "END of -- SP Names"

        #region "BEGIN of -- Function Names"

        string Get_Areas = "SELECT dbo.iS_Get_Address_Area_Informaion_Location_Id(@Child_Location_Id)";

        #endregion "END of -- Function Names"



        #region "BEGIN of -- MasterDataManagement"

        // =================================To GetDynamicMenu====================================================

        [Authorize]
        public IHttpActionResult GetDynamicMenu()
        {
            try
            {
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                string Menu = "[GetDynamicMenu]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@UserId", User_Id } };
                var q = _database.QueryValue(Menu, parameters);
                return Ok(q);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("GetDynamicMenu---Error---------", ex);
                return Ok(cs);
            }
        }

        //==================================================== Get Unit Table Tree details=======================================
        [HttpPost]
        [Route("Get_Units_Table_Tree")]
        public IHttpActionResult Get_Units_Table_Tree(int parentid)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Parent_Id", parentid } };
                var Get_Units_Tree = _database.ProductListWithCount(Units_Table_Tree, parameters);
                return Ok(Get_Units_Tree);
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
        [Route("Change_Unit_Status")]
        public IHttpActionResult Change_Unit_Status(VMModelsForMaster ba)
        {
            try
            {
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ID", ba.ID }, { "@Status", ba.Status }, { "@UpdatedBy", User_Id } };
                var Chg_Status = _database.ProductListWithCount(ChangeUnitStatus, parameters);
                return Ok(Chg_Status);
            }
            catch (Exception ex)
            {
                log.Error("Error-----Change_Unit_Status--------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        //==================================================== Get new Units details===================================
        [HttpPost]
        [Route("AddNewUnit")]
        public IHttpActionResult AddNewUnit(VMModelsForMaster ba)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Sub_Unit_ID", ba.Sub_Unit_ID },

                                                                                         { "@Unit_Name", ba.Unit_Name },
                                                                                         { "@Email", ba.Email },
                                                                                         { "@Phone_Number", ba.Phone_Number },
                                                                                         { "@ContactName", ba.ContactName },
                                                                                         { "@AddressLine_One", ba.AddressLine_One },
                                                                                         { "@AddressLine_Two", ba.AddressLine_Two},
                                                                                         { "@VillageLocation_ID", ba.VillageLocation_ID},
                                                                                         { "@Suggested_UserName", ba.Suggested_UserName},
                                                                                         { "@Unit_Additional_Data", ba.Unit_Additional_Data},
                                                                                         { "@isWHcumStore", ba.isWHcumStore}};
                var Add_New_Unit = _database.QueryValue(NewUnitAdd, parameters);
                if (Convert.ToInt32(Add_New_Unit) > 0)
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["APIBaseUrl"].ToString());
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "Your Oauth token");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    ChangePasswordBindingModel user = new ChangePasswordBindingModel();
                    user.OldPassword = "Store.123";
                    user.ConfirmPassword = ba.Password;
                    user.NewPassword = ba.Password;
                    user.user_ID = Add_New_Unit.ToString();
                    var buyer_response = client.PostAsJsonAsync("api/Account/Change_Default_Password", user).Result.Content.ReadAsStringAsync();
                    if (buyer_response.Result == "true")
                    {
                        return Ok("Unit Added Successfully");
                    }
                    else
                    {
                        return Ok(buyer_response);
                    }
                }
                else if (Convert.ToInt32(Add_New_Unit) == 0)
                {
                    return Ok("User Name Alreay Exists");
                }
                else
                {
                    return Ok(Add_New_Unit);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        //============================Get Results With Paging=======================================
        [HttpPost]
        [Route("GetResultswithPaging")]
        public IHttpActionResult GetResultswithPaging(VMModelsForMaster gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@tablename", gt.TableName }, { "@pageindex", gt.PageIndex }, { "@pagesize", gt.PageSize } };
                var getdetails = _database.ProductListWithCount(Get_Results_With_Paging_Count, parameters);
                return Ok(getdetails);
            }
            catch (Exception ex)
            {
                log.Error("GetResultswithPaging--Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        // =================================To Get Masters_Data====================================================
        [HttpPost]
        [Route("GetMasterDataList")]
        public IHttpActionResult GetMasterDataList(int? ParentID, string searchtext)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@parentid", ParentID }, { "@searchtext", searchtext } };
                var Masters_Data_Result = _database.ProductListWithCount(Get_Masters_Data, parameters);
                return Ok(Masters_Data_Result);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("GetMasterDataList--Error---------", ex);
                return Ok(cs);
            }
        }

        // =================================To Add_Edit_Masters_Data=========================================
        [HttpPost]
        [Route("Add_Edit_Masters_Data")]
        public IHttpActionResult Add_Edit_Masters_Data(VMModelsForMaster gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@master_data_id", gt.ID }, { "@item_name", gt.Name }
                                                                                          ,{ "@item_description", gt.Description },{ "@item_status", gt.Status }
                                                                                           ,{ "@item_createdby", gt.Createdby },{ "@item_updatedby", gt.Updatedby },{ "@parent_master_id", gt.ParentID }};
                var Master_data = _database.QueryValue(Add_Edit_Master_Data, parameters);
                return Ok(Master_data);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                cs.ResponseID = 0;
                log.Error("Add_Edit_Masters_Data--------Error---------", ex);
                return Ok(cs);
            }
        }

        // =================================To UpdateRecordsToTable=========================================
        [HttpPost]
        [Route("UpdateRecordsToMastersTable")]
        public IHttpActionResult UpdateRecordsToMastersTable(VMModelsForMaster gt)
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
                var Update_MastersTable = _database.UpdateRecordsToMasterTable(gt.TableName, UpdateString, gt.ID);
                CustomResponse cs = new CustomResponse();
                cs.Response = "Item added Successfully";
                cs.ResponseID = Convert.ToInt32(Update_MastersTable);
                return Ok(cs);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---", ex);
                return Ok(cs);

            }
        }

        //========================= Get Location Details=========================================
        [HttpPost]
        [Route("GetLocationDetails")]
        public IHttpActionResult GetLocationDetails(VMModelsForMaster GF)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Parent_ID",GF.ParentID }, { "@Pageindex", GF.PageIndex }
                   , { "@Pagesize", GF.PageSize } , { "@searchtext", GF.Searchtext } };
                var Get_Location_Details = _database.ProductListWithCount(Get_Master_Location_By_Parent_ID, parameters);
                return Ok(Get_Location_Details);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }

        //==================================================== Get Master Locations details========================================
        [HttpPost]
        [Route("Get_Master_Locations")]
        public IHttpActionResult Get_Master_Locations()
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { };
                var Get_Master_Locations = _database.ProductListWithCount(Get_Master_Location, parameters);
                return Ok(Get_Master_Locations);
            }
            catch (Exception ex)
            {
                log.Error("Get_Master_Locations---Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        //========================= Add HSN CODE=========================================
        [HttpPost]
        [Route("Add_HSNCode")]
        public IHttpActionResult Add_HSNCode(int HSN_Code_Id, string Gst_Percentage)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@HSN_Code_Id", HSN_Code_Id }, { "@Gst_Percentage", Gst_Percentage } };
                var res = _database.ProductListWithCount(Get_HSN_Code, parameters);
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

        //=========================  HSN CODE List=========================================
        [HttpPost]
        [Route("HSN_Code_List")]
        public IHttpActionResult HSN_Code_List(string searchtext)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@searchtext", searchtext } };
                var res = _database.ProductListWithCount(Get_HSN_List, parameters);
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

        //===========================To Get Save Default Messages===============================================================
        [HttpPost]
        [Route("Save_Default_Messages")]
        public IHttpActionResult Save_Default_Messages(VMModelsForMaster gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@user_names", gt.Name }, { "@messages", gt.Message } };
                var res = _database.QueryValue(Insert_Default_Messages, parameters);
                return Ok(res);
            }
            catch (Exception ex)
            {
                log.Error("Save_Default_Messages---Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        //================= Create Location details=========================================
        [HttpPost]
        [Route("AddLocationDetails")]
        public IHttpActionResult AddLocationDetails(VMModelsForMaster GF)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Location_Id",GF.ID }, { "@Parent_Id", GF.StoreID}
                    , { "@Loc_Name",GF.Name }, { "@Hierarchy_level",GF.PageIndex }, { "@PinCode",GF.Message }, { "@User_ID",1 } };
                var res = _database.QueryValue(Add_Location_Details, parameters);
                return Ok(res);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("AddLocationDetails---Error---------", ex);
                return Ok(cs);
            }
        }

        //================================= update Location details======================================================
        [HttpPost]
        [Route("UpdateLocationDetails")]
        public IHttpActionResult UpdateLocationDetails(VMModelsForMaster GF)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Location_Id",GF.ID }, { "@Parent_Id", GF.StoreID}
                    , { "@Loc_Name",GF.Name }, { "@Hierarchy_level",GF.PageIndex }, { "@PinCode",GF.Message }, { "@User_ID",1 } };
                var res = _database.QueryValue(Update_Location_Details, parameters);
                return Ok(res);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("UpdateLocationDetails---Error---------", ex);
                return Ok(cs);
            }
        }

        //================================= Get Cluster By Mandal ID======================================================
        [HttpGet]
        [Route("GetClusterByMandalID")]
        public IHttpActionResult GetClusterByMandalID(int mandalID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@madal_id", mandalID } };
                var locations = _database.QueryValue(GetCluster, parameters);
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

        //================================= Get Assign Pincodes To Mandal======================================================
        [HttpPost]
        [Route("Assign_Pincodes_ToMandal")]
        public IHttpActionResult Assign_Pincodes_ToMandal(VMModelsForMaster vm)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@madal_ids", vm.FilterJson }, { "@cluster_id", vm.ClusterFranchiseID } };
                var locations = _database.QueryValue(Assign_mandals_pincodes, parameters);
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

        //====================================To Get Address By UnitID========================================================
        [HttpGet]
        [Route("Get_Address_By_UnitID")]
        public IHttpActionResult Get_Address_By_UnitID(int heirarchylevel, int UnitID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@hierarchy_level", heirarchylevel }, { "@distribution_channel_id", UnitID } };
                var locations = _database.ProductListWithCount(Get_UnitAddress, parameters);
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

        //==================================To Get All Locations=========================================
        [HttpPost]
        [Route("GetAllLocations")]
        public IHttpActionResult GetAllLocations()
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { };
                var result = _database.ProductListWithCount(All_Locations, parameters);

                return Ok(result);
            }
            catch (Exception ex)
            {
                log.Error("GetAllCategories--Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        //===================To Get iHub Emi Master data=================================================
        [HttpGet]
        [Route("Get_iHub_EMI_Master_Data")]
        public IHttpActionResult Get_iHub_EMI_Master_Data()
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { };
                var totalData = _database.ProductListWithCount(EMI_Master_Data, parameters);
                return Ok(totalData.Resultset);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }

        //======================To Add or update  config===================================================================
        [HttpPost]
        [Route("AddOrUpdateConfig")]
        public IHttpActionResult AddOrUpdateConfig(List<EMISlab> AllCat)
        {
            try
            {
                foreach (var i in AllCat)
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>() {
                                                                                                { "@EMI_Master_Data_ID_In", i.EMI_Master_Data_ID },
                                                                                                { "@EMI_Slab_Lower_Limit_In", i.EMI_Slab_Lower_Limit },
                                                                                                { "@EMI_Slab_Upper_Limit_In", i.EMI_Slab_Upper_Limit },
                                                                                                { "@Processing_Fee_In", i.Processing_Fee },
                                                                                                { "@No_Of_EMIs_Allowed_In", i.No_Of_EMIs_Allowed },
                                                                                                { "@Interest_Rate_In", i.Interest_Rate } };
                    var res = _database.ProductListWithCount(ADD_Or_Update_EmiConfiguration, parameters);
                }
                return Ok();
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

        //=============================== UploadHSNcode=================================
        [HttpPost]
        [Route("UploadBulkHSNcode")]
        public IHttpActionResult UploadBulkHSNcode()
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
                            int GSTPercentage = 0;
                            int HSNCode = 0;
                            int j = 1;
                            product = new Dictionary<string, string>();
                            if (Convert.ToString(row.ItemArray[0]) == string.Empty) { break; }
                            foreach (var item in row.ItemArray)
                            {

                                if (ColumnIndexes.Contains(j))
                                {
                                    if (sListOfRecords[0][j - 1].TrimStart().TrimEnd() == "HSNCode")
                                    {
                                        HSNCode = Convert.ToInt32(item.ToString().TrimStart().TrimEnd());
                                    }
                                    if (sListOfRecords[0][j - 1].TrimStart().TrimEnd() == "GSTPercentage")
                                    {
                                        GSTPercentage = Convert.ToInt32(item.ToString().TrimStart().TrimEnd());
                                    }
                                    excelRowdata.Add(item.ToString());
                                    string itemname = item.ToString();
                                    itemname = itemname.Replace("'", "");
                                    itemname = itemname.Replace(@"""", @"\""");
                                    str += '"' + sListOfRecords[0][j - 1].TrimStart().TrimEnd() + '"' + ":" + '"' + itemname.ToString().TrimStart().TrimEnd() + '"' + ",";
                                    product.Add(sListOfRecords[0][j - 1].TrimStart().TrimEnd(), itemname.ToString().TrimStart().TrimEnd());
                                }
                                j++;
                            }
                            product.Add("EditSheet_Number", randomNumber.ToString());
                            rows.Add(product);
                            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@HSN_Code_Id", HSNCode }, { "@Gst_Percentage", GSTPercentage } };
                            var response = _database.QueryValue(Upload_HSNCode, parameters);
                            if (excelRowdata.Where(m => !string.IsNullOrWhiteSpace(m)).Count() > 0)
                            {
                                sListOfRecords.Add(excelRowdata);
                            }
                        }
                        i++;
                    }
                    var res = serializer.Serialize(rows);

                }
                return Ok("Upload Successfully");
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

        //======================================To Get Verification Otp====================================================
        [HttpPost]
        [Route("GetVerificationOTP")]
        public IHttpActionResult GetVerificationOTP()
        {
            try
            {
                string Purpose = "Order Cancel By Admin";
                int userID = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@userid", userID } };
                var PhoneNumber = _database.GetStrValue(VerificationOTP, parameters);
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

        //============================>>>>>>Get Unit Tree details<<<<<<<<======================== 
        [HttpPost]
        [Route("Get_Units_Tree")]
        public IHttpActionResult Get_Units_Tree()
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { };
                var res = _database.ProductListWithCount(Units_Tree, parameters);
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


        //=================================>>>>>To Get Location based on Pincode <<<<<<<=============================================
        [HttpGet]
        [Route("Get_Locations_Based_On_Pincode")]
        public IHttpActionResult Get_Locations_Based_On_Pincode(int pincode)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Pincode", pincode } };
                var locations = _database.ProductListWithCount(Get_Locations_By_Pincode, parameters);
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

        //=======================>>>>>>> To Get Location based on Pincode <<<<<<<<<<<<===========================
        [HttpGet]
        [Route("Get_Locations_ByVillageID")]
        public IHttpActionResult Get_Locations_ByVillageID(int VillageID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Child_Location_Id", VillageID } };
                var locations = _database.QueryFunction(Get_Areas, parameters);
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

        //=======================>>>>>>> Get Master Data Values List By MasterName <<<<<<<<<<<<===========================
        [HttpGet]
        [Route("GetMasterDataValuesListByMasterNameinDropDown")]
        public IHttpActionResult GetMasterDataValuesListByMasterNameinDropDown(string MasterName)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@mastername", MasterName } };
                var res = _database.Query(Get_Masters_List, parameters);
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


        //======================To update default Message====================================================
        [HttpPost]
        [Route("updatedefaultMessage")]
        public IHttpActionResult updatedefaultMessage(VMModelsForMaster gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@message_id", gt.ID }, { "@messages", gt.Message } };
                var updated_data = _database.Query(Update_Default_Messages, parameters);
                return Ok(updated_data);
            }
            catch (Exception ex)
            {
                log.Error("updatedefaultMessage---Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        // =================================To Add Help Manual=========================================
        [HttpPost]
        [Route("Save_Help_Manual")]
        public IHttpActionResult Save_Help_Manual(VMModelsForHelpManual gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Rolesformanual", gt.Role },{ "@DocName", gt.DocName }
                                                                                           ,{ "@Description",gt.Description},{ "@Format", "pdf" }
                                                                                           ,{ "@VedioUrl", gt.VedioUrl },{ "@FolderName", gt.FolderName }};
                var Manual_data = _database.QueryValue(Add_Help_Manual, parameters);
                return Ok(Manual_data);
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

        //-----------------------------------------Method For Upload Cash Image For store
        [HttpPost]
        [Route("Upload_Help_Manual_Pdf")]
        public IHttpActionResult Upload_Help_Manual_Pdf(int ClaimID)
        {
            try
            {
                HttpPostedFile file = HttpContext.Current.Request.Files[0];
                string physicalPath = HttpContext.Current.Server.MapPath("~/" + "PDF/HelpManual/HelpManual/");
                if (!System.IO.File.Exists(physicalPath))
                {
                    Directory.CreateDirectory(physicalPath);
                }
                string ExteName = Path.GetExtension(file.FileName);
                string pdfPath = "";
                pdfPath = HttpContext.Current.Server.MapPath("~/" + "PDF/HelpManual/HelpManual/" + ClaimID + ".pdf");
                file.SaveAs(pdfPath);
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

        // =================================To View All Help Manuals=========================================
        [HttpPost]
        [Route("Get_All_Created_Help_Manual_Details")]
        public IHttpActionResult Get_All_Created_Help_Manual_Details(VMModelsForHelpManual gt)
        {
            try
            {
                var RoleID = "";
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                string commandTextOne = "SELECT \"RoleId\" FROM \"AspNetUserRoles\" WHERE \"UserId\"=" + "'" + User_Id + "'";
                Dictionary<string, object> parametersOne = new Dictionary<string, object>() { };
                var res = _database.QueryValueCOUNT(commandTextOne, parametersOne);
                if (Convert.ToInt32(res) == 1)
                {
                    RoleID = "";
                }
                else
                {
                    RoleID = res.ToString();
                }
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Doc_Name", gt.DocName },{ "@Role_Id", RoleID }
                                                                                           ,{ "@Folder_Name",gt.FolderName},{ "@Page_Index",gt.PageIndex }
                                                                                           ,{ "@Page_Size",gt.PageSize  }};
                var ViewManual = _database.ProductListWithCount(View_Manual, parameters);
                return Ok(ViewManual);
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

        // =================================View All Manual Roles By Role ID=========================================
        [HttpGet]
        [Route("View_All_Manual_Roles_By_Role_ID")]
        public IHttpActionResult View_All_Manual_Roles_By_Role_ID(int RoleId)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ManualId", RoleId } };
                var res = _database.ProductListWithCount(Get_Role_Names, parameters);
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

        //==========>>>>>>> To Get Botton Link Data in FooterMenu <<<<<<<===============
        [HttpGet]
        [Route("getBottomLinksData")]
        public IHttpActionResult getBottomLinksData(int ID)
        {
            try
            {
                string commandText = "SELECT * FROM iD_BottomLinks WHERE MasterDataID =" + ID;
                Dictionary<string, object> parameters = new Dictionary<string, object>() { };
                var res = _database.SelectQuery(commandText, parameters);
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

        //==========>>>>>>> To Update BottomLinks Description For FooterMenu <<<<<<<===============
        [HttpPost]
        [Route("SaveBottomLinksData")]
        public IHttpActionResult SaveBottomLinksData(VMBottomLinks model)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@primarykey", model.PrimaryKey }, { "@linkid", model.ID }, { "@linktype", model.Name }, { "@description", model.Description } };
                var res = _database.Query(Save_BottomLinks_Data, parameters);
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

        //==========>>>>>>> Get ASP Net Roles <<<<<<<===============
        [HttpGet]
        [Route("Get_ASP_Net_Roles")]
        public IHttpActionResult Get_ASP_Net_Roles()
        {
            try
            {
                var ASP_Net_Roles = "SELECT * FROM \"AspNetRoles\"";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { };
                var res = _database.SelectQuery(ASP_Net_Roles, parameters);
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

        //==========>>>>>>> Add New Empolyee registation <<<<<<<===============
        [HttpPost]
        [Route("AddNewEmpolyeeregistation")]
        public IHttpActionResult AddNewEmpolyeeregistation(iHubUnits ba)
        {
            try
            {
                var SecurityStamp = Guid.NewGuid().ToString();
                UserManager.PasswordHasher = new PasswordHasher();
                var password = UserManager.PasswordHasher.HashPassword(ba.Password);
                string Add_New_Empolyeeregistation = "[iAdmin_Create_New_Admin_Employee]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                                                                                       { "@EmployeeName", ba.Unit_Name },
                                                                                       { "@Employee_MobileNumber", ba.Phone_Number},
                                                                                       { "@Employee_EmailId", ba.Email },
                                                                                       { "@Employee_RoleId", ba.RoleId },
                                                                                       { "@Employee_Qual_Name", ba.Qualification },
                                                                                       { "@Employee_AddressProff", ba.Address_Proof_ID_employee },
                                                                                       { "@Address_IDentification_Number", ba.Address_Proof_Id_Number },
                                                                                       { "@AddressLine_One", ba.AddressLine_One },
                                                                                       { "@AddressLine_Two", ba.AddressLine_Two},
                                                                                       { "@VillageLocation_ID", ba.VillageLocation_ID},
                                                                                       { "@UnitID", ba.Sub_Unit_ID},
                                                                                       { "@Suggested_UserName", ba.Suggested_UserName},
                                                                                       { "@PasswordHash",password},
                                                                                       { "@SecurityStamp", SecurityStamp}};
                var res = _database.QueryValue(Add_New_Empolyeeregistation, parameters);
                if (Convert.ToInt32(res) > 0)
                {
                    return Ok("Login Successfully Created");
                }
                else if (Convert.ToInt32(res) == 0)
                {
                    return Ok("User Name Alreay Exists");
                }
                else
                {
                    return Ok(res);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        //=============================== Upload Mandals =================================
        [HttpPost]
        [Route("UploadMandals")]
        public IHttpActionResult UploadMandals()
        {
            try
            {
                int clusterID = Convert.ToInt32(HttpContext.Current.Request.Params["model"]);
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
                            string Mandal_Name = "";
                            string District_Name = "";
                            string State_Name = "";
                            int j = 1;
                            product = new Dictionary<string, string>();
                            if (Convert.ToString(row.ItemArray[0]) == string.Empty) { break; }
                            foreach (var item in row.ItemArray)
                            {
                                if (ColumnIndexes.Contains(j))
                                {
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
                                }
                                j++;
                            }
                            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@District_Name", District_Name }, { "@Mandal_Name", Mandal_Name }, { "@Pin_code", 0 }, { "@Cluster_ID", clusterID } };
                            var response = _database.QueryValue(Add_mandals_Data, parameters);
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
                return Ok(-1);
            }
        }


        #endregion "END of -- MasterDataManagement"

        #region "BEGIN of -- HomePageManagement"



        // =================================Upload Banner Image====================================================
        [HttpPost]
        [Route("Upload_Banner_Image")]
        public IHttpActionResult Upload_Banner_Image(int ID, int iSiteID)
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
                string physicalPath = HttpContext.Current.Server.MapPath("~/" + "Images/" + iSiteID + "_BannerImages/");
                if (!System.IO.File.Exists(physicalPath))
                {
                    Directory.CreateDirectory(physicalPath);
                }
                string imgPath = HttpContext.Current.Server.MapPath("~/" + "images/" + iSiteID + "_BannerImages/" + ID + "." + imagetype);
                FileInfo file = new FileInfo(imgPath);
                if (file.Exists.Equals(true))
                {
                    System.IO.File.Delete(imgPath);
                }
                Photo.SaveAs(imgPath);
                CustomResponse cs = new CustomResponse();
                cs.Response = "Image uploaded successfully.";
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

        // =================================Upload Brand Image====================================================
        [HttpPost]
        [Route("UploadBrandImage")]
        public IHttpActionResult UploadBrandImage(int BrandID)
        {
            try
            {
                HttpPostedFile Photo = HttpContext.Current.Request.Files[0];
                string str = string.Empty;
                string type = string.Empty;
                string physicalPath = HttpContext.Current.Server.MapPath("~/" + "Images/BrandImages/");
                if (!System.IO.File.Exists(physicalPath))
                {
                    Directory.CreateDirectory(physicalPath);
                }
                string imgPath = HttpContext.Current.Server.MapPath("~/" + "images/BrandImages/" + BrandID + ".jpg");
                Photo.SaveAs(imgPath);
                CustomResponse cs = new CustomResponse();
                cs.Response = "Image uploaded successfully.";
                cs.ResponseID = Convert.ToInt32(BrandID);
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

        // =================================Get Home Products By Flag====================================================
        [HttpPost]
        [Route("Get_Home_Page_Products_Controller")]
        public IHttpActionResult Get_Home_Page_Products_Controller(string flag)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@flag", flag } };
                var res = _database.ProductListWithCount(Get_Home_Products_By_Flag, parameters);
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
        [HttpPost]
        [Route("SaveMenuStrip")]
        public IHttpActionResult SaveMenuStrip(iHub_Menu_Strip menu)
        {
            if (menu.RemoveIDs != "" && menu.RemoveIDs != null)
            {
                Dictionary<string, object> Param = new Dictionary<string, object>() { { "@iSiteID", menu.DomainID } };
                var oList = _database.ProductListWithCount("Get_Domains_List_By_DomainID", Param);
                if (oList != null)
                {
                    var oCatLeaf = oList.Resultset[0].ToList().Where(m => m.Key.ToLower() == "leafnode_ids").Select(m => m.Value).ToList().FirstOrDefault();
                    var oWhereClause = oList.Resultset[0].ToList().Where(m => m.Key.ToLower() == "whereclause").Select(m => m.Value).ToList().FirstOrDefault();
                    if (oCatLeaf != null)
                    {
                        var oCatD = oCatLeaf.Split(',').ToList();
                        if (menu.RemoveIDs != null && menu.RemoveIDs != "")
                        {
                            var oRemove = menu.RemoveIDs.Split(',').ToList();
                            foreach (var item in oRemove)
                            {
                                if (oCatD.Contains(item))
                                {
                                    oCatD.Remove(item.ToString());
                                }
                            }
                        }
                        int WhereIndex = oWhereClause.ToLower().IndexOf(" parentid in (");
                        string sSubQuery = oWhereClause.Substring(0, WhereIndex);
                        string ComboLeaf = string.Join(",", oCatD);
                        string ComboWhere = sSubQuery + " ParentID in(" + ComboLeaf + "))";
                        Dictionary<string, object> oPar = new Dictionary<string, object>() { { "@LeafNodeIDs", ComboLeaf },
                                                                     { "@WhereClause",ComboWhere},
                                                                     { "@iSiteID",menu.DomainID}};
                        var oUpdate = _database.QueryValue("Update_LeafWhere_Domains_List_By_DomainID", oPar);
                    }
                }
            }

            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@MenuName", menu.Menu_Name },{"@menutype",menu.Menu_Type },{ "CategoryIDs",menu.Category_IDs },
                                                                                { "@OnHover",menu.On_Hover},{"@UIClass",menu.UIClass },{ "@DisplayOrder",menu.DisplayOrder},
                                                                                { "@ParentID",menu.Parent_ID},{ "@Type",menu.Type},{ "@Menuid",menu.Menu_ID},{ "@Status",menu.Status},
                                                                                { "@CatIDs",menu.CatIDs},{ "@SubCatIDs",menu.SubCatIDs},{ "@LeafIDs",menu.LeafIDs},{ "@DomainID",menu.DomainID}};
            var res = _database.QueryValue(Save_Menu_Strip_Item, parameters);
            string result = res.ToString();
            var cat = dbContext.iHub_Menu_Strip.Where(x => x.Menu_ID.ToString() == result).FirstOrDefault();
            cat.CategoryNames = menu.CategoryNames;
            string CacheKey = "MenuStrip_" + menu.DomainID.ToString();
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

        [HttpGet]
        [Route("GetMenuStrip")]
        public IHttpActionResult GetMenuStrip(int DomainID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@DomainId", DomainID } };
            var res = _database.ProductListWithCount(Get_Menu_Strip_Item, parameters);
            return Ok(res);
        }

        [HttpGet]
        [Route("GetTopLevelMenuStripItem")]
        public IHttpActionResult GetTopLevelMenuStripItem(int DomainID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@DomainId", DomainID } };
            var res = _database.ProductListWithCount(Get_TopLevelMenu_Strip_Item, parameters);
            return Ok(res);
        }

        #endregion "END of -- HomePageManagement"

        //=======>>>>> To Get User Types Store Assistant or Agent
        [HttpGet]
        [Route("GetUsertypes")]
        public IHttpActionResult GetUsertypes()
        {
            try
            {
                var Data = dbContext.iHub_Login_User_Types.ToList();
                return Ok(Data);
            }
            catch (Exception ex)
            {
                log.Error("Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        //==========>>>>>>> Reset Password For Units And Agentes <<<<<<<===============
        [HttpPost]
        [Route("ResetPasswordUnits")]
        public IHttpActionResult ResetPasswordUnits(iHubUnits y)
        {
            try
            {
                var SecurityStamp = Guid.NewGuid().ToString();
                UserManager.PasswordHasher = new PasswordHasher();
                var password = UserManager.PasswordHasher.HashPassword(y.NewPassword);
                string reset_passowrd = "[iAdmin_Reset_Password_For_Units]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@UserId", y.user_ID }, { "@PasswordHash", password } };
                var res = _database.Query(reset_passowrd, parameters);
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
        //==================================================== Get Unit Table Tree details ResetPassword=======================================
        [HttpPost]
        [Route("Get_Units_Table_Tree_For_ResetPassword")]
        public IHttpActionResult Get_Units_Table_Tree_For_ResetPassword(string Username)
        {
            try
            {
                string Units_Table_Tree_Reset = "[iAdmin_Get_Units_List_By_UserID_Reset_Password]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@UserName", Username } };
                var Get_Units_Tree = _database.ProductListWithCount(Units_Table_Tree_Reset, parameters);
                return Ok(Get_Units_Tree);
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
        [Route("GetSubMenu")]
        public IHttpActionResult GetSubMenu(int MenuID, int ParentID)
        {
            try
            {
                var res = dbContext.iHub_Menu_Strip.Where(x => x.Menu_ID == MenuID).FirstOrDefault();
                return Ok(res);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.ToString();
                return Ok(cs);
            }
        }

        //Code for Footer Menu Strip data
        [HttpGet]
        [Route("GetFootMenuStrip")]
        public IHttpActionResult GetFootMenuStrip(int iSiteID)
        {
            string sKey = "FooterMenu_" + iSiteID.ToString();
            try
            {
                if (HttpRuntime.Cache[sKey] == null)
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@iSiteID", iSiteID }, { "@iShow", 1 } };
                    var res = _database.ProductListWithCount(Get_Foot_Menu_Strip_Item, parameters);
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
                cs.Response = ex.ToString();
                return Ok(cs);
            }
        }
        //Code for Footer Sub Menu Strip data
        [HttpGet]
        [Route("GetFootSubMenu")]
        public IHttpActionResult GetFootSubMenu(int ID, string sParentID)
        {
            try
            {
                var res = dbContext.iD_BottomLinks.Where(x => x.ID == ID).FirstOrDefault();
                return Ok(res);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.ToString();
                return Ok(cs);
            }
        }

        [HttpPost]
        [Route("SaveFootMenuStrip")]
        public IHttpActionResult SaveFootMenuStrip(iD_BottomLinks menu)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ID", menu.ID },
                { "@LinkType", menu.sName },
                { "@Description",menu.Description },
                { "@MasterDataID",menu.MasterDataID },
                { "@iSiteID",menu.iSiteID},
                { "@sName",menu.sName },
                { "@iDisplayOrder",menu.iDisplayOrder},
                { "@sType",menu.sType},
                { "@iStatus",menu.iStatus},
                { "@sParentID",menu.sParentID}};
            var res = _database.QueryValue(Save_Foot_Menu_Strip_Item, parameters);

            string CacheKey = "HomePageFooterMenu_" + menu.iSiteID.ToString();
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

        [HttpPost]
        [Route("UploadStripImage")]
        public IHttpActionResult UploadStripImage(int MenuID, int iSource)
        {
            try
            {
                HttpPostedFile Photo = HttpContext.Current.Request.Files[0];
                string str = string.Empty;
                string type = string.Empty;
                string physicalPath = HttpContext.Current.Server.MapPath("~/" + "Images/StripImages/");
                if (!System.IO.File.Exists(physicalPath))
                {
                    Directory.CreateDirectory(physicalPath);
                }
                string imgPath = string.Empty;
                if (iSource == 1)
                {
                    imgPath = HttpContext.Current.Server.MapPath("~/" + "images/StripImages/" + MenuID + ".jpg");
                }
                else
                {
                    imgPath = HttpContext.Current.Server.MapPath("~/" + "images/StripImages/" + MenuID + "_Background" + ".jpg");
                }
                Photo.SaveAs(imgPath);
                CustomResponse cs = new CustomResponse();
                cs.Response = "Image uploaded successfully.";
                cs.ResponseID = Convert.ToInt32(MenuID);
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

        [HttpGet]
        [Route("GetDomainsListByDomainID")]
        public IHttpActionResult GetDomainsListByDomainID(int iSiteID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@iSiteID", iSiteID } };
            var res = _database.ProductListWithCount("Get_Domains_List_By_DomainID", parameters);
            return Ok(res);
        }

        //============================Get Results With Paging=======================================
        [HttpPost]
        [Route("GetResultswithPagingErrorlog")]
        public IHttpActionResult GetResultswithPagingErrorlog(VMModelsForMaster gt)
        {
            try
            {
                string Get_Results_With_Paging_Count = "[Get_Error_Log_Data]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@pageindex", gt.PageIndex }, { "@pagesize", gt.PageSize } };
                var getdetails = _database.ProductListWithCount(Get_Results_With_Paging_Count, parameters);
                return Ok(getdetails);
            }
            catch (Exception ex)
            {
                log.Error("GetResultswithPagingErrorlog--Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }
        //=======================>>>>>>> To Get Location based on Pincode <<<<<<<<<<<<===========================
        [HttpGet]
        [Route("Get_StoreKeepers_By_UnitID")]
        public IHttpActionResult Get_StoreKeepers_By_UnitID(int storeID)
        {
            try
            {
                List<iHub_StoreAssistant_Agents> oResu = new List<iHub_StoreAssistant_Agents>();
                var cmd = "select Store_User_Type,Store_Keeper_User_Id,Store_Keeper_Name from iH_Stores_Keeper where Unit_Id=" + storeID;
                Dictionary<string, object> parameter = new Dictionary<string, object> { };
                var res = _database.SelectQuery(cmd, parameter);
                if (res.Count() > 0)
                {
                    foreach (var it in res)
                    {
                        iHub_StoreAssistant_Agents oAgent = new iHub_StoreAssistant_Agents();
                        oAgent.Store_User_Type = Convert.ToInt32(it.Where(m => m.Key == "Store_User_Type").Select(m => m.Value).FirstOrDefault());
                        oAgent.Store_Keeper_User_Id = Convert.ToInt16(it.Where(m => m.Key == "Store_Keeper_User_Id").Select(m => m.Value).FirstOrDefault());
                        string sName = it.Where(m => m.Key == "Store_Keeper_Name").Select(m => m.Value).FirstOrDefault();
                        if (oAgent.Store_User_Type == 1)
                        {
                            oAgent.Store_Keeper_UserName = sName + "-(S.A)";
                        }
                        else
                        {
                            oAgent.Store_Keeper_UserName = sName + "-(A)";
                        }
                        oResu.Add(oAgent);
                    }
                }
                return Ok(oResu);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }
        //===================== GET Cash Reports List =================
        [HttpPost]
        [Route("View_StoreKeepers_Cash_Report")]
        public IHttpActionResult View_StoreKeepers_Cash_Report(VMModelsForMaster vmodel)
        {
            try
            {
                string View_Cash_Reports = "[iS_Agents_Daily_Sale_Cash_Report]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@UnitId", vmodel.Unit_ID },
                                                                                            { "@UserId", vmodel.UserId },
                                                                                            { "@UserType", vmodel.UserType },
                                                                                          { "@Year", vmodel.year },
                                                                                          { "@Month", vmodel.month }  };
                var result = _database.ProductListWithCount(View_Cash_Reports, parameters);
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
        [Route("View_StoreKeepers_Cash_Report_Details")]
        public IHttpActionResult View_StoreKeepers_Cash_Report_Details(VMModelsForMaster vmodel)
        {
            try
            {
                string View_Cash_Reports_Details = "[iAdmin_Agents_Daily_Sale_Cash_Report_Details]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@UnitId", vmodel.Unit_ID },
                                                                                            { "@UserId", vmodel.UserId },
                                                                                            { "@UserType", vmodel.UserType },
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
        //========================= Chnage Status Location Details=========================================
        [HttpPost]
        [Route("ChangeLocationStatus")]
        public IHttpActionResult ChangeLocationStatus(VMModelsForMaster GF)
        {
            try
            {

                var uptest = "UPDATE iH_Master_Franchise_Locations SET Status=" + GF.Status + " WHERE ID  IN (" + "" + GF.Type + ")";
                Dictionary<string, object> parameters = new Dictionary<string, object> { };
                var res = _database.SelectQuery(uptest, parameters);
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
        //========================= Update To City=========================================
        [HttpPost]
        [Route("ChangeLocationType")]
        public IHttpActionResult ChangeLocationType(VMModelsForMaster GF)
        {
            try
            {

                var uptest = "UPDATE iH_Master_Franchise_Locations SET LocationType =" + "'" + GF.LocationType + "'" + " WHERE ID  IN (" + "" + GF.Type + ")";
                Dictionary<string, object> parameters = new Dictionary<string, object> { };
                var res = _database.SelectQuery(uptest, parameters);
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

        //================================= update Franchise Location details======================================================
        [HttpPost]
        [Route("UpdateFranchiseLocationDetails")]
        public IHttpActionResult UpdateFranchiseLocationDetails(VMModelsForMaster GF)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Location_Id",GF.ID }, { "@Parent_Id", GF.StoreID}
                    , { "@Loc_Name",GF.Name }, { "@Hierarchy_level",GF.PageIndex }, { "@PinCode",GF.Message }, { "@User_ID",1 } };
                var res = _database.QueryValue(Update_Franchise_Location_Details, parameters);
                return Ok(res);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("UpdateFranchiseLocationDetails---Error---------", ex);
                return Ok(cs);
            }
        }

        //================= Create Franchise Location details=========================================
        [HttpPost]
        [Route("AddFranchiseLocationDetails")]
        public IHttpActionResult AddFranchiseLocationDetails(VMModelsForMaster GF)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Location_Id",GF.ID }, { "@Parent_Id", GF.StoreID}
                    , { "@Loc_Name",GF.Name }, { "@Hierarchy_level",GF.PageIndex }, { "@PinCode",GF.Message }, { "@User_ID",1 } };
                var res = _database.QueryValue(Add_Franchise_Location_Details, parameters);
                return Ok(res);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("AddFranchiseLocationDetails---Error---------", ex);
                return Ok(cs);
            }
        }


        //==================================================== Get Franchise Master Locations details========================================
        [HttpPost]
        [Route("Get_Franchise_Master_Locations")]
        public IHttpActionResult Get_Franchise_Master_Location()
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { };
                var Get_Franchise_Master_Location = _database.ProductListWithCount(Get_Franchise_Master_Locations, parameters);
                return Ok(Get_Franchise_Master_Location);
            }
            catch (Exception ex)
            {
                log.Error("Get_Franchise_Master_Locations---Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        //========================= Get Franchise Location Details=========================================
        [HttpPost]
        [Route("GetFranchiseLocationDetails")]
        public IHttpActionResult GetFranchiseLocationDetails(VMModelsForMaster GF)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Parent_ID",GF.ParentID }, { "@Pageindex", GF.PageIndex }
                   , { "@Pagesize", GF.PageSize } , { "@searchtext", GF.Searchtext } };
                var Get_Franchise_Location_Details = _database.ProductListWithCount(Get_Franchise_Master_Location_By_ParentID, parameters);
                return Ok(Get_Franchise_Location_Details);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }

        //=========================================Added on 10May21 to Add/Edit  Master Domains Data=========================================//

        [HttpPost]
        [Route("Add_Edit_MasterDomainsData")]
        public IHttpActionResult MapDomains_To_Products(DomainsData Domainmodel)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@DomainID", Domainmodel.DomainID }, { "@Domain_name", Domainmodel.DomainName },
                    { "@Status", Domainmodel.Status } ,{ "@Description",Domainmodel.Description},{ "@ProductSaleQtyLimit",Domainmodel.ProdSaleQtyLimit},{ "@ShowPriceRequestProducts",Domainmodel.showPriceRequestProd} };
                var res = _database.QueryValue(Save_Update_DomainMasterData, parameters);
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

        //==================================================== Get All iHub Domains List========================================
        [HttpGet]
        [Route("Get_All_iHub_DomainsData")]
        public IHttpActionResult Get_All_iHub_DomainsData()
        {
            try
            {
                var cmd = "Select * from iHub_Domains Order By Site_ID";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { };
                var res = _database.SelectQuery(cmd, parameters);
                return Ok(res);
            }
            catch (Exception ex)
            {
                log.Error("Get_All_iHub_domainsData---Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

    }
}
