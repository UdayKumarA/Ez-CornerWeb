using Evolutyz.Business;
using Evolutyz.Data;
using Evolutyz.Entities;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace EvolutyzCorner.UI.Web.Controllers.Tasks
{
    //[Authorize(Roles = "Admin")]
    [Authorize]
    [EvolutyzCorner.UI.Web.MvcApplication.SessionExpire]
    [EvolutyzCorner.UI.Web.MvcApplication.NoDirectAccess]
    public class TaskController : Controller
    {
        //UserSessionInfo objSessioninfo = new UserSessionInfo();
        ProjectTaskComponent objDtl = new ProjectTaskComponent();
        public ActionResult Index()
        {

            HomeController hm = new HomeController();
            var obj = hm.GetAdminMenu();
            foreach (var item in obj)
            {

                if (item.ModuleName == "Add main Tasks")
                {
                    var mk = item.ModuleAccessType;


                    ViewBag.a = mk;

                }

            }


            var Tasks = objDtl.GetTasks().Select(a => new SelectListItem()
            {
                Value = a.tsk_TaskID.ToString(),
                Text = a.tsk_TaskName,
            });
            ViewBag.TasksList = Tasks;

            var accounts = objDtl.Getaccounts().Select(a => new SelectListItem()
            {
                Value = a.Acc_AccountID.ToString(),
                Text = a.Acc_AccountName,
            });
            ViewBag.acclist = accounts;
            UserSessionInfo info = new UserSessionInfo();
            int accid = info.AccountId;
            ViewBag.accid = accid;

            return View();
           
        }

      

        public string CreateTask([Bind(Exclude = "tsk_TaskID")] TaskEntity TaskDtl)
        {
            string strResponse = string.Empty;
            try
            {
                var TaskComponent = new TaskComponent();

                if (ModelState.IsValid)
                {
                    UserSessionInfo _objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
                    int _userID = _objSessioninfo.UserId;
                    TaskDtl.tsk_CreatedBy = _userID;

                    var Org = new TaskComponent();
                    int r = Org.AddTask(TaskDtl);
                    if (r > 0)
                    {
                        strResponse = "Record created successfully";
                    }
                    else if (r == 0)
                    {
                        strResponse = "Record already exists";
                    }
                    else if (r < 0)
                    {
                        strResponse = "Error occured in CreateTask";
                    }
                }
            }
            catch (Exception ex)
            {
                return strResponse;
            }
            return strResponse;
        }

        public string UpdateTask(TaskEntity TaskDtl)
        {
            string strResponse = string.Empty;
            short UsTCurrentVersion = 0;
            try
            {
                var TaskComponent = new TaskComponent();
                var currentTaskDetails = TaskComponent.GetTaskDetailByID(TaskDtl.tsk_TaskID);
                int TaskID = currentTaskDetails.tsk_TaskID;
                UsTCurrentVersion = Convert.ToInt16(currentTaskDetails.tsk_Version);
                bool _currentStatus = false;

                //check for version and active status
                if (ModelState["tsk_ActiveStatus"].Value != null)
                {
                    _currentStatus = TaskDtl.tsk_ActiveStatus == true;
                }

                if (ModelState.IsValid)
                {
                    UserSessionInfo _objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
                    int _userID = _objSessioninfo.UserId;
                    TaskDtl.tsk_ModifiedBy = _userID;
                    //while udating increment version by1
                    TaskDtl.tsk_Version = ++UsTCurrentVersion;
                    TaskDtl.tsk_ActiveStatus = _currentStatus;

                    var Org = new TaskComponent();
                    int r = Org.UpdateTaskDetail(TaskDtl);

                    if (r > 0)
                    {
                        strResponse = "Record updated successfully";
                    }
                    else if (r == 0)
                    {
                        strResponse = "Record does not exists";
                    }
                    else if (r < 0)
                    {
                        strResponse = "Error occured in UpdateTask";
                    }
                }
            }
            catch (Exception ex)
            {
                return strResponse;
            }
            return strResponse;
        }

        public string DeleteTask(int TaskID)
        {
            string strResponse = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    var Org = new TaskComponent();
                    int r = Org.DeleteTaskDetail(TaskID);

                    if (r > 0)
                    {
                        strResponse = "Record deleted successfully";
                    }
                    else if (r == 0)
                    {
                        strResponse = "Record does not exists";
                    }
                    else if (r < 0)
                    {
                        strResponse = "Error occured in DeleteTask";
                    }
                }
            }
            catch (Exception ex)
            {
                return strResponse;
            }
            return strResponse;
        }

        public JsonResult GetTaskCollection()
        {
            List<AccountSpecifictasks> TaskDetails = null;
            try
            {
                var objDtl = new ProjectTaskComponent();
                TaskDetails = objDtl.GetAllTasks();
             
            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(TaskDetails, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTaskByID(int catID)
        {
            TaskEntity TaskDetails = null;
            try
            {
                var objDtl = new TaskComponent();
                TaskDetails = objDtl.GetTaskDetailByID(catID);
            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(TaskDetails, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetTaskHistoryByID(int ID)
        //{
        //    List<History_TasksEntity> HisTaskDetails = null;
        //    try
        //    {
        //        var objDtl = new TaskComponent();
        //        HisTaskDetails = objDtl.GetHistoryTaskDetailsByID(ID);
        //        ViewBag.TaskDetails = HisTaskDetails;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    return Json(HisTaskDetails, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public string CrudOperation(FormCollection formCollection)
        {
            string strResponse = string.Empty;
            try
            {
                var strOperation = formCollection["oper"];

                if (strOperation == "add")
                {
                    TaskEntity updateList = new TaskEntity
                    {
                        tsk_TaskName = formCollection.Get("tsk_TaskName"),
                        tsk_TaskDescription = formCollection.Get("tsk_TaskDescription"),
                        tsk_Version = 1,
                        tsk_ActiveStatus = Convert.ToBoolean(formCollection.Get("tsk_ActiveStatus")),
                        tsk_isDeleted = false
                    };
                    strResponse = CreateTask(updateList);
                }
                else if (strOperation == "edit")
                {
                    TaskEntity updateList = new TaskEntity
                    {
                        tsk_TaskID = Convert.ToInt32(formCollection.Get("id")),
                        tsk_TaskName = formCollection.Get("tsk_TaskName"),
                        tsk_TaskDescription = formCollection.Get("tsk_TaskDescription"),
                        tsk_ActiveStatus = Convert.ToBoolean(formCollection.Get("tsk_ActiveStatus")),
                        tsk_isDeleted = Convert.ToBoolean(formCollection.Get("tsk_isDeleted"))
                    };
                    strResponse = UpdateTask(updateList);
                }
                else if (strOperation == "del")
                {
                    int TaskID = Convert.ToInt32(formCollection.Get("id"));
                    strResponse = DeleteTask(TaskID);
                }
            }
            catch (Exception ex)
            {
                return strResponse;
            }
            return strResponse;
        }

        #region Export to PDF

        private static void ExportPDF<TSource>(IList<TSource> TaskList, string[] columns, string filePath)
        {
            iTextSharp.text.Font headerFont = FontFactory.GetFont("Verdana", 10, iTextSharp.text.Color.WHITE);
            iTextSharp.text.Font rowfont = FontFactory.GetFont("Verdana", 10, iTextSharp.text.Color.BLUE);
            Document document = new Document(PageSize.A4);

            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.OpenOrCreate));
            document.Open();
            PdfPTable table = new PdfPTable(columns.Length);
            foreach (var column in columns)
            {
                PdfPCell cell = new PdfPCell(new Phrase(column, headerFont));
                cell.BackgroundColor = iTextSharp.text.Color.BLACK;
                table.AddCell(cell);
            }

            foreach (var item in TaskList)
            {
                foreach (var column in columns)
                {
                    string value = item.GetType().GetProperty(column).GetValue(item).ToString();
                    PdfPCell cell5 = new PdfPCell(new Phrase(value, rowfont));
                    table.AddCell(cell5);
                }
            }

            document.Add(table);
            document.Close();
        }
        #endregion        


        //////// new projectspecific task ///////////
        [HttpPost]
        public string SaveTasks(string Acc_AccountID, string tsk_TaskID,string Acc_SpecificTaskName, string StatusId)
        {
           
            string response = objDtl.SaveTasks(Acc_AccountID, tsk_TaskID,Acc_SpecificTaskName,StatusId);

            return response;
        }
        
        [HttpPost]
        public JsonResult Gettaskbyid(string id)
        {
            AccountSpecifictasks TaskDetails = null;
            try
            {
              
                TaskDetails = objDtl.GetTaskDetailByID(id);
            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(TaskDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string UpdateTasks(int id,string ProjectId, string tsk_TaskID, string Proj_SpecificTaskName, string StatusId)
        {

            string response = objDtl.UpdateTasks(id,ProjectId, tsk_TaskID, Proj_SpecificTaskName, StatusId);

            return response;
        }
        [HttpPost]
        public string deletetaskbyid(int id)
        {
            string strResponse = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                   // var Org = new TaskComponent();
                    int r = objDtl.deletetaskbyid(id);

                    if (r ==1)
                    {
                        strResponse = "Record deleted successfully";
                    }
                    else if (r == 0)
                    {
                        strResponse = "Record does not exists";
                    }
                    else if (r ==-1)
                    {
                        strResponse = "Error occured in DeleteTask";
                    }
                    else if (r == 2)
                    {
                        strResponse = "This Task Assigned to Some Users";
                    }
                }
            }
            catch (Exception ex)
            {
                return strResponse;
            }
            return strResponse;
        }


        public string ChangeStatus(string id, string status)
        {
            string strResponse = string.Empty;
            var objDtl = new TaskComponent();
            strResponse = objDtl.ChangeStatus(id, status);

            return strResponse;
        }

    }
}