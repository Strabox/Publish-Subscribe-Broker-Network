using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes
{
    // Interfaces to system nodes communication.

    public interface IBroker
    {
        [OneWay]
        void Diffuse(Event e);
        [OneWay]
        void Subscribe(Subscription subscription);
        void Unsubscribe(Subscription subscription);
        void Init();
        string GetName();
        void HeyDaddy(string url);
    }

    public interface ISubscriber
    {
        void Receive(Event e);
        string GetName();
    }

    public interface IPublisher
    {
        string GetName();
        //TODO
    }

    // Interfaces to test and control of system processes.

    public interface IGeneralControlServices
    {
        [OneWay]
        void Crash();
        void Freeze();
        void Unfreeze();
        void Status();
    }

    public interface IPublisherControlServices
    {
        [OneWay]
        void Publish(string topicName,int numberOfEvents,int interval);
    }

    public interface ISubscriberControlServices
    {
        [OneWay]
        void Subscribe(string topicName);
        void Unsubscribe(string topicName);
    }  

    public interface IPuppetMasterLog
    {
        [OneWay]
        void LogAction(string logMessage);
    }

    public interface IPuppetMasterLauncher
    {
        [OneWay]
        void LaunchProcess(string name,string args);
    }

}
