using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PasteBin.Data.Repositories;
using PasteBin.Models;
using Microsoft.AspNetCore.Identity;

namespace PasteBin.Controllers
{
    public class PastesController : Controller
    {
        private readonly IDbRepository<Language> languageRepository;
        private readonly IDbRepository<Paste> pasteRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public PastesController(IDbRepository<Language> languageRepository, 
            IDbRepository<Paste> pasteRepository, UserManager<ApplicationUser> userManager)
        {
            this.languageRepository = languageRepository;
            this.pasteRepository = pasteRepository;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            this.ViewData["Languages"] = this.languageRepository.All();

            return this.View();
        }

        [HttpPost]
        public IActionResult Create(Paste model)
        {
            if (model != null && this.ModelState.IsValid)
            {
                var user = this.userManager.GetUserAsync(User).Result;

                var language = this.languageRepository.Find(model.LanguageId.Value);

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