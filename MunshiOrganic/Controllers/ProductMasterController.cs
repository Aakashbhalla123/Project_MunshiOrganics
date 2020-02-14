using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MunshiModels.Models;
using AOneNutsWeb;
using Newtonsoft.Json;

namespace MunshiOrganic.Controllers
{
    public class ProductMasterController : Controller
    {
        // GET: ProcuctMaster
        public ActionResult Index()
        {
            string SearchBy = "";
            string SearchString = "";
            Guid CompanyId = Guid.Parse("f2d725f7-1d94-4cf9-95e5-cf409dc72c1b");
            int ItemsPerPage = 0;
            int RequestPage = 0;
            int CurrentPageNo = 0;
            int ProductId = 0;
            IList<ProductMasterModel> objProductsList = CommonCalls.GetProductMasterList(ProductId, 1, SearchBy, SearchString, CompanyId, ItemsPerPage, RequestPage, CurrentPageNo);
            return View(objProductsList);
         
        }
        public ActionResult ProductInser_Update(int ?ProductId)
        {
            ProductMasterModel objProductMaster = new ProductMasterModel();
            string SearchBy = "";
            string SearchString = "";
            Guid CompanyId = Guid.Parse("f2d725f7-1d94-4cf9-95e5-cf409dc72c1b");
            int ItemsPerPage = 0;
            int RequestPage = 0;
            int CurrentPageNo = 0;

            /*................General Masster List.......................*/
            IList<GeneralMasterModel> objtexture = CommonCalls.GetGeneralMasterList(0,1,2, SearchBy, SearchString, CompanyId, ItemsPerPage, RequestPage, CurrentPageNo);
            ViewBag.texture = objtexture;
            /*................General Masster List.......................*/
            IList<GeneralMasterModel> objpacking = CommonCalls.GetGeneralMasterList(0,1,6, SearchBy, SearchString, CompanyId, ItemsPerPage, RequestPage, CurrentPageNo);
            ViewBag.Packing = objpacking;
           
            /*................General Masster List.......................*/
            IList<GeneralMasterModel> objcolor = CommonCalls.GetGeneralMasterList(1,0, 3, SearchBy, SearchString, CompanyId, ItemsPerPage, RequestPage, CurrentPageNo);
            ViewBag.color = objcolor;
            int RequestPageNo = 0;
            /*................General Masster List.......................*/
            IList<UOMMasterModel> objUOMMaster = CommonCalls.GetUOMMasterList(0,1, SearchBy, SearchString, CompanyId, ItemsPerPage,RequestPageNo, CurrentPageNo);
            ViewBag.UOm = objUOMMaster;
            /*................Process List..........*/
            IList<ProcessMasterModel> objprocess = CommonCalls.GetProcessMasterList(1,1, SearchBy, SearchString, CompanyId, ItemsPerPage, RequestPageNo, CurrentPageNo);
            ViewBag.Process = objprocess;
            if (ProductId == null || ProductId == 0)
            {
                ViewBag.ButtonCaption = "Add ";
                ViewBag.PopupMessage = "";
            }
            else
            {
                ViewBag.ButtonCaption = "Update";
                IList<ProductMasterModel> objProductMasterList = CommonCalls.GetProductMasterList(0, 1, SearchBy, SearchString, CompanyId, ItemsPerPage, RequestPage, CurrentPageNo);
                objProductMaster = objProductMasterList[0];
            }
            return View(objProductMaster);
        }

        [HttpPost]
        public ActionResult ProductInser_Update(ProductMasterModel pbj)
        {
            pbj.CompanyId = Guid.Parse("f2d725f7-1d94-4cf9-95e5-cf409dc72c1b".ToString());
            string selectedValue = string.Join(",", pbj.ProcessIds);
            pbj.ProcessId = selectedValue;
            pbj = CommonCalls.ProductMasterModule_InsertUpdate(pbj);
           
            if (pbj.Productid ==0 )
                ViewBag.ButtonCaption = "Add ";
            else
                ViewBag.ButtonCaption = "Update ";
            if (pbj.ReturnCode == 0)
            { 
                ViewBag.PopupMessage = pbj.ReturnMessage;
                return RedirectToAction("Index", "ProductMaster");
            }
            else if (pbj.ReturnCode == 101)
            {
                ViewBag.PopupMessage = pbj.ReturnMessage;
                return RedirectToAction("Index", "ProductMaster");
            }
            return View("ProductInser_Update", pbj);

        }

    }
}