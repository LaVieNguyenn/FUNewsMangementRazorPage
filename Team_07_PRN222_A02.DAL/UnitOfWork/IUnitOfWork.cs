using Team_07_PRN222_A02.DAL.Repositories.AccountRepository;
using Team_07_PRN222_A02.DAL.Repositories.CategoryRepository;
using Team_07_PRN222_A02.DAL.Repositories.NewsRepository;
using Team_07_PRN222_A02.DAL.Repositories.NotificationRepository;
using Team_07_PRN222_A02.DAL.Repositories.TagRepository;

namespace Team_07_PRN222_A02.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IAccountRepository AccountRepository { get; }
        INewsRepository NewsArticleRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        INotificationRepository NotificationRepository { get; }
        ITagRepository TagRepository { get; }

        Task<int> SaveChangesAsync();
        void BeginTransaction();
        void CommitTransaction();
        void RollBack();
    }
}
