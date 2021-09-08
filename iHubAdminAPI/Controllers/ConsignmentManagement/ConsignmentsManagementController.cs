using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Excel;
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
using System.Web.Script.Serialization;
using System.Net;
using System.Net.Sockets;
using iHubAdminAPI.Mailer;
using System.Web.Http.Results;

namespace iHubAdminAPI.Controllers.ConsignmentManagement
{
    //============== [Authorize]=========================================
    [RoutePrefix("api/Consignments")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ConsignmentsManagementController : ApiController
    {
        string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private static readonly ILog log = LogManager.GetLogger(typeof(ConsignmentsManagementController).FullName);
        public SQLDatabase _database;
        iHubDBContext dbContext = new iHubDBContext();
        IUsermailer Mailer = new Usermailer();
        public ConsignmentsManagementController()
        {
            _database = new SQLDatabase();
        }
        private class CustomResponse
        {
            internal string Response;
        }
        #region "BEGIN of -- SP Names"

        // =================================Command Rext For Api Calls====================================================
        string Consignment_Create = "[iC_Get_Assigned_Products_List_By_Source_UnitID_Dest_Unit_ID]";
        string Stock_Moving = "[iAdmin_Get_Assigned_Stock_Units_List]";
        string Products_List = "[iAdmin_Get_Consignment_List_With_Filters]";
        string Dammaged_Products_List = "[iAdmin_Get_Dammaged_Inventory_Product_Consignments_List]";
        string Dammaged_Products_List_Date = "[iAdmin_Get_Dammaged_Invetory_Product_Consignments_List_Date]";
        string Change_Bulk_Status = "[iAdmin_Get_Dammaged_Inventory_Product_List_By_Consignment_Id]";
        string Dammaged_Inventory_Product = "[iAdmin_Get_Dammaged_Inventory_Product_Details]";
        string Get_Inventory_Products_List = "[iC_Get_Assigned_Inventory_Product_Ids]";
        string Get_Inventory_new_Consignment_List = "[iC_Create_New_Consignment]";
        string Change_Bulk_Consignment_Status = "[iC_Change_Bulk_Consignment_Status]";
        string Change_Status = "[iC_Change_Consignment_Status]";
        string TrackingProduct = "[iAdmin_Product_Location_Details]";
        string View_Consignment_List = "[iAdmin_View_Created_Consignments_For_Product_Details]";
        string To_Get_Partially_Received_List = "[iAdmin_Get_Partially_Received_DC_Consignments_List]";
        string Update_Dammaged_Product = "[iC_Update_Dammaged_Product_to_Store_Stock]";
        string Get_Accept_Consignment_List = "[iAdmin_Accept_Consignment_Products]";
        string Get_Assign_Inventory_Products_List = "[iAdmin_Get_Recived_Inventory_Product_Ids]";
        string More_Details = "[iAdmin_Get_Consignment_Details]";
        string Get_Assigned_StockUnits = "[iAdmin_Get_Unique_serial_Number_In_Printchallana]";
        string Assigned_Inventory_Product = "[iC_Get_Assigned_Inventory_Product_Ids_To_Dc]";
        string Get_UniqueSerial_number = "[iAdmin_Get_Unique_serial_Number]";
        string Get_Inventory_Products_List_dammaged = "[iC_Get_Assigned_Inventory_Product_Ids_Damage_Missing]";
        string Get_Inventory_Products_List_missing = "[iAdmin_Assigned_Stock_For_Units_Damages_Missing]";
        string Get_Inventory_Products_missing = "[iC_Get_Assigned_Inventory_Product_Ids_Missing]";
        string Get_Assigned_Products_List = "[iAdmin_Get_Assigned_Products_List_Print]";
        string Products_List_Invioce = "[iAdmin_Get_Consignment_List_With_Filters_And_Invioce]";
        string Get_Partially_Received_List = "[iAdmin_Get_Excel_List]";
        string Get_Inventory_Products_Damage_List = "[iC_Get_Assigned_Inventory_Product_Ids_Damage_List]";
        string Get_Inventory_Products_Packed_List = "[iC_Get_Assigned_Inventory_Product_Ids_Packed_List]";
        #endregion "END of -- SP Names"

        #region "BEGIN of -- Consignments Management"

        //===================== GET Assigned Products List =================
        [HttpPost]
        [Route("GetAssignedProductsList")]
        public IHttpActionResult GetAssignedProductsList(VMConsignments consignment)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Unit_ID", consignment.Unit_ID }
                  , { "@DC_Unit_ID", consignment.DC_Unit_ID }
                  , { "@SKU", consignment.SKU }
                  , { "@OrderNumber", consignment.OrderNumber }
                  , { "@ProductName", consignment.ProductName }
                  , { "@MobileNumber", consignment.MobileNumber }
                  , { "@OrderType", consignment.OrderType }
                  , { "@OrderBy", consignment.OrderBy }
                  , { "@Page_Size", 10 }
                  , { "@Page_Index", 1 }
                  , { "@ParentCategoryID", consignment.TopCategoryID}
                  , { "@LeafCatID", consignment.CategoryID}
                };
                var Consignment_Create_List = _database.ProductListWithCount(Consignment_Create, parameters);
                return Ok(Consignment_Create_List);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        //===================== GET Assigned Stock Store/Warehouse List =================
        [HttpGet]
        [Route("GetAssignedStockUnitsList_New")]
        public IHttpActionResult GetAssignedStockUnitsList_New(int Src_UnitID, int Dest_UnitID, int Hierarchy_Level_Id)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Source_Unit_ID", Src_UnitID },
                { "@Destination_Unit_ID", Dest_UnitID },{ "@Hierarchy_Level_Id", Hierarchy_Level_Id }
                };
                var Stock_Moving_ListNew = _database.ProductListWithCount(Stock_Moving, parameters);
                return Ok(Stock_Moving_ListNew);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        //===================== Created Consignments List=================
        [HttpPost]
        [Route("Get_Consignments_List")]
        public IHttpActionResult Get_Consignments_List(Consignments consignment)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Cons_Name", consignment.Consignment_Name },
                { "@Cons_Status", consignment.Consignment_Status },
                    { "@Source_ID", consignment.Source_Unit_Id },
                    { "@Dest_ID", consignment.Destination_Unit_Id },
                    { "@CreateDate", consignment.Created_Date },
                    { "@Page_Size", consignment.Pagesize },
                    { "@Page_Index", consignment.Pageindex }
                };
                var Products_ListNew = _database.ProductListWithCount(Products_List, parameters);
                return Ok(Products_ListNew);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        [HttpPost]
        [Route("Get_Consignments_ListOnline")]
        public IHttpActionResult Get_Consignments_ListOnline(Consignments consignment)
        {
            try
            {
                string Products_List = "[iAdmin_Get_Consignment_List_With_Filters_B2b]";//sp name chaged
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Cons_Name", consignment.Consignment_Name },
                { "@Cons_Status", consignment.Consignment_Status },
                    { "@Source_ID", consignment.Source_Unit_Id },
                    { "@Dest_ID", consignment.Destination_Unit_Id },
                    { "@CreateDate", consignment.Created_Date },
                    { "@Page_Size", consignment.Pagesize },
                    { "@Page_Index", consignment.Pageindex },
                    { "@DomainID", consignment.DomainID }
                };
                var Products_ListNew = _database.ProductListWithCount(Products_List, parameters);
                return Ok(Products_ListNew);

            }
            catch (Exception ex)
            {
                log.Error("Get_Consignments_ListOnline-----Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }
        //================================== Get Dammaged Invetory Product Consignments List==============
        [HttpPost]
        [Route("Get_Dammaged_Invetory_Product_Consignments_List")]
        public IHttpActionResult Get_Dammaged_Invetory_Product_Consignments_List(VMConsignments consignment)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Source_Unit_ID", Convert.ToInt32(consignment.Source_ID) },
                                                                                           { "@Dest_Unit_ID", Convert.ToInt32(consignment.Dest_ID) },
                                                                                           { "@ProductID", consignment.productid },
                                                                                           { "@Cons_ID", consignment.Consignment_ID },
                                                                                           { "@ConsignmentName", consignment.Cons_Name },
                                                                                            { "@Date_From", consignment.DateFrom },
                                                                                              { "@Date_To", consignment.DateTo },
                                                        };
                var Products_ListNew = _database.ProductListWithCount(Dammaged_Products_List, parameters);
                return Ok(Products_ListNew);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }


        //================================== Get Dammaged Invetory Product Consignments List==============
        [HttpPost]
        [Route("Get_Dammaged_Invetory_Product_Consignments_List_with_Date")]
        public IHttpActionResult Get_Dammaged_Invetory_Product_Consignments_List_with_Date(VMConsignments consignment)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Source_Unit_ID", Convert.ToInt32(consignment.Source_UnitID) },
                                                                                           { "@Dest_Unit_ID", Convert.ToInt32(consignment.Dest_UnitID) },
                                                                                           { "@ProductID", consignment.productid },
                                                                                           { "@Cons_ID", consignment.Consignment_ID },
                                                                                           { "@ConsignmentName", consignment.Cons_Name },
                                                                                           { "@CreatedDate", consignment.CreateDate },
                };
                var Products_ListNew = _database.ProductListWithCount(Dammaged_Products_List_Date, parameters);
                return Ok(Products_ListNew);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }


        //================================== Method To Get Dammaged Invetory Product List Consignments List==============
        [HttpPost]
        [Route("Get_Dammaged_Invetory_Product_List_Consignments_List")]
        public IHttpActionResult Get_Dammaged_Invetory_Product_List_Consignments_List(VMConsignments consignment)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Cons_ID", consignment.Consignment_ID },
                                                                                           { "@Source_Unit_ID", Convert.ToInt32(consignment.Source_UnitID) },
                                                                                           { "@Dest_Unit_ID", Convert.ToInt32(consignment.Dest_UnitID) },
                                                                                           { "@ProductID", consignment.productid },
                                                                                           };
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

        //================================== Get Dammaged Invetory Product==============
        [HttpPost]
        [Route("Get_Dammaged_Invetory_Product")]
        public IHttpActionResult Get_Dammaged_Invetory_Product(VMConsignments consignment)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Con_ID", consignment.Consignment_ID },
                                                                                           { "@ProductID", consignment.productid },
                                                                                           { "@Status", consignment.Cons_Status },
                                                                                           };
                var result = _database.ProductListWithCount(Dammaged_Inventory_Product, parameters);
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

        //================================== Get Assigned Inventory Product Ids==============
        [HttpGet]
        [Route("Get_Assigned_Inventory_Product_Ids")]
        public IHttpActionResult Get_Assigned_Inventory_Product_Ids(int Source_UnitID, int Dest_UnitID, int Product_Id, int OrderID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Source_Unit_ID", Source_UnitID },
                                                                                          { "@Dest_Unit_ID", Dest_UnitID },
                                                                                          { "@ProductID",Product_Id },
                                                                                          { "@OrderID", OrderID } };
                var result = _database.ProductListWithCount(Get_Inventory_Products_List, parameters);
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

        //================================== Create New Consignment==============
        [HttpPost]
        [Route("Create_New_Consignment")]
        public IHttpActionResult Create_New_Consignment(VMConsignments vm)
        {
            try
            {
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Con_ID", vm.ID },
                                                                                          { "@Inventory_Products_IDs", vm.Productids },
                                                                                          { "@Source_Unit_ID",vm.Source_UnitID },
                                                                                          { "@Destination_Unit_ID", vm.Dest_UnitID },
                                                                                          { "@Number_Of_Boxes", vm.NoOfBoxes } ,
                                                                                          { "@Others", vm.Others},
                                                                                          { "@UserID", User_Id } };
                var result = _database.ProductListWithCount(Get_Inventory_new_Consignment_List, parameters);
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
        [Route("Create_New_ConsignmentForOnline")]
        public IHttpActionResult Create_New_ConsignmentForOnline(VMConsignments vm)
        {
            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                List<IHubUsageController> persons = js.Deserialize<List<IHubUsageController>>(vm.Productids);
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                string Get_Inventory_Products_List = "[iC_Create_New_Consignment_Business]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Con_ID", vm.ID },
                                                                                          { "@Inventory_Products_IDs", vm.Productids },
                                                                                          { "@Source_Unit_ID",vm.Source_UnitID },
                                                                                          { "@Destination_Unit_ID", vm.Dest_UnitID },
                                                                                          { "@Number_Of_Boxes", vm.Status } ,
                                                                                          { "@UserID", User_Id },
                                                                                            { "@DomainId",vm.DomainId} };
                var result = _database.ProductListWithCount(Get_Inventory_Products_List, parameters);
                foreach (var item in persons)
                {
                    var balnce = dbContext.iD_Orders_Main.Where(m => m.Order_Number == item.OrderID).ToList();
                    string commandText = "SELECT \"PhoneNumber\" FROM \"AspNetUsers\" WHERE \"Id\" = @id";
                    Dictionary<string, object> parameters1 = new Dictionary<string, object>() { { "@id", balnce[0].Buyer_Id } };
                    var PhoneNumber = _database.GetStrValue(commandText, parameters1);
                    Common.sendmessage(PhoneNumber, "Your order " + item.OrderID + " has been dispatched by iHub /third party logistics and will be delivered soon.", "TODO");
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
        //===================== Change Bilk Consignment Status =================
        [HttpGet]
        [Route("ChangeBulkConsignmentStatus")]
        public IHttpActionResult ChangeBulkConsignmentStatus(string ConsignmentIDs, int Status)
        {
            try
            {
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Cons_IDs", ConsignmentIDs }, { "@UserId", User_Id },
                                                                                          { "@Cons_Status", Status } };
                var result = _database.Query(Change_Bulk_Consignment_Status, parameters);
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

        //===================== Change Consignment Status =================
        [HttpGet]
        [Route("ChangeConsignmentStatus")]
        public IHttpActionResult ChangeConsignmentStatus(string ConsignmentIDs, int Status)
        {
            try
            {
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Cons_ID", ConsignmentIDs },
                                                                                                { "@UserId", User_Id },
                                                                                          { "@Cons_Status", Status } };
                var result = _database.Query(Change_Status, parameters);
                if (Status == 30)
                {
                    int iDomainID = 0; int iTotal = 0; int iReached = 1; string sPhoneNumber = string.Empty;
                    int iConsignID = Convert.ToInt32(ConsignmentIDs);
                    var Order_Number = dbContext.iHub_Inventory_Products.Where(x => x.Consignment_Id == iConsignID).Select(m => m.Order_Id).FirstOrDefault();
                    iReached = dbContext.iHub_Inventory_Products.Where(x => x.Consignment_Id == iConsignID).Count();
                    iTotal = dbContext.iHub_Inventory_Products.Where(x => x.Order_Id == Order_Number).Count();
                    Dictionary<string, object> Param = new Dictionary<string, object>() { { "@OrderNumber", Order_Number } };
                    var oCatList = _database.GetMultipleResultsListAll("iS_Get_Order_Details_By_Order_Number", Param);
                    if (oCatList != null)
                    {
                        if (oCatList.Addressset != null && oCatList.Addressset.Count() > 0)
                        {
                            iTotal = oCatList.Addressset.Count();
                            sPhoneNumber = oCatList.Addressset[0]["Mobile_Number"];
                        }
                        if (oCatList.Resultset != null && oCatList.Resultset.Count() > 0)
                        {
                            iDomainID = Convert.ToInt32(oCatList.Resultset[0]["Order_From"]);
                        }
                    }
                    string message = string.Empty;
                    message = iReached + " out of " + iTotal + " Qty from your OrderID:" + Order_Number + " got reached to Store.Please collect in Store.";
                    var oSMSD = dbContext.MailSMSSettings.Where(m => m.iDomainID == iDomainID).Where(m => m.sTemplateType == "SMS").Where(m => m.sType == "Delivered").FirstOrDefault();
                    if (oSMSD != null && oSMSD.SMS == true)
                    {
                        if (oSMSD.sBody != null && oSMSD.sBody != "" && oSMSD.sBody.Contains("{{"))
                        {
                            string sMsg = oSMSD.sBody;

                            sMsg = sMsg.Replace("{{P}}", iReached.ToString());

                            sMsg = sMsg.Replace("{{T}}", iTotal.ToString());

                            sMsg = sMsg.Replace("{{O}}", Order_Number.ToString());

                            message = sMsg;
                        }
                        Common.sendmessage(sPhoneNumber, message, oSMSD.sdltid);
                    }
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
        //===================== Change Consignment Status =================
        [HttpGet]
        [Route("ChangeConsignmentStatusForOnline")]
        public IHttpActionResult ChangeConsignmentStatusForOnline(string ConsignmentIDs, int Status)
        {
            try
            {
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                string Change_Status = "[iC_Change_Consignment_Status]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Cons_ID", ConsignmentIDs },
                                                                                                { "@UserId", User_Id },
                                                                                          { "@Cons_Status", Status } };
                var result = _database.Query(Change_Status, parameters);
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

        //=== Method For Get Product Location Details ===//
        [HttpGet]
        [Route("Get_Product_Location_Details")]
        public IHttpActionResult Get_Product_Location_Details(int OrderID, int ProductID, int Ordered_Product_Details_ID, int Consignment_Id)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@OrderID", OrderID }, { "@ProductID", ProductID }, { "@Ordered_Product_Details_ID", Ordered_Product_Details_ID }, { "@Consignment_Id", Consignment_Id } };
                var Tracking_Product = _database.ProductListWithCount(TrackingProduct, parameters);
                return Ok(Tracking_Product);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }

        //===================== GET Assigned Products List =================
        [HttpGet]
        [Route("ViewProductsInConsignment")]
        public IHttpActionResult ViewProductsInConsignment(int ConsignmentID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Con_ID", ConsignmentID } };
                var result = _database.GetMultipleResultsListAll(View_Consignment_List, parameters);
                return Ok(result);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        //==============================To Get Partially Received Consignment List===================================
        [HttpPost]
        [Route("Get_Partially_Received_DC_Consignments_List")]
        public IHttpActionResult Get_Partially_Received_DC_Consignments_List(VMConsignments consignment)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Dest_Unit_ID", Convert.ToInt32(consignment.Dest_UnitID) },
                                                                                           { "@Source_Unit_ID", Convert.ToInt32(consignment.Source_UnitID) },
                                                                                           { "@ProductID", consignment.productid },
                                                                                           { "@Cons_ID", consignment.Consignment_ID },
                                                                                           { "@ConsignmentName",consignment.Cons_Name },
                                                                                           { "@CreatedDate", consignment.CreateDate}
                                                                                           };
                var result = _database.ProductListWithCount(To_Get_Partially_Received_List, parameters);
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

        //=============================================Update Dammaged Product=========================================
        [HttpPost]
        [Route("Update_Dammaged_Product_to_Store_Stock")]
        public IHttpActionResult Update_Dammaged_Product_to_Store_Stock(VMConsignments consignment)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Invt_Prd_ID", consignment.Inv_Ids }
                                                                                           };
                var result = _database.Query(Update_Dammaged_Product, parameters);
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

        //=========================================To Accept  Consignment===================================================
        [HttpPost]
        [Route("Accept_Consignment")]
        public IHttpActionResult Accept_Consignment(VMConsignments consignment)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Cons_ID",consignment.Consignment_ID },
                                                                                           { "@Cons_Status", 40 },
                                                                                           { "@Dammaged_Inv_Ids",consignment.Dammaged_Inv_Ids },
                                                                                   { "@Not_Recived_Inv_Ids", consignment.Not_Recived_Inv_Ids },
                                                                                           { "@UserID", userID } , { "@RemarksDeatils", consignment.RemarksDeatils }
                                                                                    , {"@Output_Updated" ,1 }
                };
                var result = _database.QueryValue(Get_Accept_Consignment_List, parameters);
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

        //================To Assign Inventory Products================================================================
        [HttpGet]
        [Route("Get_Assigned_Inventory_Product_Ids_Accepeted")]
        public IHttpActionResult Get_Assigned_Inventory_Product_Ids_Accepeted(int Source_UnitID, int Dest_UnitID, int Product_Id, int OrderID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Source_Unit_ID", Source_UnitID },
                                                                                           { "@Dest_Unit_ID", Dest_UnitID },
                                                                                           { "@ProductID",Product_Id },
                                                                                           { "@OrderID", OrderID } };
                var result = _database.ProductListWithCount(Get_Assign_Inventory_Products_List, parameters);
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

        //============>>>> To View Consignment Details after in View All Orders <<<<===============
        [HttpGet]
        [Route("Get_More_Tracking_Details")]
        public IHttpActionResult Get_More_Tracking_Details(int ConsignmentID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Con_ID", ConsignmentID } };
                var More_Tracking_Details = _database.ProductListWithCount(More_Details, parameters);
                return Ok(More_Tracking_Details);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }

        //====================To Get Assigned Product List Print==================================
        [HttpPost]
        [Route("GetAssignedProductsListprint")]
        public IHttpActionResult GetAssignedProductsListprint(VMConsignments consignment)
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
                                                                                          { "@Page_Size", 0 },
                                                                                          { "@Page_Index",0 }
                                                                                          };
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

        //==================To Assigned Inventory Product Dammaged and Missing=============================
        [HttpGet]
        [Route("Get_Assigned_Inventory_Product_Ids_Damage_missing")]
        public IHttpActionResult Get_Assigned_Inventory_Product_Ids_Damage_missing(int Source_UnitID, int Dest_UnitID, int Product_Id, int OrderID, string OrderType)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Source_Unit_ID", Source_UnitID },
                                                                                           { "@Dest_Unit_ID", Dest_UnitID },
                                                                                           { "@ProductID",Product_Id },
                                                                                           { "@OrderID", OrderID },
                    {"@OrderType",OrderType }};
                var result = _database.ProductListWithCount(Get_Inventory_Products_List_dammaged, parameters);
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
        [Route("GetUniqueSerialNumberPrintlist")]
        public IHttpActionResult GetUniqueSerialNumberPrintlist(int OID, int PID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@OrderNumber", OID }, { "@Product_Id", PID } };
                var result = _database.ProductListWithCount(Get_Assigned_StockUnits, parameters);
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

        //=======================To Before Accept Consignment===============================
        [HttpPost]
        [Route("Before_Accept_Consignment")]
        public IHttpActionResult Before_Accept_Consignment(VMConsignments consignment)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Cons_ID",consignment.Consignment_ID },
                                                                                           { "@Cons_Status", 40 },
                                                                                           { "@Dammaged_Inv_Ids",consignment.Dammaged_Inv_Ids },
                                                                                           { "@Not_Recived_Inv_Ids", consignment.Not_Recived_Inv_Ids },
                                                                                           { "@UserID", userID } ,
                                                                                            { "@RemarksDeatils", consignment.RemarksDeatils },
                                                                                            { "@DCUnitId", consignment.DC_Unit_ID },
                                                                                            { "@UnitId", consignment.Unit_ID },
                                                                                            { "@Productid", consignment.productid },
                                                                                            { "@Orderid", consignment.OrderID },
                                                                                            {"@OrderType",consignment.OrderType }
                };
                var result = _database.QueryValue(Get_Inventory_Products_List_missing, parameters);
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

        //======================================Assigned Inventory Product Dammaged==================
        [HttpGet]
        [Route("Get_Assigned_Inventory_Product_Ids_Dammaged")]
        public IHttpActionResult Get_Assigned_Inventory_Product_Ids_Dammaged(int Source_UnitID, int Dest_UnitID, int Product_Id, int OrderID, string Status, string Damages)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Source_Unit_ID", Source_UnitID },
                                                                                          { "@Dest_Unit_ID", Dest_UnitID },
                                                                                          { "@ProductID",Product_Id },
                                                                                          { "@OrderID", OrderID },
                                                                                          { "@Status", Status},
                    {"@Damages",Damages } };
                var result = _database.ProductListWithCount(Get_Inventory_Products_missing, parameters);
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

        //==============Assigned InventoryProduct To DC====================================================
        [HttpPost]
        [Route("Get_Assigned_Inventory_Product_Ids_To_Dc")]
        public IHttpActionResult Get_Assigned_Inventory_Product_Ids_To_Dc(VMConsignments consignment)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                                                                                          { "@Dammaged_Inv_Ids", consignment.Inv_Ids },
                                                                                           { "@UserID", userID },
                                                                                          {"@DCUnitId", consignment.DC_Unit_ID  },
                                                                                           {"@UnitId", consignment.Unit_ID},
                                                                                           {"@Productid",consignment.productid },
                                                                                            {"@OrderType",consignment.OrderType }
                                                                                          };
                var result = _database.QueryValue(Assigned_Inventory_Product, parameters);
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
        [Route("GetUniqueSerialNumber")]
        public IHttpActionResult GetUniqueSerialNumber(int OID, int PID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@OrderNumber", OID }, { "@Product_Id", PID } };
                var result = _database.ProductListWithCount(Get_UniqueSerial_number, parameters);
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
        //===================== Created Consignments List=================
        [HttpPost]
        [Route("Get_Consignments_List_Invioce")]
        public IHttpActionResult Get_Consignments_List_Invioce(Consignments consignment)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Cons_Name", consignment.Consignment_Name },
                { "@Cons_Status", consignment.Consignment_Status },
                    { "@Source_ID", consignment.Source_Unit_Id },
                    { "@Dest_ID", consignment.Destination_Unit_Id },
                    { "@CreateDate", consignment.Created_Date },
                    { "@Page_Size", consignment.Pagesize },
                    { "@Page_Index", consignment.Pageindex }
                };
                var Products_ListNew = _database.ProductListWithCount(Products_List_Invioce, parameters);
                return Ok(Products_ListNew);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        //=================get partial received list==============
        [HttpPost]
        [Route("Get_Dammaged_Invetory_Product_Consignments_Partial_List")]
        public IHttpActionResult Get_Dammaged_Invetory_Product_Consignments_Partial_List(VMConsignments consignment)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Source_Unit_ID", Convert.ToInt32(consignment.Source_ID) },
                                                                                           { "@Dest_Unit_ID ", Convert.ToInt32(consignment.Dest_ID) },
                                                                                            { "@ConsignmentName",consignment.Cons_Name },
                                                                                            { "@Date_From", consignment.DateFrom },
                                                                                              { "@Date_To", consignment.DateTo }, };

                var result = _database.ProductListWithCount(Get_Partially_Received_List, parameters);
                return Ok(result);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        //==================To Assigned Inventory Product Dammaged and Missing=============================
        [HttpGet]
        [Route("Get_Assigned_Inventory_Product_Ids_Damage_List")]
        public IHttpActionResult Get_Assigned_Inventory_Product_Ids_Damage_List(int Source_UnitID, int Dest_UnitID, int Product_Id, int OrderID, string OrderType)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Source_Unit_ID", Source_UnitID },
                                                                                           { "@Dest_Unit_ID", Dest_UnitID },
                                                                                           { "@ProductID",Product_Id },
                                                                                           { "@OrderID", OrderID },
                    {"@OrderType",OrderType }};
                var result = _database.ProductListWithCount(Get_Inventory_Products_Damage_List, parameters);
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
       //==================To Assigned Inventory Product Dammaged and Missing=============================
       [HttpGet]
       [Route("Get_Assigned_Inventory_Product_Ids_Packed_List")]
        public IHttpActionResult Get_Assigned_Inventory_Product_Ids_Packed_List(int Source_UnitID, int Dest_UnitID, int Product_Id, int OrderID, string OrderType)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Source_Unit_ID", Source_UnitID },
                                                                                          { "@Dest_Unit_ID", Dest_UnitID },
                                                                                          { "@ProductID",Product_Id },
                                                                                          { "@OrderID", OrderID },
                   {"@OrderType",OrderType }};
                var result = _database.ProductListWithCount(Get_Inventory_Products_Packed_List, parameters);
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
        [Route("GetAssignedStockMandalsList_New")]
        public IHttpActionResult GetAssignedStockMandalsList_New(int Src_UnitID, int Dest_UnitID, int DomainID)
        {
            try
            {
                string Stock_Moving = "[iB_Get_Assigned_Stock_Mandal_List]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Source_Unit_ID", Src_UnitID },
                { "@Destination_Unit_ID", Dest_UnitID }, { "@DomainID", DomainID }
                };
                var Stock_Moving_ListNew = _database.ProductListWithCount(Stock_Moving, parameters);
                return Ok(Stock_Moving_ListNew);
            }
            catch (Exception ex)
            {
                log.Error("GetAssignedStockMandalsList_New-----Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        //===================== GET Assigned Products List =================
        [HttpPost]
        [Route("GetAssignedProductsListForOnline")]
        public IHttpActionResult GetAssignedProductsListForOnline(VMConsignments consignment)
        {
            try
            {//sp name changed iC_Get_Assigned_Products_List_By_Source_UnitID_Dest_Unit_ID_B2b
                string Consignment_Create = "[iC_Get_Assigned_Products_List_By_Source_UnitID_Dest_Unit_ID_B2b]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Unit_ID", consignment.Unit_ID }

                  , { "@DC_Unit_ID", consignment.DC_Unit_ID }
                    , { "@SKU", consignment.SKU }
                   , { "@OrderNumber", consignment.OrderNumber }
                    , { "@ProductName", consignment.ProductName }
                    , { "@MobileNumber", consignment.MobileNumber }
                    , { "@OrderType", consignment.OrderType }
                    , { "@OrderBy", consignment.OrderBy }
                    , { "@Page_Size", 10 }
                    , { "@Page_Index", 1 }
                     , { "@OrderFrom", consignment.DomainId}
                };
                var Consignment_Create_List = _database.ProductListWithCount(Consignment_Create, parameters);
                return Ok(Consignment_Create_List);

            }
            catch (Exception ex)
            {
                log.Error("GetAssignedProductsList-----Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        [HttpGet]
        [Route("GetOrderDetails_ByOrderId")]
        public IHttpActionResult GetOrderDetails_ByOrderId(int OrderId)
        {
            try
            {
                string Order_details = "iS_Get_Order_Details_By_Order_Number_thermalInvoice";
                // int userID = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@OrderNumber", OrderId } };
                var Orders = _database.GetMultipleResultsListAll(Order_details, parameters);
                foreach (var item in Orders.Addressset)
                {
                    if (Convert.ToInt32(item["Package_ID"]) != 0)
                    {
                        var id = Convert.ToInt32(item["Ordered_Product_Details_ID"]);
                        // var packlist = dbContext.UserBasket_Products.Where(x => x.Order_Details_ID == id).ToList();
                        var packlist = (from pc in dbContext.UserBasket_Products
                                        join ih in dbContext.iHub_Products on pc.Product_ID equals ih.iHub_Product_ID
                                        where pc.Order_Details_ID == id
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
                    //if (Orders.Resultset != null)
                    //{
                    //    var iOrderFrom = Orders.Resultset.FirstOrDefault().Where(m => m.Key == "Order_From").Select(m => m.Value).FirstOrDefault();
                    //    string Query = "SELECT * FROM Domains_Master_Configuration WHERE DomainID=" + iOrderFrom;
                    //    Dictionary<string, object> Param = new Dictionary<string, object>() { };
                    //    var res = _database.Query(Query, Param);
                    //    if (res != null)
                    //    {
                    //        Orders.Addressset4 = new List<Dictionary<string, string>>();
                    //        Orders.Addressset4 = Orders.Addressset3;
                    //        Orders.Addressset3 = res;
                    //        //Send_MsgOrMail_To_Buyer(OrderId, Orders);
                    //    }
                    //}
                }
                return Ok(Orders);

                // return Ok(Orders);
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
        [Route("Get_Assigned_Logistics_Consignments_List")]
        public IHttpActionResult Get_Assigned_Logistics_Consignments_List(Consignments consignment)
        {
            try
            {

                string Products_List = "[iB_Get_Assigned_Logistics_Consignment_List]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Cons_Name", consignment.Consignment_Name },
                { "@Cons_Status", consignment.Consignment_Status },
                    { "@Source_ID", consignment.Source_Unit_Id },
                    { "@Dest_ID", consignment.Destination_Unit_Id },
                    { "@CreateDate", consignment.Created_Date },
                    { "@Page_Size", consignment.Pagesize },
                    { "@Page_Index", consignment.Pageindex },
                     { "@DomainID", consignment.DomainID }
                };
                var Products_ListNew = _database.ProductListWithCount(Products_List, parameters);
                return Ok(Products_ListNew);

            }
            catch (Exception ex)
            {
                log.Error("Get_Assigned_Logistics_Consignments_List-----Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        [HttpGet]
        [Route("ViewOrdersInConsignment")]
        public IHttpActionResult ViewOrdersInConsignment(int ConsignmentID)
        {
            try
            {
                string View_Consignment_List = "[iB_View_Created_Consignments_For_Order_Details]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Con_ID", ConsignmentID } };
                var result = _database.GetMultipleResultsListAll(View_Consignment_List, parameters);
                return Ok(result);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        [HttpPost]
        [Route("GetBuyerOTP")]
        public IHttpActionResult GetBuyerOTP(string PhoneNumber, string Purpose)
        {
            try
            {

                int userID = 0;
                if (Purpose == "ChangeStatus")
                {
                    userID = 0;
                }
                else
                {
                    userID = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());
                }
                string insert_otp = "[iAdmin_Save_Notification_Details]";
                string ReturnOTP = Common.GenerateOTP();
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@mobile_number", PhoneNumber }, { "@otp_generated", ReturnOTP }, { "@otp_purpose", Purpose }, { "@user_ID", userID } };
                var buyer_details = _database.QueryValue(insert_otp, parameters);
                Common.sendmessage(PhoneNumber, "Your Verification OTP is " + ReturnOTP, "1007161605656824097");
                //Common.sendmessage(PhoneNumber, "Please use this pin " + ReturnOTP + " to receive the delivery", "TODO");
                return Ok(buyer_details);
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
        [Route("ChangeAssignedConsignmentStatus")]
        public IHttpActionResult ChangeAssignedConsignmentStatus(VMPrdctDetails vm)
        {
            try
            {
                VMPagingResultsPost vmodal = new VMPagingResultsPost();
                vmodal.ProductDetails = new List<Dictionary<string, string>>();
                string sProductName = string.Empty;
                int orderID = Convert.ToInt32(vm.Order_ID);
                string Change_Status = "[iB_Change_Consignment_Order_Status]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Order_ID", vm.Order_ID },
                                                                                                  { "@Cons_Status", vm.constatus },
                        {"@Consignment_ID",vm.ConsignmentIDs },{"@Tax_Invoice_Number",vm.Tax_Invoice_Number},{"@Mobile_Number",vm.Mobile_Number}
                                                                                          };
                var result = _database.Query(Change_Status, parameters);
                if (vm.constatus == 50)
                {
                    int iDomainID = 0; int iTotal = 0; int iReached = 0; string sPhoneNumber = string.Empty;
                    int iConsignID = Convert.ToInt32(vm.ConsignmentIDs);
                    var Order_Number = dbContext.iHub_Inventory_Products.Where(x => x.Consignment_Id == iConsignID).Select(m => m.Order_Id).FirstOrDefault();
                    //iReached = dbContext.iHub_Product_Ordered_Locations.Where(x => x.OrderNumber == Order_Number).Count();
                    iTotal = dbContext.iHub_Inventory_Products.Where(x => x.Order_Id == Order_Number).Count();
                    Dictionary<string, object> Param = new Dictionary<string, object>() { { "@OrderNumber", Order_Number },{ "@Consignment_ID", vm.ConsignmentIDs } };
                    var oCatList = _database.GetMultipleResultsListAll("iS_Get_Order_Details_By_Order_Number_And_Consigment", Param);
                    if (oCatList != null)
                    {
                        iReached = oCatList.Addressset.Count();
                        if (oCatList.Addressset != null && oCatList.Addressset.Count() > 0)
                        {
                            iTotal = oCatList.Addressset.Count();
                            sPhoneNumber = oCatList.Addressset[0]["Mobile_Number"];
                            
                            foreach (var sPro in oCatList.Addressset)
                            {
                                var iProID = Convert.ToInt32(sPro.ToList().Where(m => m.Key == "Product_Id").Select(m => m.Value).FirstOrDefault());
                                
                                    sProductName = sPro.ToList().Where(m => m.Key == "Product_Name").Select(m => m.Value).FirstOrDefault();
                                    vmodal.ProductDetails.Add(sPro);
                                    if (vmodal.ProductName == null)
                                    {
                                        vmodal.ProductName = sProductName;
                                    }
                                    else
                                    {
                                        vmodal.ProductName = vmodal.ProductName + ',' + sProductName;
                                    }
                                
                            }
                        }
                        if (oCatList.Resultset != null && oCatList.Resultset.Count() > 0)
                        {
                            iDomainID = Convert.ToInt32(oCatList.Resultset[0]["Order_From"]);
                            vmodal.OrderDate = oCatList.Resultset[0]["Order_Date"];
                        }
                    }
                    
                    
                    string message = string.Empty;
                    string sEmail = string.Empty;
                    MailSMSSettings oMailSMS = new MailSMSSettings();
                    MailSMSSettings oMailEmail = new MailSMSSettings();
               
                        message = iReached + " out of " + iTotal + " products from your Order " + Order_Number + " is successfully delivered today. Thank you for shopping & visit again.";
                        oMailSMS = dbContext.MailSMSSettings.Where(m => m.iDomainID == iDomainID).Where(m => m.sTemplateType == "SMS").Where(m => m.sType == "Delivered").FirstOrDefault();

                        sEmail = iReached + " out of " + iTotal + " products from your Order " + Order_Number + " has been delivered. " +
                        "Thank you for ordering at Ihub Grocer.Please order again.";
                        oMailEmail = dbContext.MailSMSSettings.Where(m => m.iDomainID == iDomainID).Where(m => m.sTemplateType == "MAIL").Where(m => m.sType == "Delivered").FirstOrDefault();
                   



                    var oSMSD = dbContext.MailSMSSettings.Where(m => m.iDomainID == iDomainID).Where(m => m.sTemplateType == "SMS").Where(m => m.sType == "Delivered").FirstOrDefault();
                    if (oSMSD != null && oSMSD.SMS == true)
                    {
                        if (oMailSMS.sBody != null && oMailSMS.sBody != "" && oMailSMS.sBody.Contains("{{"))
                        {
                            string sMsg = oMailSMS.sBody;

                            sMsg = sMsg.Replace("{{P}}", iReached.ToString());

                            sMsg = sMsg.Replace("{{T}}", iTotal.ToString());

                            sMsg = sMsg.Replace("{{O}}", Order_Number.ToString());

                            message = sMsg;
                        }
                        Common.sendmessage(sPhoneNumber, message, oMailSMS.sdltid);
                    }
                    ProductsAndPriceController oPPC = new ProductsAndPriceController();
                    var Result = oPPC.GetOrderDetails_ByOrderId(Convert.ToInt32(Order_Number));
                    var oAddress = ((System.Web.Http.Results.OkNegotiatedContentResult<iHubAdminAPI.Models.VMDataTableResponse>)Result).Content;
                    if (oAddress.Addressset != null && oAddress.Addressset.Count() > 0)
                    {
                        VMAddress add = new VMAddress();
                        var ress = Result as OkNegotiatedContentResult<VMDataTableResponse>;
                        var results = ress.Content;
                        var buyeraddress = results.Addressset[0]["AddressJson"];
                        var name = results.Addressset[0]["Contact_Person_Name"];
                        var Number = results.Addressset[0]["Alternative_Mobile_Number"];
                        var Pin_Code = results.Addressset[0]["Pin_Code"];
                        var Email = results.Addressset[0]["Email"];
                        add.Address_Line_One = buyeraddress;
                        vmodal.EmailID = Email;
                        add.FullName = name;
                        add.MobileNumber = Number;
                        add.Pincode = Pin_Code;
                        vmodal.buyeraddress = add;
                        vmodal.Name = name;
                    }
                    vmodal.Status = 50;
                    vmodal.OrderID = Order_Number;
                    vmodal.EmailMessage = sEmail;
                    var DomainDetails = dbContext.iHub_Domains.Where(x => x.Site_ID == iDomainID).FirstOrDefault();
                    vmodal.DomainName = DomainDetails.sName;
                    vmodal.Subject = "Order Status";
                    var oMasterD = dbContext.DomainsMasterData.Where(m => m.DomainID == iDomainID).ToList().FirstOrDefault();
                    if (oMasterD != null)
                    {
                        var sImgpath = System.Configuration.ConfigurationManager.AppSettings["ImgPath"].ToString();
                        vmodal.ImagePath = sImgpath + iDomainID + "_Logo" + "/" + oMasterD.sLogo;
                        vmodal.LogoTitle = oMasterD.sLogoTagLine;
                    }
                    if (oMailEmail != null && oMailEmail.sSubject != null && oMailEmail.sSubject != "")
                    {
                        vmodal.Subject = oMailEmail.sSubject;
                        if (oMailEmail.sTrackhere != null && oMailEmail.sTrackhere != "")
                        {
                            vmodal.TrackDetails = oMailEmail.sTrackhere;
                        }
                        if (oMailEmail.sEmail != null && oMailEmail.sEmail != "")
                        {
                            vmodal.sEmail = oMailEmail.sEmail;
                        }
                    }
                    if (oMailEmail != null && oMailEmail.sMobileNumber != null && oMailEmail.sMobileNumber != "")
                    {
                        vmodal.MobileNumber = oMailEmail.sMobileNumber;
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
                    if (vmodal.EmailID != "" && vmodal.EmailID != null)
                    {
                        if (oMailEmail != null && oMailEmail.MAIL == true)
                        {
                            //Common.sendemail(vmodal.EmailID, sEmail);
                            Mailer.ChangeBuyerOrderStatus(vmodal, vmodal.EmailID).Send();
                        }
                    }
                }
                //var comment = Common.sendmessage(PhoneNumber, "Dear customer,Your iHub B2B order " + balnce.Order_Number + " was delivered successfully,Thank you for shopping with us", "TODO");

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
        [Route("PackedOrdersPickupList")]
        public IHttpActionResult PackedOrdersPickupList(VMConsignments consignment)
        {
            try
            {
                string Packed_Orders_Pickup_List = "[iB_Packed_Orders_Pickup_List]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Unit_ID", consignment.Unit_ID }

                  , { "@DC_Unit_ID", consignment.DC_Unit_ID }
                    , { "@SKU", consignment.SKU }
                   , { "@OrderNumber", consignment.OrderNumber }
                    , { "@ProductName", consignment.ProductName }
                    , { "@MobileNumber", consignment.MobileNumber }
                    , { "@OrderType", consignment.OrderType }
                    , { "@OrderBy", consignment.OrderBy }
                    , { "@Page_Size", 10 }
                    , { "@Page_Index", 1 }

                };
                var Consignment_Create_List = _database.ProductListWithCount(Packed_Orders_Pickup_List, parameters);
                return Ok(Consignment_Create_List);

            }
            catch (Exception ex)
            {
                log.Error("PackedOrdersPickupList-----Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        #endregion "END of -- Consignments Management"
    }
}
