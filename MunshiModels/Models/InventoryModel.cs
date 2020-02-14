using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunshiModels.Models
{
     public class InventoryModel:RequestResponseModel
    {
        public int ReciptNo { get; set; }
        public int NewReciptNo { get; set; }
        public int FarmerId { get; set; }
        public int LoginId { get; set; }
        public String PersonName { get; set; }
       
        public int Quantity { get; set; }
        public string  Unit { get; set; }
        public Guid ComapnyId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime Current { get; set; }
       public bool IsDelete { get; set; }
        public string RawMaterial { get; set; }
        public int Storage { get; set; }
    }
}
