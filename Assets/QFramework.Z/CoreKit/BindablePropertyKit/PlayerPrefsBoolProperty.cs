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
    public class PlayerPrefsBooleanProperty : BindableProperty<bool>
    {
        public PlayerPrefsBooleanProperty(string saveKey, bool defaultValue = false)
        {
            ObservableValue = PlayerPrefs.GetInt(saveKey, defaultValue ? 1 : 0) == 1;

            this.Register(onValueChanged: value => PlayerPrefs.SetInt(saveKey, value ? 1 : 0));
        }
    }
}