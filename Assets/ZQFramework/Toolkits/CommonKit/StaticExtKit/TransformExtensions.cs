using UnityEngine;

namespace ZQFramework.Toolkits.CommonKit.StaticExtKit
{
    public static class TransformExtension
    {
        /// <summary>
        /// 设置Position的X
        /// </summary>
        /// <param name="transform"> </param>
        /// <param name="x"> </param>
        /// <returns> </returns>
        public static Transform SetPosX(this Transform transform, float x)
        {
            var position = transform.position;
            position.x = x;
            transform.position = position;
            return transform;
        }

        /// <summary>
        /// 设置Position的Y
        /// </summary>
        /// <param name="transform"> </param>
        /// <param name="y"> </param>
        /// <returns> </returns>
        public static Transform SetPosY(this Transform transform, float y)
        {
            var position = transform.position;
            position.y = y;
            transform.position = position;
            return transform;
        }

        /// <summary>
        /// 设置Position的Z
        /// </summary>
        /// <param name="transform"> </param>
        /// <param name="z"> </param>
        /// <returns> </returns>
        public static Transform SetPosZ(this Transform transform, float z)
        {
            var position = transform.position;
            position.z = z;
            transform.position = position;
            return transform;
        }

        /// <summary>
        /// 设置本地坐标X
        /// </summary>
        /// <param name="transform"> </param>
        /// <param name="x"> </param>
        /// <returns> </returns>
        public static Transform SetLocalPosX(this Transform transform, float x)
        {
            var localPosition = transform.localPosition;
            localPosition.x = x;
            transform.localPosition = localPosition;
            return transform;
        }

        /// <summary>
        /// 设置本地坐标Y
        /// </summary>
        /// <param name="transform"> </param>
        /// <param name="y"> </param>
        /// <returns> </returns>
        public static Transform SetLocalPosY(this Transform transform, float y)
        {
            var localPosition = transform.localPosition;
            localPosition.y = y;
            transform.localPosition = localPosition;
            return transform;
        }

        /// <summary>
        /// 设置本地坐标Z
        /// </summary>
        /// <param name="transform"> </param>
        /// <param name="z"> </param>
        /// <returns> </returns>
        public static Transform SetLocalPosZ(this Transform transform, float z)
        {
            var localPosition = transform.localPosition;
            localPosition.z = z;
            transform.localPosition = localPosition;
            return transform;
        }

        /// <summary>
        /// 重置操作，也可以重置其他Transform
        /// </summary>
        /// <param name="self"> 重置自身 </param>
        /// <returns> 返回使用这个方法的自身 </returns>
        public static Transform Identity(this Transform self)
        {
            self.localPosition = Vector3.zero;
            self.localScale = Vector3.one;
            self.localRotation = Quaternion.identity;
            return self;
        }
    }
}