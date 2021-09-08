using AspNet.Identity.SQLDatabase;
using iHubAdminAPI.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace iHubAdminAPI.Controllers.EwayBill
{
    [Authorize]
    [RoutePrefix("api/EwayBill")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EwayBillController : ApiController
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(EwayBillController).FullName);
        public SQLDatabase _database;
        public EwayBillController()
        {
            _database = new SQLDatabase();
        }
        iHubDBContext dbContext = new iHubDBContext();

        [HttpGet]
        [Route("GetLocationsById")]
        public IHttpActionResult GetLocationsById(int ParentID = 1)
        {
            try
            {
                var LcnByid = (from LC in dbContext.iH_Master_Locations.Where(PI => PI.ParentID == ParentID)
                               select new VmDropDown
                               {
                                   ID = LC.ID,
                                   Name = LC.Location_Name,
                                   ParentID=LC.ParentID
                               }).OrderBy(y => y.Name).ToList();
                return Ok(LcnByid);
            }
            catch (Exception ex)
            {
                CommonMethods.LogError(ex);
                log.Error("Error", ex);
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.NotFound, ex));
            }

        }
        [HttpGet]
        [Route("GetProductsTogenorateEwayBills")]
        public IHttpActionResult GetProductsTogenorateEwayBills(string Mandals)
        {
            try
            {
                string Get_All_Categories = "[iAdmin_EwayBill_BasedOnMandalIds]";
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    { "@Mandls", Mandals }
                };
                var Get_All_Categories_List = _database.GetMultipleResultsList(Get_All_Categories, parameters);
                return Ok(Get_All_Categories_List);
            }
            catch (Exception ex)
            {
                CommonMethods.LogError(ex);
                log.Error("Error", ex);
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.NotFound, ex));
            }

        }
        [HttpGet]
        [Route("GetEwayBillMandals")]
        public IHttpActionResult GetEwayBillMandals(int District_Id)
        {
            try
            {
                string Get_All_Categories = "[iAdmin_EwayBill_Mandals]";
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    { "@District_Id", District_Id }
                };
                var Get_All_Categories_List = _database.ProductListWithCount(Get_All_Categories, parameters).Resultset;
                return Ok(Get_All_Categories_List);
            }
            catch (Exception ex)
            {
                CommonMethods.LogError(ex);
                log.Error("Error", ex);
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.NotFound, ex));
            }

        }
        [HttpGet]
        [Route("UpdateEwayBillNumber")]
        public IHttpActionResult UpdateEwayBillNumber(string Order_Number, int Consignment_ID, string EwayBillNumber)
         {
            try
            {
                string Get_All_Categories = "[iAdmin_UpdateEwayBillNumber]";
                var User_Id = HttpContext.Current.User.Identity.GetUserID();
                var RoleName = HttpContext.Current.User.Identity.GetRoleName();
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    { "@Order_Number", Order_Number },
                    { "@EwayBillNumber", EwayBillNumber },
                    { "@Consignment_ID", Consignment_ID },
                    { "@User_Id", User_Id },
                    { "@RoleName", RoleName }
                };
                var Get_All_Categories_List = _database.Query(Get_All_Categories, parameters);
                return Ok(Get_All_Categories_List);
            }
            catch (Exception ex)
            {
                CommonMethods.LogError(ex);
                log.Error("Error", ex);
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.NotFound, ex));
            }

        }
        [HttpGet]
        [Route("GetUpdatedEwaybills")]
        public IHttpActionResult GetUpdatedEwaybills(string Mandals, string EwayBill_Number, int? Order_Number, string Consignment_Number)
        {
            try
            {
                string Get_All_Categories = "[iAdmin_updated_EwayBills]";
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"@Mandls",Mandals},
                    {"@EwayBill_Number",EwayBill_Number },
                    {"@Order_Number",Order_Number },
                    {"@Consignment_Number",Consignment_Number }
                };
                var Get_All_Categories_List = _database.GetMultipleResultsList(Get_All_Categories, parameters);
                return Ok(Get_All_Categories_List);
            }
            catch (Exception ex)
            {
                CommonMethods.LogError(ex);
                log.Error("Error", ex);
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.NotFound, ex));
            }

        }
        [HttpGet]
        [Route("BusinessGetInvoiceByOrderId")]
        public IHttpActionResult BusinessGetInvoiceByOrderId(int OrderId, string Invoice_Number, int Consignment_Id)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                    { "@orderNumber", OrderId },
                    { "@Invoice_Number",Invoice_Number},
                    {"@Consignment_Id",Consignment_Id } };
                var Orders = _database.ProductListWithCount("iAdmin_IB_Tax_Invoice_OrderNumberInvoice_Number", parameters);
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
        [HttpGet]
        [Route("BusinessGetInvoiceByOrderIdForOnline")]
        public IHttpActionResult BusinessGetInvoiceByOrderIdForOnline(int OrderId, string Invoice_Number)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                    { "@orderNumber", OrderId },
                    { "@Invoice_Number",Invoice_Number} };
                var Orders = _database.ProductListWithCount("IB_Tax_Invoice_OrderNumberInvoice_Number", parameters);
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

        [HttpGet]
        [Route("GenerateDeliveryChallan")]
        public IHttpActionResult GenorateDeliveryChallan(int Consign_ID,string Challan_No)
        {
            try
            {
                Challan_No = Challan_No == "" || Challan_No == "undefined" ? null : Challan_No;
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                    { "@Consignment_ID", Consign_ID },{ "@Challan_No", Challan_No } };
                var Orders = _database.Query("iAdmin_IB_Delivery_Challan", parameters);
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

        [HttpGet]
        [Route("GenerateDeliveryChallanForOnline")]
        public IHttpActionResult GenerateDeliveryChallanForOnline(int Consign_ID, string Challan_No)
        {
            try
            {
                Challan_No = Challan_No == "" || Challan_No == "undefined" ? null : Challan_No;
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                    { "@Consignment_ID", Consign_ID },{ "@Challan_No", Challan_No } };
                var Orders = _database.Query("IB_Delivery_Challan", parameters);
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
    }
}
