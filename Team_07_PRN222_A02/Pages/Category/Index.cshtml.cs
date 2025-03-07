using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Team_07_PRN222_A02.BLL.DTOs;
using Team_07_PRN222_A02.BLL.Services.CategoryService;
using Team_07_PRN222_A02.DAL.Models;

namespace Team_07_PRN222_A02.Pages.Category
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        public IEnumerable<CategoryDTO> Categories { get; set; }
        public List<Team_07_PRN222_A02.DAL.Models.Category> Category { get; set; } = new();

        [BindProperty]
        public Team_07_PRN222_A02.DAL.Models.Category NewCategory { get; set; } = new();


        [BindProperty(SupportsGet = true)]
        public Team_07_PRN222_A02.DAL.Models.Category EditCategory { get; set; } = new();


        public SelectList ParentCategories { get; set; }

        public IndexModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task OnGetAsync()
        {
            Categories = await _categoryService.GetAllCategoryAsync();

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
            Console.WriteLine($"DELEID: {id}");
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            Console.WriteLine($"DELEID: {id}");
            await _categoryService.DeleteCategoryAsync(category.CategoryID); // Thiếu await
            return RedirectToPage("Index"); // Redirect về danh sách category
        }

    }
}
