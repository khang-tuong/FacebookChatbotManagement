using FacebookChatbotManagement.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookChatbotManagement.Models.Services
{
    public class PatternEntityMappingService : Service<PatternEntityMapping>
    {
        public PatternEntityMappingService() : base()
        {
        }

        public List<PatternEntityMapping> GetActive()
        {
            return this.Get(q => q.Active == true).ToList();
        }

        public void Delete(int id)
        {
            var pe = this.FirstOrDefault(q => q.Id == id);
            if (pe != null)
            {
                pe.Active = false;
                this.DbSet.SaveChanges();
            }
        }

    }
}