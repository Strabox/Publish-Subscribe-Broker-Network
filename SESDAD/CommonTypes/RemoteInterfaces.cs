using System.Runtime.Remoting.Messaging;

namespace CommonTypes
{
    // Interfaces to system nodes communication.

    public interface IBroker
    {
        [OneWay]
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
        [OneWay]
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
        [OneWay]
        void Freeze();
        [OneWay]
        void Unfreeze();
        [OneWay]
        void Status();
        /* It isn't one way(async) because I want wait for all Init returns
        to start the system. */
        void Init();
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
        [OneWay]
        void Unsubscribe(string topicName);
    }  

    public interface IPuppetMasterLog
    {
        void LogAction(string logMessage);
    }

    public interface IPuppetMasterLauncher
    {
        [OneWay]
        void LaunchProcess(string name,string args);
    }

}
