/****************************************************************************
 * Copyright (c) 2016 - 2022 liangxiegame UNDER MIT License
 * 
 * https://qframework.cn
 * https://github.com/liangxiegame/QFramework
 * https://gitee.com/liangxiegame/QFramework
 ****************************************************************************/

#if UNITY_EDITOR
using QFramework.Z.Framework.Observable;
using UnityEditor;

namespace QFramework.Z.CoreKit.BindablePropertyKit
{
    public class EditorPrefsBoolProperty : BindableProperty<bool>
    {

        public EditorPrefsBoolProperty(string key, bool initValue = false)
        {
            // 初始化
            ObservableValue = EditorPrefs.GetBool(key, initValue);

            Register(onValueChanged: value => { EditorPrefs.SetBool(key, value); });
        }
    }
}
#endif