using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Configuration;
using System.Text;
using System.IO;
using AspNet.Identity.SQLDatabase;
using Newtonsoft.Json;
//using static iHubAdminAPI.Models.Enums;
using System.Data;
using Newtonsoft.Json.Linq;
using iHubAdminAPI.Models;

namespace iHubAdminAPI.Models
{
    public class Common
    {


        public static string[] validatebulkCol = { "SKU", "ProductName", "MRP", "LandingPrice", "SellingPrice", "Quantity", "Vendor" };
        List<string> validatexlrange = new List<string>(validatebulkCol);
        public static string[] validateBulkdatatypes = { "String", "String", "Double", "Double", "Double", "Double", "String" };
        List<string> validatdatatypes = new List<string>(validateBulkdatatypes);
        
        public static string[] validateproductsCol = {  "ProductName", "ProductCode", "ImageCode", "ProductSeries", "MRP", "HtmlContent",

                                                        "Is_Repurchasable","Status","SellingPrice","HSN_Code","Booking_Percentage"
                                                     };

        List<string> validatexl1range = new List<string>(validateproductsCol);
        
        public static string[] validateProddatatypes = { "String", "String", "String", "String","Double", "String",

                                                          "String","String", "Double", "Double", "Double"
                                                       };
        List<string> validatdatatypes1 = new List<string>(validateProddatatypes);

        public static SQLDatabase _database = new SQLDatabase();
        
        // =================================Command Rext For Api Calls====================================================      
        static string insert_otp = "[iAdmin_Save_Notification_Details]";
        //=============================================Method  For Genarate random Otp========================
        

        public static string ProductName { get; private set; }
        public static string MRP { get; private set; }
        public static string LandingPrice { get; private set; }
        public static string SKU { get; private set; }
        public static string SellingPrice { get; private set; }
        public static string Quantity { get; private set; }
        public static string Vendor { get; private set; }

        public static string GenerateOTP()
        {
            try
            {
                int length = 6;
                string numbers = "0123456789";
                Random objrandom = new Random();
                string strrandom = string.Empty;
                int noofnumbers = length;
                for (int i = 0; i < noofnumbers; i++)
                {
                    int temp = objrandom.Next(0, numbers.Length);
                    strrandom += temp;
                }
                return strrandom;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

       /* public static void SendMessages(string PhoneNumber, string Message,string DLT_MSG_KEY_ID )
        {
            try
            {
                string BulkSMSURL = WebConfigurationManager.AppSettings["BulkSMSURL"];
                BulkSMSURL = BulkSMSURL.Replace("{PhoneNumber}", PhoneNumber);
                BulkSMSURL = BulkSMSURL.Replace("{Message}", Message);
                BulkSMSURL = BulkSMSURL.Replace("{DLTID}", DLT_MSG_KEY_ID);
                WebClient WebClient = new WebClient();
                System.IO.StreamReader reader = new System.IO.StreamReader(WebClient.OpenRead(BulkSMSURL));
                dynamic ResultHTML = reader.ReadToEnd();
                //return PhoneNumber;

                // string OTP = GenerateOTP();
                /*string BulkSMSPath = WebConfigurationManager.AppSettings["BulkSMSPath"];
                string SMSAPIKey = WebConfigurationManager.AppSettings["SMSAPIKey"];
                string sender = WebConfigurationManager.AppSettings["isender"];

                string strResult = BulkSMSPath + "authkey=" + SMSAPIKey + "&mobiles=" + PhoneNumber + "&message=" + Message + "&sender=" + sender + "&route = 6" + "&country=0&"+ DLT_MSG_KEY_ID;
                WebClient WebClient = new WebClient();
                System.IO.StreamReader reader = new System.IO.StreamReader(WebClient.OpenRead(strResult));
                dynamic ResultHTML = reader.ReadToEnd();*/
                //return OTP;
           /* }
            catch (Exception ex)
            {

                throw ex;
            }
        }*/
        //=============================================Method  For Sending Otp to Phone Number========================
        public static string sendAdminOTP(int userID, string PhoneNumber, string Purpose)
        {
            try
            {

                string OTP = GenerateOTP();
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@mobile_number", PhoneNumber }, { "@otp_generated", OTP }, { "@otp_purpose", Purpose }, { "@user_ID", userID } };
                var buyer_details = _database.QueryValue(insert_otp, parameters);
                if (PhoneNumber != "0000000000")
                {
                    sendmessage(PhoneNumber, "Your Verification OTP is " + OTP , "1007161605656824097");
                }
                return buyer_details.ToString();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static void SendMessagesOLd(string PhoneNumber, string Message)
        {
            try
            {
                // string OTP = GenerateOTP();
                string BulkSMSPath = WebConfigurationManager.AppSettings["iHubsmspath"];
                string uname = WebConfigurationManager.AppSettings["uname"];
                string password = WebConfigurationManager.AppSettings["password"];
                string sender = WebConfigurationManager.AppSettings["sender"];
                string route = WebConfigurationManager.AppSettings["route"];
                string msgtype = WebConfigurationManager.AppSettings["msgtype"];

                string strResult = BulkSMSPath + "uname=" + uname + "&password=" + password + "&sender=" + sender + "&receiver=" + PhoneNumber + "&route=" + route + "&msgtype=" + msgtype + "&sms=" + Message;
                //string strResult = BulkSMSPath + "authkey=" + SMSAPIKey + "&mobiles=" + PhoneNumber + "&message=" + Message + "&sender=ISTORE" + "&route = 6" + "&country = 0";
                WebClient WebClient = new WebClient();
                System.IO.StreamReader reader = new System.IO.StreamReader(WebClient.OpenRead(strResult));
                dynamic ResultHTML = reader.ReadToEnd();
                //return OTP;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static void SendNotification(string message, string deviceId)
        {
            var notificationurl = WebConfigurationManager.AppSettings["notificationurl"];
            deviceId = deviceId.TrimEnd('"', ',', '"');
            string GoogleAppID = WebConfigurationManager.AppSettings["GoogleAppID"];
            var SENDER_ID = WebConfigurationManager.AppSettings["SENDER_ID"];
            WebRequest tRequest;
            tRequest = WebRequest.Create(notificationurl);
            tRequest.Headers.Add(string.Format("Authorization: key={0}", GoogleAppID));
            tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));
            tRequest.Method = "POST";
            tRequest.ContentType = " application/json";
            string postData = "{\"collapse_key\":\"score_update\",\"time_to_live\":108,\"delay_while_idle\":true,\"data\": { \"message\" : " + "\"" + message + "\", \"info\" : " + "\"" + 2 + "\",\"time\": " + "\"" + System.DateTime.Now.ToString() + "\"},\"registration_ids\":[\"" + deviceId + "\"]}";
            Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            tRequest.ContentLength = byteArray.Length;
            Stream dataStream = tRequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse tResponse = tRequest.GetResponse();
            dataStream = tResponse.GetResponseStream();
            StreamReader tReader = new StreamReader(dataStream);
            String sResponseFromServer = tReader.ReadToEnd();
            tReader.Close();
            dataStream.Close();
            tResponse.Close();
        }

        public static string sendmessage(string PhoneNumber, string Message, string DLT_MSG_KEY_ID)
        {
            try
            {
                //BulkSMSURL --{PhoneNumber} { Message} {DLTID}
                string BulkSMSURL = WebConfigurationManager.AppSettings["BulkSMSURL"];
                BulkSMSURL =  BulkSMSURL.Replace("{PhoneNumber}", PhoneNumber);
                BulkSMSURL = BulkSMSURL.Replace("{Message}", (Message + "- ihubmultistores pvt ltd"));
                BulkSMSURL = BulkSMSURL.Replace("{DLTID}", DLT_MSG_KEY_ID);
                WebClient WebClient = new WebClient();
                System.IO.StreamReader reader = new System.IO.StreamReader(WebClient.OpenRead(BulkSMSURL));
                dynamic ResultHTML = reader.ReadToEnd();
                return PhoneNumber;

                /*
                string BulkSMSPath = WebConfigurationManager.AppSettings["BulkSMSPath"];
                string SMSAPIKey = WebConfigurationManager.AppSettings["SMSAPIKey"];
                string Sender = WebConfigurationManager.AppSettings["isender"];
                string strResult = BulkSMSPath + "authkey=" + SMSAPIKey + "&mobiles=" + PhoneNumber + "&message=" + Message + "&sender=" + Sender + "&route = 6" + "&country=0&" + DLT_MSG_KEY_ID;
                WebClient WebClient = new WebClient();
                System.IO.StreamReader reader = new System.IO.StreamReader(WebClient.OpenRead(strResult));
                dynamic ResultHTML = reader.ReadToEnd();
                return PhoneNumber;*/
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static string sendemail(string EmailID, string Emailmessage)
        {
            try
            {

                string BulkSMSPath = WebConfigurationManager.AppSettings["BulkSMSPath"];
                string SMSAPIKey = WebConfigurationManager.AppSettings["SMSAPIKey"];
                string Sender = WebConfigurationManager.AppSettings["isender"];
                string strResult = BulkSMSPath + "authkey=" + SMSAPIKey + "&mobiles=" + EmailID + "&message=" + Emailmessage + "&sender=" + Sender + "&route = 6" + "&country = 0";
                WebClient WebClient = new WebClient();
                System.IO.StreamReader reader = new System.IO.StreamReader(WebClient.OpenRead(strResult));
                dynamic ResultHTML = reader.ReadToEnd();
                return EmailID;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static void checknull(string str, int i)
        {
            if (str == null)
            {
                Console.WriteLine("''");
            }
            else


            {
                Console.WriteLine("0");
            }
        }

        public static string validateexcelfile(string type, string filename, DataTable Data)
        {

            int Count = Data.Columns.Count;
            if (type == "Bulkorders")
            {

                if (Count < validatebulkCol.Length)
                {
                    return "Please Provide all columns";
                }

                else
                {
                    if (Data.Rows.Count <= 1)
                    {
                        return "Can't create Single product Order";
                    }
                    else
                    {
                        var emptyfeald = validateData(Data.Rows, validatebulkCol, validateBulkdatatypes);
                        if (emptyfeald == "true")
                        {
                            var textfeald = validateDataType(0, Data.Rows, validatebulkCol, validateBulkdatatypes);
                            if (textfeald == "true")
                            {
                                return "ok";
                            }
                            else
                            {
                                return textfeald;
                            }
                        }
                        else
                        {
                            return emptyfeald;
                        }
                    }
                }
            }

            else if (type == "UploadProducts")
            {

                if (Count < validateproductsCol.Length)
                {
                    return "Please Provide all columns";
                }

                else
                {
                    if (Data.Rows.Count <= 1)
                    {
                        return "Can't create Single product Order";
                    }
                    else
                    {

                        var emptyfeald = validateData(Data.Rows, validateproductsCol, validateProddatatypes);
                        if (emptyfeald == "true")
                        {
                            var textfeald = validateDataType(1, Data.Rows, validateproductsCol, validateProddatatypes);
                            if (textfeald == "true")
                            {
                                return "True";
                            }
                            else
                            {
                                return textfeald;
                            }
                        }
                        else
                        {
                            return emptyfeald;
                        }
                    }
                }
            }
            return "true";
        }

        public static string validateDataType(int index, DataRowCollection datatype, string[] colnames, string[] datatypes)
        {
            for (var k = index; k < datatype.Count; k++)
            {

                for (var j = 0; j < datatype[k].ItemArray.Length; j++)
                {
                    object o = datatype[k].ItemArray[j];
                    var dt = o.GetType();
                    if (datatypes.Count() > j)
                    {
                        if (datatypes[j] != dt.Name)
                        {
                            return "Please Enter Valid " + colnames[j];
                        }
                    }

                    //if (datatypes[j] != dt.Name)
                    //{
                    //    return "Please Enter Valid " + colnames[j];
                    //}

                    //else if (datatypes[j]!=dt.Name)
                    //{
                    //    return "Please Enter Valid " + colnames[j];
                    //}
                }
            }
            return "true";
        }
        public static string validateData(DataRowCollection datatype, string[] colnames, string[] datatypes)
        {

            for (var k = 1; k < datatype.Count; k++)
            {

                for (var j = 1; j < datatype[k].ItemArray.Length; j++)
                {
                    if (datatype[k].ItemArray[j] == "")
                    {
                        return "Please fill all columns";

                    }
                }
            }

            return "true";
        }
        private static void Ok(int v)
        {
            throw new NotImplementedException();
        }
    }


}




