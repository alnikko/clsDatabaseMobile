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
    #endregion
  }
}
