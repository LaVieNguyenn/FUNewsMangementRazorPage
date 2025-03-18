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
            Console.WriteLine($"📝 Đang thêm bài viết: {newsArticle.NewsTitle}, Category: {newsArticle.CategoryId}");

            var entity = _mapper.Map<NewsArticle>(newsArticle);

            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Dữ liệu bài viết không hợp lệ.");

            entity.Headline = newsArticle.Headline;
            entity.NewsSource = newsArticle.NewsSource;
            entity.NewsStatus = newsArticle.NewsStatus.HasValue ? newsArticle.NewsStatus.Value : (byte)1;

            await _unitOfWork.NewsArticleRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            Console.WriteLine("✅ Bài viết đã được thêm thành công!");
        }

        public async Task UpdateNewsAsync(NewsArticleUpdateDTO newsArticle)
        {
            var existingArticle = await _unitOfWork.NewsArticleRepository.GetByIdAsync(newsArticle.Id);
            if (existingArticle != null)
            {
                _mapper.Map(newsArticle, existingArticle);
                await _unitOfWork.NewsArticleRepository.UpdateAsync(existingArticle);
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

        public async Task<List<NewsArticleDTO>> GetNewsByAuthorIdAsync(int authorId)
        {
            Console.WriteLine($"🔍 Fetching news for author ID: {authorId}");

            var newsList = await _unitOfWork.NewsArticleRepository
                .GetAllAsync()
                .Where(n => n.CreatedById == authorId)
                .OrderByDescending(n => n.CreatedDate)
                .Select(n => new NewsArticleDTO
                {
                    NewsArticleId = n.NewsArticleId,
                    NewsTitle = n.NewsTitle,
                    CreatedDate = n.CreatedDate
                })
                .ToListAsync();

            if (!newsList.Any())
            {
                Console.WriteLine($"⚠ No news found for author ID: {authorId}");
                return new List<NewsArticleDTO>();
            }

            Console.WriteLine($"✅ {newsList.Count} articles found for author ID: {authorId}");
            return newsList;
        }

        public async Task<NewsArticleDTO> GetNewsAsyncById(int id)
        {
            var news = await _unitOfWork.NewsArticleRepository.GetByIdAsync(id);
            return new NewsArticleDTO
            {
                NewsArticleId = id,
                AccountName = news.CreatedBy.AccountName,
                CreatedDate = news.CreatedDate,
                CategoryId = news.CategoryId.ToString(),
                CategoryName = news.Category.CategoryName,
                Headline = news.Headline,
                NewsContent = news.NewsContent,
                NewsSource = news.NewsSource,
                NewsStatus = news.NewsStatus,
                NewsTitle = news.NewsTitle,
                Tags = news.Tags,
            };
        }

        public async Task<IEnumerable<TagDTO>> GetAllTagsAsync()
        {
            if (_unitOfWork.TagRepository == null)
            {
                throw new NullReferenceException("TagRepository chưa được khởi tạo!");
            }

            var tags = await _unitOfWork.TagRepository.GetAllTagsAsync();
            if (tags == null)
            {
                return new List<TagDTO>();
            }

            return _mapper.Map<List<TagDTO>>(tags);
        }


    }
}