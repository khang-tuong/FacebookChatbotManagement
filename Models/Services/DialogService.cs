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
            DialogIntentMappingService dialogIntentMappingService = new DialogIntentMappingService();
            Dialog dialog = this.FirstOrDefault(q => q.Id == dialogId && q.Active == true);
            DialogEditViewModel model = new DialogEditViewModel();
            if (dialog != null)
            {
                model.Id = dialog.Id;
                model.Name = dialog.Name;
                model.Steps = dialogIntentMappingService.Get(q => q.DialogId == dialog.Id).Select(q => q.Step).ToList();
                model.Exceptions = dialogIntentMappingService.Get(q => q.DialogId == dialog.Id).Select(q => q.Exception).ToList();
                model.AllIntents = new IntentService().GetAll();
                model.SelectedIntents = dialog.DialogIntentMappings.Select(z => new IntentViewModel() { Id = z.IntentId, Name = z.Intent.Name }).ToList();
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

                DialogIntentMappingService dialogIntentMappingService = new DialogIntentMappingService();
                for (int i = 0; i < model.IntentIds.Length; i++)
                {
                    dialogIntentMappingService.Add(new DialogIntentMapping()
                    {
                        Active = true,
                        DialogId = dialog.Id,
                        Exception = model.Exceptions[i],
                        Step = model.Steps[i],
                        IntentId = model.IntentIds[i]
                    });
                }

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

                    DialogIntentMappingService dimService = new DialogIntentMappingService();
                    IEnumerable<DialogIntentMapping> dialogIntentMapping = dimService.Get(q => q.DialogId == model.Id);
                    foreach (var item in dialogIntentMapping)
                    {
                        item.Active = false;
                    }

                    for (int i = 0; i < model.IntentIds.Length; ++i)
                    {
                        var pem = dialogIntentMapping.FirstOrDefault(q => q.IntentId == model.IntentIds[i]);
                        if (pem != null)
                        {
                            pem.Active = true;
                            pem.Step = model.Steps[i];
                            pem.Exception = model.Exceptions[i];
                        }
                        else
                        {
                            dimService.Add(new DialogIntentMapping()
                            {
                                Active = true,
                                DialogId = model.Id,
                                Exception = model.Exceptions[i],
                                IntentId = model.IntentIds[i],
                                Step = model.Steps[i],
                            });
                        }
                    }
                    dimService.SaveChanges();
                }
                catch (Exception e)
                {

                    throw e;
                }
                
            }
        }
    }
}