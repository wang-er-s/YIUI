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
    [FriendOf(typeof(YIUIWindowComponent))]
    [FriendOf(typeof(YIUIPanelComponent))]
    [EntitySystemOf(typeof(LoginPanelComponent))]
    public static partial class LoginPanelComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LoginPanelComponent self)
        {
        }

        [EntitySystem]
        private static void YIUIBind(this LoginPanelComponent self)
        {
            self.UIBind();
        }
        
        private static void UIBind(this LoginPanelComponent self)
        {
            self.u_UIBase = self.GetParent<YIUIComponent>();
            self.u_UIWindow = self.UIBase.GetComponent<YIUIWindowComponent>();
            self.u_UIPanel = self.UIBase.GetComponent<YIUIPanelComponent>();
      

            self.u_ComBtnLogin = self.UIBase.ComponentTable.FindComponent<UnityEngine.UI.Button>("u_ComBtnLogin");

        }
    }
}