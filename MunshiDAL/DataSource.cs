using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Web;
using System.Data;
using System.Security;
using System.Security.Cryptography;

namespace MunshiDAL
{
    public static class DataSource
    {
        #region "Contants"
        const string EnteringProcedure = ">>>>";
        const string ExitingProcedure = "<<<<";
        const string ErrorIn = "Error in - ";
        const string Dash = " - ";
        #endregion

        #region "Properties"
        public static string ServerName { get; set; }
        public static string DatabaseName { get; set; }
        public static string UserName { get; set; }
        public static string Password { get; set; }
        public static int Port { get; set; }

        #endregion

        #region "Public Methods"
        public static SqlConnection GetConnection(string serverName, string databaseName, string userName, string password)
        {
            SqlConnection cn = null;
            const string ProcName = "GetConnection";
            try
            {
                if (serverName.Length == 0 || databaseName.Length == 0 || userName.Length == 0)
                {
                    throw (new Exception("Either ServerName or DatabaseName or UserName is blank"));
                }
                else
                {
                    string connStr = GetSqlConnectionString(serverName, databaseName, userName, password);
                    cn = new SqlConnection(connStr);
                    cn.Open();
                    return cn;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
        }
        public static SqlConnection GetConnection(string dbcredential)
        {
            string serverName = string.Empty;
            string databaseName = string.Empty;
            string userName = string.Empty;
            string password = string.Empty;

            SqlConnection cn = null;
            const string ProcName = "GetConnection";
            try
            {

                // Decript the dbcrential string into normal string
                // slipt the normal string by ":"
                // Decript the First , third and fourth element

                //dbcredential = EncryptDecryptString.EncryptDecryptString.Decrypt(dbcredential);
                string[] dbcredentialArray = dbcredential.Split(':');
                //serverName = EncryptDecryptString.EncryptDecryptString.Decrypt(dbcredentialArray[0]);
                //userName = EncryptDecryptString.EncryptDecryptString.Decrypt(dbcredentialArray[2]);
                //password = EncryptDecryptString.EncryptDecryptString.Decrypt(dbcredentialArray[3]);
                serverName = dbcredentialArray[0];
                userName = dbcredentialArray[2];
                password = dbcredentialArray[3];
                databaseName = dbcredentialArray[1];


                if (serverName.Length == 0 || databaseName.Length == 0 || userName.Length == 0)
                {
                    throw (new Exception("Either ServerName or DatabaseName or UserName is blank"));
                }
                else
                {
                    string connStr = GetSqlConnectionString(serverName, databaseName, userName, password);
                    cn = new SqlConnection(connStr);
                    cn.Open();
                    return cn;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public static SqlConnection GetConnection()
        {
            SqlConnection cn = null;
            const string ProcName = "GetConnection";
            try
            {
                if (ServerName.Length == 0 || DatabaseName.Length == 0 || UserName.Length == 0)
                {
                    throw (new Exception("Either ServerName or DatabaseName or UserName is blank"));
                }
                else
                {
                    string connStr = GetSqlConnectionString(ServerName, DatabaseName, UserName, Password);
                    cn = new SqlConnection(connStr);
                    cn.Open();
                    return cn;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public static void CloseConnection(SqlConnection cn)
        {
            const string ProcName = "CloseConnection";
            try
            {
                if (cn != null && cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
        }
        public static string GetSqlConnectionString(string serverName, string databaseName, string userId, string password)
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
            // Set the properties for the data source.

            sqlBuilder.DataSource = serverName;
            sqlBuilder.InitialCatalog = databaseName;
            //sqlBuilder.IntegratedSecurity = true ;
            sqlBuilder.UserID = userId;
            sqlBuilder.Password = password;

            sqlBuilder.IntegratedSecurity = false;
            string providerString = sqlBuilder.ToString();


            return providerString;

        }
        #endregion
    }
}
