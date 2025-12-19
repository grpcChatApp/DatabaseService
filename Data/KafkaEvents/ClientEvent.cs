using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseService.Data.KafkaEvents;

namespace Common.Data.KafkaEvents
{
    class ClientEvent : BaseEvent
    {
        public ClientEvent(string referenceId, string eventName, string clientId, string clientSecret) : base(referenceId, eventName)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
        }

        public string ClientId { get; private set; }
        public string ClientSecret { get; private  set; }
    }
}
