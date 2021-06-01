using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VeggieProductsApp2.Models;
using VeggieProductsApp2.ManagedUsers;
using System.Diagnostics;

namespace VeggieProductsApp2.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async void Initialize()
        {
            try
            {
                //run the migrations to a new db
                if (_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Message: " + ex.Message);
            }

            if (_context.Roles.Any(r => r.Name == Users.ManagerUser)) return;

            _roleManager.CreateAsync(new IdentityRole(Users.ManagerUser)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Users.EndUser)).GetAwaiter().GetResult();

            //default user Admin gets added to db
            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                FullName = "Admin",
                Zipcode = "1000",
                EmailConfirmed = true
            }, "Admin!0").GetAwaiter().GetResult();

            IdentityUser user = await _context.Users.FirstOrDefaultAsync(u => u.Email == "admin@gmail.com");

            await _userManager.AddToRoleAsync(user, Users.ManagerUser);
        }
    }
}
