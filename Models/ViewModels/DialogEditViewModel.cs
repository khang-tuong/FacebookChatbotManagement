using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookChatbotManagement.Models.ViewModels
{
    public class DialogEditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<IntentViewModel> SelectedIntents { get; set; }
        public List<int> Steps { get; set; }
        public List<int> Exceptions { get; set; }
        public List<IntentViewModel> AllIntents { get; set; }

        public DialogEditViewModel()
        {
            this.SelectedIntents = new List<IntentViewModel>();
            this.Steps = new List<int>();
            this.Exceptions = new List<int>();
            this.AllIntents = new List<IntentViewModel>();
        }
    }

    public class SimpleDialogEditViewModel {
        public int Id { get; set; }
        public string Name { get; set; }
        public int[] IntentIds { get; set; }
        public int[] Steps { get; set; }
        public int[] Exceptions { get; set; }
    }
}