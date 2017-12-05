using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookChatbotManagement.Models.ViewModels
{
    public class IntentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Step { get; set; }
        public int Exception { get; set; }
        public int DialogId { get; set; }
        public List<PatternViewModel> Patterns { get; set; }
    }
}