using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Team_07_PRN222_A02.BLL.DTOs;
using Team_07_PRN222_A02.BLL.Services.CategoryService;
using Team_07_PRN222_A02.BLL.Services.NewsArticleService;

namespace Team_07_PRN222_A02.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly INewArticleService _newsArticleService;
        private readonly ICategoryService _categoryService;
        public IEnumerable<NewsArticleDTO> news { get; set; }
        public IndexModel(ILogger<IndexModel> logger, INewArticleService newArticleService, ICategoryService categoryService)
        {
            _logger = logger;
            _newsArticleService = newArticleService;
            _categoryService = categoryService;
        }

        public async Task OnGetAsync()
        {
            news = await _newsArticleService.GetAllNewestNewsAsync();
        }

        
    }
}
