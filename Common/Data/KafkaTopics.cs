using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data
{
    public static class KafkaTopics
    {
        public class User 
        {
            public const string Registered = "user.registered";
            public const string Updated = "user.updated";
        }

        public class Client
        {
            public const string Registered = "client.registered";
            public const string Disposed = "client.disposed";
        }
    }

}
