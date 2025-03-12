using Team_07_PRN222_A02.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Team_07_PRN222_A02.DAL.Repositories.NewsRepository
{
    public class NewRepository : INewsRepository
    {
        private readonly FunewsManagementContext _context;

        public NewRepository(FunewsManagementContext funewsManagementContext)
        {
            _context = funewsManagementContext;
        }

        public async Task DeleteAsync(NewsArticle obj)
        {
            _context.NewsArticles.Remove(obj);
            await _context.SaveChangesAsync();
        }

        public IQueryable<NewsArticle> GetAllAsync() => _context.NewsArticles.AsQueryable();

        public async Task<NewsArticle?> GetByIdAsync(int id)
        {
            return await _context.NewsArticles.FindAsync(id);
        }

        public async Task InsertAsync(NewsArticle obj)
        {
            await _context.NewsArticles.AddAsync(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(NewsArticle obj)
        {
            _context.NewsArticles.Update(obj);
            await _context.SaveChangesAsync();
        }
        public async Task AddAsync(NewsArticle obj)
        {
            await _context.NewsArticles.AddAsync(obj);
            await _context.SaveChangesAsync();
        }
        public async Task<List<NewsArticle>> GetNewsByAuthorIdAsync(int authorId)
        {
            return await _context.NewsArticles
                .Where(n => n.CreatedById == authorId)
                .ToListAsync();
        }

    }
}
