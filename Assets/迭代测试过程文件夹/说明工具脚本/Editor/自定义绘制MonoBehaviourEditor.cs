using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace 迭代测试过程文件夹.说明工具脚本.Editor
{
    /// <summary>
    /// 当开启 OdinEditor 默认绘制后，子类继承父类绘制将失效，被 OdinEditor 强制覆盖
    /// </summary>
    [CustomEditor(typeof(MonoBehaviour), true)]
    [CanEditMultipleObjects]
    public class 自定义绘制MonoBehaviourEditor : OdinEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var mono = target as MonoBehaviour;
            if (mono == null) return;

            var script = MonoScript.FromMonoBehaviour(mono);
            var type = script.GetClass();
            var customAttributes = type.GetCustomAttributes(typeof(AddDescriptionButtonAttribute), true);

            if (customAttributes.Length > 0)
            {
                if (SirenixEditorGUI.SDFIconButton(new GUIContent("添加描述组件"), 20f, SdfIconType.App))
                {
                    if (mono.GetComponent<InspectorDescription>() == null)
                    {
                        mono.gameObject.AddComponent<InspectorDescription>();
                    }
                }
            }
        }
    }
}