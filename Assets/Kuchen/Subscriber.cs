using System;
using System.Collections.Generic;
using Kuchen.Util;

namespace Kuchen
{
	public class Subscriber : IDisposable
	{
		private List<ISubscribeEvent> subscribeEvents = new List<ISubscribeEvent>();
		private List<Action> bookedEvents = null;
		private bool registered = false;
		
		public SubscribeEventChain Subscribe(string topic, Action callback) { return Subscribe(SubscribeEvent.Create(topic, callback)); }
		public SubscribeEventChain Subscribe(string[] topics, Action callback) { return Subscribe(SubscribeEvent.Create(topics, callback)); }
		public SubscribeEventChain SubscribeWithTopic(string topic, Action<string> callback) { return Subscribe(SubscribeEventWithTopic.Create(topic, callback)); }
		public SubscribeEventChain SubscribeWithTopic(string[] topics, Action<string> callback) { return Subscribe(SubscribeEventWithTopic.Create(topics, callback)); }
		public SubscribeEventChain Subscribe<T1>(string topic, Action<T1> callback) { return Subscribe(SubscribeEvent.Create(topic, callback)); }
		public SubscribeEventChain Subscribe<T1>(string[] topics, Action<T1> callback) { return Subscribe(SubscribeEvent.Create(topics, callback)); }
		public SubscribeEventChain SubscribeWithTopic<T1>(string topic, Action<string, T1> callback) { return Subscribe(SubscribeEventWithTopic.Create(topic, callback)); }
		public SubscribeEventChain SubscribeWithTopic<T1>(string[] topics, Action<string, T1> callback) { return Subscribe(SubscribeEventWithTopic.Create(topics, callback)); }
		public SubscribeEventChain Subscribe<T1, T2>(string topic, Action<T1, T2> callback) { return Subscribe(SubscribeEvent.Create(topic, callback)); }
		public SubscribeEventChain Subscribe<T1, T2>(string[] topics, Action<T1, T2> callback) { return Subscribe(SubscribeEvent.Create(topics, callback)); }
		public SubscribeEventChain SubscribeWithTopic<T1, T2>(string topic, Action<string, T1, T2> callback) { return Subscribe(SubscribeEventWithTopic.Create(topic, callback)); }
		public SubscribeEventChain SubscribeWithTopic<T1, T2>(string[] topics, Action<string, T1, T2> callback) { return Subscribe(SubscribeEventWithTopic.Create(topics, callback)); }
		public SubscribeEventChain Subscribe<T1, T2, T3>(string topic, Action<T1, T2, T3> callback) { return Subscribe(SubscribeEvent.Create(topic, callback)); }
		public SubscribeEventChain Subscribe<T1, T2, T3>(string[] topics, Action<T1, T2, T3> callback) { return Subscribe(SubscribeEvent.Create(topics, callback)); }
		public SubscribeEventChain SubscribeWithTopic<T1, T2, T3>(string topic, Action<string, T1, T2, T3> callback) { return Subscribe(SubscribeEventWithTopic.Create(topic, callback)); }
		public SubscribeEventChain SubscribeWithTopic<T1, T2, T3>(string[] topics, Action<string, T1, T2, T3> callback) { return Subscribe(SubscribeEventWithTopic.Create(topics, callback)); }
		
		private SubscribeEventChain Subscribe(ISubscribeEvent se)
		{
			if(bookedEvents != null)
			{
				bookedEvents.Add(() => Subscribe(se));
				return new SubscribeEventChain(this, se);
			}
			
			if(!registered)
			{
				registered = true;
				Deliver.Instance.Subscribe(this);
			}
			
			subscribeEvents.Add(se);
			return new SubscribeEventChain(this, se);
		}
		
		public void Dispose() { Unsubscribe(); }
		
		public void Unsubscribe()
		{
			if(bookedEvents != null)
			{
				bookedEvents.Add(() => Unsubscribe());
				return;
			}
			
			for(var i=subscribeEvents.Count-1;i>=0;--i)
			{
				subscribeEvents.RemoveAt(i);
			}
			Deliver.Instance.Unsubscribe(this);
			registered = false;
		}
		
		public void Unsubscribe(string topic)
		{
			if(bookedEvents != null)
			{
				bookedEvents.Add(() => Unsubscribe(topic));
				return;
			}
			
			for(var i=subscribeEvents.Count-1;i>=0;--i)
			{
				if(subscribeEvents[i].RemoveTopic(topic)) subscribeEvents.RemoveAt(i);
			}
		}
		
		public void Unsubscribe(SubscribeEventChain sec)
		{
			Unsubscribe(sec.Event);
		}
		
		public void Unsubscribe(ISubscribeEvent se)
		{
			if(bookedEvents != null)
			{
				bookedEvents.Add(() => Unsubscribe(se));
				return;
			}
			
			for(var i=subscribeEvents.Count-1;i>=0;--i) if(se == subscribeEvents[i]) subscribeEvents.RemoveAt(i);
		}
		
		public void Mute(bool mute = true)
		{
			if(bookedEvents != null)
			{
				bookedEvents.Add(() => Mute(mute));
				return;
			}
			
			for(var i=subscribeEvents.Count-1;i>=0;--i)
			{
				subscribeEvents[i].Muting = mute;
			}
		}
		public void Unmute(){ Mute(false); }
		
		public void Mute(string topic, bool mute = true)
		{
			if(bookedEvents != null)
			{
				bookedEvents.Add(() => Mute(topic, mute));
				return;
			}
			
			for(var i=subscribeEvents.Count-1;i>=0;--i)
			{
				foreach(var t in subscribeEvents[i].Topics)
				{
					if(Util.PatternMatch.IsMatch(t, topic)) subscribeEvents[i].Muting = mute;
				}
			}
		}
		public void Unmute(string topic){ Mute(topic, false); }
		
		public void Mute(ISubscribeEvent se, bool mute = true)
		{
			if(bookedEvents != null)
			{
				bookedEvents.Add(() => Mute(se, mute));
				return;
			}
			
			for(var i=subscribeEvents.Count-1;i>=0;--i) if(se == subscribeEvents[i]) subscribeEvents[i].Muting = mute;
		}
		public void Unmute(ISubscribeEvent se){ Mute(se, false); }
		
		public int Call(string topic) { return Call(topic, new object[]{}); }
		public int Call<T1>(string topic, T1 arg1) { return Call(topic, new object[]{arg1}); }
		public int Call<T1, T2>(string topic, T1 arg1, T2 arg2) { return Call(topic, new object[]{arg1, arg2}); }
		public int Call<T1, T2, T3>(string topic, T1 arg1, T2 arg2, T3 arg3) { return Call(topic, new object[]{arg1, arg2, arg3}); }
		
		public int Call(string topic, object[] args)
		{
			int listenCount = 0;
			
			bool rootEvent = false;
			if(bookedEvents == null)
			{
				rootEvent = true;
				bookedEvents = new List<Action>();
			}
			foreach(var se in subscribeEvents)
			{
				if(se.Muting) continue;
				
				bool match = false;
				foreach(var t in se.Topics)
				{
					if(PatternMatch.IsMatch(t, topic))
					{
						match = true;
						break;
					}
				}
				if(!match) continue;
				listenCount++;
				se.Call(topic, args);
			}
			if(rootEvent)
			{
				var copied = bookedEvents.ToArray();
				bookedEvents = null;
				foreach(var e in copied) e();
			}
			
			return listenCount;
		}
	}
}