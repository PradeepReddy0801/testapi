using AspNet.Identity.PostgreSQL;
using AspNet.Identity.SQLDatabase;
using iHubAdminAPI.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
namespace iHubAdminAPI.Controllers
{
    [RoutePrefix("api/Dump")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DumpController : ApiController
    {
        public SQLDatabase _database;
        public PostgreSQLDatabase _pgdatabase;
        
        public int pageindex = 1;

        //EncrypDecrypt encrypt = new EncrypDecrypt();
        public DumpController()
        {
            _database = new SQLDatabase();
            _pgdatabase = new PostgreSQLDatabase();
        }




        public void BulkCopy(int Limit, int Offset, string tablename)
        {

            //string connectionString = GetConnectionString();
            string connectionString = ConfigurationManager.ConnectionStrings["ngDefaultConnection"].ConnectionString;
            // Open a sourceConnection to the AdventureWorks database.
            using (NpgsqlConnection sourceConnection = new NpgsqlConnection(connectionString))
            {
                sourceConnection.Open();
                // HttpContext.Current.Server.ScriptTimeout = 10000;
                string Get_Units_List = "";
                if (tablename.ToString().ToLower() == "iHub_Products".ToLower())
                {
                    var ID = "iHub_Product_ID";
                    Get_Units_List = "SELECT * FROM \"" + tablename + "\"  WHERE \"" + ID + "\"  BETWEEN " + Limit + "  AND " + Offset;
                }
                else if (tablename.ToString().ToLower() == "iHub_Inventory_Products".ToLower())
                {
                    var ID = "Inventory_Product_ID";
                    Get_Units_List = "SELECT * FROM \"" + tablename + "\"  WHERE \"" + ID + "\"  BETWEEN " + Limit + "  AND " + Offset;
                }
                else
                {
                    Get_Units_List = "SELECT * FROM \"" + tablename + "\" LIMIT " + Limit + "  OFFSET " + Offset;
                }




                // Get data from the source table as a SqlDataReader.
                NpgsqlCommand commandSourceData = new NpgsqlCommand(Get_Units_List, sourceConnection);
                commandSourceData.CommandType = CommandType.Text;

                DataTable dt = new DataTable();
                NpgsqlDataReader reader = commandSourceData.ExecuteReader();
                dt.Load(reader);

                string dconnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                using (SqlConnection destinationConnection =
                           new SqlConnection(dconnectionString))
                {
                    destinationConnection.Open();


                    using (SqlBulkCopy bulkCopy =
                               new SqlBulkCopy(destinationConnection))
                        
                    {
                        bulkCopy.BulkCopyTimeout =0;
                        bulkCopy.DestinationTableName =
                            "dbo." + tablename;
                        foreach (var item in dt.Columns)
                        {
                            if (item.ToString().ToLower() != "Search_String".ToLower() && item.ToString().ToLower() != "tsv".ToLower())
                            {
                                bulkCopy.ColumnMappings.Add(item.ToString(), item.ToString());
                            }
                        }

                        try
                        {
                            // Write from the source to the destination.
                            bulkCopy.WriteToServer(dt);
                        }
                        catch (SqlException ex)
                        {
                            if (ex.Message.Contains("Received an invalid column length from the bcp client for colid"))
                            {
                                string pattern = @"\d+";
                                Match match = Regex.Match(ex.Message.ToString(), pattern);
                                var index = Convert.ToInt32(match.Value) - 1;

                                FieldInfo fi = typeof(SqlBulkCopy).GetField("_sortedColumnMappings", BindingFlags.NonPublic | BindingFlags.Instance);
                                var sortedColumns = fi.GetValue(bulkCopy);
                                var items = (Object[])sortedColumns.GetType().GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(sortedColumns);

                                FieldInfo itemdata = items[index].GetType().GetField("_metadata", BindingFlags.NonPublic | BindingFlags.Instance);
                                var metadata = itemdata.GetValue(items[index]);

                                var column = metadata.GetType().GetField("column", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);
                                var length = metadata.GetType().GetField("length", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);
                                // var Dtaa = metadata.GetType().GetField("Data")
                                throw new DataFormatException(String.Format("Column: {0} contains data with a length greater than: {1}", column, length));
                            }

                            throw;
                        }
                        finally
                        {

                            sourceConnection.Close();
                            reader.Close();
                        }
                    }

                }
            }
        }
        [HttpGet]
        [Route("forEach")]
        public void forEach(string tablename)
        {
            int i = 10000; int TotalCount = 0; var k = 0;
            //int increament = 0;
            string tablecount = "Select Count(*) from \"" + tablename + "\"";
            string connectionString = ConfigurationManager.ConnectionStrings["ngDefaultConnection"].ConnectionString;
            using (NpgsqlConnection sourceConnection = new NpgsqlConnection(connectionString))
            {
                sourceConnection.Open();

                // Get data from the source table as a SqlDataReader.
                NpgsqlCommand commandSourceData = new NpgsqlCommand(tablecount, sourceConnection);
                commandSourceData.CommandType = CommandType.Text;
                TotalCount = int.Parse(commandSourceData.ExecuteScalar().ToString());

            }

            //Static Dumping For iHub_Products Table
            if (tablename.ToString().ToLower() == "iHub_Products".ToLower() )
            {
                for (int j = 0; j <= 10; j = j++)
                {

                    k = k + 1;
                    if (k == 1)
                    {
                        BulkCopy(0, 400000, tablename);
                    }
                    else if (k == 2)
                    {
                        BulkCopy(400001, 1500000, tablename);
                    }
                    else if (k == 3)
                    {
                        BulkCopy(1500001, 2450000, tablename);
                    }
                    else if (k == 4)
                    {
                        BulkCopy(2450001, 2475000, tablename);
                    }
                    else if (k == 5)
                    {
                        BulkCopy(2475001, 2485000, tablename);
                    }
                    else if (k == 6)
                    {
                        BulkCopy(2485001, 2495000, tablename);
                    }
                    else if (k == 7)
                    {
                        BulkCopy(2495001, 2505000, tablename);
                    }
                    else if (k == 8)
                    {
                        BulkCopy(2505001, 2515000, tablename);
                    }
                    else if (k == 9)
                    {
                        BulkCopy(2515001, 2525000, tablename);
                    }
                    else if (k == 10)
                    {
                        BulkCopy(2525001, 2545000, tablename);
                    }
                    else if (k == 11)
                    {
                        BulkCopy(2545001, 2565000, tablename);
                    }
                    else if (k == 12)
                    {
                        BulkCopy(2565001, 2585000, tablename);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else if (tablename.ToString().ToLower() == "iHub_Inventory_Products".ToLower())
            {
                for (int j = 0; j <= 10; j = j++)
                {

                    k = k + 1;
                    if (k == 1)
                    {
                        BulkCopy(0, 20000, tablename);
                    }
                    else if (k == 2)
                    {
                        BulkCopy(20001, 40000, tablename);
                    }
                    else if (k == 3)
                    {
                        BulkCopy(40001, 60000, tablename);
                    }
                    else if (k == 4)
                    {
                        BulkCopy(60001, 80000, tablename);
                    }
                    else if (k == 5)
                    {
                        BulkCopy(80001, 100000, tablename);
                    }
                    else if (k == 6)
                    {
                        BulkCopy(100001, 120000, tablename);
                    }
                    else if (k == 7)
                    {
                        BulkCopy(120001, 140000, tablename);
                    }
                    else if (k == 8)
                    {
                        BulkCopy(140001, 160000, tablename);
                    }
                    else if (k == 9)
                    {
                        BulkCopy(160001, 180000, tablename);
                    }
                    else if (k == 10)
                    {
                        BulkCopy(180001, 200000, tablename);
                    }
                    else if (k == 11)
                    {
                        BulkCopy(200001, 220000, tablename);
                    }
                    else if (k == 12)
                    {
                        BulkCopy(220001, 240000, tablename);
                    }
                    else if (k == 13)
                    {
                        BulkCopy(240001, 260000, tablename);
                    }
                    else if (k == 14)
                    {
                        BulkCopy(260001, 280000, tablename);
                    }
                    else if (k == 15)
                    {
                        BulkCopy(280001, 300000, tablename);
                    }
                    else if (k == 16)
                    {
                        BulkCopy(300001, 320000, tablename);
                    }
                    else if (k == 17)
                    {
                        BulkCopy(320001, 340000, tablename);
                    }
                    else if (k == 18)
                    {
                        BulkCopy(340001, 360000, tablename);
                    }
                    else if (k == 19)
                    {
                        BulkCopy(360001, 380000, tablename);
                    }
                    else if (k == 20)
                    {
                        BulkCopy(380001, 400000, tablename);
                    }
                    else if (k == 21)
                    {
                        BulkCopy(400001, 420000, tablename);
                    }
                    else if (k == 22)
                    {
                        BulkCopy(420001, 440000, tablename);
                    }
                    else if (k == 23)
                    {
                        BulkCopy(440001, 460000, tablename);
                    }
                    else if (k == 24)
                    {
                        BulkCopy(460001, 480000, tablename);
                    }
                    else if (k == 25)
                    {
                        BulkCopy(480001, 500000, tablename);
                    }
                    else if (k == 26)
                    {
                        BulkCopy(500001, 520000, tablename);
                    }
                    else if (k == 27)
                    {
                        BulkCopy(520001, 540000, tablename);
                    }
                    else if (k == 28)
                    {
                        BulkCopy(540001, 560000, tablename);
                    }
                    else if (k == 29)
                    {
                        BulkCopy(560001, 580000, tablename);
                    }
                    else if (k == 30)
                    {
                        BulkCopy(580001, 600000, tablename);
                    }
                    else if (k == 31)
                    {
                        BulkCopy(600001, 620000, tablename);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            //Dynamic Dumping For Remaining  Tables
            else
            {
                for (int j = 0; j <= TotalCount; j = j + i)
                {
                    BulkCopy(i, j, tablename);
                }
                //  increament += i;
            }


        }

        [HttpGet]
        [Route("DumpTables")]
        public  List<EnumModel>  DumpTables()
        {
            try
            {
                List<EnumModel> Tables = ((EnumTables[])Enum.GetValues(typeof(EnumTables))).Select(c => new EnumModel() { Name = c.ToString() }).ToList();
                List<string> MyNames = ((EnumTables[])Enum.GetValues(typeof(EnumTables))).Select(c => c.ToString()).ToList();
                var TableCount = Convert.ToInt32(MyNames.Count());
                for (int i = 0; i < MyNames.Count; i++)
                {
                  forEach(MyNames[i]);
                }
                return Tables;
            }
            catch(Exception EX)
            {
                throw EX;
            }
        }
    }
}