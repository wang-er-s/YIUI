using System;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// </summary>
    [FriendOf(typeof(YIUIComponent))]
    [EntitySystemOf(typeof(RedDotDataItemComponent))]
    public static partial class RedDotDataItemComponentSystem
    {
        [EntitySystem]
        private static void Awake(this RedDotDataItemComponent self)
        {
        }

        [EntitySystem]
        private static void YIUIBind(this RedDotDataItemComponent self)
        {
            self.UIBind();
        }
        
        private static void UIBind(this RedDotDataItemComponent self)
        {
            self.u_UIBase = self.GetParent<YIUIComponent>();

        }
    }
}