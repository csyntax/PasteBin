using Microsoft.AspNetCore.Mvc;
using PasteBin.Data.Repositories;
using PasteBin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using PasteBin.Data.Repositories.Pastes;
using System.Threading.Tasks;

namespace PasteBin.Controllers
{
    public class PastesController : Controller
    {
        private readonly IDbRepository<Language> languageRepository;
        private readonly IPasteRepository pasteRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public PastesController(IDbRepository<Language> languageRepository,
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
        public async Task<IActionResult> Details(int id)
        {
            var paste = await this.pasteRepository.FindOneAsync(x => x.Id == id);

            return this.View(paste);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            this.ViewData["Languages"] = await this.languageRepository.GetAllAsync();

            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Paste model)
        {
            if (model != null && this.ModelState.IsValid)
            {
                var user = await this.userManager.GetUserAsync(User);
                var language = await this.languageRepository.FindOneAsync(x => x.Id == model.LanguageId);

                var paste = new Paste
                {
                    Title = model.Title,
                    Content = model.Content,
                    Language = language,
                    User = user
                };

                await this.pasteRepository.AddAsync(paste);
                //this.pasteRepository.SaveChanges();

                return this.RedirectToAction("Details", new
                {
                    id = paste.Id
                });
            }

            this.ViewData["Languages"] = await this.languageRepository.GetAllAsync();

            return this.View(model);
        }
    }
}