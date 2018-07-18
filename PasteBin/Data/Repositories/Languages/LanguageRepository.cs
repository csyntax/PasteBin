using PasteBin.Models;

namespace PasteBin.Data.Repositories.Languages
{
    public class LanguageRepository : DbRepository<Language>, ILanguageRepository
    {
        public LanguageRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}