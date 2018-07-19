using Microsoft.EntityFrameworkCore;
using PasteBin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasteBin.Data.Repositories.Pastes
{
    public class PasteRepository : DbRepository<Paste>, IPasteRepository
    {
        public PasteRepository(ApplicationDbContext context) : base(context)
        {
        }

        protected override IQueryable<Paste> AddIncludes(IQueryable<Paste> queryable)
        {
            return queryable.Include(p => p.Language).Include(p => p.User);
        }
    }
}
