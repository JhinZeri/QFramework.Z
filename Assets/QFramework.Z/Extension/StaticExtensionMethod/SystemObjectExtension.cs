using System;

namespace QFramework.Z.Extension.StaticExtensionMethod
{
    /// <summary>
    /// 针对 System.Object 提供的链式扩展，理论上任何对象都可以使用
    /// </summary>
    public static class SystemObjectExtension
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
        public static T Self<T>(this T self, Func<T, T> onDo) => onDo.Invoke(self);

        /// <summary>
        /// 如果这个类对象是 null，则返回 True
        /// </summary>
        /// <param name="selfObj"> 自身对象 </param>
        /// <typeparam name="T"> System.Object </typeparam>
        public static bool IsNull<T>(this T selfObj) where T : class => selfObj == null;

        /// <summary>
        /// 如果这个类对象不是 null，则返回 True
        /// </summary>
        /// <param name="selfObj"> 自身对象 </param>
        /// <typeparam name="T"> System.Object </typeparam>
        public static bool IsNotNull<T>(this T selfObj) where T : class => selfObj != null;

        /// <summary>
        /// 转换对象类型
        /// </summary>
        /// <param name="selfObj"> 自身对象 </param>
        /// <typeparam name="T"> 转换之后的类型 </typeparam>
        /// <returns> 类型转换成功后的自身对象 </returns>
        public static T As<T>(this object selfObj) where T : class => selfObj as T;
    }
}