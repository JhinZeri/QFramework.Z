using System;
using UnityEngine;

// using Sirenix.OdinInspector;

namespace QFramework.Z.Extension.ScriptReuseUtility.SingletonUtility
{
    /*单例基础三要素
    1.私有静态变量，保存单一实例的引用
    2.非公共构造函数(private 或者 protect)，防止外部直接实例化
    3.公共静态方法(属性)，用于返回单一实例的引用*/

    /// <summary>
    /// C# 类对象的单例抽象类
    /// </summary>
    /// <typeparam name="T"> 继承单例的类型，单纯的 C# 对象，不继承 Mono </typeparam>
    public abstract class Singleton<T> where T : class
    {
        static readonly object LockObject = new(); // 创建一个静态只读的对象作为锁，在多线程时使用      

        static T _instance; // 创建一个静态字段来存储单例实例
        // 受保护的构造函数，确保该类不能被直接实例化

        public static T Instance // 公共静态属性表示获取单例实例
        {
            get
            {
                if (_instance != null) return _instance;
                lock (LockObject) // 加锁以确保多线程环境下只有一个线程可以访问下面的代码块
                {
                    try // 尝试执行下面的代码
                    {
                        _instance ??= Activator.CreateInstance(typeof(T)) as T;
                    }
                    catch (Exception ex) // 捕获异常
                    {
                        Debug.LogException(ex); // 在捕获异常时释放锁，避免死锁或资源泄漏
                        _instance = null; // 捕获到异常直接赋值 null
                    }
                }

                // lock 语句执行完毕就会释放锁
                return _instance;
            }
        }
    }
}