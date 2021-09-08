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

namespace iHubAdminAPI.Controllers
{
    //===================================== [Authorize]==========================
    [RoutePrefix("api/Coupons")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CouponsController : ApiController
    {
        string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private static readonly ILog log = LogManager.GetLogger(typeof(CouponsController).FullName);
        public SQLDatabase _database;
        

        public CouponsController()
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
        string Get_Raffel_Generated_Orders_List = "[iAdmin_Get_Raffel_Generated_Orders_List]";
        string Get_Coupon_Products_List = "[iAdmin_Coupon_Get_Products]";
        string Insert_Genrate_Coupan = "[iAdmin_Add_Coupons]";
        string ViewCouponDetails = "[iAdmin_View_Coupon_Details]";
        string ViewFreeProductDetails = "[iAdmin_View_Free_Gift_Products]";
        string Update_Status_Coupon = "[iAdmin_Update_Coupon_Status]";
        string Edit_Coupon_Details = "[iAdmin_Update_Coupon_Details]";
        string GetBuyers = "[iAdmin_Coupon_Buyer_Details]";
        string Insert_Genrate_Coupan_Multi = "[iAdmin_Add_Multi_Coupons]";
        string ViewCouponDetailsmore = "[iAdmin_View_Coupon_Details_More]";
        string ViewMultiCouponDetails = "[iAdmin_Multi_Coupon_Today_Records]";
        string ViewparentCouponDetails = "[iAdmin_Parent_Multi_Offer]";
        string ViewParentDetails = "[iAdmin_View_Parent_coupon]";
        string ViewParentsubDetails = "[iAdmin_View_Sub_Coupon_Details]";
        string ViewCouponparentDetailsmore = "[iAdmin_Parent_View_Coupon_Details_More]";
        string Update_Status_parent_Coupon = "[iAdmin_Update_Coupon_Parent_Status]";
        string Coupon_Generate_Systematic = "[GenerateMultiCoupons]";
        string CouponMainDetailsView = "[iAdmin_MainClass_View_Details]";
        #endregion "BEGIN of -- SP Names"

        #region "BEGIN of -- Coupons Management"

        //===============================Get Buyers List=================================
        [HttpPost]
        [Route("GetBuyersListByQurey")]
        public IHttpActionResult GetBuyersListByQurey(string Mobile_Number)
        {
            try
            {

                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@mobile_number", Mobile_Number } };
                var res = _database.ProductListWithCount(GetBuyers, parameters);
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
        //===================== Get Raffle Generated Orders List =================
        [HttpPost]
        [Route("GetRaffelGeneratedOrdersList")]
        public IHttpActionResult GetRaffelGeneratedOrdersList(Raffle vmodel)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@MobileNumber", vmodel.MobileNumber },
                                                                                            { "@OrderId", vmodel.OrderId },
                                                                                            { "@Order_DateFrom", vmodel.Order_DateFrom },
                                                                                            { "@Order_DateTo", vmodel.Order_DateTo },
                                                                                            { "@PageSize", vmodel.Page_Size },
                                                                                            { "@PageIndex", vmodel.Page_Index } };
                var res = _database.ProductListWithCount(Get_Raffel_Generated_Orders_List, parameters);
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

        //===================== Get free Products in coupon =================
        [HttpPost]
        [Route("GetProductsincoupon")]
        public IHttpActionResult GetProductsincoupon(Raffle vmodel)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Sku", vmodel.Product_id },
                                                                                             };
                var res = _database.ProductListWithCount(Get_Coupon_Products_List, parameters);
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

        // =================================Add insert Coupons====================================================
        [HttpPost]
        [Route("Insert_Coupon_Genrate")]
        public IHttpActionResult Insert_Coupon_Genrate(Coupons cmodel)
        {
            try
            {

                var User_Id = 0;
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Coupon_Name", cmodel.Name }, { "@Coupon_Code", cmodel.Code }, { "@Coupon_Description", cmodel.CouponDescription },{ "@Coupon_Type", cmodel.Type },{ "@Coupon_Value", cmodel.Value},{"@Coupon_Max_discount",cmodel.MaxOf},
                {"@Coupon_Min_Order_Of",cmodel.MinOf},{ "@Coupon_Numberoftimes",cmodel.NumberOfTimes} ,{ "@Coupon_FreeProductsNumberoftimes",cmodel.FreeProductsNumberoftimes},{ "@Coupon_ValidFrom",cmodel.ValidFrom},{ "@Coupon_ValidTo",cmodel.ValidTo},{ "@Coupon_BasedOnId",cmodel.CouponBasedOn},{ "@Coupon_ProductIDsNew",cmodel.ProductIDsNew},{ "@Coupon_CategoryIDs",cmodel.CategoryIDs},{ "@Coupon_BrandsIDS",cmodel.BrandsIDS},{ "@Coupon_CartValue",cmodel.CartValue},{ "@Coupon_FreeproductIDS",cmodel.FreeproductIDS},
                    { "@Coupon_CouponType",cmodel.CouponType},{ "@Coupon_Domains",cmodel.Domains},{ "@Coupon_UseStoreIDS",cmodel.StoreIDS},{ "@Coupon_Canbeuse",cmodel.IsApplied},{ "@UserID", Convert.ToInt32(User_Id) },{ "@Coupon_CustomerMobileNumber",cmodel.CustomerMobileNumber},{"@DomainID",cmodel.DomainID },{"@IsVisible",cmodel.IsVisible }  };
                var res = _database.ProductListWithCount(Insert_Genrate_Coupan, parameters);
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
        //// =================================Add multi  Coupons insert====================================================
        [HttpPost]
        [Route("Insert_Multi_Coupon_Genrate")]
        public IHttpActionResult Insert_Multi_Coupon_Genrate(Coupons cmodel)
        {
            try
            {

                var User_Id = 0;
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Coupon_Name", cmodel.Name }, { "@Coupon_Code", cmodel.Code }, { "@Coupon_Description", cmodel.CouponDescription },{ "@Coupon_Type", cmodel.Type },{ "@Coupon_Value", cmodel.Value},{"@Coupon_Max_discount",cmodel.MaxOf},
                {"@Coupon_Min_Order_Of",cmodel.MinOf},{ "@Coupon_Numberoftimes",cmodel.NumberOfTimes} ,{ "@Coupon_FreeProductsNumberoftimes",cmodel.FreeProductsNumberoftimes},{ "@CouponNoofTimes",cmodel.Couptimes},{ "@Coupon_BasedOnId",cmodel.CouponBasedOn},{ "@Coupon_ProductIDsNew",cmodel.ProductIDsNew},{ "@Coupon_CategoryIDs",cmodel.CategoryIDs},{ "@Coupon_BrandsIDS",cmodel.BrandsIDS},{ "@Coupon_CartValue",cmodel.CartValue},{ "@Coupon_FreeproductIDS",cmodel.FreeproductIDS},
                    { "@Coupon_CouponType",cmodel.CouponType},{ "@Coupon_Domains",cmodel.Domains},{ "@Coupon_UseStoreIDS",cmodel.StoreIDS},{ "@Coupon_Canbeuse",cmodel.IsApplied},{ "@UserID", Convert.ToInt32(User_Id) },{ "@Coupon_CustomerMobileNumber",cmodel.CustomerMobileNumber}};
                var res = _database.ProductListWithCount(Insert_Genrate_Coupan_Multi, parameters);
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


        // =================================View Coupon Details====================================================
        [HttpPost]
        [Route("View_Coupon_Details")]
        public IHttpActionResult View_Coupon_Details(Coupons cmodel)
        {
            try
            {
                if (cmodel.GroupId == 0)
                {
                    //var User_Id = 0;
                    Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@DateFrom", cmodel.ValidFrom }, { "@DateTo", cmodel.ValidTo },
                                                                                { "@Code",cmodel.Code} ,{ "@Name",cmodel.Name },{ "@CouponBasedon",cmodel.CouponBasedOn},{ "@Domain",cmodel.IhubCouponType},{ "@Type",cmodel.Type},{ "@UnitName",cmodel.StoreIDS},{ "@CouponGroup",cmodel.CouponGRoupChange},{"@Page_Size",cmodel.Pagesize } ,{"@Page_Index",cmodel.Page_Index }};

                    var res = _database.ProductListWithCount(ViewCouponDetails, parameters);
                    return Ok(res);
                }
                else
                {

                    Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@GroupID", cmodel.GroupId } };

                    var res = _database.ProductListWithCount(ViewParentsubDetails, parameters);
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

        // ================================= To View Free Product====================================================
        [HttpPost]
        [Route("FreeProductsView")]
        public IHttpActionResult FreeProductsView(Coupons cmodel)
        {
            try
            {

                //var User_Id = 0;
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Coupon_ID", cmodel.CouponID } };
                var res = _database.ProductListWithCount(ViewFreeProductDetails, parameters);
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

        //==========================To Update Coupon Status========================================
        [HttpPost]
        [Route("Update_Coupon_Status")]
        public IHttpActionResult Update_Coupon_Status(Coupons cmodel)
        {
            try
            {
                Dictionary<string, object> parameter = new Dictionary<string, object>() { { "@Coupon_ID", cmodel.CouponID }, { "@Coupon_Status", cmodel.StatusTypeID } };
                var res = _database.QueryValue(Update_Status_Coupon, parameter);
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
        [Route("Update_Coupon_IsVisible")]
        public IHttpActionResult Update_Coupon_IsVisible(Coupons cmodel)
        {
            try
            {
                string Update_IsVisible_Coupon = "[Grocery_Update_Coupon_IsVisibleStatus]";
                Dictionary<string, object> parameter = new Dictionary<string, object>() { { "@Coupon_ID", cmodel.CouponID }, { "@Coupon_IsVisibleStatus", cmodel.IsVisible } };
                var res = _database.QueryValue(Update_IsVisible_Coupon, parameter);
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
        //===============================To Update Parent Coupon Status==========================
        [HttpPost]
        [Route("Update_Coupon_Parent_Status")]
        public IHttpActionResult Update_Coupon_Parent_Status(Coupons cmodel)
        {
            try
            {
                Dictionary<string, object> parameter = new Dictionary<string, object>() { { "@Coupon_ID", cmodel.CouponID }, { "@Coupon_Status", cmodel.Status } };
                var res = _database.QueryValue(Update_Status_parent_Coupon, parameter);
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
        //========================To Edit Coupon Details==========================================
        [HttpPost]
        [Route("Update_Coupon_Details")]
        public IHttpActionResult Update_Coupon_Details(Coupons cmodel)
        {
            try
            {
                Dictionary<string, object> parameter = new Dictionary<string, object>() { { "@COUPONID", cmodel.CouponID }, { "@VALID_TO", cmodel.ValidTo }, { "@NUMBEROFTIMES", cmodel.NumberOfTimes }, { "@Domains", cmodel.Domains } };
                var res = _database.QueryValue(Edit_Coupon_Details, parameter);
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
        //=============================To View Coupon all Details===========================================
        [HttpPost]
        [Route("View_Coupon_Details_more")]
        public IHttpActionResult View_Coupon_Details_more(Coupons cmodel)
        {
            try
            {

                //var User_Id = 0;
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Coupon_ID", cmodel.CouponID }, { "@flag", cmodel.CouponBasedOn }, { "@DomainID", cmodel.DomainID } };

                var res = _database.ProductListWithCount(ViewCouponDetailsmore, parameters);
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

        //==========================To view Coupon Parent All Details ==============================
        [HttpPost]
        [Route("View_Coupon_parent_Details_more")]
        public IHttpActionResult View_Coupon_parent_Details_more(Coupons cmodel)
        {
            try
            {

                //var User_Id = 0;
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Coupon_ID", cmodel.CouponID }, { "@flag", cmodel.CouponBasedOn } };

                var res = _database.ProductListWithCount(ViewCouponparentDetailsmore, parameters);
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

        //==============================To view Multi Coupon Details=======================================
        [HttpGet]
        [Route("View_Multi_Coupon_Details")]
        public IHttpActionResult View_Multi_Coupon_Details(Coupons cmodel)
        {
            try
            {

                Dictionary<string, object> parameters = new Dictionary<string, object>() { };

                var res = _database.ProductListWithCount(ViewMultiCouponDetails, parameters);
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
        //====================To Insert Parent Coupon Generation====================================

        [HttpPost]
        [Route("Insert_Parent_Coupon_Genrate")]
        public IHttpActionResult Insert_Parent_Coupon_Genrate(Coupons cmodel)
        {
            try
            {

                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@MultiCouponName", cmodel.ClassName }, { "@Total_Coupons", cmodel.NoOfCoupons }, { "@GenerateMonthly", cmodel.GenerateCoupon },
                    { "@coupon_Ids",cmodel.CouponIDs },{ "@MultiCouponFrom", cmodel.ValidFrom }, { "@MultiCouponTo", cmodel.ValidTo } ,{ "@GenerateFor",cmodel.ParTFranch},{"@Franchiseid",cmodel.FranchiseID},{"@CorporateID",cmodel.CorporateID},{"@CouponType",cmodel.CouponType}

                };

                var res = _database.ProductListWithCount(ViewparentCouponDetails, parameters);
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
        //========================To View Parent Coupon=============================
        [HttpPost]
        [Route("View_Coupon_Parent")]
        public IHttpActionResult View_Coupon_Parent(Coupons cmodel)
        {
            try
            {

                //var User_Id = 0; 
                //Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@PageSize", cmodel.Pagesize }, { "@PageIndex", cmodel.Page_Index } };
                Dictionary<string, object> parameters = new Dictionary<string, object>() {{ "@Name", cmodel.Name },
                    { "@DateFrom", cmodel.ValidFrom }, { "@DateTo", cmodel.ValidTo }, { "@CouponType", cmodel.CouponType },
                    { "@Status", cmodel.StatusTypeID },{ "@Domain",cmodel.Domains},
                    { "@PageSize", cmodel.Pagesize }, { "@PageIndex", cmodel.Page_Index } };

                var res = _database.ProductListWithCount(ViewParentDetails, parameters);
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

        //=============================To generate Coupon Automatically=============================
        [HttpPost]
        [Route("CouponGenerateSystematic")]
        public IHttpActionResult CouponGenerateSystematic(Coupons cmodel)
        {
            try
            {

                //var User_Id = 0;
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@forID", cmodel.Group_Id } };

                var res = _database.ProductListWithCount(Coupon_Generate_Systematic, parameters);
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
        //====================To View Main Coupon Details=========================================

        [HttpPost]
        [Route("Coupon_Main_view_Details")]
        public IHttpActionResult Coupon_Main_view_Details(Coupons cmodel)
        {
            try
            {

                //var User_Id = 0;
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ID", cmodel.Group_Id } };

                var res = _database.ProductListWithCount(CouponMainDetailsView, parameters);
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

        #endregion "END of -- Coupons Management"
    }
}