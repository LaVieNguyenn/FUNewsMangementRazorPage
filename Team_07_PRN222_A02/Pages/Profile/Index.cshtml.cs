using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Threading.Tasks;
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
        public async Task<IActionResult> OnPostSaveProfileAsync([FromForm] SystemAccount profileData)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized();
            }

            var existingAccount = await _accountService.GetAccountByEmailAsync(userEmail);
            if (existingAccount == null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(profileData.AccountName))
            {
                return BadRequest(new { error = "Name cannot be empty." });
            }

            // Cập nhật thông tin
            existingAccount.AccountName = profileData.AccountName;

            // Nếu có mật khẩu mới, cập nhật
            if (!string.IsNullOrWhiteSpace(profileData.AccountPassword))
            {
                existingAccount.AccountPassword = profileData.AccountPassword;
            }

            try
            {
                await _accountService.UpdateProfileAsync(existingAccount);
                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, error = ex.Message });
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
