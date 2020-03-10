namespace PasteBin.Services.Data.Languages
{
    using System.Linq;
    using System.Threading.Tasks;

    using PasteBin.Data.Models;

    public interface ILanguageService
    {
        IQueryable<Language> Get();

        Task<Language> Get(int id);
    }
}