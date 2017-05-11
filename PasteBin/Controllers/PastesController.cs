using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using PasteBin.Models;
using PasteBin.Data.Repositories.Pastes;
using PasteBin.Data.Repositories.Languages;
using System.Linq;

namespace PasteBin.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> Index()
        {
            var user = await this.userManager.GetUserAsync(User);
            var pastes = await this.pasteRepository.All().Where(p => p.User == user).ToListAsync();

            this.ViewData["Username"] = user.UserName;

            return this.View(pastes);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var paste = this.pasteRepository.Find(x => x.Id == id);

            if (paste == null)
            {
                return this.NotFound();
            }

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
        public async Task<IActionResult> Create([FromForm] Paste model)
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