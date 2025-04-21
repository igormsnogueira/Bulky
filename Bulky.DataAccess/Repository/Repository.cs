using BulkyBook.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Repository.IRepository; //include the namespace use in the interface file
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BulkyBook.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T: class //implementing the IRepository interface with any entity type (using T), but of course it needs to be a class
    {
        private readonly ApplicationDbContext _db; //holder for the db context
        internal DbSet<T> dbSet; //it will hold the entity/model defined as type when instantiating this class
        public Repository(ApplicationDbContext db) //injecting the application db context, so we can use it to perform the data operations
        {
            _db = db;
            this.dbSet = _db.Set<T>(); //this will set the dbSet to whatever type you used when instantiating this repository class. So instead of using _db.SomeEntity.Add(). We will be able to use dbSet.Add() directly
        }

        public void Add(T entity)
        {
            dbSet.Add(entity); //adding the entity to db and then saving
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet; //we need to define it as queryable first before running any query functions, related to sql clauses like where() or ToList(), on it
            query = query.Where(filter); //apply the condition to it
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = dbSet;
            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);//remove all elements from the list. This is a useful Entity Framework method
        }
    }
}
