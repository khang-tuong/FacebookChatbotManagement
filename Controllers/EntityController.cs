using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FacebookChatbotManagement.Models.Services;
using FacebookChatbotManagement.Models;

namespace FacebookChatbotManagement.Controllers
{
    public class EntityController : Controller
    {
        // GET: Entity
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            EntityService service = new EntityService();
            var entity = service.FirstOrDefault(q => q.Id == id);
            return PartialView(entity);
        }

        public ActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult Create(string name, string words)
        {
            EntityService service = new EntityService();
            try
            {
                var entity = service.Add(new Models.Entities.Entity()
                {
                    Name = name,
                    Words = words,
                });
                return Json(new ResponseMessage() { Message = "Đã tạo thành công", Success = true});
            }
            catch (Exception e)
            {
                return Json(new ResponseMessage() { Message = "Đã có lỗi xảy ra", Success = false });
            }
        }

        [HttpPost]
        public JsonResult Update(int id, string name, string words)
        {
            EntityService entityService = new EntityService();
            entityService.Update(id, name, words);
            return Json(new ResponseMessage() { Message = "Đã update thành công", Success = true });
        }
    }
}