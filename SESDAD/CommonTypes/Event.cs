using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes
{
    public class Event
    {

        private string topic;

        private string content;

        public Event(string topic,string content)
        {
            this.topic = topic;
            this.content = content;
        }

    }
}
