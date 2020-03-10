namespace PasteBin.Web.Components
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Microsoft.EntityFrameworkCore;
    
    using Microsoft.Extensions.Caching.Memory;

    using PasteBin.Services.Data.Pastes;
    using PasteBin.Services.Web.Mapping;

    using PasteBin.Web.Infrastructure.ViewModels.Pastes;

    public class SidebarViewComponent : ViewComponent
    {
        private readonly IMemoryCache cache;
        private readonly IMappingService mapper;
        private readonly IPasteService pastes;
        
        public SidebarViewComponent(IMappingService mapper, IMemoryCache cache, IPasteService pastes)
        {   
            this.cache = cache;
            this.mapper = mapper;
            this.pastes = pastes;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cacheEntry = await this.cache.GetOrCreateAsync("Pastes", async entry =>
            {
                var pastes = this.pastes.GetAll();
                var model = await this.mapper.Map<PasteViewModel>(pastes).ToListAsync();

                return model;
            });

            return this.View(cacheEntry);
        }
    }
}