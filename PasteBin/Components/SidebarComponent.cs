using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasteBin.Data.Repositories;
using PasteBin.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PasteBin.Components
{
    [ViewComponent(Name = "Sidebar")]
    public class SidebarComponent : ViewComponent
    {
        private readonly IDbRepository<Paste> pasteRepository;

        public SidebarComponent(IDbRepository<Paste> pasteRepository)
        {
            this.pasteRepository = pasteRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var pastes = await this.pasteRepository
                .All()
                .Include(p => p.Language)
                .Include(p => p.User)
                .OrderByDescending(p => p.CreatedOn)
                .Take(5)
                .ToListAsync();

            return this.View(pastes);
        }
    }
}