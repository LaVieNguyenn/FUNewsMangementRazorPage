using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using Team_07_PRN222_A02.BLL.DTOs;
using Team_07_PRN222_A02.BLL.Services.NewsArticleService;
using Team_07_PRN222_A02.BLL.Services.SystemAccountService;
using System.Text.Json;
using AutoMapper;

namespace Team_07_PRN222_A02.Pages.Profile
{
    public class IndexModel : PageModel
    {
        private readonly ISystemAccountService _accountService;
        private readonly INewArticleService _newsService;
        private readonly IMapper _mapper;

        public IndexModel(ISystemAccountService accountService, INewArticleService newsService, IMapper mapper)
        {
            _accountService = accountService;
            _newsService = newsService;
            _mapper = mapper;
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

        [BindProperty]
        public SystemAccountDTO InputProfile { get; set; } = new SystemAccountDTO();

        [HttpPost]
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

            // Đảm bảo mỗi tin tức có ID
            var data = newsList.Select(n => new
            {
                newsArticleId = n.NewsArticleId, // Đảm bảo ID có dữ liệu
                newsTitle = n.NewsTitle ?? "No Title",
                createdDate = n.CreatedDate.ToString("yyyy-MM-dd HH:mm")
            }).ToList();

            return new JsonResult(new { success = true, data });
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

            Console.WriteLine($"📜 DEBUG: Loaded News ID {newsId} - {news.NewsTitle}");

            return new JsonResult(new
            {
                success = true,
                title = news.NewsTitle ?? "(No Title)",  // ✅ Đảm bảo không bị null
                createdDate = news.CreatedDate.ToString("yyyy-MM-dd HH:mm"),
                content = news.NewsContent ?? "(No Content)"  // ✅ Đảm bảo không bị null
            });
        }

    }
}