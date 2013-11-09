using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.IO;


namespace YourProjectName.Class
{
  static class clsDatabase
  {
    #region Global Variables
    /* Set first the connection string
     * or default connection when 
     * clsDatabase is called
    */
    static string conStrRemote = "";
    static string conStrLocal = "";
    
    // Declare SqlConnection and SqlCeConnection
    internal static SqlConnection objSqlConnection;
    internal static SqlCeConnection objSQlCeConnection;
    #endregion
    
    static DatabaseClass()
    {
        objSqlCeConnection = new SqlCeConnection(conStrLocal);
        objSqlConnection = new SqlConnection(conStrRemote);
    }
    
    #region SQL Methods
    public static bool OpenSqlConnection()
    {
        try
        {
            if (objSqlConnection.State != System.Data.ConnectionState.Open)
            {
                objSqlConnection.Open();
            }
            return (true);
        }
        catch (SqlException ex)
        {
            return (false);
        }
    }
    
    public static bool CloseSqlConnection()
    {
        try
        {
            if (objSqlConnection.State != System.Data.ConnectionState.Closed)
            {
                objSqlConnection.Close();
            }
            return (true);
        }
        catch (SqlException ex)
        {
            return (false);
        }
    }
	
	public static int ExecuteNonQuary(string commandText, CommandType cmdType, SqlParameter[] spParams)
        {
            int result = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = commandText;
            if (cmdType == CommandType.StoredProcedure)
            {
                cmd.CommandType = CommandType.StoredProcedure;
            }
            if (spParams != null)
            {
                for (int i = 0; i < spParams.Length; i++)
                {
                    cmd.Parameters.Add(spParams[i]);
                }
            }
            cmd.Connection = objSqlConnection;
            try
            {
                OpenSqlConnection();
                result = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
            }
            finally
            {
                CloseSqlConnection();
            }
            return result;
        }

        public static int ExecuteNonQuary(string commandText, CommandType cmdType, SqlParameter[] spParams, SqlTransaction tran)
        {
            int result = 0;
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandText = commandText;
            sqlCmd.Connection = objSqlConnection;
            sqlCmd.Transaction = tran;
            if (cmdType == CommandType.StoredProcedure)
            {
                sqlCmd.CommandType = CommandType.StoredProcedure;
            }
            if (spParams != null)
            {
                for (int i = 0; i < spParams.Length; i++)
                {
                    sqlCmd.Parameters.Add(spParams[i]);
                }
            }
            try
            {
                result = sqlCmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {

            }
            return result;
        }

        public static DataSet ExecuteDataSet(string spName, SqlParameter[] spParams)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandText = spName;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Connection = objSqlConnection;
                da.SelectCommand = sqlCmd;
                for (int i = 0; i < spParams.Length; i++)
                {
                    sqlCmd.Parameters.Add(spParams[i]);
                }
                da.Fill(ds);
            }
            catch (SqlException ex)
            { 
                //
            }
            return ds;
        }

        public static DataSet ExecuteDataSet(string commandText, CommandType cmdType)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandText = commandText;
                if (cmdType == CommandType.StoredProcedure)
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                }
                else if (cmdType == CommandType.TableDirect)
                {
                    sqlCmd.CommandType = CommandType.TableDirect;
                }
                else
                {
                    sqlCmd.CommandType = CommandType.Text;
                }
                sqlCmd.Connection = objSqlConnection;
                da.SelectCommand = sqlCmd;
                da.Fill(ds);
            }
            catch (SqlException)
            { }
            return ds;
        }

        public static SqlDataReader ExecuteReader(string _strCommand)
        {
            SqlDataReader objReader = null;
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandText = _strCommand;
            sqlCmd.Connection = objSqlConnection;
            try
            {
                objReader = sqlCmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            }
            catch (SqlException)
            { }
            return (objReader);
        }

        public static SqlDataReader ExecuteReader(string commandText, CommandType cmdType)
        {
            SqlDataReader objReader = null; 
            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandText = commandText;
                if (cmdType == CommandType.StoredProcedure)
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                }
                else if (cmdType == CommandType.TableDirect)
                {
                    sqlCmd.CommandType = CommandType.TableDirect;
                }
                else
                {
                    sqlCmd.CommandType = CommandType.Text;
                }
                sqlCmd.Connection = objSqlConnection;
                objReader = sqlCmd.ExecuteReader();
            }
            catch (SqlException)
            { }
            return objReader;
        }

        public static object ExecuteScaler(string commandText, CommandType cmdType)
        {
            object result = null;
            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = objSqlConnection;
                sqlCmd.CommandText = commandText;
                if (cmdType == CommandType.StoredProcedure)
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                }
                OpenSqlConnection();
                result = (object)sqlCmd.ExecuteScalar();
            }
            catch (SqlException)
            {
                
            }
            finally
            {
                CloseSqlConnection();
            }
            return result;
        }

        public static object ExecuteScaler(string commandText, CommandType cmdType, SqlParameter[] spParams)
        {
            object result = null;
            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = objSqlConnection;
                sqlCmd.CommandText = commandText;
                if (cmdType == CommandType.StoredProcedure)
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                }
                if (spParams != null)
                {
                    for (int i = 0; i < spParams.Length; i++)
                    {
                        sqlCmd.Parameters.Add(spParams[i]);
                    }
                }
                OpenSqlConnection();
                result = (object)sqlCmd.ExecuteScalar();
            }
            catch (SqlException ex)
            {
            }
            finally
            {
                CloseSqlConnection();
            }
            return result;
        }

        public static object ExecuteScaler(string commandText, CommandType cmdType, SqlParameter[] spParams, SqlTransaction sqlTran)
        {
            object result = null;
            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = objSqlConnection;
                sqlCmd.Transaction = sqlTran;
                sqlCmd.CommandText = commandText;
                if (cmdType == CommandType.StoredProcedure)
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                }
                if (spParams != null)
                {
                    for (int i = 0; i < spParams.Length; i++)
                    {
                        sqlCmd.Parameters.Add(spParams[i]);
                    }
                }
                result = (object)sqlCmd.ExecuteScalar();
            }
            catch (SqlException ex)
            {
            }
            return result;
        }

        public static bool CheckConnectionToServer()
        {
            bool result = false;
            try
            {
                objSqlConnection.Open();
                objSqlConnection.Close();
                result = true;
            }
            catch(SqlException ex)
            {
                //
            }
            return result;
        }

        public static void RefreshConnectionString()
        {
            objSqlConnection.ConnectionString = GetSQLRemoteConnectionString();
        }

        public static DateTime GetServerDateTime()
        {
            DateTime result = DateTime.MinValue;
            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = objSqlConnection;
                sqlCmd.CommandText = "SELECT GETDATE() AS ServerDateTime";
                OpenSqlConnection();
                result = (DateTime)sqlCmd.ExecuteScalar();
                CloseSqlConnection();
            }
            catch (SqlException)
            {
                CloseSqlConnection();
            }
            return result;
        }
    #endregion
  }
}
