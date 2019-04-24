using Evolutyz.Business;
using Evolutyz.Data;
using Evolutyz.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EvolutyzCorner.UI.Web.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        EvolutyzCornerDataEntities db = new EvolutyzCornerDataEntities();
        public ActionResult Index()
        {

            return View();
        }


        public JsonResult getLookUp()
        {
            UserSessionInfo _objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
            int userid = _objSessioninfo.UserId;
            AdminComponent admComp = new AdminComponent();
            List<TaskLookupEntity> ListLookUp = admComp.GetLookUp();
            return Json(ListLookUp, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetLookUpByEmpId(string Userid)
        {
          
            int userid = Convert.ToInt32(Userid);
            AdminComponent admComp = new AdminComponent();
            List<TaskLookupEntity> ListLookUp = admComp.GetLookUpByEmpId(userid);
            return Json(ListLookUp, JsonRequestBehavior.AllowGet);
        }


        //public List<TaskLookupEntity> getLookUp()
        //{
        //    using (EvolutyzCornerDataEntities context = new EvolutyzCornerDataEntities())
        //    {
        //        List<TaskLookupEntity> dic = (from p in context.TaskLookups
        //                                select new TaskLookupEntity
        //                                {
        //                                    tsk_TaskID = p.tsk_TaskID,
        //                                    tsk_TaskName = p.tsk_TaskName
        //                                }
        //                                ).ToList();
        //        return dic;
        //    }
        //}
    }
}