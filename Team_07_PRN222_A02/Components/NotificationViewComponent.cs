using Microsoft.AspNetCore.Mvc;
using Team_07_PRN222_A02.BLL.Services.NotificationService;

namespace Team_07_PRN222_A02.Components
{
    public class NotificationViewComponent : ViewComponent
    {
        private readonly INotificationService _notificationService;

        public NotificationViewComponent(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var notifications = await _notificationService.GetAllNotification();
            return View(notifications);
        }
    }
}
