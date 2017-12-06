using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookChatbotManagement.Models.ViewModels
{
    public class IntentEditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Dictionary<int, double> SelectedPatterns { get; set; }
        public List<PatternViewModel> AllPatterns { get; set; }

        public IntentEditViewModel()
        {
            this.SelectedPatterns = new Dictionary<int, double>();
            this.AllPatterns = new List<PatternViewModel>();
        }
    }
}