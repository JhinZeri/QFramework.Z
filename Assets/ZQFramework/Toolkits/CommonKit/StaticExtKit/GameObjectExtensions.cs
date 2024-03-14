using System;
using UnityEngine;

namespace ZQFramework.Toolkits.CommonKit.StaticExtKit
{
    /// <summary>
    /// UnityEngine.GameObject 的扩展方法
    /// </summary>
    public static class GameObjectExtensions
    {
        #region 显示和隐藏

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

        #endregion

        #region 设置 Layer 层

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

        #endregion

        #region 组件相关

        /// <summary>
        /// 获取组件，没有则添加再返回
        /// </summary>
        public static T GetOrAddComponent<T>(this GameObject self) where T : Component
        {
            return self.TryGetComponent(out T component) ? component : self.AddComponent<T>();
        }

        /// <summary>
        /// 获取组件，没有则添加再返回
        /// </summary>
        public static Component GetOrAddComponent(this GameObject self, Type type)
        {
            var component = self.GetComponent(type);
            return component ? component : self.AddComponent(type);
        }

        /// <summary>
        /// Trying to get component in this or any it's children.
        /// </summary>
        /// <typeparam name="T"> Component type. </typeparam>
        /// <param name="gameObject"> Target gameobject. </param>
        /// <param name="component"> Target component. </param>
        /// <param name="includeInactive"> Should we find component on inactive game objects? </param>
        /// <returns> <see langword="true" /> if component was found. </returns>
        public static bool TryGetComponentInChildren<T>(this GameObject gameObject, out T component,
            bool includeInactive = false) where T : Component
        {
            return component = gameObject.GetComponentInChildren<T>(includeInactive);
        }

        /// <summary>
        /// Trying to get component in this or any it's parent.
        /// </summary>
        /// <typeparam name="T"> Component type. </typeparam>
        /// <param name="gameObject"> Target gameobject. </param>
        /// <param name="component"> Target component. </param>
        /// <param name="includeInactive"> Should we find component on inactive game objects? </param>
        /// <returns> <see langword="true" /> if component was found. </returns>
        public static bool TryGetComponentInParent<T>(this GameObject gameObject, out T component,
            bool includeInactive = false) where T : Component
        {
            return component = gameObject.GetComponentInParent<T>(includeInactive);
        }

        #endregion

        #region 获取物体的 Position

        /// <summary>
        /// 快速获取position-Vector3
        /// </summary>
        /// <param name="obj"> </param>
        /// <returns> </returns>
        public static Vector3 GetPositionVector3(this GameObject obj)
        {
            return obj.transform.position;
        }

        /// <summary>
        /// 快速获取position-Vector2
        /// </summary>
        /// <param name="obj"> </param>
        /// <returns> </returns>
        public static Vector2 GetPositionVector2(this GameObject obj)
        {
            return obj.transform.position;
        }

        #endregion
    }
}