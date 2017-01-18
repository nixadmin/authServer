using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServerDemo.Data
{
    public class RedisConnection
    {
        public IDatabase Database { get; private set; }

        public RedisConnection(string host)
        {
            Database = ConnectionMultiplexer.Connect(host).GetDatabase();
        }
    }
}
