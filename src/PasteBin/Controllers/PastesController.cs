namespace PasteBin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;    

    using PasteBin.Models;
    using PasteBin.Data.Repositories;
    using System.Linq;
    using PasteBin.ViewModels.Pastes;
    using PasteBin.Extensions;
    using System.Threading.Tasks;

    public class PastesController : Controller
    {
        private readonly IEfRepository<Paste> pasteRepository;
        private readonly IEfRepository<Language> languageRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public PastesController(
            IEfRepository<Paste> pasteRepository,
            IEfRepository<Language> languageRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.pasteRepository = pasteRepository;
            this.languageRepository = languageRepository;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var pastes = this.pasteRepository.All().OrderByDescending(p => p.Date).To<PasteViewModel>().ToList();

            return this.View(pastes);
        }

        [HttpGet]
        public IActionResult View(int id)
        {
            var paste = this.pasteRepository.All().To<PasteViewModel>().Where(p => p.Id == id).FirstOrDefault();

            return this.View(paste);
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

                return this.RedirectToAction(nameof(this.Index));
            }

            this.ViewData["Languages"] = this.languageRepository.All().ToList();

            return this.View(model);
        }
    }
}