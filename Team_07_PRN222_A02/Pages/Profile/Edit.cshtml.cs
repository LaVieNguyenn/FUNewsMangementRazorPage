using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Threading.Tasks;
using Team_07_PRN222_A02.BLL.Services.SystemAccountService;
using Team_07_PRN222_A02.DAL.Models;

namespace Team_07_PRN222_A02.Pages.Profile
{
    public class EditModel : PageModel
    {
        private readonly ISystemAccountService _accountService;

        public EditModel(ISystemAccountService accountService)
        {
            _accountService = accountService;
        }

        [BindProperty]
        public SystemAccount Profile { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Authentication/Login");
            }

            Profile = await _accountService.GetAccountById(int.Parse(userId));

            if (Profile == null || Profile.AccountRole != 1)
            {
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || int.Parse(userId) != Profile.AccountId)
            {
                return Forbid();
            }

            var existingAccount = await _accountService.GetAccountById(Profile.AccountId);
            if (existingAccount == null)
            {
                return NotFound();
            }

            existingAccount.AccountName = Profile.AccountName;

            if (!string.IsNullOrWhiteSpace(Profile.AccountPassword))
            {
                existingAccount.AccountPassword = Profile.AccountPassword;
            }

            await _accountService.UpdateProfileAsync(existingAccount);

            return RedirectToPage("./Index");
        }

    }
}
