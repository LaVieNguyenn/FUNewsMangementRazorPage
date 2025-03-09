using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Team_07_PRN222_A02.DAL.Repositories.NewsRepository;
using Team_07_PRN222_A02.BLL.DTOs;

namespace Team_07_PRN222_A02.BLL.Services
{
    public class ReportService : IReportService
    {
        private readonly INewsRepository _newsRepository;

        public ReportService(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<List<NewsArticleDTO>> GetReportByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            var newsArticles = await _newsRepository
                .GetAllAsync()
                .Where(n => n.CreatedDate >= startDate && n.CreatedDate <= endDate)
                .OrderByDescending(n => n.CreatedDate)
                .Select(n => new NewsArticleDTO
                {
                    NewsArticleId = n.NewsArticleId,
                    NewsTitle = n.NewsTitle,
                    Headline = n.Headline,
                    CreatedDate = n.CreatedDate,
                    NewsContent = n.NewsContent,
                    NewsSource = n.NewsSource,
                    CategoryName = n.Category.CategoryName,
                    
                })
                .ToListAsync();

            return newsArticles;
        }
    }
}
