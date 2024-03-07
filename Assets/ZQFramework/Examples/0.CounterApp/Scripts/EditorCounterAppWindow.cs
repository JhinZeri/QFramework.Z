#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using ZQFramework.Framework.Core;
using ZQFramework.Framework.Rule;

namespace ZQFramework.Examples._0.CounterApp.Scripts
{
    public class EditorCounterAppWindow : EditorWindow, IController
    {
        CounterAppModel mCounterAppModel;

        void OnEnable()
        {
            mCounterAppModel = this.GetModel<CounterAppModel>();
        }

        void OnDisable()
        {
            mCounterAppModel = null;
        }

        void OnGUI()
        {
            if (GUILayout.Button("+")) this.SendCommand<IncreaseCountCommand>();

            GUILayout.Label(mCounterAppModel.Count.Value.ToString());

            if (GUILayout.Button("-")) this.SendCommand<DecreaseCountCommand>();
        }

        public IArchitecture GetArchitecture() => Architecture.CounterApp.Interface;

        [MenuItem("QFramework/Example/EditorCounterAppWindow")]
        static void Open()
        {
            GetWindow<EditorCounterAppWindow>().Show();
        }
    }
}
#endif