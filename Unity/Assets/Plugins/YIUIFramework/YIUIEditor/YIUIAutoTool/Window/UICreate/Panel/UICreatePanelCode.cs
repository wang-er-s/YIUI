﻿#if UNITY_EDITOR

namespace YIUIFramework.Editor
{
    public class UICreatePanelCode : BaseTemplate
    {
        private         string m_EventName = "UI继承Panel代码创建";
        public override string EventName => m_EventName;

        public override bool Cover => false;

        private         bool m_AutoRefresh = false;
        public override bool AutoRefresh => m_AutoRefresh;

        private         bool m_ShowTips = false;
        public override bool ShowTips => m_ShowTips;

        public UICreatePanelCode(out bool result, string authorName, UICreatePanelData codeData) : base(authorName)
        {
            var path     = $"{UIStaticHelper.UIETSystemGenPath}/{codeData.PkgName}/{codeData.ResName}.cs";
            var template = $"{UIStaticHelper.UITemplatePath}/UICreatePanelTemplate.txt";
            CreateVo = new CreateVo(template, path);

            m_EventName           = $"{codeData.ResName} 继承 {codeData.ResName}Base 创建";
            m_AutoRefresh         = codeData.AutoRefresh;
            m_ShowTips            = codeData.ShowTips;
            ValueDic["Namespace"] = codeData.Namespace;
            ValueDic["PkgName"]   = codeData.PkgName;
            ValueDic["ResName"]   = codeData.ResName;

            if (!TemplateEngine.FileExists(CreateVo.SavePath))
            {
                result = CreateNewFile();
            }

            if (codeData.CoverDic != null)
            {
                OverrideCheckCodeFile(codeData.CoverDic,true);
            }
            
            if (codeData.OverrideDic == null)
            {
                result = true;
                return;
            }

            result = OverrideCheckCodeFile(codeData.OverrideDic);
        }
    }
}
#endif