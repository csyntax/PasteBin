using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PasteBin.Data.Repositories.Pastes;
using PasteBin.Data.Repositories.Languages;

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

        public async Task<IActionResult> Index()
        {
            this.ViewData["Languages"] = await languageRepository.GetAllAsync();

            return this.View();
        }
    }
}