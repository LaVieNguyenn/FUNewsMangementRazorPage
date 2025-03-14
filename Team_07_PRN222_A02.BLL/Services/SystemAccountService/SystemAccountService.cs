    using Team_07_PRN222_A02.BLL.DTOs;
    using Team_07_PRN222_A02.DAL.Models;
    using Team_07_PRN222_A02.DAL.Repositories.AccountRepository;
    using Team_07_PRN222_A02.DAL.UnitOfWork;
    using Microsoft.Extensions.Configuration;
    using Microsoft.EntityFrameworkCore;

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
            public async Task<SystemAccountDTO> GetAccountByIdAsync(int id)
            {
                var account = await _unitOfWork.AccountRepository.GetByIdAsync(id);
                if (account == null)
                {
                    return null;
                }
                return new SystemAccountDTO
                {
                    AccountID = account.AccountId,
                    AccountName = account.AccountName,
                    AccountEmail = account.AccountEmail,
                    AccountRole = account.AccountRole,
                };
            }

        public async Task<bool> UpdateAccountAsync(SystemAccountDTO model)
        {
            try
            {
                var account = await _unitOfWork.AccountRepository.GetByIdAsync(model.AccountID);
                if (account == null)
                {
                    Console.WriteLine($"⚠ Account with ID {model.AccountID} not found.");
                    return false;
                }

                // Cập nhật thông tin tài khoản
                account.AccountName = model.AccountName;
                account.AccountEmail = model.AccountEmail;
                account.AccountRole = model.AccountRole;

                // Thực hiện cập nhật
                await _unitOfWork.AccountRepository.UpdateAsync(account);
                await _unitOfWork.SaveChangesAsync();

                Console.WriteLine($"✅ Account {account.AccountId} updated successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error in UpdateAccountAsync: " + ex.Message);
                return false;
            }
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