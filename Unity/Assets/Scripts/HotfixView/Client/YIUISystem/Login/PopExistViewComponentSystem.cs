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
    [FriendOf(typeof(PopExistViewComponent))]
    public static partial class PopExistViewComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this PopExistViewComponent self)
        {
        }
        
        [EntitySystem]
        private static void Destroy(this PopExistViewComponent self)
        {
        }
        
        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this PopExistViewComponent self)
        {
            await ETTask.CompletedTask;
            return true;
        }
        
        #region YIUIEvent开始
        #endregion YIUIEvent结束
    }
}