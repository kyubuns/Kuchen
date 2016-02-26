using System;
using System.Collections.Generic;

namespace Kuchen
{
	public interface ISubscribeEvent
	{
		bool Muting { get; set; }
		string[] Topics { get; }
		void Call(string topic, object[] args);
		Action PostHook { get; set; }
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
		
		public static SubscribeEvent<T1> Create<T1>(string topic, Action<T1> callback) { return Create(new string[]{topic}, callback); }
		public static SubscribeEvent<T1> Create<T1>(string[] topics, Action<T1> callback)
		{
			return new SubscribeEvent<T1>(topics, callback);
		}
		
		public static SubscribeEvent<T1, T2> Create<T1, T2>(string topic, Action<T1, T2> callback) { return Create(new string[]{topic}, callback); }
		public static SubscribeEvent<T1, T2> Create<T1, T2>(string[] topics, Action<T1, T2> callback)
		{
			return new SubscribeEvent<T1, T2>(topics, callback);
		}
		
		public static SubscribeEvent<T1, T2, T3> Create<T1, T2, T3>(string topic, Action<T1, T2, T3> callback) { return Create(new string[]{topic}, callback); }
		public static SubscribeEvent<T1, T2, T3> Create<T1, T2, T3>(string[] topics, Action<T1, T2, T3> callback)
		{
			return new SubscribeEvent<T1, T2, T3>(topics, callback);
		}
	}
	
	public partial class SubscribeEventWithTopic
	{
		public static SubscribeEventWithTopic Create(string topic, Action<string> callback) { return Create(new string[]{topic}, callback); }
		public static SubscribeEventWithTopic Create(string[] topics, Action<string> callback)
		{
			return new SubscribeEventWithTopic(topics, callback);
		}
		
		public static SubscribeEventWithTopic<T1> Create<T1>(string topic, Action<string, T1> callback) { return Create(new string[]{topic}, callback); }
		public static SubscribeEventWithTopic<T1> Create<T1>(string[] topics, Action<string, T1> callback)
		{
			return new SubscribeEventWithTopic<T1>(topics, callback);
		}
		
		public static SubscribeEventWithTopic<T1, T2> Create<T1, T2>(string topic, Action<string, T1, T2> callback) { return Create(new string[]{topic}, callback); }
		public static SubscribeEventWithTopic<T1, T2> Create<T1, T2>(string[] topics, Action<string, T1, T2> callback)
		{
			return new SubscribeEventWithTopic<T1, T2>(topics, callback);
		}
		
		public static SubscribeEventWithTopic<T1, T2, T3> Create<T1, T2, T3>(string topic, Action<string, T1, T2, T3> callback) { return Create(new string[]{topic}, callback); }
		public static SubscribeEventWithTopic<T1, T2, T3> Create<T1, T2, T3>(string[] topics, Action<string, T1, T2, T3> callback)
		{
			return new SubscribeEventWithTopic<T1, T2, T3>(topics, callback);
		}
	}
	
	public partial class SubscribeEvent : ISubscribeEvent
	{
		public string[] Topics { get; private set; }
		public bool Muting { get; set; }
		public Action PostHook { get; set; }
		private Action callback;
		
		public SubscribeEvent(string[] topics, Action callback)
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
			this.callback();
			
			if(this.PostHook != null) this.PostHook();
		}
		
		public override string ToString()
		{
			return String.Format("SubscribeEvent Topic: {0}", string.Join(",", Topics));
		}
	}
	
	public partial class SubscribeEventWithTopic : ISubscribeEvent
	{
		public string[] Topics { get; private set; }
		public bool Muting { get; set; }
		public Action PostHook { get; set; }
		private Action<string> callback;
		
		public SubscribeEventWithTopic(string[] topics, Action<string> callback)
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
			
			if(this.PostHook != null) this.PostHook();
		}
		
		public override string ToString()
		{
			return String.Format("SubscribeEvent Topic: {0}", string.Join(",", Topics));
		}
	}
	
	public class SubscribeEvent<T1> : ISubscribeEvent
	{
		public string[] Topics { get; private set; }
		public bool Muting { get; set; }
		public Action PostHook { get; set; }
		private Action<T1> callback;
		
		public SubscribeEvent(string[] topics, Action<T1> callback)
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
			if(args.Length >= 1) this.callback((T1)args[0]);
			else this.callback(default(T1));
			
			if(this.PostHook != null) this.PostHook();
		}
		
		public override string ToString()
		{
			return String.Format("SubscribeEvent<{1}> Topic: {0}", string.Join(",", Topics), typeof(T1));
		}
	}
	
	public class SubscribeEventWithTopic<T1> : ISubscribeEvent
	{
		public string[] Topics { get; private set; }
		public bool Muting { get; set; }
		public Action PostHook { get; set; }
		private Action<string, T1> callback;
		
		public SubscribeEventWithTopic(string[] topics, Action<string, T1> callback)
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
			if(args.Length >= 1) this.callback(topic, (T1)args[0]);
			else this.callback(topic, default(T1));
			
			if(this.PostHook != null) this.PostHook();
		}
		
		public override string ToString()
		{
			return String.Format("SubscribeEvent<{1}> Topic: {0}", string.Join(",", Topics), typeof(T1));
		}
	}
	
	public class SubscribeEvent<T1, T2> : ISubscribeEvent
	{
		public string[] Topics { get; private set; }
		public bool Muting { get; set; }
		public Action PostHook { get; set; }
		private Action<T1, T2> callback;
		
		public SubscribeEvent(string[] topics, Action<T1, T2> callback)
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
			if(args.Length >= 2) this.callback((T1)args[0], (T2)args[1]);
			else if(args.Length >= 1) this.callback((T1)args[0], default(T2));
			else this.callback(default(T1), default(T2));
			
			if(this.PostHook != null) this.PostHook();
		}
		
		public override string ToString()
		{
			return String.Format("SubscribeEvent<{1},{2}> Topic: {0}", string.Join(",", Topics), typeof(T1), typeof(T2));
		}
	}
	
	public class SubscribeEventWithTopic<T1, T2> : ISubscribeEvent
	{
		public string[] Topics { get; private set; }
		public bool Muting { get; set; }
		public Action PostHook { get; set; }
		private Action<string, T1, T2> callback;
		
		public SubscribeEventWithTopic(string[] topics, Action<string, T1, T2> callback)
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
			if(args.Length >= 2) this.callback(topic, (T1)args[0], (T2)args[1]);
			else if(args.Length >= 1) this.callback(topic, (T1)args[0], default(T2));
			else this.callback(topic, default(T1), default(T2));
			
			if(this.PostHook != null) this.PostHook();
		}
		
		public override string ToString()
		{
			return String.Format("SubscribeEvent<{1},{2}> Topic: {0}", string.Join(",", Topics), typeof(T1), typeof(T2));
		}
	}
	
	public class SubscribeEvent<T1, T2, T3> : ISubscribeEvent
	{
		public string[] Topics { get; private set; }
		public bool Muting { get; set; }
		public Action PostHook { get; set; }
		private Action<T1, T2, T3> callback;
		
		public SubscribeEvent(string[] topics, Action<T1, T2, T3> callback)
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
			if(args.Length >= 3) this.callback((T1)args[0], (T2)args[1], (T3)args[2]);
			else if(args.Length >= 2) this.callback((T1)args[0], (T2)args[1], default(T3));
			else if(args.Length >= 1) this.callback((T1)args[0], default(T2), default(T3));
			else this.callback(default(T1), default(T2), default(T3));
			
			if(this.PostHook != null) this.PostHook();
		}
		
		public override string ToString()
		{
			return String.Format("SubscribeEvent<{1},{2},{3}> Topic: {0}", string.Join(",", Topics), typeof(T1), typeof(T2), typeof(T3));
		}
	}
	
	public class SubscribeEventWithTopic<T1, T2, T3> : ISubscribeEvent
	{
		public string[] Topics { get; private set; }
		public bool Muting { get; set; }
		public Action PostHook { get; set; }
		private Action<string, T1, T2, T3> callback;
		
		public SubscribeEventWithTopic(string[] topics, Action<string, T1, T2, T3> callback)
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
			if(args.Length >= 3) this.callback(topic, (T1)args[0], (T2)args[1], (T3)args[2]);
			else if(args.Length >= 2) this.callback(topic, (T1)args[0], (T2)args[1], default(T3));
			else if(args.Length >= 1) this.callback(topic, (T1)args[0], default(T2), default(T3));
			else this.callback(topic, default(T1), default(T2), default(T3));
			
			if(this.PostHook != null) this.PostHook();
		}
		
		public override string ToString()
		{
			return String.Format("SubscribeEvent<{1},{2},{3}> Topic: {0}", string.Join(",", Topics), typeof(T1), typeof(T2), typeof(T3));
		}
	}
}
		