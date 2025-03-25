using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data.KafkaEvents
{
    public class BaseEvent
    {
        public int Id { get; set; }
        public required string Guid { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
