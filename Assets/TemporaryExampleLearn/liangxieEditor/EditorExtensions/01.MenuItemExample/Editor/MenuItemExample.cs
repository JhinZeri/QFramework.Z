using System.IO;
using UnityEditor;
using UnityEngine;

namespace EditorExtensions
{
    public static class MenuItemExample
    {
        static bool mOpenShotCut;

        // 编译会调用一次构造
        static MenuItemExample()
        {
            Menu.SetChecked("EditorExtensions/01.Menu/05.快捷键开关", mOpenShotCut);
        }

        [MenuItem("EditorExtensions/01.Menu/01.Hello Editor")]
        static void HelloEditor()
        {
            Debug.Log("Hello from the editor!");
        }

        [MenuItem("EditorExtensions/01.Menu/02.Open Bilibili")]
        static void OpenBilibili()
        {
            // 打开网页 可以打开APP 微信等 打开一个地址 使用默认浏览器
            Application.OpenURL("https://bilibili.com");
        }

        // 项目数据文件夹路径
        [MenuItem("EditorExtensions/01.Menu/03.Open PersistentDataPath")]
        static void OpenPersistentDataPath()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }

        [MenuItem("EditorExtensions/01.Menu/04.打开 Library 目录")]
        static void OpenDesignerFolder()
        {
            if (Directory.Exists(Application.dataPath.Replace("Assets", "Library")))
            {
                Debug.Log("存在这个文件夹");
                // Application.dataPath 指的是 Assets 文件夹
                // EditorUtility.RevealInFinder 会打开到这个目录 并且选中这个路径的文件夹
                // 能看到这个文件夹，并且要选中它
                EditorUtility.RevealInFinder(Application.dataPath.Replace("Assets", "Library"));

                // OpenURL 会打开进入到 Asset 文件夹
                // D:\Projects Folder\Unity Projects\ZFramework\Assets
                // Application.OpenURL(Application.dataPath);
            }
            else
            {
                Debug.Log("不存在这个文件夹");
            }

            // if (Directory.Exists(Application.dataPath + "/Plugins"))
            // {
            //     // 打开 Plugins
            //     EditorUtility.RevealInFinder(Application.dataPath + "/Plugins");
            // }
        }

        [MenuItem("EditorExtensions/01.Menu/05.快捷键开关")]
        static void ToggleShotCut()
        {
            mOpenShotCut = !mOpenShotCut;
            Menu.SetChecked("EditorExtensions/01.Menu/05.快捷键开关", mOpenShotCut);
        }

        [MenuItem("EditorExtensions/01.Menu/06.HelloEditorWithShotCut _c")]
        static void HelloEditorWithShotCut()
        {
            Debug.Log("HelloEditor");
        }

        [MenuItem("EditorExtensions/01.Menu/06.HelloEditorWithShotCut _c", true)]
        static bool HelloEditorWithShotCutValidate() => mOpenShotCut;


        [MenuItem("EditorExtensions/01.Menu/07.OpenBli #e")]
        static void OpenBliWithShotCut()
        {
            // EditorApplication.ExecuteMenuItem("EditorExtensions/01.Menu/02.Open Bilibili");

            // Application.OpenURL("https://www.bilibili.com/");
            Debug.Log("HelloBli");
        }

        [MenuItem("EditorExtensions/01.Menu/07.OpenBli #e", true)]
        static bool OpenBliWithShotCutValidate() => mOpenShotCut;

        [MenuItem("EditorExtensions/01.Menu/08.OpenPersistentDataPath #%e")]
        static void OpenPersistentDataPathWithShotCut()
        {
            // EditorApplication.ExecuteMenuItem("EditorExtensions/01.Menu/03.Open PersistentDataPath");

            // Application.OpenURL(Application.persistentDataPath);
            // EditorUtility.RevealInFinder(Application.persistentDataPath);
            Debug.Log("HelloPersistentDataPath");
        }

        [MenuItem("EditorExtensions/01.Menu/08.OpenPersistentDataPath #%e", true)]
        static bool OpenPersistentDataPathWithShotCutValidate() => mOpenShotCut;

        [MenuItem("EditorExtensions/01.Menu/09.OpenLibrary &r")]
        static void OpenLibraryWithShotCut()
        {
            // EditorApplication.ExecuteMenuItem("EditorExtensions/01.Menu/04.打开 Library 目录");

            // Application.OpenURL(Application.persistentDataPath);
            // EditorUtility.RevealInFinder(Application.persistentDataPath);
            Debug.Log("HelloLibrary");
        }

        [MenuItem("EditorExtensions/01.Menu/09.OpenLibrary &r", true)]
        static bool OpenLibraryWithShotCutValidate() => mOpenShotCut;
    }
}