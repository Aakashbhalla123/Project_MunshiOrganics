using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Data;
using System.Web;
using MunshiModels.Models;
using Newtonsoft.Json.Converters;
using System.Net.Http;
using System.Web.Mvc;
using System.Web.Security;
using MunshiDAL;


namespace AOneNutsWeb
{
    public static class CommonCalls
    {
        private static readonly Uri baseAddres;
        #region all enums
        public enum locations
        {
            Country = 1,
            state = 2,
            District = 3,
            City = 4,
            Tasil = 5
        }
        public enum GMClass
        {
            ProductUOM = 1,
            ProductCategory = 2
        }
        public enum SelectionType
        {
            List = 1,
            Load = 2
        }

        public class constant
        {
            public static readonly Guid FactoryIsSystemGenerated = new Guid("e60e1e79-9fd3-4a94-a910-e7b48b6d8e25");
        }
        #endregion
        
        #region Roles
        public static IList<RolesModel> GetRolesList(int? intId, int selectionType, string SearchBy, string SearchString, Guid? CompanyId, int ItemsPerPage, int RequestPage, int CurrentPageNo)
        {
            ArrayList paramList = new ArrayList();
            string resultString;
            IList<RolesModel> objRolesModel = null;
            RolesModel obj = new RolesModel();
            obj.SearchString = SearchString;
            obj.RequestType = selectionType;
            obj.IntID = intId;
            obj.CompanyId = CompanyId;
            obj.ItemsPerPage = ItemsPerPage;
            obj.RequestPageNo = RequestPage;
            obj.CurrentPageNo = CurrentPageNo;
            paramList.Add(obj);
            using (var client = new HttpClient())
            {
                Uri baseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["UriPath"]);
                client.BaseAddress = baseAddress;
                resultString = client.PostAsJsonAsync("api/RoleRight/Rights_List", paramList)
                                         .Result
                                         .Content.ReadAsStringAsync().Result;
            }

            objRolesModel = (IList<RolesModel>)Newtonsoft.Json.JsonConvert.DeserializeObject(resultString, typeof(IList<RolesModel>));
            return objRolesModel;
        }

        //internal static IList<InventoryModel> GetInventoryList(int? farmerId, object selectionType, object searchType, string searchBy, string searchString, Guid? companyId, int itemsPerPage, int requestPage, int currentPageNo)
        //{
        //    throw new NotImplementedException();
        //}

        //internal static IList<ProcessMasterModel> GetProcessMasterList(int v1, int v2, string searchBy, string searchString, Guid? companyId, int itemsPerPage, int requestPage, int currentPageNo)
        //{
        //    throw new NotImplementedException();
        //}

        public static RolesModel Roles_InsertUpdate(RolesModel objrole)
        {
            ArrayList paramList = new ArrayList();
            string resultString;
            RolesModel objRoleModel = null;
            RolesModel obj = new RolesModel();
            obj.RoleId = objrole.RoleId;
            obj.RoleName = objrole.RoleName;
            obj.RoleCode = objrole.RoleCode;
            obj.AuthorizationRequired = objrole.AuthorizationRequired;
            obj.UnderPrecedingRoleId = objrole.UnderPrecedingRoleId;
            obj.CompanyId = objrole.CompanyId;
            paramList.Add(obj);
            
            using (var client = new HttpClient())
            {
                Uri baseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["UriPath"]);
                client.BaseAddress = baseAddress;
                resultString = client.PostAsJsonAsync("api/Roles/Roles_InsertUpdate", paramList)
                                         .Result
                                         .Content.ReadAsStringAsync().Result;
            }

            objRoleModel = (RolesModel)Newtonsoft.Json.JsonConvert.DeserializeObject(resultString, typeof(RolesModel));
            return objRoleModel;
        }
        #endregion

        #region Process_Module
        public static IList<ProcessMasterModel>GetProcessMasterList(int?ProcessId,  int selectionType, string SearchBy, string SearchString,Guid ?CompanyId, int ItemsPerPage, int RequestPage, int CurrentPageNo)
        {
            ArrayList paramlist = new ArrayList();
            string resultString;
            int ProsId = ProcessId==null?0:(int)ProcessId;
            //IList<ProcessMasterModel> objprocessMasterModel = null;
            ProcessMasterModel obj = new ProcessMasterModel();
            obj.SearchString = SearchString;
            obj.RequestType = selectionType;
            obj.ProcessId = ProsId;
            obj.CompanyId = CompanyId;
            obj.ItemsPerPage = ItemsPerPage;
            obj.RequestPageNo = RequestPage;
            obj.CurrentPageNo = CurrentPageNo;
            paramlist.Add(obj);
            using (var client = new HttpClient())
            {
                Uri baseAddres = new Uri(System.Configuration.ConfigurationManager.AppSettings["UriPath"]);
              
                client.BaseAddress = baseAddres;
                resultString = client.PostAsJsonAsync("api/ProcessMaster/ProcessMaster_List", paramlist)
                                         .Result
                                         .Content.ReadAsStringAsync().Result;

            }
            IList<ProcessMasterModel> objprocessMasterModel = (IList<ProcessMasterModel>)Newtonsoft.Json.JsonConvert.DeserializeObject(resultString, typeof(IList<ProcessMasterModel>));
            return objprocessMasterModel;

        }
          public static ProcessMasterModel ProcessMasterModule_InsertUpdate(ProcessMasterModel objprocess)
        {
            ArrayList paramList = new ArrayList();
            string resultString;
           ProcessMasterModel objReturnProcessModel = null;
            ProcessMasterModel obj = new ProcessMasterModel();
            obj.ProcessId = objprocess.ProcessId;
            obj.ProcessName = objprocess.ProcessName;
            obj.QuantityFlag = objprocess.QuantityFlag;
            obj.ProcessVolume = objprocess.ProcessVolume;
            //obj.ProcessDuration = objprocess.ProcessDuration;
            obj.PackingFlag = objprocess.PackingFlag;
            obj.WastageFlag = objprocess.WastageFlag;
            obj.ByProduct = objprocess.ByProduct;
            obj.CreatedDate = objprocess.CreatedDate;
            obj.IsDelete = objprocess.IsDelete;
            obj.ProcessDuration = objprocess.ProcessDuration;
            obj.ProcessVolume = objprocess.ProcessVolume;
          obj.CompanyId = objprocess.CompanyId;
            paramList.Add(obj);
            using (var client = new HttpClient())
            {
                Uri baseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["UriPath"]);
                client.BaseAddress = baseAddress;
                resultString = client.PostAsJsonAsync("api/ProcessMaster/ProcessMaster_InsertUpdate", paramList)
                                         .Result
                                         .Content.ReadAsStringAsync().Result;
            }

            objReturnProcessModel = (ProcessMasterModel)Newtonsoft.Json.JsonConvert.DeserializeObject(resultString, typeof(ProcessMasterModel));
            return objReturnProcessModel;
        }

        public static ProcessMasterModel ProcessMasterModule_Delete(ProcessMasterModel objprocess)
        {
            ArrayList paramList = new ArrayList();
            string resultString;
            ProcessMasterModel objReturnProcessModel = null;
            ProcessMasterModel obj = new ProcessMasterModel();
            obj.ProcessId = objprocess.ProcessId;
           
            paramList.Add(obj);
            using (var client = new HttpClient())
            {
                Uri baseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["UriPath"]);
                client.BaseAddress = baseAddress;
                resultString = client.PostAsJsonAsync("api/ProcessMaster/ProcessMaster_Delete", paramList)
                                         .Result
                                         .Content.ReadAsStringAsync().Result;
            }

            objReturnProcessModel = (ProcessMasterModel)Newtonsoft.Json.JsonConvert.DeserializeObject(resultString, typeof(ProcessMasterModel));
            return objReturnProcessModel;
        }

        #endregion

        #region Product_Module
        public static IList<ProductMasterModel> GetProductMasterList(int ProcessId, int selectionType, string SearchBy, string SearchString, Guid CompanyId, int ItemsPerPage, int RequestPage, int CurrentPageNo)
        {
            ArrayList paramlist = new ArrayList();
            string resultString;
            IList<ProductMasterModel> objproductMasterModel = null;
            ProductMasterModel obj = new ProductMasterModel();
            string prosId = "";
            obj.SearchString = SearchString;
            obj.RequestType = selectionType;
            obj.ProcessId = prosId;
            obj.CompanyId = CompanyId;
            obj.ItemsPerPage = ItemsPerPage;
            obj.RequestPageNo = RequestPage;
            obj.CurrentPageNo = CurrentPageNo;
            paramlist.Add(obj);
            using (var client = new HttpClient())
            {
                Uri baseAddres = new Uri(System.Configuration.ConfigurationManager.AppSettings["UriPath"]);

                client.BaseAddress = baseAddres;
                resultString = client.PostAsJsonAsync("api/ProductMaster/ProductMaster_List", paramlist)
                                         .Result
                                         .Content.ReadAsStringAsync().Result;

            }
            objproductMasterModel = (IList<ProductMasterModel>)Newtonsoft.Json.JsonConvert.DeserializeObject(resultString, typeof(IList<ProductMasterModel>));
            return objproductMasterModel;

        }
        public static ProductMasterModel ProductMasterModule_InsertUpdate(ProductMasterModel objproduct)
        {
            ArrayList paramList = new ArrayList();
            string resultString;
            ProductMasterModel objProductModule = null;
            ProductMasterModel obj = new ProductMasterModel();
            obj.Productid = objproduct.Productid;
            obj.ProductName = objproduct.ProductName;
            obj.UOM= objproduct.UOM;
            obj.Color = objproduct.Color;
            //obj.State = objproduct.State;
            obj.Texture = objproduct.Texture;
            obj.CreatedBy = objproduct.CreatedBy;
            obj.CreatedDate = objproduct.CreatedDate;
            obj.IsDelete = objproduct.IsDelete;
            obj.SafeLifeInGodown = objproduct.SafeLifeInGodown;
            obj.ProcessId = objproduct.ProcessId;
            obj.BuyProductId = objproduct.BuyProductId;
            obj.BuyProductPacking = objproduct.BuyProductPacking;
            obj.CompanyId = objproduct.CompanyId;
            paramList.Add(obj);
            using (var client = new HttpClient())
            {
                Uri baseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["UriPath"]);
                client.BaseAddress = baseAddress;
                resultString = client.PostAsJsonAsync("api/ProductMaster/ProductMaster_InsertUpdate", paramList)
                                         .Result
                                         .Content.ReadAsStringAsync().Result;
            }

            objProductModule = (ProductMasterModel)Newtonsoft.Json.JsonConvert.DeserializeObject(resultString, typeof(ProductMasterModel));
            return objProductModule;
        }
        #endregion

        #region Field Code
        public static IList<FieldCodeModel> GetFieldCodeList(int? IntID, int selectionType, int FieldClassId, string SearchBy, string SearchString, Guid? CompanyId, int ItemsPerPage, int RequestPage, int CurrentPageNo)
        {
            ArrayList paramList = new ArrayList();
            string resultString;
            IList<FieldCodeModel> objFieldCodeModel = null;
            FieldCodeModel obj = new FieldCodeModel();
            obj.SearchString = SearchString;
            obj.RequestType = selectionType;
            obj.IntID = IntID;
            obj.CompanyId = CompanyId;
            obj.ItemsPerPage = ItemsPerPage;
            obj.RequestPageNo = RequestPage;
            obj.FieldClassId = FieldClassId;
            paramList.Add(obj);
            using (var client = new HttpClient())
            {
                Uri baseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["UriPath"]);
                client.BaseAddress = baseAddress;
                resultString = client.PostAsJsonAsync("api/FieldCode/FieldCode_List", paramList)
                                         .Result
                                         .Content.ReadAsStringAsync().Result;
            }

            objFieldCodeModel = (IList<FieldCodeModel>)Newtonsoft.Json.JsonConvert.DeserializeObject(resultString, typeof(IList<FieldCodeModel>));
            return objFieldCodeModel;
        }
        #endregion

      

        #region General Master InsertUpdate
        public static IList<GeneralMasterModel> GetGeneralMasterList(int? IntID, int selectionType, int GeneralMasterClassId, string SearchBy, string SearchString, Guid? CompanyId, int ItemsPerPage, int RequestPage, int CurrentPageNo)
        {
            ArrayList paramList = new ArrayList();
            string resultString;
            IList<GeneralMasterModel> objGeneralMasterModel = null;
            GeneralMasterModel obj = new GeneralMasterModel();
            obj.SearchString = SearchString;
            obj.RequestType = selectionType;
            obj.IntID = IntID;
            obj.CompanyId = CompanyId;
            obj.ItemsPerPage = ItemsPerPage;
            obj.RequestPageNo = RequestPage;
            obj.GeneralMasterClassId = GeneralMasterClassId;
            paramList.Add(obj);
            using (var client = new HttpClient())
            {
                Uri baseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["UriPath"]);
                client.BaseAddress = baseAddress;
                resultString = client.PostAsJsonAsync("api/GeneralMaster/GeneralMaster_List", paramList)
                                         .Result
                                         .Content.ReadAsStringAsync().Result;
            }

            objGeneralMasterModel = (IList<GeneralMasterModel>)Newtonsoft.Json.JsonConvert.DeserializeObject(resultString, typeof(IList<GeneralMasterModel>));
            return objGeneralMasterModel;
        }

        public static GeneralMasterModel GeneralMaster_InsertUpdate(GeneralMasterModel objgeneralclass)
        {
            ArrayList paramList = new ArrayList();
            string resultString;
            GeneralMasterModel objGeneralClassModel = null;
            GeneralMasterModel obj = new GeneralMasterModel();
            obj.GeneralMasterId = objgeneralclass.GeneralMasterId;
            obj.GeneralMasterClassId = objgeneralclass.GeneralMasterClassId;
            obj.GeneralMasterName = objgeneralclass.GeneralMasterName;
            obj.GeneralMasterCode = objgeneralclass.GeneralMasterCode;
            obj.GeneralMasterSquence = objgeneralclass.GeneralMasterSquence;
            obj.CompanyId = objgeneralclass.CompanyId;
            paramList.Add(obj);

            string apiCall = string.Empty;
            if (objgeneralclass.GeneralMasterId == 0)
            {
                apiCall = "api/GeneralMaster/GeneralMaster_InsertUpdate";
            }
            else
            {
                apiCall = "api/GeneralMaster/GeneralMaster_InsertUpdate";
            }
            using (var client = new HttpClient())
            {
                Uri baseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["UriPath"]);
                client.BaseAddress = baseAddress;
                resultString = client.PostAsJsonAsync(apiCall, paramList)
                                         .Result
                                         .Content.ReadAsStringAsync().Result;
            }
            objGeneralClassModel = (GeneralMasterModel)Newtonsoft.Json.JsonConvert.DeserializeObject(resultString, typeof(GeneralMasterModel));
            return objGeneralClassModel;
        }
        #endregion
        
        #region General Master Class
        public static IList<GeneralMasterClass> GetGeneralClassList(int? IntID, int selectionType, string SearchBy, string SearchString, Guid? CompanyId, int ItemsPerPage, int RequestPage, int CurrentPageNo)
        {
            ArrayList paramList = new ArrayList();
            string resultString;
            IList<GeneralMasterClass> objGeneralMasterModel = null;
            GeneralMasterClass obj = new GeneralMasterClass();
            obj.SearchString = SearchString;
            obj.RequestType = selectionType;
            obj.IntID = IntID;
            obj.CompanyId = CompanyId;
            obj.ItemsPerPage = ItemsPerPage;
            obj.RequestPageNo = RequestPage;
            paramList.Add(obj);
            using (var client = new HttpClient())
            {
                Uri baseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["UriPath"]);
                client.BaseAddress = baseAddress;
                resultString = client.PostAsJsonAsync("api/GeneralMasterClass/GeneralMasterClass_List", paramList)
                                         .Result
                                         .Content.ReadAsStringAsync().Result;
            }

            objGeneralMasterModel = (IList<GeneralMasterClass>)Newtonsoft.Json.JsonConvert.DeserializeObject(resultString, typeof(IList<GeneralMasterClass>));
            return objGeneralMasterModel;
        }
        #endregion

        #region UOMMaster
        public static IList<UOMMasterModel> GetUOMMasterList(int? IntID, int selectionType, string SearchBy, string SearchString, Guid? CompanyId, int ItemsPerPage, int RequestPageNo, int CurrentPageNo)
        {
            ArrayList paramList = new ArrayList();
            string resultString;
            IList<UOMMasterModel> objUOMMasterModel = null;
            UOMMasterModel obj = new UOMMasterModel();
            obj.SearchString = SearchString;
            obj.RequestType = selectionType;
            obj.IntID = IntID;
            obj.CompanyId = CompanyId;
            obj.ItemsPerPage = ItemsPerPage;
            obj.RequestPageNo = RequestPageNo;
            paramList.Add(obj);
            using (var client = new HttpClient())
            {
                Uri baseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["UriPath"]);
                client.BaseAddress = baseAddress;
                resultString = client.PostAsJsonAsync("api/UOMMaster/UOMMaster_List", paramList)
                                         .Result
                                         .Content.ReadAsStringAsync().Result;
            }

            objUOMMasterModel = (IList<UOMMasterModel>)Newtonsoft.Json.JsonConvert.DeserializeObject(resultString, typeof(IList<UOMMasterModel>));
            return objUOMMasterModel;
        }

        public static UOMMasterModel UOMMaster_InsertUpdate(UOMMasterModel objUOMMaster)
        {
            ArrayList paramList = new ArrayList();
            string resultString;
            UOMMasterModel objUOMMasterModel = null;
            UOMMasterModel obj = new UOMMasterModel();
            obj.UOMId = objUOMMaster.UOMId;
            obj.UOMTypeId = objUOMMaster.UOMTypeId;
            obj.ConversionRation = objUOMMaster.ConversionRation;
            obj.UOMName = objUOMMaster.UOMName;
            obj.UOMCode = objUOMMaster.UOMCode;
            obj.DecimalPoints = objUOMMaster.DecimalPoints;
            obj.BaseUnit = objUOMMaster.BaseUnit;
            obj.CompanyId = objUOMMaster.CompanyId;
            paramList.Add(obj);

            string apiCall = string.Empty;
            if (objUOMMaster.UOMId == 0)
            {
                apiCall = "api/UOMMaster/UOMMaster_InsertUpdate";
            }
            else
            {
                apiCall = "api/UOMMaster/UOMMaster_InsertUpdate";
            }
            using (var client = new HttpClient())
            {
                Uri baseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["UriPath"]);
                client.BaseAddress = baseAddress;
                resultString = client.PostAsJsonAsync(apiCall, paramList)
                                         .Result
                                         .Content.ReadAsStringAsync().Result;
            }

            objUOMMasterModel = (UOMMasterModel)Newtonsoft.Json.JsonConvert.DeserializeObject(resultString, typeof(UOMMasterModel));
            return objUOMMasterModel;
        }

        public static UOMConversionModel GetUOMConversions(int SourceUOMId, int DestinationUOMId, Guid? CompanyId)
        {
            ArrayList paramList = new ArrayList();
            string resultString;
            UOMConversionModel objUOMConversionModel = null;
            UOMConversionModel obj = new UOMConversionModel();
            obj.SourceUOMId = SourceUOMId;
            obj.DestinationUOMId = DestinationUOMId;
            obj.CompanyId = CompanyId;
            paramList.Add(obj);
            using (var client = new HttpClient())
            {
                Uri baseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["UriPath"]);
                client.BaseAddress = baseAddress;
                resultString = client.PostAsJsonAsync("api/UOMMaster/UOMConversion_Get", paramList)
                                         .Result
                                         .Content.ReadAsStringAsync().Result;
            }

            objUOMConversionModel = (UOMConversionModel)Newtonsoft.Json.JsonConvert.DeserializeObject(resultString, typeof(UOMConversionModel));
            return objUOMConversionModel;
        }

        #endregion
        #region Inventory_Module
        public static IList<InventoryModel> GetInventoryList(int? ReciptNo, int selectionType, string SearchBy, string SearchString, Guid? CompanyId, int ItemsPerPage, int RequestPage, int CurrentPageNo)
        {
            ArrayList paramlist = new ArrayList();
            string resultString;
            int ProsId = ReciptNo == null ? 0 : (int)ReciptNo;
            //IList<ProcessMasterModel> objprocessMasterModel = null;
            InventoryModel obj = new InventoryModel();
            obj.SearchString = SearchString;
            obj.RequestType = selectionType;
            obj. ReciptNo= ProsId;
            obj.ComapnyId = (Guid)CompanyId;
            obj.ItemsPerPage = ItemsPerPage;
            obj.RequestPageNo = RequestPage;
            obj.CurrentPageNo = CurrentPageNo;
            paramlist.Add(obj);
            using (var client = new HttpClient())
            {
                Uri baseAddres = new Uri(System.Configuration.ConfigurationManager.AppSettings["UriPath"]);

                client.BaseAddress = baseAddres;
                resultString = client.PostAsJsonAsync("api/InventoryMaster/InventoryMaster_List", paramlist)
                                         .Result
                                         .Content.ReadAsStringAsync().Result;

            }
            IList<InventoryModel> objinventorymodel = (IList<InventoryModel>)Newtonsoft.Json.JsonConvert.DeserializeObject(resultString, typeof(IList<InventoryModel>));
            return objinventorymodel;

        }
        public static InventoryModel InventoryModel_InsertUpdate(InventoryModel objprocess)
        {
            ArrayList paramList = new ArrayList();
            string resultString;
            InventoryModel objReturnInventory = null;
            InventoryModel obj = new InventoryModel();
            obj.ReciptNo = objprocess.ReciptNo;
            //obj.LoginId = objprocess.LoginId;
            obj.PersonName = objprocess.PersonName;
            obj.Quantity = objprocess.Quantity;
            obj.CreatedDate = objprocess.CreatedDate;
            obj.Unit= objprocess.Unit;
            
            //obj.IsDelete = objprocess.IsDelete;
            obj.RawMaterial = objprocess.RawMaterial;
            obj.Storage = objprocess.Storage;
            obj.ComapnyId = objprocess.ComapnyId;
            paramList.Add(obj);
            using (var client = new HttpClient())
            {
                Uri baseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["UriPath"]);
                client.BaseAddress = baseAddress;
                resultString = client.PostAsJsonAsync("api/InventoryMaster/Inventory_InsertUpdate", paramList)
                                         .Result
                                         .Content.ReadAsStringAsync().Result;
            }

            objReturnInventory = (InventoryModel)Newtonsoft.Json.JsonConvert.DeserializeObject(resultString, typeof(InventoryModel));
            return objReturnInventory;
        }
        #endregion
        #region ReciptNo
        public static InventoryModel GetList(InventoryModel ForList)
        {
            
            String resultString = "";
            InventoryModel models = null;
            InventoryModel obj = new InventoryModel();
            obj.ReciptNo = ForList.ReciptNo;
            using (var client = new HttpClient())
            {
                Uri baseAddres = new Uri(System.Configuration.ConfigurationManager.AppSettings["UriPath"]);

                client.BaseAddress = baseAddres;
                resultString = client.PostAsJsonAsync("api/InventoryMaster/GetList","")
                                         .Result
                                         .Content.ReadAsStringAsync().Result;

            }

            models = (InventoryModel)Newtonsoft.Json.JsonConvert.DeserializeObject(resultString, typeof(InventoryModel));
            return models;

        }
        #endregion

    }
}

