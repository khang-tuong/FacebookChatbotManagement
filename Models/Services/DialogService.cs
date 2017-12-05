using FacebookChatbotManagement.Models.Entities;
using FacebookChatbotManagement.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookChatbotManagement.Models.Services
{
    public class DialogService : Service<Dialog>
    {
        public DialogService() : base() { }

        public List<DialogViewModel> GetAll()
        {
            return this.Get(q => q.Active == true)
                .Select(q => new DialogViewModel() {
                    Id = q.Id,
                    Name = q.Name
                }).ToList();
        }

        public DialogEditViewModel GetEditViewModel(int dialogId)
        {
            Dialog dialog = this.FirstOrDefault(q => q.Id == dialogId && q.Active == true);
            DialogEditViewModel model = new DialogEditViewModel();
            IntentService intentService = new IntentService();
            if (dialog != null)
            {
                model.Id = dialog.Id;
                model.Name = dialog.Name;
                model.Steps = dialog.Intents.Select(q => q.Step.Value).ToList();
                model.Exceptions = dialog.Intents.Select(q => q.Exception.Value).ToList();
                model.AllIntents = intentService.GetAllForEditingDialog(dialogId);
                model.SelectedIntents = model.AllIntents.Where(z => z.DialogId == dialogId).Select(z => new IntentViewModel() { Id = z.Id, Name = z.Name }).ToList();
            }
            return model;
        }  

        public void Create(DialogCreateViewModel model)
        {
            try
            {
                Dialog dialog = this.Add(new Dialog()
                {
                    Active = true,
                    Name = model.Name
                });

                IntentService intentService = new IntentService();

                foreach (var intentId in model.IntentIds)
                {
                    Intent intent = intentService.FirstOrDefault(q => q.Id == intentId);
                    intent.DialogId = dialog.Id;
                    intent.Active = true;
                }

                intentService.SaveChanges();

                this.DbSet.SaveChanges();
            }
            catch (Exception e)
            {

                throw e;
            }
            
        }

        public void Update(SimpleDialogEditViewModel model)
        {
            Dialog dialog = this.FirstOrDefault(q => q.Id == model.Id && q.Active == true);
            if (dialog != null)
            {
                try
                {
                    dialog.Name = model.Name;
                    this.DbSet.SaveChanges();

                    IntentService intentService = new IntentService();
                    List<Intent> intents = intentService.Get(q => q.DialogId == model.Id).ToList();
                    foreach (var intent in intents)
                    {
                        intent.Active = false;
                        intent.DialogId = null;
                    }

                    if (model.IntentIds != null)
                    {
                        for (var i = 0; i < model.IntentIds.Length; ++i)
                        {
                            Intent intent = intentService.FirstOrDefault(q => q.Id == model.IntentIds[i]);
                            intent.DialogId = model.Id;
                            intent.Step = model.Steps[i];
                            intent.Exception = model.Exceptions[i];
                            intent.Active = true;
                        }
                    } 

                    intentService.SaveChanges();

                }
                catch (Exception e)
                {

                    throw e;
                }
                
            }
        }

        public void Delete(int dialogId)
        {
            var dialog = this.FirstOrDefault(q => q.Id == dialogId);
            if (dialog != null)
            {
                dialog.Active = false;
                this.SaveChanges();
            }
        }
    }
}