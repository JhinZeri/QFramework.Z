using System;
using UnityEngine;

namespace ZQFramework.Toolkits.CommonKit.StaticExtensionKit
{
    /// <summary>
    /// UnityEngine.GameObject 的扩展方法
    /// </summary>
    public static class GameObjectExtension
    {
        /// <summary>
        /// 显示 GameObject
        /// </summary>
        /// <param name="selfObj"> 自身物体 </param>
        /// <returns> 自身物体 </returns>
        public static GameObject Show(this GameObject selfObj)
        {
            selfObj.SetActive(true);
            return selfObj;
        }

        /// <summary>
        /// 显示挂载了这个组件的 GameObject
        /// </summary>
        /// <param name="selfComponent"> 自身组件 </param>
        /// <typeparam name="T"> 目标组件类型 </typeparam>
        /// <returns> 自身组件 </returns>
        public static T Show<T>(this T selfComponent) where T : Component
        {
            selfComponent.gameObject.Show();
            return selfComponent;
        }

        /// <summary>
        /// 隐藏 GameObject
        /// </summary>
        /// <param name="selfObj"> 自身物体 </param>
        /// <returns> 自身物体 </returns>
        public static GameObject Hide(this GameObject selfObj)
        {
            selfObj.SetActive(false);
            return selfObj;
        }

        /// <summary>
        /// 隐藏挂载了这个组件的 GameObject
        /// </summary>
        /// <param name="selfComponent"> 自身组件 </param>
        /// <typeparam name="T"> 目标组件类型 </typeparam>
        /// <returns> 自身组件 </returns>
        public static T Hide<T>(this T selfComponent) where T : Component
        {
            selfComponent.gameObject.Hide();
            return selfComponent;
        }

        /// <summary>
        /// 设置 GameObject 的 layer 层
        /// 注意：这个方法是直接设置 layer 值，而不是通过 Renderer 进行设置
        /// </summary>
        /// <param name="selfObj"> 自身物体 </param>
        /// <param name="layer"> 层级 int </param>
        /// <returns> 自身物体 </returns>
        public static GameObject Layer(this GameObject selfObj, int layer)
        {
            selfObj.layer = layer;
            return selfObj;
        }

        /// <summary>
        /// 设置 GameObject 的 layer 层
        /// 注意：这个方法是直接设置 layer 值，而不是通过 Renderer 进行设置
        /// </summary>
        /// <param name="selfObj"> 自身物体 </param>
        /// <param name="layerName"> 层 string </param>
        /// <returns> 自身物体 </returns>
        public static GameObject Layer(this GameObject selfObj, string layerName)
        {
            selfObj.layer = LayerMask.NameToLayer(layerName);
            return selfObj;
        }

        /// <summary>
        /// 设置 Component 的 layer 层
        /// 注意：这个方法是直接设置 layer 值，而不是通过 Renderer 进行设置
        /// </summary>
        /// <param name="selfComponent"> 自身组件 </param>
        /// <param name="layer"> 层数 int </param>
        /// <returns> 自身组件 </returns>
        public static T Layer<T>(this T selfComponent, int layer) where T : Component
        {
            selfComponent.gameObject.layer = layer;
            return selfComponent;
        }

        /// <summary>
        /// 设置 Component 的 layer 层
        /// 注意：这个方法是直接设置 layer 值，而不是通过 Renderer 进行设置
        /// </summary>
        /// <param name="selfComponent"> 自身组件 </param>
        /// <param name="layerName"> 层 string </param>
        /// <returns> 自身组件 </returns>
        public static T Layer<T>(this T selfComponent, string layerName) where T : Component
        {
            selfComponent.gameObject.layer = LayerMask.NameToLayer(layerName);
            return selfComponent;
        }

        /// <summary>
        /// 获取组件，没有则添加再返回
        /// </summary>
        public static T GetOrAddComponent<T>(this GameObject self) where T : Component
        {
            var comp = self.gameObject.GetComponent<T>();
            return comp ? comp : self.gameObject.AddComponent<T>();
        }

        /// <summary>
        /// 获取组件，没有则添加再返回
        /// </summary>
        public static T GetOrAddComponent<T>(this Component component) where T : Component =>
            component.gameObject.GetOrAddComponent<T>();

        /// <summary>
        /// 获取组件，没有则添加再返回
        /// </summary>
        public static Component GetOrAddComponent(this GameObject self, Type type)
        {
            var component = self.gameObject.GetComponent(type);
            return component ? component : self.gameObject.AddComponent(type);
        }

        /// <summary>
        /// 快速获取position-Vector3
        /// </summary>
        /// <param name="obj"> </param>
        /// <returns> </returns>
        public static Vector3 GetPositionVector3(this GameObject obj) => obj.transform.position;

        /// <summary>
        /// 快速获取position-Vector2
        /// </summary>
        /// <param name="obj"> </param>
        /// <returns> </returns>
        public static Vector2 GetPositionVector2(this GameObject obj) => obj.transform.position;
    }
}