using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PasteBin.Data.Repositories.Pastes;
using PasteBin.Data.Repositories.Languages;
using PasteBin.Models;

namespace PasteBin.Controllers
{
    public class PasteController : Controller
    {
        private readonly IPasteRepository pasteRepository;
        private readonly ILanguageRepository languageRepository;

        public PasteController(IPasteRepository pasteRepository, ILanguageRepository languageRepository)
        {
            this.pasteRepository = pasteRepository;
            this.languageRepository = languageRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            this.ViewData["Languages"] = await this.languageRepository.GetAllAsync();

            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePasteViewModel model)
        {
            if (ModelState.IsValid)
            {
                var language = await this.languageRepository.FindOneAsync(m => m.Id == model.Language);

                var paste = new Paste
                {
                    Content = model.Content,
                    Language = language
                };

                await this.pasteRepository.AddAsync(paste);

                return this.RedirectToAction("View", new { id = paste.Id });
            }

            this.ViewData["Languages"] = await this.languageRepository.GetAllAsync();

            return this.View("Index", model);
        }

        public async Task<IActionResult> View(int id)
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