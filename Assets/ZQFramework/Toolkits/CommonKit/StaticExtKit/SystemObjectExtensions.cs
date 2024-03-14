using System;
using System.Linq;

namespace ZQFramework.Toolkits.CommonKit.StaticExtKit
{
    /// <summary>
    /// 针对 System.Object 提供的链式扩展，理论上任何对象都可以使用
    /// </summary>
    public static class SystemObjectExtensions
    {
        /// <summary>
        /// 将自己作为参数传递给一个 Action
        /// </summary>
        /// <param name="self"> 自身对象 </param>
        /// <param name="onDo"> Action 委托 </param>
        /// <typeparam name="T"> Action 委托参数类型 </typeparam>
        /// <returns> 自身对象 </returns>
        public static T Self<T>(this T self, Action<T> onDo)
        {
            onDo?.Invoke(self);
            return self;
        }

        /// <summary>
        /// 将自己作为参数传递给一个 Func
        /// </summary>
        /// <param name="self"> 自身对象 </param>
        /// <param name="onDo"> Fuc 委托，带返回值 </param>
        /// <typeparam name="T"> Func 委托参数类型 </typeparam>
        /// <returns> 自身对象 </returns>
        public static T Self<T>(this T self, Func<T, T> onDo)
        {
            return onDo.Invoke(self);
        }

        /// <summary>
        /// 如果这个类对象是 null，则返回 True
        /// </summary>
        /// <param name="selfObj"> 自身对象 </param>
        /// <typeparam name="T"> System.Object </typeparam>
        public static bool IsNull<T>(this T selfObj) where T : class
        {
            return selfObj == null;
        }

        /// <summary>
        /// 如果这个类对象不是 null，则返回 True
        /// </summary>
        /// <param name="selfObj"> 自身对象 </param>
        /// <typeparam name="T"> System.Object </typeparam>
        public static bool IsNotNull<T>(this T selfObj) where T : class
        {
            return selfObj != null;
        }

        /// <summary>
        /// 转换对象类型
        /// </summary>
        /// <param name="selfObj"> 自身对象 </param>
        /// <typeparam name="T"> 转换之后的类型 </typeparam>
        /// <returns> 类型转换成功后的自身对象 </returns>
        public static T As<T>(this object selfObj) where T : class
        {
            return selfObj as T;
        }

        /// <summary>
        /// 检查这个 object 是否与其他的所有 object 都相等 / Checks if the <paramref name="obj" /> equals to all elements of
        /// <paramref name="objects" /> array.
        /// </summary>
        /// <param name="obj"> Object to compare. </param>
        /// <param name="objects"> Array with objects to compare. </param>
        /// <returns>
        /// <see langword="true" /> if the all element of <paramref name="objects" /> are equals to
        /// <paramref name="obj" />
        /// </returns>
        public static bool EqualsToAll(this object obj, params object[] objects)
        {
            return objects.All(o => o.Equals(obj));
        }

        /// <summary>
        /// 检查这个 object 是否与其他 object 相比，至少有一个相等
        /// </summary>
        public static bool EqualsToAny(this object obj, params object[] objects)
        {
            return objects.Any(o => o.Equals(obj));
        }
    }
}