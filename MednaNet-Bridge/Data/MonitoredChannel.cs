using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MednaNet_Bridge.Data
{
    public class MonitoredChannel
    {
        public string discordChannelId { get; set; }
        public int channelId { get; set; }
        public string channelName { get; set; }
        public int lastMessageId { get; set; }

    }
}
