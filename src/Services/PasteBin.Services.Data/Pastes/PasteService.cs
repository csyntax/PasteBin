namespace PasteBin.Services.Data.Pastes
{
    using System.Linq;
    using System.Threading.Tasks;

    using PasteBin.Data.Models;
    using PasteBin.Data.Repositories;

    public class PasteService : IPasteService
    {
        private readonly IEfRepository<Paste> pasteRepo;

        public PasteService(IEfRepository<Paste> pasteRepo) 
            => this.pasteRepo = pasteRepo;

        public IQueryable<Paste> GetAll() 
            => this.pasteRepo.All().OrderByDescending(p => p.Date).AsQueryable();

        public int Add(Paste paste)
        {
            this.pasteRepo.Add(paste);

            return this.pasteRepo.SaveChanges();
        }

        public async Task<int> AddAsync(Paste paste)
        {
            this.pasteRepo.Add(paste);

            return await this.pasteRepo.SaveChangesAsync();
        }

        public Paste Get(int id) => this.pasteRepo.Get(id);

        public int Remove(int id)
        {
            var paste = this.Get(id);

            this.pasteRepo.Delete(paste);

            return this.pasteRepo.SaveChanges();
        }

        public async Task<int> RemoveAsync(int id)
        {
            var paste = this.Get(id);

            this.pasteRepo.Delete(paste);

            return await this.pasteRepo.SaveChangesAsync();
        }

        public void Dispose() 
            => this.pasteRepo.Dispose();
    }
}