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
    public class ProcessMasterController : Controller
    {
        // GET: ProcessMaster
        public ActionResult Index()
        {
            string SearchBy = "";
            string SearchString = "";
            Guid? CompanyId = Guid.Parse("5a6f68ba-2b74-479d-8e5d-525a42a6196b".ToString());
            int ItemsPerPage = 0;
            int RequestPage = 0;
            int CurrentPageNo = 0;
            int ProcessId = 0;
            int? Id = null;
            int RequestPageNo = 0;
            IList<ProcessMasterModel> objProcessList = CommonCalls.GetProcessMasterList(ProcessId, 1, SearchBy, SearchString, CompanyId,ItemsPerPage,RequestPage, CurrentPageNo);
            return View(objProcessList);
        }
        public ActionResult ProcessInser_Update(int ?ProcessId)
        {
            ProcessMasterModel objProcessMaster = new ProcessMasterModel();
            string SearchBy = "";
            string SearchString = "";
            Guid? CompanyId = Guid.Parse("5a6f68ba-2b74-479d-8e5d-525a42a6196b".ToString());
            int ItemsPerPage = 0;
            int RequestPage = 0;
            int RequestPageNo = 0;
            int CurrentPageNo = 0;
            int Id = 0;
            IList<UOMMasterModel> objUOMMaster = CommonCalls.GetUOMMasterList(0,1, SearchBy, SearchString, CompanyId, ItemsPerPage, RequestPageNo, CurrentPageNo);
            string output = JsonConvert.SerializeObject(objUOMMaster);
            ViewBag.Menue = output;
            IList<ProcessMasterModel> objProcessList = CommonCalls.GetProcessMasterList(0, 1, SearchBy, SearchString,CompanyId, ItemsPerPage, RequestPage, CurrentPageNo);
            ViewBag.ProcessList = objProcessList;

            /*................General Masster List.......................*/
            IList<GeneralMasterModel> objvolumeList = CommonCalls.GetGeneralMasterList(Id, 1, 3, SearchBy, SearchString, CompanyId, ItemsPerPage, RequestPage, CurrentPageNo);
            ViewBag.Volume = objvolumeList;
            
            /*................General Masster List.......................*/
            IList<GeneralMasterModel> objdurationList = CommonCalls.GetGeneralMasterList(0,1, 4, SearchBy, SearchString, CompanyId, ItemsPerPage, RequestPage, CurrentPageNo);
            ViewBag.duration = objdurationList;

            if (ProcessId == null || ProcessId == 0)
            {
                ViewBag.ButtonCaption = "Add ";
                ViewBag.PopupMessage = "";
            }
            else
            {
                ViewBag.ButtonCaption = "Update";
                IList<ProcessMasterModel> objReturnProcess = CommonCalls.GetProcessMasterList(ProcessId,0, SearchBy, SearchString,CompanyId, ItemsPerPage, RequestPage, CurrentPageNo);
                objProcessMaster = objReturnProcess[0];
            }
            return View(objProcessMaster);
        }

        [HttpPost]
        public ActionResult ProcessInser_Update(ProcessMasterModel pbj)
        {
            pbj.CompanyId = Guid.Parse("5a6f68ba-2b74-479d-8e5d-525a42a6196b".ToString());
            pbj = CommonCalls.ProcessMasterModule_InsertUpdate(pbj);
            if(pbj.ProcessId == 0)
                ViewBag.ButtonCaption = "Add ";
            else
                ViewBag.ButtonCaption = "Update ";
            if (pbj.ReturnCode == 0)
            {
                ViewBag.PopupMessage = pbj.ReturnMessage;
                return RedirectToAction("Index", "ProcessMaster");
            }
            else if (pbj.ReturnCode == 101)
            {
                ViewBag.PopupMessage = pbj.ReturnMessage;
                return RedirectToAction("Index", "ProcessMaster");
            }


            return View();

        }

      
        public ActionResult Process_Delete(int id)
        {
            ProcessMasterModel pbj = new ProcessMasterModel();
            pbj.ProcessId = id;
              pbj = CommonCalls.ProcessMasterModule_Delete(pbj);

            if (pbj.ReturnCode == 101)
            {
                ViewBag.PopupMessage = pbj.ReturnMessage;
                return RedirectToAction("Index", "ProcessMaster");
            }

            return RedirectToAction("Index", "ProcessMaster");

        }
    }

    
}
    
