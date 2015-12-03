using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes
{
    public class DetectRepeatedMessageNO : IDetectMessagesRepeated
    {

        private Dictionary<string, HashSet<int>> messages;


        public DetectRepeatedMessageNO()
        {
            messages = new Dictionary<string, HashSet<int>>();
        }

        public Boolean IsRepeated(int sequenceNumber,string publisherId)
        {
            lock (messages)
            {
                if (!messages.ContainsKey(publisherId))
                {
                    messages.Add(publisherId, new HashSet<int>());
                }
            }
            lock (messages[publisherId])
            {
                if (messages[publisherId].Contains(sequenceNumber))
                {
                    return true;
                }
                else
                {
                    messages[publisherId].Add(sequenceNumber);
                    return false;
                }
            }
        }

    }
}
