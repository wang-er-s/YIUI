using System;
using YIUIFramework;
using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof (MessageTipsViewComponent))]
    public static partial class MessageTipsViewComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this MessageTipsViewComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this MessageTipsViewComponent self)
        {
        }

        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this MessageTipsViewComponent self)
        {
            await ETTask.CompletedTask;
            return true;
        }

        [EntitySystem]
        private static async ETTask YIUIOpenTween(this MessageTipsViewComponent self)
        {
            await WindowFadeAnim.In(self.UIBase);
        }

        [EntitySystem]
        private static async ETTask YIUICloseTween(this MessageTipsViewComponent self)
        {
            await WindowFadeAnim.Out(self.UIBase);
        }

        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this MessageTipsViewComponent self, ParamVo vo)
        {
            await ETTask.CompletedTask;
            var content = vo.Get<string>();
            if (string.IsNullOrEmpty(content))
            {
                Debug.LogError($"MessageTipsView 必须有消息内容 请检查");
                return false;
            }

            self.ExtraData = vo.Get(1, new MessageTipsExtraData());
            return true;
        }

        #region YIUIEvent开始

        private static void OnEventConfirmAction(this MessageTipsViewComponent self)
        {
            self.ExtraData.ConfirmCallBack?.Invoke();
            TipsHelper.CloseTipsViewSync(self);
        }

        private static void OnEventCancelAction(this MessageTipsViewComponent self)
        {
            self.ExtraData.CancelCallBack?.Invoke();
            TipsHelper.CloseTipsView(self).Coroutine();
        }

        private static void OnEventCloseAction(this MessageTipsViewComponent self)
        {
            self.ExtraData.CloseCallBack?.Invoke();
            TipsHelper.CloseTipsView(self).Coroutine();
        }

        #endregion YIUIEvent结束
    }
}