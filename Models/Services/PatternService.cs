using FacebookChatbotManagement.Models.Entities;
using FacebookChatbotManagement.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookChatbotManagement.Models.Services
{
    public class PatternService : Service<Pattern>
    {
        public PatternService() : base()
        {
        }


        public void Delete(int id)
        {
            var pattern = this.FirstOrDefault(q => q.Id == id);
            pattern.Active = false;
            this.SaveChanges();
        }

        public List<PatternViewModel> GetAll()
        {
            var patterns = this.DbSet.Patterns
                .Where(q => q.Active == true)
                .ToList()
                .Select(q => new PatternViewModel()
                {
                    MatchBegin = q.MatchBegin,
                    Id = q.Id,
                    MatchEnd = q.MatchEnd,
                    Name = q.Name,
                    Regex = string.Join(" - ", q.PatternEntityMappings.Where(z => z.Active == true).OrderBy(z => z.Position).Select(z => z.Entity.Name).ToArray()),
                    IntentId = q.IntentId ?? 0,
                    Group = q.Group ?? 0
                })
                .ToList();
            return patterns;
        }

        public List<PatternViewModel> GetAllForCreating()
        {
            var patterns = this.DbSet.Patterns
                .Where(q => q.Active == true && !q.IntentId.HasValue)
                .ToList()
                .Select(q => new PatternViewModel()
                {
                    MatchBegin = q.MatchBegin,
                    Id = q.Id,
                    MatchEnd = q.MatchEnd,
                    Name = q.Name,
                    Regex = string.Join(" - ", q.PatternEntityMappings.Where(z => z.Active == true).OrderBy(z => z.Position).Select(z => z.Entity.Name).ToArray()),
                    IntentId = q.IntentId ?? 0,
                    Group = q.Group ?? 0
                })
                .ToList();
            return patterns;
        }

        public List<PatternViewModel> GetAllForEditingIntent(int intentId)
        {
            var patterns = this.DbSet.Patterns
                .Where(q => (q.Active == true && (!q.IntentId.HasValue || q.IntentId.Value == intentId)) || q.Active == false)
                .ToList()
                .Select(q => new PatternViewModel()
                {
                    MatchBegin = q.MatchBegin,
                    Id = q.Id,
                    MatchEnd = q.MatchEnd,
                    Name = q.Name,
                    Regex = string.Join(" - ", q.PatternEntityMappings.Where(z => z.Active == true).OrderBy(z => z.Position).Select(z => z.Entity.Name).ToArray()),
                    IntentId = q.IntentId ?? 0,
                    Group = q.Group ?? 0
                })
                .ToList();
            return patterns;
        }

        public PatternEditViewModel GetEditViewModel(int patternId)
        {
            EntityService entityService = new EntityService();

            Pattern pattern = this.FirstOrDefault(q => q.Id == patternId);
            List<Entity> entities = entityService.GetAll();
            PatternEditViewModel model = new PatternEditViewModel();

            model.AllEntities = entities;
            model.SelectedEntities = pattern.PatternEntityMappings
                .Where(q => q.Active == true)
                .OrderBy(q => q.Position)
                .Select(q => new Entity()
                {
                    Id = q.EntityId.Value,
                    Name = q.Entity.Name,
                    Words = q.Entity.Words
                })
                .ToList();

            model.Id = pattern.Id;
            model.MatchBegin = pattern.MatchBegin;
            model.MatchEnd = pattern.MatchEnd;
            model.Name = pattern.Name;

            return model;
        }

        public void Update(int id, string name, bool matchBegin, bool matchEnd, int[] entities)
        {
            try
            {
                Pattern pattern = this.FirstOrDefault(q => q.Id == id);
                if (pattern != null)
                {
                    pattern.MatchBegin = matchBegin;
                    pattern.MatchEnd = matchEnd;
                    pattern.Name = name;
                }
                this.SaveChanges();
                PatternEntityMappingService pemService = new PatternEntityMappingService();
                IEnumerable<PatternEntityMapping> patternEntityMapping = pemService.Get(q => q.PatternId == id);
                foreach (var item in patternEntityMapping)
                {
                    item.Active = false;
                }
                int i = 0;
                foreach (var en in entities)
                {
                    var pem = patternEntityMapping.FirstOrDefault(q => q.EntityId == en);
                    if (pem != null)
                    {
                        pem.Active = true;
                        pem.Position = i++;
                    } else
                    {
                        pemService.Add(new PatternEntityMapping()
                        {
                            Active = true,
                            EntityId = en,
                            PatternId = id,
                            Position = i++,
                        });
                    }
                }
                pemService.SaveChanges();

            }
            catch ( Exception e)
            {

                throw e;
            }
        }

        public List<string> GetFullPatterns()
        {
            List<Pattern> patterns = this.Get(q => q.Active == true).ToList();
            List<string> result = new List<string>();
            foreach (var pattern in patterns)
            {
                List<string> newPatterns = new List<string>();
                List<string> oldPatterns = new List<string>();
                var pems = pattern.PatternEntityMappings.ToList();
                for (var i = 0; i < pems.Count; i++)
                {
                    string[] words = pems[i].Entity.Words.Split('|');
                    for (var j = 0; j < words.Length; j++)
                    {
                        if (oldPatterns.Count == 0)
                        {
                            newPatterns.Add(words[j]);
                        }
                        else
                        {
                            for (var k = 0; k < oldPatterns.Count; k++)
                            {
                                newPatterns.Add(oldPatterns[k] + " " + words[j]);
                            }
                        }
                    }
                    oldPatterns = newPatterns;
                    newPatterns = new List<string>();
                }

                result.AddRange(oldPatterns);
            }
            return result;
        }
    }
}