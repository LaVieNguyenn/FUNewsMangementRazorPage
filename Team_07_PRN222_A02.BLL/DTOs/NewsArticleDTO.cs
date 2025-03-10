using Team_07_PRN222_A02.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team_07_PRN222_A02.BLL.DTOs
{
    public class NewsArticleDTO
    {

        public int NewsArticleId { get; set; }

        public string NewsTitle { get; set; } = null!;

        public string? Headline { get; set; }

        public DateTime CreatedDate { get; set; }

        public string NewsContent { get; set; } = null!;

        public string? NewsSource { get; set; }

        public string CategoryName { get; set; }

        public string CategoryId { get; set; }
        public string AccountName
        {
            get; set;
        }

        public string? NewsStatus { get; set; }

        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}