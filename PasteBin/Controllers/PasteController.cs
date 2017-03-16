using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PasteBin.Data.Repositories.Pastes;
using PasteBin.Data.Repositories.Languages;
using PasteBin.Models;


namespace PasteBin.Controllers
{
    public class PasteController : Controller
    {
        private readonly IPasteRepository pasteRepository;
        private readonly ILanguageRepository languageRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public PasteController(IPasteRepository pasteRepository, ILanguageRepository languageRepository, UserManager<ApplicationUser> userManager)
        {
            this.pasteRepository = pasteRepository;
            this.languageRepository = languageRepository;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return this.View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            this.ViewData["Languages"] = await this.languageRepository.GetAllAsync();

            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Paste model)
        {
            if (model != null && ModelState.IsValid)
            {
                var language = await this.languageRepository.FindOneAsync(m => m.Id == model.LanguageId);

                var paste = new Paste
                {
                    Content = model.Content,
                    CreatedOn = DateTime.Now,
                    UserId = this.userManager.GetUserId(User),
                    Language = language
                };

                await this.pasteRepository.AddAsync(paste);

                return this.RedirectToAction("Details", new { id = paste.Id });
            }

            this.ViewData["Languages"] = await this.languageRepository.GetAllAsync();

            return this.View("Create", model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var paste = await this.pasteRepository.FindOneAsync(p => p.Id == id);

            if (paste == null)
            {
                return new NotFoundResult();
            }

            return this.View(paste);
        }
    }
}