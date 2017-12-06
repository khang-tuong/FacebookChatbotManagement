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
        public List<double> Steps { get; set; }
        public List<double> Exceptions { get; set; }
        public List<IntentViewModel> AllIntents { get; set; }

        public DialogEditViewModel()
        {
            this.SelectedIntents = new List<IntentViewModel>();
            this.Steps = new List<double>();
            this.Exceptions = new List<double>();
            this.AllIntents = new List<IntentViewModel>();
        }
    }

    public class SimpleDialogEditViewModel {
        public int Id { get; set; }
        public string Name { get; set; }
        public int[] IntentIds { get; set; }
        public double[] Steps { get; set; }
        public int[] Exceptions { get; set; }
    }
}