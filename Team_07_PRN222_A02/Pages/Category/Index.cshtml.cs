    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using Team_07_PRN222_A02.BLL.DTOs;
    using Team_07_PRN222_A02.BLL.Services.CategoryService;
    using Team_07_PRN222_A02.BLL.Services.NewsArticleService;
    using Team_07_PRN222_A02.DAL.Models;

    namespace Team_07_PRN222_A02.Pages.Category
    {
        public class IndexModel : PageModel
        {
            private readonly ICategoryService _categoryService;
            private readonly INewArticleService _newsArticleService;
            public IEnumerable<CategoryDTO> Categories { get; set; }
            public IEnumerable<NewsArticleDTO> NewsArticles { get; set; }
            public List<Team_07_PRN222_A02.DAL.Models.Category> Category { get; set; } = new();
            public List<Team_07_PRN222_A02.DAL.Models.NewsArticle> newsArticle { get; set; } = new();

            [BindProperty]
            public Team_07_PRN222_A02.DAL.Models.Category NewCategory { get; set; } = new();


            [BindProperty(SupportsGet = true)]
            public Team_07_PRN222_A02.DAL.Models.Category EditCategory { get; set; } = new();


            public SelectList ParentCategories { get; set; }

            public IndexModel(ICategoryService categoryService, INewArticleService newArticleService)
            {
                _categoryService = categoryService;
                _newsArticleService = newArticleService;
            }

            public async Task OnGetAsync()
            {
                Categories = await _categoryService.GetAllCategoryAsync();
            NewsArticles = await _newsArticleService.GetAllNewestNewsAsync();
            if (Categories == null)
                {
                    Console.WriteLine("❌ Categories is NULL!");
                    Categories = new List<CategoryDTO>();
                }
                else if (!Categories.Any())
                {
                    Console.WriteLine("⚠ Categories is EMPTY!");
                }
            
            // Fix lỗi `null`
            ParentCategories = new SelectList(Categories ?? new List<CategoryDTO>(), "CategoryId", "CategoryName");
            }



            public async Task<IActionResult> OnPostCreateAsync()
            {
                // Gán thủ công giá trị vào CategoryName
                if (string.IsNullOrWhiteSpace(NewCategory.CategoryName))
                {
                    NewCategory.CategoryName = "DefaultCategory";  // Hoặc giá trị nào đó hợp lý
                }

                Console.WriteLine($"Category Name: {NewCategory.CategoryName}");
                Console.WriteLine($"Description: {NewCategory.CategoryDescription}");
                Console.WriteLine($"Parent ID: {NewCategory.ParentCategoryId}");
                Console.WriteLine($"IsActive: {NewCategory.IsActive}");



            await _categoryService.CreateCategoryAsync(NewCategory);
                return RedirectToPage();
            }


            public async Task<IActionResult> OnPostEditAsync()
            {
                if (!ModelState.IsValid)
                {
                
                    ParentCategories = new SelectList(Categories, "CategoryId", "CategoryName");
                    return Page();
                }

                if (EditCategory == null)
                {
                    Console.WriteLine("❌ EditCategory is NULL!");
                    return Page();
                }

           Console.WriteLine($"🔄 Updating category: {EditCategory.CategoryId} - {EditCategory.CategoryName}");
                await _categoryService.UpdateCategoryAsync(EditCategory);


                return RedirectToPage(); // Chuyển trang để tránh reload lỗi
            }



        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            if (id <= 0)
            {
                ModelState.AddModelError(string.Empty, "Invalid category ID.");
                return Page();
            }

            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                ModelState.AddModelError(string.Empty, "Category not found.");
                return Page();
            }

            // Lấy danh sách bài báo
            var newsArticles = await _newsArticleService.GetAllNewestNewsAsync();
            bool isInUse = newsArticles.Any(n => n.CategoryId == id.ToString());

            if (isInUse)
            {
                ModelState.AddModelError(string.Empty, "Cannot delete this category because it is being used in news articles.");
                return Page(); // Trả về trang hiện tại, không xóa
            }

            await _categoryService.DeleteCategoryAsync(id);
            return RedirectToPage("Index");
        }


        public async Task<IActionResult> OnGetNewArticle()
            {
                var category = await _newsArticleService.GetAllNewestNewsAsync();
                return RedirectToPage("Index");
            }
        }
    }
