using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IUnityOfWork
    {
        //set one attribute/variable for each entity/model repository you have. Ex: CategoryRepository, ProductRepository, etc.
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        ICompanyRepository Company { get; }
        void Save(); //method to commit/save/apply all db operations to the database
    }
}
