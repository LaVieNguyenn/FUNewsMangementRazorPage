using Team_07_PRN222_A02.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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

        public  IQueryable<SystemAccount> GetAllAsync()
        {
            return _context.SystemAccounts.AsQueryable();
        }

        public async Task<SystemAccount?> GetByIdAsync(int obj) => await _context.SystemAccounts.FirstOrDefaultAsync(a => a.AccountId == obj);

        public async Task InsertAsync(SystemAccount obj) => await _context.SystemAccounts.AddAsync(obj);

        public async Task<SystemAccount?> LoginAsync(string username, string password) => await _context.SystemAccounts.FirstOrDefaultAsync(a => a.AccountEmail == username && a.AccountPassword == password);

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task UpdateAsync(SystemAccount obj) => _context.SystemAccounts.Update(obj);

    }                                                                                                                                    
}
