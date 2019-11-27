namespace PasteBin.Data
{
    using System.Security.Claims;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

    using Data.Models;

    public class ApplicationRoleStore : RoleStore<
        ApplicationRole,
        ApplicationDbContext,
        string,
        IdentityUserRole<string>,
        IdentityRoleClaim<string>>
    {
        public ApplicationRoleStore(ApplicationDbContext context, IdentityErrorDescriber describer = null)
            : base(context, describer)
        {
        }

        public new HashSet<ApplicationRole> Roles { get; set; }

        protected override IdentityRoleClaim<string> CreateRoleClaim(ApplicationRole role, Claim claim)
            => new IdentityRoleClaim<string>
            {
                RoleId = role.Id,
                ClaimType = claim.Type,
                ClaimValue = claim.Value,
            };
    }
}
