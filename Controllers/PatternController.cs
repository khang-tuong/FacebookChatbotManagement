using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FacebookChatbotManagement.Models.Services;
using FacebookChatbotManagement.Models;
using FacebookChatbotManagement.Models.ViewModels;

namespace FacebookChatbotManagement.Controllers
{
    public class PatternController : Controller
    {
        // GET: Pattern
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            PatternService service = new PatternService();
            PatternEditViewModel patternEditViewModel = service.GetEditViewModel(id);
            return PartialView(patternEditViewModel);
        }

        public ActionResult Create()
        {
            PatternCreateViewModel model = new PatternCreateViewModel();
            model.Entities = new EntityService().GetAll();
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult Create(int[] entities, string name, bool matchBegin, bool matchEnd)
        {
            PatternService service = new PatternService();
            PatternEntityMappingService peService = new PatternEntityMappingService();
            try
            {
                var pattern = service.Add(new Models.Entities.Pattern
                {
                    MatchBegin = matchBegin,
                    MatchEnd = matchEnd,
                    Name = name,
                    Active = true
                });

                int i = 0;
                foreach (var entity in entities)
                {
                    peService.Add(new Models.Entities.PatternEntityMapping()
                    {
                        Active = true,
                        EntityId = entity,
                        PatternId = pattern.Id,
                        Position = i++
                    });
                }
                return Json(new ResponseMessage() { Message = "Đã tạo thành công", Success = true });
            }
            catch (Exception)
            {
                return Json(new ResponseMessage() { Message = "Đã có lỗi xảy ra", Success = false });
            }
        }

        [HttpPost]
        public JsonResult Update(int patternId, string name, int[] entities, bool matchBegin, bool matchEnd)
        {
            try
            {
                PatternService service = new PatternService();
                service.Update(patternId, name, matchBegin, matchEnd, entities);
                return Json(new ResponseMessage() { Message = "Đã tạo thành công", Success = true });
            }
            catch (Exception)
            {
                return Json(new ResponseMessage() { Message = "Đã có lỗi xảy ra", Success = false });
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            PatternService patternService = new PatternService();
            patternService.Delete(id);
            return Json(new ResponseMessage() { Message = "Đã tạo thành công", Success = true });
        }
    }
}