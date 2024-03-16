using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{

    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// </summary>
    [YIUI(EUICodeType.View)]
    public partial class CommonResViewComponent: Entity, IDestroy, IAwake, IYIUIBind, IYIUIInitialize, IYIUIOpen
    {
        public const string PkgName = "Res";
        public const string ResName = "CommonResView";

        private EntityRef<YIUIComponent> u_UIBase;
        public YIUIComponent UIBase {get => u_UIBase; set => u_UIBase = value;}
        private EntityRef<YIUIWindowComponent> u_UIWindow;
        public YIUIWindowComponent UIWindow { get => u_UIWindow; set => u_UIWindow = value;}
        private EntityRef<YIUIViewComponent> u_UIView;
        public YIUIViewComponent UIView { get => u_UIView; set => u_UIView = value;}

    }
}