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
    public class DialogController : Controller
    {
        // GET: Dialog
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            var intent = new DialogService().GetEditViewModel(id);
            return PartialView(intent);
        }

        public ActionResult Create()
        {
            var intents = new IntentService().GetAllForCreatingDialog();
            return PartialView(intents);
        }

        [HttpPost]
        public JsonResult Create(DialogCreateViewModel model)
        {
            DialogService service = new DialogService();
            try
            {
                service.Create(model);
                return Json(new ResponseMessage() { Message = "Đã tạo thành công", Success = true });

            }
            catch (Exception e)
            {
                return Json(new ResponseMessage() { Message = "Đã có lỗi xảy ra", Success = false });
            }
        }

        [HttpPost]
        public JsonResult Update(SimpleDialogEditViewModel model)
        {
            DialogService service = new DialogService();
            try
            {
                service.Update(model);
                return Json(new ResponseMessage() { Message = "Đã tạo thành công", Success = true });

            }
            catch (Exception e)
            {
                return Json(new ResponseMessage() { Message = "Đã có lỗi xảy ra", Success = false });
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            DialogService dialogService = new DialogService();
            dialogService.Delete(id);
            return Json(new ResponseMessage() { Message = "Đã tạo thành công", Success = true });

        }
    }
}