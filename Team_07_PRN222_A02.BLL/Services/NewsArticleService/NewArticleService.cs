using AutoMapper;
using Team_07_PRN222_A02.BLL.DTOs;
using Team_07_PRN222_A02.DAL.Models;
using Team_07_PRN222_A02.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Team_07_PRN222_A02.BLL.Services.NewsArticleService
{
    

    public class NewArticleService : INewArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NewArticleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NewsArticleDTO>> GetAllNewestNewsAsync()
        {
            var newsArticles = await _unitOfWork.NewsArticleRepository
                .GetAllAsync()
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.Tags)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();

            return _mapper.Map<IEnumerable<NewsArticleDTO>>(newsArticles);
        }

        public async Task<IEnumerable<NewsArticleDTO>> GetAllNewestNewsByCategoryNameAsync(string categoryName, int max)
        {
            var newsArticles = await _unitOfWork.NewsArticleRepository
                .GetAllAsync()
                .Include(n => n.Category)
                .Where(n => n.Category.CategoryName == categoryName)
                .OrderByDescending(n => n.CreatedDate)
                .Take(max)
                .ToListAsync();

            return _mapper.Map<IEnumerable<NewsArticleDTO>>(newsArticles);
        }

        public async Task CreateNewsAsync(NewsArticleUpdateDTO newsArticle)
        {
            var entity = _mapper.Map<NewsArticle>(newsArticle);
            await _unitOfWork.NewsArticleRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateNewsAsync(NewsArticleUpdateDTO newsArticle)
        {
            var existingArticle = await _unitOfWork.NewsArticleRepository.GetByIdAsync(newsArticle.Id);
            if (existingArticle != null)
            {
                _mapper.Map(newsArticle, existingArticle);
                _unitOfWork.NewsArticleRepository.UpdateAsync(existingArticle);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task DeleteNewsAsync(int id)
        {
            var article = await _unitOfWork.NewsArticleRepository.GetByIdAsync(id);
            if (article != null)
            {
                await _unitOfWork.NewsArticleRepository.DeleteAsync(article);
            }
        }


        public async Task<List<NewsArticle>> GetNewsByAuthorIdAsync(int authorId)
        {
            return await _unitOfWork.NewsArticleRepository
                .GetAllAsync()
                .Where(n => n.CreatedById == authorId)
                .ToListAsync();
        }
    }
}
