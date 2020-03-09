namespace PasteBin
{
    using System;

    using Microsoft.EntityFrameworkCore;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;

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
    using Microsoft.Extensions.Hosting;
    using Microsoft.AspNetCore.Http;

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
                .AddDefaultIdentity<ApplicationUser>()
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            /* services
                  .AddIdentity<ApplicationUser, ApplicationRole>()
                  .AddEntityFrameworkStores<ApplicationDbContext>()
                  .AddUserStore<ApplicationUserStore>()
                  .AddRoleStore<ApplicationRoleStore>()
                  .AddDefaultTokenProviders();*/

            services
                .Configure<IdentityOptions>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 6;
                });

            /* services
                .ConfigureApplicationCookie(options =>
                {
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                    options.LoginPath = "/Account/Login";
                    options.SlidingExpiration = true;
                });*/

            services.Configure<CookiePolicyOptions>(
               options =>
               {
                   options.CheckConsentNeeded = context => true;
                   options.MinimumSameSitePolicy = SameSiteMode.None;
               });


            services.AddMemoryCache();
            services.AddResponseCompression();

            
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddSingleton(this.env);
            services.AddSingleton(this.configuration);

            services.AddScoped(typeof(IEfRepository<>), typeof(EfRepository<>));

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
            app.UseAutoMapper();

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
                //endpoints.MapControllers();
                
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}