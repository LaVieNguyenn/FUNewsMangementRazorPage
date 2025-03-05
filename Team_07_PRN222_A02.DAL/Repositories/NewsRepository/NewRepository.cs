using Team_07_PRN222_A02.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team_07_PRN222_A02.DAL.Repositories.NewsRepository
{
    public class NewRepository : INewsRepository
    {
        private readonly FunewsManagementContext _context;

        public NewRepository(FunewsManagementContext funewsManagementContext)
        {
            _context = funewsManagementContext;
        }

        public Task DeleteAsync(NewsArticle obj)
        {
            throw new NotImplementedException();
        }

        public IQueryable<NewsArticle> GetAllAsync() =>  _context.NewsArticles.AsQueryable();

        public Task<NewsArticle?> GetByIdAsync(int obj)
        {
            throw new NotImplementedException();
        }

        public Task InsertAsync(NewsArticle obj)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(NewsArticle obj)
        {
            throw new NotImplementedException();
        }
    }
}
