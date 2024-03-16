#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YIUIFramework;
using Logger = YIUIFramework.Logger;

namespace YIUIFramework
{
    
    //Editor
    public sealed partial class UIBindComponentTable
    {

        public static Dictionary<Type, string> UITypeDefaultPrefix = new Dictionary<Type, string>()
        {
            {typeof(Transform),"Trans"},
            {typeof(RectTransform),"Trans"},
            {typeof(Text),"Txt"},
            {typeof(Button),"Btn"},
            {typeof(Image),"Img"},
            {typeof(TextMeshProUGUI), "Txt"},
            {typeof(Toggle), "Tog"},
            {typeof(InputField), "Input"},
        };
        
        [OdinSerialize]
        [LabelText("所有绑定数据 编辑数据")]
        [Searchable]
        [HideReferenceObjectPicker]
        [PropertyOrder(-10)]
        [ShowIf("@UIOperationHelper.CommonShowIf()")]
        [ListDrawerSettings(CustomAddFunction = nameof(CustomAddBindPair))]
        [OnStateUpdate("@OnPairListChanged()")]
        private List<UIBindPairData> m_AllBindPair = new List<UIBindPairData>();

        private void CustomAddBindPair()
        {
            this.m_AllBindPair.Add(new UIBindPairData());
        }

        private void OnPairListChanged()
        {
            if (this.m_AllBindDic == null) this.m_AllBindDic = new Dictionary<string, Component>();
            this.m_AllBindDic.Clear();
            foreach (var pair in this.m_AllBindPair)
            {
                if(pair.Component == null) continue;
                if (this.m_AllBindDic.ContainsKey(pair.Name))
                {
                    Debug.LogError($"存在重复字段{pair.Name}");
                    break;
                }
                this.m_AllBindDic.Add(pair.Name, pair.Component);
            }
        }
    }
    
    /// <summary>
    /// 绑定数据对应关系
    /// </summary>
    [Serializable]
    [HideLabel]
    [HideReferenceObjectPicker]
    internal class UIBindPairData
    {
        [LabelText("名称")]
        public string Name ;

        [LabelText("对象")]
        [OnValueChanged(nameof(AutoFillName))]
        public Component Component;

        private void AutoFillName()
        {
            if (!string.IsNullOrEmpty(this.Name))
            {
                return;
            }

            this.Name = this.Component.gameObject.name;
            var type = this.Component.GetType();
            foreach (var kv in UIBindComponentTable.UITypeDefaultPrefix)
            {
                if (kv.Key.IsAssignableFrom(type))
                {
                    if (!this.Name.StartsWith(kv.Value))
                    {
                        this.Name = kv.Value + this.Name;
                        break;
                    }
                }
            }
        }
    }
}
#endif