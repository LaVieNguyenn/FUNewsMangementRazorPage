using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Team_07_PRN222_A02.BLL.Services.NewsArticleService;
using Team_07_PRN222_A02.BLL.DTOs;
using Team_07_PRN222_A02.BLL.Services.CategoryService;
using Microsoft.AspNetCore.SignalR;
using Team_07_PRN222_A02.Hubs;
using Team_07_PRN222_A02.DAL.Models;
using Team_07_PRN222_A02.DAL.Repositories.NotificationRepository;
using Team_07_PRN222_A02.BLL.Services.NotificationService;
using Microsoft.AspNetCore.Mvc.Rendering;

public class ManageNewsModel : PageModel
{
    private readonly INewArticleService _newsService;
    private readonly ICategoryService _categoryService;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly INotificationService _notificationService;

    public ManageNewsModel(INewArticleService newsService, ICategoryService categoryService, IHubContext<ChatHub> hubContext, IHttpContextAccessor httpContextAccessor, INotificationService notificationService)
    {
        _newsService = newsService;
        _categoryService = categoryService;
        _hubContext = hubContext;
        _httpContextAccessor = httpContextAccessor;
        _notificationService = notificationService;
    }

    public List<NewsArticleDTO> NewsList { get; set; } = new();
    public List<CategoryDTO> Categories { get; set; } = new();

    [BindProperty]
    public NewsArticleUpdateDTO NewArticle { get; set; } = new NewsArticleUpdateDTO();
    

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadDataAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAddAsync()
    {
        if (!ModelState.IsValid)
        {
            Console.WriteLine("❌ ModelState is invalid");
            await LoadDataAsync();
            return Page();
        }

        try
        {
            NewArticle.CreatedDate = DateTime.Now;
            NewArticle.CreatedById = 2;

            if (NewArticle.CategoryId <= 0)
            {
                Console.WriteLine("❌ Invalid CategoryId");
                ModelState.AddModelError(string.Empty, "Please select a valid category");
                await LoadDataAsync();
                return Page();
            }

            Console.WriteLine($"📝 Adding news: {NewArticle.NewsTitle}, Category: {NewArticle.CategoryId}");
            await _newsService.CreateNewsAsync(NewArticle);

            Console.WriteLine("✅ News added successfully!");
            await LoadDataAsync();


            var notification = new CreateNotificationDTO
            {
                Title = $"News {NewArticle.NewsTitle} Uploaded",
                Message = NewArticle.NewsContent,
                CreatedBy = _httpContextAccessor.HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value,
                CreatedAt = DateTime.Now,
                IsRead = false
            };

            await _notificationService.CreateNotification(notification);
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", notification.CreatedBy, notification.Title, notification.Message);

            return Page();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
            ModelState.AddModelError(string.Empty, $"Lỗi khi thêm bài viết: {ex.Message}");
            await LoadDataAsync();
            return Page();
        }
    }

    


    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        try
        {
            Console.WriteLine($"🗑 Deleting news ID: {id}");
            await _newsService.DeleteNewsAsync(id);
            Console.WriteLine("✅ News deleted successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Delete Error: {ex.Message}");
            ModelState.AddModelError(string.Empty, $"Lỗi khi xóa bài viết: {ex.Message}");
        }

        await LoadDataAsync();

        return Page();
    }


    private async Task LoadDataAsync()
    {
        NewsList = (await _newsService.GetAllNewestNewsAsync()).ToList();
        Categories = (await _categoryService.GetAllCategoryAsync()).ToList();
        Console.WriteLine($"🔄 Loaded {NewsList.Count} news articles & {Categories.Count} categories.");
    }
}
