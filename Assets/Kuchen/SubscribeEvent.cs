using System;
using System.Collections.Generic;

namespace Kuchen
{
	public interface ISubscribeEvent
	{
		string[] Topics { get; }
		void Call(string topic, object[] args);
		string ToString();
		bool RemoveTopic(string topic);
	}
	
	public partial class SubscribeEvent
	{
		public static SubscribeEvent Create(string topic, Action callback) { return Create(new string[]{topic}, callback); }
		public static SubscribeEvent Create(string[] topics, Action callback)
		{
			return new SubscribeEvent(topics, callback);
		}
		
		public static SubscribeEvent Create(string topic, Action<string> callback) { return Create(new string[]{topic}, callback); }
		public static SubscribeEvent Create(string[] topics, Action<string> callback)
		{
			return new SubscribeEvent(topics, callback);
		}
		
		public static SubscribeEvent<T1> Create<T1>(string topic, Action<string, T1> callback) { return Create(new string[]{topic}, callback); }
		public static SubscribeEvent<T1> Create<T1>(string[] topics, Action<string, T1> callback)
		{
			return new SubscribeEvent<T1>(topics, callback);
		}
		
		public static SubscribeEvent<T1, T2> Create<T1, T2>(string topic, Action<string, T1, T2> callback) { return Create(new string[]{topic}, callback); }
		public static SubscribeEvent<T1, T2> Create<T1, T2>(string[] topics, Action<string, T1, T2> callback)
		{
			return new SubscribeEvent<T1, T2>(topics, callback);
		}
	}
	
	public partial class SubscribeEvent : ISubscribeEvent
	{
		public string[] Topics { get; private set; }
		private Action<string> callback;
		
		public SubscribeEvent(string[] topics, Action callback)
		{
			this.Topics = topics;
			this.callback = (_) => { callback(); };
		}
		
		public SubscribeEvent(string[] topics, Action<string> callback)
		{
			this.Topics = topics;
			this.callback = callback;
		}
		
		public bool RemoveTopic(string topic)
		{
			var i = Array.IndexOf(Topics, topic);
			if(i == -1) return false;
			
			var tmp = new List<string>(Topics);
			tmp.RemoveAt(i);
			Topics = tmp.ToArray();
			return Topics.Length == 0;
		}
		
		public void Call(string topic, object[] args)
		{
			this.callback(topic);
		}
		
		public override string ToString()
		{
			return String.Format("SubscribeEvent Topic: {0}", string.Join(",", Topics));
		}
	}
	
	public class SubscribeEvent<T1> : ISubscribeEvent
	{
		public string[] Topics { get; private set; }
		private Action<string, T1> callback;
		
		public SubscribeEvent(string[] topics, Action<string, T1> callback)
		{
			this.Topics = topics;
			this.callback = callback;
		}
		
		public bool RemoveTopic(string topic)
		{
			var i = Array.IndexOf(Topics, topic);
			if(i == -1) return false;
			
			var tmp = new List<string>(Topics);
			tmp.RemoveAt(i);
			Topics = tmp.ToArray();
			return Topics.Length == 0;
		}
		
		public void Call(string topic, object[] args)
		{
			this.callback(topic, (T1)args[0]);
		}
		
		public override string ToString()
		{
			return String.Format("SubscribeEvent<{1}> Topic: {0}", string.Join(",", Topics), typeof(T1));
		}
	}
	
	public class SubscribeEvent<T1, T2> : ISubscribeEvent
	{
		public string[] Topics { get; private set; }
		private Action<string, T1, T2> callback;
		
		public SubscribeEvent(string[] topics, Action<string, T1, T2> callback)
		{
			this.Topics = topics;
			this.callback = callback;
		}
		
		public bool RemoveTopic(string topic)
		{
			var i = Array.IndexOf(Topics, topic);
			if(i == -1) return false;
			
			var tmp = new List<string>(Topics);
			tmp.RemoveAt(i);
			Topics = tmp.ToArray();
			return Topics.Length == 0;
		}
		
		public void Call(string topic, object[] args)
		{
			this.callback(topic, (T1)args[0], (T2)args[1]);
		}
		
		public override string ToString()
		{
			return String.Format("SubscribeEvent<{1},{2}> Topic: {0}", string.Join(",", Topics), typeof(T1), typeof(T2));
		}
	}
}
		