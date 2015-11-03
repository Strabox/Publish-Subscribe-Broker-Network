using Broker.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Broker.Ordering
{
    public class FifoOrdering : IOrder
    {

        private class Publisher
        {
            private string publisherId;
            public string PublisherId { get { return publisherId; } }

            private Queue<int> sequenceNumbers;

            public Publisher(string publisherId,int sequenceNumber)
            {
                this.publisherId = publisherId;
                this.sequenceNumbers = new Queue<int>();
                sequenceNumbers.Enqueue(sequenceNumber);
            }

            public int GetFirst()
            {
                return sequenceNumbers.Peek();
            }

            public void DequeueLast()
            {
                sequenceNumbers.Dequeue();
            }
            
            public void Enqueue(int sn)
            {
                sequenceNumbers.Enqueue(sn);
            }

        }

        private Dictionary<string, Publisher> controlOrder;

        public FifoOrdering()
        {
            controlOrder = new Dictionary<string, FifoOrdering.Publisher>();
        }


        public void AddNewMessage(string publisherId,int messageSequenceNumber)
        {
            lock (this)
            {
                if(!controlOrder.ContainsKey(publisherId))
                {
                    controlOrder.Add(publisherId, new Publisher(publisherId,messageSequenceNumber));
                }
                else
                {
                    lock (controlOrder[publisherId])
                    {
                        controlOrder[publisherId].Enqueue(messageSequenceNumber);
                    }
                }
            }
        }

        public void ConfirmDeliver(string publisherId)
        {
            lock (controlOrder[publisherId])
            {
                controlOrder[publisherId].DequeueLast();
                Monitor.PulseAll(controlOrder[publisherId]);
            }
        }

        public void Deliver(string publisherId,int messageSequenceNumber)
        {
            lock (controlOrder[publisherId])
            {
                
                while (messageSequenceNumber > controlOrder[publisherId].GetFirst())
                    Monitor.Wait(controlOrder[publisherId]);
            }
        }

    }
}
