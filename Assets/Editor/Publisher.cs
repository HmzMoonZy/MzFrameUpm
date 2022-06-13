using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

/// <summary>
/// UPM 包发布器, 如果是大版本发布请手动提升版本号.
/// </summary>
public class Publisher : Editor
{
    private static string workDir = Application.dataPath.Substring(0, Application.dataPath.Length - 7);

    [MenuItem("Publisher/发布到 GitHub (提升版本号)")]
    private static void PublishWithVersionUpgrade()
    {
        //Debug.Log($"准备发布新版本的 MzFrame, 确保CHANGELOG等文档编写完毕.");
    }
    
    [MenuItem("Publisher/发布到 GitHub (不提升版本号)")]
    private static void PublishWithoutVersionUpgrade()
    {
        var startInfo = new ProcessStartInfo
        {
            UseShellExecute = false,
            RedirectStandardOutput = true,
            CreateNoWindow = true,
            FileName = "CMD.exe",
            Arguments = $"/c cd {workDir} & git subtree split --prefix=Assets/MzFrameUpm --branch upm & git push origin upm",
        };
    
        var process = Process.Start(startInfo);
        string output = process.StandardOutput.ReadToEnd();
        Console.WriteLine(output);
        process.WaitForExit();
    }
    

}
