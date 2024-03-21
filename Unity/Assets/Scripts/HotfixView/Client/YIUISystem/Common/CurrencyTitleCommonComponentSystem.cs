using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// Author  YIUI
    /// Date    2024.3.21
    /// Desc
    /// </summary>
    [FriendOf(typeof(CurrencyTitleCommonComponent))]
    public static partial class CurrencyTitleCommonComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this CurrencyTitleCommonComponent self)
        {
            Log.Info("初始化");
        }

        [EntitySystem]
        private static void Destroy(this CurrencyTitleCommonComponent self)
        {
            Log.Info("销毁");
        }
        
    }
}