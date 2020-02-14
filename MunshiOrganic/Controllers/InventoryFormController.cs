using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MunshiModels.Models;
using Newtonsoft.Json;
using AOneNutsWeb;

namespace MunshiOrganic.Controllers
{
    public class InventoryFormController : Controller
    {
        private object searchType;

        // GET: InventoryForm
        public ActionResult Index()
        {
            
            string SearchBy = "";
            string SearchString = "";
            Guid? CompanyId = Guid.Parse("d36f51cc-d8d9-4168-9d67-3462ef7b479f".ToString());
            int ItemsPerPage = 0;
            int RequestPage = 0;
            int CurrentPageNo = 0;
            int ReciptNo = 0;
            int? Id = null;
            int RequestPageNo = 0;
            IList<InventoryModel> objInventoryList = CommonCalls.GetInventoryList(ReciptNo, 1, SearchBy, SearchString, CompanyId, ItemsPerPage, RequestPage, CurrentPageNo);
            return View(objInventoryList);
        }

        public ActionResult Inventory_Insert(int ?ReciptNo)
        {
            InventoryModel objProcessMaster = new InventoryModel();
            string SearchBy = "";
            string SearchString = "";
            Guid? CompanyId = Guid.Parse("5a6f68ba-2b74-479d-8e5d-525a42a6196b".ToString());
            int ItemsPerPage = 0;
            string selectiontype = "";
            int RequestPage = 0;
            int RequestPageNo = 0;
            int CurrentPageNo = 0;
            int Id = 0;
           
            if (ReciptNo == null || ReciptNo == 0)
            {
                DateTime dateob = DateTime.Now;
                objProcessMaster.Current = dateob;
                
                ViewBag.ButtonCaption = "Add ";
                ViewBag.PopupMessage = "";
                objProcessMaster= CommonCalls.GetList(objProcessMaster);
                ViewBag.FillRecipt = objProcessMaster.NewReciptNo;
            }
            else
            {
                ViewBag.ButtonCaption = "Update";
                IList<InventoryModel> objReturnInventory = CommonCalls.GetInventoryList(ReciptNo,0, SearchBy, SearchString, CompanyId, ItemsPerPage, RequestPage, CurrentPageNo);
                objProcessMaster = objReturnInventory[0];
            }
            return View(objProcessMaster);
        }


        [HttpPost]
        public ActionResult Inventory_Insert(InventoryModel obj)
        {
           obj.ComapnyId = Guid.Parse("5a6f68ba-2b74-479d-8e5d-525a42a6196b".ToString());
            obj = CommonCalls.InventoryModel_InsertUpdate(obj);
            if (obj.ReciptNo == 0)
                ViewBag.ButtonCaption = "Add ";
            else
                ViewBag.ButtonCaption = "Update ";
            if (obj.ReturnCode == 0)
            {
                TempData["ReturnMessage"] = obj.ReturnMessage;
                return RedirectToAction("Index", "InventoryForm");
            }
            else if (obj.ReturnCode == 101)
            {
                TempData["ReturnMessage"] = obj.ReturnMessage;
                return RedirectToAction("Index", "InventoryForm");
            }
            else
            {
                TempData["ReturnMessage"] = obj.ReturnMessage;
            }
            return View("Inventory_Insert", obj);

        }
    }
}