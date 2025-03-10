using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Team_07_PRN222_A02.BLL.Services.NewsArticleService;
using Team_07_PRN222_A02.BLL.DTOs;

public class ManageNewsModel : PageModel
{
    private readonly INewArticleService _newsService;

    public ManageNewsModel(INewArticleService newsService)
    {
        _newsService = newsService;
    }

    public List<NewsArticleDTO> NewsList { get; set; }
    public NewsArticleUpdateDTO NewArticle { get; set; } = new NewsArticleUpdateDTO();

    public async Task<IActionResult> OnGetAsync()
    {
        NewsList = (await _newsService.GetAllNewestNewsAsync()).ToList();
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        await _newsService.DeleteNewsAsync(id);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostAddAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        await _newsService.CreateNewsAsync(NewArticle);
        return RedirectToPage();
    }
}
