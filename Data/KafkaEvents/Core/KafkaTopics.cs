using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseService.Data.Models;

namespace DatabaseService.Data.KafkaEvents
{
    public static class KafkaTopics
    {
        public class User 
        {
            public const string Created = "user.created";
            public const string Updated = "user.updated";
            public const string Deleted = "user.deleted";
        }

        public class Client
        {
            public const string Created = "client.created";
            public const string Updated = "client.updated";
            public const string Deleted = "client.deleted";
        }
    }

}
