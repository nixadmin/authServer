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
using StackExchange.Redis;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace AuthServerDemo.Data.Stores
{
    public interface IInMemoryApplicationUserStore
    {
        ConcurrentApplicationUserCollection Users { get; }

        Task<bool> ValidateCredentialsAsync(string username, string password);

        ApplicationUser FindByUsername(string username);

        ApplicationUser FindById(int id);

        /*ApplicationUser GetUserByExpression(Func<ApplicationUser, bool> expesion);

        IEnumerable<ApplicationUser> GetUsersByExpression(Func<ApplicationUser, bool> expesion);*/
    }

    public class InMemoryApplicationUserStore : IInMemoryApplicationUserStore
    {
        protected internal IPasswordHasher<ApplicationUser> PasswordHasher { get; set; }

        public ConcurrentApplicationUserCollection Users { get; private set; }

        public InMemoryApplicationUserStore(UserManager<ApplicationUser> identityUserSotore, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            this.Users = new ConcurrentApplicationUserCollection();
            this.Users.AddRange(identityUserSotore.Users);

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
            return Users[username];
        }

        public ApplicationUser FindById(int id)
        {
            return Users[id];
        }

        /*public ApplicationUser GetUserByExpression(Func<ApplicationUser, bool> expesion)
        {
            return Users.FirstOrDefault(expesion);
        }

        public IEnumerable<ApplicationUser> GetUsersByExpression(Func<ApplicationUser, bool> expesion)
        {
            return Users.Where(expesion);
        }*/
    }

    public class ConcurrentApplicationUserCollection
    {
        private const string KEY_ID_SUFFIX = "USER_ID_";
        private const string KEY_NAME_SUFFIX = "USER_NAME_";

        private ConnectionMultiplexer connection;
        private IDatabase database;

        public ConcurrentApplicationUserCollection()
        {
            this.connection = ConnectionMultiplexer.Connect("localhost");
            database = connection.GetDatabase();
        }

        public void Add(ApplicationUser user)
        {
            var serializedUser = JsonConvert.SerializeObject(user);

            database.StringSet(KEY_ID_SUFFIX + user.Id.ToString(), serializedUser);
            database.StringSet(KEY_NAME_SUFFIX + user.UserName.ToUpper(), serializedUser);
        }

        public void AddRange(IEnumerable<ApplicationUser> users)
        {
            foreach (ApplicationUser user in users)
            {
                this.Add(user);
            }
        }

        public ApplicationUser this[int id]
        {
            get
            {
                return JsonConvert.DeserializeObject<ApplicationUser>(database.StringGet(KEY_ID_SUFFIX + id.ToString()));
            }
        }

        public ApplicationUser this[string userName]
        {
            get
            {
                return JsonConvert.DeserializeObject<ApplicationUser>(database.StringGet(KEY_NAME_SUFFIX + userName.ToUpper()));
            }
        }
    }
}