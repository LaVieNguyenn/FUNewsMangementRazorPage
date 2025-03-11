using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Threading.Tasks;
using Team_07_PRN222_A02.BLL.Services.SystemAccountService;
using Team_07_PRN222_A02.DAL.Models;

namespace Team_07_PRN222_A02.Pages.Profile
{
    public class IndexModel : PageModel
    {
        private readonly ISystemAccountService _accountService;

        public IndexModel(ISystemAccountService accountService)
        {
            _accountService = accountService;
        }

        public SystemAccount Profile { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToPage("/Authentication/Login");
            }

            Profile = await _accountService.GetAccountByEmailAsync(userEmail);
            if (Profile == null)
            {
                return NotFound();
            }

            //if (Profile.AccountRole != 1)
            //{
            //    return Forbid();
            //}

            return Page();
        }
    }
}
