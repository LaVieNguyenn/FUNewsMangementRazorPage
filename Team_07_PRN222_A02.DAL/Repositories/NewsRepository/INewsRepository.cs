﻿using Team_07_PRN222_A02.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team_07_PRN222_A02.DAL.Repositories.NewsRepository
{
    public interface INewsRepository : IGenericRepository<NewsArticle>
    {
        Task AddAsync(NewsArticle entity);
        Task UpdateAsync(NewsArticle obj);
        Task DeleteAsync(NewsArticle entity);
        Task<List<NewsArticle>> GetNewsByAuthorIdAsync(int authorId);


    }
}
