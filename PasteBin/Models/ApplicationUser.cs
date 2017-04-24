using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace PasteBin.Models
{
    public class ApplicationUser : IdentityUser
    {
        private ICollection<Paste> pastes;

        public ApplicationUser()
        {
            this.pastes = new HashSet<Paste>();
        }

        public ICollection<Paste> Pastes
        {
            get
            {
                return this.pastes;
            }
            set
            {
                this.pastes = value;
            }
        }
    }
}