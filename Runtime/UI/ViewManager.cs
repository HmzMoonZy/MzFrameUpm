using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MzFrame
{
    public static class ViewManager
    {
        private static Dictionary<string, ViewInfo> _CacheOfAllInfo = new Dictionary<string, ViewInfo>();

        /// <summary>
        /// 这个节点的子物体均会被隐藏.
        /// </summary>
        private static Transform _HiddenTransform;


        public static void Init()
        {
            _HiddenTransform = UnityEngine.Object.FindObjectsOfType<Canvas>().Where(canvas => canvas.enabled == false)
                .ToList()[0].transform;
            
        }
        
        /// <summary>
        /// 打开一个和 T 同名的 View.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static T OpenView<T>(object[] args) where T : ViewInfo
        {
            var typeName = typeof(T).Name;
            
            if (_CacheOfAllInfo.TryGetValue(typeName, out var info))
            {
                if (info.IsVisible)
                {
                    info.OnOpened(args);
                }
                else
                {
                    // TODO 唤醒逻辑
                    
                }
                return (T)info;
            }
            
            var viewObject = _FindAndInstantiateViewObject(typeName);
            info = new ViewInfo(viewObject, typeName);
            info.OnCreated();
            info._InitializationViewInfo();
            //info.OnOpening();
            info.OnOpened(args);

            return (T)info;
        }

        public static void CloseView<T>(object[] args = null)
        {
            var typeName = typeof(T).Name;
            if (!_CacheOfAllInfo.TryGetValue(typeName, out var info))
            {
                Debug.Log($"Try to close a view with name {typeName} that does not exist ");
                return;
            }

            if (info != null && !info.IsVisible)
            {
                Debug.Log($"Try to close an already hidden view with name {typeName}");
                return;
            }
            
            // TODO 正常关闭
            //info.OnClosing();

            if (info.ViewConfig.EnableAutoMask)
            {
                UnityEngine.Object.Destroy(info.ViewObject.transform.Find("__AutoMask").gameObject);
            }

            if (info.ViewConfig.IsCache)
            {
                // TODO 解除响应.
                info.ViewObject.transform.SetParent(_HiddenTransform);
            }
            else
            {
                _CacheOfAllInfo.Remove(typeName);
                UnityEngine.Object.Destroy(info.ViewObject);
                info = null;
            }
            
        }


        #region Private Method

        private static void _InitHierarchical()
        {
            
        }
        
        private static GameObject _FindAndInstantiateViewObject(string viewName)
        {
            //Debug.Log($"ViewManager 01: 准备实例化 ViewGo {viewName}");
            // TODO 用 Addressble 实现
            var viewGo = UnityEngine.Object.Instantiate(Resources.Load<GameObject>(viewName));
            return viewGo;
        }
        
        #endregion

        #region ViewInfo Extension Methods
        private static void _InitializationViewInfo(this ViewInfo info)
        {
            info.ViewCanvas.sortingLayerID = (int) info.ViewConfig.Layer;
            // TODO 分配Sort

            if (info.ViewConfig.EnableAutoMask)
            {
                var mask = info._GenerateMask();
                if (info.ViewConfig.ClickMaskTriggerClose)
                {
                    // TODO 添加快速点击组件
                }
            }

            
            if (info.ViewConfig.EnableAutoAnimation)
            {
                // TODO 使用 DoTween 和结束回调.
            }
        }
        
        private static GameObject _GenerateMask(this ViewInfo info)
        {
            var mask = new GameObject("__AutoMask", typeof(Image));
            mask.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);

            var rectTrans = mask.GetComponent<RectTransform>();
            rectTrans.sizeDelta = new Vector2(5000, 5000);

            rectTrans.SetParent(info.ViewObject.transform);
            rectTrans.localPosition = Vector2.zero;
            rectTrans.SetAsFirstSibling();
            return mask;
        }

        #endregion


    }
}