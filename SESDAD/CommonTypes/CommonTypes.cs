using System;
using System.Runtime.Serialization;

namespace CommonTypes {

    [Serializable]
    public class Message : ISerializable
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

        public Message(string topic)
        {
            Topic = topic;
        }

		public Message(SerializationInfo info, StreamingContext context) 
		{
			topic = info.GetValue("topic", typeof(string)) as string;
			content = info.GetValue("content", typeof(string)) as string;
		}
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
			info.AddValue("topic", topic);
			info.AddValue("content", content);
        }
    }
}