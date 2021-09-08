
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace iHubAdminAPI.Models
{
    public static class ServiceUtil
    {
        public static string ReplaceValuesInTemplate(Dictionary<string, string> values, string Template)
        {
            if (values != null)
            {
                foreach (var item in values)
                {
                    Template = Template.Replace(item.Key, item.Value);
                }
            }
            return Template;
        }
        public static string AttachmentType(string Extension)
        {
            Extension = Extension.ToLower();
            List<string> ImageExtensions = new List<string>() { ".png", ".jpeg", ".jpg", ".gif", ".bmp" };
            if (ImageExtensions.Contains(Extension)) return "Image";

            List<string> VideoExtensions = new List<string>() { ".avi", ".mp4", ".3gp", ".flv", ".mkv" };
            if (VideoExtensions.Contains(Extension)) return "Video";

            List<string> AudioExtensions = new List<string>() { ".mp3", ".wav" };
            if (AudioExtensions.Contains(Extension)) return "Audio";

            List<string> pdfExtensions = new List<string>() { ".pdf" };
            if (pdfExtensions.Contains(Extension)) return "PDF";

            List<string> txtExtensions = new List<string>() { ".txt" };
            if (txtExtensions.Contains(Extension)) return "txt";

            List<string> xlsExtensions = new List<string>() { ".xls", ".xlsx", ".xml" };
            if (xlsExtensions.Contains(Extension)) return "xls";

            else return "";
        }
        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9]+", "", RegexOptions.Compiled);
        }
        public static string SkipAndTakeRecords(string orderby, int skipRecords, int takeRecords)
        {
            return string.Format("ORDER BY {0} DESC OFFSET {1} ROWS FETCH NEXT {2} ROWS ONLY ", orderby, skipRecords, takeRecords);
        }
        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }
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
        public static bool IslstNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                return true;
            }
            /* If this is a list, use the Count property for efficiency. 
             * The Count property is O(1) while IEnumerable.Count() is O(N). */
            var collection = enumerable as ICollection<T>;
            if (collection != null)
            {
                return collection.Count < 1;
            }
            return !enumerable.Any();
        }

        //    public static void SendNotification(string UserID, string message, bool IsEvent = false)
        //    {
        //        try
        //        {
        //            ModelDBContext dbcontext = new ModelDBContext();
        //            int type = (int)EnumMobileOSTypes.Android;
        //            var devicelist = dbcontext.UserDevices.Where(async => async.NoHiphenUserID == UserID && async.DeviceID != "" && async.StatusTypeID == 10);
        //            SendAndroidNotification(devicelist.Where(async => async.Platform == type).Select(a => a.DeviceID).ToList(), message, IsEvent);

        //        }
        //        catch (Exception ex)
        //        {
        //            throw;
        //        }
        //    }

        //    public static void SendAndroidNotification(List<string> devicelist, string message, bool IsEvent = false)
        //    {
        //        try
        //        {
        //            string deviceId = "";
        //            foreach (var item in devicelist)
        //            {
        //                deviceId += item + "\",\"";
        //            }
        //            deviceId = deviceId.TrimEnd('"', ',', '"');
        //            string GoogleAppID = ConfigurationManager.AppSettings["GoogleAppID"].ToString();

        //            var SENDER_ID = ConfigurationManager.AppSettings["SENDER_ID"].ToString();
        //            ServicePointManager.ServerCertificateValidationCallback =
        //delegate (object s, X509Certificate certificate,
        //         X509Chain chain, SslPolicyErrors sslPolicyErrors)
        //{ return true; };
        //            WebRequest tRequest;
        //            tRequest = WebRequest.Create(ConfigurationManager.AppSettings["GoogleNotificationLink"].ToString());
        //            tRequest.Headers.Add(string.Format("Authorization: key={0}", GoogleAppID));

        //            tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));
        //            tRequest.Method = "POST";

        //            tRequest.ContentType = " application/json";
        //            //,\"info\": " + "\"" + "test" + "\"
        //            string postData = "{\"collapse_key\":\"score_update\",\"time_to_live\":2419200,\"delay_while_idle\":false,\"data\": {\"message\" : " + "\"" + message + "\",\"time\": " + "\"" + System.DateTime.Now.ToString() + "\",\"style\": " + "\"" + "inbox" + "\",\"summaryText\": " + "\"" + "There are %n% notifications" + "\",\"content-available\": " + "\"" + "1" + "\"},\"registration_ids\":[\"" + deviceId + "\"]}";
        //            if (IsEvent)
        //                postData = "{\"collapse_key\":\"score_update\",\"time_to_live\":2419200,\"delay_while_idle\":false,\"data\": {\"message\" : " + "\"" + message + "\",\"time\": " + "\"" + System.DateTime.Now.ToString() + "\",\"style\": " + "\"" + "inbox" + "\",\"info\": " + "\"" + "Event" + "\",\"summaryText\": " + "\"" + "There are %n% notifications" + "\",\"content-available\": " + "\"" + "1" + "\"},\"registration_ids\":[\"" + deviceId + "\"]}";
        //            Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
        //            tRequest.ContentLength = byteArray.Length;
        //            Stream dataStream = tRequest.GetRequestStream();
        //            dataStream.Write(byteArray, 0, byteArray.Length);
        //            dataStream.Close();
        //            WebResponse tResponse = tRequest.GetResponse();
        //            dataStream = tResponse.GetResponseStream();
        //            StreamReader tReader = new StreamReader(dataStream);
        //            String sResponseFromServer = tReader.ReadToEnd();
        //            tReader.Close();
        //            dataStream.Close();
        //            tResponse.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //            throw;
        //        }
        //    }
    }
}
