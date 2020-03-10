namespace PasteBin.Services.Data.Pastes
{
    using System.Linq;
    using System.Threading.Tasks;

    using PasteBin.Data.Models;
    
    public interface IPasteService
    {
        IQueryable<Paste> GetAll();

        Task<int> Add(Paste paste);

        Task<Paste> Get(int id);

        Task<int> Delete(int id);
    }
}