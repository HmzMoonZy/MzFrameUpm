using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MzFrameUpm;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MzFrame
{
    public static class ViewManager
    {
        /// <summary>
        /// 所有 View 的根节点.
        /// </summary>
        public static Transform ViewRoot{ get; private set; }
            
        /// <summary>
        /// 这个节点的子物体均会被隐藏.
        /// </summary>
        public static Transform HiddenTransform { get; private set; }
        
        public static Camera ViewCamera { get; private set; }
        
        public static Action<ViewInfo> OnViewOpen;

        public static Action<ViewInfo> OnViewClose;

        private static string _AddressableGroup;
        
        /// <summary>
        /// 所有 View 的 Update 方法会在这里注册, 请在合适的地方调用.
        /// </summary>
        private static Action<float> _ViewUpdate;

        // 所有存活的面板
        private static Dictionary<string, ViewInfo> _CacheOfView = new Dictionary<string, ViewInfo>();
        // 所有展示中的面板
        private static Dictionary<string, ViewInfo> _CacheOfVisibleView = new Dictionary<string, ViewInfo>();
        // 所有层级的排序
        private static Dictionary<int, int> _CacheOfOderForLayer = new Dictionary<int, int>();

        public static void Init(/*string addressableGroup*/)
        {
            ViewRoot = UnityEngine.Object.FindObjectOfType<EventSystem>().transform;
            
            HiddenTransform = 
                UnityEngine.Object.FindObjectsOfType<Canvas>().
                    Where(canvas => canvas.enabled == false).ToList()[0].transform;
            
            ViewCamera = ViewRoot.Find("ViewCamera").GetComponent<Camera>();
            
            foreach (int v in Enum.GetValues(typeof(Constant.ViewSort)))
            {
                _CacheOfOderForLayer.Add(v, 0);
            }

            OnViewOpen += info =>
            {
                info.SortOrder = _CacheOfOderForLayer[(int) info.Layer];
                _CacheOfOderForLayer[(int) info.Layer] += 1;
            };
            
            OnViewClose += info =>
            {
                _CacheOfOderForLayer[(int) info.Layer] -= 1;
                if (info.SortOrder != _CacheOfOderForLayer[(int) info.Layer])
                {
                    _CacheOfVisibleView.Values
                        .Where(viewInfo => viewInfo.Layer == info.Layer)
                        .OrderBy(viewInfo => viewInfo.SortOrder).ToList()
                        .ForEach((viewInfo, i) => viewInfo.SortOrder = i);
                }
            };
        }
        
        /// <summary>
        /// 打开一个和 T 同名的 View.
        /// </summary>
        public static T OpenView<T>(object[] args = null) where T : ViewInfo
        {
            var typeName = typeof(T).Name;
            if (!_CacheOfView.TryGetValue(typeName, out var info))  // 第一次创建 View
            {
                var assetName = typeName.Replace("Info", "");
                var viewObject = _FindAndInstantiateViewObject(assetName);
                info = (T) Activator.CreateInstance(typeof(T), viewObject, typeName);
                _CacheOfView.Add(typeName, info);
                _AutoBindViewEvent(info.GetType(), info, info.ViewObject.transform);
                info.OnCreated();
                //info.ViewObject.transform.SetParent(HiddenTransform);
            }
            
            // 唤醒 View
            info._AutoGenerateMask();
            
            if (!info.IsVisible)
            {
                info.ViewConfig.transform.SetParent(ViewRoot);
            }
            _CacheOfVisibleView.Add(typeName, info);
            
            if (info.ViewConfig.EnableAutoAnimation)
            {
                // TODO 使用 DoTween 和结束回调.
            }
            
            info.OnOpened(args);

            _ViewUpdate += info.Update;
            
            OnViewOpen?.Invoke(info);
            
            return (T)info;

        }

        public static void CloseView<T>(object[] args = null)
        {
            var typeName = typeof(T).Name;
            if (!_CacheOfView.TryGetValue(typeName, out var info))
            {
                Debug.Log($"Try to close a view with name {typeName} that does not exist ");
                return;
            }

            if (info is {IsVisible: false})     // info != null && !info.IsVisible
            {
                Debug.Log($"Try to close an already hidden view with name {typeName}");
                return;
            }

            //info.OnClosing();
            _ViewUpdate -= info.Update;
            _CacheOfVisibleView.Remove(typeName);
            OnViewClose(info);
            if (info.ViewConfig.EnableAutoMask)
            {
                UnityEngine.Object.Destroy(info.ViewObject.transform.GetChild(0).gameObject);
            }

            if (info.ViewConfig.IsCache)
            {
                info.ViewObject.transform.SetParent(HiddenTransform);
            }
            else
            {
                _CacheOfView.Remove(typeName);
                UnityEngine.Object.Destroy(info.ViewObject);
                info = null;
            }
        }

        public static void Update()
        {
            _ViewUpdate?.Invoke(Time.deltaTime);
        }

        public static Dictionary<string, Transform> Instantiate(Transform t)
        {
            // TODO 实例化一部分组件然后自动绑定
            return null;
        }


        #region Private Method

        private static GameObject _FindAndInstantiateViewObject(string viewName)
        {
            var request = new ResourcesRequest<GameObject>()
            {
                assetName = viewName,
                labels = null,
                onLoad = null,
            };

            return ResourcesManager.Instantiate(request);
        }
        
        private static void _AutoBindViewEvent(Type infoType, ViewInfo info, Transform t)
        {
            foreach (Transform child in t)
            {
                // TODO 目前只支持 Button, 之后适配 Toggle 等组件.
                var btn = child.GetComponent<Button>();
                if (btn != null)
                {
                    var realName = btn.name.StartsWith("#") || btn.name.StartsWith("$") ? btn.name.Substring(1) : btn.name;
                    var methodInfo = infoType.GetMethod($"__OnClickBtn_{realName}", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (methodInfo != null)
                    {
                        Debug.Log($"Auto bind click event : __OnClickBtn_{realName}");
                        btn.onClick.AddListener(() => { methodInfo.Invoke(info, null); });
                    }
                        
                }

                if (child.childCount > 0)
                {
                    _AutoBindViewEvent(infoType, info, child);
                }
            }
        }

        
        #endregion

        #region ViewInfo Extension Methods
        
        public static void Freeze(this ViewInfo info)
        {
            info.ViewObject.GetComponent<GraphicRaycaster>().enabled = false;
        }
        
        public static void UnFreeze(this ViewInfo info)
        {
            info.ViewObject.GetComponent<GraphicRaycaster>().enabled = true;
        }

        private static void _AutoGenerateMask(this ViewInfo info)
        {
            if (!info.ViewConfig.EnableAutoMask) return;
            
            var mask = new GameObject("__AutoMask", typeof(Image));
            mask.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
            
            var rectTrans = mask.GetComponent<RectTransform>();
            rectTrans.sizeDelta = new Vector2(5000, 5000);
            rectTrans.SetParent(info.ViewObject.transform, true);
            rectTrans.localPosition = Vector2.zero;
            rectTrans.localScale = Vector3.one;
            rectTrans.SetAsFirstSibling();
            
            if (!info.ViewConfig.ClickMaskTriggerClose) return;
            // TODO 添加快速点击组件

        }

        #endregion
    }
}