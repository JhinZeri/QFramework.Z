using System;
using System.Collections;
using UnityEngine;

namespace 迭代测试过程文件夹.协程.Scripts
{
    public class Script_03_08 : MonoBehaviour
    {
        int m_CacheValue = -1;

        public void UpdateValue(int value)
        {
            Debug.Log($"第 {Time.frameCount} 帧尝试赋值 {value} ");
            if (m_CacheValue == -1)
            {
                StartCoroutine(Wait());
                print("当前值为: " + m_CacheValue);
            }

            print("执行到这里了");
            m_CacheValue = value;
            print("当前值为: " + m_CacheValue);
        }

        IEnumerator Wait()
        {
            yield return new WaitForEndOfFrame();
            Debug.Log($"第 {Time.frameCount} 帧最终处理值 {m_CacheValue} ");
            m_CacheValue = -1;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                UpdateValue(2);
                UpdateValue(3);
            }
        }
    }
}