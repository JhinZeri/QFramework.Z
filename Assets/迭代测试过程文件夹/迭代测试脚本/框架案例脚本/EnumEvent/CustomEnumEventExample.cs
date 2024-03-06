using QFramework.Z.CoreKit.EventKit.EventSystem;
using UnityEngine;

namespace 迭代测试过程文件夹.迭代测试脚本.框架案例脚本.EnumEvent
{
    public class CustomEnumEventExample : MonoBehaviour
    {
        public enum TestEvent
        {
            Start,
            EventA,
            EventB,
            End
        }

        public enum 工作地点
        {
            Start = TestEvent.End, // 为了保证每个消息 Id 唯一，需要头尾相接
            Shanghai,
            Beijing,
            End
        }

        // Start is called before the first frame update
        void Start()
        {
            EnumEventSystem.Global.Register(TestEvent.EventA, OnEvent);
        }

        void OnEvent(int ky, params object[] objects)
        {
            switch (ky)
            {
                case (int)TestEvent.EventA:
                    Debug.Log(objects[0]);
                    break;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                EnumEventSystem.Global.Send(TestEvent.EventA, "Hello World");
            }
        }
    }
}