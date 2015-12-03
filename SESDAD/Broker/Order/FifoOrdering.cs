using Broker.Order;
using System.Collections.Generic;
using System.Threading;
using System;
using CommonTypes;

namespace Broker.Ordering
{
    public class FifoOrdering : IOrder
    {

        private class Publisher
        {
            private string publisherId;
            public string PublisherId { get { return publisherId; } }

            private Queue<int> sequenceNumbers;

            int recentMessage;

            public Publisher(string publisherId,int sequenceNumber)
            {
                this.publisherId = publisherId;
                this.recentMessage = sequenceNumber;
                this.sequenceNumbers = new Queue<int>();
                sequenceNumbers.Enqueue(sequenceNumber);
            }

            public int GetRecent()
            {
                return recentMessage;
            }

            public void SetRecent(int sequenceNumber)
            {
                recentMessage = sequenceNumber;
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


        public Boolean AddNewMessage(string publisherId,int messageSequenceNumber)
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
                        if (messageSequenceNumber <= controlOrder[publisherId].GetRecent())
                        {
                            return true;
                        }
                        else
                        {
                            controlOrder[publisherId].SetRecent(messageSequenceNumber);
                            controlOrder[publisherId].Enqueue(messageSequenceNumber);
                        }
                    }
                }
            }
            return false;
        }

        public void ConfirmDeliver(Event e)
        {
            String publisherId = e.Publisher;
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

        public bool HasMessage(string id, int seq)
        {
            return false;
        }

        public bool FreezeBludgerIfNeeded(Bludger bludger)
        {
           return false;
        }

        public bool FreezeSequencerIfNeeded(Bludger bludger)
        {
            throw new NotImplementedException();
        }
    }
}
