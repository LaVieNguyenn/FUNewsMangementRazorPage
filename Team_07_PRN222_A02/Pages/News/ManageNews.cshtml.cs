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
using Team_07_PRN222_A02.BLL.Services.NotificationService;
using Microsoft.AspNetCore.Http;

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
            await LoadDataAsync();
            return Page();
        }

        try
        {
            NewArticle.CreatedDate = DateTime.Now;
            NewArticle.CreatedById = 2;

            if (NewArticle.CategoryId <= 0)
            {
                ModelState.AddModelError(string.Empty, "Please select a valid category");
                await LoadDataAsync();
                return Page();
            }

            await _newsService.CreateNewsAsync(NewArticle);
            await LoadDataAsync();
            await NotifyNewArticleUpload();

            return Page();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Error adding article: {ex.Message}");
            await LoadDataAsync();
            return Page();
        }
    }

    public async Task<IActionResult> OnPostEditAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadDataAsync();
            return Page();
        }

        try
        {
            await _newsService.UpdateNewsAsync(NewArticle); // Sử dụng NewArticle cho việc cập nhật  
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Error editing article: {ex.Message}");
        }

        await LoadDataAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        try
        {
            await _newsService.DeleteNewsAsync(id);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Error deleting article: {ex.Message}");
        }

        await LoadDataAsync();
        return Page();
    }

    private async Task LoadDataAsync()
    {
        NewsList = (await _newsService.GetAllNewestNewsAsync()).ToList();
        Categories = (await _categoryService.GetAllCategoryAsync()).ToList();
    }

    private async Task NotifyNewArticleUpload()
    {
        var notification = new CreateNotificationDTO
        {
            Title = $"News {NewArticle.NewsTitle} Uploaded",
            Message = NewArticle.NewsContent,
            CreatedBy = _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value,
            CreatedAt = DateTime.Now,
            IsRead = false
        };

        await _notificationService.CreateNotification(notification);
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", notification.CreatedBy, notification.Title, notification.Message);
    }
}
