using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Team_07_PRN222_A02.BLL.DTOs;
using Team_07_PRN222_A02.BLL.Services.NewsArticleService;

namespace Team_07_PRN222_A02.Pages.News
{
    public class NewsDetailModel : PageModel
    {
        private readonly INewArticleService _newsService;

        public NewsDetailModel(INewArticleService newsService)
        {
            _newsService = newsService;
        }

        public NewsArticleDTO News { get; set; } = new NewsArticleDTO();


        public async Task<IActionResult> OnGetAsync(int id)
        {
            News = await _newsService.GetNewsAsyncById(id);
            if (News == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
