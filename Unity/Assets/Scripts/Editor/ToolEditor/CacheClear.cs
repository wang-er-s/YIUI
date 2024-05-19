using System.IO;
using UnityEditor;
using UnityEngine;

namespace ET
{
    public static class CacheClear
    {
        [MenuItem("Tools/打开缓存目录")]
        public static void OpenCache()
        {
            EditorUtility.OpenWithDefaultApp(Application.persistentDataPath);
        }

        [MenuItem("Tools/清除缓存")]
        public static void ClearCache()
        {
            PlayerPrefs.DeleteAll();
            if (Directory.Exists(Application.persistentDataPath))
            {
                Directory.Delete(Application.persistentDataPath, true);
            }
        }
    }
}