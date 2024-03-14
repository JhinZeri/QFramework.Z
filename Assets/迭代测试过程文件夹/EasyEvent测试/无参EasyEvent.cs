using System;
using UnityEngine;
using ZQFramework.Framework.Event;

namespace 迭代测试过程文件夹.EasyEvent测试
{
    public class 无参EasyEvent : MonoBehaviour
    {
        // 创建一个无参的EasyEvent
        public EasyEvent FirstEasyEvent = new EasyEvent();
        
        // 在Start方法中订阅EasyEvent
        void Start()
        {
            Debug.Log("在 Start 方法中订阅 EasyEvent，EasyEvent 是 Action 的封装，实际使用中可以把它当成普通 Action 使用" +
                      "只是提供了一些额外的功能，比如订阅后可以自动注销，可以一键输出所有订阅者的方法名列表");
            FirstEasyEvent.Register(() =>
                          {
                              Debug.Log("Lambda 表达式，EasyEvent被触发");
                          })
                          .UnRegisterWhenGameObjectDestroyed(this);
            FirstEasyEvent.Register(GoGo);
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                FirstEasyEvent.Trigger();
            }

            if (Input.GetMouseButtonDown(1))
            {
                foreach (string methodName in FirstEasyEvent.GetMethodNameList())
                {
                    Debug.Log(methodName);
                }
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("注销 GOGO 方法");
                FirstEasyEvent.UnRegister(GoGo);
            }
        }

        static void GoGo()
        {
            Debug.Log("非 Lambda表达式，GOGO 被触发");
        }
    }
}