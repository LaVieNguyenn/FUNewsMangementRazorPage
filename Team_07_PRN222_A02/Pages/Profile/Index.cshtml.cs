using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using Team_07_PRN222_A02.BLL.DTOs;
using Team_07_PRN222_A02.BLL.Services.NewsArticleService;
using Team_07_PRN222_A02.BLL.Services.SystemAccountService;

namespace Team_07_PRN222_A02.Pages.Profile
{
    public class IndexModel : PageModel
    {
        private readonly ISystemAccountService _accountService;
        private readonly INewArticleService _newsService;

        public IndexModel(ISystemAccountService accountService, INewArticleService newsService)
        {
            _accountService = accountService;
            _newsService = newsService;
        }

        public SystemAccountDTO Profile { get; set; }

        private string GetUserEmail()
        {
            return User.FindFirstValue(ClaimTypes.Email) ?? User.FindFirstValue(ClaimTypes.Name);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userEmail = GetUserEmail();
            if (string.IsNullOrEmpty(userEmail)) return RedirectToPage("/Authentication/Login");

            Profile = await _accountService.GetCurrentUserProfileAsync(userEmail);
            return Page();
        }

        public async Task<IActionResult> OnPostSaveProfileAsync([FromForm] SystemAccountDTO model)
        {
            var userEmail = GetUserEmail();
            var account = await _accountService.GetAccountByEmailAsync(userEmail);

            if (account == null)
            {
                return new JsonResult(new { success = false, error = "User not found!" });
            }

            account.AccountName = model.AccountName ?? account.AccountName;
            account.AccountRole = model.AccountRole;

            await _accountService.UpdateProfileAsync(account);

            return new JsonResult(new { success = true });
        }


        public async Task<IActionResult> OnGetLoadNewsHistoryAsync()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return new JsonResult(new { success = false, error = "User ID is invalid!" });
            }

            var newsList = await _newsService.GetNewsByAuthorIdAsync(userId);

            return new JsonResult(newsList ?? new List<NewsArticleDTO>());
        }

    }
}
