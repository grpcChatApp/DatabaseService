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
        public string ClientId { get; private set; }

        public ClientEvent(Guid referenceId, string eventName, string clientId) : base(referenceId.ToString(), eventName)
        {
            ClientId = clientId;
        }
    }
}
