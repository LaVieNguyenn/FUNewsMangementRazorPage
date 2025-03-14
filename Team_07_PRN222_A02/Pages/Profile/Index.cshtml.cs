using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Threading.Tasks;
using Team_07_PRN222_A02.BLL.DTOs;
using Team_07_PRN222_A02.BLL.Services.NewsArticleService;
using Team_07_PRN222_A02.BLL.Services.SystemAccountService;
using Team_07_PRN222_A02.DAL.Models;

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

            return Page();
        }

        // Load dữ liệu khi mở popup
        public async Task<IActionResult> OnGetLoadProfileAsync()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized();
            }

            var user = await _accountService.GetAccountByEmailAsync(userEmail);
            if (user == null)
            {
                return NotFound();
            }

            return new JsonResult(new
            {
                accountName = user.AccountName,
                accountEmail = user.AccountEmail
            });
        }

        // Lưu dữ liệu khi submit form trong popup
        public async Task<IActionResult> OnPostSaveProfileAsync([FromForm] SystemAccountDTO model)
        {
            try
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                if (string.IsNullOrEmpty(userEmail))
                {
                    return new JsonResult(new { success = false, error = "User not authenticated." });
                }

                var existingAccount = await _accountService.GetAccountByEmailAsync(userEmail);
                if (existingAccount == null)
                {
                    return new JsonResult(new { success = false, error = "Account not found." });
                }

                model.AccountID = existingAccount.AccountId; // Gán ID để cập nhật
                bool isUpdated = await _accountService.UpdateAccountAsync(model);

                if (isUpdated)
                {
                    return new JsonResult(new { success = true });
                }
                else
                {
                    return new JsonResult(new { success = false, error = "Update failed." });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error in OnPostSaveProfileAsync: " + ex.Message);
                return new JsonResult(new { success = false, error = "Server error: " + ex.Message });
            }
        }

        public async Task<IActionResult> OnGetLoadNewsHistoryAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var newsList = await _newsService.GetNewsByAuthorIdAsync(int.Parse(userId));

            return new JsonResult(newsList.Select(news => new
            {
                newsTitle = news.NewsTitle,
                createdDate = news.CreatedDate.ToString("yyyy-MM-dd"),
                newsArticleId = news.NewsArticleId
            }));
        }
    }
}
