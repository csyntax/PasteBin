namespace PasteBin.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
   
    using PasteBin.Models;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Paste> Pastes { get; set; }

        public DbSet<Language> Languages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<Paste>().HasOne<Language>().WithMany(c => c.Pastes).HasForeignKey(c => c.LanguageId);

            base.OnModelCreating(builder);
        }
    }
}
