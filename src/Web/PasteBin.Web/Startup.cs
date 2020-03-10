namespace PasteBin.Web
{ 
    using Microsoft.EntityFrameworkCore;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;

    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using PasteBin.Data;
    using PasteBin.Data.Models;
    using PasteBin.Data.Seeding;
    using PasteBin.Data.Repositories;
    using PasteBin.Data.Contracts.Repositories;

    using PasteBin.Services.Web;
    using PasteBin.Services.Web.Mapping;

    using PasteBin.Services.Data.Pastes;
    using PasteBin.Services.Data.Languages;
   
    using PasteBin.Web.Infrastructure.Extensions;

    public class Startup
    {
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration configuration;
        
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            this.env = env;
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext<ApplicationDbContext>(options =>
                {
                    var assembly = typeof(ApplicationDbContext).Assembly.FullName;
                    var connectionString = this.configuration.GetConnectionString("DefaultConnection");

                    options.UseSqlServer(connectionString, c => c.MigrationsAssembly(assembly));
                });

            services
                .AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>()
                .AddUserStore<ApplicationUserStore>()
                .AddRoleStore<ApplicationRoleStore>()
                .AddEntityFrameworkStores<ApplicationDbContext>() 
                .AddDefaultTokenProviders();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMemoryCache();

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddSingleton(this.env);
            services.AddSingleton(this.configuration);

            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));

            services.AddScoped<IMappingService, MappingService>();
            services.AddScoped<IPasteService, PasteService>();
            services.AddScoped<ILanguageService, LanguageService>();

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IUserStore<ApplicationUser>, ApplicationUserStore>();
            services.AddTransient<IRoleStore<ApplicationRole>, ApplicationRoleStore>();

            services.AddAutoMapper();
        }

        public void Configure(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                dbContext.Database.Migrate();

                ApplicationDbContextSeeder.Seed(dbContext);
            }

            if (this.env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }


            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {                
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}