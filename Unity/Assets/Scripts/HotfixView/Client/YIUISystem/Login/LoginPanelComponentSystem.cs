using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// Author  YIUI
    /// Date    2024.3.16
    /// Desc
    /// </summary>
    [FriendOf(typeof(LoginPanelComponent))]
    public static partial class LoginPanelComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this LoginPanelComponent self)
        {
            Log.Info($"Init");
            self.UIPanel.OpenViewAsync<CommonResViewComponent>().Coroutine();
            self.BtnLogin.onClick.AddListener(() =>
            {
                YIUIMgrComponent.Inst.OpenPanelAsync<MainPanelComponent>().Coroutine();
            });
        }

        [EntitySystem]
        private static void Destroy(this LoginPanelComponent self)
        {
        }
        
        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this LoginPanelComponent self)
        {
            Log.Info($"Open");
            // var banId = YIUIMgrComponent.Inst.BanLayerOptionForever();
            // await LoginHelper.Login(self.Root(), self.Account, self.Password);
            // YIUIMgrComponent.Inst.RecoverLayerOptionForever(banId);
            return true;
        }
        
    }
}