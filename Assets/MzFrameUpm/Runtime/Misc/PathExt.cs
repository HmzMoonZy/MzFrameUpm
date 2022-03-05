using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class PathExt
{
    public  static  string ProjectPath=> System.Environment.CurrentDirectory;
    
    public static string AssetPath  => Application.dataPath; 

    //public static string MzFramePath  => Path.Combine(AssetPath, "MzFrame");
}
