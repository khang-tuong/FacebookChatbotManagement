using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookChatbotManagement.Models.ViewModels
{
    public class IntentRedisViewModel
    {
        public int Id { get; set; }
        public List<PatternRedisViewModel> Patterns { get; set; }
        public int DialogId { get; set; }
        public int Step { get; set; }
        public int Exception { get; set; }
    }

    public class PatternRedisViewModel
    {
        public int Id { get; set; }
        public List<EntityRedisViewModel> Entities { get; set; }
        public bool MatchBegin { get; set; }
        public bool MatchEnd { get; set; }
        public int Group { get; set; }
    }

    public class EntityRedisViewModel
    {
        public int Id { get; set; }
        public string Words { get; set; }
    }
}