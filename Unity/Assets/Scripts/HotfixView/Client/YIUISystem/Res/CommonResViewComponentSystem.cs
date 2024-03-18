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
    [FriendOf(typeof(CommonResViewComponent))]
    public static partial class CommonResViewComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this CommonResViewComponent self)
        {
        }
        
        [EntitySystem]
        private static void Destroy(this CommonResViewComponent self)
        {
        }
        
        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this CommonResViewComponent self)
        {
            await ETTask.CompletedTask;
            return true;
        }
        
    }
}