using FacebookChatbotManagement.Models;
using FacebookChatbotManagement.Models.Services;
using FacebookChatbotManagement.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FacebookChatbotManagement.Controllers
{
    public class IntentController : Controller
    {
        // GET: Intent
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            var intent = new IntentService().GetEditViewModel(id);
            return PartialView(intent);
        }

        public ActionResult Create()
        {
            IntentCreateViewModel model = new IntentCreateViewModel();
            model.Patterns = new PatternService().GetAll();
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult Create(string name, int[] patterns, int[] groups)
        {
            IntentService service = new IntentService();
            try
            {
                service.Create(name, patterns, groups);
                return Json(new ResponseMessage() { Message = "Đã tạo thành công", Success = true});

            }
            catch (Exception e)
            {
                return Json(new ResponseMessage() { Message = "Đã có lỗi xảy ra", Success = false });
            }
        }

        [HttpPost]
        public JsonResult Update(int id, string name, int[] patterns, int[] groups)
        {
            IntentService service = new IntentService();
            try
            {
                service.Update(id, name, patterns, groups);
                return Json(new ResponseMessage() { Message = "Đã tạo thành công", Success = true });

            }
            catch (Exception e)
            {
                return Json(new ResponseMessage() { Message = "Đã có lỗi xảy ra", Success = false });
            }
        }
    }
}