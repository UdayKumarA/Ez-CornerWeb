using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Evolutyz.Entities;
using Evolutyz.Business;

using System.IO;
using System.Web.Configuration;
namespace EvolutyzCorner.UI.Web.Controllers
{
    [Authorize]
    [EvolutyzCorner.UI.Web.MvcApplication.SessionExpire]
    [EvolutyzCorner.UI.Web.MvcApplication.NoDirectAccess]

    public class NewsBoardController : Controller
    {
        // GET: NewsBoard
        public ActionResult Index()
        {
            HomeController hm = new HomeController();
            var obj = hm.GetAdminMenu();
            var mk = string.Empty;
            foreach (var item in obj)
            {

                if (item.ModuleName == "NewsBoard")
                {
                    mk = item.ModuleAccessType;


                    ViewBag.a = mk;


                }

            }
            if (mk == "Read/Write")
            {
                return View();
            }
            else
            {
                return RedirectToAction("PreviewNews");
            }


        }


        public JsonResult GetNewsCollection()
        {
            List<NewsboardEntity> AccDetails = null;
            try
            {
                var objDtl = new NewBoardComponent();
                AccDetails = objDtl.GetNewsCollection();
               
            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(AccDetails, JsonRequestBehavior.AllowGet);
        }

        public string AddNews(NewsboardEntity news)
        {
            string strResponse = string.Empty;
            var orgcomponent = new NewBoardComponent();
            UserSessionInfo objSessioninfo = Session["UserSessionInfo"] as UserSessionInfo;
            int _userID = objSessioninfo.UserId;
            news.CreatedBy = _userID;
            string imagename = string.Empty;
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                var fileName = "/uploadimages/Images/" + file.FileName;
                imagename = file.FileName;
                var imagepath = Server.MapPath(fileName);
                file.SaveAs(imagepath);
            }
            if (imagename=="")
            {
                news.Image = "newsDefault.jpg";
            }
            else
            {
                news.Image = imagename;
            }
         
            int r = orgcomponent.AddNews(news);
            if (r > 0)
            {
                strResponse = "News Added successfully";
            }
            else if (r == 0)
            {
                strResponse = "News already exists";
            }
            else if (r < 0)
            {
                strResponse = "Error occured in Adding News";
            }
            return strResponse;

        }


        public JsonResult GetNewsById(int id)
        {
           NewsboardEntity AccDetails = null;
            try
            {
                var objDtl = new NewBoardComponent();
                AccDetails = objDtl.GetNewsById(id);

            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(AccDetails, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PreviewNews()
        {
            var newsboardcomp = new NewBoardComponent();
            var news = newsboardcomp.GetNewsCollection();

            ViewBag.news = news;
            return View();
        }

    }
}