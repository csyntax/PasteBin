using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PasteBin.Data.Repositories.Pastes;

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

            return View("Sidebar", pastes);
        }
    }
}
