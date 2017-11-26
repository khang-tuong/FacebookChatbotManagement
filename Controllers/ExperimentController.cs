using FacebookChatbotManagement.Models.Entities;
using FacebookChatbotManagement.Models.Services;
using FacebookChatbotManagement.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FacebookChatbotManagement.Controllers
{
    public class ExperimentController : Controller
    {
        // GET: Experiment
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Analyze(string input)
        {
            ExperimentService experimentService = new ExperimentService();
            ExperimentResultViewModel pattern = experimentService.Analyze(input);
            return Json(pattern, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateBot()
        {
            ExperimentService experimentService = new ExperimentService();
            experimentService.PushToRedis();
            return Json("okay");
        }
    }
}