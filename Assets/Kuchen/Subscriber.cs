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
		
		public ISubscribeEvent Subscribe(string topic, Action callback) { return Subscribe(SubscribeEvent.Create(topic, callback)); }
		public ISubscribeEvent Subscribe(string[] topics, Action callback) { return Subscribe(SubscribeEvent.Create(topics, callback)); }
		public ISubscribeEvent Subscribe(string topic, Action<string> callback) { return Subscribe(SubscribeEvent.Create(topic, callback)); }
		public ISubscribeEvent Subscribe(string[] topics, Action<string> callback) { return Subscribe(SubscribeEvent.Create(topics, callback)); }
		public ISubscribeEvent Subscribe<T1>(string topic, Action<string, T1> callback) { return Subscribe(SubscribeEvent.Create(topic, callback)); }
		public ISubscribeEvent Subscribe<T1>(string[] topics, Action<string, T1> callback) { return Subscribe(SubscribeEvent.Create(topics, callback)); }
		public ISubscribeEvent Subscribe<T1, T2>(string topic, Action<string, T1, T2> callback) { return Subscribe(SubscribeEvent.Create(topic, callback)); }
		public ISubscribeEvent Subscribe<T1, T2>(string[] topics, Action<string, T1, T2> callback) { return Subscribe(SubscribeEvent.Create(topics, callback)); }
		
		private ISubscribeEvent Subscribe(ISubscribeEvent se)
		{
			if(bookedEvents != null)
			{
				bookedEvents.Add(() => Subscribe(se));
				return se;
			}
			
			if(!registered)
			{
				registered = true;
				Deliver.Instance.Subscribe(this);
			}
			
			subscribeEvents.Add(se);
			return se;
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
		
		public void Unsubscribe(ISubscribeEvent se)
		{
			if(bookedEvents != null)
			{
				bookedEvents.Add(() => Unsubscribe(se));
				return;
			}
			
			for(var i=subscribeEvents.Count-1;i>=0;--i) if(se == subscribeEvents[i]) subscribeEvents.RemoveAt(i);
		}
		
		public void Pause(bool active = false)
		{
			if(bookedEvents != null)
			{
				bookedEvents.Add(() => Pause(active));
				return;
			}
			
			for(var i=subscribeEvents.Count-1;i>=0;--i)
			{
				subscribeEvents[i].Pausing = !active;
			}
		}
		public void Resume(){ Pause(true); }
		
		public void Pause(string topic, bool active = false)
		{
			if(bookedEvents != null)
			{
				bookedEvents.Add(() => Pause(topic, active));
				return;
			}
			
			for(var i=subscribeEvents.Count-1;i>=0;--i)
			{
				if(Array.IndexOf(subscribeEvents[i].Topics, topic) >= 0) subscribeEvents[i].Pausing = !active;
			}
		}
		public void Resume(string topic){ Pause(topic, true); }
		
		public void Pause(ISubscribeEvent se, bool active = false)
		{
			if(bookedEvents != null)
			{
				bookedEvents.Add(() => Pause(se, active));
				return;
			}
			
			for(var i=subscribeEvents.Count-1;i>=0;--i) if(se == subscribeEvents[i]) subscribeEvents[i].Pausing = !active;
		}
		public void Resume(ISubscribeEvent se){ Pause(se, true); }
		
		public int Call(string topic) { return Call(topic, new object[]{}); }
		public int Call<T1>(string topic, T1 arg1) { return Call(topic, new object[]{arg1}); }
		public int Call<T1, T2>(string topic, T1 arg1, T2 arg2) { return Call(topic, new object[]{arg1, arg2}); }
		
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
				if(se.Pausing) continue;
				
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