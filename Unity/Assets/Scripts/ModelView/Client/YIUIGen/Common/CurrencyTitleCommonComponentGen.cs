using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{

    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// </summary>
    [YIUI(EUICodeType.Common)]
    public partial class CurrencyTitleCommonComponent: Entity, IDestroy, IAwake, IYIUIBind, IYIUIInitialize
    {
        public const string PkgName = "Common";
        public const string ResName = "CurrencyTitleCommon";

        private EntityRef<YIUIComponent> u_UIBase;
        public YIUIComponent UIBase { get => u_UIBase; set => u_UIBase = value;}
        public UnityEngine.UI.Button BtnClose;

    }
}