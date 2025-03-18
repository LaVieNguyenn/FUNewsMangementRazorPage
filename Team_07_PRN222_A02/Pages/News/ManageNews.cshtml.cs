using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Team_07_PRN222_A02.BLL.DTOs;
using Team_07_PRN222_A02.BLL.Services.CategoryService;
using Team_07_PRN222_A02.BLL.Services.NewsArticleService;
using Team_07_PRN222_A02.BLL.Services.NotificationService;
using Team_07_PRN222_A02.Hubs;
using Team_07_PRN222_A02.DAL.Models;

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
    public List<Tag> Tags { get; set; } = new(); // ✅ Initialize list to avoid null reference issues

    [BindProperty]
    public NewsArticleUpdateDTO NewArticle { get; set; } = new();

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

            // Ensure Tag selection is processed properly
            await _newsService.CreateNewsAsync(NewArticle);

            await LoadDataAsync();
            await NotifyNewArticleUpload();

            ViewData["SuccessMessage"] = "News article added successfully!";
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
            await _newsService.UpdateNewsAsync(NewArticle);
            ViewData["SuccessMessage"] = "News article updated successfully!";
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "News article updated successfully!");
            ViewData["ErrorMessage"] = "News article updated successfully!";
        }

        await LoadDataAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        try
        {
            await _newsService.DeleteNewsAsync(id);
            ViewData["SuccessMessage"] = "News article deleted successfully!";
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Error deleting article: {ex.Message}");
            ViewData["ErrorMessage"] = $"Error deleting article: {ex.Message}";
        }

        await LoadDataAsync();
        return Page();
    }

    private async Task LoadDataAsync()
    {
        NewsList = (await _newsService.GetAllNewestNewsAsync()).ToList();
        Categories = (await _categoryService.GetAllCategoryAsync()).ToList();

        // Convert TagDTOs to Tags
        var tagDTOs = await _newsService.GetAllTagsAsync();
        Tags = tagDTOs?.Select(tagDto => new Tag
        {
            TagId = tagDto.TagId,
            TagName = tagDto.TagName
        }).ToList() ?? new List<Tag>();
    }

    private async Task NotifyNewArticleUpload()
    {
        var userName = _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value ?? "Unknown User";

        var notification = new CreateNotificationDTO
        {
            Title = $"News {NewArticle.NewsTitle} Uploaded",
            Message = NewArticle.NewsContent,
            CreatedBy = userName,
            CreatedAt = DateTime.Now,
            IsRead = false
        };

        await _notificationService.CreateNotification(notification);
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", notification.CreatedBy, notification.Title, notification.Message);
    }
}
