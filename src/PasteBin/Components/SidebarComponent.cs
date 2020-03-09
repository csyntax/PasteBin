namespace PasteBin.Components
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Microsoft.EntityFrameworkCore;
    
    using Microsoft.Extensions.Caching.Memory;

    using Services.Data.Pastes;
    using Services.Web.Mapping;

    using Web.Infrastructure.ViewModels.Pastes;

    [ViewComponent(Name = "Sidebar")]
    public class SidebarComponent : ViewComponent
    {
        private readonly IMemoryCache cache;
        private readonly IMappingService mapper;
        private readonly IPasteService pastes;
        
        public SidebarComponent(IMappingService mapper, IMemoryCache cache, IPasteService pastes)
        {   
            this.cache = cache;
            this.mapper = mapper;
            this.pastes = pastes;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cacheEntry = await this.cache.GetOrCreateAsync("Pastes", async entry =>
            {
                var pastes = this.pastes.GetAll().AsNoTracking();
                var model = await this.mapper.Map<PasteViewModel>(pastes).ToListAsync();

                return model;
            });

            return this.View(cacheEntry);
        }
    }
}