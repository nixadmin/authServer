using AuthServerDemo.Data.Entities;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

namespace AuthServerDemo.Data.Repository
{
    public interface IApplicationUserRepository
    {
        void Add(ApplicationUser user);

        void AddRange(IEnumerable<ApplicationUser> users);

        ApplicationUser GetById(int id);

        ApplicationUser GetByUserName(string userName);
    }

    public class ApplicationUserRedisRepository : IApplicationUserRepository
    {
        private const string KEY_ID_SUFFIX = "USER_ID_";
        private const string KEY_NAME_SUFFIX = "USER_NAME_";

        private RedisConnection connection;

        public ApplicationUserRedisRepository(RedisConnection redisCconnection)
        {
            connection = redisCconnection;
        }

        public void Add(ApplicationUser user)
        {
            var serializedUser = JsonConvert.SerializeObject(user);

            connection.Database.StringSet(KEY_ID_SUFFIX + user.Id.ToString(), serializedUser);
            connection.Database.StringSet(KEY_NAME_SUFFIX + user.UserName.ToUpper(), serializedUser);
        }

        public void AddRange(IEnumerable<ApplicationUser> users)
        {
            foreach (ApplicationUser user in users)
            {
                this.Add(user);
            }
        }

        public ApplicationUser GetById(int id)
        {
            return JsonConvert.DeserializeObject<ApplicationUser>(connection.Database.StringGet(KEY_ID_SUFFIX + id.ToString()));
        }

        public ApplicationUser GetByUserName(string userName)
        {
            return JsonConvert.DeserializeObject<ApplicationUser>(connection.Database.StringGet(KEY_NAME_SUFFIX + userName.ToUpper()));
        }
    }
}
