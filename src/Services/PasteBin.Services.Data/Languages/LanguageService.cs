namespace PasteBin.Services.Data.Languages
{
    using System.Linq;
    using System.Threading.Tasks;
    using PasteBin.Data.Contracts.Repositories;
    using PasteBin.Data.Models;
    using PasteBin.Data.Repositories;

    public class LanguageService : ILanguageService
    {
        private readonly IRepository<Language> langRepo;

        public LanguageService(IRepository<Language> langRepo)
            => this.langRepo = langRepo;

        public IQueryable<Language> Get() => 
            this.langRepo.All().OrderBy(p => p.Name);

        public Task<Language> Get(int id) => 
            this.langRepo.GetByIdAsync(id);


        public void Dispose() 
            => this.langRepo.Dispose();
    }
}