using CommonTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Broker.Order
{
    class TotalOrdering : IOrder
    {
        private Dictionary<string, Queue<int>> messages;

        private DetectRepeatedMessageNO detectRepeatedMessages;

        private BrokerLogic broker;

        private Queue<Bludger> sequencers;
        private Queue<Bludger> bludgers;

        public TotalOrdering(BrokerLogic broker)
        {
            messages = new Dictionary<string, Queue<int>>();
            detectRepeatedMessages = new DetectRepeatedMessageNO();
            this.broker = broker;
            this.sequencers = new Queue<Bludger>();
            this.bludgers = new Queue<Bludger>();
        }


        public Boolean AddNewMessage(string publisherId, int messageSequenceNumber)
        {
            
            lock (this)
            {
                if (detectRepeatedMessages.IsRepeated(messageSequenceNumber, publisherId))
                {
                    return true;
                }
                if (!messages.ContainsKey(publisherId))
                {
                    messages.Add(publisherId, new Queue<int>());
                }
               
                lock (messages[publisherId])
                {
                    messages[publisherId].Enqueue(messageSequenceNumber);
                }
                
            }
            return false;
        }

        public void ConfirmDeliver(Event e)
        {
            string publisherId = e.Publisher;
            lock (messages[publisherId])
            {
                int dequed = messages[publisherId].Dequeue();
                Monitor.PulseAll(messages[publisherId]);
            }
            lock (this)
            { 
                while (this.sequencers.Count() > 0 && !HasMessage(this.sequencers.Peek()))
                {
                    this.broker.DoSequence(sequencers.Dequeue());
                }

                while (this.bludgers.Count() > 0 && !HasMessage(this.bludgers.Peek()))
                {
                    this.broker.DoBludger(bludgers.Dequeue());
                }
            }
            // If this is the first broker, we start a Sequence request
            if (e.Sender.Equals(this.broker.SiteName))
            {
                Bludger bludger = new Bludger(e.Publisher, e.Topic, e.SequenceNumber);
                this.broker.Sequence(bludger);
            }

        }

        public void Deliver(string publisherId, int messageSequenceNumber)
        {
            lock (messages[publisherId])
            {
                while (messageSequenceNumber > messages[publisherId].Peek())
                    Monitor.Wait(messages[publisherId]);
            }
        }

        public bool HasMessage(string id, int seq)
        {
            if (messages.ContainsKey(id))
            {
                return messages[id].Contains(seq);
            }
            return false;
        }

        private bool HasMessage(Bludger bludger)
        {
            return HasMessage(bludger.Publisher, bludger.Sequence);
        }

        public bool FreezeSequencerIfNeeded(Bludger bludger)
        {
            lock (this)
            {
                if (this.HasMessage(bludger.Publisher, bludger.Sequence) || sequencers.Count > 0)
                {
                    sequencers.Enqueue(bludger);
                    return true;
                }
                return false;
            }
        }

        public bool FreezeBludgerIfNeeded(Bludger bludger)
        {
            lock(this)
            {
                if (this.HasMessage(bludger.Publisher, bludger.Sequence) || bludgers.Count > 0)
                {
                    bludgers.Enqueue(bludger);
                    return true;
                }
                return false;
            }
        }
    }
}