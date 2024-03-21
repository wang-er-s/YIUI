using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// </summary>
    [FriendOf(typeof(YIUIComponent))]
    [EntitySystemOf(typeof(CurrencyTitleCommonComponent))]
    public static partial class CurrencyTitleCommonComponentSystem
    {
        [EntitySystem]
        private static void Awake(this CurrencyTitleCommonComponent self)
        {
        }

        [EntitySystem]
        private static void YIUIBind(this CurrencyTitleCommonComponent self)
        {
            self.UIBind();
        }
        
        private static void UIBind(this CurrencyTitleCommonComponent self)
        {
            self.UIBase = self.GetParent<YIUIComponent>();

            self.BtnClose = self.UIBase.ComponentTable.FindComponent<UnityEngine.UI.Button>("BtnClose");

        }
    }
}