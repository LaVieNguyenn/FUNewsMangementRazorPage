public class CategoryDTO
{
    public int Id => CategoryID; // Tạo property phụ để giữ nguyên code cũ
    public string Name => CategoryName; // Giúp tránh lỗi undefined

    public int CategoryID { get; set; }
    public string CategoryName { get; set; } = null!;
    public string? CategoryDescription { get; set; }
    public int? ParentCategoryID { get; set; }
    public bool IsActive { get; set; }
}
