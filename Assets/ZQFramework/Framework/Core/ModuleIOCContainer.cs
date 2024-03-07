using System;
using System.Collections.Generic;

namespace ZQFramework.Framework.Core
{
    /// <summary>
    /// 架构模块容器抽象类
    /// </summary>
    /// <typeparam name="TModule"> 模块接口 </typeparam>
    public abstract class ModuleIOCContainer<TModule>
    {
        public readonly Dictionary<Type, TModule> InstanceDictionary = new();

        public void Register<T>(T instance) where T : TModule
        {
            var key = typeof(T);

            // 如果存在这个键，那么就存在一个具体的实例对象，不需要用新的对象替换它，因此直接返回，不进行任何操作
            InstanceDictionary.TryAdd(key, instance);
        }

        public T TryGetModule<T>() where T : class, TModule
        {
            var key = typeof(T);

            if (InstanceDictionary.TryGetValue(key, out var module)) return module as T;

            return null;
        }

        public void Clear()
        {
            InstanceDictionary.Clear();
        }
    }

    public class ModelContainer : ModuleIOCContainer<IModel> { }

    public class SystemContainer : ModuleIOCContainer<ISystem> { }

    public class UtilityContainer : ModuleIOCContainer<IUtility> { }
}