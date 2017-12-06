using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookChatbotManagement.Models.ViewModels
{
    public class DialogCreateViewModel
    {
        public string Name { get; set; }
        public int[] IntentIds { get; set; }
        public double[] Steps { get; set; }
        public double[] Exceptions { get; set; }
    }
}