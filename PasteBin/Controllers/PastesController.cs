using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using PasteBin.Models;
using PasteBin.Data.Repositories.Pastes;
using PasteBin.Data.Repositories.Languages;

namespace PasteBin.Controllers
{
    public class PastesController : Controller
    {
        private readonly ILanguageRepository languageRepository;
        private readonly IPasteRepository pasteRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public PastesController(ILanguageRepository languageRepository,
            IPasteRepository pasteRepository, UserManager<ApplicationUser> userManager)
        {
            this.languageRepository = languageRepository;
            this.pasteRepository = pasteRepository;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return this.View();
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var paste = this.pasteRepository.Find(x => x.Id == id);

            return this.View(paste);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            this.ViewData["Languages"] = await this.languageRepository.All().ToListAsync();

            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Paste model)
        {
            if (model != null && this.ModelState.IsValid)
            {
                var user = await this.userManager.GetUserAsync(User);
                var language = this.languageRepository.Find(x => x.Id == model.LanguageId);

                var paste = new Paste
                {
                    Title = model.Title,
                    Content = model.Content,
                    Language = language,
                    User = user
                };

                this.pasteRepository.Add(paste);

                return this.RedirectToAction("Details", new
                {
                    id = paste.Id
                });
            }

            this.ViewData["Languages"] = await this.languageRepository.All().ToListAsync();

            return this.View(model);
        }
    }
}