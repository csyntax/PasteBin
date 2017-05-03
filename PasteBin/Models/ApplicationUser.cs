using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace PasteBin.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
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