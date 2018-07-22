namespace PasteBin.Models
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.Pastes = new HashSet<Paste>();
        }

        public ICollection<Paste> Pastes { get; set; }
    }
}