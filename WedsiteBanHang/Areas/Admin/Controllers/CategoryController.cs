using Microsoft.AspNetCore.Mvc;
using WedsiteBanHang.Models;

namespace WedsiteBanHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        // GET: Admin/Category
        public IActionResult Index()
        {
            return View();
        }

        // GET: Admin/Category/Display/5
        public IActionResult Display(int id)
        {
            return View();
        }

        // GET: Admin/Category/Add
        public IActionResult Add()
        {
            return View();
        }

        // POST: Admin/Category/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Category category)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Admin/Category/Update/5
        public IActionResult Update(int id)
        {
            return View();
        }

        // POST: Admin/Category/Update/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, Category category)
        {
            if (id != category.Id) return NotFound();

            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Admin/Category/Delete/5
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            return RedirectToAction(nameof(Index));
        }
    }
}