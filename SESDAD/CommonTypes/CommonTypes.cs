using System;
using System.Runtime.Serialization;

namespace CommonTypes {

    [Serializable]
    public class Event : ISerializable
    {
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

        public Event(string sender, string topic, string content)
        {
            Topic = topic;
            Sender = sender;
            Content = content;
        }

		public Event(SerializationInfo info, StreamingContext context) 
		{
			topic = info.GetValue("topic", typeof(string)) as string;
			content = info.GetValue("content", typeof(string)) as string;
            sender = info.GetValue("sender", typeof(string)) as string;
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
			info.AddValue("topic", topic);
			info.AddValue("content", content);
            info.AddValue("sender", sender);
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

        private string sender;
        public string Sender
        {
            get { return sender; }
            set { sender = value; }
        }

        public Subscription(string sender, string topic, ISubscriber subscriber)
        {
            Topic = topic;
            Sender = sender;
            Subscriber = subscriber;
        }

        public Subscription(SerializationInfo info, StreamingContext context)
        {
            topic = info.GetValue("topic", typeof(string)) as string;
            subscriber = info.GetValue("subscriber", typeof(ISubscriber)) as ISubscriber;
            sender = info.GetValue("sender", typeof(string)) as string;
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("topic", topic);
            info.AddValue("subscriber", subscriber);
            info.AddValue("sender", sender);
        }
    }
}