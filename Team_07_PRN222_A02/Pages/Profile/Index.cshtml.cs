using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;
using Team_07_PRN222_A02.BLL.DTOs;
using Team_07_PRN222_A02.BLL.Services.NewsArticleService;
using Team_07_PRN222_A02.BLL.Services.SystemAccountService;
using Team_07_PRN222_A02.DAL.Models;
using AutoMapper; // Nếu sử dụng AutoMapper

namespace Team_07_PRN222_A02.Pages.Profile
{
    public class IndexModel : PageModel
    {
        private readonly ISystemAccountService _accountService;
        private readonly INewArticleService _newsService;
        private readonly IMapper _mapper; // Sử dụng AutoMapper

        public IndexModel(ISystemAccountService accountService, INewArticleService newsService, IMapper mapper)
        {
            _accountService = accountService;
            _newsService = newsService;
            _mapper = mapper;
        }

        public SystemAccountDTO Profile { get; set; }

        // ✅ Hàm tiện ích: Lấy email người dùng từ Claims
        private string GetUserEmail()
        {
            return User.FindFirstValue(ClaimTypes.Email);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userEmail = GetUserEmail();
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToPage("/Authentication/Login");
            }

            var account = await _accountService.GetAccountByEmailAsync(userEmail);
            if (account == null)
            {
                return NotFound();
            }

            Profile = _mapper.Map<SystemAccountDTO>(account); // Chuyển đổi sang DTO
            return Page();
        }

        // ✅ Load profile khi mở modal Edit
        public async Task<IActionResult> OnGetLoadProfileAsync()
        {
            var userEmail = GetUserEmail();
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized();
            }

            var user = await _accountService.GetAccountByEmailAsync(userEmail);
            if (user == null)
            {
                return NotFound();
            }

            return new JsonResult(_mapper.Map<SystemAccountDTO>(user)); // Trả về DTO
        }

        // ✅ Lưu Profile khi Submit
        public async Task<IActionResult> OnPostSaveProfileAsync([FromForm] SystemAccountDTO model)
        {
            try
            {
                var userEmail = GetUserEmail();
                if (string.IsNullOrEmpty(userEmail))
                {
                    return new JsonResult(new { success = false, error = "User not authenticated." });
                }

                var existingAccount = await _accountService.GetAccountByEmailAsync(userEmail);
                if (existingAccount == null)
                {
                    return new JsonResult(new { success = false, error = "Account not found." });
                }

                // Chỉ cập nhật tên, không cập nhật email hoặc password
                existingAccount.AccountName = model.AccountName ?? existingAccount.AccountName;

                // Chuyển đổi sang DTO trước khi cập nhật
                var accountDto = _mapper.Map<SystemAccountDTO>(existingAccount);

                bool isUpdated = await _accountService.UpdateAccountAsync(accountDto);

                return new JsonResult(new { success = isUpdated });
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error in OnPostSaveProfileAsync: " + ex.Message);
                return new JsonResult(new { success = false, error = "Server error: " + ex.Message });
            }
        }

        // ✅ Load danh sách bài báo khi mở modal News History
        public async Task<IActionResult> OnGetLoadNewsHistoryAsync()
        {
            try
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
                {
                    Console.WriteLine("❌ ERROR: Invalid User ID.");
                    return Unauthorized();
                }

                Console.WriteLine($"🔍 Fetching news for User ID: {userId}");

                // Lấy danh sách bài báo của user
                var newsList = await _newsService.GetNewsByAuthorIdAsync(userId);

                if (newsList == null || !newsList.Any())
                {
                    Console.WriteLine($"⚠ No news found for User ID: {userId}");
                    return new JsonResult(new List<object>()); // Trả về mảng rỗng thay vì lỗi
                }

                Console.WriteLine($"✅ {newsList.Count()} news articles found.");

                return new JsonResult(newsList.Select(news => new
                {
                    newsTitle = news.NewsTitle,
                    createdDate = news.CreatedDate.ToString("yyyy-MM-dd"),
                    newsArticleId = news.NewsArticleId
                }));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERROR in OnGetLoadNewsHistoryAsync: {ex.Message}");
                return new JsonResult(new { error = "Server error: " + ex.Message });
            }
        }
    }
}
