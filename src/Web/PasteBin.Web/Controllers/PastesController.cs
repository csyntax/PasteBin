namespace PasteBin.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Microsoft.EntityFrameworkCore;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.Rendering;

    using Microsoft.Extensions.Caching.Memory;

    using PasteBin.Data.Models;

    using PasteBin.Services.Data.Pastes;
    using PasteBin.Services.Data.Languages;
    using PasteBin.Services.Web.Mapping;

    using PasteBin.Web.Infrastructure.InputModels;
    using PasteBin.Web.Infrastructure.ViewModels.Pastes;
    

    [Authorize]
    public class PastesController : Controller
    {
        private readonly IPasteService pastes;
        private readonly ILanguageService languages;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMemoryCache cache;
        private readonly IMappingService mapping;

        private const int MinutesBetweenPastes = 10;

        public PastesController(
            IPasteService pastes,
            ILanguageService languages,
            UserManager<ApplicationUser> userManager,
            IMappingService mapping,
            IMemoryCache cache)
        {
            this.pastes = pastes;
            this.languages = languages;
            this.mapping = mapping;
            this.userManager = userManager;
            this.cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var pastes = this.pastes.GetAll().AsNoTracking();
            var model = await this.mapping.Map<PasteViewModel>(pastes).ToListAsync();

            return this.View(model);
        }

       [HttpGet]
       [AllowAnonymous]
        public IActionResult View(int id)
        {
            var paste = this.pastes.GetAll().Where(x => x.Id == id);
            var model = this.mapping.Map<PasteViewModel>(paste).FirstOrDefault();

            return this.View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Embedded(int id)
        {
            var paste = this.pastes.GetAll().Where(x => x.Id == id);
            var model = this.mapping.Map<PasteEmbeddedViewModel>(paste).FirstOrDefault();

            return this.View(model);
        }
        
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new PasteInputModel();

            model.Languages = await this.languages.Get()
                          .Select(lang => new SelectListItem()
                          {
                              Value = lang.Id.ToString(),
                              Text = lang.Name
                          })
                          .ToListAsync();

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PasteInputModel model)
        {
            model.Languages = await this.languages.Get()
                         .Select(lang => new SelectListItem()
                         {
                             Value = lang.Id.ToString(),
                             Text = lang.Name
                         })
                         .ToListAsync();

            if (this.IsUserCommitedPasteInLastMinute())
            {
                this.ViewData["Message"] = $"You can commit every {MinutesBetweenPastes} minutes";

                return this.View(model);
            }

            if (model != null && this.ModelState.IsValid)
            {
                var user = await this.userManager.GetUserAsync(this.User);
                var language = await this.languages.Get(model.Language);

                var paste = new Paste
                {
                    Title = model.Title,
                    Content = model.Content,
                    Language = language,
                    User = user
                };

                await this.pastes.Add(paste);

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
            var lastCommit = this.pastes
                .GetAll()
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedOn)
                .FirstOrDefault();

            if (lastCommit == null)
            {
                return false;
            }

            return lastCommit.CreatedOn.AddMinutes(MinutesBetweenPastes) >= DateTime.Now;
        }

        private void GetLanguages()
        {
            if (!this.cache.TryGetValue("languages", out List<LanguageViewModel> cacheEntry))
            {
                var languages = this.languages.Get();

                cacheEntry = this.mapping.Map<LanguageViewModel>(languages).ToList();

                this.cache.Set("languages", cacheEntry);
            }

            this.ViewData["Languages"] = cacheEntry;
        }
    }
}