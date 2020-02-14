using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunshiModels.Models
{
    public class GeneralMasterModel:RequestResponseModel
    {
        public int GeneralMasterId { get; set; }
        public int GeneralMasterClassId { get; set; }
        public string GeneralMasterName { get; set; }
        public string GeneralMasterClassName { get; set; }
        public string GeneralMasterCode { get; set; }
        public string GeneralMasterSquence { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public Guid? CompanyId { get; set; }

    }
}
