using System;
using System.Collections.Generic;

namespace CommonTypes
{
    public class DetectRepeatedFIFO : IDetectMessagesRepeated
    {

        class Publisher
        {
            private string id;
            public string Id { get { return id; } }

            private int lastMessageSn;
            public int LastMessageSN
            {
                get { return lastMessageSn; }
                set { lastMessageSn = value; }
            }

            public Publisher(string id,int lastSn)
            {
                this.id = id;
                this.lastMessageSn = lastSn;
            }

        }

        private Dictionary<string, Publisher> lastMessages;

        public DetectRepeatedFIFO()
        {
            lastMessages = new Dictionary<string, Publisher>();
        }

        public Boolean IsRepeated(int sequenceNumber,string publisherId)
        {
            lock (this)
            {
                if (!lastMessages.ContainsKey(publisherId))
                {
                    lastMessages.Add(publisherId, new Publisher(publisherId, sequenceNumber));
                    return false;
                }
                else
                {
                    if (sequenceNumber <= lastMessages[publisherId].LastMessageSN)
                        return true;
                    else
                    {
                        lastMessages[publisherId].LastMessageSN = sequenceNumber;
                        return false;
                    }
                }
            }
        }


    }
}
