using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
