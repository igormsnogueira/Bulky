﻿using BulkyBook.DataAccess.Data;
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

        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null)//includeProperties will be a list of properties, split by comma ",", set in this model/entity class that are linked through a foreign key, so we can load this foreign entity in the same query together with the main entity. EX:Get All the Products, but includes for each the Category and Author associated with it. So here we would provided "Category,Author" the name must match the name of the property set in the model
        {
            IQueryable<T> query = dbSet; //we need to define it as queryable first before running any query functions, related to sql clauses like where() or ToList(), on it
            query = query.Where(filter); //apply the condition to it
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var prop in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                { //spliting the properties that are separated by , to a list of strings, and adding a filter to remove any empty strings, so we can iterate over them.
                    query = query.Include(prop); //Include also accepts a string with the name of the Property, instead of a lambda like obj=>obj.Prop
                }

            }
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(string? includeProperties = null) //includeProperties will be a list of properties, split by comma ",", set in this model/entity class that are linked through a foreign key, so we can load this foreign entity in the same query together with the main entity. EX:Get All the Products, but includes for each the Category and Author associated with it. So here we would provided "Category,Author" the name must match the name of the property set in the model
        {
            IQueryable<T> query = dbSet;
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach(var prop in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)){ //spliting the properties that are separated by , to a list of strings, and adding a filter to remove any empty strings, so we can iterate over them.
                    query = query.Include(prop); //Include also accepts a string with the name of the Property, instead of a lambda like obj=>obj.Prop
                }

            }
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
