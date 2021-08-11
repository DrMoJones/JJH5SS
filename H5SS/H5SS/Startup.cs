using H5SS.Codes;
using H5SS.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using H5SS.Areas.Identity.Codes;

namespace H5SS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Addsingleton eller addtransient for at starte med dependency injection.
            services.AddControllersWithViews();
            services.AddSingleton<Class>();
            services.AddTransient<UserRoleHandler>();
            services.AddTransient<CryptoEx>();

            services.AddDataProtection();
            var connection = Configuration.GetConnectionString("H5SSContextConnection");

            //services.AddDbContext<masterContext>(options => options.UseSqlServer(connection));

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequiredAuthenticatedUser", policy => {
                    policy.RequireAuthenticatedUser();
                });
                //options.AddPolicy("A", policy => policy.AddAuthenticationSchemes("Admin"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                
                endpoints.MapRazorPages();
            });
        }
    }
}
