namespace PasteBin.Data.Models
{
    using System;

    using Microsoft.AspNetCore.Identity;

    public class ApplicationRole : IdentityRole<string>
    {
        public ApplicationRole()
            : this(null)
        {
        }

        public ApplicationRole(string name)
            : base(name)
        {
            this.Id = Guid.NewGuid().ToString();
        }
    }
}
