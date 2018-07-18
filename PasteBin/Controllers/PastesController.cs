namespace PasteBin.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.EntityFrameworkCore;

    using PasteBin.Data.Repositories;
    using PasteBin.Models;
    using PasteBin.ViewModels.PasteViewModels;
    using PasteBin.Services.Mapping;

    [Authorize]
    public class PastesController : Controller
    {
        private readonly IEfRepository<Language> languageRepository;
        private readonly IEfRepository<Paste> pasteRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMappingService mappingService;

        public PastesController(
            IEfRepository<Language> languageRepository, 
            IEfRepository<Paste> pasteRepository,
            UserManager<ApplicationUser> userManager,
            IMappingService mappingService)
        {
            this.languageRepository = languageRepository;
            this.pasteRepository = pasteRepository;
            this.userManager = userManager;
            this.mappingService = mappingService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var pastes = this.pasteRepository.All().OrderByDescending(p => p.CreatedOn);
            var model = await this.mappingService.Map<PasteViewModel>(pastes).ToListAsync();

            return this.View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult View(int id)
        {
            var paste = this.pasteRepository.Get(id);
            var model = this.mappingService.Map<PasteViewModel>(paste);

            return this.View(model);
        }


        [HttpGet]
        public IActionResult Create()
        {
            this.ViewData["Languages"] = this.languageRepository.All().ToList();

            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Paste model)
        {
            if (model != null && ModelState.IsValid)
            {
                var user = await this.userManager.GetUserAsync(User);
                var language = this.languageRepository.Get(model.LanguageId);

                var paste = new Paste
                {
                    Title = model.Title,
                    Content = model.Content,
                    Private = model.Private,
                    Language = language,
                    User = user,
                    CreatedOn = DateTime.Now
                };

                this.pasteRepository.Add(paste);
                this.pasteRepository.SaveChanges();

                return this.RedirectToAction(nameof(Index));
            }

            this.ViewData["Languages"] = this.languageRepository.All().ToList();

            return this.View(model);
        }
    }
}