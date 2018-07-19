namespace PasteBin.Components
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using PasteBin.Models;
    using PasteBin.Data.Repositories;
    using PasteBin.ViewModels.Pastes;
    using PasteBin.Extensions;
    
    [ViewComponent(Name = "Sidebar")]
    public class SidebarComponent : ViewComponent
    {
        private readonly IEfRepository<Paste> pasteRepository;

        public SidebarComponent(IEfRepository<Paste> pasteRepository)
        {
            this.pasteRepository = pasteRepository;
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

            return this.View(pastes);
        }
    }
}