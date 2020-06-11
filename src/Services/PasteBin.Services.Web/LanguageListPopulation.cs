using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PasteBin.Services.Data.Languages;
using PasteBin.Services.Web.Mapping;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasteBin.Services.Web
{
    public class LanguageListPopulation
    {
        private readonly IMemoryCache cache;
        private readonly IMappingService mapper;
        private readonly ILanguageService languages;

        public LanguageListPopulation(IMemoryCache cache, 
            IMappingService mapper, 
            ILanguageService languages)
        {
            this.cache = cache;
            this.mapper = mapper;
            this.languages = languages;
        }

        public async Task<IEnumerable<SelectListItem>> PopulateSelectList()
        {
            var languages = await this.languages.Get()
                .Select(lang => new SelectListItem()
                {
                    Value = lang.Id.ToString(),
                    Text = lang.Name
                }).ToListAsync();

            return languages;
        }
    }
}