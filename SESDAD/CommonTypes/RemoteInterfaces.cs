using System;
using System.Runtime.Remoting.Messaging;

namespace CommonTypes
{
    // Interfaces to system nodes communication.

    public interface IBroker
    {
        void Diffuse(Event e);
        void Sequence(Bludger b);
        void Bludger(Bludger b); 
        void Subscribe(Subscription subscription);
        void Unsubscribe(Subscription subscription);
        void AddRoute(Route route);
        void RemoveRoute(Route route);
    }

    public interface ISubscriber
    {
        void Receive(Event e);
        void Bludger(Bludger bludger);
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
        [OneWay]
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
