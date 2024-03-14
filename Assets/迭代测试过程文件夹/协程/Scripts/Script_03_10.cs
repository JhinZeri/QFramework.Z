using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace 迭代测试过程文件夹.协程.Scripts
{
    public class Script_03_10 : MonoBehaviour
    {
        IEnumerator Start()
        {
            yield return new CustomWait(100f, 1f, () =>
            {
                Debug.Log($"每过1秒执行一次: {Time.time}");
            });
            Debug.Log("10秒执行结束");
        }

        public class CustomWait : CustomYieldInstruction
        {
            // 这个值返回为 false 就表示结束协程，不再等待
            public override bool keepWaiting
            {
                get
                {
                    if (Time.time - m_StartTime >= m_Time)
                    {
                        return false;
                    }

                    if (Time.time - m_LastTime >= m_Interval)
                    {
                        m_IntervalCallback?.Invoke();
                        m_LastTime = Time.time;
                    }

                    return true;
                }
            }

            UnityAction m_IntervalCallback;
            float m_StartTime;
            float m_LastTime;
            float m_Interval;
            float m_Time;

            public CustomWait(float time, float interval, UnityAction callback)
            {
                m_StartTime = Time.time;
                m_LastTime = Time.time;
                m_Interval = interval;
                m_Time = time;
                m_IntervalCallback = callback;
            }
        }
    }
}