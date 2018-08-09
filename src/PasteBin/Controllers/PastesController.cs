namespace PasteBin.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Authorization;

    using PasteBin.Models;
    using PasteBin.ViewModels;
    using PasteBin.Extensions;
    using PasteBin.Data.Repositories;
    using PasteBin.ViewModels.Pastes;

    using Microsoft.Extensions.Caching.Memory;
    using System.Collections.Generic;

    [Authorize]
    public class PastesController : Controller
    {
        private readonly IEfRepository<Paste> pasteRepository;
        private readonly IEfRepository<Language> languageRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMemoryCache cache;

        private const int MinutesBetweenPastes = 10;

        public PastesController(
            IEfRepository<Paste> pasteRepository,
            IEfRepository<Language> languageRepository,
            UserManager<ApplicationUser> userManager,
            IMemoryCache cache)
        {
            this.pasteRepository = pasteRepository;
            this.languageRepository = languageRepository;
            this.userManager = userManager;
            this.cache = cache;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var pastes = this.pasteRepository
                .All()
                .OrderByDescending(p => p.Date)
                .To<PasteViewModel>()
                .ToList();

            return this.View(pastes);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult View(int id)
        {
            var paste = this.pasteRepository
                .All()
                .Where(p => p.Id == id)
                .To<PasteViewModel>()
                .FirstOrDefault();

            return this.View(paste);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Embedded(int id)
        {
            var paste = this.pasteRepository
                .All()
                .Where(p => p.Id == id)
                .To<PasteEmbeddedViewModel>()
                .FirstOrDefault();

            return this.View(paste);
        }

        [HttpGet]
        public IActionResult Create()
        {
            this.GetLanguages();

            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Paste model)
        {
            this.GetLanguages();

            if (this.IsUserCommitedPasteInLastMinute())
            {
                this.ViewData["Message"] = $"You can commit every {MinutesBetweenPastes} minutes";

                return this.View(model);
            }

            if (model != null && this.ModelState.IsValid)
            {
                var user = await this.userManager.GetUserAsync(User);
                var language = this.languageRepository.Get(model.LanguageId);

                var paste = new Paste
                {
                    Title = model.Title,
                    Content = model.Content,
                    Language = language,
                    User = user
                };

                this.pasteRepository.Add(paste);
                this.pasteRepository.SaveChanges();

                return this.RedirectToAction(nameof(this.View), new
                {
                    id = paste.Id
                });
            }

            return this.View(model);
        }

        private bool IsUserCommitedPasteInLastMinute()
        {
            var userId = this.userManager.GetUserId(User);
            var lastCommit = this.pasteRepository
                .All()
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.Date)
                .FirstOrDefault();

            if (lastCommit == null)
            {
                return false;
            }

            return lastCommit.Date.AddMinutes(MinutesBetweenPastes) >= DateTime.Now;
        }

        private void GetLanguages()
        {
            if (!this.cache.TryGetValue("languages", out List<LanguageViewModel> cacheEntry))
            {
                cacheEntry = this.languageRepository
                    .All()
                    .OrderBy(p => p.Name)
                    .To<LanguageViewModel>()
                    .ToList();

                this.cache.Set("languages", cacheEntry);
            }

            this.ViewData["Languages"] = cacheEntry;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.pasteRepository.Dispose();
                this.languageRepository.Dispose();
                this.userManager.Dispose();
            }
        }
    }
}