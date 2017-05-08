using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasteBin.Data.Repositories;
using PasteBin.Data.Repositories.Pastes;
using PasteBin.Models;
using System.Linq;
using System.Threading.Tasks;

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
            var pastes = await this.pasteRepository.GetAllAsync();

            return this.View(pastes);
        }
    }
}