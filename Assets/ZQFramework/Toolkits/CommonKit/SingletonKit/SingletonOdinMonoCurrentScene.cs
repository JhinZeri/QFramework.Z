#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
using UnityEngine;

namespace ZQFramework.Toolkits.CommonKit.SingletonKit
{
    /// <summary>
    /// 继承了 Odin 序列化 Mono 的单例抽象类，只在当前场景内有效，在场景切换时会被销毁，默认销毁新生成的物体
    /// 需要自己预先提供场景中挂载了对应脚本的物体
    /// </summary>
    public abstract class SingletonOdinMonoCurrentScene<T> : SerializedMonoBehaviour
        where T : SingletonOdinMonoCurrentScene<T>
    {
        static T m_Instance;

        public static T Instance
        {
            get
            {
                if (m_Instance != null) return m_Instance;

                Debug.LogError("目前场景中并没有存在挂载了 T 类型脚本的物体，请自行生成！");
                return null;
            }
        }

        protected virtual void Awake()
        {
            if (m_Instance != null)
                Destroy(gameObject);
            else
                m_Instance = this as T;
        }

        protected virtual void OnDestroy()
        {
            OnBeforeDestroy();

            if (m_Instance == this) m_Instance = null;
        }

        /// <summary>
        /// 在 OnDestroy 中执行，在 instance 没有赋值 null 之前，销毁前的操作写在该方法中，不要重写 OnDestroy
        /// </summary>
        protected abstract void OnBeforeDestroy();
    }
}
#endif