using Team_07_PRN222_A02.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team_07_PRN222_A02.DAL.Repositories.AccountRepository
{
    public interface IAccountRepository : IGenericRepository<SystemAccount>
    {
        Task<SystemAccount?> LoginAsync(string username, string password);
        Task<SystemAccount?> GetAccountByEmailAsync(string email);
        Task<SystemAccount> GetAccountById(int accountID);
        Task UpdateAccountAsync(SystemAccount account);
        IQueryable<SystemAccount> GetAllAsync();
    }
}