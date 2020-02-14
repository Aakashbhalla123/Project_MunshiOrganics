using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunshiModels.Models
{
    public class ProcessMasterModel : RequestResponseModel
    {
        public int ProcessId { get; set; }
        public String ProcessName { get; set; }
        public bool WastageFlag { get; set; }
        public bool QuantityFlag { get; set; }
        public bool PackingFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDelete { get; set; }
        public float ProcessVolume { get; set; }
        public String ProcessDuration { get; set; }
        public int ProcessUnit { get; set; }
        public Guid ?CompanyId { get; set; }
        public bool ByProduct { get; set; }

    }
}
