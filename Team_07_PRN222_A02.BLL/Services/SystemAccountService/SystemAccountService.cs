using Team_07_PRN222_A02.BLL.DTOs;
using Team_07_PRN222_A02.DAL.Models;
using Team_07_PRN222_A02.DAL.Repositories.AccountRepository;
using Team_07_PRN222_A02.DAL.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Team_07_PRN222_A02.BLL.Services.SystemAccountService
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private SystemAccount Admin = new SystemAccount();
        public SystemAccountService(IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
            var account = await _unitOfWork.AccountRepository.GetAccountByEmailAsync(email);
            if (account == null)
            {
                Console.WriteLine($"⚠ Account with email {email} not found!");
            }
            return account;
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
        public async Task<SystemAccountDTO?> GetAccountByIdAsync(int id)
        {
            var account = await _unitOfWork.AccountRepository.GetByIdAsync(id);
            return account == null ? null : _mapper.Map<SystemAccountDTO>(account);
        }

        public async Task<bool> UpdateAccountAsync(SystemAccountDTO model)
        {
            var account = await _unitOfWork.AccountRepository.GetAccountByEmailAsync(model.AccountEmail);
            if (account == null)
            {
                Console.WriteLine($"⚠ Account with email {model.AccountEmail} not found!");
                return false;
            }

            _mapper.Map(model, account);
            await _unitOfWork.AccountRepository.UpdateAccountAsync(account);
            var rowsAffected = await _unitOfWork.SaveChangesAsync();
            Console.WriteLine($"🔍 DEBUG: Rows affected: {rowsAffected}");

            return rowsAffected > 0;
        }
        public async Task<SystemAccountDTO?> GetCurrentUserProfileAsync(string email)
        {
            var account = await _unitOfWork.AccountRepository.GetAccountByEmailAsync(email);
            if (account == null)
            {
                Console.WriteLine($"⚠ Account with email {email} not found!");
                return null;
            }

            // Đảm bảo không dùng cache (nếu có)
            var freshAccount = await _unitOfWork.AccountRepository.GetAccountByEmailAsync(email); // Lấy lại từ DB
            return _mapper.Map<SystemAccountDTO>(freshAccount);
        }

        Task ISystemAccountService.DeleteAccount(int accountId)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> DeleteAccountAsync(int accountId)
        {
            var account = await _unitOfWork.AccountRepository.GetByIdAsync(accountId);
            if (account == null)
            {
                return false;
            }

            await _unitOfWork.AccountRepository.DeleteAsync(account);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }


        Task ISystemAccountService.AddAccount(SystemAccountDTOAdd model)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SystemAccountDTO>> GetAllAccountsAsync()
        {
            var accounts = await _unitOfWork.AccountRepository.GetAllAsync().ToListAsync();

            return accounts.Select(a => new SystemAccountDTO
            {
                AccountID = a.AccountId,
                AccountName = a.AccountName,
                AccountEmail = a.AccountEmail,
                AccountRole = a.AccountRole
            }).ToList();
        }
        public async Task<bool> CreateAccountAsync(SystemAccountDTOAdd model)
        {
            var existingAccount = await _unitOfWork.AccountRepository.GetByEmailAsync(model.AccountEmail);
            if (existingAccount != null)
            {
                return false;
            }
            var account = new SystemAccount
            {
                AccountName = model.AccountName,
                AccountEmail = model.AccountEmail,
                AccountPassword = model.AccountPassword,
                AccountRole = model.AccountRole
            };
            await _unitOfWork.AccountRepository.InsertAsync(account);
            return await _unitOfWork.SaveChangesAsync() > 0;

        }
    }
}