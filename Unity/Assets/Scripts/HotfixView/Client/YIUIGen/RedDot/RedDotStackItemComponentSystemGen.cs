using System;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// </summary>
    [FriendOf(typeof(YIUIComponent))]
    [EntitySystemOf(typeof(RedDotStackItemComponent))]
    public static partial class RedDotStackItemComponentSystem
    {
        [EntitySystem]
        private static void Awake(this RedDotStackItemComponent self)
        {
        }

        [EntitySystem]
        private static void YIUIBind(this RedDotStackItemComponent self)
        {
            self.UIBind();
        }
        
        private static void UIBind(this RedDotStackItemComponent self)
        {
            self.u_UIBase = self.GetParent<YIUIComponent>();

            self.u_ComStackText = self.UIBase.ComponentTable.FindComponent<TMPro.TextMeshProUGUI>("u_ComStackText");

        }
    }
}