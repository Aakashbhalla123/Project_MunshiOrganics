using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using log4net;
using MunshiDAL;
using MunshiModels.Models;
using System.Data.SqlClient;
using System.Data;
using System.Collections;

namespace MunshiAPI.Controllers
{
    public class ProcessMasterController : ApiController
    {
        private static ILog m_Logger = LogManager.GetLogger(typeof(RoelsController));
        #region ProcessMaster Insert Update Select

        [Route("api/ProcessMaster/ProcessMaster_List")]
        [HttpPost]
        public IList<ProcessMasterModel> Product_List(ArrayList paramList)
        {
            ProcessMasterModel apiObject = new ProcessMasterModel();
            string strResult = "";
            IList<ProcessMasterModel> apiObjectList = null;
            apiObject = Newtonsoft.Json.JsonConvert.DeserializeObject<ProcessMasterModel>(paramList[0].ToString());
            apiObjectList = fnProcessMaster_List(apiObject, ref strResult);
            return apiObjectList;
        }
        private IList<ProcessMasterModel> fnProcessMaster_List(ProcessMasterModel apiObject, ref string strResult)
        {
            string strReturnCode = "000";
            string strReturnMsg = "UnDefined";
            string crCnString = UtilityLib.GetConnectionString();
            IList<ProcessMasterModel> objFieldClassModelList = new List<ProcessMasterModel>();
            DataSet usersInfoDS = DL_ProcessMaster.Process_List(crCnString, apiObject.RequestType, apiObject.SearchBy, apiObject.SearchString,
                    apiObject.ProcessId, apiObject.CompanyId, apiObject.ItemsPerPage, apiObject.RequestPageNo, apiObject.CurrentPageNo);
            DataTable usersInfoDT = usersInfoDS.Tables[0];
            if (usersInfoDT != null && usersInfoDT.Rows.Count > 0)
            {
                strReturnCode = "001";
                strReturnMsg = "Success";
                foreach (DataRow dr in usersInfoDT.Rows)
                {
                    apiObject = new ProcessMasterModel();
                    apiObject.ProcessId = UtilityLib.FormatNumber(dr["ProcessId"].ToString());
                    apiObject.ProcessName = (string)dr["ProcessName"];
                    apiObject.QuantityFlag = UtilityLib.FormatBoolean(dr["QuantityFlag"].ToString());
                    apiObject.ByProduct = UtilityLib.FormatBoolean(dr["ByProduct"].ToString());
                    apiObject.WastageFlag = UtilityLib.FormatBoolean(dr["WastageFlag"].ToString());
                    apiObject.QuantityFlag = UtilityLib.FormatBoolean(dr["QuantityFlag"].ToString());
                    apiObject.IsDelete = UtilityLib.FormatBoolean(dr["IsDelete"].ToString());
                    apiObject.ProcessDuration = UtilityLib.FormatDate(dr["ProcessDuration"]).ToString();
                    apiObject.ProcessVolume = UtilityLib.FormatFloat(dr["ProcessVolume"]);
                    //apiObject.CompanyId = (Guid)(dr["CompanyId"]);
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
        [Route("api/ProcessMaster/ProcessMaster_InsertUpdate")]
        [HttpPost]
        public ProcessMasterModel Process_InsertUpdate(ArrayList paramList)
        {
            string strResult = "";
            string strReturnCode = "000";
            string strReturnMsg = "UnDefined";
            ProcessMasterModel apiObject = new ProcessMasterModel();
            apiObject = Newtonsoft.Json.JsonConvert.DeserializeObject<ProcessMasterModel>(paramList[0].ToString());
            string crCnString = UtilityLib.GetConnectionString();
            int Processinfo = DL_ProcessMaster.ProcessInsert(crCnString, apiObject.ProcessId, apiObject.ProcessName, apiObject.PackingFlag, apiObject.PackingFlag, apiObject.ProcessDuration, apiObject.ProcessVolume, apiObject.WastageFlag, apiObject.ProcessUnit, apiObject.CompanyId, apiObject.ByProduct);
            if (Processinfo == 0)
            {
                apiObject.ReturnCode = Processinfo;
                apiObject.ReturnMessage = "Process Added Successfully";
            }
            else if (Processinfo == 1)
            {
                apiObject.ReturnCode = Processinfo;
                apiObject.ReturnMessage = "Process already exists";
            }
            else if (Processinfo == 101)
            {
                apiObject.ReturnCode = Processinfo;
                apiObject.ReturnMessage = "Process updated successfully";
            }
            else if (Processinfo == 2)
            {
                apiObject.ReturnCode = Processinfo;
                apiObject.ReturnMessage = "record is already updated by someone else";
            }
            else
            {
                apiObject.ReturnCode = Processinfo;
                apiObject.ReturnMessage = "Fail-Record Not Inserted";
            }
            strResult = strReturnCode + "|" + strReturnMsg;
            return apiObject;

        }
        #endregion
        [Route("api/ProcessMaster/ProcessMaster_Delete")]
        [HttpPost]
        public ProcessMasterModel DeleteProcess(ArrayList paramList)
        {
            string strResult = "";
            string strReturnCode = "000";
            string strReturnMsg = "UnDefined";
            ProcessMasterModel apiObject = new ProcessMasterModel();
            apiObject = Newtonsoft.Json.JsonConvert.DeserializeObject<ProcessMasterModel>(paramList[0].ToString());
            string crCnString = UtilityLib.GetConnectionString();
            int Processinfo = DL_ProcessMaster.Delete_Process(crCnString, apiObject.ProcessId);
            if (Processinfo == 101)
            {
                apiObject.ReturnCode = Processinfo;
                apiObject.ReturnMessage = "Process delete successfully";
            }
            else
            {
                apiObject.ReturnCode = Processinfo;
                apiObject.ReturnMessage = "Fail-Record Not Delete";
            }
            strResult = strReturnCode + "|" + strReturnMsg;
            return apiObject;
        }
    }
}

