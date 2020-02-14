using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunshiModels.Models
{
    
        public class UOMMasterModel : RequestResponseModel
        {
            public int UOMId { get; set; }
            public int UOMTypeId { get; set; }
            public string UOMName { get; set; }
            public string UOMCode { get; set; }
            public int DecimalPoints { get; set; }
            public bool BaseUnit { get; set; }
            public decimal ConversionRation { get; set; }
            public Guid? CreatedBy { get; set; }
            public Guid? ModifiedBy { get; set; }
            public Guid? CompanyId { get; set; }
        }
    
}
