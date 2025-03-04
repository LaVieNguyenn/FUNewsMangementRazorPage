using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team_07_PRN222_A02.BLL.DTOs
{
    public class NewsArticleUpdateDTO
    {
        public int? NewsArticleId { get; set; }

        public string? NewsTitle { get; set; } = null!;

        public string? Headline { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string NewsContent { get; set; } = null!;

        public string? NewsSource { get; set; }

        public int? CategoryId { get; set; }

        public string? NewsStatus { get; set; } = null!;

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }
    }
}
