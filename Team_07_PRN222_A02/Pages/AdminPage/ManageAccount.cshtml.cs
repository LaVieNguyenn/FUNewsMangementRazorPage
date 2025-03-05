using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using Team_07_PRN222_A02.BLL.DTOs;
using Team_07_PRN222_A02.BLL.Services.SystemAccountService;
using Microsoft.AspNetCore.Authorization;

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

        public async Task<IActionResult> OnGetAsync()
        {
            Accounts = await _services.GetAllAccountsAsync();
            return Page();
        }
    }
}
