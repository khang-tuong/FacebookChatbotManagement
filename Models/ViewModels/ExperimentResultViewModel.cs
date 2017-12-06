using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookChatbotManagement.Models.ViewModels
{
    public class ExperimentResultViewModel
    {
        public int IntentId { get; set; }
        public double Group { get; set; }
        public Dictionary<string, string> Matches { get; set; }
    }
}