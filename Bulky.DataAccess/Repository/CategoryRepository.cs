using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository //as we already have defined the logic for the common method required in the IRepository within Repository class, we can simply extend this class as well as the ICategoryRepository that requires these method plus the new ones specific for a Category entity
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db){ //as we are extending Repository, we need to pass the db to the constructor of this base class as well, because its controller requires it
            _db = db;
        }

        public void UpdateCategory(Category category)
        {
            _db.Categories.Update(category); //simply updating the category object with the whole new one passed  via parameter
        }
    }
}
