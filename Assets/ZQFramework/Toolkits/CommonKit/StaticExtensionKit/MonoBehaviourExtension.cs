using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ZQFramework.Toolkits.CommonKit.StaticExtensionKit
{
    public static class MonoBehaviourExtension
    {
        public static void Example()
        {
            var gameObject = new GameObject();
            var component = gameObject.GetComponent<MonoBehaviour>();

            component.Enable(); // component.enabled = true
            component.Disable(); // component.enabled = false
        }

        /// <summary>
        /// 激活自身物体
        /// </summary>
        public static void ShowSelf(this MonoBehaviour monoBehaviour)
        {
            monoBehaviour.gameObject.SetActive(true);
        }

        /// <summary>
        /// 隐藏自身物体
        /// </summary>
        public static void HideSelf(this MonoBehaviour monoBehaviour)
        {
            monoBehaviour.gameObject.SetActive(false);
        }

        /// <summary>
        /// 打开组件
        /// </summary>
        /// <param name="selfBehaviour"></param>
        /// <param name="enable"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Enable<T>(this T selfBehaviour, bool enable = true) where T : Behaviour
        {
            selfBehaviour.enabled = enable;
            return selfBehaviour;
        }


        /// <summary>
        /// 关闭组件
        /// </summary>
        /// <param name="selfBehaviour"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Disable<T>(this T selfBehaviour) where T : Behaviour
        {
            selfBehaviour.enabled = false;
            return selfBehaviour;
        }

        /// <summary>
        /// 简易延迟回调方法
        /// </summary>
        /// <returns> 返回 Coroutine 即可以存档这个协程为变量 </returns>
        public static Coroutine WaitTimeDo(this MonoBehaviour monoBehaviour, float seconds, UnityAction onFinished)
        {
            return monoBehaviour.StartCoroutine(WaitCoroutine(seconds, onFinished));

            static IEnumerator WaitCoroutine(float sec, UnityAction action)
            {
                yield return new WaitForSeconds(sec);
                action?.Invoke();
            }
        }

        /// <summary>
        /// 在 Coroutine 的过程中停止
        /// </summary>
        /// <param name="monoBehaviour"> 自身 </param>
        /// <param name="coroutine"> 想要停止的 Coroutine </param>
        public static void CancelWaitDo(this MonoBehaviour monoBehaviour, ref Coroutine coroutine)
        {
            if (coroutine != null) monoBehaviour.StopCoroutine(coroutine);

            coroutine = null;
        }
    }
}