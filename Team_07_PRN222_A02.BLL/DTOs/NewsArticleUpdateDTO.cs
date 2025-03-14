using System;

namespace Team_07_PRN222_A02.BLL.DTOs
{
    public class NewsArticleUpdateDTO
    {
        public int Id { get; set; }
        public int? NewsArticleId { get; set; }
        public string? NewsTitle { get; set; }
        public string? Headline { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? NewsContent { get; set; } = null!;
        public string? NewsSource { get; set; }
        public int? CategoryId { get; set; }
        public byte? NewsStatus { get; set; } 
        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }
    }
}
