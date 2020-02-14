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
    public class GeneralMasterController : ApiController
    {
        private static ILog m_Logger = LogManager.GetLogger(typeof(GeneralMasterController));

        #region Field Class Insert Update Select

        [Route("api/FieldClass/FieldClass_List")]
        [HttpPost]
        public IList<FieldClassModel> FieldClass_List(ArrayList paramList)
        {
            FieldClassModel apiObject = new FieldClassModel();
            string strResult = "";
            IList<FieldClassModel> apiObjectList = null;
            apiObject = Newtonsoft.Json.JsonConvert.DeserializeObject<FieldClassModel>(paramList[0].ToString());
            /// =============
            apiObjectList = fnFieldClass_List(apiObject, ref strResult);
            return apiObjectList;
        }

        private IList<FieldClassModel> fnFieldClass_List(FieldClassModel apiObject, ref string strResult)
        {
            string strReturnCode = "000";
            string strReturnMsg = "UnDefined";
            string crCnString = UtilityLib.GetConnectionString();
            IList<FieldClassModel> objFieldClassModelList = new List<FieldClassModel>();

            DataSet usersInfoDS = DAL_FieldClass.FieldClass_List(crCnString, apiObject.RequestType, apiObject.SearchBy, apiObject.SearchString,
                apiObject.IntID, apiObject.CompanyId, apiObject.ItemsPerPage, apiObject.RequestPageNo, apiObject.CurrentPageNo);

            DataTable usersInfoDT = usersInfoDS.Tables[0];
            if (usersInfoDT != null && usersInfoDT.Rows.Count > 0)
            {
                strReturnCode = "001";
                strReturnMsg = "Success";
                foreach (DataRow dr in usersInfoDT.Rows)
                {
                    apiObject = new FieldClassModel();
                    apiObject.FieldClassId = UtilityLib.FormatNumber(dr["FieldClassId"].ToString());
                    apiObject.FieldClassName = (string)dr["FieldClassName"];
                    apiObject.FieldClassCode = (string)dr["FieldClassCode"];
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

        [Route("api/FieldClass/FieldClass_InsertUpdate")]
        [HttpPost]
        public FieldClassModel FieldClass_InsertUpdate(ArrayList paramList)
        {
            string strResult = "";
            string strReturnCode = "000";
            string strReturnMsg = "UnDefined";
            FieldClassModel apiObject = new FieldClassModel();

            apiObject = Newtonsoft.Json.JsonConvert.DeserializeObject<FieldClassModel>(paramList[0].ToString());
            string crCnString = UtilityLib.GetConnectionString();

            int FieldInfo = DAL_FieldClass.FieldClass_InsertUpdate(crCnString, apiObject.FieldClassId, apiObject.FieldClassName, apiObject.FieldClassCode, apiObject.CompanyId);

            if (FieldInfo == 0)
            {
                apiObject.ReturnCode = FieldInfo;
                apiObject.ReturnMessage = "Success";
            }
            else if (FieldInfo == 1)
            {
                apiObject.ReturnCode = FieldInfo;
                apiObject.ReturnMessage = "Field Class already exists";
            }
            else if (FieldInfo == 101)
            {
                apiObject.ReturnCode = FieldInfo;
                apiObject.ReturnMessage = "Field Class updated successfully";
            }
            else if (FieldInfo == 2)
            {
                apiObject.ReturnCode = FieldInfo;
                apiObject.ReturnMessage = "record is already updated by someone else";
            }
            else
            {
                apiObject.ReturnCode = FieldInfo;
                apiObject.ReturnMessage = "Fail-Record Not Inserted";
            }
            strResult = strReturnCode + "|" + strReturnMsg;
            return apiObject;
        }
        #endregion

        #region Field Code Insert Update Select
        [Route("api/FieldCode/FieldCode_List")]
        [HttpPost]
        public IList<FieldCodeModel> FieldCode_List(ArrayList paramList)
        {
            FieldCodeModel apiObject = new FieldCodeModel();
            string strResult = "";
            IList<FieldCodeModel> apiObjectList = null;
            apiObject = Newtonsoft.Json.JsonConvert.DeserializeObject<FieldCodeModel>(paramList[0].ToString());
            /// =============
            apiObjectList = fnFieldCode_List(apiObject, ref strResult);
            return apiObjectList;
        }

        private IList<FieldCodeModel> fnFieldCode_List(FieldCodeModel apiObject, ref string strResult)
        {
            string strReturnCode = "000";
            string strReturnMsg = "UnDefined";
            string crCnString = UtilityLib.GetConnectionString();
            IList<FieldCodeModel> objFieldCodeModelList = new List<FieldCodeModel>();

            DataSet usersInfoDS = DAL_FieldCode.FieldCode_List(crCnString, apiObject.RequestType, apiObject.SearchBy, apiObject.SearchString,
                apiObject.IntID, apiObject.FieldClassId, apiObject.CompanyId, apiObject.ItemsPerPage, apiObject.RequestPageNo, apiObject.CurrentPageNo);

            DataTable usersInfoDT = usersInfoDS.Tables[0];
            if (usersInfoDT != null && usersInfoDT.Rows.Count > 0)
            {
                strReturnCode = "001";
                strReturnMsg = "Success";
                foreach (DataRow dr in usersInfoDT.Rows)
                {
                    apiObject = new FieldCodeModel();
                    apiObject.FieldCodeId = UtilityLib.FormatNumber(dr["FieldCodeId"].ToString());
                    apiObject.FieldClassId = UtilityLib.FormatNumber(dr["FieldClassId"].ToString());
                    apiObject.FieldClassName = (string)dr["FieldClassName"];
                    apiObject.FieldCodeName = (string)dr["FieldCodeName"];
                    apiObject.FieldCodeAlias = (string)dr["FieldCodeAlias"];
                    apiObject.FieldCodeOrder = (string)dr["FieldCodeOrder"];
                    apiObject.CompanyId = (Guid)(dr["CompanyId"]);
                    objFieldCodeModelList.Add(apiObject);
                }
            }
            else
            {
                strReturnCode = "002";
                strReturnMsg = "Fail-Record Not Found";
            }
            strResult = strReturnCode + "|" + strReturnMsg;
            return objFieldCodeModelList;
        }

        [Route("api/FieldCode/FieldCode_InsertUpdate")]
        [HttpPost]
        public FieldCodeModel FieldCode_InsertUpdate(ArrayList paramList)
        {
            string strResult = "";
            string strReturnCode = "000";
            string strReturnMsg = "UnDefined";
            FieldCodeModel apiObject = new FieldCodeModel();

            apiObject = Newtonsoft.Json.JsonConvert.DeserializeObject<FieldCodeModel>(paramList[0].ToString());
            string crCnString = UtilityLib.GetConnectionString();

            int FieldCodeInfo = DAL_FieldCode.FieldCode_InsertUpdate(crCnString, apiObject.FieldCodeId, apiObject.FieldClassId, apiObject.FieldCodeName,
               apiObject.FieldCodeAlias, apiObject.FieldCodeOrder, apiObject.CompanyId);

            if (FieldCodeInfo == 0)
            {
                apiObject.ReturnCode = FieldCodeInfo;
                apiObject.ReturnMessage = "Success";
            }
            else if (FieldCodeInfo == 1)
            {
                apiObject.ReturnCode = FieldCodeInfo;
                apiObject.ReturnMessage = "Field Code already exists";
            }
            else if (FieldCodeInfo == 101)
            {
                apiObject.ReturnCode = FieldCodeInfo;
                apiObject.ReturnMessage = "Field Code updated successfully";
            }
            else if (FieldCodeInfo == 2)
            {
                apiObject.ReturnCode = FieldCodeInfo;
                apiObject.ReturnMessage = "record is already updated by someone else";
            }
            else
            {
                apiObject.ReturnCode = FieldCodeInfo;
                apiObject.ReturnMessage = "Fail-Record Not Inserted";
            }
            strResult = strReturnCode + "|" + strReturnMsg;
            return apiObject;
        }
        #endregion

        #region General Master Insert Update Select
        [Route("api/GeneralMaster/GeneralMaster_List")]
        [HttpPost]
        public IList<GeneralMasterModel> GeneralMaster_List(ArrayList paramList)
        {
            GeneralMasterModel apiObject = new GeneralMasterModel();
            string strResult = "";
            IList<GeneralMasterModel> apiObjectList = null;
            apiObject = Newtonsoft.Json.JsonConvert.DeserializeObject<GeneralMasterModel>(paramList[0].ToString());
            /// =============
            apiObjectList = fnGeneralMaster_List(apiObject, ref strResult);
            return apiObjectList;
        }

        private IList<GeneralMasterModel> fnGeneralMaster_List(GeneralMasterModel apiObject, ref string strResult)
        {
            string strReturnCode = "000";
            string strReturnMsg = "UnDefined";
            string crCnString = UtilityLib.GetConnectionString();
            IList<GeneralMasterModel> objFieldCodeModelList = new List<GeneralMasterModel>();

            DataSet usersInfoDS = DAL_GeneralMaster.GeneralMaster_List(crCnString, apiObject.RequestType, apiObject.SearchBy, apiObject.SearchString,
                apiObject.IntID, apiObject.GeneralMasterClassId, apiObject.CompanyId, apiObject.ItemsPerPage, apiObject.RequestPageNo, apiObject.CurrentPageNo);
            if (usersInfoDS.Tables.Count != 0)
            {

                DataTable usersInfoDT = usersInfoDS.Tables[0];
                if (usersInfoDT != null && usersInfoDT.Rows.Count > 0)
                {
                    strReturnCode = "001";
                    strReturnMsg = "Success";
                    foreach (DataRow dr in usersInfoDT.Rows)
                    {
                        apiObject = new GeneralMasterModel();
                        apiObject.GeneralMasterId = UtilityLib.FormatNumber(dr["GeneralMasterId"].ToString());
                        apiObject.GeneralMasterName = (string)dr["GeneralMasterName"];
                        apiObject.GeneralMasterClassId = UtilityLib.FormatNumber(dr["GeneralMasterClassId"].ToString());
                        apiObject.GeneralMasterClassName = (string)dr["GeneralMasterClassName"];
                        apiObject.GeneralMasterCode = (string)dr["GeneralMasterCode"];
                        apiObject.GeneralMasterSquence = (string)dr["GeneralMasterSequence"];
                        apiObject.CreatedBy = (Guid)dr["CreatedBy"];
                        apiObject.ModifiedBy = (Guid)dr["ModifiedBy"];
                        apiObject.CompanyId = (Guid)(dr["CompanyId"]);
                        objFieldCodeModelList.Add(apiObject);
                    }
                }
                else
                {
                    strReturnCode = "002";
                    strReturnMsg = "Fail-Record Not Found";
                }
            }
            strResult = strReturnCode + "|" + strReturnMsg;
            return objFieldCodeModelList;
        }

        [Route("api/GeneralMaster/GeneralMaster_InsertUpdate")]
        [HttpPost]
        public GeneralMasterModel GeneralMaster_InsertUpdate(ArrayList paramList)
        {
            string strResult = "";
            string strReturnCode = "000";
            string strReturnMsg = "UnDefined";
            GeneralMasterModel apiObject = new GeneralMasterModel();

            apiObject = Newtonsoft.Json.JsonConvert.DeserializeObject<GeneralMasterModel>(paramList[0].ToString());
            string crCnString = UtilityLib.GetConnectionString();

            int FieldCodeInfo = DAL_GeneralMaster.GeneralMaster_InsertUpdate(crCnString, apiObject.GeneralMasterId,
                apiObject.GeneralMasterClassId, apiObject.GeneralMasterName,
               apiObject.GeneralMasterCode, apiObject.GeneralMasterSquence, apiObject.CreatedBy, apiObject.ModifiedBy, apiObject.CompanyId);

            if (FieldCodeInfo == 0)
            {
                apiObject.ReturnCode = FieldCodeInfo;
                apiObject.ReturnMessage = "Success";
            }
            else if (FieldCodeInfo == 1)
            {
                apiObject.ReturnCode = FieldCodeInfo;
                apiObject.ReturnMessage = "General Master already exists";
            }
            else if (FieldCodeInfo == 101)
            {
                apiObject.ReturnCode = FieldCodeInfo;
                apiObject.ReturnMessage = "General Master updated successfully";
            }
            else if (FieldCodeInfo == 2)
            {
                apiObject.ReturnCode = FieldCodeInfo;
                apiObject.ReturnMessage = "record is already updated by someone else";
            }
            else
            {
                apiObject.ReturnCode = FieldCodeInfo;
                apiObject.ReturnMessage = "Fail-Record Not Inserted";
            }
            strResult = strReturnCode + "|" + strReturnMsg;
            return apiObject;
        }
        #endregion

        #region General Master Class List
        [Route("api/GeneralMasterClass/GeneralMasterClass_List")]
        [HttpPost]
        public IList<GeneralMasterClass> GeneralMasterClass_List(ArrayList paramList)
        {
            GeneralMasterClass apiObject = new GeneralMasterClass();
            string strResult = "";
            IList<GeneralMasterClass> apiObjectList = null;
            apiObject = Newtonsoft.Json.JsonConvert.DeserializeObject<GeneralMasterClass>(paramList[0].ToString());
            /// =============
            apiObjectList = fnGeneralMasterClass_List(apiObject, ref strResult);
            return apiObjectList;
        }

        private IList<GeneralMasterClass> fnGeneralMasterClass_List(GeneralMasterClass apiObject, ref string strResult)
        {
            string strReturnCode = "000";
            string strReturnMsg = "UnDefined";
            string crCnString = UtilityLib.GetConnectionString();
            IList<GeneralMasterClass> objGeneralClassModelList = new List<GeneralMasterClass>();

            DataSet usersInfoDS = DAL_GeneralMasterClass.GeneralMasterClass_List(crCnString, apiObject.RequestType, apiObject.SearchBy, apiObject.SearchString,
                apiObject.IntID, apiObject.CompanyId, apiObject.ItemsPerPage, apiObject.RequestPageNo, apiObject.CurrentPageNo);

            DataTable usersInfoDT = usersInfoDS.Tables[0];
            if (usersInfoDT != null && usersInfoDT.Rows.Count > 0)
            {
                strReturnCode = "001";
                strReturnMsg = "Success";
                foreach (DataRow dr in usersInfoDT.Rows)
                {
                    apiObject = new GeneralMasterClass();
                    apiObject.GeneralMasterClassId = UtilityLib.FormatNumber(dr["GeneralMasterClassId"].ToString());
                    apiObject.GeneralMasterClassName = (string)dr["GeneralMasterClassName"];
                    apiObject.CompanyId = (Guid)(dr["CompanyId"]);
                    objGeneralClassModelList.Add(apiObject);
                }
            }
            else
            {
                strReturnCode = "002";
                apiObject.ReturnMessage = "Fail-Record Not Found";
            }
            strResult = strReturnCode + "|" + strReturnMsg;
            return objGeneralClassModelList;
        }
        #endregion
    }
}
