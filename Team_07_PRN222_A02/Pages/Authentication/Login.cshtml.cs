using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Team_07_PRN222_A02.BLL.DTOs;
using Team_07_PRN222_A02.BLL.Services.SystemAccountService;
using Team_07_PRN222_A02.DAL.Helper;

namespace Team_07_PRN222_A02.Pages.Authentication
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LoginDTO login { get; set; }
        private readonly ISystemAccountService _services;
        public LoginModel(ISystemAccountService system)
        {
            _services = system;
        }
        public void OnGet(string? returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = await _services.Login(login.Email, login.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid Email or Password");
                    return Page();
                }
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.AccountId.ToString()),
                    new Claim(ClaimTypes.Name, user.AccountName),
                    new Claim(ClaimTypes.Email, user.AccountEmail),
                    new Claim(ClaimTypes.Role, user.AccountRole == (byte)Enums.Role.Staff ? "Staff" : user.AccountRole == (byte)Enums.Role.Lecturer ? "Lecture" : "Admin")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else return Redirect("/");
            }
            return Page();
        }

        public async Task<IActionResult> OnGetLogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect("/");
        }
    }
}
