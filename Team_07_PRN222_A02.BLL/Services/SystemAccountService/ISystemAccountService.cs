using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Team_07_PRN222_A02.BLL.DTOs;
using Team_07_PRN222_A02.DAL.Models;

namespace Team_07_PRN222_A02.BLL.Services.SystemAccountService
{
    public interface ISystemAccountService
    {
        Task<SystemAccount> Login(string email, string password);
        Task<SystemAccount> GetAccountById(int accountID);
        Task<IEnumerable<SystemAccount>> GetAllAccounts();
        Task DeleteAccount(int accountId);
        Task<SystemAccount?> GetAccountByEmailAsync(string email); // lay tk theo email
        Task UpdateProfileAsync(SystemAccount account); // cap nhat ho so
        Task<SystemAccount> GetAccountWithNewsHistoryAsync(string email);
        Task AddAccount(SystemAccountDTOAdd model);
        Task<List<SystemAccountDTO>> GetAllAccountsAsync();
        Task<bool> UpdateAccountAsync(SystemAccountDTO model);

        Task<SystemAccountDTO> GetAccountByIdAsync(int id);
        Task<bool> DeleteAccountAsync(int accountId);
        Task<bool> CreateAccountAsync(SystemAccountDTOAdd model);


    }
}
