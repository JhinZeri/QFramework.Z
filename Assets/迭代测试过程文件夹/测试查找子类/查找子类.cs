using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using ZQFramework.Framework.Core;
using ZQFramework.Toolkits.CommonKit.StaticExtKit;
using ZQFramework.Toolkits.EditorKit.SimulationEditor;

namespace 迭代测试过程文件夹.测试查找子类
{
    public class 查找子类 : MonoBehaviour
    {
        [Button("查找 <父类或基类> 的子类")]
        public void FindScript()
        {
            var abstractType = typeof(父类或基类);
            List<Type> list = ScriptUtil.FindIsSubClassOf(abstractType);
            foreach (var type in list)
            {
                Debug.Log("子类有：" + type.FullName);
            }
        }

        [Button("查找 <抽象父类> 的子类")]
        public void FindScript2()
        {
            var abstractType = typeof(抽象父类);
            List<Type> list = ScriptUtil.FindIsSubClassOf(abstractType);
            foreach (var type in list)
            {
                Debug.Log("子类有：" + type.FullName);
            }
        }

        [Button("查找 <泛型父类<泛型类的子类>> 的子类")]
        public void FindScript3()
        {
            var abstractType = typeof(泛型父类<泛型类的子类>);
            List<Type> list = ScriptUtil.FindIsSubClassOf(abstractType);
            foreach (var type in list)
            {
                Debug.Log("子类有：" + type.FullName);
            }
        }

        [Button("查找 < 泛型父类<> > 的子类")]
        public void Find4()
        {
            Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集

            List<Type> architectureSubclasses = new List<Type>();
            foreach (Type type in assembly.GetTypes())
            {
                if (type.BaseType != null && type.BaseType.IsGenericType &&
                    type.BaseType.GetGenericTypeDefinition() == typeof(泛型父类<>))
                {
                    architectureSubclasses.Add(type);
                }
            }

            Console.WriteLine("The following subclasses of 'Architecture' have been found:");
            foreach (Type subclass in architectureSubclasses)
            {
                Console.WriteLine(subclass.FullName);
            }
        }

        [Button("查找 < 泛型父类<泛型类的子类> > 的子类，使用凉鞋的方法")]
        public void Find6()
        {
            var type = typeof(泛型父类<泛型类的子类>);
            type.GetSubTypesInProjectAssemblies();
            foreach (var item in type.GetSubTypesInProjectAssemblies())
            {
                Debug.Log(item.Name);
            }
        }

        [Button("检测 < 泛型类的子类 > 的父类属性")]
        public void Find5()
        {
            var type = typeof(泛型类的子类);
            Debug.Log("type.GetNiceFullName(): " + type.GetNiceFullName());
            Debug.Log("type.BaseType.GetNiceFullName(): " + type.BaseType.GetNiceFullName());
            Debug.Log("type.BaseType is { IsGenericType: true }: " + (type.BaseType is { IsGenericType: true }));
        }

        [Button("查找继承了 < IArchitecture > 接口的子类")]
        public void Find8()
        {
            Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集

            List<Type> interfaceImplementingClasses = new List<Type>();
            Type interfaceType = typeof(IArchitecture);
            foreach (Type type in assembly.GetTypes())
            {
                if (interfaceType.IsAssignableFrom(type) && type.IsClass)
                {
                    interfaceImplementingClasses.Add(type);
                }
            }

            Console.WriteLine("The following classes implement 'IMyInterface':");
            foreach (Type implementingClass in interfaceImplementingClasses)
            {
                Console.WriteLine(implementingClass.FullName);
            }
        }

        [Button("查找继承了 < IFulei > 接口的子类")]
        public void Find()
        {
            Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集

            List<Type> interfaceImplementingClasses = new List<Type>();
            Type interfaceType = typeof(IFulei);
            Debug.Log(interfaceType.FullName);
            foreach (Type type in assembly.GetTypes())
            {
                if (interfaceType.IsAssignableFrom(type) && type.IsClass)
                {
                    interfaceImplementingClasses.Add(type);
                }
            }

            Console.WriteLine($"The following classes implement '{interfaceType.Name}':");
            foreach (Type implementingClass in interfaceImplementingClasses)
            {
                Console.WriteLine(implementingClass.FullName);
            }
        }

        [Button("使用字符串，查找 < 泛型父类<泛型类的子类> > 的子类，使用凉鞋的方法")]
        public void Find0()
        {
            var type = Type.GetType(
                "迭代测试过程文件夹.测试查找子类.泛型父类<泛型类的子类>, Assembly-Csharp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            string typeName =
                "迭代测试过程文件夹.测试查找子类.泛型父类<泛型类的子类>`1[[迭代测试过程文件夹.测试查找子类.泛型父类, Assembly-Csharp]], Assembly-Csharp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";

            Type parentGenericType = Type.GetType(typeName);
            Debug.Log(parentGenericType.FullName);
            var subTypes = Assembly.GetExecutingAssembly()
                                   .GetTypes()
                                   .Where(t => t.IsSubclassOf(parentGenericType))
                                   .ToList();

            foreach (var subType in subTypes)
            {
                Console.WriteLine(subType.FullName);
            }
        }

        /// <summary>
        /// 可行！！！
        /// </summary>
        [Button("使用AI的办法，查找 < 泛型父类<泛型类的子类> > 的子类")]
        public static void Findd()
        {
            Type parentGenericType = typeof(泛型父类<>);
            Debug.Log(parentGenericType.FullName);
            var subTypes = Assembly.GetExecutingAssembly()
                                   .GetTypes()
                                   .Where(t => t.BaseType != null && t.BaseType.IsGenericType &&
                                               t.BaseType.GetGenericTypeDefinition() == typeof(泛型父类<>) &&
                                               t.BaseType.GetGenericArguments()[0] == t)
                                   .ToList();

            Debug.Log(subTypes.Count);
            foreach (var subType in subTypes)
            {
                Debug.Log(subType.FullName);
            }
        }
        [Button("测试Util ，查找 < 泛型父类<泛型类的子类> > 的子类")]
        public static void Finds()
        {
            var type = typeof(泛型父类<>);
            const string path = "Assets/迭代测试过程文件夹/测试查找子类";
            ScriptUtil.FindIsGenericSubClassOfInFolderReturnPath(type, path);
        }
    }
}