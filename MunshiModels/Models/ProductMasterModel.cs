using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunshiModels.Models
{
    public class ProductMasterModel : RequestResponseModel
    {
        public int? Productid { get; set; }

        public String ProductName { get; set; }
        public int UOM { get; set; }
        public int Color { get; set; }

        public int Texture { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDelete { get; set; }
        public DateTime SafeLifeInGodown { get; set; }
        public string ProcessId { get; set; }
        public string [] ProcessIds {get;set;}
        public int BuyProductId { get; set;}
        public int BuyProductPacking { get; set; }
        public Guid CompanyId { get; set; }
        public int Catagory { get; set; }

       
    }
}
