﻿using System;
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
    [FriendOf(typeof(PopLoadViewComponent))]
    public static partial class PopLoadViewComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this PopLoadViewComponent self)
        {
        }
        
        [EntitySystem]
        private static void Destroy(this PopLoadViewComponent self)
        {
        }
        
        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this PopLoadViewComponent self)
        {
            await ETTask.CompletedTask;
            return true;
        }
        
        #region YIUIEvent开始
        #endregion YIUIEvent结束
    }
}