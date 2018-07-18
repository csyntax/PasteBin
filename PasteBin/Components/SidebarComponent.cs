namespace PasteBin.Components
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using PasteBin.Models;
    using PasteBin.Data.Repositories;

    [ViewComponent(Name="Sidebar")]
    public class SidebarComponent : ViewComponent
    {
        private readonly IEfRepository<Paste> pasteRepository;

        public SidebarComponent(IEfRepository<Paste> pasteRepository)
        {
            this.pasteRepository = pasteRepository;
        }

        public IViewComponentResult Invoke()
        {
            var pastes = this.pasteRepository.All().OrderByDescending(p => p.CreatedOn).Take(5).ToList();

            return this.View(pastes);
        }
    }
}