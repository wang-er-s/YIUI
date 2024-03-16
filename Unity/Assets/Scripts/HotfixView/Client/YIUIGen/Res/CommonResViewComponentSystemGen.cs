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
    [FriendOf(typeof(YIUIViewComponent))]
    [EntitySystemOf(typeof(CommonResViewComponent))]
    public static partial class CommonResViewComponentSystem
    {
        [EntitySystem]
        private static void Awake(this CommonResViewComponent self)
        {
        }

        [EntitySystem]
        private static void YIUIBind(this CommonResViewComponent self)
        {
            self.UIBind();
        }
        
        private static void UIBind(this CommonResViewComponent self)
        {
            self.UIBase = self.GetParent<YIUIComponent>();
            self.UIWindow = self.UIBase.GetComponent<YIUIWindowComponent>();
            self.UIView = self.UIBase.GetComponent<YIUIViewComponent>();
            self.UIWindow.WindowOption = EWindowOption.CanUseBaseOpen;
            self.UIView.ViewWindowType = EViewWindowType.View;
            self.UIView.StackOption = EViewStackOption.VisibleTween;


        }
    }
}