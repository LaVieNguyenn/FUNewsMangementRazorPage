using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Team_07_PRN222_A02.BLL.DTOs;
using Team_07_PRN222_A02.BLL.Services;

namespace Team_07_PRN222_A02.Pages.Report
{
    [Authorize(Roles = "Admin")]
    public class ReportModel : PageModel
    {
        private readonly IReportService _reportService;

        public ReportModel(IReportService reportService)
        {
            _reportService = reportService ?? throw new ArgumentNullException(nameof(reportService));
        }

        public Dictionary<string, int> CategoryDistribution { get; private set; } = new();

        [BindProperty]
        public DateTime StartDate { get; set; } = DateTime.Today.AddDays(-7);

        [BindProperty]
        public DateTime EndDate { get; set; } = DateTime.Today;

        [BindProperty]
        public string Category { get; set; }

        public List<string> Categories { get; private set; } = new();
        public List<NewsArticleDTO> ReportData { get; private set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Categories = await _reportService.GetAllCategoriesAsync();
                ReportData = await _reportService.GetReportByPeriodAsync(StartDate, EndDate);
                CalculateCategoryDistribution();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Lỗi khi tải báo cáo: {ex.Message}");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (StartDate > EndDate)
            {
                ModelState.AddModelError("", "Ngày bắt đầu phải nhỏ hơn ngày kết thúc.");
                return Page();
            }

            try
            {
                var allData = await _reportService.GetReportByPeriodAsync(StartDate, EndDate);
                ReportData = !string.IsNullOrEmpty(Category)
                    ? allData.Where(a => a.CategoryName == Category).ToList()
                    : allData;

                Categories = await _reportService.GetAllCategoriesAsync();
                CalculateCategoryDistribution();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Lỗi khi lấy dữ liệu báo cáo: {ex.Message}");
            }

            return Page();
        }

        private void CalculateCategoryDistribution()
        {
            CategoryDistribution = ReportData
                .GroupBy(a => a.CategoryName)
                .ToDictionary(g => g.Key, g => g.Count());

            ViewData["CategoryLabels"] = CategoryDistribution.Keys.ToArray();
            ViewData["CategoryCounts"] = CategoryDistribution.Values.ToArray();
        }
    }
}
