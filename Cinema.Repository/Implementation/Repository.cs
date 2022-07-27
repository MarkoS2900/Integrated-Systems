using Cinema.Domain.DomainModels;
using Cinema.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cinema.Repository.Implementation
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext context;
        private DbSet<T> entities;

        public Repository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }
        public IEnumerable<T> GetAll()
        {
            return entities.AsEnumerable();
        }

        public T Get(Guid? id)
        {
            return entities.SingleOrDefault(s => s.Id == id);
        }

        public void CheckIsEntityNull(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
        }

        public void Insert(T entity)
        {
            CheckIsEntityNull(entity);
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(T entity)
        {
            CheckIsEntityNull(entity);
            entities.Update(entity);
            context.SaveChanges();
        }

        public void Delete(T entity)
        {
            CheckIsEntityNull(entity);
            entities.Remove(entity);
            context.SaveChanges();
        }
    }
}
