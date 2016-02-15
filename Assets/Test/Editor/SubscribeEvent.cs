using NUnit.Framework;

namespace Kuchen.Test
{
	namespace SubscribeEventTest
	{
		public class NoArgs
		{
			[Test]
			public void Create()
			{
				var se = SubscribeEvent.Create("topic1", () => {});
				Assert.AreEqual(se.Topics[0], "topic1");
			}
			
			[Test]
			public void Call()
			{
				bool called = false;
				var se = SubscribeEvent.Create("topic1", () => { called = true; });
				se.Call("dummy call topic", new object[]{});
				Assert.IsTrue(called);
			}
		}
		
		public class TopicOnly
		{
			[Test]
			public void Create()
			{
				var se = SubscribeEvent.Create("topic1", (t) => {});
				Assert.AreEqual(se.Topics[0], "topic1");
			}
			
			[Test]
			public void Call()
			{
				bool called = false;
				string topic = null;
				var se = SubscribeEvent.Create("topic1", (t) => {
					called = true;
					topic = t;
				});
				se.Call("dummy call topic", new object[]{});
				Assert.IsTrue(called);
				Assert.AreEqual(topic, "dummy call topic");
			}
		}
		
		public class Args1
		{
			[Test]
			public void Create()
			{
				var se = SubscribeEvent.Create<int>("topic1", (t, a1) => {});
				Assert.AreEqual(se.Topics[0], "topic1");
			}
			
			[Test]
			public void Call()
			{
				bool called = false;
				string topic = null;
				int arg1 = 0;
				var se = SubscribeEvent.Create<int>("topic1", (t, a1) => {
					called = true;
					topic = t;
					arg1 = a1;
				});
				se.Call("dummy call topic", new object[]{ 192 });
				Assert.IsTrue(called);
				Assert.AreEqual(topic, "dummy call topic");
				Assert.AreEqual(arg1, 192);
			}
		}
		
		public class Args2
		{
			[Test]
			public void Create()
			{
				var se = SubscribeEvent.Create<int, string>("topic1", (t, a1, a2) => {});
				Assert.AreEqual(se.Topics[0], "topic1");
			}
			
			[Test]
			public void Call()
			{
				bool called = false;
				string topic = null;
				int arg1 = 0;
				string arg2 = null;
				var se = SubscribeEvent.Create<int, string>("topic1", (t, a1, a2) => {
					called = true;
					topic = t;
					arg1 = a1;
					arg2 = a2;
				});
				se.Call("dummy call topic", new object[]{ 168, "kyu" });
				Assert.IsTrue(called);
				Assert.AreEqual(topic, "dummy call topic");
				Assert.AreEqual(arg1, 168);
				Assert.AreEqual(arg2, "kyu");
			}
		}
	}
}
