using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ET
{
    public class Test : MonoBehaviour
    {
        [Button]
        private void Start()
        {
            var recordFilePath = Path.Combine(Application.dataPath, "..", "Record",
                $"{System.DateTime.Now.ToString("MM-dd-HH:mm:ss")}-level{1}.txt");
            print(recordFilePath);
            Directory.CreateDirectory(Path.GetDirectoryName(recordFilePath));
        }

    }
}
