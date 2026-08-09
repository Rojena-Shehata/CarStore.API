using CarStore.Domain.Contracts;
using CarStore.Domain.Entities;
using CarStore.Domain.Entities.IdentityModule;
using CarStore.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.persistence.IdentityData.Data.DataSeed
{
    public class IdentityDataInitializer(UserManager<ApplicationUser> userManager, 
                                         RoleManager<IdentityRole> roleManager,
                                         ILogger<IdentityDataInitializer> logger) : IDataInitializer
    {
       
        public async Task InitializeAsync()
        {
            try
            {
                if (!await roleManager.Roles.AsNoTracking().AnyAsync())
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin.ToString()));
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.SuperAdmin.ToString()));
                }

                if (!await userManager.Users.AsNoTracking().AnyAsync())
                {
                    var user01 = new ApplicationUser()
                    {
                        FullName = "Admin User",
                        UserName = "Admin_User",

                       Email = "admin@example.com",
                        PhoneNumber = "01113129654"
                    };
                    var user02 = new ApplicationUser()
                    {
                        FullName = "Super Admin User",
                        UserName = "Super_Admin_User",
                        Email = "superadmin@example.com",
                        PhoneNumber = "01113129454"
                    };
                    await userManager.CreateAsync(user01, "P@ssw0rd");
                    await userManager.CreateAsync(user02, "P@ssw0rd");

                    await userManager.AddToRoleAsync(user01, UserRoles.Admin.ToString());
                    await userManager.AddToRoleAsync(user02, UserRoles.SuperAdmin.ToString());
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Error In Seeding Identity Data <IdentityDataInitializer> With Message:{ex.Message}");
            }

        }
    }
}
