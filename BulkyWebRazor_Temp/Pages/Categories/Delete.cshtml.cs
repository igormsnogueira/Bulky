using BulkyWebRazor_Temp.Data;
using BulkyWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor_Temp.Pages.Categories
{
    [BindProperties]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Category? DeletedCategory { get; set; }
        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult OnGet(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            DeletedCategory = _db.Categories.Find(id);

            if (DeletedCategory == null)
            {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            Category? obj =  _db.Categories.Find(DeletedCategory.Id);
            if (obj != null)
            {
                _db.Categories.Remove(obj);
                _db.SaveChanges();
                TempData["error"] = "Category deleted successfully!";
                return RedirectToPage("Index");
            }

            return NotFound();
        }
    }
}
