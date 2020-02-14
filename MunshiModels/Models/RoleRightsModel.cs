using System;

namespace MunshiModels.Models
{
    public class RoleRightsModel: RequestResponseModel
    {
        public int RoleRightsId { get; set; }
        public int RoleId { get; set; }
        public string  RoleName { get; set; }

        public int ModuleId { get; set; }

        public string ModuleName { get; set; }
        public string ModuleCode { get; set; }
        public string ModuleImagePath { get; set; }
        public string ModuleOrder { get; set; }
        public string ModuleURL { get; set; }
        public int PrecedingModuleId { get; set; }
        public bool View { get; set; }
        public bool Edit { get; set; }
        public bool Create { get; set; }
        public bool Delete { get; set; }
        public Guid? CompanyId { get; set; }

    }
}
