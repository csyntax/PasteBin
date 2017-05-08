using Microsoft.AspNetCore.Mvc;
using PasteBin.Data.Repositories;
using PasteBin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using PasteBin.Data.Repositories.Pastes;

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
        public IActionResult Details(int id)
        {
            var paste = this.pasteRepository.Find(id);

            return this.View(paste);
        }

        [HttpGet]
        public IActionResult Create()
        {
            this.ViewData["Languages"] = this.languageRepository.All();

            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Paste model)
        {
            if (model != null && this.ModelState.IsValid)
            {
                var user = this.userManager.GetUserAsync(User).Result;
                var language = this.languageRepository.Find(model.LanguageId);

                var paste = new Paste
                {
                    Title = model.Title,
                    Content = model.Content,
                    Language = language,
                    User = user
                };

                this.pasteRepository.Add(paste);
                this.pasteRepository.SaveChanges();

                return this.RedirectToAction("Details", new
                {
                    id = paste.Id
                });
            }

            this.ViewData["Languages"] = this.languageRepository.All();

            return this.View(model);
        }
    }
}