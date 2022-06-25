using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MzFrame
{
    [InfoBox("请确保这个 ViewPrefab 的名称唯一且没有空格和特殊符号❗")]
    public class ViewConfig : MonoBehaviour
    {
        [Title("层级")] [EnumPaging] public ViewSort Layer;

        [Title("自动遮罩")] public bool EnableAutoMask = true;
        [ShowIf("EnableAutoMask")] public bool ClickMaskTriggerClose = false;

        [Title("动画")] public bool EnableAutoAnimation = false;

        [Title("是否缓存")] public bool IsCache = true;

#if UNITY_EDITOR

        #region 代码生成

        [Button(ButtonSizes.Large)]
        private void 刷新视图()
        {
            var viewerPath = Path.Combine(PathExt.AssetPath, "Temp", "MzFrameScripts");
            var viewerFilePath =
                $"{Path.Combine(viewerPath, "__" + transform.name + "Info")}__.cs"; //  __ViewTestInfo__.cs
            if (!Directory.Exists(viewerPath))
            {
                Directory.CreateDirectory(viewerPath);
            }

            // 创建文件.
            new FileStream(viewerFilePath, FileMode.Create).Close();

            // 识别路径.
            var cache = new Dictionary<string, Transform>();
            __TraverseLegalRootNode(transform, ref cache);
            var roots = cache.Values.ToArray();
            foreach (var root in roots)
            {
                Debug.Log($"开始查询{root.name}");
                __TraverseLegalNode(root, ref cache);
            }

            var result = new Dictionary<string, string>();
            foreach (var kv in cache)
            {
                result.Add(kv.Key, __FindFullPath(kv.Value));
            }

            // 写入.
            var className = Path.GetFileNameWithoutExtension(viewerFilePath).Replace("_", "");
            using var sw = new StreamWriter(viewerFilePath);
            ViewCodeGenerator.GenerateViewCode(sw, className, result);
        }

        private void __TraverseLegalRootNode(Transform t, ref Dictionary<string, Transform> result)
        {
            foreach (Transform child in t)
            {
                if (child.name.StartsWith("#"))
                {
                    string name = child.name.Substring(1);
                    if (result.ContainsKey(name))
                    {
                        Debug.LogError("有同名的根路径!");
                        break;
                    }

                    Debug.Log($"Find Root :{name}");
                    result.Add(name, child);
                }

                if (child.childCount > 0)
                {
                    __TraverseLegalRootNode(child, ref result);
                }
            }
        }

        // 遍历节点，查找所有合法的需要自动生成的节点并返回对应的名称和路径字典
        private void __TraverseLegalNode(Transform root, ref Dictionary<string, Transform> result)
        {
            foreach (Transform leaf in root)
            {
                bool isContinue = leaf.name.StartsWith("#");

                if (leaf.name.StartsWith("$"))
                {
                    var parent = leaf.parent;
                    if (parent.name.StartsWith("#") || parent.name.StartsWith("$"))
                    {
                        var _path = __FindSimplePath(leaf);
                        Debug.Log($"Find Leaf :{_path}");
                        result.Add(
                            _path.Substring(1).Replace("/", "_").Replace("$", "").Replace("#", ""),
                            leaf);
                    }
                    else
                    {
                        Debug.LogWarning($"发现一个中断的带符号节点{leaf.name}");
                    }
                }

                if (leaf.childCount > 0 && !isContinue)
                {
                    __TraverseLegalNode(leaf, ref result);
                }
            }
        }

        // 计算并缓存变量名和路径
        private string __FindSimplePath(Transform t)
        {
            var parent = t.parent;
            var path = t.name;

            if (t.name.StartsWith("#"))
            {
                while (parent != transform)
                {
                    path = $"{parent.name}/{path}";
                    parent = parent.parent;
                }

                return path;
            }

            while (!parent.name.StartsWith("#"))
            {
                path = $"{parent.name}/{path}";
                parent = parent.parent;
            }

            path = $"{parent.name}/{path}";
            return path;
        }

        private string __FindFullPath(Transform t)
        {
            var parent = t.parent;
            var path = t.name;
            while (parent != transform)
            {
                path = $"{parent.name}/{path}";
                parent = parent.parent;
            }

            return path;
        }

        #endregion

#endif
    }
}