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
    public class ProductMasterController : ApiController
    {
        private static ILog m_Logger = LogManager.GetLogger(typeof(RoelsController));
        #region ProductMaster Insert Update Select

        [Route("api/ProductMaster/ProductMaster_List")]
        [HttpPost]
        public IList<ProductMasterModel> Product_List(ArrayList paramList)
        {
            ProductMasterModel apiObject = new ProductMasterModel();
            string strResult = "";
            IList<ProductMasterModel> apiObjectList = null;
            apiObject = Newtonsoft.Json.JsonConvert.DeserializeObject<ProductMasterModel>(paramList[0].ToString());
            apiObjectList = fnProductMaster_List(apiObject, ref strResult);
            return apiObjectList;
        }

        private IList<ProductMasterModel> fnProductMaster_List(ProductMasterModel apiObject, ref string strResult)
        {
            string strReturnCode = "000";
            string strReturnMsg = "UnDefined";
            string crCnString = UtilityLib.GetConnectionString();
            IList<ProductMasterModel> objFieldClassModelList = new List<ProductMasterModel>();
            DataSet usersInfoDS = DL_ProductMaster.Product_List(crCnString, apiObject.RequestType, apiObject.SearchBy, apiObject.SearchString,
                    apiObject.Productid, apiObject.CompanyId, apiObject.ItemsPerPage, apiObject.RequestPageNo, apiObject.CurrentPageNo);
            DataTable usersInfoDT = usersInfoDS.Tables[0];
            if (usersInfoDT != null && usersInfoDT.Rows.Count > 0)
            {
                strReturnCode = "001";
                strReturnMsg = "Success";
                foreach (DataRow dr in usersInfoDT.Rows)
                {
                    apiObject = new ProductMasterModel();
                    apiObject.Productid = UtilityLib.FormatNumber(dr["Processid"].ToString());
                    apiObject.ProductName = (string)dr["ProductName"];
                    apiObject.UOM = UtilityLib.FormatNumber(dr["UOM"].ToString());
                    //apiObject.State = UtilityLib.FormatString(dr["State"]);
                    //apiObject.Texture = UtilityLib.FormatNumber(dr["Texture "].ToString());
                    //apiObject.Catagory = UtilityLib.FormatNumber(dr["Catagory "].ToString());
                    apiObject.CreatedBy = (Guid)(dr["CreatedBy"]);
                    apiObject.CreatedDate = UtilityLib.FormatDate(dr["CreatedDate"]);
                    apiObject.IsDelete = UtilityLib.FormatBoolean(dr["IsDelete"].ToString());
                    apiObject.SafeLifeInGodown = UtilityLib.FormatDate(dr["SafeLifeInGodown"]);
                    apiObject.ProcessId =(string)(dr["ProcessId"]);
                    apiObject.BuyProductId = UtilityLib.FormatNumber(dr["BuyProductId"].ToString());
                    apiObject.BuyProductPacking = UtilityLib.FormatNumber(dr["BuyProductPacking"].ToString());
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

        [Route("api/ProductMaster/ProductMaster_InsertUpdate")]
        [HttpPost]
        public ProductMasterModel ProductMaster_InsertUpdate(ArrayList paramList)
        {
            string strResult = "";
            string strReturnCode = "000";
            string strReturnMsg = "UnDefined";
            ProductMasterModel apiObject = new ProductMasterModel();
            apiObject = Newtonsoft.Json.JsonConvert.DeserializeObject<ProductMasterModel>(paramList[0].ToString());
            string crCnString = UtilityLib.GetConnectionString();
            int Processinfo = DL_ProductMaster.ProductInsert(crCnString, apiObject.Productid,
                apiObject.ProductName, apiObject.UOM,apiObject.Color,apiObject.Texture,apiObject.CreatedBy,apiObject.ProcessId,apiObject.BuyProductId,apiObject.BuyProductPacking,apiObject.Catagory, apiObject.CompanyId);
            if (Processinfo == 0)
            {
                apiObject.ReturnCode = Processinfo;
                apiObject.ReturnMessage = "Product Added Successfully";
            }
            else if (Processinfo == 1)
            {
                apiObject.ReturnCode = Processinfo;
                apiObject.ReturnMessage = "Product already exists";
            }
            else if (Processinfo == 101)
            {
                apiObject.ReturnCode = Processinfo;
                apiObject.ReturnMessage = "Product updated successfully";
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


    }
}
