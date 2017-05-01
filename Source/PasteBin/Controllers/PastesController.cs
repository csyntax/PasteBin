using Microsoft.AspNetCore.Mvc;
using PasteBin.Data.Repositories.Pastes;
using PasteBin.Data.Repositories.Languages;
using Microsoft.AspNetCore.Identity;
using PasteBin.Models;

namespace PasteBin.Controllers
{
    public class PastesController : Controller
    {
        private readonly IPasteRepository pasteRepository;
        private readonly ILanguageRepository languageRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public PastesController(IPasteRepository pasteRepository, ILanguageRepository languageRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.pasteRepository = pasteRepository;
            this.languageRepository = languageRepository;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}