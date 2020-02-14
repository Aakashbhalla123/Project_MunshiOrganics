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
    public class ModulesController : ApiController
    {
        private static ILog m_Logger = LogManager.GetLogger(typeof(ModulesController));

        //#region Roles Insert Update Select

        //[Route("api/Modules/Modules_List")]
        //[HttpPost]
        //public IList<ModuleMasterModel> Modules_List(ArrayList paramList)
        //{
        //    ModuleMasterModel apiObject = new ModuleMasterModel();
        //    string strResult = "";
        //    IList<ModuleMasterModel> apiObjectList = null;
        //    apiObject = Newtonsoft.Json.JsonConvert.DeserializeObject<ModuleMasterModel>(paramList[0].ToString());
        //    /// =============
        //    apiObjectList = fnModules_List(apiObject, ref strResult);
        //    return apiObjectList;
        //}

        //private IList<ModuleMasterModel> fnModules_List(ModuleMasterModel apiObject, ref string strResult)
        //{
        //    string strReturnCode = "000";
        //    string strReturnMsg = "UnDefined";
        //    string crCnString = UtilityLib.GetConnectionString();
        //    IList<ModuleMasterModel> objFieldClassModelList = new List<ModuleMasterModel>();

        //    DataSet usersInfoDS = DAL_ModuleMaster.Modules_List(crCnString, apiObject.RequestType, apiObject.SearchBy, apiObject.SearchString, apiObject.IntID,
        //         apiObject.CompanyId, apiObject.ItemsPerPage, apiObject.RequestPageNo, apiObject.CurrentPageNo);

        //    DataTable usersInfoDT = usersInfoDS.Tables[0];
        //    if (usersInfoDT != null && usersInfoDT.Rows.Count > 0)
        //    {
        //        strReturnCode = "001";
        //        strReturnMsg = "Success";
        //        foreach (DataRow dr in usersInfoDT.Rows)
        //        {
        //            apiObject = new ModuleMasterModel();
        //            apiObject.ModuleId = UtilityLib.FormatNumber(dr["ModuleId"].ToString());
        //            apiObject.ModuleName = (string)dr["ModuleName"];
        //            apiObject.ModuleCode = (string)dr["ModuleCode"];
        //            apiObject.PrecedingModuleId = UtilityLib.FormatNumber(dr["PrecedingModuleId"].ToString());
        //            apiObject.ModuleOrder = (string)dr["ModuleOrder"];
        //            apiObject.ModuleURL = (string)dr["ModuleURL"];
        //            apiObject.ModuleImagePath = (string)dr["ModuleImagePath"];
        //            apiObject.CompanyId = (Guid)(dr["CompanyId"]);
        //            apiObject.ReturnMessage=strReturnMsg;
        //            objFieldClassModelList.Add(apiObject);
        //        }
        //    }
        //    else
        //    {
        //        strReturnCode = "002";
        //        strReturnMsg = "Fail-Record Not Found";
        //        apiObject.ReturnMessage = strReturnMsg;
        //    }
        //    strResult = strReturnCode + "|" + strReturnMsg;
        //    return objFieldClassModelList;
        //}
        //#endregion

    }
}
