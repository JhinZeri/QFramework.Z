#if UNITY_EDITOR
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace ZQFramework.Toolkits.CodeGenKit.UICodeGen.Editor
{
    public class UICodeGenPreviewEditorWindow : OdinEditorWindow
    {
        [Title("脚本路径")]
        [Sirenix.OdinInspector.FilePath]
        [LabelText("UI Designer 检视脚本路径")]
        public string DesignerScriptPath;

        [Sirenix.OdinInspector.FilePath]
        [LabelText("UI Logic 逻辑脚本路径")]
        public string UILogicCodePath;

        string m_DesignerMetaPath;
        string m_UIDesignerCode;
        string m_UILogicCode;
        string m_UILogicMetaPath;

        /// <summary>
        /// UIDesigner代码生成预览窗口
        /// </summary>
        /// <param name="designerCode"> </param>
        /// <param name="designerCodePath"> </param>
        /// <param name="designerMetaPath"> </param>
        /// <param name="uiLogicCode"> </param>
        /// <param name="uiLogicCodePath"> </param>
        /// <param name="uiLogicMetaPath"> </param>
        public static void OpenPreviewWindow(string designerCode, string designerCodePath, string designerMetaPath,
            string uiLogicCode, string uiLogicCodePath, string uiLogicMetaPath)
        {
            var window = GetWindow<UICodeGenPreviewEditorWindow>("UIDesigner 检视脚本预览");
            window.m_UIDesignerCode = designerCode;
            window.DesignerScriptPath = designerCodePath;
            window.m_DesignerMetaPath = designerMetaPath;
            window.m_UILogicCode = uiLogicCode;
            window.UILogicCodePath = uiLogicCodePath;
            window.m_UILogicMetaPath = uiLogicMetaPath;
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(900, 800);
            window.minSize = new Vector2(300, 200);
            window.Show();
        }

        protected override void OnBeginDrawEditors()
        {
            base.OnBeginDrawEditors();
            SirenixEditorGUI.InfoMessageBox("请注意: 此预览窗口仅用于预览 Designer 视图脚本，重点查看变量命名是否符合规范，Logic 逻辑脚本不会出错，" +
                                            "可以在代码生成配置界面关闭预览窗口");
        }

        [OnInspectorGUI]
        [Title("Designer 检视脚本预览")]
        void DrawPreviewCodeText()
        {
            // 绘制代码预览
            EditorGUILayout.TextArea(m_UIDesignerCode);
            EditorGUILayout.Space(10f);
        }

        protected override void DrawEditors()
        {
            base.DrawEditors();
            //绘制按钮
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("一键生成 UI 检视脚本和逻辑脚本", GUILayout.Height(30f)))
                {
                    // 准备写入
                    if (File.Exists(DesignerScriptPath))
                    {
                        File.Delete(DesignerScriptPath);
                        File.Delete(m_DesignerMetaPath);
                    }

                    if (File.Exists(UILogicCodePath))
                    {
                        File.Delete(UILogicCodePath);
                        File.Delete(m_UILogicMetaPath);
                    }

                    AssetDatabase.Refresh();
                    // 生成 view 脚本
                    File.WriteAllText(DesignerScriptPath, m_UIDesignerCode);
                    // 生成 Logic 脚本
                    File.WriteAllText(UILogicCodePath, m_UILogicCode);

                    AssetDatabase.ImportAsset(DesignerScriptPath, ImportAssetOptions.Default);
                    AssetDatabase.ImportAsset(UILogicCodePath, ImportAssetOptions.Default);
                    AssetDatabase.Refresh();

                    if (EditorUtility.DisplayDialog("自动化生成所有 UI 脚本", "UI 逻辑脚本和检视脚本生成成功", "确定")) Close();
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
#endif