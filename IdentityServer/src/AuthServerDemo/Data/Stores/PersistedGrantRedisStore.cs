using IdentityServer4.Stores;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using AuthServerDemo.Data.Repository;

namespace AuthServerDemo.Data.Stores
{
    public class PersistedGrantRedisStore : IPersistedGrantStore
    { 
        public IGrantRepository Tokens { get; private set; }

        public PersistedGrantRedisStore(IGrantRepository tokenConnections)
        {
            this.Tokens = tokenConnections;
        }

        public Task StoreAsync(PersistedGrant grant)
        {
            Tokens.Add(grant);

            return Task.FromResult(0);
        }

        public Task<PersistedGrant> GetAsync(string key)
        {
            try
            {
                return Task.FromResult(Tokens.GetByKey(key));
            }
            catch
            {
                return Task.FromResult<PersistedGrant>(null);
            }
        }

        public Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
            return Task.FromResult(Tokens.GetBySubject(subjectId));
        }

        public Task RemoveAsync(string key)
        {
            return Task.FromResult(0);
        }

        public Task RemoveAllAsync(string subjectId, string clientId)
        {
            var query =
                from item in _repository
                where item.Value.ClientId == clientId &&
                    item.Value.SubjectId == subjectId
                select item.Key;

            var keys = query.ToArray();
            foreach (var key in keys)
            {
                PersistedGrant grant;
                _repository.TryRemove(key, out grant);
            }

            return Task.FromResult(0);
        }

        public Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            var query =
                from item in _repository
                where item.Value.SubjectId == subjectId &&
                    item.Value.ClientId == clientId &&
                    item.Value.Type == type
                select item.Key;

            var keys = query.ToArray();
            foreach (var key in keys)
            {
                PersistedGrant grant;
                _repository.TryRemove(key, out grant);
            }

            return Task.FromResult(0);
        }
    }
}