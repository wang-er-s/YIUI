﻿using System;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    [FriendOf(typeof(LobbyPanelComponent))]
    public static partial class LobbyPanelComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this LobbyPanelComponent self)
        {
            self.BtnEnterMap.onClick.AddListener(() => self.EnterMap().Coroutine());
        }

        [EntitySystem]
        private static void Destroy(this LobbyPanelComponent self)
        {
        }

        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this LobbyPanelComponent self)
        {
            await ETTask.CompletedTask;
            return true;
        }
        
        public static async ETTask EnterMap(this LobbyPanelComponent self)
        {
            Scene root = self.Root();
            await EnterMapHelper.EnterMapAsync(root);
            self.UIPanel.Close();
        }

        #region YIUIEvent开始
        
        private static async ETTask OnEventEnterAction(this LobbyPanelComponent self)
        {
            var banId = YIUIMgrComponent.Inst.BanLayerOptionForever();
            await EnterMapHelper.EnterMapAsync(self.Root());
            YIUIMgrComponent.Inst.RecoverLayerOptionForever(banId);
            self.UIPanel.Close(false,true);
        }
        
        #endregion YIUIEvent结束
    }
}