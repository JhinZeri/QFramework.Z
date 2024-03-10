using System;
using System.Collections.Generic;
using System.Linq;

namespace ZQFramework.Toolkits.CommonKit.IOCKit
{
    /// <summary>
    /// 简易通用 IOC 容器
    /// </summary>
    public class SimpleIOCContainer
    {
        readonly Dictionary<Type, object> m_InstanceDictionary = new();

        public void Register<T>(T instance)
        {
            var key = typeof(T);

            // 如果存在这个键，那么就存在一个具体的实例对象，不需要用新的对象替换它，因此直接返回，不进行任何操作
            m_InstanceDictionary.TryAdd(key, instance);
        }

        public T Get<T>() where T : class
        {
            var key = typeof(T);

            if (m_InstanceDictionary.TryGetValue(key, out object retInstance)) return retInstance as T;

            return null;
        }

        public IEnumerable<T> GetInstancesByType<T>()
        {
            var type = typeof(T);
            return m_InstanceDictionary.Values.Where(instance => type.IsInstanceOfType(instance)).Cast<T>();
        }

        public void Clear()
        {
            m_InstanceDictionary.Clear();
        }
    }
}