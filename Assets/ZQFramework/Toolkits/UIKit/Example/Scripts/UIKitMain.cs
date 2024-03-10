using UnityEngine;
using ZGameProject.UI;

namespace ZQFramework.Toolkits.UIKit.Example.Scripts
{
    public class UIKitMain : MonoBehaviour
    {
        void Start()
        {
            Core.UIKit.OpenCanvas<UILoginTest>();
            // Core.UIKit.OpenCanvas<UIRegisterTest>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q)) Core.UIKit.OpenCanvas<UILoginTest>();

            if (Input.GetKeyDown(KeyCode.W)) Core.UIKit.HideCanvas<UILoginTest>();

            if (Input.GetKeyDown(KeyCode.E)) Core.UIKit.DestroyCanvas<UILoginTest>();
        }
    }
}