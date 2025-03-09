using System;
using System.Collections.Generic;

namespace Team_07_PRN222_A02.BLL.DTOs
{
    public class ReportViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<NewsArticleDTO> Articles { get; set; } = new();
    }
}
