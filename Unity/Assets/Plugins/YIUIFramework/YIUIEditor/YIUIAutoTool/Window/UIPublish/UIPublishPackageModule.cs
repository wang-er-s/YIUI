#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

namespace YIUIFramework.Editor
{
    public enum EUIPublishPackageData
    {
        [LabelText("组件")]
        CDETable,

        [LabelText("精灵")]
        Sprites,

        [LabelText("图集")]
        Atlas,
    }

    public class UIPublishPackageModule
    {
        private UIPublishModule m_UIPublishModule;

        [LabelText("模块名")]
        [ReadOnly]
        public string PkgName;

        [FolderPath]
        [LabelText("模块路径")]
        [ReadOnly]
        public string PkgPath;

        [EnumToggleButtons]
        [HideLabel]
        public EUIPublishPackageData m_UIPublishPackageData = EUIPublishPackageData.CDETable;

        [LabelText("当前模块所有组件")]
        [ReadOnly]
        [ShowInInspector]
        [ShowIf("m_UIPublishPackageData", EUIPublishPackageData.CDETable)]
        private List<UIBindCDETable> m_AllCDETable = new List<UIBindCDETable>();

        [LabelText("当前模块所有精灵")]
        [ReadOnly]
        [ShowInInspector]
        [ShowIf("m_UIPublishPackageData", EUIPublishPackageData.Sprites)]
        private List<TextureImporter> m_AllTextureImporter = new List<TextureImporter>();

        //根据精灵文件夹创建对应的图集数量
        [LabelText("所有图集名称")]
        [ReadOnly]
        [HideInInspector]
        [ShowIf("m_UIPublishPackageData", EUIPublishPackageData.Atlas)]
        public HashSet<string> m_AtlasName = new HashSet<string>();

        [GUIColor(0.4f, 0.8f, 1)]
        [Button("发布当前模块", 50)]
        [PropertyOrder(-999)]
        private void PublishCurrent()
        {
            PublishCurrent(true);
        }

        public void PublishCurrent(bool showTips)
        {
            if (!UIOperationHelper.CheckUIOperation()) return;

            foreach (var current in m_AllCDETable)
            {
                current.CreateUICode(false, false);
            }

            if (showTips)
                UnityTipsHelper.CallBackOk($"YIUI当前模块 {PkgName} 发布完毕", YIUIAutoTool.CloseWindowRefresh);
        }

        [Button("创建or重置 文件结构", 30)]
        [PropertyOrder(-998)]
        public void ResetDirectory()
        {
            if (!UIOperationHelper.CheckUIOperation()) return;

            UICreateResModule.Create(PkgName);
        }

        #region 初始化

        public UIPublishPackageModule(UIPublishModule publishModule, string pkgName)
        {
            m_UIPublishModule = publishModule;
            PkgName           = pkgName;
            PkgPath           = $"{UIStaticHelper.UIProjectResPath}/{pkgName}";
            FindUIBindCDETableResources();
            FindUITextureResources();
        }

        private void FindUIBindCDETableResources()
        {
            var strings = AssetDatabase.GetAllAssetPaths().Where(x =>
                x.StartsWith($"{PkgPath}/{UIStaticHelper.UIPrefabs}", StringComparison.InvariantCultureIgnoreCase));

            foreach (var path in strings)
            {
                var cdeTable = AssetDatabase.LoadAssetAtPath<UIBindCDETable>(path);
                if (cdeTable == null) continue;
                if (!cdeTable.IsSplitData)
                {
                    m_AllCDETable.Add(cdeTable);
                }
            }
        }

        private void FindUITextureResources()
        {
            var strings = AssetDatabase.GetAllAssetPaths().Where(x =>
                x.StartsWith($"{PkgPath}/{UIStaticHelper.UISprites}", StringComparison.InvariantCultureIgnoreCase));

            m_AtlasName.Clear();

            foreach (var path in strings)
            {
                if (AssetImporter.GetAtPath(path) is TextureImporter texture)
                {
                    var atlasName = GetSpritesAtlasName(path);
                    if (string.IsNullOrEmpty(atlasName))
                    {
                        Logger.LogError(texture,
                            $"此文件位置错误 {path}  必须在 {UIStaticHelper.UISprites}/XX 图集文件下 不可以直接在根目录");
                        continue;
                    }

                    if (!m_AtlasName.Contains(atlasName))
                        m_AtlasName.Add(atlasName);

                    m_AllTextureImporter.Add(texture);
                }
            }
        }

        private string GetSpritesAtlasName(string path, string currentName = "")
        {
            if (!path.Replace("\\", "/").Contains($"{PkgPath}/{UIStaticHelper.UISprites}"))
            {
                return null;
            }

            var parentInfo = System.IO.Directory.GetParent(path);
            if (parentInfo == null)
            {
                return currentName;
            }

            if (parentInfo.Name == UIStaticHelper.UISprites)
            {
                return currentName;
            }

            return GetSpritesAtlasName(parentInfo.FullName, parentInfo.Name);
        }

        #endregion

    }
}
#endif