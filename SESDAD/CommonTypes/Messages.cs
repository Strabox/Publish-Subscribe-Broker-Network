using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes
{
    [Serializable]
    public class Subscription
    {

        private string topic;

        public Subscription(string topic)
        {
            this.topic = topic;
        }

    }

    [Serializable]
    public class Messages
    {
        private string topic;

        private string content;

        public Messages(string topic,string content)
        {
            this.topic = topic;
            this.content = content;
        }

    }
}
