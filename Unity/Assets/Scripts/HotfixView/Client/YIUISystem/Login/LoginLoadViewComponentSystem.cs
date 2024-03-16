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
    [FriendOf(typeof(LoginLoadViewComponent))]
    public static partial class LoginLoadViewComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this LoginLoadViewComponent self)
        {
        }
        
        [EntitySystem]
        private static void Destroy(this LoginLoadViewComponent self)
        {
        }
        
        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this LoginLoadViewComponent self)
        {
            await ETTask.CompletedTask;
            return true;
        }
        
        #region YIUIEvent开始
        #endregion YIUIEvent结束
    }
}