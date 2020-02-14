using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunshiModels.Models
{
    public class RequestResponseModel
    {
        /// <summary>
        /// request values
        /// {"requesttype"  : "0 means root call,...",
        ///  "searchby"     : "all(default) , category , ...",
        ///  "searchstring" : "user define string value",
        ///  "orderby"      : "name(default), rate, category,...",
        ///  "itemperpage"  : "20 (default), 50, 100",
        ///  "requestpageno": "1 (default), user define number",
        ///  "currentpageno": "1 (default)",
        /// </summary>
        public int RequestType { get; set; }
        public string SearchBy { get; set; }
        public string SearchString { get; set; }
        public string OrderBy { get; set; }
        public int ItemsPerPage { get; set; }
        public int RequestPageNo { get; set; }
        public int CurrentPageNo { get; set; }

        public int? IntID { get; set; }

        public Guid? Id { get; set; }

        /// <summary>
        /// responce values
        ///  "returncode"   : "0 (success), non zero (fail)"
        ///  "returnmessage": "exception - System genereate message / Success"}+ 
        /// </summary>
        public int ReturnCode { get; set; }
        public string ReturnMessage { get; set; }
    }
}
