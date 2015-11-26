using CommonTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Order
{
    class NoOrdering : IOrder
    {
        public void AddNewMessage(string publisherId, int msgSequenceNumber)
        {
            //Do Nothing
        }

        public void ConfirmDeliver(Event e)
        {
            //Do nothing
        }

        public void Deliver(string publisherId, int msgSequenceNumber)
        {
            //Do nothing
        }

        public bool FreezeBludgerIfNeeded(Bludger bludger)
        {
            throw new NotImplementedException();
        }

        public bool FreezeSequencerIfNeeded(Bludger bludger)
        {
            throw new NotImplementedException();
        }

        public bool HasMessage(string id, int seq)
        {
            return false;
        }
    }
}
