using Team_07_PRN222_A02.DAL.Models;
using Team_07_PRN222_A02.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Team_07_PRN222_A02.BLL.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        // Constructor to inject the repository
        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Get all top-level categories
        public async Task<IEnumerable<Category>> GetAllCategoryAsync()
        {
            return  _unitOfWork.CategoryRepository.GetAllAsync().ToList();
        }

        // Get all categories (including subcategories)
        public Task<IEnumerable<Category>> GetAllAsync()
        {
            throw new NotImplementedException();
            //return _repository.GetAllCategoriesAsync();
        }

        // Create a new category and return its CategoryID
        public async Task<int> CreateCategoryAsync(Category category)
        {
            throw new NotImplementedException();
            // Call the repository to create the category and get the new CategoryID
           // return await _repository.CreateCategoryAsync(category);
        }

        // Optionally implement this method if required
        public Task<IEnumerable<Category>> GetCategoryAsync()
        {
            // You can implement this if you need a method to get all categories with some specific logic
            throw new NotImplementedException();
        }
        public async Task DeleteCategoryAsync(int categoryId)
        {
            throw new NotImplementedException();
            //await _repository.DeleteCategoryAsync(categoryId);
        }
        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            throw new NotImplementedException();
            //return await _repository.GetCategoryByIdAsync(id); // Call the repository to get the category
        }
        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            throw new NotImplementedException();
            //return await _repository.UpdateCategoryAsync(category);
        }


    }
}
