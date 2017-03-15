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

        public PasteController()
        {

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}