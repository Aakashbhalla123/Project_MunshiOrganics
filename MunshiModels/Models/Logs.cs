using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MunshiModels.Models
{
    public class Logs
    {

        #region Logs - Properties
        public int ID { get; set; }
        public string ProcedureName { get; set; }
        public string LogMessage { get; set; }
        public DateTime LogDate { get; set; }
        #endregion
    }
}