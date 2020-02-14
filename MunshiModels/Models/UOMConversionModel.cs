using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunshiModels.Models
{
   public  class UOMConversionModel:RequestResponseModel
    {
        public int SourceUOMId { get; set; }
        public int DestinationUOMId { get; set; }
        public decimal SourceConversionRation { get; set; }
        public decimal DestinationConversionRation { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public Guid? CompanyId { get; set; }
    }
}
