using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Team_07_PRN222_A02.BLL.Services.NewsArticleService;
using Team_07_PRN222_A02.DAL.Models;

namespace Team_07_PRN222_A02.Pages.Profile
{
    public class NewsHistoryModel : PageModel
    {
        private readonly INewArticleService _newsService;

        public NewsHistoryModel(INewArticleService newsService)
        {
            _newsService = newsService;
        }

        public List<NewsArticle> NewsList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Authentication/Login");
            }

            NewsList = await _newsService.GetNewsByAuthorIdAsync(int.Parse(userId));

            return Page();
        }
    }
}
