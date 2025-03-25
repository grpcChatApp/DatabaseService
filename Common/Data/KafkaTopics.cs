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
            public const string Create = "user.create";
            public const string Update = "user.update";
            public const string Delete = "user.delete";
        }

        public class Client
        {
            public const string Create = "client.create";
            public const string Delete = "client.delete";
        }
    }

}
