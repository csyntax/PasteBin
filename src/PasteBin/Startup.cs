namespace PasteBin
{
    using System;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using Data;
    using Data.Models;
    using Data.Seeding;
    using Data.Repositories;

    using Services.Web;
    using Services.Web.Mapping;
    using Services.Data.Languages;
    using Services.Data.Pastes;

    using Web.Infrastructure.Extensions;

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
                .AddScoped<IApplicationDbContext>(p => p.GetService<ApplicationDbContext>())
                .AddDbContextPool<ApplicationDbContext>(options =>
                {
                    var assembly = typeof(ApplicationDbContext).Assembly.FullName;
                    var connectionString = this.configuration.GetConnectionString("DefaultConnection");

                    options.UseSqlServer(connectionString, c => c.MigrationsAssembly(assembly));
                });

           

            services
                .AddIdentity<ApplicationUser, ApplicationRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 6;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserStore<ApplicationUserStore>()
                .AddRoleStore<ApplicationRoleStore>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Account/Login";
                options.SlidingExpiration = true;
            });

            services.AddMemoryCache();
            services.AddAutoMapper();
            services.AddResponseCompression();
            services.AddControllersWithViews();

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IUserStore<ApplicationUser>, ApplicationUserStore>();
            services.AddTransient<IRoleStore<ApplicationRole>, ApplicationRoleStore>();

            services.AddScoped(typeof(IEfRepository<>), typeof(EfRepository<>));

            services.AddScoped<IMappingService, MappingService>();
            services.AddScoped<IPasteService, PasteService>();
            services.AddScoped<ILanguageService, LanguageService>();
        }

        public void Configure(IApplicationBuilder app)
        { 
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
                var langRepo = serviceScope.ServiceProvider.GetRequiredService<IEfRepository<Language>>();

                dbContext.Database.Migrate();

                ApplicationDbContextSeeder.Seed(langRepo);
            }

            if (this.env.EnvironmentName == "Development")
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseRouting();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseResponseCompression();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}