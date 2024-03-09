using UnityEngine;
using ZGameProject.UI;

namespace ZQFramework.Toolkits.UIKit.Example.Scripts
{
    public class UIKitMain : MonoBehaviour
    {
        void Start()
        {
            Core.UIKit.OpenCanvas<UILoginTest>();
            Core.UIKit.OpenCanvas<UIRegisterTest>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q)) Core.UIKit.OpenCanvas<UILoginTest>();

            if (Input.GetKeyDown(KeyCode.W)) Core.UIKit.HideCanvas<UILoginTest>();

            if (Input.GetKeyDown(KeyCode.E)) Core.UIKit.DestroyCanvas<UILoginTest>();

            if (Input.GetKeyDown(KeyCode.R)) Core.UIKit.OpenCanvas<UIRegisterTest>();

            if (Input.GetKeyDown(KeyCode.T)) Core.UIKit.HideCanvas<UIRegisterTest>();

            if (Input.GetKeyDown(KeyCode.Y)) Core.UIKit.DestroyCanvas<UIRegisterTest>();

            if (Input.GetKeyDown(KeyCode.U)) Core.UIKit.DestroyAllCanvas();
        }
    }
}