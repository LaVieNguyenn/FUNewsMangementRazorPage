using Team_07_PRN222_A02.DAL.Repositories.AccountRepository;
using Team_07_PRN222_A02.DAL.Repositories.CategoryRepository;
using Team_07_PRN222_A02.DAL.Repositories.NewsRepository;

namespace Team_07_PRN222_A02.DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        IAccountRepository AccountRepository { get; }
        INewsRepository NewsArticleRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
