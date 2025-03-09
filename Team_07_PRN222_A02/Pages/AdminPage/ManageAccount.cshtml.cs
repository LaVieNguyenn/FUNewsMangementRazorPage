using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using Team_07_PRN222_A02.BLL.DTOs;
using Team_07_PRN222_A02.BLL.Services.SystemAccountService;

namespace Team_07_PRN222_A02.Pages.AdminPage
{
    [Authorize(Roles = "Admin")]
    public class ManageAccountModel : PageModel
    {
        private readonly ISystemAccountService _services;

        public ManageAccountModel(ISystemAccountService system)
        {
            _services = system;
        }
        public List<SystemAccountDTO> Accounts { get; set; }
        [BindProperty]
        public SystemAccountDTOAdd AccountAdd { get; set; } = new SystemAccountDTOAdd();

        [BindProperty]
        public SystemAccountDTO Account { get; set; }
       

        public async Task<IActionResult> OnGetAsync()
        {
            Accounts = await _services.GetAllAccountsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            ModelState.Clear();
            TryValidateModel(Account);

            if (!ModelState.IsValid)
            {
                TempData["PopupMessage"] = "Update failed! Please check your input.";
                TempData["PopupType"] = "error";
                return RedirectToPage("/AdminPage/ManageAccount");
            }

            var result = await _services.UpdateAccountAsync(Account);
            if (!result)
            {
                TempData["PopupMessage"] = "Error: Failed to update account.";
                TempData["PopupType"] = "error";
                return RedirectToPage("/AdminPage/ManageAccount");
            }

            TempData["PopupMessage"] = "Account updated successfully!";
            TempData["PopupType"] = "success";
            return RedirectToPage("/AdminPage/ManageAccount");
        }
        public async Task<IActionResult> OnPostCreateAsync()
        {
            ModelState.Clear();
            TryValidateModel(AccountAdd); 

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
                TempData["PopupMessage"] = string.Join("<br>", errors);
                TempData["PopupType"] = "error";
                return RedirectToPage("/AdminPage/ManageAccount");
            }

            var result = await _services.CreateAccountAsync(AccountAdd);
            if (!result)
            {
                TempData["PopupMessage"] = "Error: This email is already registered!";
                TempData["PopupType"] = "error";
                return RedirectToPage("/AdminPage/ManageAccount");
            }

            TempData["PopupMessage"] = "Account created successfully!";
            TempData["PopupType"] = "success";
            return RedirectToPage("/AdminPage/ManageAccount");
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            if (Account == null)
            {
                return NotFound();
            }

            var result = await _services.DeleteAccountAsync(Account.AccountID);
            if (!result)
            {
                ModelState.AddModelError("", "Failed to delete the account.");
                return RedirectToPage("/AdminPage/ManageAccount");

            }
            TempData["SuccessMessage"] = "Account deleted successfully!";
            TempData["PopupType"] = "success";
            return RedirectToPage("/AdminPage/ManageAccount");
        }

    }
}
