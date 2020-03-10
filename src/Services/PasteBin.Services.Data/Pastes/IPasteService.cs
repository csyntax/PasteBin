namespace PasteBin.Services.Data.Pastes
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using PasteBin.Data.Models;
    
    public interface IPasteService  //: IDisposable
    {
        IQueryable<Paste> GetAll();

        Task<int> Add(Paste paste);

        Task<Paste> Get(int id);

        Task<int> Remove(int id);
    }
}