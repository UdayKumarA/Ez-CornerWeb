using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Evolutyz.Data;
using Newtonsoft.Json;
using EvolutyzCorner.UI.Web.Models;
using System.Data.Entity.SqlServer;
using Evolutyz.Entities;

namespace EvolutyzCorner.UI.Web.Controllers
{
    [Authorize]
    [EvolutyzCorner.UI.Web.MvcApplication.SessionExpire]
    public class TimeSheetManagementController : Controller
    {
        EvolutyzCornerDataEntities entities = new EvolutyzCornerDataEntities();

        // GET: TimeSheetManagement
        public JsonResult Getdropdown()
        {

            EvolutyzCornerDataEntities db = new EvolutyzCornerDataEntities();

            IEnumerable<SelectListItem> items = db.Projects.Select(c => new SelectListItem
            {
                Value = c.Proj_ProjectName,
                Text = c.Proj_ProjectName

            });
          //  ViewBag.ProjName = items;
            ViewBag.ProjName = JsonConvert.SerializeObject(items, Formatting.Indented);
            //var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            //String serializedResult = serializer.Serialize(items);
            //ViewBag.ProjName = serializedResult;

            return Json(ViewBag.ProjName, JsonRequestBehavior.AllowGet);
        }
        public ActionResult TimeSheetSubmit()
        {
            // List<SelectListItem> obj = new List<SelectListItem>();

            //  List<SelectListItem> obj = GetCustomers();

            EvolutyzCornerDataEntities db = new EvolutyzCornerDataEntities();

            IEnumerable<SelectListItem> items = db.Projects.Select(c => new SelectListItem
            {
                Value = SqlFunctions.StringConvert((double)c.Proj_ProjectID),
                Text = c.Proj_ProjectName

            });
            ViewBag.client = items;

            IEnumerable<SelectListItem> itemssss = db.GenericTasks.Select(c => new SelectListItem
            {

             Value = SqlFunctions.StringConvert((double)c.tsk_TaskID),
                Text = c.tsk_TaskName

            });

            ViewBag.taskname = itemssss;

            return View();
        }

    


  public JsonResult values(string TaskDate, string ProjectId, string taskid, string hoursworked, string comment)
        {
            try
            {
                if (Session["userid"] != null)
                {

                    TIMESHEET objj = new TIMESHEET();

                    objj.UserID = Convert.ToInt32(Session["userid"]);

                    objj.TaskDate = Convert.ToDateTime(TaskDate);

                    objj.Comments = comment;

                    entities.TIMESHEETs.Add(objj);


                    entities.SaveChanges();

                    TaskDetail tsk = new TaskDetail();
                    tsk.TimesheetID = objj.TimesheetID;


                    tsk.ProjectID = Convert.ToInt32(ProjectId);

                    tsk.HoursWorked = hoursworked;
                    tsk.TaskID = Convert.ToInt32(taskid);
                    entities.TaskDetails.Add(tsk);
                    entities.SaveChanges();
                }
            }
            catch (Exception ex)
            {

            }

            return Json("succesfully inserted", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //public JsonResult SaveTimeSheet(TimesheetEntity timesheetEntity)
        public JsonResult SaveTimeSheet(string dtTaskDate, TimesheetEntity timesheetEntity)
        {

            TIMESHEET objTimeSheet = new TIMESHEET();
            List<TaskDetail> lstDetail = new List<TaskDetail>();

            objTimeSheet.TaskDate =Convert.ToDateTime(dtTaskDate);

            objTimeSheet.UserID = Convert.ToInt32(Session["userid"]);

            objTimeSheet.Comments = timesheetEntity.Comments;

            entities.TIMESHEETs.Add(objTimeSheet);
            int iResult= entities.SaveChanges();

            if (iResult==1)
           {
                
                foreach(TaskDetailEntity objTaskDetail in timesheetEntity.taskDetails)
                {
                    TaskDetail objTaskdetail = new TaskDetail();

                    objTaskdetail.ProjectID = objTaskDetail.ProjectID;

                    objTaskdetail.TaskID = objTaskDetail.TaskID;

                    objTaskdetail.TimesheetID = objTimeSheet.TimesheetID;

                    objTaskdetail.HoursWorked = objTaskDetail.HoursWorked;


                    lstDetail.Add(objTaskdetail);
               }
            }
            entities.TaskDetails.AddRange(lstDetail);
            entities.SaveChanges();

            return Json("test",JsonRequestBehavior.AllowGet);
        }
     

         

            public ActionResult NewSheet(string TaskDate)
        {
            //   TIMESHEET objj = new TIMESHEET();

            // objj.UserID = Convert.ToInt32(Session["userid"]);

            //entities.sp_timesheetdetails(Convert.ToInt32(Session["userid"]),Convert.ToDateTime(TaskDate));


            return Json("", JsonRequestBehavior.AllowGet);
        }


            //    return Json("succesfully inserted", JsonRequestBehavior.AllowGet);
            //}
            public ActionResult Timesheet()
        {


            return View();
        }
        [HttpPost]
            public ActionResult Timesheet(EvolutyzCornerDataEntities db)
        {

           // EvolutyzCornerDataEntities db = new EvolutyzCornerDataEntities();

            IEnumerable<SelectListItem> items = db.Projects.Select(c => new SelectListItem
            {



              Value=c.Proj_ProjectName,
                Text = c.Proj_ProjectName

            });
          


            return Json(items,JsonRequestBehavior.AllowGet);
        }

   
        public ActionResult ApproveOrRejectTimeSheet()
        {
            return View();
        }
       
        public ActionResult Details(int id)
        {
            return View();
        }

        
        public ActionResult Create()
        {
            return View();
        }

        // POST: TimeSheetManagement/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: TimeSheetManagement/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TimeSheetManagement/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: TimeSheetManagement/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TimeSheetManagement/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Charts()
        {



            return View();
        }

        public JsonResult chartsdata()
        {

            var person = entities.Projects.ToList();


            return Json(person, JsonRequestBehavior.AllowGet);
        }
    }
}
