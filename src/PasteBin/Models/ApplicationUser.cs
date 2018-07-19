using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PasteBin.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.Pastes = new HashSet<Paste>();
        }

        public ICollection<Paste> Pastes { get; set; }
    }
}