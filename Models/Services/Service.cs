using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FacebookChatbotManagement.Models.Entities;

namespace FacebookChatbotManagement.Models.Services
{
    public interface IService<TEntity>
    {

    }

    public class Service<TEntity> where TEntity : class
    {
        protected ChatbotEntities DbSet = null;
        public Service()
        {
            this.DbSet = new ChatbotEntities();
        }

        public TEntity Add(TEntity entity)
        {
            entity = this.DbSet.Set<TEntity>().Add(entity);
            this.DbSet.SaveChanges();
            return entity;
        }

        public void Update(TEntity entity)
        {
            this.DbSet.SaveChanges();
        }

        public IEnumerable<TEntity> Get(Func<TEntity, bool> predicate)
        {
            return this.DbSet.Set<TEntity>().Where(predicate);
        }

        public TEntity FirstOrDefault(Func<TEntity, bool> predicate)
        {
            return this.DbSet.Set<TEntity>().FirstOrDefault(predicate);
        }

        public void SaveChanges()
        {
            this.DbSet.SaveChanges();
        }
    }
}