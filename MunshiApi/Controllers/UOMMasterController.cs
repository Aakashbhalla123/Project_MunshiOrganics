using log4net;
using MunshiDAL;
using MunshiModels.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MunshiAPI.Controllers
{
    public class UOMMasterController : ApiController
    {
        private static ILog m_Logger = LogManager.GetLogger(typeof(UOMMasterController));

        #region UOM Master Insert Update Select
        [Route("api/UOMMaster/UOMMaster_InsertUpdate")]
        [HttpPost]
        public UOMMasterModel UOMMaster_InsertUpdate(ArrayList paramList)
        {
            string strResult = "";
            string strReturnCode = "000";
            string strReturnMsg = "UnDefined";
            UOMMasterModel apiObject = new UOMMasterModel();

            apiObject = Newtonsoft.Json.JsonConvert.DeserializeObject<UOMMasterModel>(paramList[0].ToString());
            string crCnString = UtilityLib.GetConnectionString();

            int FieldCodeInfo = DAL_UOMMaster.UOMMaster_InsertUpdate(crCnString, apiObject.UOMId, apiObject.UOMTypeId, apiObject.UOMName,
               apiObject.UOMCode, apiObject.DecimalPoints, apiObject.BaseUnit, apiObject.ConversionRation, apiObject.CreatedBy, apiObject.ModifiedBy, apiObject.CompanyId);

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

        #region UOM Master List
        [Route("api/UOMMaster/UOMMaster_List")]
        [HttpPost]
        public IList<UOMMasterModel> UOMMaster_List(ArrayList paramList)
        {
            UOMMasterModel apiObject = new UOMMasterModel();
            string strResult = "";
            IList<UOMMasterModel> apiObjectList = null;
            apiObject = Newtonsoft.Json.JsonConvert.DeserializeObject<UOMMasterModel>(paramList[0].ToString());
            /// =============
            apiObjectList = fnUOMMaster_List(apiObject, ref strResult);
            return apiObjectList;
        }

        private IList<UOMMasterModel> fnUOMMaster_List(UOMMasterModel apiObject, ref string strResult)
        {
            string strReturnCode = "000";
            string strReturnMsg = "UnDefined";
            string crCnString = UtilityLib.GetConnectionString();
            IList<UOMMasterModel> objUOMMasterModelList = new List<UOMMasterModel>();

            DataSet usersInfoDS = DAL_UOMMaster.UOMMaster_List(crCnString, apiObject.RequestType, apiObject.SearchBy, apiObject.SearchString,
                apiObject.IntID, apiObject.CompanyId, apiObject.ItemsPerPage, apiObject.RequestPageNo, apiObject.CurrentPageNo);

            DataTable usersInfoDT = usersInfoDS.Tables[0];
            if (usersInfoDT != null && usersInfoDT.Rows.Count > 0)
            {
                strReturnCode = "001";
                strReturnMsg = "Success";
                foreach (DataRow dr in usersInfoDT.Rows)
                {
                    apiObject = new UOMMasterModel();
                    apiObject.UOMId = UtilityLib.FormatNumber(dr["UOMId"].ToString());
                    apiObject.UOMTypeId = UtilityLib.FormatNumber(dr["UOMTypeId"].ToString());
                    apiObject.DecimalPoints = UtilityLib.FormatNumber(dr["DecimalPoints"].ToString());
                    apiObject.BaseUnit = (bool)dr["BaseUnit"];
                    apiObject.ConversionRation = UtilityLib.FormatDecimal(dr["ConversionRation"].ToString());
                    apiObject.UOMName = (string)dr["UOMName"];
                    apiObject.UOMCode = (string)dr["UOMCode"];
                    apiObject.CompanyId = (Guid)(dr["CompanyId"]);
                    objUOMMasterModelList.Add(apiObject);
                }
            }
            else
            {
                strReturnCode = "002";
                apiObject.ReturnMessage = "Fail-Record Not Found";
            }
            strResult = strReturnCode + "|" + strReturnMsg;
            return objUOMMasterModelList;
        }
        #endregion

        #region UOM Conversion
        [Route("api/UOMMaster/UOMConversion_Get")]
        [HttpPost]
        public UOMConversionModel UOMConversion_Get(ArrayList paramList)
        {
            UOMConversionModel objUOMConversion = new UOMConversionModel();
            string strResult = "";
            UOMConversionModel apiObject = null;
            apiObject = Newtonsoft.Json.JsonConvert.DeserializeObject<UOMConversionModel>(paramList[0].ToString());
            /// =============
            objUOMConversion = fnUOMConversion_Get(apiObject, ref strResult);
            return objUOMConversion;
        }

        private UOMConversionModel fnUOMConversion_Get(UOMConversionModel apiObject, ref string strResult)
        {
            string strReturnCode = "000";
            string strReturnMsg = "UnDefined";
            string crCnString = UtilityLib.GetConnectionString();
            UOMConversionModel objUOMConversion = new UOMConversionModel();

            DataSet usersInfoDS = DAL_UOMMaster.UOMConversion_Get(crCnString, apiObject.SourceUOMId, apiObject.DestinationUOMId,
                apiObject.CompanyId);

            DataTable usersInfoDT = usersInfoDS.Tables[0];
            if (usersInfoDT != null && usersInfoDT.Rows.Count > 0)
            {
                strReturnCode = "001";
                strReturnMsg = "Success";
                DataRow dr = usersInfoDT.Rows[0];
                objUOMConversion.SourceUOMId = UtilityLib.FormatNumber(dr["SourceUOMId"].ToString());
                objUOMConversion.SourceConversionRation = UtilityLib.FormatDecimal(dr["SourceConversionRation"].ToString());
                objUOMConversion.DestinationUOMId = UtilityLib.FormatNumber(dr["DestinationUOMId"].ToString());
                objUOMConversion.DestinationConversionRation = UtilityLib.FormatDecimal(dr["DestinationConversionRation"].ToString());

            }
            else
            {
                strReturnCode = "002";
                apiObject.ReturnMessage = "Fail-Record Not Found";
            }
            strResult = strReturnCode + "|" + strReturnMsg;
            return objUOMConversion;
        }
        #endregion
    }
}
