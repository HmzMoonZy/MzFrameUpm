#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;


namespace MzFrame
{
    public static class ViewCodeGenerator
    {

        private const string HEAD = 
            @"// 
using System.Collections.Generic;
using System;
using MzFrame;
using UnityEngine;
using UnityEngine.UI;
";
        
        public static void WriteHead(StreamWriter sw)
        {
            sw.Write(HEAD);
        }

        public static void GenerateViewCode(StreamWriter sw, string className, Dictionary<string, string> paramPath)
        {
            WriteHead(sw);
            sw.Write($"public  partial class {className} : ViewInfo");
            sw.Write("{");
            foreach (var element in paramPath)
            {
                sw.WriteLine($"public ViewCompFinder {element.Key};");
            }
            // 构造方法
            sw.Write($"public {className}(GameObject viewObject, string name) : base(viewObject, name)");
            sw.Write("{");
            foreach (var element in paramPath)
            {
                sw.WriteLine($"{element.Key} = new ViewCompFinder(viewObject.transform.Find(\"{element.Value}\"));");
            }
                
            sw.Write("}");
            sw.Write("}");
        }

        public static void GenerateViewLogic(StreamWriter sw, string className)
        {
            WriteHead(sw);
            sw.Write($"public  partial class {className}");
            sw.Write("{");
            
            sw.Write("        public override void OnOpened(params object[] args)");
            sw.Write("        {");
            
            sw.Write("        }");
            sw.Write("}");
        }

        
    }
}

#endif