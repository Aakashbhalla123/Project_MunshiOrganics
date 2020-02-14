using System;

namespace MunshiModels.Models
{
    public class FieldCodeModel : RequestResponseModel
    {
        public int FieldCodeId { get; set; }
        public int FieldClassId { get; set; }
        public string FieldClassName { get; set; }
        public string FieldCodeName { get; set; }
        public string FieldCodeAlias { get; set; }
        public string FieldCodeOrder { get; set; }
        public Guid? CompanyId { get; set; }
    }
}
