using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookChatbotManagement.Models.ViewModels
{
    public class PatternViewModel
    {
        public int Id { get; set; }
        public bool MatchBegin { get; set; }
        public bool MatchEnd { get; set; }
        public string Regex { get; set; }
        public string Name { get; set; }
    }
}