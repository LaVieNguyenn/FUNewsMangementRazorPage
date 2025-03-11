
using Team_07_PRN222_A02.BLL.DTOs;
using Team_07_PRN222_A02.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Team_07_PRN222_A02.BLL.Services.NewsArticleService
{
    public interface INewArticleService
    {
        Task<IEnumerable<NewsArticleDTO>> GetAllNewestNewsAsync();
        Task<IEnumerable<NewsArticleDTO>> GetAllNewestNewsByCategoryNameAsync(string categoryName, int max);

        Task CreateNewsAsync(NewsArticleUpdateDTO newsArticle);
        Task UpdateNewsAsync(NewsArticleUpdateDTO newsArticle);
        Task DeleteNewsAsync(int id);

        Task<List<NewsArticle>> GetNewsByAuthorIdAsync(int authorId);
    }
}