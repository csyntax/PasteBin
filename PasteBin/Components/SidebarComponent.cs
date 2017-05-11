using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PasteBin.Data.Repositories.Pastes;
using Microsoft.EntityFrameworkCore;

namespace PasteBin.Components
{
    [ViewComponent(Name = "Sidebar")]
    public class SidebarComponent : ViewComponent
    {
        private readonly IPasteRepository pasteRepository;

        public SidebarComponent(IPasteRepository pasteRepository)
        {
            this.pasteRepository = pasteRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var pastes = await this.pasteRepository.All().Where(paste => paste.Private == false).OrderByDescending(paste => paste.CreatedOn).Take(5).ToListAsync();

            return this.View(pastes);
        }
    }
}