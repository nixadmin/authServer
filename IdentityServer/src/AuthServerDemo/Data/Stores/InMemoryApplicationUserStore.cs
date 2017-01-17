using AuthServerDemo.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4;

namespace AuthServerDemo.Data.Stores
{
    public interface IInMemoryApplicationUserStore
    {
        ConcurrentDictionary<int, ApplicationUser> Users { get; }

        Task<bool> ValidateCredentialsAsync(string username, string password);

        ApplicationUser FindByUsername(string username);

        ApplicationUser FindById(int id);

        ApplicationUser GetUserByExpression(Func<KeyValuePair<int, ApplicationUser>, bool> expesion);

        IEnumerable<ApplicationUser> GetUsersByExpression(Func<KeyValuePair<int, ApplicationUser>, bool> expesion);
    }

    public class InMemoryApplicationUserStore : IInMemoryApplicationUserStore
    {
        public ConcurrentDictionary<int, ApplicationUser> Users { get; private set; }

        protected internal IPasswordHasher<ApplicationUser> PasswordHasher { get; set; }

        public InMemoryApplicationUserStore(UserManager<ApplicationUser> identityUserSotore, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            this.Users = new ConcurrentDictionary<int, ApplicationUser>(identityUserSotore.Users.ToDictionary(q => q.Id));
            this.PasswordHasher = passwordHasher;
        }

        public async Task<bool> ValidateCredentialsAsync(string username, string password)
        {
            var user = FindByUsername(username);
            if (user != null)
            {
                var verificationResult = this.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
                return await Task.FromResult(verificationResult != PasswordVerificationResult.Failed);
            }

            return false;
        }

        public ApplicationUser FindByUsername(string username)
        {
            return Users.FirstOrDefault(x => x.Value.UserName.Equals(username, StringComparison.OrdinalIgnoreCase)).Value;
        }

        public ApplicationUser FindById(int id)
        {
            return Users[id];
        }

        public ApplicationUser GetUserByExpression(Func<KeyValuePair<int, ApplicationUser>, bool> expesion)
        {
            return Users.FirstOrDefault(expesion).Value;
        }

        public IEnumerable<ApplicationUser> GetUsersByExpression(Func<KeyValuePair<int, ApplicationUser>, bool> expesion)
        {
            return Users.Where(expesion).Select(q => q.Value);
        }
    }
}