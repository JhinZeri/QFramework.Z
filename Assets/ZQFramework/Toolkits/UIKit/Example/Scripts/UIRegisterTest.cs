using UnityEngine;
using ZQFramework.Toolkits.UIKit.Core;

namespace ZQFramework.Toolkits.UIKit.Example.Scripts
{
    public class UIRegisterTest : CanvasView
    {
        protected override void OnInit()
        {
            Debug.Log("OnInit UIRegisterTest !");
        }

        protected override void OnShow()
        {
            Debug.Log("OnShow UIRegisterTest !");
        }

        protected override void OnUpdate()
        {
            Debug.Log("OnUpdate UIRegisterTest !");
        }

        protected override void OnHide()
        {
            Debug.Log("OnHide UIRegisterTest !");
        }

        protected override void OnUIDestroy()
        {
            Debug.Log("OnUIDestroy UIRegisterTest !");
        }

        protected override void BindCanvasViewUIComponents()
        {
            
        }
    }
}