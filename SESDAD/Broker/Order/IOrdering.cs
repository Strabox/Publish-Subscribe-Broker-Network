using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Order
{
    public interface IOrder
    {
        void AddNewMessage(string publisherId, int msgSequenceNumber);

        void ConfirmDeliver(string publisherId);

        void Deliver(string publisherId, int msgSequenceNumber);
    }
}
