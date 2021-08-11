using System;
using H5SS.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(H5SS.Areas.Identity.IdentityHostingStartup))]
namespace H5SS.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<H5SSContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("H5SSContextConnection")));

                services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                //For at roles fungere skal man have addroles<IdentityRole>() med   
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<H5SSContext>();


                services.AddAuthorization(options =>
                {
                    options.AddPolicy("RequiredAuthenticatedUser", policy => {
                        policy.RequireAuthenticatedUser();
                    });
                    options.AddPolicy("RequireAdminUser", policy => {
                        policy.RequireRole("Admin");
                    });

                });
            });

        }
    }
}