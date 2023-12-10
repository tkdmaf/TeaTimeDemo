using Microsoft.AspNetCore.Mvc;
using TeaTimeDemo.DataAccess.Repository.IRepository;
using TeaTimeDemo.Models;

namespace TeaTimeDemo.Areas.Api
{
    [Area("Api")]
    public class ApiProductController : Controller
    {
        private readonly IUnitOfWork _work;
        public ApiProductController(IUnitOfWork work) { 
            _work = work;
        }

        [HttpGet]
        [Route("api/v1/product/list")]
        public IActionResult GetAll()
        {
            List<Product> productList = _work.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = productList });
        }
    }
}
