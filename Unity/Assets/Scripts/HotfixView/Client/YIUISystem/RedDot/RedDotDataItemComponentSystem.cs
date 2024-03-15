using System;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    [FriendOf(typeof (RedDotDataItemComponent))]
    public static partial class RedDotDataItemComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this RedDotDataItemComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this RedDotDataItemComponent self)
        {
        }

        public static void RefreshData(this RedDotDataItemComponent self, RedDotData data)
        {
            self.m_Data        = data;
        }

        #region YIUIEvent开始

        private static void OnEventTipsAction(this RedDotDataItemComponent self, bool p1)
        {
            RedDotMgr.Inst.SetTips(self.m_Data.Key, p1);
        }

        private static void OnEventParentAction(this RedDotDataItemComponent self)
        {
            self.Fiber().UIEvent(new OnClickParentListEvent() { Data = self.m_Data }).Coroutine();
        }

        private static void OnEventClickItemAction(this RedDotDataItemComponent self)
        {
            self.Fiber().UIEvent(new OnClickItemEvent { Data = self.m_Data }).Coroutine();
        }

        private static void OnEventChildAction(this RedDotDataItemComponent self)
        {
            self.Fiber().UIEvent(new OnClickChildListEvent { Data = self.m_Data }).Coroutine();
        }
        #endregion YIUIEvent结束
    }
}