
using iHubAdminAPI.Models;

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace iHubAdminAPI
{
    public static class CommonMethods
    {
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            ModelDBContext dbContext = new ModelDBContext();
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }


        /// <purpose>
        /// it Save the Exception Details in Database
        /// </purpose>
        public static bool LogError(Exception ex)
        {
            string InnerExp = string.Empty;
            string StrDetailedMsg = ex.ToString().Replace("'", string.Empty);
            if (ex.InnerException != null)
            {
                InnerExp = ex.InnerException.Message;
                if (ex.InnerException.InnerException != null)
                {
                    InnerExp = ex.InnerException.InnerException.Message.Replace("'", string.Empty);
                }
            }
            string expmsg = ex.Message.Replace("'", string.Empty);
            var st = new StackTrace(ex, true);
            var Expinfo = st.GetFrames()
                          .Select(frame => new
                          {
                              FileName = frame.GetFileName(),
                              LineNumber = frame.GetFileLineNumber(),
                              ColumnNumber = frame.GetFileColumnNumber(),
                              Method = frame.GetMethod().Name,
                              Controller = frame.GetMethod().DeclaringType.Name, //frame.GetMethod().DeclaringType
                          }).FirstOrDefault();

            ModelDBContext dbcontext = new ModelDBContext();
            string query = "INSERT INTO dbo.tblErrorLog(ExeptionMessage,Controller,Method,FileName,LineNumber,ColumnNumber,InnerException,DetailedMessage) VALUES('" + expmsg + "','" + Expinfo.Controller + "','" + Expinfo.Method + "','" + Expinfo.FileName + "','" + Expinfo.LineNumber + "','" + Expinfo.ColumnNumber + "','" + InnerExp + "','" + StrDetailedMsg + "')";
            dbcontext.Database.ExecuteSqlCommand(query);
            return true;
        }
        public static Dictionary<string, string> ToDictionary(this DataTable datatble)
        {
            try
            {

                Dictionary<string, string> result = new Dictionary<string, string>();
                foreach (DataRow rrow in datatble.Rows)
                {
                    result = Enumerable.Range(0, datatble.Columns.Count)
                        .ToDictionary(i => datatble.Columns[i].ColumnName, i => rrow.ItemArray[i].ToString());
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<Dictionary<string, string>> GetDataTableDictionaryList(DataTable dt)
        {
            try
            {
                return dt.AsEnumerable().Select(
                row => dt.Columns.Cast<DataColumn>().ToDictionary(
                    column => column.ColumnName,
                    column => row[column].ToString()
                )).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
