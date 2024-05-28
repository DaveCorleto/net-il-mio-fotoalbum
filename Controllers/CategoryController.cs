//using Microsoft.AspNetCore.Mvc;
//using net_il_mio_fotoalbum.Models;

//namespace net_il_mio_fotoalbum.Controllers
//{
//    public class CategoryController : Controller
//    {
//        public IActionResult Index()
//        {
//            return View();
//        }
//    }
//}

using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System.Linq;

namespace net_il_mio_fotoalbum.Controllers
{
    public class CategoryController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            List<Category> categories = Data.CategoryManager.GetAllCategories();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Show(int id)
        {
            Category category = Data.CategoryManager.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCategory (Category category)
        {
            if (ModelState.IsValid)
            {
                var categoryManager = new Data.CategoryManager();
                categoryManager.CategoryInsert(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }


        [HttpGet]
        public IActionResult UpdateCategory(int id)
        {
            Category category = Data.CategoryManager.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var categoryManager = new Data.CategoryManager();
                bool result = await categoryManager.CategoryUpdate(category);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return NotFound();
                }
            }
            return View(category);
        }

        public IActionResult Delete(int id)
        {
            Category category = Data.CategoryManager.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            bool result = Data.CategoryManager.CategoryDelete(id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return NotFound();
            }
        }
    }
}
