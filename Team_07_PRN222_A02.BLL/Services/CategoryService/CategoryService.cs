using Team_07_PRN222_A02.DAL.Models;
using Team_07_PRN222_A02.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Team_07_PRN222_A02.BLL.DTOs;
using AutoMapper;


namespace Team_07_PRN222_A02.BLL.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;

        // Constructor to inject the repository
        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        // Get all top-level categories
        public async Task<IEnumerable<CategoryDTO>> GetAllCategoryAsync()
        {
            var categories =  _unitOfWork.CategoryRepository.GetAllAsync();
            var testCategories = _unitOfWork.CategoryRepository.GetAllAsync();
            Console.WriteLine($"Test GetAllCategoryAsync: {testCategories?.Count()} categories");

            return categories.Select(n => this.mapper.Map<CategoryDTO>(n)).ToList();
        }



        // Get all categories (including subcategories)
        public Task<IEnumerable<Category>> GetAllAsync()
        {
            throw new NotImplementedException();
            //return _repository.GetAllCategoriesAsync();
        }

        // Create a new category and return its CategoryID
        public async Task<int> CreateCategoryAsync(Category obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "Category cannot be null.");
            }

            await _unitOfWork.CategoryRepository.InsertAsync(obj);
            await _unitOfWork.CategoryRepository.SaveChangesAsync();

            return obj.CategoryId; // ✅ Trả về ID sau khi thêm vào database
        }




        // Optionally implement this method if required
        public Task<IEnumerable<Category>> GetCategoryAsync()
        {
            // You can implement this if you need a method to get all categories with some specific logic
            throw new NotImplementedException();
        }
        public async Task DeleteCategoryAsync(int categoryId)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryId);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with ID {categoryId} not found.");
            }

            await _unitOfWork.CategoryRepository.DeleteAsync(category);
            await _unitOfWork.CategoryRepository.SaveChangesAsync();
        }

       public async Task<CategoryDTO> GetCategoryByIdAsync(int id)
{
    var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);

    if (category == null)
    {
        throw new KeyNotFoundException($"Category with ID {id} not found.");
    }

    return mapper.Map<CategoryDTO>(category); // Dùng AutoMapper để chuyển đổi sang DTO
}



        public async Task UpdateCategoryAsync(Category category)
        {
            try
            {
                _unitOfWork.CategoryRepository.UpdateAsync(category);
                await _unitOfWork.SaveChangesAsync(); // ✅ Chỉ gọi SaveChangesAsync() một lần duy nhất
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating category: {ex.Message}");
                throw; // ✅ Rethrow để log lỗi chính xác hơn
            }
        }

    }
}
