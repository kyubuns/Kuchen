using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kuchen
{
	public static class GameObjectExtension
	{
		public static ISubscribeEvent Subscribe(this MonoBehaviour behaviour, string topic, Action callback)
		{
			return GetOrAddComponent<KuchenSubscriberGameObject>(behaviour.gameObject).Subscriber.Subscribe(topic, callback);
		}
		
		public static ISubscribeEvent SubscribeOnce(this MonoBehaviour behaviour, string topic, Action callback)
		{
			var subscriber = GetOrAddComponent<KuchenSubscriberGameObject>(behaviour.gameObject).Subscriber;
			ISubscribeEvent se = null;
			se = subscriber.Subscribe(topic, () => {
				subscriber.Unsubscribe(se);
				callback();
			});
			return se;
		}
		
		public static ISubscribeEvent Subscribe(this MonoBehaviour behaviour, string topic, IEnumerator callback)
		{
			return GetOrAddComponent<KuchenSubscriberGameObject>(behaviour.gameObject).Subscriber.Subscribe(topic, () => {
				behaviour.StartCoroutine(callback);
			});
		}
		
		public static ISubscribeEvent Subscribe(this MonoBehaviour behaviour, string topic, Action<string> callback)
		{
			return GetOrAddComponent<KuchenSubscriberGameObject>(behaviour.gameObject).Subscriber.Subscribe(topic, callback);
		}
		
		public static ISubscribeEvent SubscribeOnce(this MonoBehaviour behaviour, string topic, Action<string> callback)
		{
			var subscriber = GetOrAddComponent<KuchenSubscriberGameObject>(behaviour.gameObject).Subscriber;
			ISubscribeEvent se = null;
			se = subscriber.Subscribe(topic, (t) => {
				subscriber.Unsubscribe(se);
				callback(t);
			});
			return se;
		}
		
		public static ISubscribeEvent Subscribe<T1>(this MonoBehaviour behaviour, string topic, Action<string, T1> callback)
		{
			return GetOrAddComponent<KuchenSubscriberGameObject>(behaviour.gameObject).Subscriber.Subscribe(topic, callback);
		}
		
		public static ISubscribeEvent SubscribeOnce<T1>(this MonoBehaviour behaviour, string topic, Action<string, T1> callback)
		{
			var subscriber = GetOrAddComponent<KuchenSubscriberGameObject>(behaviour.gameObject).Subscriber;
			ISubscribeEvent se = null;
			se = subscriber.Subscribe<T1>(topic, (t, a1) => {
				subscriber.Unsubscribe(se);
				callback(t, a1);
			});
			return se;
		}
		
		public static ISubscribeEvent Subscribe<T1, T2>(this MonoBehaviour behaviour, string topic, Action<string, T1, T2> callback)
		{
			return GetOrAddComponent<KuchenSubscriberGameObject>(behaviour.gameObject).Subscriber.Subscribe(topic, callback);
		}
		
		public static ISubscribeEvent SubscribeOnce<T1, T2>(this MonoBehaviour behaviour, string topic, Action<string, T1, T2> callback)
		{
			var subscriber = GetOrAddComponent<KuchenSubscriberGameObject>(behaviour.gameObject).Subscriber;
			ISubscribeEvent se = null;
			se = subscriber.Subscribe<T1, T2>(topic, (t, a1, a2) => {
				subscriber.Unsubscribe(se);
				callback(t, a1, a2);
			});
			return se;
		}
		
		public static void Unsubscribe(this MonoBehaviour behaviour)
		{
			GetOrAddComponent<KuchenSubscriberGameObject>(behaviour.gameObject).Subscriber.Unsubscribe();
		}
		
		public static void Unsubscribe(this MonoBehaviour behaviour, string topic)
		{
			GetOrAddComponent<KuchenSubscriberGameObject>(behaviour.gameObject).Subscriber.Unsubscribe(topic);
		}
		
		public static void Unsubscribe(this MonoBehaviour behaviour, ISubscribeEvent se)
		{
			GetOrAddComponent<KuchenSubscriberGameObject>(behaviour.gameObject).Subscriber.Unsubscribe(se);
		}
		
		public static void Publish(this MonoBehaviour behaviour, string topic) { Publisher.Publish(topic); }
		public static void Publish<T1>(this MonoBehaviour behaviour, string topic, T1 arg1) { Publisher.Publish(topic, arg1); }
		public static void Publish<T1, T2>(this MonoBehaviour behaviour, string topic, T1 arg1, T2 arg2) { Publisher.Publish(topic, arg1, arg2); }
		
		public static Coroutine WaitForMessage(this MonoBehaviour behaviour, string topic, float timeout = 0.0f)
		{
			return behaviour.WaitForMessage(new string[]{topic}, timeout);
		}
		
		public static Coroutine WaitForMessage(this MonoBehaviour behaviour, string[] topics, float timeout = 0.0f)
		{
			var subscriberObject = GetOrAddComponent<KuchenSubscriberGameObject>(behaviour.gameObject);
			return behaviour.StartCoroutine(Util.Coroutine.WaitForMessage(subscriberObject.Subscriber, topics, timeout));
		}
		
		private static T GetOrAddComponent<T>(GameObject gameObject) where T : Component
		{
			var component = gameObject.GetComponent<T>();
			if(component == null) component = gameObject.AddComponent<T>();
			return component;
		}
	}
}