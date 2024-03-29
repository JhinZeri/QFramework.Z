using System;
using UnityEngine;
using ZQFramework.Framework.Event;

namespace 迭代测试过程文件夹.迭代测试脚本.框架案例脚本.EasyEvent
{
    public class CustomEasyEventExample : MonoBehaviour
    {
        readonly EasyEvent<string> m_EasyEvent = new();

        void Awake()
        {
            m_EasyEvent.Register(str =>
                      {
                          Debug.Log(str);
                      })
                      .UnRegisterWhenGameObjectDestroyed(gameObject);
            EasyEvents.Register<EasyEvent<int>>();
            EasyEvents.Register<TestEvent>();

            EasyEvents.Get<EasyEvent<int>>()
                      .Register(number =>
                      {
                          Debug.Log(number);
                      })
                      .UnRegisterWhenGameObjectDestroyed(gameObject);

            EasyEvents.Get<TestEvent>()
                      .Register((str1, str2) =>
                      {
                          Debug.Log(str1 + ":" + str2);
                      })
                      .UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_EasyEvent.Trigger("Hello ZQFramework");
            }

            if (Input.GetMouseButtonDown(1))
            {
                EasyEvents.Get<EasyEvent<int>>().Trigger(123);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                EasyEvents.Get<TestEvent>().Trigger("你好！", "框架");
            }
        }

        public class TestEvent : EasyEvent<string, string> { }
    }
}