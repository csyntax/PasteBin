namespace PasteBin.Services.Data.Languages
{
    using System.Linq;
    using System.Threading.Tasks;

    using PasteBin.Data.Models;
    using PasteBin.Data.Contracts.Repositories;

    public class LanguageService : ILanguageService
    {
        private readonly IRepository<Language> langRepo;

        public LanguageService(IRepository<Language> langRepo) => 
            this.langRepo = langRepo;

        public IQueryable<Language> Get() => 
            this.langRepo.AllAsNoTracking().OrderBy(p => p.Name);

        public Task<Language> Get(int id) => 
            this.langRepo.GetByIdAsync(id);
    }
}