using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using Team_07_PRN222_A02.BLL.DTOs;
using Team_07_PRN222_A02.BLL.Services.NewsArticleService;
using Team_07_PRN222_A02.BLL.Services.SystemAccountService;
using System.Text.Json;

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
            Console.WriteLine($"🔍 DEBUG: Dữ liệu deserialize: AccountName = {model?.AccountName}, AccountEmail = {model?.AccountEmail}, AccountId = {model?.AccountID}");

            if (model == null || string.IsNullOrWhiteSpace(model.AccountName) || string.IsNullOrWhiteSpace(model.AccountEmail))
                return BadRequest(new { success = false, error = "Invalid or incomplete data!" });

            var userEmail = GetUserEmail();
            if (string.IsNullOrEmpty(userEmail))
                return StatusCode(401, new { success = false, error = "User is not authenticated!" });

            try
            {
                model.AccountEmail = userEmail; // Giữ email không đổi
                Console.WriteLine($"🔍 DEBUG: Trước khi cập nhật: AccountName = {model.AccountName}, AccountEmail = {model.AccountEmail}, AccountId = {model.AccountID}");

                var isUpdated = await _accountService.UpdateAccountAsync(model);


                Console.WriteLine("🔍 DEBUG: Cập nhật thành công");
                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error updating profile: {ex.Message}");
                return BadRequest(new { success = false, error = ex.Message });
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
