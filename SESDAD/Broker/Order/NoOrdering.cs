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

        public void ConfirmDeliver(string publisherId)
        {
            //Do nothing
        }

        public void Deliver(string publisherId, int msgSequenceNumber)
        {
            //Do nothing
        }
    }
}
