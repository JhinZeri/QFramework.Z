using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace ZQFramework.Toolkits.UIKit.UIHelper
{
    public class CanvasRebuildTest : MonoBehaviour
    {
        public IList<ICanvasElement> GraphicRebuildQueue;
        public IList<ICanvasElement> LayoutRebuildQueue;

        void Start()
        {
            var type = typeof(CanvasUpdateRegistry);
            var field = type.GetField("m_LayoutRebuildQueue", BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
                LayoutRebuildQueue = (IList<ICanvasElement>)field.GetValue(CanvasUpdateRegistry.instance);

            field = type.GetField("m_GraphicRebuildQueue", BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
                GraphicRebuildQueue = (IList<ICanvasElement>)field.GetValue(CanvasUpdateRegistry.instance);
        }

        void Update()
        {
            for (var i = 0; i < LayoutRebuildQueue.Count; i++)
            {
                var rebuild = LayoutRebuildQueue[i];
                if (ObjectValidForUpdate(rebuild))
                    Debug.LogFormat("{0} 引起网格 {1} 重建", rebuild.transform.name,
                        rebuild.transform.GetComponent<Graphic>().canvas.name);
            }

            for (var i = 0; i < GraphicRebuildQueue.Count; i++)
            {
                var rebuild = GraphicRebuildQueue[i];
                if (ObjectValidForUpdate(rebuild))
                    Debug.LogFormat("{0} 引起网格 {1} 重建", rebuild.transform.name,
                        rebuild.transform.GetComponent<Graphic>().canvas.name);
            }
        }

        bool ObjectValidForUpdate(ICanvasElement element)
        {
            bool valid = element != null;
            bool isUnityObject = element is Object;
            if (isUnityObject) valid = (Object)element != null;

            return valid;
        }
    }
}