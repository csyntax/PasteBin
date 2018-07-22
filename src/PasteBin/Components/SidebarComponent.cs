namespace PasteBin.Components
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;

    using PasteBin.Models;
    using PasteBin.Extensions;
    using PasteBin.Data.Repositories;
    using PasteBin.ViewModels.Pastes;

    [ViewComponent(Name = "Sidebar")]
    public class SidebarComponent : ViewComponent
    {
        private readonly IEfRepository<Paste> pasteRepository;
        private readonly IMemoryCache memoryCache;

        public SidebarComponent(IEfRepository<Paste> pasteRepository, IMemoryCache memoryCache)
        {
            this.pasteRepository = pasteRepository;
            this.memoryCache = memoryCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var pastes = await this.pasteRepository
                .All()
                .Where(p => p.IsPrivate == false)
                .OrderByDescending(p => p.Date)
                .Take(5)
                .To<PasteViewModel>()
                .ToListAsync();

            var cacheEntry = this.memoryCache.GetOrCreate<ICollection<PasteViewModel>>(nameof(pastes), entry =>
            {
                return pastes;
            });

            return this.View(cacheEntry);
        }
    }
}