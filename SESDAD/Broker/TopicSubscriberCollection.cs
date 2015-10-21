using CommonTypes;
using System.Collections.Generic;
using System.Linq;

namespace Broker
{
    public class TopicSubscriberCollection 
	{
		class Router<T>
		{
			public string topic;
			public ICollection<T> subscribers;
			public ICollection<T> subscribersAll;
			public IDictionary<string, Router<T>> children;
			
			public Router(string topic, ICollection<T> subscribers, ICollection<T> subscribersAll, IDictionary<string, Router<T>> children)
			{
				this.topic = topic;
				this.subscribers = subscribers;
				this.subscribersAll = subscribersAll;
				this.children = children;
			}
			
			public static Router<T> ForTopic(string topic)
			{
				return new Router<T>(topic, new HashSet<T>(), new HashSet<T>(), new Dictionary<string, Router<T>>());
			}
			
			public bool hasChild(string part)
			{
				return this.children.ContainsKey(part);
			}
		}
		
		private Router<ISubscriber> data;
		
		public TopicSubscriberCollection()
		{
			this.data = Router<ISubscriber>.ForTopic("/");
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
					current.children.Add(part, Router<ISubscriber>.ForTopic(currentTopic));
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