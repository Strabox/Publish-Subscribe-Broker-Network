using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;

namespace CommonTypes
{

    [Serializable]
    public class Event : ISerializable
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

        private int sequenceNumber;
        public int SequenceNumber
        {
            get { return sequenceNumber; }
            set { sequenceNumber = value; }
        }

        public Event(string publisher, string sender, string topic, string content, int sequenceNumber)
        {
            Publisher = publisher;
            Topic = topic;
            Sender = sender;
            Content = content;
            SequenceNumber = sequenceNumber;
        }

        public Event(SerializationInfo info, StreamingContext context)
        {
            Publisher = info.GetValue("publisher", typeof(string)) as string;
            Topic = info.GetValue("topic", typeof(string)) as string;
            Content = info.GetValue("content", typeof(string)) as string;
            Sender = info.GetValue("sender", typeof(string)) as string;
            SequenceNumber = (int)info.GetValue("sn", typeof(int));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
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

        private string siteName;
        public string SiteName
        {
            get { return siteName; }
            set { siteName = value; }
        }

        public Route(string topic, string name, IBroker broker)
        {
            Topic = topic;
            Broker = broker;
            SiteName = name;
        }

        public Route(SerializationInfo info, StreamingContext context)
        {
            Topic = info.GetValue("topic", typeof(string)) as string;
            SiteName = info.GetValue("brokerName", typeof(string)) as string;
            Broker = info.GetValue("broker", typeof(IBroker)) as IBroker;
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("topic", topic);
            info.AddValue("brokerName", siteName);
            info.AddValue("broker", broker);
        }
    }

    public abstract class GenericNode : IGeneralControlServices
    {
        private bool freeze;
        protected bool IsFreeze
        {
            get { return freeze; }
            set { freeze = value; }
        }

        public GenericNode()
        {
            IsFreeze = false;
        }

        protected void BlockWhileFrozen()
        {
            lock (this)
            {
                while (IsFreeze)
                    Monitor.Wait(this);
            }
        }

        // Control Services Interface method's

        public void Crash()
        {
            System.Environment.Exit(-1);
        }

        public void Freeze()
        {
            lock (this)
            {
                IsFreeze = true;
            }
        }

        public void Unfreeze()
        {
            lock (this)
            {
                IsFreeze = false;
                Monitor.PulseAll(this);
            }
        }

        public abstract void Init(Object o);

        public abstract void Status();
    }
    
    // ========= Data Transfer Objects for system boot ==========

    [Serializable]
    public class SiteDTO
    {
        [Serializable]
        public class SiteBrokers {

            public string Name { get; set; }

            public List<BrokerPairDTO> Brokers { get; set; }

            public SiteBrokers(string name,List<BrokerPairDTO> brokers)
            {
                Name = name;
                Brokers = brokers;
            }

            public override string ToString()
            {
                string res = "";
                List<BrokerPairDTO> dtos = Brokers;
                res += "Site " + Name + " :" + Environment.NewLine;
                foreach(BrokerPairDTO dto in dtos)
                {
                    res += dto.ToString() + Environment.NewLine;
                }
                return res;
            }

        }

        public string Name { get; set; }

        public bool IsRoot { get; set; }

        public SiteBrokers Parent { get; set; }

        public List<BrokerPairDTO> Brokers { get; set; }

        public List<SiteBrokers> Childs { get; set; }

        public SiteDTO(string name,bool isRoot,SiteBrokers parent,List<BrokerPairDTO> brokers)
        {
            Name = name;
            IsRoot = isRoot;
            Parent = parent;
            Brokers = brokers;
            Childs = new List<SiteBrokers>();
        }


        public void AddSiteChild(string siteName,List<BrokerPairDTO> brokers)
        {
            Childs.Add(new SiteBrokers(siteName,brokers));
        }

        public override string ToString()
        {
            String res = "";
            List<BrokerPairDTO> brokers = Brokers;
            List<SiteBrokers> child = Childs;
            res += "Site " + Name + " :" + Environment.NewLine;
            if (IsRoot)
            {
                res += "Root Site" + Environment.NewLine;
            }
            else
            {
                res += "Parent " + Parent.ToString();
            }
            res += "Brothers :" + Environment.NewLine;
            foreach(BrokerPairDTO dto in brokers)
            {
                res += dto.ToString() + Environment.NewLine;
            }
            res += "Child :" + Environment.NewLine;
            foreach(SiteBrokers dto in child)
            {
                res += dto.ToString();
            } 
            return res;
        }

    }

    [Serializable]
    public class BrokerPairDTO
    {
        public string Url { get; set; }

        public string LogicName { get; set; }

        public BrokerPairDTO(string url, string logicName)
        {
            Url = url;
            LogicName = logicName;
        }

        public override string ToString()
        {
            return "Broker " + LogicName + " At " + Url;
        }

    }

    /// <summary>
    ///     Remote Object 
    /// </summary>
    public abstract class GenericRemoteNode : MarshalByRefObject
    {
        public override object InitializeLifetimeService()
        {
            return null;
        }
    }

    [Serializable]
    public class Bludger
    {
        public String Publisher { get; set;}
        public String Topic { get; set; }
        public int Sequence { get; set; }

        public Bludger (string publisher, string topic, int sequence)
        {
            this.Publisher = publisher;
            this.Topic = topic;
            this.Sequence = sequence;
        }
    }
}