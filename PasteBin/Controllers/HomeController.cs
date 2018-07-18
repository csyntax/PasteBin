namespace PasteBin.Controllers
{
    using System.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using PasteBin.ViewModels;

    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Error()
        {
            var model = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return this.View(model);
        }
    }
}
