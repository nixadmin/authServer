using IdentityServer4.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServerDemo.Data.Repository
{
    public interface IGrantRepository
    {
        void Add(PersistedGrant token);

        void AddRange(IEnumerable<PersistedGrant> tokens);

        PersistedGrant GetByKey(string key);

        IEnumerable<PersistedGrant> GetBySubject(string subject);
    }

    public class GrantRedisRepository : IGrantRepository
    {
        private const string TOKEN = "TOKEN";
        private const string SUBJECT = "SUBJECT";

        private RedisConnection connection;

        public GrantRedisRepository(RedisConnection redisCconnection)
        {
            connection = redisCconnection;
        }

        public void Add(PersistedGrant token)
        {
            var serToken = JsonConvert.SerializeObject(token);

            connection.Database.StringSet(TOKEN + token.Key, serToken);
            connection.Database.StringSet(SUBJECT + token.SubjectId, serToken);
        }

        public void AddRange(IEnumerable<PersistedGrant> tokens)
        {
            foreach (PersistedGrant token in tokens)
            {
                this.Add(token);
            }
        }

        public PersistedGrant GetByKey(string key)
        {
            return JsonConvert.DeserializeObject<PersistedGrant>(connection.Database.StringGet(TOKEN + key));
        }

        public IEnumerable<PersistedGrant> GetBySubject(string subject)
        {
            // should return enumerable of records insted of one, compilation goint to fail. Need to fix it
            return JsonConvert.DeserializeObject<PersistedGrant>(connection.Database.StringGet(SUBJECT + subject));
        }
    }
}
