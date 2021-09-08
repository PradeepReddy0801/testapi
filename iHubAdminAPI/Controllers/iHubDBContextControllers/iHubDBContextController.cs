using log4net;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using iHubAdminAPI.Models;
using System.Configuration;
using AspNet.Identity.SQLDatabase;
using System.Web;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Data;
using Excel;
using System.Web.Script.Serialization;
using iHubAdminAPI.Models.ProductAndPrice;
using NPOI.HSSF.Model; // InternalWorkbook
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel; // HSSFWorkbook, HSSFSheet
using System.Text;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;

namespace iHubAdminAPI.Controllers
{
    [RoutePrefix("api/iHubDBContext")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class iHubDBContextController : ApiController
    {
        string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private static readonly ILog log = LogManager.GetLogger(typeof(iHubDBContextController).FullName);
        public SQLDatabase _database;

        iHubDBContext dbContext = new iHubDBContext();
        private class CustomResponse
        {
            internal string Response;
            internal int ResponseID;
        }
        public iHubDBContextController()
        {
            _database = new SQLDatabase();
        }

        string Upload_New_Products = "[iAdmin_Upload_Products_With_Selected_Category]";

        //==================================================== Get GetLocationPath
        [HttpGet]
        [Route("GetLocationPath")]
        public IHttpActionResult GetLocationPath(int? ID)
        {
            List<CatNamesList> CatNamesLists = new List<CatNamesList>();
            try
            {
                if (ID == 0)
                {
                    CatNamesLists.Add(new CatNamesList() { Name = "State", ID = 0 });
                }
                else
                {
                label:
                    var CategoryPath = dbContext.iH_Master_Locations.Find(ID);
                    int CategoryParentID = Convert.ToInt32(CategoryPath.Location_ParentID);
                    CatNamesLists.Add(new CatNamesList() { Name = CategoryPath.Location_Name, ID = CategoryPath.Location_ID, ParentID = Convert.ToInt32(CategoryPath.IsLeaf) });

                    if (CategoryParentID == 0)
                    {
                        CatNamesLists.Reverse();

                    }
                    else
                    {
                        ID = CategoryParentID;
                        goto label;
                    }
                }
                return Ok(CatNamesLists);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("GetLocationPath--Error---------", ex);
                return Ok(cs);
            }
        }

        // =================================To Get Products By Grooup ID====================================
        [HttpPost]
        [Route("Get_Products_By_Grooup_ID")]
        public IHttpActionResult Get_Products_By_Grooup_ID(int ID)
        {
            try

            {
                if (ID > 0)
                {
                    var Products = dbContext.iHub_Products.Where(m => m.Group_ID == ID).ToList();
                    return Ok(Products);
                }
                else
                {
                    return Ok("Invalid Group ID");
                }
            }
            catch (Exception ex)
            {
                log.Error("GetAllCategories--Error---------", ex);
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                return Ok(cs);
            }
        }

        //=============================== UpdateImagecode=================================
        [HttpPost]
        [Route("UpdateImagecode")]
        public IHttpActionResult UpdateImagecode(int productid, string imagecode)
        {
            try
            {
                dbContext.iHub_Products.First(x => x.iHub_Product_ID == productid).Image_Code = imagecode;

                var res = dbContext.SaveChanges();
                string fileNameExst;
                HttpPostedFile fileExst = null;
                var data = HttpContext.Current.Request.Form[0];
                List<object> obj = JsonConvert.DeserializeObject<List<object>>(data);
                var filecount = HttpContext.Current.Request.Files.Count;
                for (int i = 0; i < filecount; i++)
                {
                    fileExst = HttpContext.Current.Request.Files[i];
                    fileNameExst = fileExst.FileName + "_" + productid.ToString();
                    if (fileExst.FileName != "filename")
                    {
                        string physicalPath = HttpContext.Current.Server.MapPath("~/" + "Images/ProductImages/" + imagecode);
                        if (!System.IO.File.Exists(physicalPath))
                        {
                            Directory.CreateDirectory(physicalPath);
                        }
                        string imgPath = HttpContext.Current.Server.MapPath("~/" + "Images/ProductImages/" + imagecode + "/" + (i + 1) + ".jpg");
                        fileExst.SaveAs(imgPath);
                    }
                }

                return Ok(res);
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

        //===================>>>>  To Check Valid HSN or Not <<<<======================//
        public List<Dictionary<string, string>> CreateInvalidHSNs(DataTable dt)
        {

            var sfdf = dt.AsEnumerable().Select(
       row => dt.Columns.Cast<DataColumn>().ToDictionary(
           column => column.ColumnName,
           column => row[column].ToString()
       )).ToList();

            return sfdf;
        }

        //=============================== UploadNewProductsForEditDetails=================================
        [HttpPost]
        [Route("UploadNewProducts")]
        public IHttpActionResult UploadNewProducts()
        {
            try
            {
                CustomResponse cs = new CustomResponse();
                List<string> resss = new List<string>();
                var res1 = "";
                var categorydata = HttpContext.Current.Request.Form[0];
                VMModelsForCategory category = JsonConvert.DeserializeObject<VMModelsForCategory>(categorydata);
                HttpPostedFile file = HttpContext.Current.Request.Files[0];
                var Dictionary = new List<string>();
                string str = "";
                IExcelDataReader reader = null;
                List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();
                Dictionary<string, string> product;
                var random = new Random(System.DateTime.Now.Millisecond);
                var randomNumber = random.Next(1, 5000000);
                var bIsMRPSelling = true;
                var bIsSellLanding = true;
                string iIndexOut = string.Empty;
                string sResponse = string.Empty;
                using (var stream = file.InputStream)
                {
                    if (file.FileName.EndsWith(".xls"))
                    {
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
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

                    //Code for Validating the MRP, SellingPrice and LandingPrice From Uploaded Excel Sheet
                    var oData = data.Rows[0];
                    var iMrpIndex = 0;
                    var iSPIndex = 0;
                    var iLPIndex = 0;
                    var results = oData.Table.Select().Select(dr => dr.ItemArray.Select(x => x.ToString()).ToArray()).ToList();
                    var oNewTotal = results.ToArray().Skip(1).ToList(); //Removing the Header Index i.e, 0 Index.
                    var oTotalIndex = results[0].ToList();
                    int q = 0;
                    foreach (var SingleIn in oTotalIndex)
                    {
                        if (SingleIn.ToLower() == "mrp")
                        {
                            iMrpIndex = q;
                        }
                        else if (SingleIn.ToLower() == "sellingprice")
                        {
                            iSPIndex = q;
                        }
                        else if (SingleIn.ToLower() == "landingpriceincgst")
                        {
                            iLPIndex = q;
                        }
                        q++;
                    }
                    var oMRPList = new List<string>();
                    var oSellingList = new List<string>();
                    var oLandingList = new List<string>();
                    foreach (var list in oNewTotal)
                    {
                        oMRPList.Add(list[iMrpIndex]);
                        oSellingList.Add(list[iSPIndex]);
                        if (iLPIndex != 0)
                        {
                            oLandingList.Add(list[iLPIndex]);
                        }
                    }
                    int u = 0;
                    foreach (var isd in oMRPList)
                    {
                        decimal a = 0;
                        decimal b = 0;
                        if (isd != "")
                        {
                            a = Convert.ToDecimal(isd);
                            b = Convert.ToDecimal(oSellingList[u]);
                        }
                        if (a == b || a > b)
                        {
                            if (bIsMRPSelling != false)
                            {
                                bIsMRPSelling = true;
                            }
                        }
                        else
                        {
                            sResponse = "You have Entered Selling Price greater than MRP in the following Rows at - ";
                            int y = 2;
                            if (!string.IsNullOrEmpty(iIndexOut))
                            {
                                y = y + u;
                                iIndexOut = iIndexOut + ", " + y;
                            }
                            else
                            {
                                iIndexOut = y.ToString();
                            }
                            bIsMRPSelling = false;
                        }
                        u++;
                    }
                    if (bIsMRPSelling == true)
                    {
                        int p = 0;
                        int C = 2;
                        foreach (var oLL in oLandingList)
                        {
                            decimal a = 0;
                            decimal b = 0;
                            //decimal c = 0;
                            if (oLL != " " && oLL != "")
                            {
                                a = Convert.ToDecimal(oLL);
                                b = Convert.ToDecimal(oMRPList[p]);
                                //c = Convert.ToDecimal(oSellingList[p]);
                            }
                            //if ((a == b || a <= b) && (a == c || a <= c))
                            if (a == b || a <= b)
                            {
                                if (bIsSellLanding != false)
                                {
                                    bIsSellLanding = true;
                                }
                            }
                            else
                            {
                                //sResponse = "You have Entered Landing Price Less than MRP or Selling Price in the following Rows at - ";
                                sResponse = "You have Entered Landing Price Less than MRP in the following Rows at - ";
                                if (!string.IsNullOrEmpty(iIndexOut))
                                {
                                    iIndexOut = iIndexOut + ", " + C.ToString();
                                }
                                else
                                {
                                    iIndexOut = C.ToString();
                                }
                                bIsSellLanding = false;
                            }
                            p++;
                            C++;
                        }
                    }

                    if (bIsMRPSelling == true && bIsSellLanding == true)
                    {
                        JavaScriptSerializer serializer = new JavaScriptSerializer();

                        foreach (var column in data.Columns.Cast<DataColumn>().ToArray())
                        {
                            if (data.AsEnumerable().All(dr => dr.IsNull(column)))
                                data.Columns.Remove(column);
                        }

                        data = data.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is System.DBNull ||
                              string.Compare((field as string).Trim(), string.Empty) == 0)).CopyToDataTable();

                        var valresult = Common.validateexcelfile("UploadProducts", "excel", data);
                        if (valresult == "True")
                        {
                            var excelColdata = new List<string>();
                            List<int> ColumnIndexes = new List<int>();
                            List<List<string>> sListOfRecords = new List<List<string>>();
                            int i = 1;
                            bool Colomnrequired = false;
                            bool ColomnrequiredStatus = false;
                            bool ColomnrequiredProductName = false;
                            bool ColomnrequiredMRP = false;
                            bool ColomnrequiredSP = false;
                            bool ColomnrequiredBP = false;
                            bool ColomnrequiredHSN = false;
                            bool ColomnrequiredProductSeries = false;
                            foreach (var column in data.Rows[0].ItemArray.Where(m => m is string))
                            {
                                if (column.ToString().Trim() == "Is_Repurchasable")
                                {
                                    Colomnrequired = true;
                                }
                                if (column.ToString().Trim() == "Status")
                                {
                                    ColomnrequiredStatus = true;
                                }
                                if (column.ToString().Trim() == "ProductName")
                                {
                                    ColomnrequiredProductName = true;
                                }
                                if (column.ToString().Trim() == "MRP")
                                {
                                    ColomnrequiredMRP = true;
                                }
                                if (column.ToString().Trim() == "SellingPrice")
                                {
                                    ColomnrequiredSP = true;
                                }
                                if (column.ToString().Trim() == "Booking_Percentage")
                                {
                                    ColomnrequiredBP = true;
                                }
                                if (column.ToString().Trim() == "HSN_Code")
                                {
                                    ColomnrequiredHSN = true;
                                }
                                if (column.ToString().Trim() == "ProductSeries")
                                {
                                    ColomnrequiredProductSeries = true;
                                }
                                string ColumnName = column.ToString();
                                ColumnName.Replace("'", " ");
                                Dictionary.Add(ColumnName);
                            }
                            string HtmlContent = "";
                            if (Colomnrequired == false)
                            {
                                return Ok("Repurchasable Coloumn required");
                            }
                            if (ColomnrequiredStatus == false)
                            {
                                return Ok("Status Coloumn required");
                            }
                            if (ColomnrequiredProductName == false)
                            {
                                return Ok("ProductName Coloumn required");
                            }
                            if (ColomnrequiredMRP == false)
                            {
                                return Ok("MRP Coloumn required");
                            }
                            if (ColomnrequiredSP == false)
                            {
                                return Ok("SellingPrice Coloumn required");
                            }
                            if (ColomnrequiredBP == false)
                            {
                                return Ok("BookingPercentage Coloumn required");
                            }
                            if (ColomnrequiredHSN == false)
                            {
                                return Ok("HSN Code Coloumn required");
                            }
                            if (ColomnrequiredProductSeries == false)
                            {
                                return Ok("Product Series Coloumn required");
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
                                            if (sListOfRecords[0][j - 1] == "")
                                            {
                                            }
                                            excelRowdata.Add(item.ToString());
                                            string itemname = item.ToString();
                                            if (sListOfRecords[0][j - 1] == "Price")
                                            {
                                                Price = Convert.ToInt32(itemname);
                                            }
                                            if (sListOfRecords[0][j - 1].ToString().Trim() == "Is_Repurchasable")
                                            {
                                                if (itemname == "" || (itemname != "Active" && itemname != "InActive"))
                                                {
                                                    cs.Response = "Invalid Repurchasable Value";
                                                    cs.ResponseID = Convert.ToInt32(13);
                                                    resss.Clear();
                                                    flag = true;
                                                    break;
                                                }
                                            }
                                            if (sListOfRecords[0][j - 1].ToString().Trim() == "Status")
                                            {
                                                if (itemname == "" || (itemname != "Active" && itemname != "InActive" && itemname != "New"))
                                                {
                                                    cs.Response = "Invalid Status Value";
                                                    cs.ResponseID = Convert.ToInt32(14);
                                                    resss.Clear();
                                                    flag = true;
                                                    break;
                                                }
                                            }
                                            if (sListOfRecords[0][j - 1].ToString().Trim() == "MRP")
                                            {
                                                if (itemname == "")
                                                {
                                                    cs.Response = "Invalid MRP Value";
                                                    cs.ResponseID = Convert.ToInt32(15);
                                                    resss.Clear();
                                                    flag = true;
                                                    break;
                                                }
                                            }
                                            if (sListOfRecords[0][j - 1].ToString().Trim() == "SellingPrice")
                                            {
                                                if (itemname == "")
                                                {
                                                    cs.Response = "Invalid SellingPrice Value";
                                                    cs.ResponseID = Convert.ToInt32(16);
                                                    resss.Clear();
                                                    flag = true;
                                                    break;
                                                }
                                            }
                                            if (sListOfRecords[0][j - 1].ToString().Trim() == "HSN_Code")
                                            {
                                                if (itemname == "")
                                                {
                                                    cs.Response = "Invalid HSN Code Value";
                                                    cs.ResponseID = Convert.ToInt32(16);
                                                    resss.Clear();
                                                    flag = true;
                                                    break;
                                                }
                                            }
                                            if (sListOfRecords[0][j - 1].ToString().Trim() == "Booking_Percentage")
                                            {
                                                if (itemname == "")
                                                {
                                                    cs.Response = "Invalid Booking Percentage Value";
                                                    cs.ResponseID = Convert.ToInt32(16);
                                                    resss.Clear();
                                                    flag = true;
                                                    break;
                                                }
                                            }
                                            if (sListOfRecords[0][j - 1].ToString().Trim() == "ProductName")
                                            {
                                                if (itemname == "")
                                                {
                                                    cs.Response = "Invalid Product Name Value";
                                                    cs.ResponseID = Convert.ToInt32(16);
                                                    resss.Clear();
                                                    flag = true;
                                                    break;
                                                }
                                            }
                                            if (sListOfRecords[0][j - 1].ToString().Trim() == "ProductSeries")
                                            {
                                                if (itemname == "")
                                                {
                                                    cs.Response = "Invalid ProductSeries Value";
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
                                            if (sListOfRecords[0][j - 1].ToString() == "Status")
                                            {
                                                if (itemname.ToString() == "Active")
                                                {
                                                    itemname = "10";
                                                }
                                                else if (itemname.ToString() == "InActive")
                                                {
                                                    itemname = "20";
                                                }
                                                else if (itemname.ToString() == "New")
                                                {
                                                    itemname = "0";
                                                }
                                            }
                                            if (sListOfRecords[0][j - 1].ToString() == "Is_Repurchasable")
                                            {
                                                if (itemname.ToString() == "Active")
                                                {
                                                    itemname = "1";
                                                }
                                                else if (itemname.ToString() == "InActive")
                                                {
                                                    itemname = "0";
                                                }
                                            }
                                            if (sListOfRecords[0][j - 1].ToString() == "HasBarcode")
                                            {
                                                if (itemname.ToLower().ToString() == "true" || itemname.ToString() == "1")
                                                {
                                                    itemname = "1";
                                                }
                                                else if (itemname.ToLower().ToString() == "false" || itemname.ToString() == "0")
                                                {
                                                    itemname = "0";
                                                }
                                            }
                                            if (sListOfRecords[0][j - 1].ToString() == "HtmlContent")
                                            {
                                                HtmlContent = itemname;
                                            }
                                            product.Add(sListOfRecords[0][j - 1].TrimStart().TrimEnd(), itemname.ToString().TrimStart().TrimEnd());
                                        }
                                        if (flag) break;
                                        j++;
                                    }
                                    if (flag) break;
                                    var Newcat = dbContext.iH_Categories.Find(category.ID);
                                    var bookingpercentage = Convert.ToInt32(Newcat.BookingPercentage);
                                    product.Add("BookingPercentage", bookingpercentage.ToString());
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
                                JavaScriptSerializer jss = new JavaScriptSerializer();

                                string output = jss.Serialize(resss);

                                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@User_ID", 1},
                                                                                           { "@RF_Number", randomNumber},
                                                                                           { "@IP", "205.122.23322" },
                                                                                           { "@CategoryID", category.ID },
                                                                                           { "@Json_Data", output }};
                                var Upload_Products = _database.ProductListWithCount(Upload_New_Products, parameters);

                                string commandText2 = "[iAdmin_Get_Products_By_Random_Number]";
                                List<Dictionary<string, string>> resultset = new List<Dictionary<string, string>>();
                                Dictionary<string, object> parameters2 = new Dictionary<string, object>() { { "@Randomnumber", randomNumber.ToString() } };
                                var response2 = _database.ProductListWithCount(commandText2, parameters2);
                                response2.Addressset = Upload_Products.Resultset;
                                return Ok(response2);
                            }
                            else
                            {
                                return Ok("Invalid Values Entered");
                            }
                        }
                        else
                        {
                            return Ok(valresult);
                        }
                    }
                    else
                    {
                        cs.Response = sResponse + iIndexOut;
                        return Ok(cs.Response);
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
            return null;
        }
        //===========================================To Remove Discounted Products======================================================
        [HttpPost]
        [Route("RemoveDiscountedProducts")]
        public IHttpActionResult RemoveDiscountedProducts(int ItemID)
        {
            try
            {
                iHub_MegaDeals Item = dbContext.iHub_MegaDeals.Find(ItemID);
                int ID = Item.Product_ID;
                iHub_Products Product = dbContext.iHub_Products.Find(ID);
                dbContext.iHub_MegaDeals.Remove(Item);
                Product.Offer_Price = 0;
                dbContext.SaveChanges();
                return Ok(1);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Ok(ex);
            }
        }
        //===================== GET FMCG List =================
        [HttpGet]
        [Route("GetFMCGList")]
        public IHttpActionResult GetFMCGList()
        {
            try
            {
                var result = dbContext.iH_Categories.Where(m => m.Top_Category_Id == 18 && m.ParentID != 18).ToList();
                Dictionary<string, object> parameters = new Dictionary<string, object>() { };
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

        // ===================== GET DC List =================
        [HttpGet]
        [Route("GetDCList")]
        public List<iHub_Units_DC_WH_ST> GetDCList()
        {
            try
            {
                var sdfds = dbContext.iHub_Units_DC_WH_ST.Where(m => m.Unit_Hierarchy_Level == 1 && m.iHub_Unit_ID == 1).ToList();
                return sdfds;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        [HttpGet]
        [Route("Get_Corporates_List")]
        public IHttpActionResult Get_Corporates_List()
        {
            try
            {

                string cmd = "SELECT* FROM iHub_Corporate_Companies where Company_Status = 10";
                Dictionary<string, object> parameters = new Dictionary<string, object>() { };
                var Result = _database.SelectQuery(cmd, parameters);
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

        //==============================Get hierarchy Of Ware House and store=================================================
        [HttpGet]
        [Route("Get_WH_And_Stores")]
        public IHttpActionResult Get_WH_And_Stores()
        {
            try
            {
                var NEW = dbContext.iHub_Units_DC_WH_ST.Where(x => x.Unit_Hierarchy_Level == 3 || x.Unit_Hierarchy_Level == 2).ToList();
                return Ok(NEW);
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
        [Route("Get_DC_WH_And_Stores")]
        public IHttpActionResult Get_DC_WH_And_Stores()
        {
            try
            {
                var NEW = dbContext.iHub_Units_DC_WH_ST.Where(x => x.Unit_Hierarchy_Level == 3 || x.Unit_Hierarchy_Level == 2 || x.Unit_Hierarchy_Level == 1).ToList();
                return Ok(NEW);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("Error---------", ex);
                return Ok(cs);
            }
        }

        // =================================To GetCategoryPath=========================================
        [HttpGet]
        [Route("GetCategoryPath")]
        public IHttpActionResult GetCategoryPath(int? ID)
        {
            List<CatNamesList> CatNamesLists = new List<CatNamesList>();
            try
            {
                if (ID == 0)
                {
                    CatNamesLists.Add(new CatNamesList() { Name = "Categories", ID = 0 });
                }
                else
                {
                label:
                    var CategoryPath = dbContext.iH_Categories.Find(ID);
                    int CategoryParentID = Convert.ToInt32(CategoryPath.ParentID);
                    CatNamesLists.Add(new CatNamesList() { Name = CategoryPath.CategoryName, ID = Convert.ToInt32(CategoryPath.ID), ParentID = Convert.ToInt32(CategoryPath.Is_LeafNode) });

                    if (CategoryParentID == 0)
                    {
                        CatNamesLists.Reverse();
                    }
                    else
                    {
                        ID = CategoryParentID;
                        goto label;
                    }
                }
                return Ok(CatNamesLists);
            }
            catch (Exception ex)
            {
                CustomResponse cs = new CustomResponse();
                cs.Response = ex.Message;
                log.Error("GetCategoryPath--Error---------", ex);
                return Ok(cs);
            }
        }



        //===========================To Create Coupon==========================================================================
        [HttpPost]
        [Route("CreateCoupon")]
        public IHttpActionResult CreateCoupon(Coupons model)  //StoreID = 10000  then Coupon eligable for all stores
        {
            try
            {

                model.ItemIDs = model.ItemIDs == null ? "" : model.ItemIDs;
                model.StatusTypeID = 0;
                model.CreatedByID = model.UpdatedByID = 1;
                model.CreatedOn = model.UpdatedOn = DateTime.Now;
                var Jsonstore = "{\"CouponCode\":\"" + model.Code + "\",\"CouponAmount\":\"" + model.Value + "\",\"CustomerMobileNumber\":" + model.CustomerMobileNumber + ",\"StoreID\":" + model.StoreID + ",\"ValidFrom\":\"" + model.ValidFrom + "\",\"ValidTo\":\"" + model.ValidTo + "\",\"OrderDate\":\"" + System.DateTime.Now.ToString() + "\"}";
                string CouponCreateNotification = "Coupon Code :" + model.Code + ",Coupon Amount : " + model.Value + ", is assigned to " + model.CustomerMobileNumber + " in your store and it is only valid from " + model.ValidFrom + "to " + model.ValidTo;
                if (!model.IsProvider)
                {
                    model.ProviderID = 0;
                }
                if (!model.UserSpecifyWithoutUser)
                {
                    try
                    {
                        dbContext.Coupons.Add(model);
                    }
                    catch (Exception ex)
                    {
                        return Ok(ex.Message);
                    }
                    dbContext.SaveChanges();
                    if (!string.IsNullOrEmpty(model.UserIDs) && model.IsUser)
                    {
                        List<int> lstUserIDs = model.UserIDs.Split(',').Select(int.Parse).ToList();
                        foreach (var item in lstUserIDs)
                        {
                            Coupon_User_Validations user = new Coupon_User_Validations();
                            user.CouponID = model.CouponID;
                            user.UserID = item;
                            user.NumberOfTimesUsed = 0;
                            dbContext.Coupon_User_Validations.Add(user);
                        }
                        dbContext.SaveChanges();
                    }
                }
                else
                {
                    for (int k = 0; k < model.NumberOfCoupons; k++)
                    {
                        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                        var stringChars = new char[10];
                        var random = new Random();
                        for (int i = 0; i < stringChars.Length; i++)
                        {
                            stringChars[i] = chars[random.Next(chars.Length)];
                        }
                        model.Code = new String(stringChars);
                        dbContext.Coupons.Add(model);
                        dbContext.SaveChanges();
                        Coupon_User_Validations user = new Coupon_User_Validations();
                        user.CouponID = model.CouponID;
                        user.UserID = -1;
                        user.NumberOfTimesUsed = 0;
                        dbContext.Coupon_User_Validations.Add(user);
                        dbContext.SaveChanges();
                    }
                }
                return Ok("CreateCoupon");
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        //============================================To Check Coupon Code==============================================================
        [HttpPost]
        [Route("CheckCouponCode")]
        public IHttpActionResult CheckCouponCode(string Couponcode)
        {
            try
            {
                string CheckCoupon = "Select count(*) AS CouponCount from CM_Coupons where Code = " + "'" + Couponcode + "'";
                Dictionary<string, object> parameter = new Dictionary<string, object> { };
                var CouponUserValidations = _database.SelectQuery(CheckCoupon, parameter);
                //Coupon.Couponnumberofvalidation = int.Parse(CouponUserValidations[0]["NumberOfTimesUsed"]);
                //var a = dbContext.Coupons.Where(x => x.Code == Couponcode).ToList();
                if (int.Parse(CouponUserValidations[0]["CouponCount"]) != 0)
                {
                    return Ok(1);
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
        //==============================To Get Store List==========================================
        [HttpGet]
        [Route("GetStoresList")]
        public IHttpActionResult GetStoresList()
        {
            try
            {
                var res = dbContext.iHub_Units_DC_WH_ST.Where(x => x.Unit_Hierarchy_Level == 3).OrderBy(x => x.iHub_Unit_ID).ToList();
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


        //===================== Get All Raffles List =================
        [HttpGet]
        [Route("Get_All_Raffles_List")]
        public IHttpActionResult Get_All_Raffles_List(int OrderID)
        {
            try
            {
                var res = dbContext.RffleCoupon.Where(y => y.Order_Id == OrderID).ToList();
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

        //=======================>>>>>>> Get DropDown For Tenure <<<<<<<<<<<<===========================
        [HttpGet]
        [Route("Get_Tenure_DropDown")]
        public IHttpActionResult Get_Tenure_DropDown()
        {
            try
            {
                var Tenures = dbContext.iHub_EMI_Orders.GroupBy(s => s.Tenure).ToList().Select(m => m.Key).ToList();
                return Ok(Tenures);
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
        [Route("Get_ProcurementList")]
        public IHttpActionResult Get_ProcurementList(int RoleID)
        {
            try
            {
                var cmd = "select ul.AspNetUserId,asp.Id  from AspNetUsers asp join AspNetUserRoles rl on rl.UserId = asp.ID join AspNetRoles ro on ro.Id = rl.RoleId join Userlogins ul on ul.UserID=asp.Id where ro.Id = " + RoleID;
                Dictionary<string, object> parameter1 = new Dictionary<string, object> { };
                var res = _database.SelectQuery(cmd, parameter1);
                //SignalRController oSig = new SignalRController();
                //var od = oSig.GetGUID();
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
        [Route("Get_ProcurementList_ByID")]
        public IHttpActionResult Get_ProcurementList_ByID(string UserID)
        {
            try
            {
                string sUserID = UserID.Replace("-", "");
                var cmd = "SELECT COUNT(*) As ProCount FROM UserContacts where UserID=" + sUserID;
                Dictionary<string, object> parameter1 = new Dictionary<string, object> { };
                var res = _database.SelectQuery(cmd, parameter1);
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
        [Route("ExportExcelwithAttribute")]
        public IHttpActionResult ExportExcelwithAttribute(int iCatID)
        {
            try
            {
                string sQuery = "SELECT Product_Name,MRP,Selling_Price AS SellingPrice,Attribute_Json,(CAST(x.Product_Series AS VARCHAR(70)) + '-' + CAST((CAST(x.iHub_Product_ID  AS INT) + 100000)AS VARCHAR(500))) AS SKU, HasBarcode, SourceVendor, ReorderLevel FROM iHub_Products x LEFT JOIN iH_Category_Product_Mapping y ON x.iHub_Product_ID = y.Product_Id LEFT JOIN iH_Product_Attributes z  ON x.iHub_Product_ID = z.Product_Id LEFT JOIN iH_Products_Grouping D  ON x.Group_ID = D.Products_Grouping_ID WHERE iHub_Product_ID IS NOT NULL and y.Category_Id =" + iCatID + " order by created_date_time";
                Dictionary<string, object> oPP = new Dictionary<string, object> { };
                var oPPD = _database.SelectQuery(sQuery, oPP);
                Dictionary<string, object> oAllHead = new Dictionary<string, object> { };
                int i = 0;
                foreach (var oPCS in oPPD)
                {
                    var iValue = oPCS.Where(m => m.Key.ToLower() == "attribute_json").Select(m => m.Value).FirstOrDefault();
                    if (iValue != null)
                    {
                        var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(iValue.ToString());
                        int index = oPCS.ToList().FindIndex(m => m.Key.ToLower().Contains("attribute_json"));
                        if (index > 0)
                        {
                            oPPD[i].Remove("Attribute_Json");
                        }
                        foreach (KeyValuePair<string, object> keyValuePair in dict)
                        {
                            oPPD[i].Add(keyValuePair.Key, keyValuePair.Value.ToString());
                            if (!oAllHead.ContainsKey(keyValuePair.Key))
                            {
                                oAllHead.Add(keyValuePair.Key, keyValuePair.Value.ToString());
                            }
                        }
                        i++;
                    }
                }
                //string FileFormat = ".xlsx";
                string sName = "Product with Attributes";

                if (iCatID > 0)
                {
                    string cmd = "SELECT CategoryName FROM iH_Categories WHERE ID=" + iCatID;
                    Dictionary<string, object> par = new Dictionary<string, object> { };
                    var oCat = _database.SelectQuery(cmd, par);
                    sName = oCat.FirstOrDefault().Where(m => m.Key.ToLower() == "categoryname").Select(m => m.Value).FirstOrDefault();
                    sName = DateTime.Now.TimeOfDay.Hours.ToString() + DateTime.Now.TimeOfDay.Minutes.ToString() + DateTime.Now.TimeOfDay.Seconds.ToString() + "-" + sName;

                    foreach (var item in oPPD.FirstOrDefault())
                    {
                        oAllHead[item.Key] = item.Value;
                    }
                }

                if (sName.Contains('/'))
                {
                    sName = sName.Replace("/", "//");
                }
                List<string> oHeadings = new List<string>();
                if (oAllHead != null)
                {
                    foreach (var item in oAllHead)
                    {
                        oHeadings.Add(item.Key); //Adding Headings
                    }
                }
                if (oHeadings != null && oHeadings.Count() > 0)
                {
                    var iIndex = oHeadings.IndexOf("Product_Name");
                    if (iIndex != -1)
                    {
                        oHeadings.RemoveAt(iIndex);
                        oHeadings.Insert(0, "Product_Name");
                    }
                }
                VMDataTableResponse oDataRe = new VMDataTableResponse();
                if (sName.Contains('-'))
                {
                    oDataRe.sName = sName.Split('-')[1].Replace(" ", "");
                }
                oDataRe.Resultset1 = new List<Dictionary<string, object>>();
                int b = 0;
                foreach (var d in oPPD)
                {
                    int t = 0;
                    Dictionary<string, object> oD = new Dictionary<string, object>();
                    foreach (var q in oHeadings)
                    {
                        var oColD = d.ToList().Where(y => y.Key == q).Select(y => y.Value).FirstOrDefault();
                        //if (q == "ReorderLevel")
                        //{
                        //    oD[q] = Convert.ToInt32(oColD);
                        //}
                        //else if(q =="MRP" || q =="SellingPrice")
                        //{
                        //    oD[q] = decimal.Parse(oColD);
                        //}
                        //else
                        //{
                        oD[q] = oColD;
                        //}

                        t++;
                    }
                    oDataRe.Resultset1.Add(oD);
                    b++;
                }
                return Ok(oDataRe);
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
    }
}