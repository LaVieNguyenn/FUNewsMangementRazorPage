using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Team_07_PRN222_A02.BLL.DTOs;
using Team_07_PRN222_A02.BLL.Services;

namespace Team_07_PRN222_A02.Pages
{
    public class ReportModel : PageModel
    {
        private readonly IReportService _reportService;

        public ReportModel(IReportService reportService)
        {
            _reportService = reportService;
        }

        [BindProperty]
        public DateTime StartDate { get; set; }

        [BindProperty]
        public DateTime EndDate { get; set; }

        public List<NewsArticleDTO> ReportData { get; set; } = new();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (StartDate > EndDate)
            {
                ModelState.AddModelError("", "Ngày bắt đầu phải nhỏ hơn ngày kết thúc.");
                return Page();
            }

            ReportData = await _reportService.GetReportByPeriodAsync(StartDate, EndDate);
            return Page();
        }
    }
}
