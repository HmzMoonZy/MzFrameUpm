using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Publisher : Editor
{

    [MenuItem("Publisher/发布到 GitHub")]
    private static void Publish()
    {
        //Debug.Log($"准备发布新版本的 MzFrame, 确保CHANGELOG等文档编写完毕.");
    }
    
}
