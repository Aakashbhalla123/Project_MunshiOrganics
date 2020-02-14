using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MunshiModels.Models
{
    public class DBCredentialValues
    {
        public string ServerName { get; set; }
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Guid? CurrentCompID { get; set; }
        public string CredentialValues { get; set; }
        public int Port { get; set; }
    }
}