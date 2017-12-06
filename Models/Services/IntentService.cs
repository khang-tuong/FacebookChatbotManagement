using FacebookChatbotManagement.Models.Entities;
using FacebookChatbotManagement.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookChatbotManagement.Models.Services
{
    public class IntentService : Service<Intent>
    {
        public IntentService() : base()
        {

        }

        public List<IntentViewModel> GetAll()
        {
            return this.Get(q => q.Active == true)
                .Select(q => new IntentViewModel() {
                    Id = q.Id,
                    Name = q.Name,
                }).ToList();
        }

        public List<IntentViewModel> GetAllForCreatingDialog()
        {
            return this.Get(q => (q.Active == true && !q.DialogId.HasValue) || q.Active == false)
                .Select(q => new IntentViewModel()
                {
                    Id = q.Id,
                    Name = q.Name,
                    DialogId = 0,
                    Exception = 0,
                    Step = 0,
                }).ToList();
        }

        public List<IntentViewModel> GetAllForEditingDialog(int dialogId)
        {
            return this.Get(q => (q.Active == true && (!q.DialogId.HasValue || q.DialogId == dialogId)) || q.Active == false)
                .Select(q => new IntentViewModel()
                {
                    Id = q.Id,
                    Name = q.Name,
                    DialogId = q.DialogId ?? 0,
                    Exception = q.Exception ?? 0,
                    Step = q.Step ?? 0,
                }).ToList();
        }


        public Intent Create(string name, int[] patterns, int[] groups)
        {
            Intent intent = this.Add(new Intent()
            {
                Active = true,
                Name = name,
            });

            PatternService patternService = new PatternService();

            int i = 0;
            foreach (var patternId in patterns)
            {
                var pattern = patternService.FirstOrDefault(q => q.Id == patternId && q.Active == true);
                pattern.Group = groups[i];
                pattern.IntentId = intent.Id;
                ++i;
            }
            patternService.SaveChanges();

            this.SaveChanges();
            return intent;
        }

        public IntentEditViewModel GetEditViewModel(int intentId)
        {
            Intent intent = this.FirstOrDefault(q => q.Id == intentId && q.Active == true);
            if (intent != null)
            {
                IntentEditViewModel model = new IntentEditViewModel();
                model.Id = intent.Id;
                model.Name = intent.Name;
                model.AllPatterns = new PatternService().GetAllForEditingIntent(intentId);
                foreach (var pattern in model.AllPatterns)
                {
                    if (pattern.IntentId == intent.Id)
                        model.SelectedPatterns.Add(pattern.Id, pattern.Group);
                }
                return model;
            }
            return null;
        }

        public void Update(int id, string name, int[] patterns, int[] groups)
        {
            try
            {
                Intent intent = this.FirstOrDefault(q => q.Id == id);
                if (intent != null)
                {
                    intent.Name = name;
                }
                this.DbSet.SaveChanges();

                PatternService patternService = new PatternService();
                List<Pattern> ps = patternService.Get(q => q.IntentId == id).ToList();
                foreach (var p in ps)
                {
                    p.Active = false;
                    p.IntentId = null;
                }

                if (patterns != null)
                {
                    for (var i =0; i < patterns.Length; ++i)
                    {
                        Pattern pattern = patternService.FirstOrDefault(q => q.Id == patterns[i]);
                        pattern.IntentId = id;
                        pattern.Group = groups[i];
                        pattern.Active = true;
                    }
                }


                patternService.SaveChanges();
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public List<IntentRedisViewModel> GetAllForRedis()
        {
            var intents = this.Get(q => q.Active == true)
                .ToList()
                .Select(q => new IntentRedisViewModel()
                {
                    Id = q.Id,
                    DialogId = q.DialogId ?? 0,
                    Exception = q.Exception ?? -1,
                    Step = q.Step ?? -1,
                    Patterns = q.Patterns.Where(z => z.Active == true).Select(z => new PatternRedisViewModel() {
                        Id = z.Id,
                        MatchBegin = z.MatchBegin,
                        MatchEnd = z.MatchEnd,
                        Group = z.Group ?? 0,
                        Entities = z.PatternEntityMappings.Where(x => x.Active == true).OrderBy(x => x.Position).Select(x => new EntityRedisViewModel() {
                            Id = x.EntityId.Value,
                            Words = x.Entity.Words
                        }).ToList(),
                    }).ToList()
                })
                .ToList();


            return intents;
        }

        public void Delete(int intentId)
        {
            var intent = this.FirstOrDefault(q => q.Id == intentId);
            if (intent != null) {
                intent.Active = false;
                this.SaveChanges();
            }
        }
    }
}