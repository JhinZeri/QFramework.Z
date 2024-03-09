using Sirenix.OdinInspector;
using UnityEngine;

namespace 迭代测试过程文件夹.迭代测试脚本.特性测试脚本
{
    public class ShowInInspectorExample : MonoBehaviour
    {
        [ShowInInspector]
        int m_MyNumber;

        [Button("输出")]
        public void ShowInInspector()
        {
            Debug.Log("MyNumber is " + m_MyNumber);
        }
    }
}