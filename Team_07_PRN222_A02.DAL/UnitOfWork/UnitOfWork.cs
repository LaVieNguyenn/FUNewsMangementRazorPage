using Team_07_PRN222_A02.DAL.Models;
using Team_07_PRN222_A02.DAL.Repositories.AccountRepository;
using Team_07_PRN222_A02.DAL.Repositories.CategoryRepository;
using Team_07_PRN222_A02.DAL.Repositories.NewsRepository;
using Team_07_PRN222_A02.DAL.Repositories.NotificationRepository;
using Team_07_PRN222_A02.DAL.Repositories.TagRepository;

namespace Team_07_PRN222_A02.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FunewsManagementContext _context;

        private IAccountRepository? _accountRepository;
        private INewsRepository? _newArticleRepository;
        private ICategoryRepository? _categoryRepository;
        private INotificationRepository? _notificationRepository;
        private ITagRepository? _tagRepository;

        private bool disposed = false;
        public UnitOfWork(FunewsManagementContext context)
        {
            _context = context;
        }
        public ITagRepository TagRepository
        {
            get
            {
                if (_tagRepository == null)
                {
                    _tagRepository = new TagRepository(_context);
                }
                return _tagRepository;
            }
        }

        public IAccountRepository AccountRepository
        {
            get
            {
                if (_accountRepository == null)
                {
                    _accountRepository = new AccountRepository(_context);
                }
                return _accountRepository;
            }
        }
        public INewsRepository NewsArticleRepository
        {
            get
            {
                if (_newArticleRepository == null)
                {
                    _newArticleRepository = new NewRepository(_context);
                }
                return _newArticleRepository;
            }
        }
        public ICategoryRepository CategoryRepository
        {
            get
            {
                if (_categoryRepository == null)
                {
                    _categoryRepository = new CategoryRepository(_context);
                }
                return _categoryRepository;
            }
        }

        public INotificationRepository NotificationRepository
        {
            get
            {
                if (_notificationRepository == null)
                {
                    _notificationRepository = new NotificationRepository(_context);
                }
                return _notificationRepository;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public void BeginTransaction()
        {
            _context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _context.Database.CommitTransaction();
        }

        public void RollBack()
        {
            _context.Database.RollbackTransaction();
        }
    }
}
