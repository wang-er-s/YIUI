using UnityEngine;

namespace ET
{
    public static class UnityExtension
    {
        public static void DestroyAllChildren(this Transform root)
        {
            for (int i = root.childCount - 1; i >= 0; i--)
            {
                UnityEngine.Object.Destroy(root.GetChild(i).gameObject);
            }
        }
        
        public static void DestroyImmediateAllChildren(this Transform root)
        {
            for (int i = root.childCount - 1; i >= 0; i--)
            {
                UnityEngine.Object.DestroyImmediate(root.GetChild(i).gameObject);
            }
        }
    }
}