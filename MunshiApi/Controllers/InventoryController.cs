using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MunshiModels.Models;
using System.Collections;
using MunshiDAL;
using System.Data;

namespace MunshiAPI.Controllers
{
    public class InventoryController : ApiController
    {
        private static ILog m_Logger = LogManager.GetLogger(typeof(RoelsController));
        #region ProcessMaster Insert Update Select

        [Route("api/InventoryMaster/InventoryMaster_List")]
        [HttpPost]
        public IList<InventoryModel> InventoryMaster_List(ArrayList paramList)
        {
            InventoryModel apiObject = new InventoryModel();
            string strResult = "";
            IList<InventoryModel> apiObjectList = null;
            apiObject = Newtonsoft.Json.JsonConvert.DeserializeObject<InventoryModel>(paramList[0].ToString());
            apiObjectList = fnInventory_List(apiObject, ref strResult);
            return apiObjectList;
        }
        private IList<InventoryModel> fnInventory_List(InventoryModel apiObject, ref string strResult)
        {
            string strReturnCode = "000";
            string strReturnMsg = "UnDefined";
            string crCnString = UtilityLib.GetConnectionString();
            IList<InventoryModel> objFieldClassModelList = new List<InventoryModel>();
            DataSet usersInfoDS = DAL_Inventory.Inventory_List(crCnString, apiObject.RequestType, apiObject.SearchBy, apiObject.SearchString,
                    apiObject.ReciptNo, apiObject.ComapnyId, apiObject.ItemsPerPage, apiObject.RequestPageNo, apiObject.CurrentPageNo);
            DataTable usersInfoDT = usersInfoDS.Tables[0];
            if (usersInfoDT != null && usersInfoDT.Rows.Count > 0)
            {
                strReturnCode = "001";
                strReturnMsg = "Success";
                foreach (DataRow dr in usersInfoDT.Rows)
                {
                    apiObject = new InventoryModel();
                    apiObject.ReciptNo = UtilityLib.FormatNumber(dr["ReciptNo"].ToString());
                    apiObject.FarmerId = UtilityLib.FormatNumber(dr["FarmerId"].ToString());
                    apiObject.LoginId = UtilityLib.FormatNumber(dr["LoginId"].ToString());
                  
                    apiObject.PersonName = (string)dr["PersonName"];
                    apiObject.Quantity =UtilityLib.FormatNumber(dr["Qunatity"].ToString());
                    apiObject.Unit = (string)dr["Unit"];
                   
                    apiObject.RawMaterial = (string)dr["RawMaterial"];
                    apiObject.Storage = UtilityLib.FormatNumber(dr["Storage"].ToString());
                    apiObject.CreatedDate = UtilityLib.FormatDate(dr["CreatedDate"]);
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
        [Route("api/InventoryMaster/Inventory_InsertUpdate")]
        [HttpPost]
        public InventoryModel Inventory_InsertUpdate(ArrayList paramList)
        {
            string strResult = "";
            string strReturnCode = "000";
            string strReturnMsg = "UnDefined";
            InventoryModel apiObject = new InventoryModel();
            apiObject = Newtonsoft.Json.JsonConvert.DeserializeObject<InventoryModel>(paramList[0].ToString());
            string crCnString = UtilityLib.GetConnectionString();
            int Inventoryinfo = DAL_Inventory.InventoryInsert(crCnString,apiObject.FarmerId,
                apiObject.ReciptNo,apiObject.LoginId, apiObject.PersonName,
                apiObject.Quantity,apiObject.Unit,apiObject.ComapnyId,
                apiObject.RawMaterial,apiObject.Storage,apiObject.CreatedDate);

            if (Inventoryinfo == 0)
            {
                apiObject.ReturnCode = Inventoryinfo;
                apiObject.ReturnMessage = "Inventory Added Successfully";
            }
            else if (Inventoryinfo == 1)
            {
                apiObject.ReturnCode = Inventoryinfo;
                apiObject.ReturnMessage = "Inventory already exists";
            }
            else if (Inventoryinfo == 101)
            {
                apiObject.ReturnCode = Inventoryinfo;
                apiObject.ReturnMessage = "Inventory updated successfully";
            }
            else if (Inventoryinfo == 2)
            {
                apiObject.ReturnCode = Inventoryinfo;
                apiObject.ReturnMessage = "record is already updated by someone else";
            }
            else
            {
                apiObject.ReturnCode = Inventoryinfo;
                apiObject.ReturnMessage = "Fail-Record Not Inserted";
            }
            strResult = strReturnCode + "|" + strReturnMsg;
            return apiObject;

        }
        #endregion
        #region ListGet
        [Route("api/InventoryMaster/GetList")]
        [HttpPost]
        public InventoryModel GetREciptNo()
        {
            string strResult = "";
            string strReturnCode = "000";
            string strReturnMsg = "UnDefined";
            InventoryModel apiObject = new InventoryModel();
            string crCnString = UtilityLib.GetConnectionString();
            
            DataSet dt  = DAL_Inventory.GetRecipt(crCnString);
            DataTable usersInfoDT = dt.Tables[0];
            if (usersInfoDT != null && usersInfoDT.Rows.Count > 0)
            {
                strReturnCode = "001";
                strReturnMsg = "Success";
                foreach (DataRow dr in usersInfoDT.Rows)
                {
                    apiObject = new InventoryModel();
                    apiObject.NewReciptNo = UtilityLib.FormatNumber(dr["UpRwcipt"].ToString());
                }
            }
            else
            {
                strReturnCode = "002";
                strReturnMsg = "Fail-Record Not Found";
            }
            strResult = strReturnCode + "|" + strReturnMsg;
            return apiObject;


            ;


        }
        #endregion
    }



}
