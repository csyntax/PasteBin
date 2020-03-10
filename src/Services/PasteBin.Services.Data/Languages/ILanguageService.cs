namespace PasteBin.Services.Data.Languages
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using PasteBin.Data.Models;

    public interface ILanguageService : IDisposable
    {
        IQueryable<Language> Get();

        Task<Language> Get(int id);
    }
}