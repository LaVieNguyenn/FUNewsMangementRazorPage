using Team_07_PRN222_A02.DAL.Models;
using Team_07_PRN222_A02.DAL.Repositories.AccountRepository;
using Team_07_PRN222_A02.DAL.Repositories.CategoryRepository;
using Team_07_PRN222_A02.DAL.Repositories.NewsRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team_07_PRN222_A02.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FunewsManagementContext _context;

        private IAccountRepository? _accountRepository;
        private INewsRepository? _newArticleRepository;
        private ICategoryRepository? _categoryRepository;

        public UnitOfWork(FunewsManagementContext context)
        {
            _context = context;
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

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
