#if ENABLE_VIEW && UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    public class ComponentView: MonoBehaviour
    {
        [ShowInInspector]
        public Entity Component
        {
            get;
            set;
        }
    }
}
#endif