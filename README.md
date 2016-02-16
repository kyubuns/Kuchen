# Kuchen

Pub/Sub Library for Unity

## 使い方

### シンプルに

```C#
using UnityEngine;
using Kuchen;

public class SimpleSample : MonoBehaviour
{
    void Start()
    {
        this.Subscribe("SampleTopic", () => { Debug.Log("baum") });
    }
}
```
```C#
using UnityEngine;
using Kuchen;

public class SimpleButton : MonoBehaviour
{
    void OnClick()
    {
        this.Publish("SampleTopic");
    }
}
```

### 引数付き

```C#
using UnityEngine;
using Kuchen;

public class WithArgs : MonoBehaviour
{
    void Start()
    {
        this.Subscribe<string, int>("SampleTopic", (topic, message, number) => {
            Debug.LogFormat("{0}: {1}", message, number);
        });

        this.Publish("SampleTopic", "test message", 611);
    }
}
```

### コルーチンで

```C#
using System.Collections;
using UnityEngine;
using Kuchen;

public class KuchenCoroutineSample : MonoBehaviour
{
    public IEnumerator Start()
    {
        Debug.Log("SampleTopicが送信されるまで待つよ。");
        yield return this.WaitForMessage("SampleTopic");
        Debug.Log("SampleTopicが呼ばれたよ！");
    }
}
```

### ワイルドカード

```C#
using UnityEngine;
using Kuchen;

public class Wildcard : MonoBehaviour
{
    void Start()
    {
        this.Subscribe("Topic.*", (topic) => { Debug.Log(topic); });

        this.Publish("Topic.Hoge");
        this.Publish("Topic.Fuga");
    }
}
```

### 複数トピック

```C#
using UnityEngine;
using Kuchen;

public class Multiple : MonoBehaviour
{
    void Start()
    {
        this.Subscribe(new string[]{"Topic.Hoge", "Topic.Fuga"}, (topic) => { Debug.Log(topic); });

        this.Publish("Topic.Hoge");
        this.Publish("Topic.Fuga");
    }
}
```

### SubscribeOnce

```C#
using UnityEngine;
using Kuchen;

public class SubscribeOnce : MonoBehaviour
{
    void Start()
    {
        this.SubscribeOnce("SampleTopic", (topic) => { Debug.Log(topic); });

        this.Publish("SampleTopic");
        this.Publish("SampleTopic"); // 2回目は呼び出されない
    }
}
```

### SubscribeWithCoroutine

```C#
using System.Collections;
using UnityEngine;
using Kuchen;

public class SubscribeWithCoroutine : MonoBehaviour
{
    void Start()
    {
        this.SubscribeWithCoroutine("SampleTopic", Coroutine);
        this.Publish("SampleTopic");
    }

    IEnumerator Coroutine()
    {
        yield return null;
    }
}
```

### Mute

```C#
using System.Collections;
using UnityEngine;
using Kuchen;

public class SubscribeWithCoroutine : MonoBehaviour
{
    void Start()
    {
        this.SubscribeWithCoroutine("SampleTopic", () => { Debug.Log("!"); });

        this.Publish("SampleTopic");

        this.Mute("SampleTopic");
        this.Publish("SampleTopic"); // Muteしてる間は呼ばれない
        this.Unmute("SampleTopic");

        this.Publish("SampleTopic");
    }
}
```

### GameObject無し

```C#
using Kuchen;

public class NonGameObject
{
	void SubscribeTest()
	{
		using(var subscriber = new Subscriber())
		{
			subscriber.Subscribe("SampleTopic", () => { /* hoge */ });
			Publisher.Publish("SampleTopic");
		}
	}
}
```


## Special Thanks

* Y.O.
* MiniRegex(@kimika127)
