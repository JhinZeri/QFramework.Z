using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ZQFramework.Toolkits.CommonKit.StaticExtKit
{
    public static class TypeExtension
    {
        /// <summary>
        /// 没有特殊情况时，该方法可以获取整个项目中继承了该类的所有子类,仅查找 Assembly 开头的程序集，通常也就是指自定义的代码编译之后的程序集
        /// </summary>
        /// <param name="self"> 想要查阅的父类 </param>
        /// <returns> 可以遍历的集合 IEnumerable </returns>
        public static IEnumerable<Type> GetSubTypesInProjectAssemblies(this Type self)
        {
            // .NET Core 只有一个 AppDomain，虽然现在 Unity 还没有完全实行 .NET Core
            // 在 .NET Core 上，AppDomain 实现受设计限制，不提供隔离、卸载或安全边界。
            // 对于 .NET Core，只有一个 AppDomain。隔离和卸载通过 AssemblyLoadContext 提供。安全边界应由进程边界和适当的远程处理技术提供。

            // AppDomain 通常只有一个，也就是整个 Unity 项目的全体编译集
            // 不过，如果程序是动态加载的，可能会有多个 AppDomain
            return AppDomain.CurrentDomain.GetAssemblies()
                            .Where(assembly => assembly.FullName.StartsWith("Assembly"))
                            .SelectMany(assembly => assembly.GetTypes())
                            .Where(thisType => thisType.IsSubclassOf(self));
        }

        /// <summary>
        /// 没有特殊情况时，该方法可以获取整个项目中继承了该类的所有子类，并且带有指定的特性
        /// </summary>
        /// <param name="self"> 想要查阅的父类 </param>
        /// <typeparam name="TAttribute"> 自定义的特性 </typeparam>
        /// <returns> 可以遍历的集合 IEnumerable </returns>
        public static IEnumerable<Type> GetSubTypesWithClassAttributeInProjectAssemblies<TAttribute>(this Type self)
            where TAttribute : Attribute
        {
            return self.GetSubTypesInProjectAssemblies()
                       .Where(subType => subType.GetCustomAttribute<TAttribute>() != null);
        }

        /// <summary>
        /// 没有特殊情况时，该方法可以获取整个项目中带有指定的特性的类
        /// </summary>
        /// <param name="self"> 任何 Type </param>
        /// <typeparam name="TAttribute"> 自定义的特性 </typeparam>
        /// <returns> 可以遍历的集合 IEnumerable </returns>
        public static IEnumerable<Type> GetTypesWithClassAttributeInProjectAssemblies<TAttribute>(this Type self)
            where TAttribute : Attribute
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                            .SelectMany(assembly => assembly.GetTypes())
                            .Where(type => type.GetCustomAttribute<TAttribute>() != null);
        }
    }
}