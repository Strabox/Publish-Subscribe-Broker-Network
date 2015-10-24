using System;
using System.Runtime.Serialization;

namespace CommonTypes {

    public interface IMessage
    {
        int GetSequenceNumber();
        string GetId();
    }

    [Serializable]
    public class Event : ISerializable, IMessage
    {
        private string publisher;
        public string Publisher
        {
            get { return publisher; }
            private set { publisher = value; }
        }

		private string topic;
		public string Topic 
		{
			get { return topic; }
			set { topic = value; }
		}
		
		private string content;
		public string Content 
		{
			get { return content; }
			set { content = value; }
		}

        private string sender;
        public string Sender
        {
            get { return sender; }
            set { sender = value; }
        }

        private int eventNumber;
        public int EventNumber
        {
            get { return eventNumber; }
            set { eventNumber = value; }
        }

        private int sequenceNumber;
        public int SequenceNumber
        {
            get { return sequenceNumber; }
            set { sequenceNumber = value; }
        }

        public Event(string publisher,string sender, string topic, string content,int eventNumber,int sequenceNumber)
        {
            Publisher = publisher;
            Topic = topic;
            Sender = sender;
            Content = content;
            EventNumber = eventNumber;
            SequenceNumber = sequenceNumber;
        }

		public Event(SerializationInfo info, StreamingContext context) 
		{
            Publisher = info.GetValue("publisher", typeof(string)) as string;
            EventNumber = (int)info.GetValue("eventNumber",typeof(int));
			Topic = info.GetValue("topic", typeof(string)) as string;
			Content = info.GetValue("content", typeof(string)) as string;
            Sender = info.GetValue("sender", typeof(string)) as string;
            SequenceNumber = (int) info.GetValue("sn", typeof(int));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("eventNumber", EventNumber);
            info.AddValue("publisher", Publisher);
			info.AddValue("topic", Topic);
			info.AddValue("content", Content);
            info.AddValue("sender", Sender);
            info.AddValue("sn", SequenceNumber);
        }

        public int GetSequenceNumber()
        {
            return SequenceNumber;
        }

        public string GetId()
        {
            return Publisher;
        }
    }

    [Serializable]
    public class Subscription : ISerializable
    {
        private string topic;
        public string Topic
        {
            get { return topic; }
            set { topic = value; }
        }

        private ISubscriber subscriber;
        public ISubscriber Subscriber
        {
            get { return subscriber; }
            set { subscriber = value; }
        }
        
        private string subscriberName;
        public string SubscriberName
        {
            get { return subscriberName; }
            set { subscriberName = value; }
        }

        private string sender;
        public string Sender
        {
            get { return sender; }
            set { sender = value; }
        }

        public Subscription(string sender, string topic, string name, ISubscriber subscriber)
        {
            Topic = topic;
            Sender = sender;
            SubscriberName = name;
            Subscriber = subscriber;
        }

        public Subscription(SerializationInfo info, StreamingContext context)
        {
            Topic = info.GetValue("topic", typeof(string)) as string;
            Subscriber = info.GetValue("subscriber", typeof(ISubscriber)) as ISubscriber;
            SubscriberName = info.GetValue("subscriberName", typeof(string)) as string;
            Sender = info.GetValue("sender", typeof(string)) as string;
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("topic", topic);
            info.AddValue("subscriber", subscriber);
            info.AddValue("subscriberName", subscriberName);
            info.AddValue("sender", sender);
        }
    }
    
    [Serializable]
    public class Route : ISerializable
    {
        private string topic;
        public string Topic
        {
            get { return topic; }
            set { topic = value; }
        }

        private IBroker broker;
        public IBroker Broker
        {
            get { return broker; }
            set { broker = value; }
        }
        
        private string brokerName;
        public string BrokerName
        {
            get { return brokerName; }
            set { brokerName = value; }
        }

        public Route(string topic, string name, IBroker broker)
        {
            Topic = topic;
            Broker = broker;
            BrokerName = name;
        }

        public Route(SerializationInfo info, StreamingContext context)
        {
            Topic = info.GetValue("topic", typeof(string)) as string;
            BrokerName = info.GetValue("brokerName", typeof(string)) as string;            
            Broker = info.GetValue("broker", typeof(IBroker)) as IBroker;
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("topic", topic);
            info.AddValue("brokerName", brokerName);
            info.AddValue("broker", broker);
        }
    }
}