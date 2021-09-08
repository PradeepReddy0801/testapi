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
using AspNet.Identity.SQLDatabase;
using System.Configuration;
using iHubAdminAPI.Models.CouponsModels;

namespace iHubAdminAPI.Controllers
{
    //================================= [Authorize]=====================================
    [RoutePrefix("api/OffersManagement")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class OffersManagementController : ApiController
    {
        string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private static readonly ILog log = LogManager.GetLogger(typeof(OffersManagementController).FullName);
        public SQLDatabase _database;

        public OffersManagementController()
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
        string Get_OfferDetails = "[iAdmin_Get_Multi_Offers]";
        string Update_Status = "[iAdmin_Update_Child_Offer_Details]";
        string Offer_Details = "[iAdmin_Get_Offer_Applicable_Products_Categorys_Brands_List]";
        string RecordsList = "[iAdmin_Get_Table_Records_By_Table_Name]";
        string Offers_Create = "[iAdmin_Create_Child_Offer]";
        string GET_COUNT = "[iAdmin_Get_Raffle_Count_by_Units]";
        string Raffel_Details = "[iAdmin_Get_Raffel_Draw]";
        string Customize_Offers = "[iAdmin_Get_All_Customized_Offers]";
        string Create_Customize_Offers = "[iAdmin_Create_New_Customized_Offer_For_Buyer]";
        string Update_CustomizeOffer_Status = "[iAdmin_Update_Customize_Offer_Status]";
        string Update_Parent_Offer = "[iAdmin_iO_Update_Parent_Offer_Details]";
        string BusinessMasters = "[iAdmin_Get_Business_Masters]";
        string RegisteredCompanies = "[iAdmin_Get_All_Registered_Companies]";
        string EmployeesList = "[iAdmin_Employees_List_By_CompanyId]";
        string GetCorporates = "[iAdmin_Get_All_Created_Corporate_Discount]";
        string Create_Discount = "[iAdmin_Create_New_Corporate_Discount_Offer]";
        string Update_Detais = "[iAdmin_Update_Company_Details]";
        string Update_Offer_Detais = "[iAdmin_Update_Corporate_Offer_Details]";
        string Update_EmployeeOffer_Status = "[iAdmin_Update_Employee_Offer_Status]";
        string Get_BundleOffers = "[iAdmin_Get_Bundle_OfferDetails]";
        string Get_Offered_Products = "[iAdmin_Get_Bundle_Offer_Product_Details]";
        string Get_Orders_Details = "[iAdmin_Get_Cashback_Order_Products]";
        string Save_Orders_DetailsCash = "[iAdmin_Save_Orders_DetailsCash_Back]";
        string Get_Alloted_Orders = "[iAdmin_Get_Alloted_Cashback_Offers_List]";
        string Get_Orders_Details_ToApply_BulkCoupon = "[iAdmin_Get_Order_List_To_Apply_Bulk_Coupon]";
        string Bulk_Coupon_Apply_save = "[iAdmin_Save_Bulk_Coupon_Applicable_Users]";
        string Get_Bulk_Coupon_Alloted_Orders = "[iAdmin_Get_Alloted_Bulk_Coupon_List]";

        #endregion "END of -- SP Names"

        #region "BEGIN of -- offer Management"

        // =================================To GetAllCategories=========================================
        [HttpPost]
        [Route("GetTableRecords")]
        public IHttpActionResult GetTableRecords(VMCategoriesModel gt)
        {
            try
            {
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@TableName", gt.Category_Name }};
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

        //================================Save Offers ============================================================
        [HttpPost]
        [Route("Save_Multi_offer")]
        public IHttpActionResult Save_Multi_offer(OfferCreateModel offers)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());
                Dictionary<string, object> parameter = new Dictionary<string, object>() {
                    { "@ParentOfferID", offers.ParentOfferID},
                    { "@ChildOfferName", offers.ChildOfferName},
                    { "@ChildOfferDescription", offers.ChildOfferDescription},
                    {"@OfferApplicationDomainId", offers.OfferApplicationDomainId},
                    { "@OfferUserType", offers.OfferUserType},
                    { "@OfferUsage_Limit", offers.OfferUsage_Limit},
                    { "@Offer_StartDate", Convert.ToDateTime(offers.Offer_StartDate) },
                    { "@Offer_EndDate", Convert.ToDateTime(offers.Offer_EndDate) },
                    { "@OfferApplicable_Unit_Type_Id", offers.OfferApplicable_Unit_Type_Id },
                    { "@OfferApplicable_Parent_Unit_Id", offers.OfferApplicable_Parent_Unit_Id },
                    { "@UnitIDs", offers.UnitIDs },
                    { "@IsCorporate_Discount", offers.IsCorporate_Discount},
                    { "@Offer_Sub_ChildID",  offers.Offer_Sub_ChildID },
                    { "@Offer_BuyQuantity",  offers.Offer_BuyQuantity },
                    { "@Offer_GetQuantity", offers.Offer_GetQuantity },
                    { "@OfferSale_Amount", offers.OfferSale_Amount },
                    { "@OfferDiscount_Amount", offers.OfferDiscount_Amount },
                    { "@OfferDiscount_Percentage", offers.OfferDiscount_Percentage},
                    { "@OfferBasedOnId",  offers.OfferBasedOnId },
                    { "@IsApplied",  offers.IsApplied },
                    { "@ProductIDs", offers.ProductIDs },
                    { "@CategoryIDs", offers.CategoryIDs },
                    { "@BrandIDs",offers.BrandIDs },
                    { "@UserID", userID},{ "@DomainID", offers.DomainID}
                };
                var res = _database.QueryValue(Offers_Create, parameter);
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

        // =================================To Get Special Offer Details====================================================
        [HttpPost]
        [Route("Get_OfferDetails_Data")]
        public IHttpActionResult Get_OfferDetails_Data(OfferCreateModel offers)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                        { "@OfferName", offers.ChildOfferName},
                        { "@UnitType ", offers.OfferApplicable_Unit_Type_Id},
                        { "@OfferBasedOn", offers.OfferBasedOnId},
                        {"@OfferType", offers.ParentOfferID},
                        { "@StartDate", offers.Offer_StartDate},
                        { "@EndDate", offers.Offer_EndDate},
                        { "@PageSize", offers.OfferUserType},
                        { "@PageIndex", offers.OfferUsage_Limit}
                };
                var Result = _database.ProductListWithCount(Get_OfferDetails, parameters);
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
        //================================Update Parent Offers ================================
        [HttpPost]
        [Route("Update_Parent_Offer_Details")]
        public IHttpActionResult Update_Parent_Offer_Details(OfferCreateModel offers)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());
                Dictionary<string, object> parameter = new Dictionary<string, object>() {
                    { "@ID", offers.ParentOfferID},
                    { "@Description", offers.ChildOfferDescription},
                    {"@Status",offers.OfferApplicable_Unit_Type_Id },
                     {"@Priority",offers.OfferApplicable_Parent_Unit_Id },
                    { "@UserID", userID}
                };
                var res = _database.QueryValue(Update_Parent_Offer, parameter);
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

        //========================Update Buy N Get N Offer=====================
        [HttpPost]
        [Route("Update_BuyNGetN_Offer_Details")]
        public IHttpActionResult Update_BuyNGetN_Offer_Details(OfferCreateModel offers)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());
                string Update_Parent_Offer = "SELECT * FROM \"iO_Update_BuyNGetN_Offer_Details\"" + "(@ID,@Description,@Status,@UserID)";
                Dictionary<string, object> parameter = new Dictionary<string, object>() {
                    { "@ID", offers.ParentOfferID},
                    { "@Description", offers.ChildOfferDescription},
                    {"@Status",offers.OfferApplicable_Unit_Type_Id },
                    { "@UserID", userID}
                };
                var res = _database.QueryValue(Update_Parent_Offer, parameter);
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


        //================================Update Offer Status===================================================================
        [HttpPost]
        [Route("Update_Offer_Status")]
        public IHttpActionResult Update_Offer_Status(OfferCreateModel offers)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());
                Dictionary<string, object> parameter = new Dictionary<string, object>() {
                    { "@ChildOfferID", offers.childOfferID},
                    { "@Update_Flag", offers.UpdateFlag},
                    { "@OfferName", offers.ChildOfferName},
                    {"@OfferDescription", offers.ChildOfferDescription},
                    { "@Status", offers.Status},
                    { "@OfferStart_Date", "" },
                    { "@OfferEnd_Date",""},
                    { "@Priority_Id",offers.PriorityID  },
                    { "@OfferUsageLimit", offers.OfferUsage_Limit },
                    { "@OfferApplication_DomainId", offers.OfferApplicationDomainId },
                    { "@OfferBased_On_Id", offers.OfferBasedOnId }, 
                    { "@Offer_UserType", offers.OfferUserType},
                    { "@Offer_SubChildId",  offers.Offer_Sub_ChildID },
                    { "@OfferBuy_Quantity",  offers.Offer_BuyQuantity },
                    { "@OfferGet_Quantity", offers.Offer_GetQuantity },
                    { "@OfferSale_Amount", offers.OfferSale_Amount },
                    { "@OfferDiscount_Amount", offers.OfferDiscount_Amount },
                    { "@OfferDiscount_Percentage", offers.OfferDiscount_Percentage},
                    { "@IsAppliedFor",  offers.IsApplied },
                    { "@UnitIDs",  offers.UnitIDs },
                    { "@Offer_Applicable_Unit_Type_Id", offers.OfferApplicable_Unit_Type_Id },
                    { "@Offer_Applicable_Parent_Unit_Id", offers.ParentOfferID },
                    { "@Is_Corporate_Discount",offers.IsCorporate_Discount },
                    { "@UserID", userID}
                };
                var res = _database.QueryValue(Update_Status, parameter);
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

        // =================================To Get  Offer Details====================================================
        [HttpGet]
        [Route("Get_Offer_Inner_Details_Data")]
        public IHttpActionResult Get_Offer_Inner_Details_Data(int OfferID,int OfferBasedOn)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ChildOfferID", OfferID },{ "@Offer_BasedOnID", OfferBasedOn } };
                var Result = _database.ProductListWithCount(Offer_Details, parameters);
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

        
        //==============================Get raffel coupon count by Unit IDs=============================================================
        [HttpGet]
        [Route("Get_Raffel_Count")]
        public IHttpActionResult Get_Raffel_Count(string Unit_Ids)
        {
            try
            {
                Dictionary<string, object> parameter = new Dictionary<string, object>() { { "@Unit_Ids", Unit_Ids } };
                var res = _database.QueryValue(GET_COUNT, parameter);
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

        //================================get raffel lucky draw winners===================================================================
        [HttpPost]
        [Route("Get_Lucky_Draw_List")]
        public IHttpActionResult Get_Lucky_Draw_List(OfferCreateModel offers)
        {
            try
            {
                Dictionary<string, object> parameter = new Dictionary<string, object>() {
                    { "@UnitIds", offers.UnitIDs},
                    { "@No_Of_Draw", offers.OfferUsage_Limit}
                };
                var res = _database.ProductListWithCount(Raffel_Details, parameter);
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

        //================================get customized Offers===================================================================
        [HttpPost]
        [Route("Get_Customized_Offers")]
        public IHttpActionResult Get_Customized_Offers(CustomizeOffers offers)
        {
            try
            {
                Dictionary<string, object> parameter = new Dictionary<string, object>() {
                    { "@DateFrom", offers.OfferDateFrom}, { "@DateTo", offers.OfferDateTo},{"@UnitName",offers.UnitName},
                    {"@MobileNumber",offers.MobileNumber},{"@Status",offers.OfferStatus},{"@OfferType",offers.OfferType},
                    {"@Page_Size",offers.Pagesize},{"@Page_Index",offers.Pageindex}
                };
                var res = _database.ProductListWithCount(Customize_Offers, parameter);
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

        //================================Create New Customize Offer===================================================================
        [HttpPost]
        [Route("Create_NewOffer_ForBuyer")]
        public IHttpActionResult Create_NewOffer_ForBuyer(CustomizeOffers offers)
        {
            try
            {
                Dictionary<string, object> parameter = new Dictionary<string, object>() {
                    { "@OfferId", offers.Offerid},
                    { "@UnitId", offers.UnitID},
                    {"@MobileNumber",offers.MobileNumber},
                     {"@CartValue",offers.MinCartValue},
                      {"@CashbackAmount",offers.OfferAmount}
                };
                var res = _database.QueryValue(Create_Customize_Offers, parameter);
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

        //================================Create New Customize Offer==================================================
        [HttpPost]
        [Route("Update_Customize_Offer_Status")]
        public IHttpActionResult Update_Customize_Offer_Status(CustomizeOffers offers)
        {
            try
            {
                Dictionary<string, object> parameter = new Dictionary<string, object>() { { "@OfferId", offers.Offerid }, { "@Status", offers.OfferStatus } };
                var res = _database.QueryValue(Update_CustomizeOffer_Status, parameter);
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


        //================================get customized Offers
        [HttpPost]
        [Route("Get_Ristered_Companies")]
        public IHttpActionResult Get_Ristered_Companies(CustomizeOffers offers)
        {
            try
            {
                Dictionary<string, object> parameter = new Dictionary<string, object>() {
                    { "@DateFrom", offers.OfferDateFrom}, { "@DateTo", offers.OfferDateTo},{"@CompanyName",offers.UnitName},
                     {"@MobileNumber",offers.MobileNumber},{"@Status",offers.OfferStatus},{"@Businessype",offers.OfferType},
                      {"@Page_Size",offers.Pagesize},{"@Page_Index",offers.Pageindex}
                };
                var res = _database.ProductListWithCount(RegisteredCompanies, parameter);
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

        [Route("GetMasterNatureBusiness")]
        public IHttpActionResult GetMasterNatureBusiness()
        {
            try
            {
               
                Dictionary<string, object> parameters = new Dictionary<string, object>() { };
                var Result = _database.ProductListWithCount(BusinessMasters, parameters);
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

        //================================Register New Company With EMployee=============================================================
        [HttpPost]
        [Route("Add_New_Company_With_Employees")]
        public IHttpActionResult Add_New_Company_With_Employees()
        {
            try
            {

                List<string> resss = new List<string>();
                var res1 = "";
                var Offers = HttpContext.Current.Request.Form[0];
                CustomizeOffers category = JsonConvert.DeserializeObject<CustomizeOffers>(Offers);
                HttpPostedFile file = HttpContext.Current.Request.Files[0];
                var pathToExcel = file.FileName;
                var Dictionary = new List<string>();
                string str = "";
                Excel.IExcelDataReader reader = null;
                List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();
                Dictionary<string, string> product;
                string xlfilename = file.FileName;
                string Create_Company = "[iAdmin_Register_New_Company_For_Corporate_Discount]";
                Dictionary<string, object> parameter = new Dictionary<string, object>() { { "@CompanyName ", category.CompanyName}, { "@BusinessTypeId ", category.BusinessTypeId},{ "@DirectorName ", category.DirectorName},
                   {"@MobileNumber ",category.MobileNumber},{"@MailId ",category.MailId},{"@Address ",category.Address},{"@Contact_Person_Name ",category.Contact_Person_Name},
                      {"@Contact_Person_MobileNumber  ",category.Contact_Person_MobileNumber},{"@Contact_Person_MailId  ",category.Contact_Person_MailId}

               };
                var CompanyId = _database.QueryValue(Create_Company, parameter);

                var Response = 0;

                if (Convert.ToInt32(CompanyId) != 0)
                {
                    using (var stream = file.InputStream)
                    {
                        if (file.FileName.EndsWith(".xls"))
                        {
                            reader = ExcelReaderFactory.CreateOpenXmlReader(stream); //ExcelReaderFactory thowing an error but not an issue
                        }
                        else if (file.FileName.EndsWith(".xlsx"))
                        {
                            reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        }
                        else
                        {

                            return Ok("Invalid file format");
                        }
                    }
                    DataSet result = reader.AsDataSet();
                    reader.Close();
                    DataTable data = result.Tables[0];
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    var excelColdata = new List<string>();
                    List<int> ColumnIndexes = new List<int>();
                    List<List<string>> sListOfRecords = new List<List<string>>();
                    int i = 1;
                    bool EmpName = false;
                    bool EmpGender = false;
                    bool EmpDOJ = false;
                    bool EmpMobileNumber = false;

                    foreach (var column in data.Rows[0].ItemArray.Where(m => m is string))
                    {
                        if (column.ToString().Trim() == "EmployeeName")
                        {
                            EmpName = true;
                        }
                        if (column.ToString().Trim() == "MobileNumber")
                        {
                            EmpMobileNumber = true;
                        }
                        if (column.ToString().Trim() == "Gender")
                        {
                            EmpGender = true;
                        }
                        if (column.ToString().Trim() == "DOJ")
                        {
                            EmpDOJ = true;
                        }
                        string ColumnName = column.ToString();
                        ColumnName.Replace("'", " ");
                        Dictionary.Add(ColumnName);

                    }
                    if (EmpName == false)
                    {
                        return Ok("Employee Name Coloumn required");
                    }
                    if (EmpMobileNumber == false)
                    {
                        return Ok("Employee Mobile Number Coloumn required");
                    }
                    if (EmpGender == false)
                    {
                        return Ok("Employee Gender Coloumn required");
                    }
                    if (EmpDOJ == false)
                    {
                        return Ok("Employee DOJ Coloumn required");
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
                                    excelRowdata.Add(item.ToString());
                                    string itemname = item.ToString();
                                    product.Add(sListOfRecords[0][j - 1].TrimStart().TrimEnd(), itemname.ToString().TrimStart().TrimEnd());
                                }
                                if (flag) break;
                                j++;
                            }
                            rows.Add(product);
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
                            string commandText = "[iAdmin_Register_Employess_For_Corporate_Discount]";
                            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Company_Id", CompanyId }, { "@EmployeeJsonData", resultss } };
                            var response = _database.QueryValue(commandText, parameters);
                        }
                        Response = Convert.ToInt32(CompanyId);
                    }
                    else
                    {
                        Response = -1;
                    }


                }
                else
                {
                    Response = 0;
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }
        //================================get customized Offers==========================
        [HttpPost]
        [Route("Get_Employees_By_CompanyId")]
        public IHttpActionResult Get_Employees_By_CompanyId(CustomizeOffers offers)
        {
            try
            {
                Dictionary<string, object> parameter = new Dictionary<string, object>() {
                    { "@Id", offers.ID}, { "@Name", offers.EmployeeName},{"@MobileNumber",offers.EmployeeMobileNumber},
                     {"@Designation",offers.EmployeeDesignation},{"@Mailid",offers.EmployeeMailid},{"@Status",offers.EmployeeStatus},
                      {"@Page_Size",offers.Pagesize},{"@Page_Index",offers.Pageindex}
                };
                var res = _database.ProductListWithCount(EmployeesList, parameter);
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
        //================================get customized Offers
        [HttpPost]
        [Route("Get_All_Corporate_Discounts")]
        public IHttpActionResult Get_All_Corporate_Discounts(CustomizeOffers offers)
        {
            try
            {
                Dictionary<string, object> parameter = new Dictionary<string, object>() {
                    { "@DateFrom", offers.OfferDateFrom}, { "@DateTo", offers.OfferDateTo},{"@CompanyName",offers.CompanyName},
                    {"@Status",offers.OfferStatus},{"@AppliedPercentage",offers.OfferType},
                      {"@Page_Size",offers.Pagesize},{"@Page_Index",offers.Pageindex}
                };
                var res = _database.ProductListWithCount(GetCorporates, parameter);
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

        //================================Create New Customize Offer
        [HttpPost]
        [Route("Create_New_Corporate_Discount")]
        public IHttpActionResult Create_New_Corporate_Discount(CustomizeOffers Vm)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());
                Dictionary<string, object> parameter = new Dictionary<string, object>() { { "@Companyid ", Vm.ID}, { "@OfferType ",3},{ "@Percentage ", Vm.ApplicablePercentage},
                    {"@ExcludeCatIds ",Vm.CatIds},{"@Valid_From ",Vm.Corporate_DiscountDateFrom},{"@Valid_To ",Vm.Corporate_DiscountDateTo},{"@Applicable_On ",Vm.Applicableon},
                       {"@Times_Of_use  ",Vm.TimesOfUse},{"@MaxDiscount  ",Vm.OfferAmount},{"@UserId  ",userID}

                };
                var CompanyId = _database.QueryValue(Create_Discount, parameter);
                return Ok(CompanyId);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }

        //==============================Get hierarchy Of Ware House and store
        [HttpGet]
        [Route("Get_Companies_List")]
        public IHttpActionResult Get_Companies_List()
        {
            try
            {
                string get_Lits = "SELECT \"Company_ID\", \"Company_Name\",\"Offer_Created\" FROM \"iHub_Corporate_Companies\" WHERE \"Company_Status\" = 10";
                Dictionary<string, object> parameter = new Dictionary<string, object>() { };
                var res = _database.SelectQuery(get_Lits, parameter);
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
        //==============================To Update Company Details==========================================================================
        [HttpPost]
        [Route("Update_Company_Details")]
        public IHttpActionResult Update_Company_Details()
        {
            try
            {
                List<string> resss = new List<string>();
                var res1 = "";
                var Offers = HttpContext.Current.Request.Form[0];
                CustomizeOffers category = JsonConvert.DeserializeObject<CustomizeOffers>(Offers);
                if (category.Flag == "Edit")
                {
                    Dictionary<string, object> parameter = new Dictionary<string, object>() { { "@CId ", category.ID},{ "@DirectorName ", category.DirectorName},
                   {"@MobileNumber ",category.MobileNumber},{"@MailId ",category.MailId},{"@Address ",category.Address},{"@Contact_Person_Name ",category.Contact_Person_Name},
                      {"@Contact_Person_MobileNumber  ",category.Contact_Person_MobileNumber},{"@Contact_Person_MailId  ",category.Contact_Person_MailId},{"@Status  ",category.OfferStatus}};
                    var Response = _database.QueryValue(Update_Detais, parameter);
                    return Ok(Response);
                }
                else
                {

                    HttpPostedFile file = HttpContext.Current.Request.Files[0];
                    var pathToExcel = file.FileName;
                    var Dictionary = new List<string>();
                    string str = "";
                    Excel.IExcelDataReader reader = null;
                    List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();
                    Dictionary<string, string> product;
                    string xlfilename = file.FileName;
                    Dictionary<string, object> parameter = new Dictionary<string, object>() { { "@CId ", category.ID},{ "@DirectorName ", category.DirectorName},
                   {"@MobileNumber ",category.MobileNumber},{"@MailId ",category.MailId},{"@Address ",category.Address},{"@Contact_Person_Name ",category.Contact_Person_Name},
                      {"@Contact_Person_MobileNumber  ",category.Contact_Person_MobileNumber},{"@Contact_Person_MailId  ",category.Contact_Person_MailId},{"@Status  ",category.OfferStatus}

               };
                    var Response = _database.QueryValue(Update_Detais, parameter);
                    if (xlfilename != null && xlfilename != "")
                    {
                        using (var stream = file.InputStream)
                        {
                            if (file.FileName.EndsWith(".xls"))
                            {
                                reader = ExcelReaderFactory.CreateOpenXmlReader(stream); //ExcelReaderFactory thowing an error but not an issue
                            }
                            else if (file.FileName.EndsWith(".xlsx"))
                            {
                                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                            }
                            else
                            {

                                return Ok("Invalid file format");
                            }
                        }
                        DataSet result = reader.AsDataSet();
                        reader.Close();
                        DataTable data = result.Tables[0];
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        var excelColdata = new List<string>();
                        List<int> ColumnIndexes = new List<int>();
                        List<List<string>> sListOfRecords = new List<List<string>>();
                        int i = 1;
                        bool EmpName = false;
                        bool EmpGender = false;
                        bool EmpDOJ = false;
                        bool EmpMobileNumber = false;

                        foreach (var column in data.Rows[0].ItemArray.Where(m => m is string))
                        {
                            if (column.ToString().Trim() == "EmployeeName")
                            {
                                EmpName = true;
                            }
                            if (column.ToString().Trim() == "MobileNumber")
                            {
                                EmpMobileNumber = true;
                            }
                            if (column.ToString().Trim() == "Gender")
                            {
                                EmpGender = true;
                            }
                            if (column.ToString().Trim() == "DOJ")
                            {
                                EmpDOJ = true;
                            }
                            string ColumnName = column.ToString();
                            ColumnName.Replace("'", " ");
                            Dictionary.Add(ColumnName);

                        }
                        string HtmlContent = "";
                        if (EmpName == false)
                        {
                            return Ok("Employee Name Coloumn required");
                        }
                        if (EmpMobileNumber == false)
                        {
                            return Ok("Employee Mobile Number Coloumn required");
                        }
                        if (EmpGender == false)
                        {
                            return Ok("Employee Gender Coloumn required");
                        }
                        if (EmpDOJ == false)
                        {
                            return Ok("Employee DOJ Coloumn required");
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

                                int Price = 0;

                                int j = 1;
                                product = new Dictionary<string, string>();
                                if (Convert.ToString(row.ItemArray[0]) == string.Empty) { break; }
                                foreach (var item in row.ItemArray)
                                {

                                    if (ColumnIndexes.Contains(j))
                                    {
                                        excelRowdata.Add(item.ToString());
                                        string itemname = item.ToString();
                                        product.Add(sListOfRecords[0][j - 1].TrimStart().TrimEnd(), itemname.ToString().TrimStart().TrimEnd());
                                    }
                                    if (flag) break;
                                    j++;
                                }
                                rows.Add(product);
                                res1 = serializer.Serialize(product);
                                resss.Add(res1);
                                if (excelRowdata.Where(m => !string.IsNullOrWhiteSpace(m)).Count() > 0)
                                {
                                    sListOfRecords.Add(excelRowdata);
                                }

                            }
                            i++;
                        }
                    }
                    if (resss.Count != 0)
                    {
                        foreach (var resultss in resss)
                        {
                            string commandText = "[iAdmin_Register_Employess_For_Corporate_Discount]";
                            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Company_Id", category.ID }, { "@EmployeeJsonData", resultss } };
                            var response = _database.QueryValue(commandText, parameters);
                        }
                    }
                    else
                    {
                        return Ok("Invalid Values Entered");
                    }
                    return Ok(Response);
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

        //==============================Get hierarchy Of Ware House and store

        [Route("Get_Categories_List")]
        public IHttpActionResult Get_Categories_List(int OfferId)
        {
            try
            {
                string get_Lits = "SELECT \"CategoryName\", \"ID\" FROM \"iH_Categories\" WHERE \"ID\"" +
                    " IN (SELECT \"Exclude_Category_Id\" FROM \"iHub_Discount_Offers_Exclude_Categories\" WHERE \"Offer_Id\"=" + OfferId + ")";
                Dictionary<string, object> parameter = new Dictionary<string, object>() { };
                var res = _database.SelectQuery(get_Lits, parameter);
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

        //=================Update corporate offer Details==========================
        [HttpPost]
        [Route("Update_Corporate_Offer_Details")]
        public IHttpActionResult Update_Corporate_Offer_Details(CustomizeOffers category)
        {
            try
            {
                Dictionary<string, object> parameter = new Dictionary<string, object>() {{"@OfferId",category.ID }, { "@DateTo ", category.OfferDateTo},{ "@Percentage ", category.ApplicablePercentage},
                    {"@Amount ",category.OfferAmount},{"@Status ",category.OfferStatus} };
                var Response = _database.QueryValue(Update_Offer_Detais, parameter);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }
        //================================Update Employee Offer Status
        [HttpPost]
        [Route("Update_Employee_Offer_Status")]
        public IHttpActionResult Update_Employee_Offer_Status(CustomizeOffers offers)
        {
            try
            {
                Dictionary<string, object> parameter = new Dictionary<string, object>() { { "@Id", offers.Offerid }, { "@Status", offers.OfferStatus } };
                var res = _database.QueryValue(Update_EmployeeOffer_Status, parameter);
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
        // =================================To Get Special Offer Details====================================================
        [HttpPost]
        [Route("Get_Bundle_OfferDetails_Data")]
        public IHttpActionResult Get_Bundle_OfferDetails_Data(VMPagingResultsPost GF)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@status",GF.Status}
                    , { "@start_date",GF.FromDate }
                    , { "@end_date", GF.ToDate }
                    , { "@pageindex", GF.PageIndex}
                    , { "@pagesize",GF.Pagesize}};
                var Result = _database.ProductListWithCount(Get_BundleOffers, parameters);
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
        //============================================Get Ordered Products By OrderID=====================================
        [HttpGet]
        [Route("GetBundleOfferProducts")]
        public IHttpActionResult GetBundleOfferProducts(int OfferId)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@offer_id", OfferId } };
                var res = _database.ProductListWithCount(Get_Offered_Products, parameters);
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
        //-----------------------------------------Method For Upload Image for Bundle Offer -----------
        [HttpPost]
        [Route("Upload_Bundle_Offer_Image")]
        public IHttpActionResult Upload_Bundle_Offer_Image(int ClaimID)
        {
            try
            {
                HttpPostedFile file = HttpContext.Current.Request.Files[0];
                string physicalPath = HttpContext.Current.Server.MapPath("~/" + "Images/BundleOffer/");
                if (!System.IO.File.Exists(physicalPath))
                {
                    Directory.CreateDirectory(physicalPath);
                }
                string ExteName = Path.GetExtension(file.FileName);
                string imgPath = "";
                imgPath = HttpContext.Current.Server.MapPath("~/" + "Images/BundleOffer/" + ClaimID + ".jpg");
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

        //======================================================================================================Cash_Back==================================
        // =================================To Get Category List Cash_Back ====================================
        [HttpGet]
        [Route("GetCategoryListCash_Back")]
        public IHttpActionResult GetCategoryListCash_Back(int ParentID)
        {
            try
            {
                string Category_GetListnew = "[iAdmin_Get_Category_List_Cash_Back]";
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
        [HttpPost]
        [Route("Get_Orders_DetailsCash_Back")]
        public IHttpActionResult Get_Orders_DetailsCash_Back(VMPagingResultsPost GF)
        {
            try
            {
                if (GF.ExcludedFilters == "True")
                {
                    GF.ExcludedFilters = "1";
                }
                else
                {
                    GF.ExcludedFilters = "0";
                }
                if (GF.ExcludedEMIOrders == "True")
                {
                    GF.ExcludedEMIOrders = "1";
                }
                else
                {
                    GF.ExcludedEMIOrders = "0";
                }
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Order_Date_From",GF.orderfrom}
                    , { "@Order_Date_To",GF.orderto }
                    , { "@Page_Index", GF.PageIndex}
                    , { "@Page_Size",GF.Pagesize}
                     , { "@Orders_Value_Greter",GF.Orders_Value_Greter}
                    , {"@CashBacklistID",GF.CashBacklistID }
                    , {"@ExcludeBulkOrders",GF.ExcludedFilters }
                    , {"@ExcludeWalletAmount",GF.WalletApplyon }
                    ,{"@ExcludeEMIOrders",GF.ExcludedEMIOrders }
                    ,{"@Applyon",GF.Applyon },
                    { "@saveToTables",GF.saveToTables}
                };
                var Result = _database.GetMultipleResultsList(Get_Orders_Details, parameters);
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
        [HttpGet]
        [Route("GetCashback_OfferListData")]
        public IHttpActionResult GetCashback_OfferListData()
        {
            try
            {
                string OfferListData = "SELECT * FROM iH_Cashback_Offers_List WHERE Status=10 ";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { };
                var Cashback_OfferListData = _database.SelectQuery(OfferListData, parameters);
                return Ok(Cashback_OfferListData);
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
        [Route("Save_Orders_DetailsCash_Back")]
        public IHttpActionResult Save_Orders_DetailsCash_Back(VMPagingResultsPost GF)
        {
            try
            {
                if (GF.ExcludedFilters == "True")
                {
                    GF.ExcludedFilters = "1";
                }
                else
                {
                    GF.ExcludedFilters = "0";
                }
                if (GF.ExcludedEMIOrders == "True")
                {
                    GF.ExcludedEMIOrders = "1";
                }
                else
                {
                    GF.ExcludedEMIOrders = "0";
                }
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Order_Date_From",GF.orderfrom}
                    , { "@Order_Date_To",GF.orderto }
                    , { "@Page_Index", GF.PageIndex}
                    , { "@Page_Size",GF.Pagesize}
                     , { "@Orders_Value_Greter",GF.Orders_Value_Greter}
                    , {"@CashBacklistID",GF.CashBacklistID }
                    , {"@ExcludeBulkOrders",GF.ExcludedFilters }
                    , {"@ExcludeWalletAmount",GF.WalletApplyon }
                    ,{"@ExcludeEMIOrders",GF.ExcludedEMIOrders }
                    ,{"@Applyon",GF.Applyon },
                {"@EditValues",GF.EditValues } };
                var Result = _database.QueryValue(Save_Orders_DetailsCash, parameters);
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
        //==============>>>>>>>>>>> To Get Alloted Cashback Offers List
        [HttpPost]
        [Route("Get_Assign_CBOffer_Orders_List")]
        public IHttpActionResult Get_Assign_CBOffer_Orders_List(VMPagingResultsPost GF)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@OfferName",GF.Message},
                                                                                            { "@MobileNumber",GF.Phone_Number},
                                                                                            { "@CustomerName",GF.ContactName},
                                                                                            { "@OrderId",GF.OrderID},
                                                                                            { "@FromDate",GF.FromDate},
                                                                                            { "@Page_Index", GF.PageIndex},
                                                                                            { "@Page_Size",GF.Pagesize}};
                var Result = _database.GetMultipleResultsList(Get_Alloted_Orders, parameters);
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

        [HttpPost]
        [Route("DetailsBUyerExcluded")]
        public IHttpActionResult DetailsBUyerExcluded(VMPagingResultsPost model)
        {
            try
            {
                string Save_mobile_Details = "[SaveCustomerMobileNumber]";
                int userID = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"Mobile_Number",model.mobile_number },{"USERID",userID }
                };
                var savemobileDetails = _database.ProductListWithCount(Save_mobile_Details, parameters);
                return Ok(savemobileDetails);


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
        [Route("Get_Buyer_Details")]
        public IHttpActionResult Get_Buyer_Details(string MobileNumber, int Pagesize, int Page_Index)
        {
            try
            {
                string Get_BuyerCashback_Details = "[View_CashBack_Buyer_Details]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Mobile_number", MobileNumber }, { "@PageSize", Pagesize }, { "@PageIndex", Page_Index } };
                var BuyerCashback_Details = _database.Query(Get_BuyerCashback_Details, parameters);
                return Ok(BuyerCashback_Details);
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
        [Route("UploadCashbackBuyerslist")]
        public IHttpActionResult UploadCashbackBuyerslist()
        {
            try
            {
                //int userID = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());
                CustomResponse cs = new CustomResponse();
                List<string> resss = new List<string>();
                var res1 = "";
                var categorydata = HttpContext.Current.Request.Form[0];
                // iH_Categories category = JsonConvert.DeserializeObject<iH_Categories>(categorydata);
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

                    foreach (var column in data.Rows[0].ItemArray.Where(m => m is string))
                    {
                        if (column.ToString().Trim() == "MobileNumber")
                        {
                            ColomnrequiredMobileNumber = true;
                        }

                        string ColumnName = column.ToString();
                        ColumnName.Replace("'", " ");
                        Dictionary.Add(ColumnName);

                    }

                    if (ColomnrequiredMobileNumber == false)
                    {
                        return Ok("Invalid MobileNumber Coloumn Name");
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
                            int userID = 0;
                            cashbackdetails a = JsonConvert.DeserializeObject<cashbackdetails>(resultss);
                            //string commandTextnew = "Select \"Buyers_ID\" from \"iD_Buyers\" where \"Mobile_Number\" = " + "'" + a.MobileNumber + "'";
                            string Save_mobile_Details = "[SaveCustomerMobileNumber]";

                            Dictionary<string, object> parameters = new Dictionary<string, object>()
                        {
                            {"Mobile_Number",a.MobileNumber },{"USERID",userID }
                        };
                            var savemobileDetails = _database.ProductListWithCount(Save_mobile_Details, parameters);

                            // return Ok();

                        }
                        return Ok(1);

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
        [Route("DeleteBuyerCashback")]
        public IHttpActionResult DeleteBuyerCashback(VMPagingResultsPost model)
        {
            try
            {
                string DeleteDetails = "Delete from CashbackExcludeUsers where MobileNumber=" + "'" + model.MobileNumber + "'" + "";
                //  = " + "'" + BuyerDetailsType + "'" + " and BuyerCards_Schemes_ID = " + BuyerCardsID + "";
                //int userID = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());+ "'" + a.MobileNumber + "'
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {

                };
                var delete = _database.SelectQuery(DeleteDetails, parameters);
                return Ok(delete);


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
        [Route("UpdateCashbackPercentage")]
        public IHttpActionResult UpdateCashbackPercentage(VMPagingResultsPost model)
        {
            try
            {
                string UpdateCashBackDetails = "[iAdmin_Update_Cashback_Percentage]";
                //  = " + "'" + BuyerDetailsType + "'" + " and BuyerCards_Schemes_ID = " + BuyerCardsID + "";
                //int userID = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());+ "'" + a.MobileNumber + "'
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    { "CashbackPercentage",model.Percentage}
                };
                var UpdateCashBack = _database.Query(UpdateCashBackDetails, parameters);
                return Ok(UpdateCashBack);


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
        [Route("Bulk_Coupon_List")]
        public IHttpActionResult Bulk_Coupon_List()
        {
            try
            {
                string OfferListData = "SELECT * FROM iHub_Multi_Coupons WHERE Status=1 and Corporate_Franchise='B'";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { };
                var Cashback_OfferListData = _database.SelectQuery(OfferListData, parameters);
                return Ok(Cashback_OfferListData);
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
        [Route("Get_Orders_Details_ToApply_Bulk_Coupon")]
        public IHttpActionResult Get_Orders_Details_ToApply_Bulk_Coupon(VMPagingResultsPost GF)
        {
            try
            {
                if (GF.ExcludedFilters == "True")
                {
                    GF.ExcludedFilters = "1";
                }
                else
                {
                    GF.ExcludedFilters = "0";
                }
                if (GF.ExcludedEMIOrders == "True")
                {
                    GF.ExcludedEMIOrders = "1";
                }
                else
                {
                    GF.ExcludedEMIOrders = "0";
                }
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                      {"@SpecificUser", GF.PhoneNumber }
                    , {"@usertype", GF.ID }
                    , {"@ordercount", GF.ordercount }
                    , {"@ordercountfilter", GF.ordercountfilter }
                    , {"@Order_Date_From",GF.orderfrom}
                    , {"@Order_Date_To",GF.orderto }
                    , {"@Page_Index", GF.PageIndex}
                    , {"@Page_Size",GF.Pagesize}
                    , {"@Orders_Value_Greter",GF.Orders_Value_Greter}
                    , {"@CashBacklistID",GF.CashBacklistID }
                    , {"@ExcludeBulkOrders",GF.ExcludedFilters }
                    , {"@ExcludeEMIOrders",GF.ExcludedEMIOrders }
                    , {"@ExcludeWalletAmount",GF.WalletApplyon }
                    , {"@Applyon",GF.Applyon }
                    , {"@saveToTables",GF.saveToTables}
                    , {"@fileName",GF.fileName}};
                var Result = _database.GetMultipleResultsList(Get_Orders_Details_ToApply_BulkCoupon, parameters);
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

        [HttpPost]
        [Route("Bulk_Coupon_Apply")]
        public IHttpActionResult Bulk_Coupon_Apply(VMPagingResultsPost GF)
        {
            try
            {
                if (GF.ExcludedFilters == "True")
                {
                    GF.ExcludedFilters = "1";
                }
                else
                {
                    GF.ExcludedFilters = "0";
                }
                if (GF.ExcludedEMIOrders == "True")
                {
                    GF.ExcludedEMIOrders = "1";
                }
                else
                {
                    GF.ExcludedEMIOrders = "0";
                }
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                    {"@SpecificUser", GF.PhoneNumber }, //{"@buyerid", GF.BuyerId },
                    { "@usertype", GF.ID }
                     , {"@ordercount", GF.ordercount }
                    , {"@ordercountfilter", GF.ordercountfilter },
                    { "@Order_Date_From",GF.orderfrom}
                    , { "@Order_Date_To",GF.orderto }
                    , { "@Page_Index", GF.PageIndex}
                    , { "@Page_Size",GF.Pagesize}
                     , { "@Orders_Value_Greter",GF.Orders_Value_Greter}
                    , {"@CashBacklistID",GF.CashBacklistID }
                    , {"@ExcludeBulkOrders",GF.ExcludedFilters }
                    ,{"@ExcludeEMIOrders",GF.ExcludedEMIOrders }
                    , {"@ExcludeWalletAmount",GF.WalletApplyon }
                    ,{"@Applyon",GF.Applyon },
                    {"@EditValues",GF.EditValues } 
                };
                var Result = _database.QueryValue(Bulk_Coupon_Apply_save, parameters); //iAdmin_Save_Bulk_Coupon_Applicable_Users
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

        //==============>>>>>>>>>>> To Get Alloted Cashback Offers List
        [HttpPost]
        [Route("Get_Bulk_Coupon_Applied_List")]
        public IHttpActionResult Get_Bulk_Coupon_Applied_List(VMPagingResultsPost GF)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@CashBacklistID",GF.CashBacklistID},
                                                                                            { "@MobileNumber",GF.Phone_Number},
                                                                                            { "@CustomerName",GF.ContactName},
                                                                                            //{ "@OrderId",GF.OrderID},
                                                                                            //{ "@FromDate",GF.FromDate},
                                                                                            { "@Page_Index", GF.PageIndex},
                                                                                            { "@Page_Size",GF.Pagesize}};
                var Result = _database.GetMultipleResultsList2(Get_Bulk_Coupon_Alloted_Orders, parameters);
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
        [HttpGet]
        [Route("GetDomainsForBulkCoupon")]
        public IHttpActionResult GetDomainsForBulkCoupon()
        {
            try
            {
                string OfferListData = "SELECT * FROM iHub_Domains";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { };
                var Domains = _database.SelectQuery(OfferListData, parameters);
                return Ok(Domains);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }

        }

        #endregion "BEGIN of -- offer Management"
    }
}