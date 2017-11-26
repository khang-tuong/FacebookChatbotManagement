using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FacebookChatbotManagement.Models.Services;
using FacebookChatbotManagement.Models.ViewModels;

namespace FacebookChatbotManagement.Controllers
{
    public class BotController : Controller
    {
        // GET: Bot
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Entities()
        {
            var entities = new EntityService().GetAll();
            return View(entities);
        }

        public ActionResult Patterns()
        {
            var patterns = new PatternService().GetAll();
            return View(patterns);
        }

        public ActionResult Intents()
        {
            var intents = new IntentService().GetAll();
            return View(intents);
        }

        public ActionResult Dialogs()
        {
            var dialogs = new DialogService().GetAll();
            return View(dialogs);
        }

        public ActionResult Experiment()
        {
            return View();
        }
    }
}