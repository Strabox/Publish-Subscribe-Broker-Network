using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes
{

    public interface ICommon
    {
        [OneWayAttribute]
        void Crash();
        void Freeze();
        void Unfreeze();
        void Status();
    }

    public interface IPublisher
    {
        void Publish(string topicName,int numberOfEvents,int interval);
    }

    public interface ISubscriber
    {
        void Subscribe(string topicName);
        void Unsubscribe(string topicName);
    }

    public interface IPuppetMaster
    {
        //TODO Logging Calls ....
    }

}
