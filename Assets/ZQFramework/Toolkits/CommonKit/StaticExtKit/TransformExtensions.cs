using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace ZQFramework.Toolkits.CommonKit.StaticExtKit
{
    public static class TransformExtensions
    {
        #region 完全重置

        /// <summary>
        /// 完全重置，根据需要选择是否使用世界坐标，如果不使用，则在父物体的基础上重置本地属性 / Resets transform position, rotation and scale.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="useWorldSpace"> Use world space? </param>
        public static Transform Reset(this Transform transform, bool useWorldSpace = false)
        {
            if (useWorldSpace)
            {
                transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            }
            else
            {
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
            }

            transform.localScale = Vector3.one;

            return transform;
        }

        #endregion

        #region 设置 Position

        /// <summary>
        /// Sets x position of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="x"> Value to set. </param>
        public static Transform SetPositionX(this Transform transform, float x)
        {
            transform.position = transform.position.WithX(x);
            return transform;
        }

        /// <summary>
        /// Sets y position of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="y"> Value to set. </param>
        public static Transform SetPositionY(this Transform transform, float y)
        {
            transform.position = transform.position.WithY(y);
            return transform;
        }

        /// <summary>
        /// Sets z position of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="z"> Value to set. </param>
        public static Transform SetPositionZ(this Transform transform, float z)
        {
            transform.position = transform.position.WithZ(z);
            return transform;
        }

        /// <summary>
        /// Sets x and y position of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="x"> Value to set. </param>
        /// <param name="y"> Value to set. </param>
        public static Transform SetPositionXY(this Transform transform, float x, float y)
        {
            transform.position = transform.position.WithXY(x, y);
            return transform;
        }

        /// <summary>
        /// Sets x and y position of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="position"> Position to set. </param>
        public static Transform SetPositionXY(this Transform transform, Vector2 position)
        {
            transform.position = transform.position.WithXY(position);
            return transform;
        }

        /// <summary>
        /// Sets x and z position of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="x"> Value to set. </param>
        /// <param name="z"> Value to set. </param>
        public static Transform SetPositionXZ(this Transform transform, float x, float z)
        {
            transform.position = transform.position.WithXZ(x, z);
            return transform;
        }

        /// <summary>
        /// Sets x and z position of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="position"> Position to set. </param>
        public static Transform SetPositionXZ(this Transform transform, Vector2 position)
        {
            transform.position = transform.position.WithXZ(position);
            return transform;
        }

        /// <summary>
        /// Sets y and z position of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="y"> Value to set. </param>
        /// <param name="z"> Value to set. </param>
        public static Transform SetPositionYZ(this Transform transform, float y, float z)
        {
            transform.position = transform.position.WithYZ(y, z);
            return transform;
        }

        /// <summary>
        /// Sets y and z position of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="position"> Position to set. </param>
        public static Transform SetPositionYZ(this Transform transform, Vector2 position)
        {
            transform.position = transform.position.WithYZ(position);
            return transform;
        }

        /// <summary>
        /// Sets local x position of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="x"> Value to set. </param>
        public static Transform SetLocalPositionX(this Transform transform, float x)
        {
            transform.localPosition = transform.localPosition.WithX(x);
            return transform;
        }

        /// <summary>
        /// Sets local y position of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="y"> Value to set. </param>
        public static Transform SetLocalPositionY(this Transform transform, float y)
        {
            transform.localPosition = transform.localPosition.WithY(y);
            return transform;
        }

        /// <summary>
        /// Sets local z position of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="z"> Value to set. </param>
        public static Transform SetLocalPositionZ(this Transform transform, float z)
        {
            transform.localPosition = transform.localPosition.WithZ(z);
            return transform;
        }

        /// <summary>
        /// Sets local x and y position of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="x"> Value to set. </param>
        /// <param name="y"> Value to set. </param>
        public static Transform SetLocalPositionXY(this Transform transform, float x, float y)
        {
            transform.localPosition = transform.localPosition.WithXY(x, y);
            return transform;
        }

        /// <summary>
        /// Sets local x and y position of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="position"> Position to set. </param>
        public static Transform SetLocalPositionXY(this Transform transform, Vector2 position)
        {
            transform.position = transform.localPosition.WithXY(position);
            return transform;
        }

        /// <summary>
        /// Sets local x and z position of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="x"> Value to set. </param>
        /// <param name="z"> Value to set. </param>
        public static Transform SetLocalPositionXZ(this Transform transform, float x, float z)
        {
            transform.localPosition = transform.localPosition.WithXZ(x, z);
            return transform;
        }

        /// <summary>
        /// Sets local x and z position of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="position"> Position to set. </param>
        public static Transform SetLocalPositionXZ(this Transform transform, Vector2 position)
        {
            transform.position = transform.localPosition.WithXZ(position);
            return transform;
        }

        /// <summary>
        /// Sets local y and z position of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="y"> Value to set. </param>
        /// <param name="z"> Value to set. </param>
        public static Transform SetLocalPositionYZ(this Transform transform, float y, float z)
        {
            transform.localPosition = transform.localPosition.WithYZ(y, z);
            return transform;
        }

        /// <summary>
        /// Sets local y and z position of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="position"> Position to set. </param>
        public static Transform SetLocalPositionYZ(this Transform transform, Vector2 position)
        {
            transform.position = transform.localPosition.WithYZ(position);
            return transform;
        }

        #endregion

        #region 设置 欧拉角 角度

        /// <summary>
        /// Sets euler angles x value of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="x"> Value to set. </param>
        public static Transform SetEulerAnglesX(this Transform transform, float x)
        {
            transform.eulerAngles = transform.eulerAngles.WithX(x);
            return transform;
        }

        /// <summary>
        /// Sets euler angles y value of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="y"> Value to set. </param>
        /// <returns> The updated transform. </returns>
        public static Transform SetEulerAnglesY(this Transform transform, float y)
        {
            transform.eulerAngles = transform.eulerAngles.WithY(y);
            return transform;
        }

        /// <summary>
        /// Sets euler angles z value of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="z"> Value to set. </param>
        /// <returns> The updated transform. </returns>
        public static Transform SetEulerAnglesZ(this Transform transform, float z)
        {
            transform.eulerAngles = transform.eulerAngles.WithZ(z);
            return transform;
        }

        /// <summary>
        /// Sets euler angles x and y values of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="x"> Value to set. </param>
        /// <param name="y"> Value to set. </param>
        /// <returns> The updated transform. </returns>
        public static Transform SetEulerAnglesXY(this Transform transform, float x, float y)
        {
            transform.eulerAngles = transform.eulerAngles.WithXY(x, y);
            return transform;
        }

        /// <summary>
        /// Sets euler angles x and y values of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="eulers"> Angles to set. </param>
        /// <returns> The updated transform. </returns>
        public static Transform SetEulerAnglesXY(this Transform transform, Vector2 eulers)
        {
            transform.eulerAngles = transform.eulerAngles.WithXY(eulers);
            return transform;
        }

        /// <summary>
        /// Sets euler angles x and z values of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="x"> Value to set. </param>
        /// <param name="z"> Value to set. </param>
        /// <returns> The updated transform. </returns>
        public static Transform SetEulerAnglesXZ(this Transform transform, float x, float z)
        {
            transform.eulerAngles = transform.eulerAngles.WithXZ(x, z);
            return transform;
        }

        /// <summary>
        /// Sets euler angles x and z values of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="eulers"> Angles to set. </param>
        /// <returns> The updated transform. </returns>
        public static Transform SetEulerAnglesXZ(this Transform transform, Vector2 eulers)
        {
            transform.eulerAngles = transform.eulerAngles.WithXZ(eulers);
            return transform;
        }


        /// <summary>
        /// Sets euler angles y and z values of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="y"> Value to set. </param>
        /// <param name="z"> Value to set. </param>
        /// <returns> The updated transform. </returns>
        public static Transform SetEulerAnglesYZ(this Transform transform, float y, float z)
        {
            transform.eulerAngles = transform.eulerAngles.WithYZ(y, z);
            return transform;
        }

        /// <summary>
        /// Sets euler angles y and z values of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="eulers"> Angles to set. </param>
        /// <returns> The updated transform. </returns>
        public static Transform SetEulerAnglesYZ(this Transform transform, Vector2 eulers)
        {
            transform.eulerAngles = transform.eulerAngles.WithYZ(eulers);
            return transform;
        }

        /// <summary>
        /// Sets local euler angles x value of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="x"> Value to set. </param>
        /// <returns> The updated transform. </returns>
        public static Transform SetLocalEulerAnglesX(this Transform transform, float x)
        {
            transform.localEulerAngles = transform.localEulerAngles.WithX(x);
            return transform;
        }

        /// <summary>
        /// Sets local euler angles y value of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="y"> Value to set. </param>
        /// <returns> The updated transform. </returns>
        public static Transform SetLocalEulerAnglesY(this Transform transform, float y)
        {
            transform.localEulerAngles = transform.localEulerAngles.WithY(y);
            return transform;
        }

        /// <summary>
        /// Sets local euler angles z value of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="z"> Value to set. </param>
        /// <returns> The updated transform. </returns>
        public static Transform SetLocalEulerAnglesZ(this Transform transform, float z)
        {
            transform.localEulerAngles = transform.localEulerAngles.WithZ(z);
            return transform;
        }

        /// <summary>
        /// Sets local euler angles x and y values of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="x"> Value to set. </param>
        /// <param name="y"> Value to set. </param>
        /// <returns> The updated transform. </returns>
        public static Transform SetLocalEulerAnglesXY(this Transform transform, float x, float y)
        {
            transform.localEulerAngles = transform.localEulerAngles.WithXY(x, y);
            return transform;
        }

        /// <summary>
        /// Sets local euler angles x and y values of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="eulers"> Angles to set. </param>
        /// <returns> The updated transform. </returns>
        public static Transform SetLocalEulerAnglesXY(this Transform transform, Vector2 eulers)
        {
            transform.localEulerAngles = transform.localEulerAngles.WithXY(eulers);
            return transform;
        }

        /// <summary>
        /// Sets local euler angles x and z values of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="x"> Value to set. </param>
        /// <param name="z"> Value to set. </param>
        /// <returns> The updated transform. </returns>
        public static Transform SetLocalEulerAnglesXZ(this Transform transform, float x, float z)
        {
            transform.localEulerAngles = transform.localEulerAngles.WithXZ(x, z);
            return transform;
        }

        /// <summary>
        /// Sets local euler angles x and z values of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="eulers"> Angles to set. </param>
        /// <returns> The updated transform. </returns>
        public static Transform SetLocalEulerAnglesXZ(this Transform transform, Vector2 eulers)
        {
            transform.localEulerAngles = transform.localEulerAngles.WithXZ(eulers);
            return transform;
        }

        /// <summary>
        /// Sets local euler angles y and z values of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="y"> Value to set. </param>
        /// <param name="z"> Value to set. </param>
        /// <returns> The updated transform. </returns>
        public static Transform SetLocalEulerAnglesYZ(this Transform transform, float y, float z)
        {
            transform.localEulerAngles = transform.localEulerAngles.WithYZ(y, z);
            return transform;
        }

        /// <summary>
        /// Sets local euler angles y and z values of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="eulers"> Angles to set. </param>
        /// <returns> The updated transform. </returns>
        public static Transform SetLocalEulerAnglesYZ(this Transform transform, Vector2 eulers)
        {
            transform.localEulerAngles = transform.localEulerAngles.WithYZ(eulers);
            return transform;
        }

        #endregion

        #region 设置 Scale

        /// <summary>
        /// Sets local scale x value of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="x"> Value to set. </param>
        /// <returns> Updated transform. </returns>
        public static Transform SetLocalScaleX(this Transform transform, float x)
        {
            transform.localScale = transform.localScale.WithX(x);
            return transform;
        }

        /// <summary>
        /// Sets local scale y value of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="y"> Value to set. </param>
        /// <returns> Updated transform. </returns>
        public static Transform SetLocalScaleY(this Transform transform, float y)
        {
            transform.localScale = transform.localScale.WithY(y);
            return transform;
        }

        /// <summary>
        /// Sets local scale z value of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="z"> Value to set. </param>
        /// <returns> Updated transform. </returns>
        public static Transform SetLocalScaleZ(this Transform transform, float z)
        {
            transform.localScale = transform.localScale.WithZ(z);
            return transform;
        }

        /// <summary>
        /// Sets local scale x and y values of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="x"> Value to set. </param>
        /// <param name="y"> Value to set. </param>
        /// <returns> Updated transform. </returns>
        public static Transform SetLocalScaleXY(this Transform transform, float x, float y)
        {
            transform.localScale = transform.localScale.WithXY(x, y);
            return transform;
        }

        /// <summary>
        /// Sets local scale x and y values of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="scale"> Scale to set. </param>
        /// <returns> Updated transform. </returns>
        public static Transform SetLocalScaleXY(this Transform transform, Vector2 scale)
        {
            transform.localScale = transform.localScale.WithXY(scale);
            return transform;
        }

        /// <summary>
        /// Sets local scale x and z values of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="x"> Value to set. </param>
        /// <param name="z"> Value to set. </param>
        /// <returns> Updated transform. </returns>
        public static Transform SetLocalScaleXZ(this Transform transform, float x, float z)
        {
            transform.localScale = transform.localScale.WithXZ(x, z);
            return transform;
        }

        /// <summary>
        /// Sets local scale x and z values of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="scale"> Scale to set. </param>
        /// <returns> Updated transform. </returns>
        public static Transform SetLocalScaleXZ(this Transform transform, Vector2 scale)
        {
            transform.localScale = transform.localScale.WithXZ(scale);
            return transform;
        }

        /// <summary>
        /// Sets local scale y and z values of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="y"> Value to set. </param>
        /// <param name="z"> Value to set. </param>
        /// <returns> Updated transform. </returns>
        public static Transform SetLocalScaleYZ(this Transform transform, float y, float z)
        {
            transform.localScale = transform.localScale.WithYZ(y, z);
            return transform;
        }

        /// <summary>
        /// Sets local scale y and z values of transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="scale"> Scale to set. </param>
        /// <returns> Updated transform. </returns>
        public static Transform SetLocalScaleYZ(this Transform transform, Vector2 scale)
        {
            transform.localScale = transform.localScale.WithYZ(scale);
            return transform;
        }

        /// <summary>
        /// Sets uniform local scale.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="scale"> Uniform scale to set. </param>
        /// <returns> Updated transform. </returns>
        public static Transform SetLocalScale(this Transform transform, float scale)
        {
            transform.localScale = new Vector3(scale, scale, scale);
            return transform;
        }

        #endregion

        #region 父物体和子物体的处理

        /// <summary>
        /// 设置当前物体的 Sibiling 序号 - 1 ，也就是 Hierarchy 窗口中向上移动一个位置，不改变父子级关系
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        public static Transform SetToPreviousSibling(this Transform transform)
        {
            transform.SetSiblingIndex(Mathf.Max(transform.GetSiblingIndex() - 1, 0));
            return transform;
        }

        /// <summary>
        /// 设置当前物体的 Sibiling 序号 + 1 ，也就是 Hierarchy 窗口中向下移动一个位置，不改变父子级关系
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        public static Transform SetToNextSibling(this Transform transform)
        {
            int maxSiblingIndex = (transform.parent != null
                ? transform.parent.childCount
                : transform.gameObject.scene.rootCount) - 1;
            transform.SetSiblingIndex(Mathf.Min(transform.GetSiblingIndex() + 1, maxSiblingIndex));
            return transform;
        }

        /// <summary>
        /// 返回上一个 Sibiling 序号的物体，也就是 Hierarchy 窗口中的上一个物体，不跨越父子级，如果不存在，返回空
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <returns> Previous object's transform. </returns>
        public static Transform GetPreviousSiblingTransform(this Transform transform)
        {
            if (transform.GetSiblingIndex() == 0)
                return null;

            if (transform.parent)
                return transform.parent.GetChild(transform.GetSiblingIndex() - 1);
            return transform.gameObject.scene.GetRootGameObjects()[transform.GetSiblingIndex() - 1].transform;
        }

        /// <summary>
        /// 返回下一个 Sibiling 序号的物体，也就是 Hierarchy 窗口中的下一个物体，不跨越父子级，如果不存在，返回空
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <returns> Next object's transform. </returns>
        public static Transform GetNextSiblingTransform(this Transform transform)
        {
            if (transform.parent)
            {
                if (transform.GetSiblingIndex() == transform.parent.childCount - 1)
                    return null;

                return transform.parent.GetChild(transform.GetSiblingIndex() + 1);
            }

            if (transform.GetSiblingIndex() == transform.gameObject.scene.rootCount - 1)
                return null;

            return transform.gameObject.scene.GetRootGameObjects()[transform.GetSiblingIndex() + 1].transform;
        }

        /// <summary>
        /// 返回当前父物体的所有子物体列表，可以选择是否包含自身物体，如果没有父物体，返回场景中的所有根节点物体列表
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="includeThis"> Include this <paramref name="transform" /> object. </param>
        /// <returns> Sibling objects transforms. </returns>
        public static List<Transform> GetAllSiblingObjects(this Transform transform, bool includeThis = true)
        {
            List<Transform> siblings;

            if (transform.parent)
            {
                siblings = new List<Transform>(transform.parent.childCount);

                for (var i = 0; i < transform.parent.childCount; i++)
                {
                    var child = transform.parent.GetChild(i);

                    if (includeThis || (!includeThis && child != transform))
                        siblings.Add(child);
                }
            }
            else
            {
                GameObject[] childs = transform.gameObject.scene.GetRootGameObjects();
                siblings = new List<Transform>(childs.Length);

                for (var i = 0; i < childs.Length; i++)
                {
                    var child = childs[i].transform;

                    if (includeThis || (!includeThis && child != transform))
                        siblings.Add(child);
                }
            }

            return siblings;
        }

        /// <summary>
        /// 获得所有的子物体，返回列表 / Gets list of all childs.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <returns> List of all childs. </returns>
        public static List<Transform> GetChilds(this Transform transform)
        {
            return transform.Cast<Transform>().ToList();
        }

        /// <summary>
        /// 获得一个随机的子物体 / Gets a random child.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <returns> Random child. </returns>
        public static Transform GetRandomChild(this Transform transform)
        {
            return transform.GetChild(UnityEngine.Random.Range(0, transform.childCount));
        }

        /// <summary>
        /// 添加新的物体成为目标物体的子物体，可变参数 / Adds childs to transform.
        /// </summary>
        /// <param name="childs"> Childs to add. </param>
        /// <param name="transform"> Target transform. </param>
        public static Transform AddChilds(this Transform transform, params Transform[] childs)
        {
            AddChilds(transform, (IEnumerable<Transform>)childs);
            return transform;
        }

        /// <summary>
        /// 添加新的物体成为目标物体的子物体，参数为列表 / Adds childs to transform.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="childs"> Childs to add. </param>
        public static Transform AddChilds(this Transform transform, IEnumerable<Transform> childs)
        {
            foreach (var child in childs)
                child.parent = transform;
            return transform;
        }

        /// <summary>
        /// 摧毁所有的子物体 / Destroy all childs.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        public static Transform DestroyChilds(this Transform transform)
        {
            GetChilds(transform).ForEach(child => UnityObject.Destroy(child.gameObject));
            return transform;
        }

        /// <summary>
        /// 有条件的摧毁子物体，第二个参数为 Condition 条件 / Destroy all childs by coindition.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="predicate"> Condition. </param>
        public static Transform DestroyChildsWhere(this Transform transform, Predicate<Transform> predicate)
        {
            IEnumerable<Transform> filtered = GetChilds(transform).Where(c => predicate(c));

            foreach (var child in filtered)
                UnityObject.Destroy(child.gameObject);

            return transform;
        }

        /// <summary>
        /// 通过序号 index 摧毁子物体，返回自身 / Destroy child by index.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        /// <param name="index"> </param>
        public static Transform DestroyChild(this Transform transform, int index)
        {
            UnityObject.Destroy(transform.GetChild(index).gameObject);
            return transform;
        }

        /// <summary>
        /// 摧毁第一个子物体 / Destroy first child.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        public static Transform DestroyFirstChild(this Transform transform)
        {
            DestroyChild(transform, 0);
            return transform;
        }

        /// <summary>
        /// 摧毁最后一个子物体 / Destroy last child.
        /// </summary>
        /// <param name="transform"> Target transform. </param>
        public static Transform DestroyLastChild(this Transform transform)
        {
            DestroyChild(transform, transform.childCount - 1);
            return transform;
        }

        #endregion
    }
}