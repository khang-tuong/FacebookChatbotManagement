using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookChatbotManagement.Models.ViewModels
{
    public class IntentCreateViewModel
    {
        public string Name { get; set; }
        public List<PatternViewModel> Patterns { get; set; }

        public IntentCreateViewModel()
        {
            this.Patterns = new List<PatternViewModel>();
        }
    }
}