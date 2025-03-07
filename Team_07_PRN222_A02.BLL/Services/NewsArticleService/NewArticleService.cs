using AutoMapper;
using Team_07_PRN222_A02.BLL.DTOs;
using Team_07_PRN222_A02.DAL.Models;
using Team_07_PRN222_A02.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Team_07_PRN222_A02.BLL.Services.NewsArticleService
{
    public class NewArticleService : INewArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;
        public NewArticleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public Task CreateNews(NewsArticleUpdateDTO newsArticle)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<NewsArticleDTO>> GetAllNewestNewsAsync()
        {
            return _unitOfWork.NewsArticleRepository.GetAllAsync()
                                                            .Include(n => n.Category)
                                                            .Include(n => n.CreatedBy)
                                                            .Include(n => n.Tags)
                                                            .OrderByDescending(n => n.CreatedDate)
                                                            .Select(n => this.mapper.Map<NewsArticleDTO>(n))
                                                            .ToList(); 
        }

        public Task<IEnumerable<NewsArticleDTO>> GetAllNewestNewsByCategoryNameAsync(string categoryName, int max)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<NewsArticle>> GetNewsArticlesAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateNews(NewsArticleUpdateDTO newsArticle)
        {
            throw new NotImplementedException();
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
