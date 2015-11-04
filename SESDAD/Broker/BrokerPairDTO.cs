using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker
{
    public class BrokerPairDTO
    {
        private string url;
        public string Url { get { return url; } }

        private string logicName;
        public string LogicName { get { return logicName; } }

        public BrokerPairDTO(string url, string logicName)
        {
            this.url = url;
            this.logicName = logicName;
        }
    }
}
