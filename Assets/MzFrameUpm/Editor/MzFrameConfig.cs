using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace MzFrame.Editor
{
    public class MzFrameConfig : ScriptableObject
    {
     
        [Title("View代码生成路径","View的部分代码可以直接生成,在这里配置它的生成路径.")]
        [FolderPath(ParentFolder = "Assets/")]
        //[LabelText("view 自动生成代码的路径")]
        public string autoViewPath = "Assets/";


        
        private static MzFrameConfig _config;
        public static MzFrameConfig Config
        {
            get
            {
                if (_config != null) return _config;
                
                if (Application.identifier.Contains("MzFrameUpm"))
                {
                    Debug.Log("开发中");
                    _config = AssetDatabase.LoadAssetAtPath<MzFrameConfig>(Path.Combine("Assets","MzFrameUpm","Config","MainConfig.asset"));
                }
                else
                {
                    _config = AssetDatabase.LoadAssetAtPath<MzFrameConfig>(PathExt.ProjectPath);
                }

                return _config;
            }
        }
    }

}

