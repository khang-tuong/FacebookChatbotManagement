using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FacebookChatbotManagement.Models.Entities;

namespace FacebookChatbotManagement.Models.Services
{
    public interface IEntityService
    {

    }

    public class EntityService : Service<Entity>
    {
        public EntityService() : base()
        {
        }


        public void Delete(int id)
        {
            var entity = this.FirstOrDefault(q => q.Id == id);
            this.DbSet.Entities.Remove(entity);
            this.DbSet.SaveChanges();
        }

        public List<Entity> GetAll()
        {
            return this.DbSet.Entities.ToList();
        }

        public void Update(int id, string name, string words)
        {
            var e = this.FirstOrDefault(q => q.Id == id);
            if (e != null)
            {
                e.Name = name;
                e.Words = words;
            }
            this.SaveChanges();
        }
    }
}