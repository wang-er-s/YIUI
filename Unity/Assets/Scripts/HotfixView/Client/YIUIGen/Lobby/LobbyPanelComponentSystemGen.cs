﻿using System;
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
    [EntitySystemOf(typeof(LobbyPanelComponent))]
    public static partial class LobbyPanelComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LobbyPanelComponent self)
        {
        }

        [EntitySystem]
        private static void YIUIBind(this LobbyPanelComponent self)
        {
            self.UIBind();
        }
        
        private static void UIBind(this LobbyPanelComponent self)
        {
            self.UIBase = self.GetParent<YIUIComponent>();
            self.UIWindow = self.UIBase.GetComponent<YIUIWindowComponent>();
            self.UIPanel = self.UIBase.GetComponent<YIUIPanelComponent>();
            self.UIWindow.WindowOption = EWindowOption.None;
            self.UIPanel.Layer = EPanelLayer.Panel;
            self.UIPanel.PanelOption = EPanelOption.None;
            self.UIPanel.StackOption = EPanelStackOption.VisibleTween;
            self.UIPanel.Priority = 0;

            self.BtnEnterMap = self.UIBase.ComponentTable.FindComponent<UnityEngine.UI.Button>("BtnEnterMap");
            self.UICurrencyTitleView = self.UIBase.CDETable.FindUIOwner<ET.Client.CurrencyTitleCommonComponent>("CurrencyTitleView");

        }
    }
}