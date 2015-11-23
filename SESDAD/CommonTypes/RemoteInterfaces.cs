using System;
using System.Runtime.Remoting.Messaging;

namespace CommonTypes
{
    // Interfaces to system nodes communication.

    public interface IBroker
    {
        void Diffuse(Event e);
        [OneWay]
        void Subscribe(Subscription subscription);
        [OneWay]
        void Unsubscribe(Subscription subscription);
        [OneWay]
        void AddRoute(Route route);
        [OneWay]
        void RemoveRoute(Route route);
    }

    public interface ISubscriber
    {
        void Receive(Event e);
    }

    public interface IPublisher
    {
        // Nothing here for now
    }

    // Interfaces to test and control of system processes.

    public interface IGeneralControlServices
    {
        [OneWay]
        void Crash();
        void Freeze();
        void Unfreeze();
        void Status();
        void Init(Object o);
    }

    public interface IPublisherControlServices
    {
        void Publish(string topicName,int numberOfEvents,int interval);
    }

    public interface ISubscriberControlServices
    {
        void Subscribe(string topicName);
        void Unsubscribe(string topicName);
    }  

    public interface IPuppetMasterLog
    {
        void LogAction(string logMessage);
    }

    public interface IPuppetMasterLauncher
    {
        void LaunchProcess(string name,string args);
    }

}
