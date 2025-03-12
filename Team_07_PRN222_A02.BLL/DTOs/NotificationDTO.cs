using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team_07_PRN222_A02.BLL.DTOs
{
    public class NotificationDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public bool? IsRead { get; set; } = false;
        public string? CreatedBy { get; set; }
    }
}
