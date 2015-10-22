using CommonTypes;
using System.Collections.Generic;
using System.Linq;

namespace Broker
{
    public class TopicSubscriberCollection 
	{
		/**
		*	This static class contains helper methods for handling string topics
		*/
		static class Topic
		{
			public static string[] Split(string topic)
			{
				return topic.Split('/').Skip(1).ToArray();
			}
			
			public static bool IsWildcard(string subtopic)
			{
				return subtopic == "*";
			}
		}
		
		class Router<T>
		{
			public string topic;
			public ICollection<T> nodes;
			public ICollection<T> nodesAsterisk;
			public IDictionary<string, Router<T>> subtopics;
			
			public Router(string topic, ICollection<T> nodes, ICollection<T> nodesAsterisk, IDictionary<string, Router<T>> subtopics)
			{
				this.topic = topic;
				this.nodes = nodes;
				this.nodesAsterisk = nodesAsterisk;
				this.subtopics = subtopics;
			}
			
			public static Router<T> ForTopic(string topic)
			{
				return new Router<T>(topic, new HashSet<T>(), new HashSet<T>(), new Dictionary<string, Router<T>>());
			}
			
			public bool HasSubtopic(string subtopic)
			{
				return this.subtopics.ContainsKey(subtopic);
			}
			
			public void Add(string topic, T node)
			{
				string[] parts = Topic.Split(topic);
				var current = this;
				
				string currentTopic = "";
				
				foreach (var part in parts)
				{
					currentTopic += "/" + part;
	
					if (Topic.IsWildcard(part))
					{
						current.nodesAsterisk.Add(node);
						return;
					}
	
					if ( ! current.HasSubtopic(part))
					{
						current.subtopics.Add(part, Router<T>.ForTopic(currentTopic));
					}
					
					current = current.subtopics[part];				
				}
				
				current.nodes.Add(node);
			}
			
			public void Remove(string topic, T node)
			{
				string[] parts = Topic.Split(topic);
				var current = this;
				
				foreach (var part in parts)
				{
					if (Topic.IsWildcard(part))
					{
						current.nodesAsterisk.Remove(node);
					}
					
					if ( ! current.HasSubtopic(part))
					{
						// No subscriber has subscribed the topic
						// Still, that's weird.
						return;
					}
					
					current = current.subtopics[part];				
				}
				
				current.nodes.Remove(node);
			}
			
			public ICollection<T> NodesFor(string topic)
			{
				string[] parts = Topic.Split(topic);
				var current = this;
				HashSet<T> nodes = new HashSet<T>();
				
				foreach (var part in parts)
				{
					nodes.UnionWith(current.nodesAsterisk);
					
					if ( ! current.HasSubtopic(part))
					{
						return nodes;
					}
					
					current = current.subtopics[part];
				}
				
				nodes.UnionWith(current.nodes);
	
				return nodes;
			}
			
			public bool HasNodesFor(string topic)
			{
				string[] parts = Topic.Split(topic);
				var current = this;
				
				foreach (var part in parts)
				{
					if (current.nodesAsterisk.Count > 0)
					{
						return true;
					}
					
					if ( ! current.HasSubtopic(part))
					{
						return false;
					}
					
					current = current.subtopics[part];
				}
				
				return current.nodes.Count > 0;
			}
			
		}
		
		private Router<ISubscriber> subscribers;
		private Router<IBroker> brokers;
		
		public TopicSubscriberCollection()
		{
			this.subscribers = Router<ISubscriber>.ForTopic("/");
			this.brokers = Router<IBroker>.ForTopic("/");
		}
		
		/**
		 * Returns true if this node gains interest in the topic. Returns false otherwise.
		 */
		public bool AddSubscriber(string topic, ISubscriber subscriber)
		{
			lock (this)
			{
				bool result = ( ! this.brokers.HasNodesFor(topic)) && ( ! this.subscribers.HasNodesFor(topic));
				this.subscribers.Add(topic, subscriber);
				return result;
			}			
		}
		
		/**
		 * Returns true if this node loses all interest in the topic. Returns false otherwise.
		 */
		public bool RemoveSubscriber(string topic, ISubscriber subscriber)
		{
			lock (this)
			{
				this.subscribers.Remove(topic, subscriber);
				return ( ! this.brokers.HasNodesFor(topic)) && ( ! this.subscribers.HasNodesFor(topic));	
			}		
		}
		
		public ICollection<ISubscriber> SubscribersFor(string topic)
		{
			lock (this)
			{
				return this.subscribers.NodesFor(topic);
			}
		}
		
		/**
		 * Returns true if this node gains interest in the topic. Returns false otherwise.
		 */
		public bool AddRoute(string topic, IBroker broker)
		{
			lock (this)
			{
				bool result = ( ! this.brokers.HasNodesFor(topic)) && ( ! this.subscribers.HasNodesFor(topic));
				this.brokers.Add(topic, broker);
				return result;
			}
		}
		
		/**
		 * Returns true if this node loses all interest in the topic. Returns false otherwise.
		 */
		public bool RemoveRoute(string topic, IBroker broker)
		{
			lock (this)
			{
				this.brokers.Remove(topic, broker);
				return ( ! this.brokers.HasNodesFor(topic)) && ( ! this.subscribers.HasNodesFor(topic));
			}			
		}
		
		public ICollection<IBroker> RoutingFor(string topic)
		{
			lock (this)
			{
				return this.brokers.NodesFor(topic);
			}
		}
		
	}
}