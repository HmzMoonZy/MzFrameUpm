using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MzFrame
{
    [InfoBox("请确保这个 ViewPrefab 的名称唯一且没有空格和特殊符号❗")]
    public class ViewConfig : MonoBehaviour
    {
        [Title("层级")]
        [EnumPaging]
        public Constant.ViewSort Layer;

        [Title("自动遮罩")]
        public bool EnableAutoMask = true;
        [ShowIf("EnableAutoMask")]
        public bool ClickMaskTriggerClose = false;

        [Title("动画")]
        public bool EnableAutoAnimation = false;

        [Title("是否缓存")]
        public bool IsCache = true;

#if UNITY_EDITOR
        #region 代码生成
        
        [Button(ButtonSizes.Large)]
        private void 刷新视图()
        {
            var viewerPath = Path.Combine(PathExt.AssetPath, "Temp", "MzFrameScripts");
            var viewerFilePath = $"{Path.Combine(viewerPath, "__" + transform.name + "Info")}__.cs"; //  __ViewTestInfo__.cs
            if (!Directory.Exists(viewerPath))
            {
                Directory.CreateDirectory(viewerPath);
            }

            // 创建文件.
            new FileStream(viewerFilePath, FileMode.Create).Close();
            
            // 识别路径.
            var cache = new Dictionary<string, string>();
            __TraverseLegalNode(transform, transform, ref cache);
            
            // 写入.
            var className = Path.GetFileNameWithoutExtension(viewerFilePath).Replace("_", "");
            using var sw = new StreamWriter(viewerFilePath);
            ViewCodeGenerator.GenerateViewCode(sw, className, cache);
        }

        // 遍历节点，查找所有合法的需要自动生成的节点并返回对应的名称和路径字典
        private void __TraverseLegalNode(
            Transform t, 
            Transform target, 
            ref Dictionary<string, string> result)
        {
            foreach (Transform child in t)
            {
                if (child.name.StartsWith("#"))
                {
                    result.Add(child.name.Substring(1), __FindPath(child, target));
                }

                if (child.name.StartsWith("$"))
                {
                    if (child.parent.name.StartsWith("#") || child.parent.name.StartsWith("$"))
                    {
                        var _path = __FindPath(child, target);
                        result.Add(_path.Substring(_path.IndexOf("#") + 1).Replace("/", "_").Replace("$", "").Replace("#", ""), _path);
                    }
                }

                if (child.childCount > 0)
                {
                    __TraverseLegalNode(child, target, ref result);
                }
            }


            // 计算并缓存变量名和路径
            static string __FindPath(Transform t, Transform stop)
            {
                bool isRoot = t.name.StartsWith("#");
                var parent = t.parent;
                var path = t.name;

                while (parent != stop)
                {
                    path = $"{parent.name}/{path}";
                    parent = parent.parent;
                }
                return path;
            }
        }

        #endregion
#endif

    }
}