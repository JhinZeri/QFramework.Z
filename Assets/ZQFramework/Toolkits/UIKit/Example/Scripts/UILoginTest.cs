using UnityEngine;
using ZQFramework.Toolkits.UIKit.Core;

namespace ZQFramework.Toolkits.UIKit.Example.Scripts
{
    public class UILoginTest : CanvasView
    {
        protected override void OnInit()
        {
            Debug.Log("UILoginTest OnInit !");
        }

        protected override void OnShow()
        {
            Debug.Log("UILoginTest OnShow !");
        }

        protected override void OnUpdate()
        {
            Debug.Log("UILoginTest OnUpdate !");
        }

        protected override void OnHide()
        {
            Debug.Log("UILoginTest OnHide !");
        }

        protected override void OnUIDestroy()
        {
            Debug.Log("UILoginTest OnUIDestroy !");
        }

        protected override void BindCanvasViewUIComponents()
        {
            
        }
    }
}