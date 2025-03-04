using Team_07_PRN222_A02.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team_07_PRN222_A02.DAL.Repositories.CategoryRepository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly FunewsManagementContext _context;

        public CategoryRepository(FunewsManagementContext funewsManagementContext)
        {
            _context = funewsManagementContext;
        }

        public async Task DeleteAsync(Category obj) =>  _context.Categories.Remove(obj);

        public IQueryable<Category> GetAllAsync() => _context.Categories.AsQueryable();

        public Task<Category?> GetByIdAsync(int obj)
        {
            throw new NotImplementedException();
        }

        public Task InsertAsync(Category obj)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Category obj)
        {
            throw new NotImplementedException();
        }
    }
}
