
using Team_07_PRN222_A02.BLL.DTOs;
using Team_07_PRN222_A02.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Team_07_PRN222_A02.BLL.Services.NotificationService
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDTO>> GetAllNotification();

        public Task CreateNotification(CreateNotificationDTO createNotificationDTO);
    }
}