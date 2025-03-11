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
            Categories = await _reportService.GetAllCategoriesAsync();
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

                if (!string.IsNullOrEmpty(Category))
                {
                    ReportData = allData.Where(a => a.CategoryName == Category).ToList();
                }
                else
                {
                    ReportData = allData;
                }

                Categories = await _reportService.GetAllCategoriesAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Lỗi khi lấy dữ liệu báo cáo: {ex.Message}");
            }

            return Page();
        }
    }
}
