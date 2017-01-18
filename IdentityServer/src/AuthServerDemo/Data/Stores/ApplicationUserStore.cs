using AuthServerDemo.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using AuthServerDemo.Data.Repository;

namespace AuthServerDemo.Data.Stores
{
    public interface IApplicationUserStore
    {
        IApplicationUserRepository UsersRepository { get; }

        Task<bool> ValidateCredentialsAsync(string username, string password);

        Task<ApplicationUser> FindByUsernameAsync(string username);

        Task<ApplicationUser> FindByIdAsync(int id);
    }

    public class ApplicationUserStore : IApplicationUserStore
    {
        protected internal IPasswordHasher<ApplicationUser> PasswordHasher { get; set; }

        public IApplicationUserRepository UsersRepository { get; private set; }

        public ApplicationUserStore(IPasswordHasher<ApplicationUser> passwordHasher, IApplicationUserRepository repo)
        {
            this.UsersRepository = repo;
            this.PasswordHasher = passwordHasher;            
        }

        public async Task<bool> ValidateCredentialsAsync(string userName, string password)
        {
            var user = await FindByUsernameAsync(userName);
            if (user != null)
            {
                var verificationResult = this.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
                return await Task.FromResult(verificationResult != PasswordVerificationResult.Failed);
            }

            return false;
        }

        public async Task<ApplicationUser> FindByUsernameAsync(string userName)
        {
            return await UsersRepository.GetByUserNameAsync(userName);
        }

        public async Task<ApplicationUser> FindByIdAsync(int id)
        {
            return await UsersRepository.GetByIdAsync(id);
        }
    }    
}