namespace PasteBin.Services.Data.Pastes
{
    using System.Linq;
    using System.Threading.Tasks;

    using PasteBin.Data.Models;
    using PasteBin.Data.Contracts.Repositories;
   
    public class PasteService : IPasteService
    {
        private readonly IDeletableEntityRepository<Paste> pasteRepo;

        public PasteService(IDeletableEntityRepository<Paste> pasteRepo) => 
            this.pasteRepo = pasteRepo;

        public IQueryable<Paste> GetAll() => 
            this.pasteRepo.AllAsNoTracking();

        public async Task<int> Add(Paste paste)
        {
            await this.pasteRepo.AddAsync(paste);

            return await this.pasteRepo.SaveChangesAsync();
        }

        public Task<Paste> Get(int id) => 
            this.pasteRepo.GetByIdAsync(id);

        public async Task<int> Delete(int id)
        {
            var paste = await this.pasteRepo.GetByIdAsync(id);

            this.pasteRepo.Delete(paste);

            return await this.pasteRepo.SaveChangesAsync();
        }
    }
}