using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TeaTimeDemo.DataAccess.Data;
using TeaTimeDemo.DataAccess.Repository;
using TeaTimeDemo.DataAccess.Repository.IRepository;
using TeaTimeDemo.Models;
using TeaTimeDemo.Models.ViewModels;

namespace TeaTimeDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _work;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork work, IWebHostEnvironment webHostEnvironment)
        {
            _work = work;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objList = _work.Product.GetAll(includeProperties: "Category").ToList();
            return View(objList);
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                CategoryList = _work.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                return View(productVM);
            }
            productVM.Product = _work.Product.Get(u => u.Id == id);
            return View(productVM);
         
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM vm, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string rootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(rootPath, @"images\product");
                    if (!string.IsNullOrEmpty(vm.Product.ImageUrl))
                    {
                        // 有新圖片上傳，刪除舊圖片
                        var oldImagePath = Path.Combine(rootPath, vm.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath)) 
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var filestream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    vm.Product.ImageUrl = @"\images\product\" + filename;
                }
                if (vm.Product.Id == 0)
                {
                    _work.Product.Add(vm.Product);
                }
                else
                {
                    _work.Product.Update(vm.Product);
                }
                _work.Save();
                TempData["success"] = "產品新增成功";
                return RedirectToAction("Index");
            }
            else
            {
                vm.CategoryList = _work.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(vm);
            }
            
        }

        //public IActionResult Delete(int? id)
        //{
        //    if (id != null && id != 0 && _work.Product.Get(u => u.Id == id) is var obj && obj != null)
        //    {
        //        return View(obj);
        //    }
        //    return NotFound();
        //}

        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeletePost(int? id)
        //{
        //    if (id != null && id != 0 && _work.Product.Get(u => u.Id == id) is var obj && obj != null)
        //    {
        //        _work.Product.Remove(obj);
        //        _work.Save();
        //        TempData["success"] = "刪除類別成功";
        //        return RedirectToAction("Index");
        //    }
        //    return NotFound();
        //}

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> productList = _work.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = productList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _work.Product.Get(u => u.Id == id);
            if (productToBeDeleted == null) {
                return Json(new { success = false, message = "刪除失敗" });
            }
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _work.Product.Remove(productToBeDeleted);
            _work.Save();
            return Json(new { success = true, message = "刪除成功" });
        }
        #endregion

    }

}
