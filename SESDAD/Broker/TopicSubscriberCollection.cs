using CommonTypes;
using System.Collections.Generic;
using System.Linq;

namespace Broker
{
    class TopicSubscriberCollection 
	{
		class Struct 
		{
			public string topic;
			public ICollection<ISubscriber> subscribers;
			public ICollection<ISubscriber> subscribersAll;
			public IDictionary<string, Struct> children;
			
			public Struct(string topic, ICollection<ISubscriber> subscribers, ICollection<ISubscriber> subscribersAll, IDictionary<string, Struct> children)
			{
				this.topic = topic;
				this.subscribers = subscribers;
				this.subscribersAll = subscribersAll;
				this.children = children;
			}
			
			public static Struct ForTopic(string topic)
			{
				return new Struct(topic, new HashSet<ISubscriber>(), new HashSet<ISubscriber>(), new Dictionary<string, Struct>());
			}
			
			public bool hasChild(string part)
			{
				return this.children.ContainsKey(part);
			}
		}
		private Struct data;
		
		public TopicSubscriberCollection()
		{
			this.data = Struct.ForTopic("/");
		}
		
		public void Add(string topic, ISubscriber subscriber)
		{
			string[] parts = this.splitTopic(topic);
			var current = this.data;
			string currentTopic = "";
			
			foreach (var part in parts)
			{
				currentTopic += "/" + part;

				if (isWildcard(part))
				{
					current.subscribersAll.Add(subscriber);
					return;
				}

				if ( ! current.hasChild(part))
				{
					current.children.Add(part, Struct.ForTopic(currentTopic));
				}
				
				current = current.children[part];				
			}
			
			current.subscribers.Add(subscriber);
		}
		
		public void Remove(string topic, ISubscriber subscriber)
		{
			string[] parts = this.splitTopic(topic);
			var current = this.data;
			
			foreach (var part in parts)
			{
				if (isWildcard(part))
				{
					current.subscribersAll.Remove(subscriber);
				}
				
				if ( ! current.hasChild(part))
				{
					// No subscriber has subscribed the topic
					return;
				}
				
				current = current.children[part];				
			}
			
			current.subscribers.Remove(subscriber);
		}
		
		public ICollection<ISubscriber> SubscribersForTopic(string topic)
		{
			string[] parts = this.splitTopic(topic);
			var current = this.data;
			HashSet<ISubscriber> subscribers = new HashSet<ISubscriber>();
			
			foreach (var part in parts)
			{
				subscribers.UnionWith(current.subscribersAll);
				
				if ( ! current.hasChild(part))
				{
					return subscribers;
				}
				
				current = current.children[part];
			}
			
			subscribers.UnionWith(current.subscribers);

			return subscribers;
		}
		
		private string[] splitTopic(string topic)
		{
			return topic.Split('/').Skip(1).ToArray();
		}
		
		private bool isWildcard(string part)
		{
			return part == "*";
		}
	}
}