using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Team_07_PRN222_A02.BLL.DTOs;
using Team_07_PRN222_A02.BLL.Services.SystemAccountService;

namespace Team_07_PRN222_A02.Pages.AdminPage
{
    [Authorize(Roles = "Admin")]
    public class CreateAccountModel : PageModel
    {
        private readonly ISystemAccountService _service;

        public CreateAccountModel(ISystemAccountService service)
        {
            _service = service;
        }

        [BindProperty]
        public SystemAccountDTOAdd Account { get; set; } = new SystemAccountDTOAdd();

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _service.CreateAccountAsync(Account);
            if (!result)
            {
                ModelState.AddModelError("Account.AccountEmail", "This email is already registered.");
                return Page();
            }

            return RedirectToPage("/AdminPage/ManageAccount");
        }
    }
}
