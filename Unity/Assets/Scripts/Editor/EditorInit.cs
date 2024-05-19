using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ET
{
    public class EditorInit
    {
        [InitializeOnLoadMethod]
        public static void Init()
        {
            World.Instance.AddSingleton<TimeInfo>();
            World.Instance.AddSingleton<IdGenerater>();
        }
    }
}