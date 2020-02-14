using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunshiModels.Models
{
    public class GeneralMasterClass:RequestResponseModel
    {
        public int GeneralMasterClassId { get; set; }
        public string GeneralMasterClassName { get; set; }
        public Guid? CompanyId { get; set; }
        public int GeneralClassId { get; set; }

    }
}
