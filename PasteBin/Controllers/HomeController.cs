using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PasteBin.Data.Repositories.Languages;

namespace PasteBin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILanguageRepository languageRepository;

        public HomeController(ILanguageRepository languageRepository)
        {
            this.languageRepository = languageRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            this.ViewData["Languages"] = await this.languageRepository.GetAllAsync();

            return this.View();
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }
    }
}
