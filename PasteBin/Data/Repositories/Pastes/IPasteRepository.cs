using PasteBin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasteBin.Data.Repositories.Pastes
{
    public interface IPasteRepository : IDbRepository<Paste>
    {
    }
}