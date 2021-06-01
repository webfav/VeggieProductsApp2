using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using VeggieProductsApp2.ManagedUsers;
using VeggieProductsApp2.Models;

namespace VeggieProductsApp2.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        

        //added RoleManager
        //deleted emailSender

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            RoleManager<IdentityRole> roleManager        
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _roleManager = roleManager;           
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        //public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "Navn")]
            public string FullName { get; set; }

            [Required]
            [Display(Name = "Postnr")]
            public string Zipcode { get; set; }
        }


        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            string role = Request.Form["radioBtnUserRole"].ToString();

            returnUrl = returnUrl ?? Url.Content("~/");
            
            if (ModelState.IsValid)
            {
                //changed from IdentityUser to ApplicationUser
                var user = new ApplicationUser 
                { 
                    UserName = Input.Email, 
                    Email = Input.Email,
                    FullName = Input.FullName,
                    Zipcode = Input.Zipcode
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    //if (!await _roleManager.RoleExistsAsync(Users.EndUser))
                    //{
                    //    await _roleManager.CreateAsync(new IdentityRole(Users.EndUser));
                    //}

                    //if (!await _roleManager.RoleExistsAsync(Users.ManagerUser))
                    //{
                    //    await _roleManager.CreateAsync(new IdentityRole(Users.ManagerUser));
                    //}
                    
                    //await _userManager.AddToRoleAsync(user, Users.ManagerUser);

                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    //return LocalRedirect(returnUrl);


                    if (role == Users.EndUser)
                    {
                        await _userManager.AddToRoleAsync(user, Users.EndUser);
                    }
                    else
                    {
                        if (role == Users.ManagerUser)
                        {
                            await _userManager.AddToRoleAsync(user, Users.ManagerUser);
                        }
                        else
                        {
                            await _userManager.AddToRoleAsync(user, Users.EndUser);
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }

                    }

                    return RedirectToAction("Index", "User", new { area = "Admin" });


                }
                 _logger.LogInformation("Bruger har lavet ny konto med password.");

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
