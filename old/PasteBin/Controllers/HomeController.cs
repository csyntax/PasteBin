using Microsoft.AspNetCore.Mvc;

namespace PasteBin.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Error()
        {
            return this.View();
        }
    }
}