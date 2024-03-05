#if UNITY_EDITOR
using QFramework.Z.Framework.Core;
using QFramework.Z.Framework.Rule;
using UnityEditor;
using UnityEngine;

namespace QFramework.Z.Examples._0.CounterApp
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

        public IArchitecture GetArchitecture() => CounterApp.Interface;

        [MenuItem("QFramework/Example/EditorCounterAppWindow")]
        static void Open()
        {
            GetWindow<EditorCounterAppWindow>().Show();
        }
    }
}
#endif