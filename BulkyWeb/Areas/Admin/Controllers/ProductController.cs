using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnityOfWork _unityOfWork; //variable to hold the _unityOfWork
        public ProductController(IUnityOfWork unityOfWork) //the Dependency Injection will provide an implementation of IUnityOfWork here
        {
            _unityOfWork = unityOfWork;
        }
        public IActionResult Index()
        {
            var productsList = _unityOfWork.Product.GetAll(); //using the method we setup in Product Repository to GetAll elements for this entity, but through the unity of work class
            return View(productsList);
        }

        public IActionResult Create()
        {
            IEnumerable<SelectListItem> categoryList = _unityOfWork.Category.GetAll().Select((p) =>
                new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                }
            );
            ProductVM productVM = new()
            {
                Product = new Product(), //passing an empty product with default values, so we can set them in the Create view
                CategoryList = categoryList
            };
            return View(productVM);
        }

        [HttpPost]
        public IActionResult Create(ProductVM productVM)
        {
            if (ModelState.IsValid)//if no errors
            {
                _unityOfWork.Product.Add(productVM.Product);
                _unityOfWork.Save();
                TempData["success"] = "Product created successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _unityOfWork.Category.GetAll().Select((p) =>
                        new SelectListItem
                        {
                            Text = p.Name,
                            Value = p.Id.ToString()
                        }
                );

                return View(productVM);
            }
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) //if no id provided or if it is 0 , we return the default NotFound page
            {
                return NotFound();
            }

            //Product? productFromDb = _db.Products.Find(id);
            Product? productFromDb = _unityOfWork.Product.Get(c => c.Id == id); //using the method we created to get an element, we need to pass the query logic to find the element as lambda expression as defined in the method implementation
            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)//if no errors
            {
                _unityOfWork.Product.UpdateProduct(obj);
                _unityOfWork.Save();
                TempData["success"] = "Product updated successfully!";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //Product? productFromDb = _db.Products.Find(id); //finding the object
            Product? productFromDb = _unityOfWork.Product.Get((c) => c.Id == id);
            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);//return the view providing the product found
        }


        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? productFromDb = _unityOfWork.Product.Get((c) => c.Id == id);
            if (productFromDb == null)
            {
                return NotFound();
            }

            _unityOfWork.Product.Remove(productFromDb);
            _unityOfWork.Save();
            TempData["success"] = "Product deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}
