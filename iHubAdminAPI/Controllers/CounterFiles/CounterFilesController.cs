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
using iHubAdminAPI.Models.AppModels;
using System.Web.Caching;

namespace iHubAdminAPI.Controllers
{
    [RoutePrefix("api/CounterFiles")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CounterFilesController : ApiController
    {
        string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private static readonly ILog log = LogManager.GetLogger(typeof(CounterFilesController).FullName);
        public SQLDatabase _database;
        iHubDBContext dbContext = new iHubDBContext();
        private ApplicationUserManager _userManager;
        public CounterFilesController()
        {
            _database = new SQLDatabase();
        }
        private class CustomResponse
        {
            internal string Response;
            internal int ResponseID;
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


        // =================================Command Rext For Api Calls====================================================
        string Validate_OTP = "[iAdmin_Validate_OTP]";
        string Counter_file_Status = "[iAdmin_Update_Cash_Counter_File_Status]";
        string Get_Counterfiles = "[iAdmin_Get_Cash_And_Cheque_CounterFiles]";
        string Order_Details_By_Order_Id = "[iAdmin_Get_Order_Details_By_Order_Number]";
        string Get_iHub_Employee_Details = "[iAdmin_Get_iHub_Employee_Details]";
        string Update_Menu_In_Store = "[iAdmin_Add_Upd_Directmenu]";
        string Make_DuePayment = "[iAdmin_Pay_Total_Due_Amount_By_OrderNumber]";
        string update_home_main_banner = "[iAdmin_update_home_main_banner]";
        string Delete_Home_Main_Banner = "[iAdmin_Delete_Home_Main_Banner]";
        string get_brands_category = "[iAdmin_Get_Brands]";
        string Add_Update_HomeProducts = "[iAdmin_Add_Upd_HomeProducts]";
        string Get_All_Store_Expenses = "[iAdmin_Get_All_Store_Expenses]";
        string Counter_store_Status = "[iAdmin_Change_Store_Expenses_Status]";
        string Date_For_Expenes = "[iAdmin_Get_Manage_Expenses_Masters]";
        string Change_Employee_status_Reset_Password = "[iAdmin_Change_Employee_status_Reset_Password]";
        string Wallet_Details_By_Mobile_Number = "[iAdmin_Get_Wallet_Details_By_Mobile_Number]";
        string Create_New_Store_Keeper = "[iAdmin_Create_New_Store_Keeper_OR_Agent]";
        string Store_Keeper_List = "[iAdmin_Get_Store_Keepers_List_By_Unit_ID]";
        string Update_Domains_For_Employee = "[iAdmin_Update_Domains_For_Employee]";
        string iHub_Role_Subscription = "[iAdmin_Create_Role_Subscription]";
        string Get_Subscription_By_RoleID = "[iAdmin_Get_Subscription_By_RoleID]";


        #endregion "BEGIN of -- SP Names"


        #region "BEGIN of -- SELECT TABLES"

        string HomePage = "SELECT * FROM \"iD_HomePage\"";

        #endregion "END of -- SELECT TABLES"

        #region "BEGIN of -- CouterFilesManagement"

        //=========================  change counter file status=========================================

        [HttpPost]
        [Route("ChangeCounterFileStatus")]
        public IHttpActionResult ChangeCounterFileStatus(int ID, int Status)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@counterfileid", ID }, { "@status", Status }, { "@AdminUserId", userID } };
                var res = _database.ProductListWithCount(Counter_file_Status, parameters);
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

        //=========================Get Cash Counter Files================================================
        [HttpPost]
        [Route("GetCashCounterFiles")]
        public IHttpActionResult GetCashCounterFiles(VMModelsForMaster vmodel)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {{ "@Unitname", vmodel.Name },
                                                                                          { "@FileType", vmodel.Type },
                                                                                          { "@OrderID", vmodel.OrderID } ,
                                                                                          { "@Status",vmodel.Status },
                                                                                          { "@Orderdatefrom", vmodel.orderdatefrom},
                                                                                          { "@Orderdateto", vmodel.orderdateto },
                                                                                          { "@PageIndex", vmodel.PageIndex },
                                                                                          { "@PageSize", vmodel.Pagesize }};
                var locations = _database.ProductListWithCount(Get_Counterfiles, parameters);
                return Ok(locations);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        //==========>>>>>>> Get Empolyee registation For Admin <<<<<<<===============
        [HttpPost]
        [Route("iH_Get_Empolyee_registation_For_Admin")]
        public IHttpActionResult iH_Get_Empolyee_registation_For_Admin(iHubUnits ba)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@RoleID",ba.RoleId},
                                                                                         { "@MobileNumber", ba.Phone_Number},
                                                                                         { "@EmployeeName",ba.ContactName },
                                                                                         { "@PageIndex", ba.Page_Index},
                                                                                         { "@PageSize", ba.Page_Size}
                                                                                        };
                var res = _database.ProductListWithCount(Get_iHub_Employee_Details, parameters);
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
        [Route("iH_Update_Domains_For_Employee")]
        public IHttpActionResult iH_Update_Domains_For_Employee(iHubUnits ba)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Domains", ba.Domains},
                                                                                         { "@AspNetUserID", ba.user_ID}
                                                                                        };
                var res = _database.Query(Update_Domains_For_Employee, parameters);
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
        [Route("iH_Role_Subscription")]
        public IHttpActionResult iH_Role_Subscription(iHubUnits ba)
        {
            try
            {

                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                                                                                         { "@RoleId", ba.RoleId},
                                                                                          { "@Domains", ba.Domains}
                                                                                        };
                if (ba.removeMasterIDs != "") { 
                    foreach (var item in ba.removeMasterIDs.Split(','))
                    {
                        var delete_item = dbContext.iHub_RoleSubscriptions.Where(z => z.Notification_TypeID.ToString() == item && z.RoleID == ba.RoleId).FirstOrDefault();
                        if (delete_item != null) { 
                        dbContext.iHub_RoleSubscriptions.Remove(delete_item);
                        dbContext.SaveChanges();
                        }
                    }
                }
                if (ba.Domains != "")
                {
                    var res = _database.Query(iHub_Role_Subscription, parameters);
                    return Ok(res);
                }
                else
                {
                    return Ok(0);
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

        [HttpGet]
        [Route("iH_Get_Role_Subscriptions")]
        public IHttpActionResult iH_Get_Role_Subscriptions(int RoleID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {{ "@RoleId", RoleID}};
                var res = _database.Query(Get_Subscription_By_RoleID, parameters);
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
        [Route("iH_User_Subscriptions")]
        public IHttpActionResult iH_User_Subscriptions(iHubUnits ba)
        {
            try
            {
                
                if (ba.removeMasterIDs != "")
                {
                    foreach (var item in ba.removeMasterIDs.Split(','))
                    {
                        var delete_item = dbContext.iHub_User_Subscriptions.Where(z => z.Notification_TypeID.ToString() == item && z.UserID == ba.user_ID).FirstOrDefault();
                        if (delete_item != null)
                        {
                            dbContext.iHub_User_Subscriptions.Remove(delete_item);
                            dbContext.SaveChanges();
                        }
                    }
                }
                if (ba.Domains != "")
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Notification_IDs", ba.Domains }, { "@UserID", ba.user_ID }, { "@Role_Subscription_IDs", ba.Role_Subscription_IDs } };
                    var res = _database.Query("iAdmin_User_Subscription", parameters);
                    return Ok(res);
                }
                else
                {
                    return Ok(0);
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

        [HttpGet]
        [Route("iH_Get_Role_Subscriptions_RoleID")]
        public IHttpActionResult iH_Get_Role_Subscriptions_RoleID(int RoleID)
        {
            try
            {
                var sunscriptions = dbContext.iHub_RoleSubscriptions.Where(x => x.RoleID == RoleID).ToList();
                return Ok(sunscriptions);
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
        [Route("iH_Get_User_Subscriptions")]
        public IHttpActionResult iH_Get_User_Subscriptions(int UserID)
        {
            try
            {
                var sunscriptions = dbContext.iHub_User_Subscriptions.Where(x => x.UserID==UserID).ToList();
                return Ok(sunscriptions);
            }
            catch (Exception ex)
            {
                log.Error("Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }


        //=======================To Get Order Details By Order Number==================================================
        [HttpPost]
        [Route("Get_Order_Details_By_Order_Number")]
        public IHttpActionResult Get_Order_Details_By_Order_Number(int OrderNumber)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@OrderNumber", OrderNumber } };
                var res = _database.ProductListWithCount(Order_Details_By_Order_Id, parameters);
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

        //==========>>>>>>> update menu <<<<<<<===============  
        [HttpPost]
        [Route("updatemenu")]
        public IHttpActionResult updatemenu(List<iH_Categories> mlist)
        {
            try
            {
                var jsonSerialiser = new JavaScriptSerializer();
                var json = JsonConvert.SerializeObject(mlist);
                var jsons = jsonSerialiser.Serialize(json);
                var ID = 1;
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Form_Id", ID }, { "@JsonData", json } };
                var res = _database.QueryValue(Update_Menu_In_Store, parameters);
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

        //=============================================To Make Payment For Bokking Orders ==========================================================
        [HttpPost]
        [Route("Pay_Total_Due_Amount_By_OrderNumber")]
        public IHttpActionResult Pay_Total_Due_Amount_By_OrderNumber(VMModelsForMaster VModel)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>() { { "@otp", VModel.StoreOTP }, { "@notification_ID", VModel.Notification_ID } };
                var validate_result = _database.QueryValue(Validate_OTP, param);
                if (Convert.ToBoolean(validate_result))
                {
                    var User_Id = 1;
                    Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@OrderNumber", VModel.ProductID }, { "@PaymentMode",  VModel.Payment_Mode_Type }, { "@Amount",  VModel.Amount },
                    { "@Payment_JsonData", VModel.pamentjson }, { "@UserID", Convert.ToInt32(User_Id) },{ "@Use_Wallet", VModel.Use_Wallet_Amount } };
                    var Result = _database.Query(Make_DuePayment, parameters);
                    return Ok(Result);
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

        #endregion "END of -- CouterFilesManagement"

        #region "BEGIN of -- ManageExpanses"

        //====================== To Get All Store Expenses==========================
        [HttpPost]
        [Route("GetAllStoreExpenses")]
        public IHttpActionResult GetAllStoreExpenses(VMModelsForMaster vmodel)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@unit_name", vmodel.Name }, { "@status", vmodel.Status }, { "@expensedate",vmodel.expensedate }, { "@expensestype", vmodel.expensestype }
                   , { "@pagesize", vmodel.pagesize }, { "@pageindex", vmodel.pageindex } };
                var res = _database.ProductListWithCount(Get_All_Store_Expenses, parameters);
                return Ok(res);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        //=============================== To Change Store Expenses Status===========================
        [HttpPost]
        [Route("Change_Store_Expenses_Status")]
        public IHttpActionResult Change_Store_Expenses_Status(int ID, int Status)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@expenses_id", ID }, { "@status", Status } };
                var res = _database.ProductListWithCount(Counter_store_Status, parameters);
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

        //=============================== To Change Store Expenses Status===========================
        [HttpGet]
        [Route("Get_Master_Date_For_Expenes")]
        public IHttpActionResult Get_Master_Date_For_Expenes(string LinkType)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@LinkType", LinkType } };
                var res = _database.ProductListWithCount(Date_For_Expenes, parameters);
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

        //=============================== Change_Employee_Status=================================
        [HttpPost]
        [Route("Change_Employee_Status")]
        public IHttpActionResult Change_Employee_Status(int ID, int Status, int IS_Reset)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@AspNetUserID", ID }, { "@Status", Status }, { "@IS_Reset", IS_Reset } };
                var changed_status = _database.QueryValue(Change_Employee_status_Reset_Password, parameters);
                return Ok(changed_status);
            }
            catch (Exception ex)
            {
                log.Error("ChangeEmployee----Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        //=======================>>>>>>> Get Wallet Details By Mobile Number <<<<<<<<<<<<===========================
        [HttpPost]
        [Route("Get_Wallet_Details_By_Mobile_Number")]
        public IHttpActionResult Get_Wallet_Details_By_Mobile_Number(string MobileNumber)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@MobileNumber", MobileNumber } };
                var res = _database.ProductListWithCount(Wallet_Details_By_Mobile_Number, parameters);
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

        //=============================== Add New Store Keeper =================================
        [HttpPost]
        [Route("Add_New_Store_Keeper_Or_Agent")]
        public IHttpActionResult Add_New_Store_Keeper_Or_Agent(StoreAgent ba)
        {
            try
            {
                var SecurityStamp = Guid.NewGuid().ToString();
                UserManager.PasswordHasher = new PasswordHasher();
                var password = UserManager.PasswordHasher.HashPassword(ba.Password);

                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Unit_ID", ba.UnitId },
                                                                                      { "@StoreKeeper_Name", ba.FullName },
                                                                                      { "@Email", ba.Email },
                                                                                      { "@MobileNumber", ba.Phone_Number },
                                                                                      { "@Address_Line_1", ba.AddressLine_One },
                                                                                      {"@Address_Line_2", null},
                                                                                      { "@Village_Area_ID", ba.VillageLocation_ID},
                                                                                      { "@Suggested_User_Name", ba.Suggested_UserName},
                                                                                      {"@Gender" , ba.Gender},
                                                                                      {"@Date_Of_Birth" , ba.Date_Of_Birth},
                                                                                      {"@Qualification",null},
                                                                                      {"@Address_Proof_ID" , ba.Address_Proof_ID},
                                                                                       {"@Address_Proof_Id_Number" , ba.Address_Proof_Id_Number},
                                                                                       { "@User_Type",Convert.ToInt32(ba.UserType)},
                                                                                       { "@PasswordHash",password},
                                                                                        {"@SecurityStamp", SecurityStamp}};
                var res = _database.QueryValue(Create_New_Store_Keeper, parameters);

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

        //=============================== Get Store Keeper List =================================
        [HttpPost]
        [Route("Get_Store_Keeper_List")]
        public IHttpActionResult Get_Store_Keeper_List(int UnitID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@UnitID", UnitID } };
                var res = _database.ProductListWithCount(Store_Keeper_List, parameters);
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

        //==============================To Verify OTP====================================================================================
        [HttpPost]
        [Route("Validate_OTP_And_Notification_Id")]
        public IHttpActionResult Validate_OTP_And_Notification_Id(VMPagingResultsPost vmodel)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>() { { "@otp", vmodel.Otp }, { "@notification_ID", vmodel.Notification_ID } };
                var validate_result = _database.QueryValue(Validate_OTP, param);
                return Ok(validate_result);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }

        //=============================================Raffle Wallet Amount Credit ======================================================
        [HttpPost]
        [Route("RaffleWalletAmountCreditWithExcel")]
        public IHttpActionResult RaffleWalletAmountCreditWithExcel()
        {
            try
            {
                CustomResponse cs = new CustomResponse();
                List<string> resss = new List<string>();
                var res1 = "";
                var categorydata = HttpContext.Current.Request.Form[0];
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
                    bool ColomnrequiredRaffleAmount = false;
                    bool ColomnrequiredOrderNumber = false;

                    foreach (var column in data.Rows[0].ItemArray.Where(m => m is string))
                    {
                        if (column.ToString().Trim() == "MobileNumber")
                        {
                            ColomnrequiredMobileNumber = true;
                        }
                        if (column.ToString().Trim() == "OrderNumber")
                        {
                            ColomnrequiredOrderNumber = true;
                        }
                        if (column.ToString().Trim() == "RaffleAmount")
                        {
                            ColomnrequiredRaffleAmount = true;
                        }
                        string ColumnName = column.ToString();
                        ColumnName.Replace("'", " ");
                        Dictionary.Add(ColumnName);
                    }
                    if (ColomnrequiredMobileNumber == false)
                    {
                        return Ok("Invalid MobileNumber Coloumn Name");
                    }
                    if (ColomnrequiredRaffleAmount == false)
                    {
                        return Ok("Invalid RaffleAmount Coloumn Name");
                    }
                    if (ColomnrequiredOrderNumber == false)
                    {
                        return Ok("Invalid OrderNumber Coloumn Name");
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
                                    if (sListOfRecords[0][j - 1].ToString().Trim() == "RaffleAmount")
                                    {

                                        if (itemname == "")
                                        {
                                            cs.Response = "Invalid RaffleAmount Value";
                                            cs.ResponseID = Convert.ToInt32(16);
                                            resss.Clear();
                                            flag = true;
                                            break;
                                        }
                                    }
                                    if (sListOfRecords[0][j - 1].ToString().Trim() == "OrderNumber")
                                    {
                                        if (itemname == "")
                                        {
                                            cs.Response = "Invalid OrderNumber Value";
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
                            string commandText = "[iAdmin_Creadit_Amount_To_Raffle_Lucky_Winner]";
                            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@RaffleLuckyAmount", a.RaffleAmount }, { "@MobileNumber", a.MobileNumber }, { "@TransactionNumber", randomNumber.ToString() }, { "@OrderedNumber", a.OrderNumber } };
                            var response = _database.Query(commandText, parameters);
                        }
                        return Ok("Raffle Wallet Updated Successfully..");
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

        #endregion "END of -- ManageExpanses"

        #region "BEGIN of -- HomePageManagement"

        // =================================Get Banners====================================================
        [HttpPost]
        [Route("Get_Banners")]
        public IHttpActionResult Get_Banners()
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { };
                var result = _database.SelectQuery(HomePage, parameters);
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

        // =================================Update Home Main Banner====================================================
        [HttpPost]
        [Route("update_Home_Main_Banner")]
        public IHttpActionResult update_Home_Main_Banner(VMPagingResultsPost vmodel)
        {
            try
            {
                var categoryid = vmodel.CategoryID;
                if (categoryid == 0)
                {
                    Random generator = new Random();
                    String r = generator.Next(0, 999999).ToString();
                    Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@pkid", vmodel.ProductID }, { "@categoryname", vmodel.CategoryName }, { "@categoryid", r }, { "@status", vmodel.Status }, { "@imagecode", vmodel.Imagecode }, { "@fromdate", vmodel.FromDate }, { "@todate", vmodel.ToDate }, { "@DisplayOrder", vmodel.DisplayOrder }, { "SectionId", vmodel.SectionId }, { "@Price", vmodel.Price }, { "@Product_Mrp", vmodel.Product_Mrp }, { "@Site_ID", vmodel.Site_ID }, { "@bIsLeaf", vmodel.bIsLeaf }, { "@sProductType", vmodel.sProductType }, { "@sTitle", vmodel.sTitle }, { "@iTitleStatus", vmodel.iTitleStatus } };
                    var res = _database.QueryValue(update_home_main_banner, parameters);
                    var iID = Convert.ToInt32(res);
                    if (iID > 0)
                    {
                        Dictionary<string, object> param = new Dictionary<string, object>() { };
                        var sCmds = "Update iD_HomePage SET bIsSectionStatus='1' where HomePageID=" + res + " and Site_ID=" + vmodel.Site_ID;
                        var oHomePages = _database.SelectQuery(sCmds, param);
                    }
                    return Ok(res);
                }
                else
                {
                    Dictionary<string, object> param = new Dictionary<string, object>() { };
                    var sCmd = "SELECT * FROM iD_HomePage where SectionID=" + "'" + vmodel.SectionId + "' and Site_ID=" + vmodel.Site_ID;
                    var oHomePage = _database.SelectQuery(sCmd, param);
                    var sName = string.Empty;
                    var iType = string.Empty;
                    if (oHomePage.Count() != 0)
                    {
                        sName = oHomePage[0].Where(m => m.Key.ToLower() == "name").Select(m => m.Value).FirstOrDefault();
                        iType = oHomePage[0].Where(m => m.Key.ToLower() == "type_id").Select(m => m.Value).FirstOrDefault();
                    }
                    if ((oHomePage.Count() == 1) && (sName == "" && iType == "0"))
                    {
                        var iHomePageID = oHomePage[0].Where(m => m.Key.ToLower() == "homepageid").Select(m => m.Value).FirstOrDefault();
                        Dictionary<string, object> Par = new Dictionary<string, object>() { { "@pkid", iHomePageID } };
                        var oRes = _database.Query(Delete_Home_Main_Banner, Par);
                    }
                    Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@pkid", vmodel.ID }, { "@categoryname", vmodel.CategoryName }, { "@categoryid", categoryid }, { "@status", vmodel.Status }, { "@imagecode", vmodel.Imagecode }, { "@fromdate", vmodel.FromDate }, { "@todate", vmodel.ToDate }, { "@DisplayOrder", vmodel.DisplayOrder }, { "SectionId", vmodel.SectionId }, { "@Price", vmodel.Price }, { "@Product_Mrp", vmodel.Product_Mrp }, { "@Site_ID", vmodel.Site_ID }, { "@bIsLeaf", vmodel.bIsLeaf }, { "@sProductType", vmodel.sProductType }, { "@sTitle", vmodel.sTitle }, { "@iTitleStatus", vmodel.iTitleStatus } };
                    var res = _database.QueryValue(update_home_main_banner, parameters);
                    if (vmodel.SectionId != null)
                    {
                        if (oHomePage.Count() == 0)
                        {
                            var sCmds = "Update iD_HomePage SET bIsSectionStatus='1' where HomePageID=" + res + " and Site_ID=" + vmodel.Site_ID;
                            var oHomePages = _database.SelectQuery(sCmds, param);
                        }
                        else
                        {
                            if (vmodel.ID == 0)
                            {
                                var bIsThere = true;
                                var bIsTitle = true;
                                var sTitle = string.Empty;
                                foreach (var item in oHomePage)
                                {
                                    var bIS = item.Where(m => m.Key.ToLower() == "bissectionstatus").Select(m => m.Value).FirstOrDefault();
                                    if (bIS == "True")
                                    {
                                        bIsThere = false;
                                        var bTit = item.Where(m => m.Key.ToLower() == "stitle").Select(m => m.Value).FirstOrDefault();
                                        if (bTit != null || bTit != "")
                                        {
                                            bIsTitle = false;
                                            sTitle = bTit;
                                        }
                                        break;
                                    }
                                }
                                var sCmds = string.Empty;
                                if (bIsThere == true)
                                {
                                    sCmds = "Update iD_HomePage SET bIsSectionStatus='0' where HomePageID=" + res;
                                }
                                else
                                {
                                    sCmds = "Update iD_HomePage SET bIsSectionStatus='1' where HomePageID=" + res;
                                }
                                var oHomePages = _database.SelectQuery(sCmds, param);
                                if (bIsTitle == true)
                                {
                                    sCmds = "Update iD_HomePage SET sTitle='" + sTitle + "' where HomePageID=" + res;
                                }
                                else
                                {
                                    sCmds = "Update iD_HomePage SET sTitle='" + sTitle + "' where HomePageID=" + res;
                                }
                                var oHomePagesT = _database.SelectQuery(sCmds, param);
                            }
                        }
                    }

                    string CacheKey = "HomePageBanners_" + vmodel.Site_ID.ToString();
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

                    CacheKey = "HomePageProductList_" + vmodel.Site_ID.ToString();
                    var keyItems1 = dbContext.CacheTransactions.Where(x => x.CacheKey == CacheKey && !x.Status).ToList();
                    if (keyItems != null)
                    {
                        foreach (var keyItem in keyItems1)
                        {
                            keyItem.Status = true;
                            keyItem.Last_Updated_DateTime = DateTime.Now;
                        }
                    }

                    CacheTransactions ct1 = new CacheTransactions();
                    ct1.CacheKey = CacheKey;
                    ct1.Type = "HomePage";
                    ct1.Status = false;
                    ct1.Last_Updated_DateTime = DateTime.Now;
                    dbContext.CacheTransactions.Add(ct1);

                    var num = dbContext.SaveChanges();

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



        // =================================Delete Home Main Banner====================================================
        [HttpPost]
        [Route("delete_Home_Main_Banner")]
        public IHttpActionResult delete_Home_Main_Banner(int ID)
        {
            try
            {



                var imagetype = "jpg";
                Dictionary<string, object> param = new Dictionary<string, object>() { };
                var cmd = "SELECT * FROM iD_HomePage where HomePageID=" + ID;
                var oHomePage = _database.SelectQuery(cmd, param);

                string sSecID = oHomePage[0].Where(m => m.Key.ToLower() == "sectionid").Select(m => m.Value).FirstOrDefault();
                var iSiteID = oHomePage[0].Where(m => m.Key.ToLower() == "site_id").Select(m => m.Value).FirstOrDefault();
                Dictionary<string, object> param2 = new Dictionary<string, object>() { };
                var cmd2 = "SELECT * FROM iD_HomePage where SectionID=" + "'" + sSecID + "'";
                var oSecCount = _database.SelectQuery(cmd2, param2);


                if (oHomePage != null)
                {

                    string CacheKey = "HomePageBanners_" + iSiteID.ToString();
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

                    CacheKey = "HomePageProductList_" + iSiteID.ToString();
                    var keyItems1 = dbContext.CacheTransactions.Where(x => x.CacheKey == CacheKey && !x.Status).ToList();
                    if (keyItems1 != null && keyItems1.Count > 0)
                    {
                        foreach (var keyItem in keyItems1)
                        {
                            keyItem.Status = true;
                            keyItem.Last_Updated_DateTime = DateTime.Now;
                        }
                    }

                    CacheTransactions ct1 = new CacheTransactions();
                    ct1.CacheKey = CacheKey;
                    ct1.Type = "HomePage";
                    ct1.Status = false;
                    ct1.Last_Updated_DateTime = DateTime.Now;
                    dbContext.CacheTransactions.Add(ct1);

                    if(sSecID.ToLower() == "section19" || sSecID.ToLower() == "section20" || sSecID.ToLower() == "section22" || sSecID.ToLower() == "section23")
                    {
                        CacheKey = "HomePageSelFooter_" + iSiteID.ToString();
                        CacheTransactions fc = new CacheTransactions();
                        fc.CacheKey = CacheKey;
                        fc.Type = "HomePage";
                        fc.Status = false;
                        fc.Last_Updated_DateTime = DateTime.Now;
                        dbContext.CacheTransactions.Add(fc);
                    }
                    var num = dbContext.SaveChanges();
                }


                if (oHomePage != null && sSecID.ToLower() == "section18" || sSecID.ToLower() == "section14")
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@pkid", ID } };
                    string imgPath = HttpContext.Current.Server.MapPath("~/" + "images/" + iSiteID + "_BannerImages/" + ID + "." + imagetype);
                    FileInfo file = new FileInfo(imgPath);
                    if (file.Exists.Equals(true))
                    {
                        System.IO.File.Delete(imgPath);
                    }
                    var res = _database.Query(Delete_Home_Main_Banner, parameters);
                    return Ok(res);
                }
                else if ((oHomePage != null && oHomePage.Count() == 1) && (oSecCount.Count() == 1))
                {
                    var cmd1 = "Update iD_HomePage set Name='',Type_ID=0 where HomePageID=" + ID;
                    string imgPath = HttpContext.Current.Server.MapPath("~/" + "images/" + iSiteID + "_BannerImages/" + ID + "." + imagetype);
                    FileInfo file = new FileInfo(imgPath);
                    if (file.Exists.Equals(true))
                    {
                        System.IO.File.Delete(imgPath);
                    }
                    var oUpHome = _database.SelectQuery(cmd1, param);
                    return Ok(oHomePage);
                }
                else
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@pkid", ID } };
                    var res = _database.Query(Delete_Home_Main_Banner, parameters);
                    string imgPath = HttpContext.Current.Server.MapPath("~/" + "images/" + iSiteID + "_BannerImages/" + ID + "." + imagetype);
                    FileInfo file = new FileInfo(imgPath);
                    if (file.Exists.Equals(true))
                    {
                        System.IO.File.Delete(imgPath);
                    }
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

        // =================================get brands category====================================================
        [HttpPost]
        [Route("getbrandscategory")]
        public IHttpActionResult getbrandscategory(VMPagingResultsPost vm)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@pagesize", vm.Pagesize }, { "@PageIndex", vm.PageIndex }, { "@brandname", vm.BrandName } };
                var res = _database.ProductListWithCount(get_brands_category, parameters);
                return Ok(res);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error-------", ex);
                return Ok(cs);
            }
        }


        // =================================Add Update HomeProducts====================================================
        [HttpPost]
        [Route("Add_Upd_HomeProducts")]
        public IHttpActionResult Add_Upd_HomeProducts(VMPagingResultsPost vmodel)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@productids", vmodel.Productids }, { "@flag_type", vmodel.RelevanceType }, { "@status", vmodel.Status } };
                var res = _database.QueryValue(Add_Update_HomeProducts, parameters);
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
        // ============>>>>>>>>>>>> Add Update HomeProducts  <<<<<<<<<<========================
        [HttpPost]
        [Route("Get_Store_Users_And_Agents")]
        public IHttpActionResult Get_Store_Users_And_Agents(StoreAgent Gt)
        {
            try
            {
                string Users_And_Agents = "[iAdmin_Get_StoreKeeper_Or_Agents]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@DC_Unit_ID", Gt.DCUnitID },
                                                                                            { "@Cluster_Unit_ID", Gt.ClusterID },
                                                                                            { "@Unit_ID", Gt.UnitId },
                                                                                            { "@Year", Gt.Year },
                                                                                            { "@Month", Gt.Month },
                                                                                            { "@Page_Size", Gt.Page_Size },
                                                                                            { "@Page_Index", Gt.Page_Index }};
                var res = _database.ProductListWithCount(Users_And_Agents, parameters);
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
        //================>>>>>>>>>>>>> To Get Agent Details View <<<<<<<<<<<<<<<<=====================
        [HttpGet]
        [Route("Get_User_Details_By_StoreKeeperID")]
        public IHttpActionResult Get_Membership_Details_By_Membership_ID(int StoreKeeperID)
        {
            try
            {
                string Get_User_Or_Agent_Details = "[iAdmin_Get_User_Or_Agent_Details]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@StoreKeeperID", StoreKeeperID } };
                var res = _database.GetMultipleResultsListAll(Get_User_Or_Agent_Details, parameters);
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
        // ============>>>>>>>>>>>> Add Update HomeProducts  <<<<<<<<<<========================
        [HttpPost]
        [Route("Update_Store_Keeper_Or_Agent")]
        public IHttpActionResult Update_Store_Keeper_Or_Agent(StoreAgent Up)
        {
            try
            {
                string Update_Users_And_Agents = "[iAdmin_Update_Store_Assistant_Agent_Details]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Gender", Up.Gender },
                                                                                            { "@DOB", Up.DOB },
                                                                                            { "@Address", Up.Address },
                                                                                            { "@AddressId", Up.AddressId },
                                                                                            { "@MobileNumber", Up.MobileNumber },
                                                                                            { "@EmailId", Up.EmailId },
                                                                                            { "@Address_Proof_ID", Up.Address_Proof_ID },
                                                                                            { "@Address_Proof_Id_Number", Up.Address_Proof_Id_Number },
                                                                                            { "@Id", Up.Id }};
                var res = _database.QueryValue(Update_Users_And_Agents, parameters);
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


        // ============>>>>>>>>>>>> Add Update HomeProducts  <<<<<<<<<<========================
        [HttpPost]
        [Route("LockOut_User_Forcefully")]
        public IHttpActionResult LockOut_User_Forcefully(StoreAgent Up)
        {
            try
            {
                string result = "";
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                if (User_Id == 1 || User_Id == 16357)
                {
                    AspNetUsers Update = dbContext.AspNetUsers.First(i => i.Id == Up.AspNetId);
                    iHub_StoreAssistant_Agents Agent = dbContext.iHub_StoreAssistant_Agents.First(m => m.Store_Keeper_User_Id == Up.AspNetId);
                    if (Up.UserStatus == 10)
                    {
                        Update.AccessFailedCount = 0;
                        Update.Last_Updated_Datetime = DateTime.Now;

                        Agent.Store_User_Status = 10;
                        Agent.Last_Updated_Date = DateTime.Now;

                        result = "<b Style='Color: blue'>Login activated successful</b>";
                    }
                    else
                    {
                        Update.AccessFailedCount = 1;
                        Update.Last_Updated_Datetime = DateTime.Now;

                        Agent.Store_User_Status = 20;
                        Agent.Last_Updated_Date = DateTime.Now;
                        result = "<b Style='Color: blue'>Login deactivation successful</b>";

                    }
                    dbContext.SaveChanges();
                }
                else
                {
                    result = "<b Style='Color: red'>user do not have permissions to change the login status</b>";
                }
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
        //================>>>>>>>>>>>>> To Get Agent Details View <<<<<<<<<<<<<<<<=====================
        [HttpGet]
        [Route("Get_StoreKeepers_Data_By_Unit")]
        public IHttpActionResult Get_StoreKeepers_Data_By_Unit(int UnitID)
        {
            try
            {
                string Get_User_Details = "[iAdmin_StoreKeppers_List_By_Unit]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@UnitID", UnitID } };
                var res = _database.ProductListWithCount(Get_User_Details, parameters);
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
        // =====================>>>>>>>>>>>>>>> To Update Payment Receipts Details <<<<<<<<<<=================================
        [HttpPost]
        [Route("Update_Payment_Receipts_Details")]
        public IHttpActionResult Update_Payment_Receipts_Details(VMPagingResultsPost OFD)
        {
            try
            {
                string Update_CBOffer = "[iAdmin_View_PaymentRecepits_Orderlist]";
                int userID = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@payment_startdate", OFD.Startdate }, { "@payment_enddate", OFD.Enddate }, { "@payment_mode", OFD.PaymentMode },
                                                                                      { "@RECIEVIEDAMOUNT", OFD.ReceivingAmount },{ "@GATEWAY", OFD.Gatewaycharges },{ "@Notes",OFD.NotesDescription},{"@reconcile",OFD.ReconcileId },{ "@UserId", userID } };// ,{ "@pamentId",OFD.paymentid}  };
                var Result = _database.GetMultipleResultsList2(Update_CBOffer, parameters);
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
        // ============>>>>>>>>>>>> Payment Receipts <<<<<<<<<<========================
        [HttpPost]
        [Route("Get_Payment_Receipts_Details")]
        public IHttpActionResult Get_Payment_Receipts_Details(StoreAgent Gt)
        {
            try
            {
                string Pay_Receipts = "[iAdmin_Get_Payment_Receipts]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Year", Gt.Year },
                                                                                            { "@Month", Gt.Month },
                                                                                             { "@PayMode", Gt.PayMode },
                                                                                            { "@Page_Size", Gt.Page_Size },
                                                                                            { "@Page_Index", Gt.Page_Index }};
                var res = _database.ProductListWithCount(Pay_Receipts, parameters);
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
        // ============>>>>>>>>>>>> Payment Receipts <<<<<<<<<<========================
        [HttpPost]
        [Route("Get_Payment_Receipts_Reports_Details")]
        public IHttpActionResult Get_Payment_Receipts_Reports_Details(StoreAgent MN)
        {
            try
            {
                string Pay_Reports = "[iAdmin_Get_iHub_Daily_Sale_Report]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Years", MN.Years },
                                                                                            { "@Months", MN.Months },
                                                                                             { "@PaymentMode", MN.PaymentMode }};
                var res = _database.ProductListWithCount(Pay_Reports, parameters);
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
        //=========>>>>>>>>>>>>>>>> To Add Top Brands <<<<<<<<<<<<<<==============//
        [HttpPost]
        [Route("RejectCounterFileStatus")]
        public IHttpActionResult RejectCounterFileStatus(VMPagingResultsPost rej)
        {
            try
            {
                string Reject_CounterFile = "[iAdmin_Reject_Counter_File_Status]";
                Dictionary<string, object> parameter = new Dictionary<string, object> { { "@CID", rej.CID }, { "@RejectStatus", rej.RejectStatus }, { "@ReasonNote", rej.ReasonNote } };
                var response = _database.Query(Reject_CounterFile, parameter);
                return Ok(response);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }
        [HttpPost]
        [Route("Update_HomePage_Section_Status")]
        public IHttpActionResult Update_HomePage_Section_Status(VMPagingResultsPost oSecSta)
        {
            try
            {
                var sSectionIDs = string.Empty;
                if (oSecSta.SectionId.ToLower() == "section1")
                {
                    sSectionIDs = "SectionID='section1'";
                }
                else if (oSecSta.SectionId.ToLower() == "section2")
                {
                    sSectionIDs = "sectionID='section2'";
                }
                else if (oSecSta.SectionId.ToLower() == "section3")
                {
                    sSectionIDs = "sectionID='section3'";
                }
                else if (oSecSta.SectionId.ToLower() == "section4" || oSecSta.SectionId.ToLower() == "section5" || oSecSta.SectionId.ToLower() == "section6")
                {
                    sSectionIDs = "SectionID='section4' or SectionID='section5' or SectionID='section6'";
                }
                else if (oSecSta.SectionId.ToLower() == "section7" || oSecSta.SectionId.ToLower() == "section8" || oSecSta.SectionId.ToLower() == "section25")
                {
                    sSectionIDs = "SectionID='section7' or SectionID='section8' or SectionID='section25'";
                }
                else if (oSecSta.SectionId.ToLower() == "section9" || oSecSta.SectionId.ToLower() == "section10" || oSecSta.SectionId.ToLower() == "section11")
                {
                    sSectionIDs = "SectionID='section9' or SectionID='section10' or SectionID='section11'";
                }
                else if (oSecSta.SectionId.ToLower() == "section12")
                {
                    sSectionIDs = "SectionID='section12'";
                }
                else if (oSecSta.SectionId.ToLower() == "section13")
                {
                    sSectionIDs = "SectionID='section13' or SectionID='section14'";
                }
                else if (oSecSta.SectionId.ToLower() == "section15")
                {
                    sSectionIDs = "SectionID='section15'";
                }
                else if (oSecSta.SectionId.ToLower() == "section17")
                {
                    sSectionIDs = "SectionID='section17' or SectionID='section18'";
                }
                else if (oSecSta.SectionId.ToLower() == "section19" || oSecSta.SectionId.ToLower() == "section20" || oSecSta.SectionId.ToLower() == "section22" || oSecSta.SectionId.ToLower() == "section23")
                {
                    sSectionIDs = "SectionID='section19' or SectionID='section20' or SectionID='section22' or SectionID='section23'";
                }
                else if (oSecSta.SectionId.ToLower() == "section26")
                {
                    sSectionIDs = "SectionID='section26'";
                    var Que = "Select * from iD_HomePage where Site_ID=" + oSecSta.Site_ID + " and " + sSectionIDs;
                    Dictionary<string, object> Param = new Dictionary<string, object> { };
                    var oRes = _database.SelectQuery(Que, Param);
                    if (oRes.Count() == 0)
                    {
                        Dictionary<string, object> Param1 = new Dictionary<string, object> { };
                        //var sColumns = "HomePageID,CreatedDate, UpdatedDate, Status, Name, Type_ID, Type, DisplayOrder, SectionID, Product_Price, Product_Mrp, sProductType, Site_ID, bIsLeaf";
                        var sColumns = "CreatedDate, UpdatedDate, Status, Name, Type_ID, Type, DisplayOrder, SectionID, Product_Price, Product_Mrp, sProductType, Site_ID, bIsLeaf";
                        //var sValues = "(SELECT COALESCE(MAX(HomePageID), NULL, 0) + 1 FROM iD_HomePage),GETDATE(),GETDATE(),10,'Shop By Brand',10,'Category',26,'section26',0.00,0.00,'C'," + oSecSta.Site_ID + ",0";
                        var sValues = "GETDATE(),GETDATE(),10,'Shop By Brand',10,'Category',26,'section26',0.00,0.00,'C'," + oSecSta.Site_ID + ",0";
                        string sQuery = "INSERT into iD_HomePage (" + sColumns + ") VALUES (" + sValues + ")";
                        SQLDatabase oSql = new SQLDatabase();
                        var oObje = oSql.InsertQueryCommand(sQuery, Param1);
                    }
                }
                var Cmd = string.Empty;
                if (oSecSta.IsRefund == 1)
                {
                    Cmd = "Update iD_HomePage set sTitle=" + "'" + oSecSta.sTitle + "'" + ", iTitleStatus=" + oSecSta.iTitleStatus + " where Site_ID=" + oSecSta.Site_ID + " and " + sSectionIDs;
                }
                else
                {
                    Cmd = "Update iD_HomePage set bIsSectionStatus=" + "'" + oSecSta.bIsSectionStatus + "'" + " where Site_ID=" + oSecSta.Site_ID + " and " + sSectionIDs;
                }
                //var cmd = "Update iD_HomePage set bIsSectionStatus=" + "'" + oSecSta.bIsSectionStatus + "'" + " where Site_ID=" + oSecSta.Site_ID + " and " + sSectionIDs;
                Dictionary<string, object> parameter1 = new Dictionary<string, object> { };
                var res = _database.SelectQuery(Cmd, parameter1);

                string CacheKey = "HomePageBanners_" + oSecSta.Site_ID.ToString();
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

                CacheKey = "HomePageProductList_" + oSecSta.Site_ID.ToString();
                var keyItems1 = dbContext.CacheTransactions.Where(x => x.CacheKey == CacheKey && !x.Status).ToList();
                if (keyItems1 != null && keyItems1.Count > 0)
                {
                    foreach (var keyItem in keyItems1)
                    {
                        keyItem.Status = true;
                        keyItem.Last_Updated_DateTime = DateTime.Now;
                    }
                }

                CacheTransactions ct1 = new CacheTransactions();
                ct1.CacheKey = CacheKey;
                ct1.Type = "HomePage";
                ct1.Status = false;
                ct1.Last_Updated_DateTime = DateTime.Now;
                dbContext.CacheTransactions.Add(ct1);

                var num = dbContext.SaveChanges();

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

        // =================================Get_SectionData_By_SectionID====================================================
        [HttpPost]
        [Route("Get_SectionData_By_SectionID")]
        public IHttpActionResult Get_SectionData_By_SectionID(string sSecID)
        {
            try
            {
                Dictionary<string, object> param2 = new Dictionary<string, object>() { };
                var cmd2 = "SELECT * FROM iD_HomePage where SectionID=" + "'" + sSecID + "'";
                var oSecData = _database.SelectQuery(cmd2, param2);
                return Ok(oSecData);
            }
            catch (Exception ex)
            {
                log.Error("Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }
        //=========>>>>>>>>>>>>>>>> To Get Activity Report <<<<<<<<<<<<<<==============//
        [HttpPost]
        [Route("Get_Sale_Activity_Report_Details")]
        public IHttpActionResult Get_Sale_Activity_Report_Details(VMPagingResultsPost act)
        {
            try
            {
                string Activity = "[iAdmin_Get_Activity_Report]";
                Dictionary<string, object> parameter = new Dictionary<string, object> { { "@Year", act.Years }, { "@Month", act.Months },
                                                                                        { "@UnitID", act.UnitID }, { "@Flag", act.Flag },
                                                                                        { "@Page_Index", act.pageindex }, { "@Page_Size", act.pagesize },
                                                                                        { "@SearchDate", act.SearchDate }, { "@SearchType", act.SearchType }};
                var response = _database.ProductListWithCount(Activity, parameter);
                return Ok(response);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }
        // =====================>>>>>>>>>>>>>>> Weekly Activity Report Export into Excel <<<<<<<<<<=================================
        [HttpPost]
        [Route("Get_Export_Excel_Activity_Report")]
        public IHttpActionResult Get_Export_Excel_Activity_Report(VMPagingResultsPost GF)
        {
            try
            {
                int StoreId = Convert.ToInt32(HttpContext.Current.User.Identity.GetUnitID());
                var Report = "SELECT U.Unit_Name,(CASE WHEN UserType = 0 THEN 'Franchise' ELSE SK.Store_Keeper_Name END) AS UserName,(CASE WHEN AR.UserType = 0 THEN 'Franchise' WHEN AR.UserType = 1 THEN 'Store Assistant'ELSE 'Agent'  END) AS UserType, AR.DateFrom,AR.DateTo,AR.TotalWorkingDays,AR.WorkedDays,AR.NoOfOrders,AR.OrdersValue,AR.NoOfRegUsers,AR.NoOfWalkins,AR.Rating,AR.AnyComments,CONVERT(DATE,AR.Created_Date) AS SubmittedDate FROM iHub_Daily_Activity_Report AR LEFT JOIN iH_Stores_Keeper SK ON SK.Store_Keeper_User_Id = AR.UserID  LEFT JOIN iHub_Units_DC_WH_ST U ON U.iHub_Unit_ID = AR.UnitID WHERE ReportType = 7 AND ReportStatus = 10 AND UnitID = " + GF.UnitID + "  AND DateFrom = '" + GF.DateFrom + "' and DateTo = '" + GF.DateTo + "'";
                Dictionary<string, object> oPC = new Dictionary<string, object> { };
                var res = _database.SelectQuery(Report, oPC);
                var oData = res.FirstOrDefault();
                List<string> oHeadings = new List<string>();
                if (oData != null)
                {
                    foreach (var item in oData)
                    {
                        oHeadings.Add(item.Key); //Adding Headings
                    }
                }
                VMPagingResultsPost oHub = new VMPagingResultsPost();
                oHub.Resultset = new List<Dictionary<string, object>>();
                int b = 0;
                foreach (var d in res)
                {
                    int t = 0;
                    Dictionary<string, object> oD = new Dictionary<string, object>();
                    foreach (var q in oHeadings)
                    {
                        var oColD = d.ToList().Where(y => y.Key == q).Select(y => y.Value).FirstOrDefault();
                        oD[q] = oColD;
                        t++;
                    }
                    oHub.Resultset.Add(oD);
                    b++;
                }
                return Ok(oHub);
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
        //=============================== Create New Cashback Offer =================================
        [HttpPost]
        [Route("Create_New_Cashback_Offer")]
        public IHttpActionResult Create_New_Cashback_Offer(Cashback ba)
        {
            try
            {
                int UserID = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());
                string CB_Offer = "[iAdmin_Create_New_Cashback_Offer]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@OfferType", ba.OfferType },
                                                                                      { "@OfferName", ba.OfferName },
                                                                                      { "@OfferDescription", ba.OfferDescription },
                                                                                      { "@CashbackValue", ba.CashbackValue },
                                                                                      { "@MaxValue", ba.MaxValue },
                                                                                      { "@CreditCashback", ba.CreditCashback},
                                                                                      { "@Startdate", ba.Startdate},
                                                                                      {"@ExpiresInMonths" , ba.ExpiresInMonths},
                                                                                      {"@Domains" , ba.Domains},
                                                                                      {"@Status" , ba.Status},{"@UserId" , UserID}};
                var res = _database.QueryValue(CB_Offer, parameters);

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
        //=============================== Cashback Offers List Grid =================================
        [HttpPost]
        [Route("Get_Cashback_Offer_List")]
        public IHttpActionResult Get_Cashback_Offer_List(Cashback ba)
        {
            try
            {

                string CB_Offer_List = "[iAdmin_Get_Cashback_Offer_List]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@NameSearch", ba.NameSearch },{ "@MonthsSearch", ba.MonthsSearch },
                                                                                        { "@StatusSearch", ba.StatusSearch },{ "@DomainSearch", ba.DomainSearch },
                                                                                        { "@pagesize", ba.pagesize },{ "@pageindex", ba.pageindex }};
                var res = _database.ProductListWithCount(CB_Offer_List, parameters);
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
        // =================================View Cashback Offers From Grid===================================================
        [HttpGet]
        [Route("CBOfferDeatilsView")]
        public IHttpActionResult CBOfferDeatilsView(int CBID)
        {
            try
            {
                Dictionary<string, object> param2 = new Dictionary<string, object>() { };
                var cmd2 = "select * from iH_Cashback_Offers_List where ID=" + CBID + "";
                var oSecData = _database.SelectQuery(cmd2, param2);
                return Ok(oSecData);
            }
            catch (Exception ex)
            {
                log.Error("Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }
        // ============>>>>>>>>>>>> Update Status For CashbackOffers  <<<<<<<<<<========================
        [HttpPost]
        [Route("Update_Cashback_OfferStatus")]
        public IHttpActionResult Update_Cashback_OfferStatus(int OfferId, int NewStatus)
        {
            try
            {
                string result = "";
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                if (User_Id == 1 || User_Id == 16357)
                {
                    var cmd = "Select Status from iH_Cashback_Offers_List where ID=" + OfferId;
                    Dictionary<string, object> Param = new Dictionary<string, object> { };
                    var oSelect = _database.SelectQuery(cmd, Param);
                    if (oSelect != null && oSelect.Count() > 0)
                    {
                        var iStatus = oSelect.FirstOrDefault().Select(m => m.Value).FirstOrDefault();
                        cmd = "Update iH_Cashback_Offers_List set Status=" + "'" + NewStatus + "',Updated_Date=" + "'" + DateTime.Now + "'" + " where ID=" + OfferId;
                        Dictionary<string, object> Param1 = new Dictionary<string, object> { };
                        var oUpdate = _database.SelectQuery(cmd, Param1);
                        if (iStatus == "10")
                        {
                            result = "<b Style='Color: red'>Cashback offer deactivated successfully..!</b>";
                        }
                        else
                        {
                            result = "<b Style='Color: green'>Cashback offer activated successfully..!</b>";
                        }
                    }
                }
                else
                {
                    result = "<b Style='Color: red'>user do not have permissions to change the login status</b>";
                }
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
        // =================================View Cashback Offers From Grid===================================================
        [HttpPost]
        [Route("Update_Cashback_Offer_List")]
        public IHttpActionResult Update_Cashback_Offer_List(Cashback OFD)
        {
            try
            {
                string Update_CBOffer = "[iAdmin_Update_Cashback_Offer_List]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@ID", OFD.ID }, { "@DateFrom", OFD.Startdate }, { "@CashbackPercent", OFD.CashbackValue },
                                                                                            { "@MaxValue", OFD.MaxValue },{ "@CreditCashbackTenure", OFD.CreditCashback },{ "@ExpiresInMonths", OFD.ExpiresInMonths }};
                var Result = _database.ProductListWithCount(Update_CBOffer, parameters);
                return Ok(Result);
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
        [Route("Get_All_Saved_Master_Data")]
        public IHttpActionResult Get_All_Saved_Master_Data(int iDomainID)
        {
            try
            {
                var Cmd = "SELECT * FROM Domains_Master_Configuration where DomainID=" + iDomainID;
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
        //--Ramya code
        [HttpPost]
        [Route("View_Refund_product_Based_Transactions")]
        public IHttpActionResult View_Refund_product_Based_Transactions(Cashback Fund)
        {
            try
            {
                //int storeid = Convert.ToInt32(HttpContext.Current.User.Identity.GetUnitID());
                string ViewFund = "[iAdmin_ViewPayment_product_based_Details_Refund]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@orderid", Fund.ID }, { "@productid", Fund.productid }, { "@Flag", Fund.Flag } };
                var res = _database.GetMultipleResultsListAll(ViewFund, parameters);
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
        [Route("insert_Refund_Transactions")]
        public IHttpActionResult insert_Refund_Transactions(Cashback Fund)
        {
            try
            {
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                string ViewFund = "[iAdmin_Insert_Fund_Refund]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@NewRefund", Fund.Refundnewamount }, { "@UserId", User_Id }, { "@Notes", Fund.Notes }, { "@ProductID", Fund.productid }, { "@NewRefundAmont", 0 } };
                var res = _database.ProductListWithCount(ViewFund, parameters);
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
        [Route("ViewPromotionDetails")]
        public IHttpActionResult ViewPromotionDetails(Cashback Fund)
        {
            try
            {
                //int storeid = Convert.ToInt32(HttpContext.Current.User.Identity.GetUnitID());
                string ViewPromotions = "[iAdmin_ViewPromotions]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@orderid", Fund.ID }, { "@productid", Fund.productid }, { "@Flag", Fund.Flag } };
                var res = _database.GetMultipleResultsListAll(ViewPromotions, parameters);
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


        #endregion "END of -- HomePageManagement"
    }
}