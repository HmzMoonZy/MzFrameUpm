using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class Initializer : Editor
{
    [MenuItem("MzFrame/在场景中初始化")]
    private static void InitMzFrame()
    {
        
    }
    
    [MenuItem("MzFrame/UI TOOL/初始化 ViewSort")]
    private static void InitSortingLayer()
    {
        var type = typeof(MzFrame.Constant.ViewSort);

        // 先遍历枚举拿到枚举的字符串
        List<string> lstSceenPriority = new List<string>();
        foreach (int v in Enum.GetValues(type))
        {
            lstSceenPriority.Add(Enum.GetName(type, v));
        }

        // 清除数据
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty it = tagManager.GetIterator();
        while (it.NextVisible(true))
        {
            if (it.name != "m_SortingLayers")
            {
                continue;
            }
            // 先删除所有
            while (it.arraySize > 0)
            {
                it.DeleteArrayElementAtIndex(0);
            }

            // 重新插入
            // 将枚举字符串生成到 sortingLayer
            foreach (var s in lstSceenPriority)
            {
                it.InsertArrayElementAtIndex(it.arraySize);
                SerializedProperty dataPoint = it.GetArrayElementAtIndex(it.arraySize - 1);

                while (dataPoint.NextVisible(true))
                {
                    if (dataPoint.name == "name")
                    {
                        dataPoint.stringValue = s;
                    }
                    else if (dataPoint.name == "uniqueID")
                    {
                        dataPoint.intValue = (int)Enum.Parse(type, s);
                    }
                }
            }
        }
        tagManager.ApplyModifiedProperties();
        AssetDatabase.SaveAssets();
    }
}