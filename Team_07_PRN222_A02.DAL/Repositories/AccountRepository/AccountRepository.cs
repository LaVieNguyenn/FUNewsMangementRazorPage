using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Team_07_PRN222_A02.DAL.Models;

namespace Team_07_PRN222_A02.DAL.Repositories.AccountRepository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly FunewsManagementContext _context;

        public AccountRepository(FunewsManagementContext funewsManagementContext)
        {
            _context = funewsManagementContext;
        }
        public async Task DeleteAsync(SystemAccount obj) => _context.SystemAccounts.Remove(obj);

        public IQueryable<SystemAccount> GetAllAsync()
        {
            return _context.SystemAccounts.AsQueryable();
        }

        public async Task<SystemAccount?> GetByIdAsync(int obj) => await _context.SystemAccounts.FirstOrDefaultAsync(a => a.AccountId == obj);

        public async Task InsertAsync(SystemAccount obj) => await _context.SystemAccounts.AddAsync(obj);

        public async Task<SystemAccount?> LoginAsync(string username, string password) => await _context.SystemAccounts.FirstOrDefaultAsync(a => a.AccountEmail == username && a.AccountPassword == password);

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task UpdateAsync(SystemAccount obj) => _context.SystemAccounts.Update(obj);
        public async Task UpdateAccount(SystemAccount obj)                                                                          
        {
            _context.SystemAccounts.Update(obj);
            await _context.SaveChangesAsync(); 
        }
        public async Task<SystemAccount?> GetByEmailAsync(string email)
        {
            return await _context.SystemAccounts
                                 .FirstOrDefaultAsync(a => a.AccountEmail == email);
        }


        public async Task<SystemAccount?> GetAccountByEmailAsync(string email)
        {
            return await _context.SystemAccounts
                .FirstOrDefaultAsync(a => a.AccountEmail == email);
        }

        public async Task<SystemAccount> GetAccountById(int accountID)
        {
            return await _context.SystemAccounts.FindAsync(accountID);
        }

        public async Task UpdateAccountAsync(SystemAccount account)
        {
            _context.SystemAccounts.Update(account);
            await _context.SaveChangesAsync();
        }

    }
}
