using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Project_ITLab.Data;
using Project_ITLab.Data.IServices;
using Project_ITLab.Data.ServiceInstances;
using Project_ITLab.Models.Domain;

namespace Project_ITLab {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {


            services.AddAuthorization(o => {
                o.AddPolicy("Admins", s => s.Requirements.Add(new RolesAuthorizationRequirement(new string[] { "Admin", "HeadAdmin" })));
                o.AddPolicy("HeadAdmins", s => s.RequireRole("HeadAdmin"));
            });
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddSession();
            services.AddMemoryCache();

            services.AddScoped<DataInit>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<IUserService, UserService>();
            //services.AddScoped<SignInManager<IdentityUser>>();
            //services.AddScoped<UserManager<IdentityUser>>();

            //services.Add(new ServiceDescriptor(typeof(IUserService), new UserService()));//#bruv what this? =Iets voor eenzelfde userservice bij te houden bij elke view,controller,etc. die het nodig heeft. 
            //services.Add(new ServiceDescriptor(typeof(ISessionService), new SessionService()));

            services.AddDbContext<Context>(options =>
                options.UseSqlServer(
                    //Configuration.GetConnectionString("Azure")
                    Environment.GetEnvironmentVariable("AzureConnectionString")
                    ));

            services.AddIdentity<IdentityUser, IdentityRole>(o => {
                o.Password.RequireDigit = false;
                o.Password.RequireUppercase = false;
                o.Password.RequiredLength = 6;
                o.Password.RequireLowercase = false;
                o.Password.RequireNonAlphanumeric = false;

                //o.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<Context>();

            services.ConfigureApplicationCookie(o => {
                o.Cookie.Name = "Token";
                o.Cookie.HttpOnly = true;
                o.ExpireTimeSpan = TimeSpan.FromHours(1);
                o.LoginPath = "/Identity/Account/Login";
                o.LogoutPath = "/Session/Index";
                o.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataInit dataInit) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Session}/{action=Index}/{id?}");
                endpoints.MapRazorPages();

            });
            dataInit.InitAsync().Wait();
        }
    }
}
