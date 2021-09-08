using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using iHubAdminAPI.Models;
using System.Web.Configuration;
using AspNet.Identity.SQLDatabase;
using System.Configuration;
using iHubAdminAPI.Models.ProductAndPrice;
using System.Data.SqlClient;

namespace iHubAdminAPI.Controllers
{
    [RoutePrefix("api/Jobrun")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class JobrunController : ApiController
    {
        string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private static readonly ILog log = LogManager.GetLogger(typeof(JobrunController).FullName);
        public SQLDatabase _database;
        private ApplicationUserManager _userManager;
        iHubDBContext dbContext = new iHubDBContext();
        private class CustomResponse
        {
            internal string Response;
            internal int ResponseID;
        }
        public JobrunController()
        {
            _database = new SQLDatabase();
        }
     
        #region "BEGIN of -- Jobrun"

        //----------------------------------Update Search Json file in Store Side-------------------------------------------------------//
        [AllowAnonymous]
        [HttpGet]
        [Route("UpdateSearchJsonfile")]
        public IHttpActionResult UpdateSearchJsonfile()
        {
            try
            {
                string targetPath = WebConfigurationManager.AppSettings["JobRun"] + "\\autocompletesearchresults.js";
                log.Info(targetPath);
                var list = new List<string>();
                string[] firstline = new string[] { " var AutoCompleteData = [" };
                string[] lastline = new string[] { "];" };
                var categories = dbContext.Database.SqlQuery<string>("[iAdmin_Update_categoryNames_Js_File]");
                var products = dbContext.Database.SqlQuery<string>("[iAdmin_Update_ProductNames_Js_File]");
                list.AddRange(firstline);
                list.AddRange(categories);
                list.AddRange(products);
                list.AddRange(lastline);
                System.IO.File.WriteAllLines(targetPath, list);
                log.Info("Suggession file updated successully on " + DateTime.Now.ToString());
                return Ok("Suggession file updated successully on " + DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                log.Error("Error in Update search suggession file..", ex);
                return Ok(ex.Message);
            }
        }

        //----------------------------------Update Search Json file in Direct Side------------------------------------------------------//
        [AllowAnonymous]
        [HttpGet]
        [Route("UpdateSearchJsonfile_Direct")]
        public IHttpActionResult UpdateSearchJsonfile_Direct(int SiteID)
        {
            try
            {
                string targetPath = WebConfigurationManager.AppSettings["JobRun"] + "\\autocompletesearchresults.js";
                string[] firstline = new string[] { " var AutoCompleteData = [" };


                if (SiteID == 1)
                {
                    var DomainDetails = dbContext.iHub_Domains.Where(x => x.Site_ID == SiteID).FirstOrDefault();
                    targetPath = WebConfigurationManager.AppSettings["JobRun_Direct"] + "\\" + DomainDetails.Search_FileName + ".js";
                    firstline = new string[] { " var "+DomainDetails.SearchKeyword+ " = [" };
                }
                if (SiteID > 3)
                {
                    var DomainDetails = dbContext.iHub_Domains.Where(x => x.Site_ID == SiteID).FirstOrDefault();
                    targetPath = WebConfigurationManager.AppSettings["JobRun_Grocery"] + "\\" + DomainDetails.Search_FileName + ".js";
                    firstline = new string[] { " var " + DomainDetails.SearchKeyword + " = [" };
                }
                log.Info(targetPath);
                var list = new List<string>();
               
                string[] lastline = new string[] { "];" };
                
                        var categories = dbContext.Database.SqlQuery<string>("[iAdmin_Update_categoryNames_Js_File] @SiteID", new SqlParameter("SiteID", SiteID)).ToList().ToArray();
                        var products = dbContext.Database.SqlQuery<string>("[iAdmin_Update_ProductNames_Js_File] @SiteID", new SqlParameter("SiteID", SiteID)).ToList().ToArray();
                        list.AddRange(firstline);
                        list.AddRange(categories);
                        list.AddRange(products);
                        list.AddRange(lastline);
                  
                System.IO.File.WriteAllLines(targetPath, list);
                log.Info("Suggession file updated successully on " + DateTime.Now.ToString());
                return Ok("Suggession file updated successully on " + DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                log.Error("Error in Update search suggession file..", ex);
                return Ok(ex.Message);
            }
        }

        //----------------------------------Update MegaDeals Job Run-------------------------------------------------------------------//
        [AllowAnonymous]
        [HttpGet]
        [Route("MegaDeals_Job_Run")]
        public IHttpActionResult MegaDeals_Job_Run(VMPagingResultsPost vmodel)
        {
            try
            {
                string cmdText = "[MegaDeals_Job_Run]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { };
                var res = _database.QueryValue(cmdText, parameters);
                log.Info("MegaDeals Prices Updated successully on " + DateTime.Now.ToString());
                return Ok("MegaDeals Prices Updated successully on " + DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                log.Error("Error in Update MegaDeals Prices..", ex);
                return Ok(ex.Message);
            }
        }
        //----------------------------------Update Full Text Search Job Run-------------------------------------------------------------------//
        [AllowAnonymous]
        [HttpGet]
        [Route("FTS_Job_Run")]
        public IHttpActionResult FTS_Job_Run(VMPagingResultsPost vmodel)
        {
            try
            {
                string FtsProducts = "[iS_InsertProductstoFTS]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { };
                var res = _database.QueryValue(FtsProducts, parameters);
                log.Info("Universal Search Job Run Successully Done on " + DateTime.Now.ToString());
                return Ok("Universal Search Job Run Successully Done on " + DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                log.Error("Error in Universal Search..", ex);
                return Ok(ex.Message);
            }
        }

        //----------------------------------Update Full Text Search Job Run-------------------------------------------------------------------//
        [AllowAnonymous]
        [HttpGet]
        [Route("Cash_report_Jobrun")]
        public IHttpActionResult Cash_report_Jobrun(string Startdate, string Enddate, int Unit_id)
        {
            try
            {

                string FtsProducts = "[iAdmin_Monthly_And_Cash_Reports]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@StartFromDate", Startdate }, { "@EndfromDate", Enddate }, { "@Unit_ID", Unit_id } };
                var res = _database.QueryValue(FtsProducts, parameters);
                log.Info("Monthly Cash Reports Job Run Successully Done on " + DateTime.Now.ToString());
                return Ok("Monthly Cash Reports Job Run Successully Done on " + DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                log.Error("Error in Universal Search..", ex);
                return Ok(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("Cash_Daily_report_Jobrun")]
        public IHttpActionResult Cash_Daily_report_Jobrun(string Startdate, string Enddate, int Unit_id)
        {
            try
            {

                string FtsProducts = "[iAdmin_Daily_And_Cash_Reports]";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@StartFromDate", Startdate }, { "@EndfromDate", Enddate }, { "@Unit_ID", Unit_id } };
                var res = _database.QueryValue(FtsProducts, parameters);
                log.Info("Daily Cash Reports Job Run Successully Done on " + DateTime.Now.ToString());
                return Ok("Daily Cash Reports Job Run Successully Done on " + DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                log.Error("Error in Universal Search..", ex);
                return Ok(ex.Message);
            }
        }
        #endregion "END of -- Jobrun"
    }
}