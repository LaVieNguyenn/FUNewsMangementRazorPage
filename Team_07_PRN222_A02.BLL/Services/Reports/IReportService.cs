using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Team_07_PRN222_A02.BLL.DTOs;

namespace Team_07_PRN222_A02.BLL.Services
{
    public interface IReportService
    {
        Task<List<NewsArticleDTO>> GetReportByPeriodAsync(DateTime startDate, DateTime endDate);
        Task<List<string>> GetAllCategoriesAsync();

    }
}
