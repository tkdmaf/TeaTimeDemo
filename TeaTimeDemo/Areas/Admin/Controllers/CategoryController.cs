using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeaTimeDemo.DataAccess.Data;
using TeaTimeDemo.DataAccess.Repository;
using TeaTimeDemo.DataAccess.Repository.IRepository;
using TeaTimeDemo.Models;
using TeaTimeDemo.Utility;

namespace TeaTimeDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _work;
        public CategoryController(IUnitOfWork work)
        {
            _work = work;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _work.Category.GetAll().ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "類別名稱不能跟顯示順序一致。");
            }
            if (ModelState.IsValid)
            {
                _work.Category.Add(obj);
                _work.Save();
                TempData["success"] = "類別新增成功";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id != null && id != 0 && _work.Category.Get(u => u.Id == id) is var obj && obj != null)
            {
                return View(obj);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _work.Category.Update(category);
                _work.Save();
                TempData["success"] = "修改類別成功";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id != null && id != 0 && _work.Category.Get(u => u.Id == id) is var obj && obj != null)
            {
                return View(obj);
            }
            return NotFound();
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            if (id != null && id != 0 && _work.Category.Get(u => u.Id == id) is var obj && obj != null)
            {
                _work.Category.Remove(obj);
                _work.Save();
                TempData["success"] = "刪除類別成功";
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }

}
