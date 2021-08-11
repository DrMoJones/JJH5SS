using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace H5SS.Areas.Identity.Codes
{
    public class UserRoleHandler
    {
        public async Task CreateRole(string user, string role, IServiceProvider _serviceProvider)
        {
            //
            var RoleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            //Usermanager kan blive brugt til at lave database funktioner som at finde user via email
            var UserManager = _serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            var userRoleCheck = await RoleManager.RoleExistsAsync(role);

            if(!userRoleCheck)
            {
                await RoleManager.CreateAsync(new IdentityRole(role));
            }

            IdentityUser identityUser = await UserManager.FindByEmailAsync(user);
            Console.WriteLine(await UserManager.FindByEmailAsync(user));
            await UserManager.AddToRoleAsync(identityUser, role);
        }
    }
}
