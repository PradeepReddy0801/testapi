using log4net;
using Newtonsoft.Json;
using System;
using Excel;
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
using System.Web.Script.Serialization;
using System.Net;
using System.Net.Sockets;
using System.Web.Http.Results;

using iHubAdminAPI.Models.ProductAndPrice;
using iHubAdminAPI.Mailer;

namespace iHubAdminAPI.Controllers
{
    //================================[Authorize]=============================================
    [RoutePrefix("api/InventoryAndStock")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class InventoryManagementController : ApiController
    {
        string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private static readonly ILog log = LogManager.GetLogger(typeof(InventoryManagementController).FullName);
        public SQLDatabase _database;

        iHubDBContext dbContext = new iHubDBContext();
        IUsermailer Mailer = new Usermailer();

        public InventoryManagementController()
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
        string Get_FMCG_Stock = "[iAdmin_Get_New_Inbunded_FMCG_Stock]";
        string FMCG_Brands_List = "[iAdmin_Get_All_Brands_By_CatID]";
        string Inventory_Date_history = "[iAdmin_Get_Inventory_History]";
        string Inventory_Products = "[iAdmin_Get_Inventory_Products]";       
        string Product_List_BY_Sku = "[iAdmin_Get_Products_List_By_Skus]";
        string Units_Data = "[iAdmin_Get_Unit_List_With_UnitLevel]";
        string product_GetList_New = "[iAdmin_Create_New_Inventory_Products]";
        string Reference_Number = "[iAdmin_Get_Inventory_Products_By_Reference_Number]";
        string Stock_Moving = "[iAdmin_Get_Stock_Moving_Units_List]";
        string Stock_Assigned_list = "[iAdmin_Get_Stock_Assigned_Units_List]";
        string Products_List = "[iAdmin_Stock_By_UnitID]";
        string assign_Stock_To_Unit_new = "[iAdmin_Assign_Stock_DC_To_W_S_DC]";
        string Products_Stock = "[iAdmin_Get_Stock_By_Category]";
        string Products_Avilable = "[iAdmin_Get_Avaibale_Inventory_Products]";
        string Get_Available_Stock = "[iAdmin_Get_AvailableStock_By_Category]";
        string Stock_Moving_Products = "[iAdmin_Get_StockMoving_Products_By_UnitID]";
        string Assign_Stock = "[iAdmin_Assign_Stock_DC_To_W_S_DC]";
        string Get_Cat_List = "[iAdmin_GetCategoriesList]";
        string Get_Leaf_Cat_List = "[iAdmin_GetLeafCategoriesList_Assigned_Products]";
        string Get_CategoryAttributes = "[iAdmin_Get_CategoryAttributes]";
        string Get_Un_Packed = "[iAdmin_Get_Un_Packed_Product_List_By_Unit_ID]";
        string Get_Un_Order_Packed = "[iAdmin_Get_Un_Packed_Order_List_By_Unit_ID]";
        string Update_To_Packing = "[iAdmin_Update_Inventory_Product_Status_To_Packed]";
        string assign_to_Multiple = "[iAdmin_Assign_Stock_To_Multiple_Locations]";
        string GetIntraStoreStock = "[iAdmin_Get_IntraStore_For_Admin]";
        string sourceIntraStores = "[iAdmin_Get_InOutflow_UnitsList_For_Selected_Unit]";
        string destinationIntraStores = "[iAdmin_Get_InOutflow_UnitsList_For_Selected_Unit]";
        string Get_All_Vendors = "[iAdmin_Get_All_Vendor_Orders_To_InBound]";
        string Get_InventoryIds = "[iAdmin_Get_InventoryIDs_To_InBound]";
        string Get_InventoryIds_Packed = "[iAdmin_Get_InventoryIDs_To_Packed]";
        string InBoud_Vendor_Products = "[iAdmin_InBoud_Vendor_Products]";
        string WarehouseInflowOutflow = "[iAdmin_Get_Warehouse_Inflow_Outflow_Admin]";
        string Get_Wh_Stock = "[iAdmin_Get_WareHouse_Products]";
        string destStores = "[iAdmin_Get_InOutflow_Destination_Stores]";
        string Products_Not_In_MegaDeals = "[iAdmin_Get_Products_Not_In_MegaDeals]";
        string Admin_Dashboard = "[iAdmin_Get_Data_For_Admin_Dashboard]";
        string commandText = "[iAdmin_Get_MegaDeals_Parent_CatIds]";
        string MegaDeals_Filters = "[iAdmin_MegaDeal_Products_By_Filters]";
        string Chq_Order_Status = "[iAdmin_Update_Cheque_Counter_File_Status]";
        string insert_MegaDeals = "[iAdmin_Insert_Products_Into_MegaDeals]";
        string Get_Inventory_Cat_List = "[iAdmin_Inventory_GetCategoriesList]";
        string DC_WarehouseInflowOutflow = "[iAdmin_Get_DC_Warehouse_Inflow_Outflow_Admin]";
        string Get_All_Products = "[iAdmin_Get_All_Products_List_By_Skus]";
        string WarehouseIntra = "[iAdmin_Get_IntraStore_For_WareHouse_Admin]";
        string Get_Dammaged_Missing = "[iAdmin_Get_Shrinkage_Invetory_Product_List]";
        string Get_Consignment_Units = "[iS_Get_Consignment_Unit_List_ByUnitID]";
        string schemes = "[iAdmin_Insert_Products_Into_Schemes]";
        string Get_Cat_List_Outbound = "[iAdmin_GetCategoriesList_StoreOrDCOrders_App]";
        string map_IMEI_againstProduct = "[SP_Map_ProductIMEI]";
        string map_Barcode_againstProduct = "[iAdmin_InOutBarcodeMapping]";
        string Get_Packed_List = "[iAdmin_Get_Packed_Product_List_By_Unit_ID]";
        string Update_MulProduct_Status = "[iAdmin_Update_Warehouse_and_vendor_Status_Direct]";
        string Reorderlevel = "[iAdmin_Get_ReOrderLevel_Report]";
        string Update_FMCG = "[iAdmin_Update_FMCG_Data]";
        string testUpdateStockCheckDamage = "[iAdmin_Shrinkage_Stock_Allocate_And_Update]";
        string Inventory_Products_Shrinkage = "[iAdmin_Get_Inventory_Products_Shrinkage_Stock]";
        string Get_InventoryIds_Shrinkage = "[iAdmin_Get_InventoryIDs_To_Shrinkage]";
        string UpdateShrinkageStock_SP = "[iAdmin_Shrinkage_Stock]";
        string Get_InventoryIds_Shrinkage_Approvale = "[iAdmin_Get_InventoryIDs_To_Shrinkage__Approvale]";
        string SpiltApproval = "[iAdmin_Order_Product_Details_Spilt_Approval]";
        string Assign_Logistics = "[iAdmin_Save_Assign_Logistics]";
        string string_MapConsignment_IMEI = "[iAdmin_Consignment_Map_ProductIMEI]";

        #endregion "END of -- SP Names"

        #region "BEGIN of -- Select Tables"

        string Get_WhId = "SELECT \"WareHouse_ID\" FROM \"iHub_Units_DC_WH_ST\" WHERE \"Unit_Hierarchy_Level\"=2 AND \"iHub_Unit_ID\"=";


        #endregion "END of -- Select Tables"

        #region "BEGIN of -- InventoryAndStock Management"


        //===================Get Inventory Stock======================//
        [HttpPost]
        [Route("Get_Inventory_Stock")]
        public IHttpActionResult Get_Inventory_Stock(VMModelsForInventory Cat)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ProductName", Cat.ProductName }
                   , { "@SKU", Cat.SKU }
                   , { "@Dc_Id", Cat.Dc_Unit_ID }
                   , { "@Order_Date_From", Cat.OrderDateFrom}
                   , { "@Order_Date_To", Cat.OrderDateTo}
                   , { "@MainCatID", Cat.TopCategoryID}
                   , { "@Category_Id", Cat.CategoryID}
                   , { "@Page_Size", Cat.Pagesize }
                   , { "@Page_Index", Cat.PageIndex }
                     , { "@Status", Cat.Status }
               };
                var Stock_Moving_ListNew = _database.Query(Inventory_Products, parameters);
                return Ok(Stock_Moving_ListNew);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

      
        [HttpPost]
        [Route("Get_RTS_Due_Payments")]
        public IHttpActionResult Get_RTS_Due_Payments()
        {
            try
            {
                return Ok();

            }
            catch (Exception ex)
            {
                log.Error("Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
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
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ProductIDs", Cat.FilterJson },{ "@unitid", Cat.Unit_ID }
            };
                var Stock_Moving_ListNew = _database.ProductListWithCount(Product_List_BY_Sku, parameters);
                return Ok(Stock_Moving_ListNew);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        //================================== Method To Get Units Data ByHierarchy==============
        [HttpPost]
        [Route("Get_Units_Data_ByHierarchy")]
        public IHttpActionResult Get_Units_Data_ByHierarchy(VMModelsForInventory Units)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@unit_type", Units.Unit_Type }, {"@parent_id", Units.ParentID }
            };
                var Stock_Moving_ListNew = _database.ProductListWithCount(Units_Data, parameters);
                return Ok(Stock_Moving_ListNew);
            }
            catch (Exception ex)
            {
                log.Error("Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);

                //throw Ex;
            }
        }

        //===================== Get Consignments Units=================
        [HttpGet]
        [Route("Get_Consignments_Units_List")]
        public IHttpActionResult Get_Consignments_Units_List(int unitid)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@unitid", unitid } };
                var result = _database.ProductListWithCount(Get_Consignment_Units, parameters);
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

        //=================To Assign Inbound Stock=====================================================================
        [HttpPost]
        [Route("Assign_Inbound_Stock")]
        public IHttpActionResult GetProductListWithExcel(VMModelsForInventory Cat)
        {
            try
            {
                string reference_num = Common.GenerateOTP();
                int userID = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());
                int RF_Number = Convert.ToInt32(reference_num);
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ProductIds_Qty", Cat.FilterJson },
                                                                                           { "@Dc_Unit_ID", Cat.Unit_ID },
                                                                                           { "@User_ID", userID },
                                                                                           { "@IP", "465" },
                                                                                           { "@RF_Number", RF_Number},
                                                                                           {"@Vpo_id",Cat.Vpo_id },
                                                                                           {"@DeliveryChallNo",Cat.DeliveryChallNo },
                                                                                           {"@InvoiceNumber",Cat.InvoiceNumber },
                                                                                           {"@TotalQtyAssign",Cat.TotalQtyAssign }};
                var product_Get_List = _database.QueryValuenew(product_GetList_New, parameters);
                Dictionary<string, object> parameters2 = new Dictionary<string, object>() { { "@randomnumber", reference_num.ToString() } };
                var response2 = _database.ProductListWithCount(Reference_Number, parameters2);
                return Ok(response2);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("---Error---------", ex);
                return Ok(cs);
            }
        }

        //==================================Get Stock moving List===========================================
        [HttpGet]
        [Route("GetStockMovingSourceList")]
        public IHttpActionResult GetStockMovingSourceList(int HierarchyLevel, string Type, string RoleName, int SourceUnitID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Hierarchy_Level", HierarchyLevel },
                { "@Unit_Type", Type },{ "@Role_Name", RoleName },{ "@Source_Unit_ID", SourceUnitID }
                };
                var Stock_Moving_ListNew = _database.ProductListWithCount(Stock_Moving, parameters);
                return Ok(Stock_Moving_ListNew);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        //================================== Method To Get Stock Assigned SourceList==============
        [HttpGet]
        [Route("GetStockAssignedSourceList")]
        public IHttpActionResult GetStockAssignedSourceList(int HierarchyLevel, int Previous_UnitID, string Type)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Hierarchy_Level", HierarchyLevel },
                { "@UnitID", Previous_UnitID },{ "@Unit_Type", Type }
                };
                var Stock_Moving_ListNew = _database.ProductListWithCount(Stock_Assigned_list, parameters);
                return Ok(Stock_Moving_ListNew);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        //================================== Method To Get Stock By UnitID==============
        [HttpGet]
        [Route("Get_Stock_By_UnitID")]
        public IHttpActionResult Get_Stock_By_UnitID(int heirarchyID, int unitID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@unit_heirarchy_level", heirarchyID },
                { "@unitid", unitID }
                };
                var Products_ListNew = _database.ProductListWithCount(Products_List, parameters);
                return Ok(Products_ListNew);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        //====================================To Assign_Stock_ToUnit===============================================
        [HttpPost]
        [Route("Assign_Stock_ToUnit")]
        public IHttpActionResult Assign_Stock_ToUnit(VMModelsForInventory vmodel)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Product_IDs_Qty", vmodel.FilterJson },
                                                                                           { "@DC_Unit_ID", vmodel.Dc_Unit_ID },
                                                                                           { "@Dest_Unitid", vmodel.Unit_ID },
                };
                var assign_Stock_To_Unit = _database.Query(assign_Stock_To_Unit_new, parameters);
                return Ok(assign_Stock_To_Unit);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        //================================== Method To Get Products Stock By UnitID==============
        [HttpPost]
        [Route("Get_Products_Stock_By_UnitID")]
        public IHttpActionResult Get_Products_Stock_By_UnitID(VMModelsForInventory Cat)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@unitid", Cat.Unit_ID },
                { "@categoryid", Cat.CategoryID },
                { "@productname", Cat.ProductName },
                { "@sku", Convert.ToInt32(Cat.SKU) },
                { "@Page_Size", Cat.Pagesize },
                { "@Page_Index", Cat.PageIndex }
                };
                var Products_ListNew = _database.ProductListWithCount(Products_Stock, parameters);
                return Ok(Products_ListNew);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        //================================== Method To Get GetAvailable products By UnitID==============
        [HttpPost]
        [Route("GetAvailable_products_By_UnitID")]
        public IHttpActionResult GetAvailable_products_By_UnitID(VMModelsForInventory Cat)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Filter_Json", Cat.FilterJson },
                { "@Category_ID", Cat.CategoryID },
                { "@ProductName", Cat.ProductName },
                { "@Unit_ID", Cat.Unit_ID },
                { "@Page_Size", Cat.Pagesize },
                { "@Page_Index", Cat.PageIndex }
                };
                var Products_ListNew = _database.ProductListWithCount(Products_Avilable, parameters);
                return Ok(Products_ListNew);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        //====================================================To Get available Products Stock  by unitid
        [HttpPost]
        [Route("Get_Available_Products_By_UnitID")]
        public IHttpActionResult Get_Available_Products_By_UnitID(VMModelsForInventory vmodel)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@unitid", vmodel.Unit_ID },
                                                                                           { "@categoryid", vmodel.CategoryID },
                                                                                           { "@productname", vmodel.ProductName },
                                                                                           { "@sku", Convert.ToInt32(vmodel.SKU) },
                                                                                           { "@brand", vmodel.BrandName },
                                                                                           { "@Page_Size", vmodel.Pagesize },
                                                                                           { "@Page_Index", vmodel.PageIndex }
                };
                var Available_Stock = _database.ProductListWithCount(Get_Available_Stock, parameters);
                return Ok(Available_Stock);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        //=============================================To Get Stock Moving Product List=======================================
        [HttpPost]
        [Route("GetStockMovingProductsList")]
        public IHttpActionResult GetStockMovingProductsList(VMModelsForInventory Cat)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@UnitID", Cat.Unit_ID },
                { "@CategoryID", Cat.CategoryID },
                { "@ProductName", Cat.ProductName },
                { "@SKU", Cat.SKU },
                { "@Page_Size", Cat.Pagesize },
                { "@Page_Index", Cat.PageIndex }
                };
                var Products_ListNew = _database.ProductListWithCount(Stock_Moving_Products, parameters);
                return Ok(Products_ListNew);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        //================================== Method To Get StockMoving==============
        [HttpGet]
        [Route("StockMoving")]
        public IHttpActionResult StockMoving(int SourceID, int DestinationID, string ProductsWithQty)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Product_IDs_Qty", ProductsWithQty },
                { "@DC_Unit_ID", SourceID },
                { "@Dest_Unitid", DestinationID },
                };
                var Products_ListNew = _database.ProductListWithCount(Assign_Stock, parameters);
                return Ok(Products_ListNew);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        //================================== Method To Get ALL Categories List Based On Orders==============
        [HttpPost]
        [Route("GetCategoriesList")]
        public IHttpActionResult GetCategoriesList(VMModelsForInventory vmodel)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Unit_ID", vmodel.Unit_ID }
                    , { "@DC_Unit_ID", vmodel.Dc_Unit_ID } };
                var ProductsList = _database.ProductListWithCount(Get_Cat_List, parameters);
                return Ok(ProductsList);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }

        //================================== Method To Get ALL leaf Categories List Based On Orders==============
        [HttpPost]
        [Route("GetLeafCategoriesList")]
        public IHttpActionResult GetLeafCategoriesList(VMModelsForInventory vmodel)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@unitid", vmodel.Unit_ID }
                    , { "@previousunitid", vmodel.Dc_Unit_ID }, { "@parentcatid", vmodel.ParentID} };
                var ProductsList = _database.ProductListWithCount(Get_Leaf_Cat_List, parameters);
                return Ok(ProductsList);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error-GetLeafCategoriesList--------", ex);
                return Ok(cs);
            }
        }
        //==================================To Get Category Attributes====================================================
        [HttpGet]
        [Route("GetCategoryAttributes")]
        public IHttpActionResult GetCategoryAttributes(int CategoryID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@categoryid", CategoryID } };
                var res = _database.Query(Get_CategoryAttributes, parameters).FirstOrDefault();
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

        //================================To Get UnPacked Product List==============================================================
        [HttpPost]
        [Route("Get_Un_Packed_Product_List_By_Unit_ID")]
        public IHttpActionResult Get_Un_Packed_Product_List_By_Unit_ID(VMPagingResultsPost gt)
        {
            try
            {
                int User_ID = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                     { "@Source_UnitID", gt.Source_UnitID }
                    ,{ "@Dest_Unit_ID", gt.Dest_UnitID }
                    ,{ "@OrderID",gt.OrderID}
                    ,{ "@TopCategoryID", gt.TopCategory_ID}
                    ,{ "@SKU",gt.SKU}
                    ,{ "@ProductName", gt.ProductName }
                    ,{ "@CategoryID", gt.CategoryID}
                    ,{ "@OrderDateFrom", gt.orderdatefrom }
                    ,{ "@OrderDateTo", gt.orderdateto }
                    ,{ "@BuyerMobileNumber",gt.MobileNumber }
                    ,{ "@Pageindex", gt.PageIndex }
                    ,{ "@Pagesize", gt.Pagesize }
                    ,{ "@Serch_Type",gt.Unit_Type}
                     ,{ "@PackedType",gt.PackedType}
                    ,{ "@Browser_Falg",gt.Browser_Falg}
                    ,{ "@UserId", User_ID }
                    ,{"@Order_From",gt.OrderFrom}
                };
                var res = _database.GetMultipleResultsList2(Get_Un_Packed, parameters);
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


        //================================To Get UnPacked Product List==============================================================
        [HttpPost]
        [Route("Get_Un_Packed_Order_List_By_Unit_ID")]
        public IHttpActionResult Get_Un_Packed_Order_List_By_Unit_ID(VMPagingResultsPost gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                     { "@Source_UnitID", gt.Source_UnitID }
                    ,{ "@Dest_Unit_ID", gt.Dest_UnitID }
                    ,{ "@OrderID",gt.OrderID}
                    ,{ "@TopCategoryID", gt.TopCategory_ID}
                    ,{ "@SKU",gt.SKU}
                    ,{ "@ProductName", gt.ProductName }
                    ,{ "@CategoryID", gt.CategoryID}
                    ,{ "@OrderDateFrom", gt.orderdatefrom }
                    ,{ "@OrderDateTo", gt.orderdateto }
                    ,{ "@BuyerMobileNumber",gt.MobileNumber }
                    ,{ "@Pageindex", gt.PageIndex }
                    ,{ "@Pagesize", gt.Pagesize }
                    ,{ "@Serch_Type",gt.Unit_Type}
                     ,{ "@PackedType",gt.PackedType}
                    ,{"@Order_From",gt.OrderFrom}
                };
                var res = _database.GetMultipleResultsList2(Get_Un_Order_Packed, parameters);
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



        // ===================== Create Consignments =================
        [HttpPost]
        [Route("Update_Inventory_Product_Status_To_Packed")]
        public IHttpActionResult Update_Inventory_Product_Status_To_Packed(VMConsignments vmodel)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Source_Unit_ID", vmodel.Source_UnitID },
                                                                                           { "@ProductID", vmodel.productid },
                                                                                           { "@Destination_Unit_ID", vmodel.Dest_UnitID  },
                                                                                           { "@OrderID", vmodel.OrderNumber }};
                var result = _database.QueryValue(Update_To_Packing, parameters);
                int iDomainID = 0; int iTotal = 0; int iPacked = 1; string sPhoneNumber = string.Empty;
                Dictionary<string, object> Param = new Dictionary<string, object>() { { "@OrderNumber", vmodel.OrderNumber } };
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
                message = iPacked + " out of " + iTotal + " items in your OrderID:" + vmodel.OrderNumber + " got packed.We will let you know once shipped.";
                var oSMSD = dbContext.MailSMSSettings.Where(m => m.iDomainID == iDomainID).Where(m => m.sTemplateType == "SMS").Where(m => m.sType == "Packed").FirstOrDefault();
                if (oSMSD != null && oSMSD.SMS == true)
                {
                    if (oSMSD.sBody != null && oSMSD.sBody != "" && oSMSD.sBody.Contains("{{"))
                    {
                        string sMsg = oSMSD.sBody;
                        sMsg = sMsg.Replace("{{P}}", iPacked.ToString());

                        sMsg = sMsg.Replace("{{T}}", iTotal.ToString());

                        sMsg = sMsg.Replace("{{O}}", vmodel.OrderNumber.ToString());

                        message = sMsg;
                    }
                    Common.sendmessage(sPhoneNumber, message, oSMSD.sdltid);
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
        [HttpPost]
        [Route("Save_Assign_Logistics")]
        public IHttpActionResult Save_Assign_Logistics(VMPagingResultsPost vmodel)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                    { "@MasterID", vmodel.MasterID },
                    { "@Track_Vachile_Id", vmodel.Track_Vachile_Id },
                    { "@Consignmentids", vmodel.Productids },
                    {"@UserID",userID }
                };
                var referenceNum = _database.ProductListWithCount(Assign_Logistics, parameters);

                int i = 0;
                var oPack = vmodel.oPackedList.ToList();
                if (oPack != null && oPack.Count() > 0)
                {
                    i = oPack.Count();
                    if (i > 0)
                    {
                        Send_SMS_For_Packed_Picked_Status(oPack, "AssignToLogistics");
                    }
                }
                return Ok(referenceNum);
            }
            catch (Exception ex)
            {
                log.Error("Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }
        [Route("Assign_Orders_To_Logistics_Online")]
        public IHttpActionResult Assign_Orders_To_Logistics_Online(VMPagingResultsPost GF)
        {
            try
            {
                string assign_Logistic = "[iB_Assign_Order_To_Logistics]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() {{ "@ordered_product_Id", GF.Order_productID }, { "@master_id", GF.MasterID },
                                { "@isThirdParty",GF.IsThirdParty },{ "@trackingid",GF.TrackingVechlNo }};
                var res = _database.QueryValue(assign_Logistic, parameters);
                int i = 0;
                var oPack = GF.oPackedList.ToList();
                if (oPack != null && oPack.Count() > 0)
                {

                    i = oPack.Count();
                    if (i > 0)
                    {
                        Send_SMS_For_Packed_Picked_Status(oPack, "AssignToLogistics");
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
        // =================================To Assign Product to multiple stores/Warehouse====================================
        [HttpPost]
        [Route("iH_Assign_Stock_To_Multiple_Locations")]
        public IHttpActionResult iH_Assign_Stock_To_Multiple_Locations(string value, int unitid)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@prdids_qty_loc", value }, { "@unitid", unitid } };
                var res = _database.Query(assign_to_Multiple, parameters);
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

        //===================================To Get Intrastore Stock=================================================================
        [HttpPost]
        [Route("Get_IntraStore_Stock")]
        public IHttpActionResult Get_IntraStore_Stock(VMPagingResultsPost gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                    { "@SKU",gt.SKU},
                    { "@Product_Name", gt.ProductName }, { "@Order_Id",gt.OrderID},
                    { "@Order_Date", gt.OrderDate }, { "@Source_UnitId",gt.Source_UnitID},
                    { "@Destination_UnitId", gt.Dest_UnitID },{ "@Order_Status",gt.Status2 },
                    { "@Page_Index", gt.PageIndex },{ "@Page_Size", gt.Pagesize } };
                var res = _database.ProductListWithCount(GetIntraStoreStock, parameters);
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

        //========================================To Get IntraStore source Stores================================
        [HttpGet]
        [Route("Get_IntraStore_SourceStores")]
        public IHttpActionResult Get_IntraStore_SourceStores(int DC_ID)
        {
            try
            {
                int inflow_or_outflow = 2;
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
               { "@DC_Or_Source_Unit_Id", DC_ID}, { "@inflow_or_outflow", inflow_or_outflow} };
                var SourceUnits = _database.Query(sourceIntraStores, parameters);
                return Ok(SourceUnits);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }

        //==================================To Get IntraStore Destination Stores=============================
        [HttpGet]
        [Route("Get_IntraStore_destStores")]
        public IHttpActionResult Get_IntraStore_DestinationStores(int Source_Unit_ID)
        {
            try
            {
                int inflow_or_outflow = 1;
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
               { "@DC_Or_Source_Unit_Id", Source_Unit_ID}, { "@inflow_or_outflow", inflow_or_outflow} };
                var DestinationUnits = _database.Query(destinationIntraStores, parameters);
                return Ok(DestinationUnits);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }

        //=============================To Get All Vendor Orders====================================
        [HttpPost]
        [Route("Get_All_Vendor_Orders_To_InBound")]
        public IHttpActionResult Get_All_Vendor_Orders_To_InBound(VMPagingResultsPost gt)
        {
            try
            {
                int User_ID = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                     { "@Source_UnitID", gt.Source_UnitID }
                    ,{ "@Dest_Unit_ID", gt.Dest_UnitID }
                    ,{ "@OrderID",gt.OrderID},{"@OrderFrom",gt.OrderFrom}
                    ,{ "@SKU",gt.SKU}
                    ,{ "@ProductName", gt.ProductName }
                    ,{ "@OrderDateFrom", gt.orderdatefrom }
                    ,{ "@OrderDateTo", gt.orderdateto }
                    ,{ "@BuyerMobileNumber",gt.MobileNumber }
                    ,{ "@TopCategoryID", gt.TopCategory_ID}
                    ,{ "@LeafCategoryID", gt.CategoryID}
                    ,{ "@Pageindex", gt.PageIndex }
                    ,{ "@Pagesize", gt.Pagesize }
                    ,{ "@UserId", User_ID }
                ,{ "@VendorOrder_type", gt.VendorOrder_type }};
                var res = _database.GetMultipleResultsList2(Get_All_Vendors, parameters);
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

        //==========================To Get Inventory ProductIds==============================================================
        [HttpGet]
        [Route("Get_Inventory_ProductIDs")]
        public IHttpActionResult Get_Inventory_ProductIDs(int OrderID, int ProductID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Order_ID", OrderID }, { "@Product_ID", ProductID } };
                var res = _database.ProductListWithCount(Get_InventoryIds, parameters);
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
        //==========================To Get Inventory ProductIds==============================================================
        [HttpGet]
        [Route("Get_Inventory_ProductIDs_Packed")]
        public IHttpActionResult Get_Inventory_ProductIDs_Packed(int OrderID, int ProductID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Order_ID", OrderID }, { "@Product_ID", ProductID } };
                var res = _database.ProductListWithCount(Get_InventoryIds_Packed, parameters);
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

        //==========================================To Assign Inbound stock==========================================================
        [HttpPost]
        [Route("Assign_Inventory_Product_IDs_Against_Order")]
        public IHttpActionResult Assign_Inventory_Product_IDs_Against_Order(VMPagingResultsPost vmodel)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());
                Dictionary<string, object> parameters = new Dictionary<string, object>() {{ "@OrderID", vmodel.OrderID },{ "@ProductID", vmodel.Productid },{ "@NewLandingPrice", vmodel.MaxValue }, { "@ProductIds_Qty", vmodel.FilterJson },
                    { "@User_ID", userID } ,{ "@IP", "465" }};
                var referenceNum = _database.QueryValue(InBoud_Vendor_Products, parameters);
                return Ok(referenceNum);
            }
            catch (Exception ex)
            {
                log.Error("Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        // =================================Get Warehouse Inflow Outflow Stock====================================
        [HttpPost]
        [Route("Get_Warehouse_Inflow_Outflow_Stock")]
        public IHttpActionResult Get_Warehouse_Inflow_Outflow_Stock(vmmodelsforDCoutflow gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {{ "@SKU",gt.SKU},
                                                                                          { "@Product_Name", gt.ProductName },
                                                                                          { "@Order_Id",gt.OrderID},
                                                                                          { "@Order_Date", gt.OrderDate },
                                                                                          { "@Source_Name",gt.Source_UnitID},
                                                                                          { "@Destination_Name", gt.Dest_UnitID },
                                                                                          { "@Order_Status",gt.Status },
                                                                                          { "@Page_Index", gt.PageIndex },
                                                                                          { "@Page_Size", gt.Pagesize },
                                                                                          { "@UnitID",gt.Unit_ID },
                                                                                          { "@Heirarachy_Level",gt.Heirarachy_Level }
                                                                                          };
                var res = _database.ProductListWithCount(WarehouseInflowOutflow, parameters);
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

        //========================================================To Get WareHouseID From UnitId===========================================
        [HttpGet]
        [Route("Get_WHID_From_UnitId")]
        public IHttpActionResult Get_WHID_From_UnitId(int UnitID)
        {
            try
            {
                Get_WhId = Get_WhId + UnitID;
                Dictionary<string, object> parameters = new Dictionary<string, object>() { };
                var res = _database.QueryValueCOUNT(Get_WhId, parameters);
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

        //===================== Get Warehouse Products List =================
        [HttpPost]
        [Route("GetWarehouseProducts")]
        public IHttpActionResult GetWarehouseProducts(VMModelsForWHProducts model)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ProductName", model.ProductName },
                                                                                           { "@Sku", model.SKU },
                                                                                           { "@WH_Id", model.Unit_ID }
                                                                                           };
                var result = _database.ProductListWithCount(Get_Wh_Stock, parameters);
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

        //================================To Get InOutFlow Destination Stores===============================================
        [HttpGet]
        [Route("Get_InOutflow_DestinationStores")]
        public IHttpActionResult Get_InOutflow_DestinationStores(int inoutFlow, int sourceid)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@unitid", sourceid },
                                                                                           { "@inflow_or_outflow",inoutFlow},};
                var res = _database.ProductListWithCount(destStores, parameters);
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

        //=============================================To Get Not Availble Products============================================
        [HttpPost]
        [Route("Get_Prds_Not_Avil_MD")]
        public IHttpActionResult Get_UnDiscount_Prds(VMModelsForWHProducts vmodel)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@sku", vmodel.ID },
                                                                                         { "@filterjson", vmodel.FilterJson },
                                                                                         { "@categoryid", vmodel.CategoryID },
                                                                                         { "@Page_Size", vmodel.Pagesize },
                                                                                         { "@Page_Index", vmodel.PageIndex },
                                                                                         { "@productname", vmodel.ProductName }
                                                                                                                                };
                var res = _database.ProductListWithCount(Products_Not_In_MegaDeals, parameters);
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

        // =================================Method For getting Dashboard Details by UnitID====================================================
        [HttpGet]
        [Route("iH_Get_Data_For_Admin_Dashboard")]
        public IHttpActionResult iH_Get_Data_For_Admin_Dashboard()
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { };
                var dashboard_result = _database.QueryValue(Admin_Dashboard, parameters);
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

        //=====================================To Get FMCG Stock=============================================================
        [HttpPost]
        [Route("GetFMCGStock")]
        public IHttpActionResult GetFMCGStock(FMCG gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@DcId",gt.Dc_ID },
                    { "@Sku",gt.SKU },{ "@ProductName",gt.ProductName},{ "@Category_Id",gt.catid},{ "@BrandName",gt.BrandName},
            { "@BatchNumber", gt.BatchNumber }, { "@ManufactureDate",gt.ManufactureDate},
            { "@ExpirationDate", gt.ExpirationDate }, {"@Days_To_Expire",gt.daystoexpire },
            { "@PageSize", gt.pagesize },{ "@PageIndex", gt.pageindex }};
                var res = _database.GetMultipleResultsList(Get_FMCG_Stock, parameters);
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

        //=================================================To Get Brand List================================================
        [HttpPost]
        [Route("Get_Brand_List")]
        public IHttpActionResult Get_Brand_List(int CatID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@CategoryId", CatID } };
                var res = _database.ProductListWithCount(FMCG_Brands_List, parameters);
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

        //=============================================To Get Inventory Date history====================================
        [HttpPost]
        [Route("Get_All_Inventory_Date_history")]
        public IHttpActionResult Get_All_Inventory_Date_history(VMModelsForCategory gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Product_Id", gt.ProductIds }, { "@Order_Date_From", gt.OrderDateFrom }, { "@Order_Date_To", gt.OrderDateTo } };
                var res = _database.ProductListWithCount(Inventory_Date_history, parameters);
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



        //=====================To Get Mega Deals parent and CatId=======================================
        [HttpPost]
        [Route("MD_Parent_Catids")]
        public IHttpActionResult MD_Parent_Catids()
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { };
                var res = _database.ProductListWithCount(commandText, parameters);
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

        //===================================To Get MegaDeals Products With Filters============================================
        [HttpPost]
        [Route("Get_MD_Products")]
        public IHttpActionResult Get_MD_Products(VMPagingResultsPost vmodel)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@sku", vmodel.CategoryName }
                                                      , { "@categoryid",vmodel.CategoryID }
                                                      , { "@filterjson",vmodel.JsonData}
                                                      , { "@productname",vmodel.ProductName}
                                                      , { "@validfrom", vmodel.orderdatefrom}
                                                      , { "@validto", vmodel.orderdateto}
                                                      , { "@status", vmodel.Status}
                                                      , { "@parentcatid",vmodel.ID} };
                var res = _database.ProductListWithCount(MegaDeals_Filters, parameters);
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

        //============================To Assign_Stock_ToUnit===========================================================
        [HttpPost]
        [Route("Cheque_Order_Status_Change")]
        public IHttpActionResult Cheque_Order_Status_Change(VMModelsForOrderManagement vmodel)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@OrderNumber", vmodel.OrderID }, { "@Status", vmodel.Status }, { "@AdminUserId", userID } };
                var ProductsList = _database.QueryValue(Chq_Order_Status, parameters);
                return Ok(ProductsList);
            }
            catch (Exception ex)
            {
                log.Error("Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        //============================================To Add product Into Mega Deals=============================================
        [HttpPost]
        [Route("InsertProductsIntoMegaDeals")]
        public IHttpActionResult InsertProductsIntoMegaDeals(VMPagingResultsPost vmodel)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@productids", vmodel.Productids }, { "@validfrom", vmodel.orderdatefrom.ToString() }, { "@validto", vmodel.orderdateto.ToString() }, { "@status", vmodel.Status } };
                var res = _database.QueryValue(insert_MegaDeals, parameters);
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


        //================================== Method To Get ALL Categories List Based On Orders==============
        [HttpPost]
        [Route("InventoryGetCategoriesList")]
        public IHttpActionResult InventoryGetCategoriesList()
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { };
                var ProductsList = _database.ProductListWithCount(Get_Inventory_Cat_List, parameters);
                return Ok(ProductsList);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }

        // =================================Get DC and Warehouse Inflow Outflow Stock====================================
        [HttpPost]
        [Route("Get_DC_Warehouse_Inflow_Outflow_Stock")]
        public IHttpActionResult Get_DC_Warehouse_Inflow_Outflow_Stock(vmmodelsforDCoutflow gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {{ "@SKU",gt.SKU},
                                                                                          { "@Product_Name", gt.ProductName },
                                                                                          { "@Order_Id",gt.OrderID},
                                                                                          { "@Order_Date", gt.OrderDate },
                                                                                          { "@Source_Name",gt.Source_UnitID},
                                                                                          { "@Destination_Name", gt.Dest_UnitID },
                                                                                          { "@Order_Status",gt.Status },
                                                                                          { "@Page_Index", gt.PageIndex },
                                                                                          { "@Page_Size", gt.Pagesize },
                                                                                          { "@UnitID",gt.Unit_ID },
                                                                                          { "@Heirarachy_Level",gt.Heirarachy_Level }
                                                                                          };
                var res = _database.ProductListWithCount(DC_WarehouseInflowOutflow, parameters);
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

        //==================================To Get All Products================================================
        [HttpPost]
        [Route("GET_Prds_List_BY_IDS")]
        public IHttpActionResult GET_Prds_List_BY_IDS(VMModelsForProduct gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ProductIDs", gt.productids } };
                var res = _database.ProductListWithCount(Get_All_Products, parameters);
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

        //=============================================TO Get Intrsstore For WareHouse  Admin===========================================
        [HttpPost]
        [Route("Get_IntraStore_Orderes_For_Warehouse")]
        public IHttpActionResult Get_IntraStore_Orderes_For_Warehouse(VMModelsForWareHouseIntrastore gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
             { "@UnitID",gt.Unit_ID},{ "@SKU",gt.SKU},
             { "@Product_Name", gt.ProductName }, { "@Order_Id",gt.OrderID},
             { "@Order_Date", gt.OrderDate }, { "@Source_Name",gt.Source_UnitID},
             { "@Destination_Name", gt.Dest_UnitID },{ "@Order_Status",gt.Status2 },
             { "@Page_Index", gt.PageIndex },{ "@Page_Size", gt.Pagesize },
              };
                var res = _database.ProductListWithCount(WarehouseIntra, parameters);
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
        //===================== GET Assigned Stock From WH To Store =================
        [HttpGet]
        [Route("GetDammagedorMissingInventory")]
        public IHttpActionResult GetDammagedorMissingInventory(string Sku, string Type)
        {
            try
            {


                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Sku", Sku }, { "@Type", Type } };
                var result = _database.GetMultipleResultsList2(Get_Dammaged_Missing, parameters);
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

        //------------------------To Add Schemes-------------------------------------------------------------------
        [HttpPost]
        [Route("InsertProductsIntoSchemes")]
        public IHttpActionResult InsertProductsIntoSchemes(VMPagingResultsPost vmodel)
        {
            try
            {

                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@productids", vmodel.Productids }, { "@type", vmodel.Type }, { "@Status", vmodel.Status },
                {"@Amount" ,vmodel.Amount}, { "@Bonus_Amount",vmodel.Bonus_Amount} };
                var res = _database.QueryValue(schemes, parameters);
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
                string Get_Assigned_Stock_ST_Units = "[iAdmin_Get_Assigned_Stock_ST_Units]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@WarehouseID", WareHouseID } };
                var result = _database.ProductListWithCount(Get_Assigned_Stock_ST_Units, parameters);
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
        //------------=========================================App Related Code =============================

        //================================== Method To Get ALL Categories List Based On Store/DC Orders for iHub Outbound APP==============
        [HttpPost]
        [Route("GetCategoriesListOutbound")]
        public IHttpActionResult GetCategoriesListOutbound(VMModelsForInventory vmodel)
        {
            try
            {

                Dictionary<string, object> parameters = new Dictionary<string, object>() {{ "@flagDCStore", vmodel.flagDCStore }, { "@Unit_ID", vmodel.Unit_ID }
                    , { "@DC_Unit_ID", vmodel.CategoryID } };
                var ProductsList = _database.ProductListWithCount(Get_Cat_List_Outbound, parameters);
                return Ok(ProductsList);

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
        [HttpPost]
        [Route("Map_ProductIMEI_App")]
        public IHttpActionResult Map_ProductIMEI_App(int OrderID, int ProductID, int InventoryProdID, String ScannedIMEI)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@OrderID", OrderID }, { "@ProductID", ProductID }, { "@InventoryProdID", InventoryProdID }, { "@ScannedIMEI", ScannedIMEI } };
                var res = _database.Query(map_IMEI_againstProduct, parameters);
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


        //==================================Get Stock moving List===========================================
        [HttpPost]
        [Route("MapProductBarcode")]
        public IHttpActionResult MapProductBarcode(int ProductID, string Barcode, char hasBarcode)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Product_ID", ProductID },
                { "@varBarcode", Barcode }, { "@hasBarcode", hasBarcode }
                };
                var MapBarcodeForProduct = _database.Query(map_Barcode_againstProduct, parameters);
                return Ok(MapBarcodeForProduct);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        //------------=========================================App Related Code END=============================

        //=======>>>>>>> Update Multiple Orders Pack List
        [HttpPost]
        [Route("UpdateMultiplePackList")]
        public IHttpActionResult UpdateMultiplePackList(VMPagingResultsPost Model)
        {
            try
            {

                int i = 0;
                var oPack = Model.oPackedList.ToList();
                if (oPack != null && oPack.Count() > 0)
                {

                    foreach (var Pack in oPack)
                    {
                        Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Source_Unit_ID", Pack.Source_UnitID },
                                                                                           { "@ProductID",Pack.productid },
                                                                                           { "@Destination_Unit_ID", Pack.Dest_UnitID  },
                                                                                           { "@OrderID",Pack.OrderID },
                            { "@Ordered_Product_Details_ID",Pack.Ordered_Product_Details_ID},
                            {"@Quantity",Pack.Ordered_Quantity } };
                        var result = _database.QueryValue(Update_To_Packing, parameters);
                    }
                    i = oPack.Count();
                    if (i > 0)
                    {
                        Send_SMS_For_Packed_Picked_Status(oPack, "Packed");
                        //Send_Mail_For_Packed_Picked_Status(oPack, "Packed");
                    }
                }
                return Ok(i);
            }
            catch (Exception Ex)
            {
                log.Error("Error---------", Ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = Ex.Message;
                return Ok(cs);
            }
        }
        //================================To Get UnPacked Product List==============================================================
        [HttpPost]
        [Route("Get_Packed_Product_List_By_Unit_ID")]
        public IHttpActionResult Get_Packed_Product_List_By_Unit_ID(VMPagingResultsPost gt)
        {
            try
            {
                int User_ID = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                    { "@OrderDateFrom", gt.orderfrom }
                    ,{ "@OrderDateTo", gt.orderto }
                    //,{ "@Source_UnitID", gt.Source_UnitID }
                    ,{ "@TopCategoryID", gt.TopCategory_ID}
                    ,{ "@Source_UnitID", gt.Source_UnitID }
                    ,{ "@OrderID",gt.OrderID}
                    ,{ "@SKU",gt.SKU}
                    ,{ "@ProductName", gt.ProductName },{ "@PackedType",gt.PackedType}
                    ,{ "@PageIndex", gt.PageIndex }
                    ,{ "@PageSize", gt.Pagesize }
                    ,{ "@UserId", User_ID }
                    ,{"@Order_From",gt.OrderFrom}
                };
                var res = _database.GetMultipleResultsList2(Get_Packed_List, parameters);
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
        //=======>>>>>>> Update Multiple Orders Pack List
        [HttpPost]
        [Route("DeliryChallanForMultipleOrders")]
        public IHttpActionResult DeliryChallanForMultipleOrders(VMPagingResultsPost Model)
        {
            try
            {
                List<VMDataTableResponse> oData = new List<VMDataTableResponse>();
                string Invoice = "[iAdmin_Get_Order_Details_By_Order_Number_and_ProductIds]";
                var GrpOrderIdList = Model.oPackedList.GroupBy(u => u.OrderID).Select(grp => grp.ToList());
                int OrderIDs = 0; var ProductIDs = string.Empty;
                var ExecutedOrders = new List<int>();
                foreach (var item in GrpOrderIdList)
                {
                    OrderIDs = item[0].OrderID;
                    if (!ExecutedOrders.Contains(OrderIDs))
                    {
                        var sdsd = Model.oPackedList.Where(s => s.OrderID == OrderIDs).ToList();
                        var ProIDs = string.Join(",", sdsd.Select(M => M.ProductIDs));
                        Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@OrderID", OrderIDs }, { "@ProductIDs", ProIDs } };
                        ExecutedOrders.Add(OrderIDs);
                        var Result = _database.GetMultipleResultsListAll(Invoice, parameters);
                        oData.Add(Result);
                    }
                    if (oData.Count() > 0)
                    {
                        oData.FirstOrDefault().TotalCount = oData.Count();
                    }
                }
                return Ok(oData);
            }
            catch (Exception Ex)
            {
                log.Error("Error---------", Ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = Ex.Message;
                return Ok(cs);
            }
        }
        //=======>>>>>>> Update Multiple Orders Pack List
        [HttpPost]
        [Route("UpdateMultipleProductList")]
        public IHttpActionResult UpdateMultipleProductList(VMPagingResultsPost Model)
        {
            try
            {
                int i = 0;
                var oPack = Model.oPackedList.ToList();
                if (oPack != null && oPack.Count() > 0)
                {

                    foreach (var Pack in oPack)
                    {
                        Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Product_details_Id", Pack.iProDetailsID },
                                                                                           { "@unitid",Pack.Source_UnitID },
                                                                                           { "@orderedlocation_id", Pack.iLocationID  },
                                                                                           { "@Status",Pack.status }};
                        var result = _database.QueryValue(Update_MulProduct_Status, parameters);
                    }
                    i = oPack.Count();
                    if (i > 0)
                    {
                        Send_SMS_For_Packed_Picked_Status(oPack, "Picked");
                    }
                }
                return Ok(i);
            }
            catch (Exception Ex)
            {
                log.Error("Error---------", Ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = Ex.Message;
                return Ok(cs);
            }
        }
        //===============>>>>>>>>>>> Assign_Orders_To_Logistics <<<<<<<<<<<<<<=====================
        [Route("Assign_Orders_To_Logistics")]
        public IHttpActionResult Assign_Orders_To_Logistics(VMPagingResultsPost GF)
        {
            try
            {
                string assign_Logistic = "[iAdmin_Assign_Order_To_Logistics]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() {{ "@ordered_product_Id", GF.Order_productID },
                                                                                            { "@master_id", GF.MasterID },
                                                                                            { "@isThirdParty",GF.IsThirdParty },
                                                                                            { "@trackingid",GF.TrackingID }};
                var res = _database.QueryValue(assign_Logistic, parameters);
                int i = 0;
                var oPack = GF.oPackedList.ToList();
                if (oPack != null && oPack.Count() > 0)
                {
                                       
                    i = oPack.Count();
                    if (i > 0)
                    {
                        Send_SMS_For_Packed_Picked_Status(oPack, "AssignToLogistics");
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
        #endregion "END of -- InventoryAndStock Management"
        //------------=========================================App Related Code END=============================
        //=================>>>>>>>>>>> Update FMCG Data <<<<<<<<<<<<<=========================//
        [HttpPost]
        [Route("Update_FMCG_Data")]
        public IHttpActionResult Update_FMCG_data(FMCG gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ProductId", gt.ProductId }, { "@Unit_Id",gt.Dc_ID },
                { "@BatchNumber", gt.BatchNumber }, { "@ManufactureDate",gt.ManufactureDate},
                { "@ExpirationDate", gt.ExpirationDate }, { "@flgExpired", gt.flgExpired },{ "@Quantity", gt.qty }};
                var res = _database.QueryValue(Update_FMCG, parameters);
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
        //=========================================  Get Reorder level for inventry==================================================
        [HttpPost]
        [Route("GetReorder_Level")]
        public IHttpActionResult GetReorder_Level(FMCG gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                    { "@SKU",gt.SKU },{ "@Product_Name",gt.ProductName},{ "@ReorderLevel",gt.Re_OrderLevel_Qnty},{ "@AvailablQuantity",gt.AvailableQuantity},{ "@SourceVendor",gt.SourceVendor},
            { "@Leafcatogory", gt.LeafCategory }, { "@MainCategory",gt.MainCategory},{"@Quantity",gt.Quantity },{"@Condition",gt.Condition}, { "@PageSize", gt.pagesize },{ "@PageIndex", gt.pageindex } };
                var res = _database.GetMultipleResultsList2(Reorderlevel, parameters);
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
        ///=====================================
        [HttpGet]
        [Route("Get_Units_with_data")]
        public IHttpActionResult Get_Units_with_data(int unitid)
        {
            try
            {

                var StoreNames = "	SELECT iHub_Unit_ID,Unit_Name,Available_Cash FROM iHub_Units_DC_WH_ST WHERE Master_Franchise_ID in (SELECT Master_Franchise_ID FROM iHub_Units_DC_WH_ST WHERE iHub_Unit_ID=" + unitid + ") AND Unit_Hierarchy_Level IN (2,3) AND Is_Store_Type=1";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { };
                var res = _database.SelectQuery(StoreNames, parameters);
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
        //===============get storesfor clusters================
        [HttpGet]
        [Route("Getstores")]
        public IHttpActionResult Getstores(int unitid)
        {
            try
            {

                var StoreNames = " SELECT iHub_Unit_ID, Unit_Name FROM iHub_Units_DC_WH_ST where Unit_Hierarchy_Level= " + unitid;
                Dictionary<string, object> parameters = new Dictionary<string, object>() { };
                var res = _database.SelectQuery(StoreNames, parameters);
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
        //==========================================To Assign Inbound stock==========================================================
        [HttpPost]
        [Route("UpdateStockCheckDamage")]
        public IHttpActionResult UpdateStockCheckDamage(VMPagingResultsPost vmodel)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                    {"@Order_Id",vmodel.Order_Id },
                    { "@ProductID", vmodel.Productid },
                    { "@Inventory_Product_ID", vmodel.Inventory_Product_ID },
                    { "@Quantitiy", vmodel.Quantitiy },
                    { "@UnitID", vmodel.UnitID },
                    { "@User_ID", userID },
                    {"@ReasonNote",vmodel.ReasonNote },
                    { "@Damages",vmodel.Damages},
                    { "@IMEI", vmodel.Unique_Serial_Number},
                    { "@Batch_Number", vmodel.Batch_Number },
                    { "@Manufacture_Date", vmodel.Manufacture_Date },
                    { "@Expiration_Date", vmodel.Expiration_Date },
                    {"@Inventory_Product_Status",vmodel.Inventory_Product_Status },
                    {"@Login_UnitID",vmodel.Login_UnitID },
                    {"@Ordered_Product_Details_ID",vmodel.Ordered_Product_Details_ID },
                    {"@DamageFlag",vmodel.DamageFlag }

                };
                var referenceNum = _database.ProductListWithCount(testUpdateStockCheckDamage, parameters);
                return Ok(referenceNum);
            }
            catch (Exception ex)
            {
                log.Error("Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }
        //===================Get Inventory Stock======================//
        [HttpPost]
        [Route("Get_Inventory_Stock_Shrinkage")]
        public IHttpActionResult Get_Inventory_Stock_Shrinkage(VMModelsForInventory Cat)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ProductName", Cat.ProductName }
                   , { "@SKU", Cat.SKU }
                   , { "@Page_Size", Cat.Pagesize }
                   , { "@Page_Index", Cat.PageIndex }
               };
                var Stock_Moving_ListNew = _database.Query(Inventory_Products_Shrinkage, parameters);
                return Ok(Stock_Moving_ListNew);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        //===================Get Inventory Stock======================//
        [HttpPost]
        [Route("Get_Inventory_Stock_Shrinkage_Approvale")]
        public IHttpActionResult Get_Inventory_Stock_Shrinkage_Approvale(VMModelsForInventory Cat)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ProductName", Cat.ProductName }
                   , { "@SKU", Cat.SKU }
                    , { "@Order_Number", Cat.Order_Number }
                    , { "@Leaf_Category_ID", Cat.CategoryID }
                    , { "@Top_Category_ID", Cat.Top_Category_ID }
                   , { "@Page_Size", Cat.Pagesize }
                   , { "@Page_Index", Cat.PageIndex }
               };
                var Stock_Moving_ListNew = _database.GetMultipleResultsList2(Get_InventoryIds_Shrinkage_Approvale, parameters);
                return Ok(Stock_Moving_ListNew);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        //==========================To Get Inventory ProductIds==============================================================
        [HttpGet]
        [Route("GetInventoryProductIDs_Shrinkage")]
        public IHttpActionResult GetInventoryProductIDs_Shrinkage(int OrderID, int ProductID, int Unit_Id)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Order_ID", OrderID }, { "@Product_ID", ProductID }, { "@Unit_Id", Unit_Id } };
                var res = _database.ProductListWithCount(Get_InventoryIds_Shrinkage, parameters);
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
        //==========================================To Assign Inbound stock==========================================================
        [HttpPost]
        [Route("UpdateShrinkageStock")]
        public IHttpActionResult UpdateShrinkageStock(VMPagingResultsPost vmodel)
        {
            try
            {
                int userID = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                    {"@Order_Id",0 },
                    { "@ProductID", vmodel.Productid },
                    { "@Inventory_Product_ID", vmodel.Inventory_Product_ID },
                    { "@Quantitiy", vmodel.Quantitiy },
                    { "@UnitID", vmodel.UnitID },
                    { "@User_ID", userID },
                    {"@ReasonNote",vmodel.ReasonNote },
                    { "@Damages",vmodel.Damages},
                    { "@IMEI", vmodel.Unique_Serial_Number},
                    { "@Batch_Number", vmodel.Batch_Number },
                    { "@Manufacture_Date", vmodel.Manufacture_Date },
                    { "@Expiration_Date", vmodel.Expiration_Date },
                    {"@Inventory_Product_Status",vmodel.Inventory_Product_Status },
                    {"@Login_UnitID",vmodel.Login_UnitID }
                };
                var referenceNum = _database.ProductListWithCount(UpdateShrinkageStock_SP, parameters);
                return Ok(referenceNum);
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
        [Route("UpdateStockSpiltApproval")]
        public IHttpActionResult UpdateStockSpiltApproval(int Ordered_Product_Details_ID, int OrderID, int Product_Id)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                    { "@Product_Details_ID", Ordered_Product_Details_ID },{ "@OrderID", OrderID },{ "@Product_Id", Product_Id }
                };
                var result = _database.GetMultipleResultsList2(SpiltApproval, parameters);
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
        

        //=======>>>>>> Get Store Funds <<<<<<<============
        [HttpGet]
        [Route("Units_StockFunds")]
        public IHttpActionResult Units_StockFunds(int unitid, string Type)
        {
            try
            {

                var UnitNames = "SELECT UNT.iHub_Unit_ID,UNT.Unit_Name,(CASE WHEN SF.UnitID IS NULL THEN UNT.iHub_Unit_ID ELSE SF.UnitID END) AS UnitID,COALESCE(SF.AllocatedAmt, 0) AS AllocatedAmt,COALESCE(SF.AvailableAmt, 0) AS AvailableAmt,(CASE WHEN SF.FundType IS NULL THEN  '" + Type + "' ELSE SF.FundType END) AS FundType FROM iHub_Units_DC_WH_ST   UNT  LEFT JOIN UnitStockFund SF ON SF.UnitID = UNT.iHub_Unit_ID AND FundType=" + "'" + Type + "'" + " where UNT.iHub_Unit_ID=" + unitid;
                Dictionary<string, object> parameters = new Dictionary<string, object>() { };
                var res = _database.SelectQuery(UnitNames, parameters);
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
        //===============>>>>> Get Funds Debit Or Credit Details <<<<<=========================
        [HttpPost]
        [Route("Update_Fund_Deposit_Details")]
        public IHttpActionResult Update_Fund_Deposit_Details(PackedList Fund)
        {
            try
            {
                string Update_Fund = "[iAdmin_Update_Fund_Deposit_Details]";
                int User_ID = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                    { "@UnitID", Fund.UnitID }
                    ,{ "@AllocatedAmt", Fund.AllocatedAmt }
                    ,{ "@AvailableAmt", Fund.AvailableAmt }
                    ,{ "@FundsAmountAndType", Fund.FundsAmountAndType}
                    ,{ "@Notes", Fund.Notes}
                    ,{ "@Type", Fund.Type}
                    ,{ "@UserId", User_ID }
                     ,{ "@FundAdjustment", Fund.FundAdjustment }
                };
                var res = _database.ProductListWithCount(Update_Fund, parameters);
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
        //=======>>>>>> Get Units Funds List Grid <<<<<<<============
        [HttpPost]
        [Route("Get_Unit_Fund_Details")]
        public IHttpActionResult Get_Unit_Fund_Details(PackedList Fund)
        {
            try
            {
                string FundList = "[iAdmin_Get_Fund_Deposit_Details]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() {{ "@FullName", Fund.Name },
                                                                                           { "@FundType",Fund.Type },
                                                                                           { "@Page_Index", Fund.PageIndex  },
                                                                                           { "@Page_Size",Fund.PageSize } };
                var res = _database.ProductListWithCount(FundList, parameters);
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
        // ============>>>>>>>>>>>> Update Status For CashbackOffers  <<<<<<<<<<========================
        [HttpPost]
        [Route("Update_UnitsFund_Status")]
        public IHttpActionResult Update_UnitsFund_Status(int UnitId, int NewStatus)
        {
            try
            {
                string result = "";
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                if (User_Id == 1 || User_Id == 16357)
                {
                    var cmd = "Select unitStatus from UnitStockFund where UnitID=" + UnitId;
                    Dictionary<string, object> Param = new Dictionary<string, object> { };
                    var oSelect = _database.SelectQuery(cmd, Param);
                    if (oSelect != null && oSelect.Count() > 0)
                    {
                        var iStatus = oSelect.FirstOrDefault().Select(m => m.Value).FirstOrDefault();
                        cmd = "Update UnitStockFund set unitStatus=" + "'" + NewStatus + "',Updated_Date=" + "'" + DateTime.Now + "'" + " where UnitID=" + UnitId;
                        Dictionary<string, object> Param1 = new Dictionary<string, object> { };
                        var oUpdate = _database.SelectQuery(cmd, Param1);
                        if (iStatus == "10")
                        {
                            result = "<b Style='Color: red'>Funds for Unit deactivated successfully..!</b>";
                        }
                        else
                        {
                            result = "<b Style='Color: green'>Funds for Unit activated successfully..!</b>";
                        }
                    }
                }
                else
                {
                    result = "<b Style='Color: red'>user do not have permissions to change the status</b>";
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
        //=======>>>>>> Get  Funds Transactions<<<<<<<============
        [HttpPost]
        [Route("View_Fund_Transactions")]
        public IHttpActionResult View_Fund_Transactions(PackedList Fund)
        {
            try
            {
                string ViewFund = "[iAdmin_View_Fund_Deposit_Transaction]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() {{ "@Unit_ID", Fund.UnitID },
                                                                                           { "@FundType",Fund.Type },
                                                                                           { "@Page_Index", Fund.PageIndex  },
                                                                                           { "@Page_Size",Fund.PageSize } };
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
        //=======>>>>>> Get  Funds Stock Request List<<<<<<<============
        [HttpPost]
        [Route("Get_All_Stock_Request_Orders")]
        public IHttpActionResult Get_All_Stock_Request_Orders(PackedList Fund)
        {
            try
            {
                string ViewFund = "[iAdmin_Get_Stock_Request_Orders]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ReqId", Fund.OrderID },
                                                                                           { "@UnitID", Fund.UnitID },
                                                                                           { "@Status",Fund.Status },
                                                                                           { "@FromDate", Fund.FromDate  },
                                                                                           { "@ToDate",Fund.ToDate },
                                                                                           { "@Page_Index", Fund.PageIndex },
                                                                                           { "@Page_Size",Fund.PageSize }};
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
        //=======>>>>>> Get  Funds Stock Request List<<<<<<<============
        [HttpPost]
        [Route("View_Request_Products_List")]
        public IHttpActionResult View_Request_Products_List(PackedList Fund)
        {
            try
            {
                string ViewFund = "[iAdmin_Get_Stock_Request_Ordered_Products_List]";

                //string ViewFund = "SELECT P.iHub_Product_ID,(CAST(Product_Series AS VARCHAR) + '-' + CAST((CAST(iHub_Product_ID AS INT) + 100000) AS VARCHAR)) AS SKU,D.CategoryName as LeafCategory, U.RequestSP,U.ReqQuantity,(CASE WHEN Quantity = 0 AND U.ReqStatus = 10 THEN U.ReqQuantity ELSE Quantity END) AS Quantity, Selling_Price as UnitPrice, (U.RequestSP * U.ReqQuantity) as TotalPrice ,Product_Name,RequestID,U.ReqStatus,U.notes,(Selling_Price * Quantity) as TotalNewPrice,Is_Repurchasable  FROM UnitStockProducts U join UnitStockRequests R on R.StockReqID = U.RequestID JOIN iHub_Products  P on P.iHub_Product_ID = U.ProductID  JOIN iH_Category_Product_Mapping  M on Product_Id = iHub_Product_ID  join iH_Categories D ON M.Category_Id = D.ID  JOIN iH_categories E ON D.Top_Category_Id = E.ID  WHERE RequestID = " + "'" + Fund.ReqId + "'" + " and U.UnitId=" + Fund.UnitID;
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                    { "@UnitID", Fund.UnitID }, { "@ReqId",Fund.ReqId }
                };
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

        // =====================>>>>>>>>>>>>>>> To Update Weekly Activity Report Details <<<<<<<<<<=================================
        [HttpPost]
        [Route("Update_RequestProduct_Details")]
        public IHttpActionResult Update_RequestProduct_Details(PackedList GF)
        {
            try
            {
                string UpdateWeeklyReport = "[iAdmin_Update_Request_Product_Details]";
                int User_ID = Convert.ToInt32(HttpContext.Current.User.Identity.GetUserID());
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@sReqId", GF.ReqId },{ "@UpdateProData", GF.UpdateProData }, { "@ReqAmnt", GF.ReqAmnt },
                                                                                            { "@AprvedAmnt", GF.AprvedAmnt }, { "@DiffAmnt", GF.DiffAmnt }, { "@User_ID", User_ID } };
                var Result = _database.ProductListWithCount(UpdateWeeklyReport, parameters);
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

        public int Send_SMS_For_Packed_Picked_Status(List<PackedList> oPack, string sStatus)
        {
            int iDomainID = 0; int iTotal = 0; string sPhoneNumber = string.Empty; string sProductName = string.Empty; int iStatus = 0;
            VMPagingResultsPost vmodal = new VMPagingResultsPost();
            vmodal.ProductDetails = new List<Dictionary<string, string>>();
            int i = 0;
            try
            {
                var oOrders = oPack.Select(m => m.OrderID).Distinct().ToList();
                var sNewStatus = oPack.Select(m => m.status).Distinct().ToList().FirstOrDefault();
                if (oOrders.Count() > 0)
                {
                    foreach (var iOrderNum in oOrders)
                    {
                        int iPacked = oPack.ToList().Where(m => m.OrderID == iOrderNum).Count();

                        Dictionary<string, object> Param = new Dictionary<string, object>() { { "@OrderNumber", iOrderNum } };
                        var oCatList = _database.GetMultipleResultsListAll("iS_Get_Order_Details_By_Order_Number", Param);
                        if (oCatList != null)
                        {
                            if (oCatList.Addressset != null && oCatList.Addressset.Count() > 0)
                            {
                                iTotal = oCatList.Addressset.Count();
                                sPhoneNumber = oCatList.Addressset[0]["Mobile_Number"];
                                for (int k = 0; k < oPack.Count(); k++)
                                {
                                    var iPackOrderID = oPack[k].productid;
                                    foreach (var sPro in oCatList.Addressset)
                                    {
                                        var iProID = Convert.ToInt32(sPro.ToList().Where(m => m.Key == "Product_Id").Select(m => m.Value).FirstOrDefault());
                                        if (iPackOrderID == iProID)
                                        {
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
                                }
                                //sProductName = oCatList.Addressset[0]["Product_Name"];                                
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
                        if(sNewStatus == 50)
                        {
                            sStatus = "Delivered";
                        }
                        if (sStatus == "Packed")
                        {
                            message = iPacked + " out of " + iTotal + " products from your Order " + iOrderNum + " got packed.We will let you know once shipped.";
                            oMailSMS = dbContext.MailSMSSettings.Where(m => m.iDomainID == iDomainID).Where(m => m.sTemplateType == "SMS").Where(m => m.sType == "Packed").FirstOrDefault();

                            sEmail = iPacked + " out of " + iTotal + " products from your Order " + iOrderNum + " has been packed and will be dispatched soon.";
                            oMailEmail = dbContext.MailSMSSettings.Where(m => m.iDomainID == iDomainID).Where(m => m.sTemplateType == "MAIL").Where(m => m.sType == "Packed").FirstOrDefault();
                            iStatus = 20;
                        }
                        else if(sStatus == "Picked")
                        {
                            message = iPacked + " out of " + iTotal + " products from your Order " + iOrderNum + " got picked.We will let you know once shipped.";
                            oMailSMS = dbContext.MailSMSSettings.Where(m => m.iDomainID == iDomainID).Where(m => m.sTemplateType == "SMS").Where(m => m.sType == "Picked").FirstOrDefault();

                            sEmail = iPacked + " out of " + iTotal + " products from your Order " + iOrderNum + " has been dispatched and will be delivered soon.";
                            oMailEmail = dbContext.MailSMSSettings.Where(m => m.iDomainID == iDomainID).Where(m => m.sTemplateType == "MAIL").Where(m => m.sType == "Picked").FirstOrDefault();
                            iStatus = 30;
                        }
                        else if(sStatus == "Delivered")
                        {
                            message = iPacked + " out of " + iTotal + " products from your Order " + iOrderNum + " is successfully delivered today. Thank you for shopping & visit again.";
                            oMailSMS = dbContext.MailSMSSettings.Where(m => m.iDomainID == iDomainID).Where(m => m.sTemplateType == "SMS").Where(m => m.sType == "Delivered").FirstOrDefault();

                            //sEmail = "Your item(s) " + vmodal.ProductName + " of order id " + iOrderNum + " is successfully delivered today. Thank you for shopping & visit again.";
                            sEmail = iPacked + " out of " + iTotal + " products from your Order " + iOrderNum + " is successfully delivered today. Thank you for shopping & visit again.";
                            oMailEmail = dbContext.MailSMSSettings.Where(m => m.iDomainID == iDomainID).Where(m => m.sTemplateType == "MAIL").Where(m => m.sType == "Delivered").FirstOrDefault();
                            iStatus = 50;
                        }
                        else if (sStatus == "AssignToLogistics")
                        {
                            message = iPacked + " out of " + iTotal + " products from your Order " + iOrderNum + " is dispatched and will be delivered soon..";
                            oMailSMS = dbContext.MailSMSSettings.Where(m => m.iDomainID == iDomainID).Where(m => m.sTemplateType == "SMS").Where(m => m.sType == "AssignToLogistics").FirstOrDefault();

                            //sEmail = "Following item(s) from your recent order " + iOrderNum + " has been dispatched and will be delivered soon.";
                            sEmail = iPacked + " out of " + iTotal + " products from your Order " + iOrderNum + " is dispatched and will be delivered soon..";
                            oMailEmail = dbContext.MailSMSSettings.Where(m => m.iDomainID == iDomainID).Where(m => m.sTemplateType == "MAIL").Where(m => m.sType == "AssignToLogistics").FirstOrDefault();
                            iStatus = 40;
                            
                        }
                        var oSMSD = dbContext.MailSMSSettings.Where(m => m.iDomainID == iDomainID).Where(m => m.sTemplateType == "SMS").Where(m => m.sType == sStatus).FirstOrDefault();
                        if (oSMSD != null && oSMSD.SMS == true)
                        {
                            if (oMailSMS.sBody != null && oMailSMS.sBody != "" && oMailSMS.sBody.Contains("{{"))
                            {
                                string sMsg = oMailSMS.sBody;

                                sMsg = sMsg.Replace("{{P}}", iPacked.ToString());

                                sMsg = sMsg.Replace("{{T}}", iTotal.ToString());

                                sMsg = sMsg.Replace("{{O}}", iOrderNum.ToString());

                                message = sMsg;
                            }
                            Common.sendmessage(sPhoneNumber, message, oMailSMS.sdltid);
                        }
                        ProductsAndPriceController oPPC = new ProductsAndPriceController();
                        var Result = oPPC.GetOrderDetails_ByOrderId(Convert.ToInt32(iOrderNum));
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
                            var Email = result.Addressset[0]["Email"];
                            add.Address_Line_One = buyeraddress;
                            vmodal.EmailID = Email;
                            add.FullName = name;
                            add.MobileNumber = Number;
                            add.Pincode = Pin_Code;
                            vmodal.buyeraddress = add;
                            vmodal.Name = name;
                        }
                        vmodal.Status = iStatus;
                        vmodal.OrderID = iOrderNum;
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
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return 0;
            }
            return 0;
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

        [HttpPost]
        [Route("Get_All_PO_Inventory_Date_history")]
        public IHttpActionResult Get_All_PO_Inventory_Date_history(VMModelsForCategory gt)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Product_Id", gt.ProductIds }, { "@POID", gt.POID } };
                var res = _database.ProductListWithCount("iAdmin_Get_PO_Inventory_History", parameters);
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
        [Route("Get_PO_Details")]
        public IHttpActionResult Get_PO_Details(VMModelsForProduct gt)
        {
            try
            {
                string Stock_Moving = "[iB_Get_PO_Details]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@POID", gt.POID } };
                var Stock_Moving_ListNew = _database.ProductListWithCount(Stock_Moving, parameters);
                return Ok(Stock_Moving_ListNew);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }


        [HttpGet]
        [Route("ShowVPOInboundHistory")]
        public IHttpActionResult ShowVPOInboundHistory()
        {
            try
            {

                Dictionary<string, object> parameters = new Dictionary<string, object>() { };
                var Orders = _database.ProductListWithCount("iB_GET_InboundHistory", parameters);
                return Ok(Orders);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        [HttpPost]
        [Route("TotalGRNHistoryGrid")]
        public IHttpActionResult TotalGRNHistoryGrid(VMModelsForProduct gt)
        {
            try
            {
                string TotalInboundHistory = "[iB_GET_TotalInboundHistory]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@PoNo", gt.PoNo }, { "@InboundDate", gt.InboundDate } };
                var Orders = _database.ProductListWithCount(TotalInboundHistory, parameters);
                return Ok(Orders);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        //=== Method For Upload Cash Image For store ===//
        [HttpPost]
        [Route("Upload_Delivery_Invoice_Image")]
        public IHttpActionResult Upload_Delivery_Invoice_Image(int ImageVPOID)
        {
            try
            {
                var random = new Random(System.DateTime.Now.Millisecond);
                var randomNumber = random.Next(1, 5000000);
                HttpPostedFile file = HttpContext.Current.Request.Files[0];
                string physicalPath = HttpContext.Current.Server.MapPath("~/" + "Images/InboundImages/");
                if (!System.IO.File.Exists(physicalPath))
                {
                    Directory.CreateDirectory(physicalPath);
                }
                string ExteName = Path.GetExtension(file.FileName);
                string imgPath = "";
                //if (Type == 1)
                //{
                imgPath = HttpContext.Current.Server.MapPath("~/" + "Images/InboundImages/" + ImageVPOID + "-" + randomNumber + ".png");
                //}
                //else
                //{
                //    imgPath = HttpContext.Current.Server.MapPath("~/" + "Images/CounterFiles/CashCounterFiles/" + CounterID + ".png");
                //}

                file.SaveAs(imgPath);
                return Ok(ImageVPOID);
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
        [Route("GetGoodsReceviedPrintForm")]
        public IHttpActionResult GetGoodsReceviedPrintForm(int BatchNumber)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@BatchNumber", BatchNumber } };
                var Orders = _database.ProductListWithCount("iB_Get_GRN_Form_Details", parameters);
                return Ok(Orders);
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
                if (Purpose == "LandingPriceVerification")
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
                Common.sendmessage(PhoneNumber, "Enter OTP " + ReturnOTP + " to verify Landing Price ", "1007161613674758939");
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

       // API to get IMEI details of the Product while consignment creation
        [HttpPost]
        [Route("Map_Consignment_ProductIMEI_App")]
        public IHttpActionResult Map_Consignment_ProductIMEI_App(int OrderID, int ProductID, int InventoryProdID, String ScannedIMEI)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@OrderID", OrderID }, { "@ProductID", ProductID }, { "@InventoryProdID", InventoryProdID }, { "@ScannedIMEI", ScannedIMEI } };
                var res = _database.Query(string_MapConsignment_IMEI, parameters);
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
        [Route("GetStockAssignedMandalList")]
        public IHttpActionResult GetStockAssignedMandalList(int Previous_UnitID, string Type)
        {
            try
            {
                string GetStockAssignedMandals = "[iAdmin_Get_Stock_Assigned_Mandals_List]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                { "@UnitID", Previous_UnitID },{ "@Unit_Type", Type }
                };
                var Stock_Moving_ListNew = _database.ProductListWithCount(GetStockAssignedMandals, parameters);
                return Ok(Stock_Moving_ListNew);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("GetStockAssignedMandalList---Error---------", ex);
                return Ok(cs);
            }
        }

        

        //Method to get All Top Category List and Leaf Category List 
        [HttpPost]
        [Route("Get_Top_LeafCategoryList")]
        public IHttpActionResult Get_Top_LeafCategoryList(VMPagingResultsPost dt)
        {
            try
            {
                string Get_Top_LeafCategoryList = "[iAdmin_Get_Top_LeafCategoryList]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                { "@CategoryID", dt.CategoryID }
                };
                var resList = _database.GetMultipleResultsList2(Get_Top_LeafCategoryList, parameters);
                return Ok(resList);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("GetStockAssignedMandalList---Error---------", ex);
                return Ok(cs);
            }
        }

    }
}
