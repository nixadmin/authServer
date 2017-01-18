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

        ApplicationUser FindByUsername(string username);

        ApplicationUser FindById(int id);
    }

    public class ApplicationUserStore : IApplicationUserStore
    {
        protected internal IPasswordHasher<ApplicationUser> PasswordHasher { get; set; }

        public IApplicationUserRepository UsersRepository { get; private set; }

        public ApplicationUserStore(UserManager<ApplicationUser> identityUserSotore, IPasswordHasher<ApplicationUser> passwordHasher, IApplicationUserRepository repo)
        {
            this.UsersRepository = repo;
            this.UsersRepository.AddRange(identityUserSotore.Users);

            this.PasswordHasher = passwordHasher;            
        }

        public async Task<bool> ValidateCredentialsAsync(string userName, string password)
        {
            var user = FindByUsername(userName);
            if (user != null)
            {
                var verificationResult = this.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
                return await Task.FromResult(verificationResult != PasswordVerificationResult.Failed);
            }

            return false;
        }

        public ApplicationUser FindByUsername(string userName)
        {
            return UsersRepository.GetByUserName(userName);
        }

        public ApplicationUser FindById(int id)
        {
            return UsersRepository.GetById(id);
        }
    }    
}