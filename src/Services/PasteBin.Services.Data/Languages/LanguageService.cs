namespace PasteBin.Services.Data.Languages
{
    using System.Linq;

    using PasteBin.Data.Models;
    using PasteBin.Data.Repositories;

    public class LanguageService : ILanguageService
    {
        private readonly IEfRepository<Language> langRepo;

        public LanguageService(IEfRepository<Language> langRepo)
            => this.langRepo = langRepo;

        public IQueryable<Language> Get() => this.langRepo.All().OrderBy(p => p.Name);

        public Language Get(int id) => this.langRepo.Get(id);


        public void Dispose() 
            => this.langRepo.Dispose();
    }
}