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


        public Intent Create(string name, int[] patterns, int[] groups)
        {
            Intent intent = this.Add(new Intent()
            {
                Active = true,
                Name = name,
            });

            IntentPatternMappingService service = new IntentPatternMappingService();
            for (int i = 0; i < patterns.Length; ++i)
            {
                service.Add(new IntentPatternMapping() {
                    Active = true,
                    IntentId = intent.Id,
                    PatternId = patterns[i],
                    Group = groups[i]
                });
            }

            this.DbSet.SaveChanges();
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
                model.AllPatterns = new PatternService().GetAll();
                foreach (var map in intent.IntentPatternMappings)
                {
                    if (map.Active)
                        model.SelectedPatterns.Add(map.PatternId, map.Group);
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

                IntentPatternMappingService ipmService = new IntentPatternMappingService();
                IEnumerable<IntentPatternMapping> intentPatternMapping = ipmService.Get(q => q.IntentId == id);
                foreach (var item in intentPatternMapping)
                {
                    item.Active = false;
                }

                for (int i = 0; i < patterns.Length; ++i)
                {
                    int p = patterns[i];
                    var pem = intentPatternMapping.FirstOrDefault(q => q.PatternId == p);
                    if (pem != null)
                    {
                        pem.Active = true;
                        pem.Group = groups[i];
                    }
                    else
                    {
                        ipmService.Add(new IntentPatternMapping()
                        {
                            Active = true,
                            PatternId = patterns[i],
                            Group = groups[i],
                            IntentId = id,
                        });
                    }
                }
                ipmService.SaveChanges();
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
                    DialogId = q.DialogIntentMappings.ElementAt(0).DialogId,
                    Exception = q.DialogIntentMappings.ElementAt(0).Exception,
                    Step = q.DialogIntentMappings.ElementAt(0).Step,
                    Patterns = q.IntentPatternMappings.Where(z => z.Active == true).Select(z => new PatternRedisViewModel() {
                        Id = z.PatternId,
                        MatchBegin = z.Pattern.MatchBegin,
                        MatchEnd = z.Pattern.MatchEnd,
                        Group = z.Group,
                        Entities = z.Pattern.PatternEntityMappings.Where(x => x.Active == true).OrderBy(x => x.Position).Select(x => new EntityRedisViewModel() {
                            Id = x.EntityId.Value,
                            Words = x.Entity.Words
                        }).ToList(),
                    }).ToList()
                })
                .ToList();


            return intents;
        }
    }
}