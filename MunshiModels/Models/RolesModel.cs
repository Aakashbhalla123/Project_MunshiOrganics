using System;

namespace MunshiModels.Models
{
    public class RolesModel : RequestResponseModel
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleCode { get; set; }
        public int UnderPrecedingRoleId { get; set; }
        public bool AuthorizationRequired { get; set; }
        public Guid? CompanyId { get; set; }

    }
}
