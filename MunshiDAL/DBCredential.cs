using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MunshiDAL
{
    public class DBCredential
    {
        public string ServerName { get; set; }
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public Guid? CurrentCompID { get; set; }
       
        public DBCredential(string serverName, string databaseName, string userName, string password)
        {
            ServerName = serverName;
            DatabaseName = databaseName;
            UserName = userName;
            Password = password;
            Port = 0;
            CurrentCompID = null;
        }
        public DBCredential(string serverName, string databaseName, string userName, string password, int port)
        {
            ServerName = serverName;
            DatabaseName = databaseName;
            UserName = userName;
            Password = password;
            Port = port;
            CurrentCompID = null;
        }

        public DBCredential(string serverName, string databaseName, string userName, string password, int port =0 , Guid? currentCompId = null )
        {
            ServerName = serverName;
            DatabaseName = databaseName;
            UserName = userName;
            Password = password;
            Port = 0;
            CurrentCompID = currentCompId == null ? currentCompId : (Guid)currentCompId;
        }


    }
}
