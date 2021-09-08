using iHubAdminAPI;
using iHubAdminAPI.Models;
using log4net;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;

namespace AspNet.Identity.SQLDatabase
{
    /// <summary>
    /// Class that encapsulates PostgreSQL database connections and CRUD operations.
    /// </summary>
    public class SQLDatabase : IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SQLDatabase).FullName);
        private SqlConnection _connection;

        /// Default constructor which uses the "DefaultConnection" connectionString, often located in web.config.
        /// </summary>
        public SQLDatabase()
            : this("DefaultConnection")
        {
        }

        /// <summary>
        /// Constructor which takes the connection string name.
        /// </summary>
        /// <param name="connectionStringName">The name of the connection string.</param>
        public SQLDatabase(string connectionStringName)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            _connection = new SqlConnection(connectionString);
        }

        /// <summary>
        /// Executes a non-query PostgreSQL statement.
        /// </summary>
        /// <param name="commandText">The PostgreSQL query to execute.</param>
        /// <param name="parameters">Optional parameters to pass to the query.</param>
        /// <returns>The count of records affected by the PostgreSQL statement.</returns>
        public int Execute(string commandText, Dictionary<string, object> parameters)
        {
            int result = 0;

            if (String.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("Command text cannot be null or empty.");
            }

            try
            {
                OpenConnection();
                var command = CreateCommand(commandText, parameters);
                result = command.ExecuteNonQuery();
            }
            finally
            {
                _connection.Close();
            }

            return result;
        }

        /// <summary>
        /// Executes a PostgreSQL query that returns a single scalar value as the result.
        /// </summary>
        /// <param name="commandText">The PostgreSQL query to execute.</param>
        /// <param name="parameters">Optional parameters to pass to the query.</param>
        /// <returns></returns>
        public object QueryValue(string commandText, Dictionary<string, object> parameters)
        {
            object result = null;

            if (String.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("Command text cannot be null or empty.");
            }

            try
            {
                OpenConnection();
                var command = CreateCommand(commandText, parameters);
                command.CommandType = CommandType.StoredProcedure;
                result = command.ExecuteScalar();
            }
            finally
            {
                CloseConnection();
            }

            return result;
        }
        public object QueryValueCOUNT(string commandText, Dictionary<string, object> parameters)
        {
            object result = null;

            if (String.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("Command text cannot be null or empty.");
            }

            try
            {
                OpenConnection();
                var command = CreateCommand(commandText, parameters);
                //command.CommandType = CommandType.StoredProcedure;
                result = command.ExecuteScalar();
            }
            finally
            {
                CloseConnection();
            }

            return result;
        }
        public object QueryValuenew(string commandText, Dictionary<string, object> parameters)
        {
            object result = null;

            if (String.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("Command text cannot be null or empty.");
            }

            try
            {
                OpenConnection();
                var command = CreateCommand(commandText, parameters);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 3000;
                result = command.ExecuteScalar();
            }
            finally
            {
                CloseConnection();
            }

            return result;
        }
        public object QueryFunction(string commandText, Dictionary<string, object> parameters)
        {
            object result = null;

            if (String.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("Command text cannot be null or empty.");
            }

            try
            {
                OpenConnection();
                var command = CreateCommand(commandText, parameters);
                result = command.ExecuteScalar();
            }
            finally
            {
                CloseConnection();
            }

            return result;
        }
        /// <summary>
        /// Executes a SQL query that returns a list of rows as the result.
        /// </summary>
        /// <param name="commandText">The PostgreSQL query to execute.</param>
        /// <param name="parameters">Parameters to pass to the PostgreSQL query.</param>
        /// <returns>A list of a Dictionary of Key, values pairs representing the ColumnName and corresponding value.</returns>
        public List<Dictionary<string, string>> Query(string commandText, Dictionary<string, object> parameters)
        {
            List<Dictionary<string, string>> rows = null;
            if (String.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("Command text cannot be null or empty.");
            }

            try
            {
                OpenConnection();
                var command = CreateCommand(commandText, parameters);
               // command.Parameters.Add("@categoryid",21);
                command.CommandType = CommandType.StoredProcedure;

              //  command.Parameters.Add(new SqlParameter("@categoryid", 21));
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    rows = new List<Dictionary<string, string>>();
                    while (reader.Read())
                    {
                        var row = new Dictionary<string, string>();
                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            var columnName = reader.GetName(i);
                            var columnValue = reader.IsDBNull(i) ? null : reader.GetValue(i).ToString();
                            row.Add(columnName, columnValue);
                        }
                        rows.Add(row);
                    }
                }
            }
            finally
            {
                CloseConnection();
            }

            return rows;
        }
        public List<Dictionary<string, string>> SelectQuery(string commandText, Dictionary<string, object> parameters)
        {
            List<Dictionary<string, string>> rows = null;
            if (String.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("Command text cannot be null or empty.");
            }

            try
            {
                OpenConnection();
                var command = CreateCommand(commandText, parameters);
                // command.Parameters.Add("@categoryid",21);
                //command.CommandType = CommandType.StoredProcedure;

                //  command.Parameters.Add(new SqlParameter("@categoryid", 21));
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    rows = new List<Dictionary<string, string>>();
                    while (reader.Read())
                    {
                        var row = new Dictionary<string, string>();
                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            var columnName = reader.GetName(i);
                            var columnValue = reader.IsDBNull(i) ? null : reader.GetValue(i).ToString();
                            row.Add(columnName, columnValue);
                        }
                        rows.Add(row);
                    }
                }
            }
            finally
            {
                CloseConnection();
            }

            return rows;
        }


        /// <summary>
        /// Executes a SQL query that returns a list of rows as the result.
        /// </summary>
        /// <param name="commandText">The PostgreSQL query to execute.</param>
        /// <param name="parameters">Parameters to pass to the PostgreSQL query.</param>
        /// <returns>A list of a Dictionary of Key, values pairs representing the ColumnName and corresponding value.</returns>
        public VMDataTableResponse ProductListWithCount(string commandText, Dictionary<string, object> parameters)
        {

            VMDataTableResponse vmresult = new VMDataTableResponse();
            if (String.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("Command text cannot be null or empty.");
            }
            try
            {
                OpenConnection();
                //SqlTransaction tran = _connection.BeginTransaction();
                List<Dictionary<string, string>> resultset = new List<Dictionary<string, string>>();
                var command = CreateCommand(commandText, parameters);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = command.ExecuteReader();

                // Output the rows of the first result set
                while (dr.Read())
                {
                    var row = new Dictionary<string, string>();
                    for (var i = 0; i < dr.FieldCount; i++)
                    {
                        var columnName = dr.GetName(i);
                        var columnValue = dr.IsDBNull(i) ? null : dr.GetValue(i).ToString();
                        row.Add(columnName, columnValue);
                    }
                    resultset.Add(row);
                }
                vmresult.Resultset = resultset;
                // Switch to the second result set
                dr.NextResult();
                // Output the rows of the second result set
                while (dr.Read())
                    vmresult.TotalCount = Convert.ToInt32(dr[0]);

            }
            finally
            {
                CloseConnection();
            }

            return vmresult;
        }
        public VMDataTableResponse ProductListWithCountnew(string commandText, Dictionary<string, object> parameters)
        {

            VMDataTableResponse vmresult = new VMDataTableResponse();
            if (String.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("Command text cannot be null or empty.");
            }
            try
            {
                OpenConnection();
                //SqlTransaction tran = _connection.BeginTransaction();
                List<Dictionary<string, string>> resultset = new List<Dictionary<string, string>>();
                var command = CreateCommand(commandText, parameters);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 1000;
                SqlDataReader dr = command.ExecuteReader();

                // Output the rows of the first result set
                while (dr.Read())
                {
                    var row = new Dictionary<string, string>();
                    for (var i = 0; i < dr.FieldCount; i++)
                    {
                        var columnName = dr.GetName(i);
                        var columnValue = dr.IsDBNull(i) ? null : dr.GetValue(i).ToString();
                        row.Add(columnName, columnValue);
                    }
                    resultset.Add(row);
                }
                vmresult.Resultset = resultset;
                // Switch to the second result set
                dr.NextResult();
                // Output the rows of the second result set
                while (dr.Read())
                    vmresult.TotalCount = Convert.ToInt32(dr[0]);

            }
            finally
            {
                CloseConnection();
            }

            return vmresult;
        }
        public VMDataTableResponse GetMultipleResultsList(string commandText, Dictionary<string, object> parameters)
        {

            VMDataTableResponse vmresult = new VMDataTableResponse();
            if (String.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("Command text cannot be null or empty.");
            }
            try
            {
                OpenConnection();
                //SqlTransaction tran = _connection.BeginTransaction();
                List<Dictionary<string, string>> resultset = new List<Dictionary<string, string>>();
                List<Dictionary<string, string>> Addressresultset = new List<Dictionary<string, string>>();

                var command = CreateCommand(commandText, parameters);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = command.ExecuteReader();
               
                // Output the rows of the first result set
                while (dr.Read())
                {
                    var row = new Dictionary<string, string>();
                    for (var i = 0; i < dr.FieldCount; i++)
                    {
                        var columnName = dr.GetName(i);
                        var columnValue = dr.IsDBNull(i) ? null : dr.GetValue(i).ToString();
                        row.Add(columnName, columnValue);
                    }
                    resultset.Add(row);
                }
                vmresult.Resultset = resultset;
                // Switch to the second result set
                dr.NextResult();
                // Output the rows of the second result set
                while (dr.Read())
                {
                    var row = new Dictionary<string, string>();
                    for (var i = 0; i < dr.FieldCount; i++)
                    {
                        var columnName = dr.GetName(i);
                        var columnValue = dr.IsDBNull(i) ? null : dr.GetValue(i).ToString();
                        row.Add(columnName, columnValue);
                    }
                    Addressresultset.Add(row);
                }
                vmresult.Addressset = Addressresultset;
                dr.NextResult();
                // Output the rows of the second result set
                while (dr.Read())
                    vmresult.TotalCount = Convert.ToInt32(dr[0]);

            }
            finally
            {
                CloseConnection();
            }

            return vmresult;
        }
        /// <summary>
        /// Creates a NpgsqlCommand object with the given parameters.
        /// </summary>
        /// <param name="commandText">The PostgreSQL query to execute.</param>
        /// <param name="parameters">Parameters to pass to the PostgreSQL query.</param>
        /// <returns></returns>
        private SqlCommand CreateCommand(string commandText, Dictionary<string, object> parameters)
        {
            try
            {
                SqlCommand command = _connection.CreateCommand();
                command.CommandText = commandText;
                AddParameters(command, parameters);

                return command;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        /// <summary>
        /// Adds the parameters to a PostgreSQL command.
        /// </summary>
        /// <param name="commandText">The PostgreSQL query to execute.</param>
        /// <param name="parameters">Parameters to pass to the PostgreSQL query.</param>
        private static void AddParameters(SqlCommand command, Dictionary<string, object> parameters)
        {
            if (parameters == null)
            {
                return;
            }
            var commandqry = command.CommandText;
            foreach (var param in parameters)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = param.Key;
                parameter.Value = param.Value ?? DBNull.Value;
                command.Parameters.Add(parameter);
                //commandqry= commandqry
            }
        }

        /// <summary>
        /// Helper method to return query a string value. 
        /// </summary>
        /// <param name="commandText">The PostgreSQL query to execute.</param>
        /// <param name="parameters">Parameters to pass to the PostgreSQL query.</param>
        /// <returns>The string value resulting from the query.</returns>
        public string GetStrValue(string commandText, Dictionary<string, object> parameters)
        {
            string value = QueryValueCOUNT(commandText, parameters) as string;
            return value;
        }

        public string InsertStrValue(string commandText, Dictionary<string, object> parameters)
        {
            var value = InsertQueryCommand(commandText, parameters);
            return value.ToString();
        }


        public object InsertQueryCommand(string commandText, Dictionary<string, object> parameters)
        {
            object result = null;

            if (String.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("Command text cannot be null or empty.");
            }

            try
            {
                OpenConnection();
                var command = CreateCommand(commandText, parameters);
                result = command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                log.Error("Error----", ex);
            }
            finally
            {
                CloseConnection();
            }

            return result;
        }

        public int InsertRecordToTable(string TableName, string StrColumns, string StrValues)
        {
            OpenConnection();

            string saveStaff = "INSERT into \"" + TableName + "\" (" + StrColumns + ") VALUES (" + StrValues + ")";

            SqlCommand command = _connection.CreateCommand();
            command.CommandText = saveStaff;
            command.ExecuteNonQuery();
            CloseConnection();
            return 1;
        }


        public int UpdateRecordsToTable(string TableName, string UpdateString, int? ID)
        {
            try
            {
                OpenConnection();

                string saveStaff = "Update \"" + TableName + "\" Set " + UpdateString + " Where \"ID\"=" + ID;

                SqlCommand command = _connection.CreateCommand();
                command.CommandText = saveStaff;
                command.ExecuteNonQuery();
                return Convert.ToInt32(ID);
            }
            finally
            {
                CloseConnection();

            }



        }


        public int UpdateRecordsToMasterTable(string TableName, string UpdateString, int? ID)
        {
            try
            {
                OpenConnection();

                string saveStaff = "Update \"" + TableName + "\" Set " + UpdateString + " Where \"Masters_ID\"=" + ID;

                SqlCommand command = _connection.CreateCommand();
                command.CommandText = saveStaff;
                command.ExecuteNonQuery();
                return Convert.ToInt32(ID);
            }
            finally
            {
                CloseConnection();

            }



        }

        //public int AddCategoryAttributes(VMModelsForCategory gt)
        //{
        //    try
        //    {
        //        OpenConnection();

        //        string saveStaff = "INSERT into \"iH_CategoryAttributes\" (\"CategoryID\",\"JsonData\") VALUES (" + gt.ID + ",'" + gt.JsonData + "')";
        //        SqlCommand command = _connection.CreateCommand();
        //        command.CommandText = saveStaff;
        //        command.ExecuteNonQuery();
        //        return 1;
        //    }
        //    finally
        //    {
        //        CloseConnection();

        //    }

        //}

        public VMDataTableResponse GetDataTableResults(string commandText)
        {
            VMDataTableResponse vmresult = new VMDataTableResponse();
            try
            {
                OpenConnection();

                // Start a transaction as it is required to work with cursors in PostgreSQL
                //SqlTransaction tran = _connection.BeginTransaction();

                // Define a command to call stored procedure show_cities_multiple
                SqlCommand command = new SqlCommand(commandText, _connection);
                command.CommandType = CommandType.StoredProcedure;
                List<Dictionary<string, string>> resultset = new List<Dictionary<string, string>>();

                // Execute the stored procedure and obtain the first result set
                SqlDataReader dr = command.ExecuteReader();

                // Output the rows of the first result set
                while (dr.Read())
                {
                    var row = new Dictionary<string, string>();
                    for (var i = 0; i < dr.FieldCount; i++)
                    {
                        var columnName = dr.GetName(i);
                        var columnValue = dr.IsDBNull(i) ? null : dr.GetValue(i).ToString();
                        row.Add(columnName, columnValue);
                    }
                    resultset.Add(row);
                }
                vmresult.Resultset = resultset;
                // Switch to the second result set
                dr.NextResult();

                // Output the rows of the second result set
                while (dr.Read())
                    vmresult.TotalCount = Convert.ToInt32(dr[0]);

            }
            finally
            {
                CloseConnection();
            }

            return vmresult;
        }





        /// <summary>
        /// Opens a connection if not open.
        /// </summary>
        private void OpenConnection()
        {
            var retries = 10;
            if (_connection.State == ConnectionState.Open)
            {
                return;
            }
            else
            {
                while (retries >= 0 && _connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                    retries--;
                    Thread.Sleep(50);
                }
            }
        }

        /// <summary>
        /// Closes the connection if it is open.
        /// </summary>
        public void CloseConnection()
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }

        public VMDataTableResponse ProductListWithCount2(string commandText, Dictionary<string, object> parameters)
        {

            VMDataTableResponse vmresult = new VMDataTableResponse();
            if (String.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("Command text cannot be null or empty.");
            }
            try
            {
                OpenConnection();
                SqlTransaction tran = _connection.BeginTransaction();
                List<Dictionary<string, string>> resultset = new List<Dictionary<string, string>>();
                var command = CreateCommand(commandText, parameters);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = command.ExecuteReader();

                // Output the rows of the first result set
                while (dr.Read())
                {
                    var row = new Dictionary<string, string>();
                    for (var i = 0; i < dr.FieldCount; i++)
                    {
                        var columnName = dr.GetName(i);
                        var columnValue = dr.IsDBNull(i) ? null : dr.GetValue(i).ToString();
                        row.Add(columnName, columnValue);
                    }
                    resultset.Add(row);
                }
                vmresult.Resultset = resultset;
                // Switch to the second result set
                dr.NextResult();
                // Output the rows of the second result set
                while (dr.Read())
                    vmresult.TotalCount = Convert.ToInt32(dr[0]);

            }
            finally
            {
                CloseConnection();
            }

            return vmresult;
        }

        public VMDataTableResponse GetMultipleResultsList2(string commandText, Dictionary<string, object> parameters)
        {

            VMDataTableResponse vmresult = new VMDataTableResponse();
            if (String.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("Command text cannot be null or empty.");
            }
            try
            {
                OpenConnection();
                //SqlTransaction tran = _connection.BeginTransaction();
                List<Dictionary<string, string>> resultset = new List<Dictionary<string, string>>();
                List<Dictionary<string, string>> Addressresultset = new List<Dictionary<string, string>>();
                List<Dictionary<string, string>> Addressresultset2 = new List<Dictionary<string, string>>();
                List<Dictionary<string, string>> Addressresultset3 = new List<Dictionary<string, string>>();
                List<Dictionary<string, string>> Addressresultset4 = new List<Dictionary<string, string>>();
                List<Dictionary<string, string>> Addressresultset5 = new List<Dictionary<string, string>>();
                List<Dictionary<string, string>> Addressresultset1 = new List<Dictionary<string, string>>();

                var command = CreateCommand(commandText, parameters);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = command.ExecuteReader();

                // Output the rows of the first result set
                while (dr.Read())
                {
                    var row = new Dictionary<string, string>();
                    for (var i = 0; i < dr.FieldCount; i++)
                    {
                        var columnName = dr.GetName(i);
                        var columnValue = dr.IsDBNull(i) ? null : dr.GetValue(i).ToString();
                        row.Add(columnName, columnValue);
                    }
                    resultset.Add(row);
                }
                vmresult.Resultset = resultset;
                // Switch to the second result set
                dr.NextResult();
                // Output the rows of the second result set
                while (dr.Read())
                {
                    var row = new Dictionary<string, string>();
                    for (var i = 0; i < dr.FieldCount; i++)
                    {
                        var columnName = dr.GetName(i);
                        var columnValue = dr.IsDBNull(i) ? null : dr.GetValue(i).ToString();
                        row.Add(columnName, columnValue);
                    }
                    Addressresultset.Add(row);
                }
                vmresult.Addressset = Addressresultset;
                // vmresult.TotalCount = Convert.ToInt32(dr[0]);
                dr.NextResult();
                // Output the rows of the Third result set
                while (dr.Read())
                {
                    var row = new Dictionary<string, string>();
                    for (var i = 0; i < dr.FieldCount; i++)
                    {
                        var columnName = dr.GetName(i);
                        var columnValue = dr.IsDBNull(i) ? null : dr.GetValue(i).ToString();
                        row.Add(columnName, columnValue);
                    }
                    Addressresultset2.Add(row);
                }
                vmresult.Addressset2 = Addressresultset2;

                dr.NextResult();
                // Output the rows of the Third result set
                while (dr.Read())
                {
                    var row = new Dictionary<string, string>();
                    for (var i = 0; i < dr.FieldCount; i++)
                    {
                        var columnName = dr.GetName(i);
                        var columnValue = dr.IsDBNull(i) ? null : dr.GetValue(i).ToString();
                        row.Add(columnName, columnValue);
                    }
                    Addressresultset3.Add(row);
                }
                vmresult.Addressset3 = Addressresultset3;

                dr.NextResult();
                // Output the rows of the Third result set
                while (dr.Read())
                {
                    var row = new Dictionary<string, string>();
                    for (var i = 0; i < dr.FieldCount; i++)
                    {
                        var columnName = dr.GetName(i);
                        var columnValue = dr.IsDBNull(i) ? null : dr.GetValue(i).ToString();
                        row.Add(columnName, columnValue);
                    }
                    Addressresultset4.Add(row);
                }
                vmresult.Addressset4 = Addressresultset4;

                dr.NextResult();
                // Output the rows of the Third result set
                while (dr.Read())
                {
                    var row = new Dictionary<string, string>();
                    for (var i = 0; i < dr.FieldCount; i++)
                    {
                        var columnName = dr.GetName(i);
                        var columnValue = dr.IsDBNull(i) ? null : dr.GetValue(i).ToString();
                        row.Add(columnName, columnValue);
                    }
                    Addressresultset5.Add(row);
                }
                vmresult.Addressset5 = Addressresultset5;
                dr.NextResult();
                while (dr.Read())
                {
                    var row = new Dictionary<string, string>();
                    for (var i = 0; i < dr.FieldCount; i++)
                    {
                        var columnName = dr.GetName(i);
                        var columnValue = dr.IsDBNull(i) ? null : dr.GetValue(i).ToString();
                        row.Add(columnName, columnValue);
                    }
                    Addressresultset1.Add(row);
                }
                vmresult.Addressset1 = Addressresultset1;
            }
            finally
            {
                CloseConnection();
            }

            return vmresult;
        }

        public List<int> GetAllParentCategoryIDs(List<int> categoryID)
        {
            List<int> lst = new List<int>();
            for (int item = 0; item < categoryID.Count; item++)
            {
                int id = Convert.ToInt32(categoryID[item]);
                //int? pid = dbContext.iH_Categories.Find(id).ParentID;

                try
                {
                    OpenConnection();
                    //  var command = CreateCommand(commandText, parameters);
                    //result = command.ExecuteScalar();


                    string cmdtxt = "SELECT \"ParentID\" FROM \"iH_Categories\" where \"ID\"=" + id;
                    Dictionary<string, object> parms = new Dictionary<string, object>() { };
                    var command1 = CreateCommand(cmdtxt, parms);
                    var pid = Convert.ToInt32(command1.ExecuteScalar());


                    // var pid = Convert.ToInt32(CreateCommand(commandText, parameters););
                    var categoryid = id;
                    while (pid != 0)
                    {
                        categoryid = pid;
                        string cmdtxt2 = "SELECT \"ParentID\" FROM \"iH_Categories\" where \"ID\"=" + pid;
                        Dictionary<string, object> parms2 = new Dictionary<string, object>() { };
                        var command2 = CreateCommand(cmdtxt2, parms2);
                        pid = Convert.ToInt32(command2.ExecuteScalar());
                        //pid = Convert.ToInt32(_database.QueryValue(cmdtxt2, parms2));
                    }
                    if (!lst.Contains(categoryid))
                    {
                        lst.Add(Convert.ToInt32(categoryid));
                    }
                }
                finally
                {
                    CloseConnection();
                }

            }
            return lst;
        }

        public VMDataTableResponse GetMultipleResultsListAll(string commandText, Dictionary<string, object> parameters)
        {

            VMDataTableResponse vmresult = new VMDataTableResponse();
            if (String.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("Command text cannot be null or empty.");
            }
            try
            {
                OpenConnection();
                //SqlTransaction tran = _connection.BeginTransaction();
                List<Dictionary<string, string>> resultset = new List<Dictionary<string, string>>();
                List<Dictionary<string, string>> Addressresultset = new List<Dictionary<string, string>>();
                List<Dictionary<string, string>> Addressresultset2 = new List<Dictionary<string, string>>();
                List<Dictionary<string, string>> Addressresultset3 = new List<Dictionary<string, string>>();
                var command = CreateCommand(commandText, parameters);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = command.ExecuteReader();

                // Output the rows of the first result set
                while (dr.Read())
                {
                    var row = new Dictionary<string, string>();
                    for (var i = 0; i < dr.FieldCount; i++)
                    {
                        var columnName = dr.GetName(i);
                        var columnValue = dr.IsDBNull(i) ? null : dr.GetValue(i).ToString();
                        row.Add(columnName, columnValue);
                    }
                    resultset.Add(row);
                }
                vmresult.Resultset = resultset;
                // Switch to the second result set
                dr.NextResult();
                // Output the rows of the second result set
                while (dr.Read())
                {
                    var row = new Dictionary<string, string>();
                    for (var i = 0; i < dr.FieldCount; i++)
                    {
                        var columnName = dr.GetName(i);
                        var columnValue = dr.IsDBNull(i) ? null : dr.GetValue(i).ToString();
                        row.Add(columnName, columnValue);
                    }
                    Addressresultset.Add(row);
                }
                vmresult.Addressset = Addressresultset;
                // vmresult.TotalCount = Convert.ToInt32(dr[0]);
                dr.NextResult();
                // Output the rows of the Third result set
                while (dr.Read())
                {
                    var row = new Dictionary<string, string>();
                    for (var i = 0; i < dr.FieldCount; i++)
                    {
                        var columnName = dr.GetName(i);
                        var columnValue = dr.IsDBNull(i) ? null : dr.GetValue(i).ToString();
                        row.Add(columnName, columnValue);
                    }
                    Addressresultset2.Add(row);
                }
                vmresult.Addressset2 = Addressresultset2;

                dr.NextResult();
                // Output the rows of the Third result set
                while (dr.Read())
                {
                    var row = new Dictionary<string, string>();
                    for (var i = 0; i < dr.FieldCount; i++)
                    {
                        var columnName = dr.GetName(i);
                        var columnValue = dr.IsDBNull(i) ? null : dr.GetValue(i).ToString();
                        row.Add(columnName, columnValue);
                    }
                    Addressresultset3.Add(row);
                }
                vmresult.Addressset3 = Addressresultset3;
            }
            finally
            {
                CloseConnection();
            }

            return vmresult;
        }


        public VMResponseForSp BusinessResultset(string commandText, Dictionary<string, object> parameters)
        {
            VMResponseForSp Rtn = new VMResponseForSp();
            if (String.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("Command text cannot be null or empty.");
            }
            try
            {
                OpenConnection();
                List<Dictionary<string, string>> resultset = new List<Dictionary<string, string>>();
                var command = CreateCommand(commandText, parameters);
                command.CommandType = CommandType.StoredProcedure;
                using (var reader = command.ExecuteReader())
                {
                    int i = 1;
                    while (!reader.IsClosed)
                    {
                        var dataTable = new DataTable();
                        dataTable.Load(reader, LoadOption.Upsert);
                        if (i == 1)
                        {
                            Rtn.Resultset = GetDataTableDictionaryList(dataTable);
                        }
                        else if (i == 2)
                        {
                            Rtn.ResultsetTwo = GetDataTableDictionaryList(dataTable);
                        }
                        else if (i == 3)
                        {
                            Rtn.ResultsetThree = GetDataTableDictionaryList(dataTable);
                        }
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                string s = string.Join(",", parameters.Select(x => x.Key + "=" + x.Value).ToArray());
                CommonMethods.LogError(ex);
                throw new ArgumentException(ex.Message);
            }
            finally
            {
                CloseConnection();
            }

            return Rtn;
        }
        public static List<Dictionary<string, object>> GetDataTableDictionaryList(DataTable dt)
        {
            try
            {
                return dt.AsEnumerable().Select(
                row => dt.Columns.Cast<DataColumn>().ToDictionary(
                    column => column.ColumnName,
                    column => row[column]
                )).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
  