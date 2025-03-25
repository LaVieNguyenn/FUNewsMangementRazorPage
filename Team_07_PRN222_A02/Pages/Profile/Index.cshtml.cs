using Azure.Core;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
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


        public async Task<IActionResult> OnPostSaveProfileAsync()
        {
            using var reader = new StreamReader(Request.Body);
            var requestBody = await reader.ReadToEndAsync();
            Console.WriteLine($"🔍 DEBUG: Nhận request: {requestBody}");

            if (string.IsNullOrEmpty(requestBody))
                return BadRequest(new { success = false, error = "Empty request body!" });

            var model = JsonSerializer.Deserialize<SystemAccountDTO>(requestBody);
            if (model == null || string.IsNullOrWhiteSpace(model.AccountName))
                return BadRequest(new { success = false, error = "Invalid data!" });

            // Tiếp tục cập nhật dữ liệu
            return new JsonResult(new { success = true });
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

            if (newsList == null || newsList.Count == 0)
            {
                return new JsonResult(new { success = true, data = new List<NewsArticleDTO>() });
            }

            return new JsonResult(new { success = true, data = newsList });
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