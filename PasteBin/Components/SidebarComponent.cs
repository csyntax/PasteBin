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

        public IViewComponentResult Invoke()
        {
            var pastes = this.pasteRepository.GetAllAsync();

            return this.View("Sidebar", pastes);
        }
    }
}
