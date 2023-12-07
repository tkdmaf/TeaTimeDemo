using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TeaTimeDemo.DataAccess.Repository.IRepository;
using TeaTimeDemo.Models;

namespace TeaTimeDemo.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _work;
        public HomeController(ILogger<HomeController> logger, IUnitOfWork work)
        {
            _logger = logger;
            _work = work;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _work.Product.GetAll(includeProperties: "Category");
            return View(productList);
        }

        public ActionResult Details(int productId) {
            Product product = _work.Product.Get(u => u.Id == productId, includeProperties: "Category");
            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
