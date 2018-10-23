using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using DndCharacterBuilder.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DndCharacterBuilder.Areas.Account.Pages
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        public RegisterViewModel Input { get; set; }

        public RegisterModel(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            var user = new ApplicationUser {
                UserName = Input.Email,
                Email = Input.Email,
            };
            var result = await _userManager.CreateAsync(user, Input.Password);

            if(result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }

            foreach(var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }

        public class RegisterViewModel
        {
            [Required]
            public string Email { get; set; }

            [Required]
            public string Password { get; set; }

            [Required, Compare("Password")]
            public string ConfirmPassword { get; set; }
        }
    }
}