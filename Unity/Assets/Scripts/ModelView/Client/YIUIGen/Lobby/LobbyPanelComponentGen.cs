using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{

    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// </summary>
    [YIUI(EUICodeType.Panel, EPanelLayer.Panel)]
    public partial class LobbyPanelComponent: Entity, IDestroy, IAwake, IYIUIBind, IYIUIInitialize, IYIUIOpen
    {
        public const string PkgName = "Lobby";
        public const string ResName = "LobbyPanel";

        private EntityRef<YIUIComponent> u_UIBase;
        public YIUIComponent UIBase { get => u_UIBase; set => u_UIBase = value;}
        private EntityRef<YIUIWindowComponent> u_UIWindow;
        public YIUIWindowComponent UIWindow { get => u_UIWindow; set => u_UIWindow = value;}
        private EntityRef<YIUIPanelComponent> u_UIPanel;
        public YIUIPanelComponent UIPanel { get => u_UIPanel; set => u_UIPanel = value;}
        public UnityEngine.UI.Button BtnEnterMap;
        private EntityRef<ET.Client.CurrencyTitleCommonComponent> u_UICurrencyTitleView;
        public ET.Client.CurrencyTitleCommonComponent UICurrencyTitleView {get => u_UICurrencyTitleView; set => u_UICurrencyTitleView = value;} 

    }
}