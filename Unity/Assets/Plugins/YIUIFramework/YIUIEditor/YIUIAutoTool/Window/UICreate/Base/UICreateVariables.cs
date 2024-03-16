#if UNITY_EDITOR
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace YIUIFramework.Editor
{
    /// <summary>
    /// 变量的生成
    /// </summary>
    public static class UICreateVariables
    {
        public static string Get(UIBindCDETable cdeTable)
        {
            var sb = SbPool.Get();
            cdeTable.GetOverrideConfig(sb);
            cdeTable.GetComponentTable(sb);
            cdeTable.GetCDETable(sb);
            return SbPool.PutAndToStr(sb);
        }

        private static void GetComponentTable(this UIBindCDETable self, StringBuilder sb)
        {
            var tab = self.ComponentTable;
            if (tab == null) return;

            foreach (var value in tab.AllBindDic)
            {
                var name = value.Key;
                if (string.IsNullOrEmpty(name)) continue;
                var bindCom = value.Value;
                if (bindCom == null) continue;
                sb.AppendFormat("        public {0} {1};\r\n", bindCom.GetType(), name);
            }
        }

        private static void GetCDETable(this UIBindCDETable self, StringBuilder sb)
        {
            var tab = self.AllChildCdeTable;
            if (tab == null) return;
            var existName = new HashSet<string>();

            foreach (var value in tab)
            {
                var name = value.name;
                if (string.IsNullOrEmpty(name)) continue;
                var pkgName = value.PkgName;
                var resName = value.ResName;
                if (string.IsNullOrEmpty(pkgName) || string.IsNullOrEmpty(resName)) continue;
                var newName = GetCDEUIName(name);
                if (existName.Contains(newName))
                {
                    Debug.LogError($"{self.name} 内部公共组件存在同名 请修改 {name} 当前会被忽略 {newName}");
                    continue;
                }

                existName.Add(newName);
                sb.AppendFormat("        private {0} {1};\r\n",
                    $"EntityRef<{UIStaticHelper.UINamespace}.{resName}Component>", newName);
                sb.Append(
                    $"        public {UIStaticHelper.UINamespace}.{resName}Component {newName.Replace(NameUtility.FirstName, "")} {{get => {newName}; set => {newName} = value;}} \r\n");
            }
        }

        internal static string GetCDEUIName(string oldName)
        {
            var newName = oldName;

            if (!oldName.CheckFirstName(NameUtility.UIName))
            {
                newName = $"{NameUtility.FirstName}{NameUtility.UIName}{oldName}";
            }

            newName = Regex.Replace(newName, NameUtility.NameRegex, "");

            return newName.ChangeToBigName(NameUtility.UIName);
        }

        private static void GetOverrideConfig(this UIBindCDETable self, StringBuilder sb)
        {
            switch (self.UICodeType)
            {
                case EUICodeType.Common:
                    sb.AppendFormat("        private EntityRef<YIUIComponent> u_UIBase;\r\n");
                    sb.Append("        public YIUIComponent UIBase { get => u_UIBase; set => u_UIBase = value;}\r\n");
                    return;
                case EUICodeType.Panel:
                    sb.AppendFormat("        private EntityRef<YIUIComponent> u_UIBase;\r\n");
                    sb.Append("        public YIUIComponent UIBase { get => u_UIBase; set => u_UIBase = value;}\r\n");

                    sb.AppendFormat("        private EntityRef<YIUIWindowComponent> u_UIWindow;\r\n");
                    sb.Append("        public YIUIWindowComponent UIWindow { get => u_UIWindow; set => u_UIWindow = value;}\r\n");

                    sb.AppendFormat("        private EntityRef<YIUIPanelComponent> u_UIPanel;\r\n");
                    sb.Append("        public YIUIPanelComponent UIPanel { get => u_UIPanel; set => u_UIPanel = value;}\r\n");
                    break;
                case EUICodeType.View:
                    sb.AppendFormat("        private EntityRef<YIUIComponent> u_UIBase;\r\n");
                    sb.Append("        public YIUIComponent UIBase {get => u_UIBase; set => u_UIBase = value;}\r\n");

                    sb.AppendFormat("        private EntityRef<YIUIWindowComponent> u_UIWindow;\r\n");
                    sb.Append("        public YIUIWindowComponent UIWindow { get => u_UIWindow; set => u_UIWindow = value;}\r\n");

                    sb.AppendFormat("        private EntityRef<YIUIViewComponent> u_UIView;\r\n");
                    sb.Append("        public YIUIViewComponent UIView { get => u_UIView; set => u_UIView = value;}\r\n");
                    break;
                default:
                    Debug.LogError($"新增类型未实现 {self.UICodeType}");
                    break;
            }
        }
    }
}
#endif