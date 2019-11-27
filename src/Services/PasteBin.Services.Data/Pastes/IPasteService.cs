namespace PasteBin.Services.Data.Pastes
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using PasteBin.Data.Models;
    
    public interface IPasteService : IDisposable
    {
        IQueryable<Paste> GetAll();

        Paste Get(int id);

        int Add(Paste paste);

        Task<int> AddAsync(Paste paste);

        int Remove(int id);

        Task<int> RemoveAsync(int id);
    }
}