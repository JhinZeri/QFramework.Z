using UnityEngine;

namespace ZQFramework.Toolkits.CommonKit.SingletonKit
{
    /// <summary>
    /// 继承了 Mono 的单例抽象类，默认销毁新生成的单例物体
    /// 只要在编辑器模式下调用 Instance 也会生成单例物体
    /// 自动进入 DontDestroyOnLoad，不会在场景切换时销毁
    /// 只会在程序退出时销毁，所以不需要设计 OnDestroy
    /// </summary>
    /// <typeparam name="T"> 继承单例的类型，继承 Mono </typeparam>
    public abstract class SingletonMonoDontDestroyAlways<T> : MonoBehaviour where T : SingletonMonoDontDestroyAlways<T>
    {
        static T m_Instance;
        static readonly object LockObject = new();

        public static T Instance
        {
            get
            {
                if (m_Instance == null)
                    lock (LockObject)
                    {
                        m_Instance.EnsureGameObjectExist();
                    }

                return m_Instance;
            }
        }

        protected virtual void Awake()
        {
            if (m_Instance == null)
            {
                m_Instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// 提供了虚方法 OnBeforeDestroy ，在 OnDestroy 中执行，销毁前的操作写在该方法中，不要重写 OnDestroy
        /// </summary>
        protected virtual void OnDestroy()
        {
            OnBeforeDestroy();

            if (m_Instance == this) m_Instance = null;
        }

        /// <summary>
        /// 确保 GameObject 存在，如果 GameObject 不存在，则创建一个新实例，同时虚方法可以让子类重写，自定义确保存在的方式
        /// </summary>
        protected virtual void EnsureGameObjectExist()
        {
            if (m_Instance == null)
            {
                // 查找场景中是否已存在该类型的实例
                m_Instance = FindFirstObjectByType<T>();

                // 如果场景中不存在该实例，则创建一个新实例
                if (m_Instance == null)
                {
                    var singletonObject = new GameObject(typeof(T).Name);
                    m_Instance = singletonObject.AddComponent<T>();
                }
            }
        }

        /// <summary>
        /// 在 OnDestroy 中执行，在 instance 没有赋值 null 之前，销毁前的操作写在该方法中，不要重写 OnDestroy
        /// </summary>
        protected virtual void OnBeforeDestroy() { }
    }
}