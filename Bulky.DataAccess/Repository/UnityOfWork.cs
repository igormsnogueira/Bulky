using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class UnityOfWork : IUnityOfWork
    {
        private ApplicationDbContext _db;
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }

        public ICompanyRepository Company { get; private set; }

        public UnityOfWork(ApplicationDbContext db) { //setup a constructor to receive the db context and instantiate all entity repositories
            _db = db; //holding the db context injected
            //instantiate all your entity repositories providing the db context as parameter, and set them on their correspondent attributes,
            Category = new CategoryRepository(_db);
            Product = new ProductRepository(_db);
            Company = new CompanyRepository(_db);
        }
        public void Save() //method to be called when you want to save/commit/apply all queries to the db at once.
        {
            _db.SaveChanges();
        }
    }
}
