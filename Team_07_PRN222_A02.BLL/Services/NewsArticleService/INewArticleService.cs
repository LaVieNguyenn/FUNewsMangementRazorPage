using Team_07_PRN222_A02.BLL.DTOs;
using Team_07_PRN222_A02.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Team_07_PRN222_A02.BLL.Services.NewsArticleService
{
    public interface INewArticleService
    {
        public Task<IEnumerable<NewsArticle>> GetNewsArticlesAsync();
        public Task<IEnumerable<NewsArticleDTO>> GetAllNewestNewsAsync();
        public Task<IEnumerable<NewsArticleDTO>> GetAllNewestNewsByCategoryNameAsync(string categoryName, int max);
        Task CreateNews(NewsArticleUpdateDTO newsArticle);
        Task UpdateNews(NewsArticleUpdateDTO newsArticle);
        Task Delete(int id);
        Task<List<NewsArticle>> GetNewsByAuthorIdAsync(int authorId);

    }
}
