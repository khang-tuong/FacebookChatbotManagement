using FacebookChatbotManagement.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookChatbotManagement.Models.ViewModels
{
    public class PatternEditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool MatchBegin { get; set; }
        public bool MatchEnd { get; set; }
        public string Regex { get; set; }
        public List<Entity> SelectedEntities { get; set; }
        public List<Entity> AllEntities { get; set; }

        public PatternEditViewModel()
        {
            this.SelectedEntities = new List<Entity>();
            this.AllEntities = new List<Entity>();
        }
    }
}