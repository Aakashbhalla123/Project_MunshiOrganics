using log4net;
using MunshiDAL;
using MunshiModels.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;

namespace MunshiAPI.Controllers
{
    public class RoelsController : ApiController
    {
        private static ILog m_Logger = LogManager.GetLogger(typeof(RoelsController));

        #region Roles Insert Update Select

        [Route("api/Roles/Roles_List")]
        [HttpPost]
        public IList<RolesModel> Roles_List(ArrayList paramList)
        {
            RolesModel apiObject = new RolesModel();
            string strResult = "";
            IList<RolesModel> apiObjectList = null;
            apiObject = Newtonsoft.Json.JsonConvert.DeserializeObject<RolesModel>(paramList[0].ToString());
            /// =============
            apiObjectList = fnRoles_List(apiObject, ref strResult);
            return apiObjectList;
        }

        private IList<RolesModel> fnRoles_List(RolesModel apiObject, ref string strResult)
        {
            string strReturnCode = "000";
            string strReturnMsg = "UnDefined";
            string crCnString = UtilityLib.GetConnectionString();
            IList<RolesModel> objFieldClassModelList = new List<RolesModel>();

            DataSet usersInfoDS = DAL_Roles.Roles_List(crCnString, apiObject.RequestType, apiObject.SearchBy, apiObject.SearchString,apiObject.IntID,
                apiObject.UnderPrecedingRoleId, apiObject.CompanyId, apiObject.ItemsPerPage, apiObject.RequestPageNo, apiObject.CurrentPageNo);

            DataTable usersInfoDT = usersInfoDS.Tables[0];
            if (usersInfoDT != null && usersInfoDT.Rows.Count > 0)
            {
                strReturnCode = "001";
                strReturnMsg = "Success";
                foreach (DataRow dr in usersInfoDT.Rows)
                {
                    apiObject = new RolesModel();
                    apiObject.RoleId = UtilityLib.FormatNumber(dr["RoleId"].ToString());
                    apiObject.RoleName = (string)dr["RoleName"];
                    apiObject.RoleCode = (string)dr["RoleCode"];
                    apiObject.AuthorizationRequired= UtilityLib.FormatBoolean(dr["AuthorizationRequired"].ToString());
                    apiObject.UnderPrecedingRoleId = UtilityLib.FormatNumber(dr["UnderPrecedingRoleId"].ToString());
                    apiObject.CompanyId = (Guid)(dr["CompanyId"]);
                    objFieldClassModelList.Add(apiObject);
                }
            }
            else
            {
                strReturnCode = "002";
                strReturnMsg = "Fail-Record Not Found";
            }
            strResult = strReturnCode + "|" + strReturnMsg;
            return objFieldClassModelList;
        }

        [Route("api/Roles/Roles_InsertUpdate")]
        [HttpPost]
        public RolesModel Roles_InsertUpdate(ArrayList paramList)
        {
            string strResult = "";
            string strReturnCode = "000";
            string strReturnMsg = "UnDefined";
            RolesModel apiObject = new RolesModel();

            apiObject = Newtonsoft.Json.JsonConvert.DeserializeObject<RolesModel>(paramList[0].ToString());
            string crCnString = UtilityLib.GetConnectionString();

            int RoleInfo = DAL_Roles.Roles_InsertUpdate(crCnString, apiObject.RoleId, apiObject.RoleName,apiObject.RoleCode,apiObject.UnderPrecedingRoleId,
                apiObject.AuthorizationRequired, apiObject.CompanyId);

            if (RoleInfo == 0)
            {
                apiObject.ReturnCode = RoleInfo;
                apiObject.ReturnMessage = "Role Added Successfully";
            }
            else if (RoleInfo == 1)
            {
                apiObject.ReturnCode = RoleInfo;
                apiObject.ReturnMessage = "Role already exists";
            }
            else if (RoleInfo == 101)
            {
                apiObject.ReturnCode = RoleInfo;
                apiObject.ReturnMessage = "Role updated successfully";
            }
            else if (RoleInfo == 2)
            {
                apiObject.ReturnCode = RoleInfo;
                apiObject.ReturnMessage = "record is already updated by someone else";
            }
            else
            {
                apiObject.ReturnCode = RoleInfo;
                apiObject.ReturnMessage = "Fail-Record Not Inserted";
            }
            strResult = strReturnCode + "|" + strReturnMsg;
            return apiObject;
        }
        #endregion

        #region Roles Right Insert Update Select

        [Route("api/RoleRight/Rights_List")]
        [HttpPost]
        public IList<RoleRightsModel> Rights_List(ArrayList paramList)
        {
            RoleRightsModel apiObject = new RoleRightsModel();
            string strResult = "";
            IList<RoleRightsModel> apiObjectList = null;
            apiObject = Newtonsoft.Json.JsonConvert.DeserializeObject<RoleRightsModel>(paramList[0].ToString());
            /// =============
            apiObjectList = fnRights_List(apiObject, ref strResult);
            return apiObjectList;
        }

        private IList<RoleRightsModel> fnRights_List(RoleRightsModel apiObject, ref string strResult)
        {
            string strReturnCode = "000";
            string strReturnMsg = "UnDefined";
            string crCnString = UtilityLib.GetConnectionString();
            IList<RoleRightsModel> objRightsModelList = new List<RoleRightsModel>();

            DataSet usersInfoDS = DAL_RoleRights.Rights_List(crCnString, apiObject.RequestType, apiObject.SearchBy, apiObject.SearchString, apiObject.IntID,
                apiObject.RoleId,apiObject.ModuleId, apiObject.CompanyId, apiObject.ItemsPerPage, apiObject.RequestPageNo, apiObject.CurrentPageNo);

            DataTable usersInfoDT = usersInfoDS.Tables[0];
            if (usersInfoDT != null && usersInfoDT.Rows.Count > 0)
            {
                strReturnCode = "001";
                strReturnMsg = "Success";
                foreach (DataRow dr in usersInfoDT.Rows)
                {
                    apiObject = new RoleRightsModel();
                    apiObject.RoleRightsId = UtilityLib.FormatNumber(dr["RoleRightsId"].ToString());
                    apiObject.RoleId = UtilityLib.FormatNumber(dr["RoleId"].ToString());
                    apiObject.RoleName = (string)dr["RoleName"];
                    apiObject.ModuleId = UtilityLib.FormatNumber(dr["ModuleId"].ToString());
                    apiObject.ModuleName = (string)dr["ModuleName"];
                    apiObject.View = UtilityLib.FormatBoolean(dr["View"].ToString());
                    apiObject.Edit = UtilityLib.FormatBoolean(dr["Edit"].ToString());
                    apiObject.Create = UtilityLib.FormatBoolean(dr["Create"].ToString());
                    apiObject.Delete = UtilityLib.FormatBoolean(dr["Delete"].ToString());
                    apiObject.CompanyId = (Guid)(dr["CompanyId"]);
                    objRightsModelList.Add(apiObject);
                }
            }
            else
            {
                strReturnCode = "002";
                strReturnMsg = "Fail-Record Not Found";
            }
            strResult = strReturnCode + "|" + strReturnMsg;
            return objRightsModelList;
        }

        [Route("api/RoleRight/Rights_InsertUpdate")]
        [HttpPost]
        public RoleRightsModel Rights_InsertUpdate(ArrayList paramList)
        {
            string strResult = "";
            string strReturnCode = "000";
            string strReturnMsg = "UnDefined";
            RoleRightsModel apiObject = new RoleRightsModel();

            apiObject = Newtonsoft.Json.JsonConvert.DeserializeObject<RoleRightsModel>(paramList[0].ToString());
            string crCnString = UtilityLib.GetConnectionString();

            int RightsInfo = DAL_RoleRights.Rights_InsertUpdate(crCnString,apiObject.RoleRightsId,apiObject.RoleId, apiObject.ModuleId, apiObject.View,
                apiObject.Edit,apiObject.Create,apiObject.Delete, apiObject.CompanyId);

            if (RightsInfo == 0)
            {
                apiObject.ReturnCode = RightsInfo;
                apiObject.ReturnMessage = "Rights Added Successfully";
            }
            else if (RightsInfo == 1)
            {
                apiObject.ReturnCode = RightsInfo;
                apiObject.ReturnMessage = "Rights already exists";
            }
            else if (RightsInfo == 101)
            {
                apiObject.ReturnCode = RightsInfo;
                apiObject.ReturnMessage = "Rights updated successfully";
            }
            else if (RightsInfo == 2)
            {
                apiObject.ReturnCode = RightsInfo;
                apiObject.ReturnMessage = "record is already updated by someone else";
            }
            else
            {
                apiObject.ReturnCode = RightsInfo;
                apiObject.ReturnMessage = "Fail-Record Not Inserted";
            }
            strResult = strReturnCode + "|" + strReturnMsg;
            return apiObject;
        }
        #endregion

        #region Show Menu List BasedOnRoles

        [Route("api/RoleRight/RightsBasedMenu")]
        [HttpPost]
        public IList<RoleRightsModel> ShowModules_RightsBased(ArrayList paramList)
        {
            RoleRightsModel apiObject = new RoleRightsModel();
            string strResult = "";
            IList<RoleRightsModel> apiObjectList = null;
            apiObject = Newtonsoft.Json.JsonConvert.DeserializeObject<RoleRightsModel>(paramList[0].ToString());
            /// =============
            apiObjectList = fnRightsBasedMenu_List(apiObject, ref strResult);
            return apiObjectList;
        }

        private IList<RoleRightsModel> fnRightsBasedMenu_List(RoleRightsModel apiObject, ref string strResult)
        {
            string strReturnCode = "000";
            string strReturnMsg = "UnDefined";
            string crCnString = UtilityLib.GetConnectionString();
            IList<RoleRightsModel> objRightsModelList = new List<RoleRightsModel>();

            DataSet usersInfoDS = DAL_RoleRightsBasedMenu.ShowModules_RightsBased(crCnString, apiObject.RequestType,apiObject.RoleId, apiObject.CompanyId);

            DataTable usersInfoDT = usersInfoDS.Tables[0];
            if (usersInfoDT != null && usersInfoDT.Rows.Count > 0)
            {
                strReturnCode = "001";
                strReturnMsg = "Success";
                foreach (DataRow dr in usersInfoDT.Rows)
                {
                    apiObject = new RoleRightsModel();
                    apiObject.RoleRightsId = UtilityLib.FormatNumber(dr["RoleRightsId"].ToString());
                    apiObject.RoleId = UtilityLib.FormatNumber(dr["RoleId"].ToString());
                    apiObject.RoleName = (string)dr["RoleName"];
                    apiObject.ModuleName = (string)dr["ModuleName"];
                    //apiObject.ModuleOrder = (string)dr["ModuleOrder"];
                    apiObject.ModuleURL = (string)dr["ModuleURL"];
                    apiObject.ModuleCode = (string)dr["ModuleCode"];
                    apiObject.ModuleImagePath = (string)dr["ModuleImagePath"];
                    apiObject.ModuleId = UtilityLib.FormatNumber(dr["ModuleId"].ToString());
                    apiObject.PrecedingModuleId = UtilityLib.FormatNumber(dr["PrecedingModuleId"].ToString());
                    apiObject.View = UtilityLib.FormatBoolean(dr["View"].ToString());
                    apiObject.Edit = UtilityLib.FormatBoolean(dr["Edit"].ToString());
                    apiObject.Create = UtilityLib.FormatBoolean(dr["Create"].ToString());
                    apiObject.Delete = UtilityLib.FormatBoolean(dr["Delete"].ToString());
                    apiObject.CompanyId = (Guid)(dr["CompanyId"]);
                    objRightsModelList.Add(apiObject);
                }
            }
            else
            {
                strReturnCode = "002";
                strReturnMsg = "Fail-Record Not Found";
            }
            strResult = strReturnCode + "|" + strReturnMsg;
            return objRightsModelList;
        }
        #endregion
    }
}
