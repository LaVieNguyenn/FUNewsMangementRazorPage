using Team_07_PRN222_A02.BLL.DTOs;
using Team_07_PRN222_A02.DAL.Models;
using Team_07_PRN222_A02.DAL.Repositories.AccountRepository;
using Team_07_PRN222_A02.DAL.UnitOfWork;
using Microsoft.Extensions.Configuration;

namespace Team_07_PRN222_A02.BLL.Services.SystemAccountService
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private SystemAccount Admin = new SystemAccount();
        public SystemAccountService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            this.Admin.AccountName = configuration["AdminAccount:AccountName"]!;
            this.Admin.AccountEmail = configuration["AdminAccount:Email"]!;
            this.Admin.AccountPassword = configuration["AdminAccount:Password"]!;
            this.Admin.AccountRole = 0;
        }
        public async Task<SystemAccount> Login(string email, string password)
        {
            if (this.Admin.AccountEmail == email && this.Admin.AccountPassword == password)
            {
                return this.Admin;
            }
            else
                return await _unitOfWork.AccountRepository.LoginAsync(email, password);
        }


        public async Task<SystemAccount> GetAccountById(int accountID) => await _unitOfWork.AccountRepository.GetByIdAsync(accountID);

        public async Task<IEnumerable<SystemAccount>> GetAllAccounts() => throw new NotImplementedException();

        public async Task<SystemAccount?> GetAccountByEmailAsync(string email)
        {
            return await _unitOfWork.AccountRepository.GetAccountByEmailAsync(email);
        }
        public async Task<SystemAccount?> GetAccountWithNewsHistoryAsync(string email)
        {
            return await _unitOfWork.AccountRepository.GetAccountByEmailAsync(email);
        }

        public async Task UpdateProfileAsync(SystemAccount account)
        {
            await _unitOfWork.AccountRepository.UpdateAccountAsync(account);
            await _unitOfWork.SaveChangesAsync();
        }

        Task ISystemAccountService.UpdateAccount(SystemAccount model)
        {
            throw new NotImplementedException();
        }

        Task ISystemAccountService.DeleteAccount(int accountId)
        {
            throw new NotImplementedException();
        }

        Task ISystemAccountService.AddAccount(SystemAccountDTOAdd model)
        {
            throw new NotImplementedException();
        }

        public Task<List<SystemAccountDTO>> GetAllAccountsAsync()
        {
            throw new NotImplementedException();
        }
    }
}