namespace PasteBin.Services.Data.Pastes
{
    using System.Linq;
    using System.Threading.Tasks;

    using PasteBin.Data.Contracts.Repositories;
    using PasteBin.Data.Models;

    public class PasteService : IPasteService
    {
        private readonly IRepository<Paste> pasteRepo;

        public PasteService(IRepository<Paste> pasteRepo) 
            => this.pasteRepo = pasteRepo;

        public IQueryable<Paste> GetAll() 
            => this.pasteRepo.All().AsQueryable();

        public async Task<int> Add(Paste paste)
        {
            await this.pasteRepo.AddAsync(paste);

            return await this.pasteRepo.SaveChangesAsync();
        }

        public Task<Paste> Get(int id) => 
            this.pasteRepo.GetByIdAsync(id);

        public async Task<int> Remove(int id)
        {
            var paste = await this.pasteRepo.GetByIdAsync(id);

            this.pasteRepo.Delete(paste);

            return await this.pasteRepo.SaveChangesAsync();
        }
    }
}