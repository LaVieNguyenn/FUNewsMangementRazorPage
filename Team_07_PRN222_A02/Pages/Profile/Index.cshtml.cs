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

        private string? GetUserEmail()
        {
            return User?.Identity?.IsAuthenticated == true
                ? User.FindFirstValue(ClaimTypes.Email) ?? User.FindFirstValue(ClaimTypes.Name)
                : null;
        }


        public async Task<IActionResult> OnGetAsync()
        {
            var userEmail = GetUserEmail();
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToPage("/Authentication/Login");
            }

            Profile = await _accountService.GetCurrentUserProfileAsync(userEmail);
            if (Profile == null)
            {
                return RedirectToPage("/Error");
            }

            return Page();
        }


        public async Task<IActionResult> OnPostSaveProfileAsync([FromForm] SystemAccountDTO model)
        {
            Console.WriteLine($"🔍 DEBUG: Nhận request cập nhật Profile cho {model.AccountEmail} - Tên mới: {model.AccountName}");

            var userEmail = GetUserEmail();
            if (string.IsNullOrEmpty(userEmail))
            {
                Console.WriteLine("❌ ERROR: User không xác thực!");
                return new JsonResult(new { success = false, error = "User is not authenticated!" });
            }

            var account = await _accountService.GetAccountByEmailAsync(userEmail);
            if (account == null)
            {
                Console.WriteLine("❌ ERROR: Không tìm thấy user trong database!");
                return new JsonResult(new { success = false, error = "User not found!" });
            }

            if (string.IsNullOrWhiteSpace(model.AccountName))
            {
                Console.WriteLine("❌ ERROR: Account Name bị trống!");
                return new JsonResult(new { success = false, error = "Account Name cannot be empty!" });
            }

            account.AccountName = model.AccountName.Trim();

            try
            {
                Console.WriteLine("✅ DEBUG: Đang cập nhật profile vào DB...");
                await _accountService.UpdateProfileAsync(account);
                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERROR: {ex.Message}");
                return new JsonResult(new { success = false, error = "Error updating profile: " + ex.Message });
            }
        }
        public async Task<IActionResult> OnGetLoadNewsHistoryAsync()
        {
            var userEmail = GetUserEmail();
            if (string.IsNullOrEmpty(userEmail))
            {
                return new JsonResult(new { success = false, error = "User is not authenticated!" });
            }

            var account = await _accountService.GetAccountByEmailAsync(userEmail);
            if (account == null)
            {
                return new JsonResult(new { success = false, error = "User not found!" });
            }

            var newsList = await _newsService.GetNewsByAuthorIdAsync(account.AccountId);
            return new JsonResult(newsList ?? new List<NewsArticleDTO>());
        }

        public async Task<IActionResult> OnGetLoadNewsDetailAsync(int newsId)
        {
            if (newsId <= 0)
            {
                return new JsonResult(new { success = false, error = "Invalid news ID!" });
            }

            var news = await _newsService.GetNewsAsyncById(newsId);
            if (news == null)
            {
                return new JsonResult(new { success = false, error = "News not found!" });
            }

            return new JsonResult(new
            {
                success = true,
                title = news.NewsTitle,
                createdDate = news.CreatedDate.ToString("yyyy-MM-dd HH:mm"),
                content = news.NewsContent
            });
        }
    }
}
