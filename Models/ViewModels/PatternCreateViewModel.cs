using FacebookChatbotManagement.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookChatbotManagement.Models.ViewModels
{
    public class PatternCreateViewModel
    {
        public Pattern Pattern { get; set; }
        public List<Entity> Entities { get; set; }
    }
}