using CommonTypes;
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

        void ConfirmDeliver(Event e);

        void Deliver(string publisherId, int msgSequenceNumber);

        bool HasMessage(String id, int seq);

        bool FreezeSequencerIfNeeded(Bludger bludger);

        bool FreezeBludgerIfNeeded(Bludger bludger);

    }
}
