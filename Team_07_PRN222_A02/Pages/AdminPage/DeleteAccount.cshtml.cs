using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Team_07_PRN222_A02.BLL.DTOs;
using Team_07_PRN222_A02.BLL.Services.SystemAccountService;

namespace Team_07_PRN222_A02.Pages.AdminPage
{
    [Authorize(Roles = "Admin")]
    public class DeleteAccountModel : PageModel
    {
        private readonly ISystemAccountService _services;

        public DeleteAccountModel(ISystemAccountService system)
        {
            _services = system;
        }

        [BindProperty]
        public SystemAccountDTO Account { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Account = await _services.GetAccountByIdAsync(id);
            if (Account == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Account == null)
            {
                return NotFound();
            }

            var result = await _services.DeleteAccountAsync(Account.AccountID);
            if (!result)
            {
                ModelState.AddModelError("", "Failed to delete the account.");
                return Page();
            }

            return RedirectToPage("/AdminPage/ManageAccount");
        }
    }
}
