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
            self.UIBase = self.GetParent<YIUIComponent>();
            self.UIWindow = self.UIBase.GetComponent<YIUIWindowComponent>();
            self.UIPanel = self.UIBase.GetComponent<YIUIPanelComponent>();
            self.UIWindow.WindowOption = EWindowOption.None;
            self.UIPanel.Layer = EPanelLayer.Panel;
            self.UIPanel.PanelOption = EPanelOption.None;
            self.UIPanel.StackOption = EPanelStackOption.VisibleTween;
            self.UIPanel.Priority = 0;

            self.BtnLogin = self.UIBase.ComponentTable.FindComponent<UnityEngine.UI.Button>("BtnLogin");
            self.InputAccount = self.UIBase.ComponentTable.FindComponent<TMPro.TMP_InputField>("InputAccount");
            self.ImpntPassword = self.UIBase.ComponentTable.FindComponent<TMPro.TMP_InputField>("ImpntPassword");
            self.UICommonResView = self.UIBase.CDETable.FindUIOwner<ET.Client.CommonResViewComponent>("CommonResView");
            self.UILoginExistView = self.UIBase.CDETable.FindUIOwner<ET.Client.LoginExistViewComponent>("LoginExistView");

        }
    }
}