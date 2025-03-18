using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Team_07_PRN222_A02.DAL.Models;

namespace Team_07_PRN222_A02.DAL.Repositories.NotificationRepository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly FunewsManagementContext _context;

        public NotificationRepository(FunewsManagementContext context)
        {
            _context = context;
        }

        public Task DeleteAsync(Notification obj)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Notification> GetAllAsync() => _context.Notifications.AsQueryable();

        public Task<Notification?> GetByIdAsync(int obj)
        {
            throw new NotImplementedException();
        }

        public async Task InsertAsync(Notification obj) =>
           await _context.Notifications.AddAsync(obj);

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Notification obj)
        {
            throw new NotImplementedException();
        }
    }
}
