using BulkyWebRazor_Temp.Data;
using BulkyWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor_Temp.Pages.Categories
{
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public Category? CurrentCategory { get; set; }
        public EditModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult OnGet(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            CurrentCategory = _db.Categories.Find(id);

            if(CurrentCategory == null)
            {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPost() {
            if (ModelState.IsValid && CurrentCategory != null)
            {
                _db.Categories.Update(CurrentCategory);
                _db.SaveChanges();
                TempData["success"] = "Category edited successfully!";
                return RedirectToPage("Index");
            }

            return Page();
        }
    }
}
