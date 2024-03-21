using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// 当前Panel所有可用view枚举
    /// </summary>
    public enum ELoginPanelViewEnum
    {
        LoginExistView = 1,
        LoginLoadView = 2,
        PopExistView = 3,
        PopLoadView = 4,
    }
    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// </summary>
    [YIUI(EUICodeType.Panel, EPanelLayer.Panel)]
    public partial class LoginPanelComponent: Entity, IDestroy, IAwake, IYIUIBind, IYIUIInitialize, IYIUIOpen
    {
        public const string PkgName = "Login";
        public const string ResName = "LoginPanel";

        private EntityRef<YIUIComponent> u_UIBase;
        public YIUIComponent UIBase { get => u_UIBase; set => u_UIBase = value;}
        private EntityRef<YIUIWindowComponent> u_UIWindow;
        public YIUIWindowComponent UIWindow { get => u_UIWindow; set => u_UIWindow = value;}
        private EntityRef<YIUIPanelComponent> u_UIPanel;
        public YIUIPanelComponent UIPanel { get => u_UIPanel; set => u_UIPanel = value;}
        public UnityEngine.UI.Button BtnLogin;
        public TMPro.TMP_InputField InputAccount;
        public TMPro.TMP_InputField ImpntPassword;
        private EntityRef<ET.Client.LoginExistViewComponent> u_UILoginExistView;
        public ET.Client.LoginExistViewComponent UILoginExistView {get => u_UILoginExistView; set => u_UILoginExistView = value;} 

    }
}