using System;

namespace MunshiModels.Models
{
    public class FieldClassModel : RequestResponseModel
    {
        public int FieldClassId { get; set; }
        public string FieldClassName { get; set; }
        public string FieldClassCode { get; set; }
        public Guid? CompanyId { get; set; }

    }
}
