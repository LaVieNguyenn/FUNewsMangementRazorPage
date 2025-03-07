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

        public async Task DeleteAsync(Category obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "Category cannot be null.");
            }

            _context.Categories.Remove(obj);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Category> GetAllAsync() => _context.Categories.AsQueryable();

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }


        public async Task InsertAsync(Category obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "Category cannot be null.");
            }

            await _context.Categories.AddAsync(obj); 
            await _context.SaveChangesAsync(); 

        }


        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync(Category obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "Category cannot be null.");
            }

             _context.Categories.Update(obj);
        }

    }
}
