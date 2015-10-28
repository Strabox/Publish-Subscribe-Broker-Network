using CommonTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscriber
{
    public interface OrderGuarantee
    {
        void DeliverMessage(IMessage m,DeliverMessage d);
    }

    public delegate void DeliverMessage(Object o);

    /* Guarantee FIFO ordering in messages, according with each
       publisher.
    */
    public class FifoOrdering : OrderGuarantee
    {
        class WaitingMessages
        {
            private string id;

            private int nextSequenceNumber;

            private List<IMessage> messages;

            public WaitingMessages(string id)
            {
                this.id = id;
                nextSequenceNumber = 0;
                messages = new List<IMessage>();
            }


            public void DeliverMessages(IMessage m, DeliverMessage del)
            {
                lock (this)
                {
                    Console.WriteLine("Delivering {0} from {1}",m.GetSequenceNumber(),id);
                    if (!(m.GetSequenceNumber() == nextSequenceNumber))
                    {
                        messages.Add(m);
                        return;
                    }
                    messages.Sort(delegate (IMessage x, IMessage y)
                    {
                        return x.GetSequenceNumber().CompareTo(y.GetSequenceNumber());
                    });
                    del(m);
                    int i = ++nextSequenceNumber;
                    foreach (IMessage mTemp in messages)
                    {
                        if (mTemp.GetSequenceNumber() != i)
                        {
                            break;
                        }
                        else
                        {
                            del(mTemp);
                            i++;
                        }
                    }
                    if(i != nextSequenceNumber)
                        messages.RemoveRange(0,i - nextSequenceNumber);
                    nextSequenceNumber = i;
                }
            }
        }

        private Dictionary<string, WaitingMessages> messages;

        public FifoOrdering()
        {
            messages = new Dictionary<string, WaitingMessages>();
        }


        public void DeliverMessage(IMessage m,DeliverMessage del)
        {
            lock(this)
            {
                Console.WriteLine(m.GetSequenceNumber());
                if (!messages.ContainsKey(m.GetId()))
                    messages.Add(m.GetId(), new WaitingMessages(m.GetId()));
            }
            messages[m.GetId()].DeliverMessages(m, del);
        }
    }

    // Guarantee absolute no order in message delivering
    public class NoOrder : OrderGuarantee
    {
        public void DeliverMessage(IMessage m, DeliverMessage d)
        {
            d(m);
        }
    }

}
