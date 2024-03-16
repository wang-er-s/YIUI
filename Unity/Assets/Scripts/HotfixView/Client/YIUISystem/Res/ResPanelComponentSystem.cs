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
    [FriendOf(typeof(ResPanelComponent))]
    public static partial class ResPanelComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this ResPanelComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this ResPanelComponent self)
        {
        }
        
        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this ResPanelComponent self)
        {
            await ETTask.CompletedTask;
            return true;
        }
        
    }
}