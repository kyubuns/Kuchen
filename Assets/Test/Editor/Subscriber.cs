using NUnit.Framework;

namespace Kuchen.Test
{
	public class SubscriberTest
	{
		[Test]
		public void SubscriveしたらTopicが設定されたEventが帰ってくる()
		{
			var subscriber = new Subscriber();
			var subscribeEvent1 = subscriber.Subscribe("topic1", () => {});
			var subscribeEvent2 = subscriber.SubscribeWithTopic("topic2", (t) => {});
			var subscribeEvent3 = subscriber.SubscribeWithTopic<int>("topic3", (t, a1) => {});
			var subscribeEvent4 = subscriber.SubscribeWithTopic<int, int>("topic4", (t, a1, a2) => {});
			Assert.AreEqual(subscribeEvent1.Event.Topics[0], "topic1");
			Assert.AreEqual(subscribeEvent2.Event.Topics[0], "topic2");
			Assert.AreEqual(subscribeEvent3.Event.Topics[0], "topic3");
			Assert.AreEqual(subscribeEvent4.Event.Topics[0], "topic4");
		}
		
		[Test]
		public void 合ってるTopicだけ呼び出される()
		{
			var subscriber = new Subscriber();
			
			bool topic1 = false;
			subscriber.Subscribe("topic1", () => { topic1 = true; });
			
			bool topic2 = false;
			subscriber.Subscribe("topic2", () => { topic2 = true; });
			
			bool topic3 = false;
			subscriber.Subscribe("topic3", () => { topic3 = true; });
			
			var num = subscriber.Call("topic2");
			
			Assert.IsFalse(topic1);
			Assert.IsTrue(topic2);
			Assert.IsFalse(topic3);
			Assert.AreEqual(num, 1);
		}
		
		[Test]
		public void 色んな引数でSubscribeしてみる()
		{
			var subscriber = new Subscriber();

			bool noArgs = false;
			subscriber.Subscribe("topic1", () => { noArgs = true; });

			bool topicOnly = false;
			subscriber.SubscribeWithTopic("topic1", (topic) => { topicOnly = true; });

			bool args1 = false;
			subscriber.SubscribeWithTopic<string>("topic1", (topic, arg) => { args1 = true; });

			bool args2 = false;
			subscriber.SubscribeWithTopic<string, string>("topic1", (topic, arg1, arg2) => { args2 = true; });
			
			var num = subscriber.Call("topic1", "ami", "mami");
			
			Assert.IsTrue(noArgs);
			Assert.IsTrue(topicOnly);
			Assert.IsTrue(args1);
			Assert.IsTrue(args2);
			Assert.AreEqual(num, 4);
		}
		
		[Test]
		[ExpectedException(typeof(System.InvalidCastException))]
		public void 引数の型を間違うとエラーになる()
		{
			var subscriber = new Subscriber();
			subscriber.SubscribeWithTopic<string>("topic1", (topic, arg) => {});
			subscriber.Call("topic1", 123);
		}
		
		[Test]
		public void Call()
		{
			var subscriber = new Subscriber();
			
			int callNum1 = 0;
			int callNum2 = 0;
			int callNum3 = 0;
			subscriber.Subscribe("topic1", () => { callNum1++; });
			subscriber.Subscribe("topic1", () => { callNum2++; });
			subscriber.Subscribe("topic2", () => { callNum3++; });
			
			subscriber.Call("topic1");
			subscriber.Call("topic2");
			subscriber.Call("topic1");
			subscriber.Call("topic2");
			Assert.AreEqual(callNum1, 2);
			Assert.AreEqual(callNum2, 2);
			Assert.AreEqual(callNum3, 2);
		}
		
		[Test]
		public void UnsubscribeAll()
		{
			var subscriber = new Subscriber();
			
			int callNum1 = 0;
			int callNum2 = 0;
			int callNum3 = 0;
			subscriber.Subscribe("topic1", () => { callNum1++; });
			subscriber.Subscribe("topic1", () => { callNum2++; });
			subscriber.Subscribe("topic2", () => { callNum3++; });
			
			subscriber.Call("topic1");
			subscriber.Call("topic2");
			subscriber.Call("topic1");
			subscriber.Call("topic2");
			
			subscriber.Unsubscribe();
			
			subscriber.Call("topic1");
			subscriber.Call("topic2");
			subscriber.Call("topic1");
			subscriber.Call("topic2");
			Assert.AreEqual(callNum1, 2);
			Assert.AreEqual(callNum2, 2);
			Assert.AreEqual(callNum3, 2);
		}
		
		[Test]
		public void TopicでUnsubscribe()
		{
			var subscriber = new Subscriber();
			
			int callNum1 = 0;
			int callNum2 = 0;
			int callNum3 = 0;
			subscriber.Subscribe("topic1", () => { callNum1++; });
			subscriber.Subscribe("topic1", () => { callNum2++; });
			subscriber.Subscribe("topic2", () => { callNum3++; });
			
			subscriber.Call("topic1");
			subscriber.Call("topic2");
			subscriber.Call("topic1");
			subscriber.Call("topic2");
			
			subscriber.Unsubscribe("topic1");
			
			subscriber.Call("topic1");
			subscriber.Call("topic2");
			subscriber.Call("topic1");
			subscriber.Call("topic2");
			Assert.AreEqual(callNum1, 2);
			Assert.AreEqual(callNum2, 2);
			Assert.AreEqual(callNum3, 4);
		}
		
		[Test]
		public void EventでUnsubscribe()
		{
			var subscriber = new Subscriber();
			
			int callNum1 = 0;
			int callNum2 = 0;
			int callNum3 = 0;
			subscriber.Subscribe("topic1", () => { callNum1++; });
			var e = subscriber.Subscribe("topic1", () => { callNum2++; });
			subscriber.Subscribe("topic2", () => { callNum3++; });
			
			subscriber.Call("topic1");
			subscriber.Call("topic2");
			subscriber.Call("topic1");
			subscriber.Call("topic2");
			
			subscriber.Unsubscribe(e);
			
			subscriber.Call("topic1");
			subscriber.Call("topic2");
			subscriber.Call("topic1");
			subscriber.Call("topic2");
			Assert.AreEqual(callNum1, 4);
			Assert.AreEqual(callNum2, 2);
			Assert.AreEqual(callNum3, 4);
		}
		
		[Test]
		public void SubscribeOnce()
		{
			var go = new UnityEngine.GameObject();
			var mb = go.AddComponent<UnityEngine.MonoBehaviour>();
			
			int callNum1 = 0;
			mb.Subscribe("topic1", () => { callNum1++; }).Once();
			Publisher.Publish("topic1");
			Publisher.Publish("topic1");
			Publisher.Publish("topic1");
			Assert.AreEqual(callNum1, 1);
		}
		
		[Test]
		public void 複数登録したうち1つだけ解除しても動く()
		{
			var subscriber = new Subscriber();
			
			int callNum1 = 0;
			subscriber.Subscribe(new string[]{"topic1", "topic2"}, () => { callNum1++; });
			
			subscriber.Call("topic1");
			subscriber.Call("topic2");
			Assert.AreEqual(callNum1, 2);
			
			subscriber.Unsubscribe("topic1");
			
			subscriber.Call("topic1");
			subscriber.Call("topic2");
			Assert.AreEqual(callNum1, 3);
		}
		
		[Test]
		public void Mute()
		{
			var subscriber = new Subscriber();
			
			int callNum1 = 0;
			subscriber.Subscribe("topic1", () => { callNum1++; });
			
			subscriber.Call("topic1");
			Assert.AreEqual(callNum1, 1);
			
			subscriber.Mute("topic1");
			
			subscriber.Call("topic1");
			Assert.AreEqual(callNum1, 1);
		}
		
		[Test]
		public void Unmute()
		{
			var subscriber = new Subscriber();
			
			int callNum1 = 0;
			subscriber.Subscribe("topic1", () => { callNum1++; });
			subscriber.Mute("topic1");
			
			subscriber.Call("topic1");
			Assert.AreEqual(callNum1, 0);
			
			subscriber.Unmute("topic1");
			
			subscriber.Call("topic1");
			Assert.AreEqual(callNum1, 1);
		}
	}
}
