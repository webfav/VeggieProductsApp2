using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VeggieProductsApp2.Data;

namespace VeggieProductsApp2.Areas.Admin.Controllers
{
    [Authorize(Roles = ManagedUsers.Users.ManagerUser)]
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            return View(await _context.ApplicationUser.Where(u => u.Id != claim.Value).ToListAsync());
        }

        //Method for locking user from site
        public async Task<IActionResult> LockingUser(string id)
        {
            //id is guid
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUser.FirstOrDefaultAsync(m => m.Id == id);

            if (applicationUser == null)
            {
                return NotFound();
            }
            //setting a long time 5000 years for lock
            applicationUser.LockoutEnd = DateTime.Now.AddYears(5000);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //method of unlocking user from site
        public async Task<IActionResult> UnLockingUser(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUser.FirstOrDefaultAsync(m => m.Id == id);

            if (applicationUser == null)
            {
                return NotFound();
            }

            applicationUser.LockoutEnd = DateTime.Now;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


    }
}
