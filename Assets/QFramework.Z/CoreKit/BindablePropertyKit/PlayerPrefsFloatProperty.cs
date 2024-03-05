/****************************************************************************
 * Copyright (c) 2016 - 2022 liangxiegame UNDER MIT License
 * 
 * https://qframework.cn
 * https://github.com/liangxiegame/QFramework
 * https://gitee.com/liangxiegame/QFramework
 ****************************************************************************/

using QFramework.Z.Framework.Observable;
using UnityEngine;

namespace QFramework.Z.CoreKit.BindablePropertyKit
{
    public class PlayerPrefsFloatProperty : BindableProperty<float>
    {
        public PlayerPrefsFloatProperty(string saveKey, float defaultValue = 0.0f)
        {
            ObservableValue =  PlayerPrefs.GetFloat(saveKey, defaultValue);

            this.Register(onValueChanged: value => PlayerPrefs.SetFloat(saveKey, value));
        }
    }
}