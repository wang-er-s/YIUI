using System;
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
    [EntitySystemOf(typeof(RedDotPanelComponent))]
    public static partial class RedDotPanelComponentSystem
    {
        [EntitySystem]
        private static void Awake(this RedDotPanelComponent self)
        {
        }

        [EntitySystem]
        private static void YIUIBind(this RedDotPanelComponent self)
        {
            self.UIBind();
        }
        
        private static void UIBind(this RedDotPanelComponent self)
        {
            self.u_UIBase = self.GetParent<YIUIComponent>();
            self.u_UIWindow = self.UIBase.GetComponent<YIUIWindowComponent>();
            self.u_UIPanel = self.UIBase.GetComponent<YIUIPanelComponent>();
            self.UIWindow.WindowOption = EWindowOption.BanTween|EWindowOption.BanAwaitOpenTween|EWindowOption.BanAwaitCloseTween|EWindowOption.SkipOtherOpenTween|EWindowOption.SkipOtherCloseTween;
            self.UIPanel.Layer = EPanelLayer.Tips;
            self.UIPanel.PanelOption = EPanelOption.TimeCache;
            self.UIPanel.StackOption = EPanelStackOption.VisibleTween;
            self.UIPanel.Priority = 1000;
            self.UIPanel.CachePanelTime = 10;

            self.u_ComSearchScroll = self.UIBase.ComponentTable.FindComponent<UnityEngine.UI.LoopVerticalScrollRect>("u_ComSearchScroll");
            self.u_ComDropdownSearch = self.UIBase.ComponentTable.FindComponent<TMPro.TMP_Dropdown>("u_ComDropdownSearch");
            self.u_ComStackScroll = self.UIBase.ComponentTable.FindComponent<UnityEngine.UI.LoopVerticalScrollRect>("u_ComStackScroll");
            self.u_ComInputChangeCount = self.UIBase.ComponentTable.FindComponent<TMPro.TMP_InputField>("u_ComInputChangeCount");
        }
    }
}